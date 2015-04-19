using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace CircusCharlie.Classes
{
    class Editor
    {
        Sprite sprMap;
        Texture2D texLine;
        Texture2D texBlack;
        Texture2D texWhite;
        Texture2D texCursor;
        Texture2D texCross;
        Texture2D texSelect;
        Texture2D texBall;
        Texture2D texTools;

        public static SpriteFont font;

        Sprite sprBlock;

        public static Sprite sprCircle;
        public static Sprite sprDebug; // For use by other classes for debug.
        public static Color colorDebug;

        private bool DPressed = false;
        private bool GPressed = false;
        private bool SPressed = false;
        private bool PPressed = false;
        private bool gameRunning = false;

        private MainGame mainGame;

        public static bool showDebug = false;
        private bool showGrid = false;

        private Room[,] rooms;
        private const int ROOMSMAP = 21;

        private const int TILESMENU_X = 784;
        private const int TILESMENU_Y = 51;

        private const int TILESMAP_X = 24;
        private const int TILESMAP_Y = 48;

        private const int TOOLS_Y = 544;

        private const int COLORSMENU_X = 744;
        private const int COLORSMENU_Y = 456;

        private int mapWidth = 29;
        private int mapHeight = 22;

        IntVector2D currentRoom;
        IntVector2D currentTileSelect;
        IntVector2D currentCOLORSelect;
        IntVector2D currentActorSelect;
        Tile currentTile = null;

        IntVector2D roomPos;
        IntVector2D roomPosLast;
        IntVector2D roomPosLastMouse;

        public static Color[] COLORS;

        private Tile[,] tilesMap;
        private Tile[,] tilesMenu;
        SpriteBatch spriteBatch;

        // Constructor
        public Editor(SpriteBatch _spriteBatch, ContentManager Content)
        {
            roomPos = new IntVector2D();
            roomPosLast = new IntVector2D();
            roomPosLastMouse = new IntVector2D();

            font = Content.Load<SpriteFont>("Arial");


            spriteBatch = _spriteBatch;

            mainGame = new MainGame(_spriteBatch, Content);

            sprMap = new Sprite(Content.Load<Texture2D>("Sprites/spr_wall"),
                                ref spriteBatch,
                                8, 8);

            sprBlock = new Sprite(Content.Load<Texture2D>("Sprites/spr_special"),
                                ref spriteBatch);

            texBall = Content.Load<Texture2D>("Sprites/spr_ball");
            texLine = Content.Load<Texture2D>("Sprites/spr_line");
            texBlack = Content.Load<Texture2D>("Sprites/spr_black");
            texWhite = Content.Load<Texture2D>("Sprites/spr_white");
            texCursor = Content.Load<Texture2D>("Sprites/spr_cursor");
            texSelect = Content.Load<Texture2D>("Sprites/spr_select");
            texCross = Content.Load<Texture2D>("Sprites/spr_cross");
            texTools = Content.Load<Texture2D>("Sprites/spr_tools");

            Color[] data = new Color[texWhite.Width * texWhite.Height];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Color.White;
            }
            texWhite.SetData<Color>(data);

            colorDebug = new Color(255, 0, 0, 80);
            sprDebug = new Sprite(texWhite,
                                  ref spriteBatch);
            sprCircle = new Sprite(Content.Load<Texture2D>("Sprites/spr_tools"),
                                   ref spriteBatch);

            currentRoom = new IntVector2D(11, 11);
            rooms = new Room[ROOMSMAP, ROOMSMAP];
            for (int i = 0; i < ROOMSMAP; i++)
            {
                for (int j = 0; j < ROOMSMAP; j++)
                {
                    rooms[i,j] = new Room();
                }
            }

            tilesMenu = new Tile[8, 16];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    tilesMenu[i, j] = new Tile(sprMap,
                                               new IntVector2D(i, j),
                                               new IntVector2D(i + mapWidth + 2,
                                                               j + 1));
                }
            }

            tilesMap = new Tile[mapWidth, mapHeight];
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    /*tilesMap[i,j] = new Tile(sprMap,
                                             Vector2.Zero,
                                             new Vector2(i * tileScale*(tileSize) + TILESMAP_X,
                                                         j * tileScale*(tileSize) + TILESMAP_Y));*/
                    tilesMap[i, j] = null;
                }
            }

            currentTile = tilesMenu[0, 0];
            

            COLORS = new Color[16];
            COLORS[0] =     new Color(255, 255, 255);
            COLORS[1] =     new Color(170, 116, 73);
            COLORS[2] =     new Color(120, 41, 34);
            COLORS[3] =     new Color(135, 214, 221);
            COLORS[4] =     new Color(170, 95, 182);
            COLORS[5] =     new Color(85, 160, 73);
            COLORS[6] =     new Color(64, 49, 141);
            COLORS[7] =     new Color(191, 206, 114);
            COLORS[8] =     new Color(40,40,40);
            COLORS[9] =     new Color(234, 180, 137);
            COLORS[10] =    new Color(184, 105, 98);
            COLORS[11] =    new Color(199, 255, 255);
            COLORS[12] =    new Color(234, 159, 246);
            COLORS[13] =    new Color(148, 224, 137);
            COLORS[14] =    new Color(128, 113, 204);
            COLORS[15] =    new Color(255, 255, 178);

            currentCOLORSelect = new IntVector2D(0,0);
            currentTileSelect = new IntVector2D(0, 0);
            RecolorMenu();

            LoadMap();

            rooms[currentRoom.X, currentRoom.Y].SetActor(new IntVector2D(5,5), new Enemy(new Vector2(96,118), new Sprite(Content.Load<Texture2D>("Sprites/spr_pyramid"),
                                   ref spriteBatch, 48, 48)));
            
            currentActorSelect = new IntVector2D(0, 0);
        }

        private void SaveCurrentMap()
        {
            // Check input
            if (!Keyboard.GetState().IsKeyDown(Keys.S) || !Keyboard.GetState().IsKeyDown(Keys.LeftControl))
            {
                SPressed = false;
                return;

            }
            else if (SPressed)
            {
                return;
            }

            SPressed = true;

            string filename = "room-" + (currentRoom.X).ToString() + "-" + (currentRoom.Y).ToString() + ".room";

            rooms[currentRoom.X, currentRoom.Y].SaveRoom(filename);
        }

        private void LoadMap()
        {
            string filename = "room-" + (currentRoom.X).ToString() + "-" + (currentRoom.Y).ToString() + ".room";

            rooms[currentRoom.X, currentRoom.Y].LoadRoom(filename, ref sprMap, ref sprBlock);
        }

        private void ToggleDebug()
        {
            if (!Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                DPressed = false;
                return;

            }
            else if (DPressed)
            {
                return;
            }

            DPressed = true;

            showDebug = !showDebug;
        }

        private void ToggleGrid()
        {
            if (!Keyboard.GetState().IsKeyDown(Keys.G))
            {
               GPressed = false;
               return;
                
            }
            else if (GPressed)
            {
                return;
            }

            GPressed = true;

            showGrid = !showGrid;
        }

        private void DrawSelect()
        {
            spriteBatch.Draw(texSelect, new Rectangle(currentTile.getPos().X * Global.gridSize,
                                                      currentTile.getPos().Y * Global.gridSize,
                                                      Global.gridSize,
                                                      Global.gridSize), Color.LimeGreen);

            spriteBatch.Draw(texSelect, new Rectangle(COLORSMENU_X + currentCOLORSelect.X * Global.gridSize,
                                                      COLORSMENU_Y + currentCOLORSelect.Y * Global.gridSize,
                                                      Global.gridSize,
                                                      Global.gridSize), Color.LimeGreen);

            spriteBatch.Draw(texSelect, new Rectangle(currentActorSelect.X * Global.gridSize,
                                                      TOOLS_Y + currentActorSelect.Y * Global.gridSize,
                                                      Global.gridSize,
                                                      Global.gridSize), Color.LimeGreen);
        }

        private void DrawColorMenu()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    spriteBatch.Draw(texWhite, new Rectangle(COLORSMENU_X + i * Global.gridSize,
                                                             COLORSMENU_Y + j * Global.gridSize,
                                                             Global.gridSize,
                                                             Global.gridSize), COLORS[i + j * 8]);
                }
            }
        }

        private void DrawTileMap()
        {
            // Draw tiles
            rooms[currentRoom.X, currentRoom.Y].Draw(new IntVector2D());

            // Draw grid
            if (showGrid)
            {

                for (int i = -1; i < 64; i++)
                {
                    Rectangle rec = new Rectangle(i * Global.gridSize - (int)Global.viewCenter.X,
                                                  0,
                                                  1,
                                                  Global.gridSize * mapHeight);

                    spriteBatch.Draw(texLine, rec, Color.White);
                }
                for (int i = -1; i < 64; i++)
                {
                    Rectangle rec = new Rectangle(0,
                                                  i * Global.gridSize - (int)Global.viewCenter.Y,
                                                  Global.gridSize * mapWidth,
                                                  1);

                    spriteBatch.Draw(texLine, rec, Color.White);
                }
            }
            // Draw overlap.
            sprDebug.Draw(new IntVector2D(696, 0), new IntVector2D(310, 800), Color.DarkGray);
            sprDebug.Draw(new IntVector2D(0, 528), new IntVector2D(800, 310), Color.DarkGray);
        }

        private void SelectTileMap()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt)) return;

            if (Mouse.GetState().LeftButton != ButtonState.Pressed &&
                Mouse.GetState().RightButton != ButtonState.Pressed) return;

            if (Mouse.GetState().X >= 696 || Mouse.GetState().Y >= 528) return;

            IntVector2D mouse = GetMouseMenuPos();
            if (mouse == new IntVector2D(-1, -1)) return;

            // Delete tile
            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                rooms[currentRoom.X, currentRoom.Y].SetTile(mouse, null);
                rooms[currentRoom.X, currentRoom.Y].SetActor(mouse, null);
                return;
            }
            // Create block
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && Keyboard.GetState().IsKeyDown(Keys.LeftControl))
            {
                Block block = new Block(new Vector2(mouse.X, mouse.Y), sprBlock);
                rooms[currentRoom.X, currentRoom.Y].SetActor(mouse, block);
                return;
            }
            // Create tile
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Tile other = new Tile(currentTile);
                other.setPos(mouse);

                rooms[currentRoom.X, currentRoom.Y].SetTile(mouse, other);
                return;
            }
        }

        private IntVector2D GetMouseMenuPos()
        {
            int x = Mouse.GetState().X+(int)Global.viewCenter.X;
            int y = Mouse.GetState().Y+(int)Global.viewCenter.Y;

            x = x / Global.gridSize;
            y = y / Global.gridSize;

            /*if (x < 0 || x >= mapWidth ||
                y < 0 || y >= mapHeight) return new IntVector2D(-1, -1);
            */
            return new IntVector2D(x, y);
        }

        private void RecolorMenu()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    tilesMenu[i, j].SetColor(currentCOLORSelect.X + 8 * (currentCOLORSelect.Y));
                }
            }
        }

        private void SelectColorMenu()
        {
            if (Mouse.GetState().LeftButton != ButtonState.Pressed) return;

            int x = Mouse.GetState().X - COLORSMENU_X;
            int y = Mouse.GetState().Y - COLORSMENU_Y;

            x = x / Global.gridSize;
            y = y / Global.gridSize;

            if (x < 0 || x >= 8 || y < 0 || y >= 2) return;

            currentCOLORSelect = new IntVector2D(x, y);
            RecolorMenu();
        }

        private void SelectTileMenu()
        {
            if (Mouse.GetState().LeftButton != ButtonState.Pressed) return;

            int x = Mouse.GetState().X - (mapWidth + 2) * Global.gridSize;
            int y = Mouse.GetState().Y - Global.gridSize;

            x = x / Global.gridSize;
            y = y / Global.gridSize;

            if (x < 0 || x > 7 || y < 0 || y > 15) return;

            currentTileSelect = new IntVector2D(x, y);
            currentTile = tilesMenu[x, y];
        }

        private void DrawTileMenu()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    tilesMenu[i,j].DrawEditor();
                }
            }
        }

        private void DrawCurrentActor()
        {

        }

        private void DrawCurrentTile()
        {
            /*spriteBatch.Draw(texSelect, new Rectangle(TILESMENU_X - 51 - tileScale * tileSize,
                                                      TILESMENU_Y + 9 - tileScale * tileSize,
                                                      tileSize * tileScale * 2 + 12,
                                                      tileSize * tileScale * 2 + 12), Color.Black);
            currentTile.Draw(new IntVector2D(TILESMENU_X - 45, TILESMENU_Y + 15), (float)tileScale * 2);*/
        }

        public void DrawShadow(bool isActive)
        {
            mainGame.DrawBackdrop();
            
        }

        public void LaunchGame()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                PPressed = true;
                return;

            }
            else if (!PPressed)
            {
                return;
            }

            PPressed = false;

            if (!gameRunning)
            {
                gameRunning = true;
                mainGame.InitGame(ref rooms[currentRoom.X, currentRoom.Y], new IntVector2D((int)(Mouse.GetState().X + Global.viewCenter.X), (int)(Mouse.GetState().Y + Global.viewCenter.Y)));
            }
        }

        public void MoveRoom()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                // Set initial location the mouse was.
                if (roomPosLastMouse.X == 0 && roomPosLastMouse.Y == 0)
                {
                    roomPosLastMouse = new IntVector2D(Mouse.GetState().X, Mouse.GetState().Y);
                }

                // Move room
                roomPos = roomPosLast + roomPosLastMouse - new IntVector2D(Mouse.GetState().X, Mouse.GetState().Y);

                Global.viewCenter = new Vector2(roomPos.X, roomPos.Y);
            }
            else
            {
                roomPosLastMouse.X = 0;
                roomPosLastMouse.Y = 0;
                roomPosLast = roomPos;
            }
        }

        public void Draw(bool isActive)
        {
            if (!gameRunning)
            {
                LaunchGame();
                DrawTileMap();

                //rooms[currentRoom.X, currentRoom.Y].DrawShadow(roomPos);
                //rooms[currentRoom.X, currentRoom.Y].Draw(roomPos);

                DrawTileMenu();
                DrawColorMenu();

                DrawCurrentTile();
                

                spriteBatch.Draw(texTools, new Rectangle(0, TOOLS_Y, 384, 96), new Rectangle(0, 0, 128, 32), Color.White);

                DrawSelect();

                if (isActive)
                {
                        SelectTileMenu();
                        SelectTileMap();
                        SelectColorMenu();

                        SaveCurrentMap();
                        ToggleGrid();

                        MoveRoom();
                }
            }
            else
            {
                mainGame.Draw();
                ToggleDebug();
                //sprDebug.Draw(new IntVector2D(696, 0), new IntVector2D(400, 700), Color.Black);
                //sprDebug.Draw(new IntVector2D(0, 528), new IntVector2D(696, 200), Color.Black);

                if (Keyboard.GetState().IsKeyDown(Keys.Escape) || rooms[currentRoom.X, currentRoom.Y].IsLevelComplete())
                {
                    roomPos = new IntVector2D((int)Global.viewCenter.X, (int)Global.viewCenter.Y);
                    mainGame.StopGame();
                    gameRunning = false;
                }
            }

            

            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                spriteBatch.Draw(texBall, new Rectangle(Mouse.GetState().X-12,
                                                        Mouse.GetState().Y-12,
                                                        24,
                                                        24), Color.White);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt))
            {
                spriteBatch.Draw(texCursor, new Rectangle(Mouse.GetState().X,
                                                          Mouse.GetState().Y,
                                                          12,
                                                          19),
                                            new Rectangle(12,
                                                          0,
                                                          12,
                                                          19), Color.White);
            }
            else
            { 
                spriteBatch.Draw(texCursor, new Rectangle(Mouse.GetState().X,
                                                          Mouse.GetState().Y,
                                                          12,
                                                          19),
                                            new Rectangle(0,
                                                          0,
                                                          12,
                                                          19), Color.White);
            }

            sprDebug.DrawView(Global.viewCenter, Color.White);

            spriteBatch.DrawString(font,
                                   "X: " + (Global.viewCenter.X + Global.viewSize.X) + " Y: " + (Global.viewCenter.Y + Global.viewSize.Y),
                                   new Vector2(20, 20),
                                   Color.White);

            Tile temp = rooms[currentRoom.X, currentRoom.Y].GetTile(0,0);

            spriteBatch.DrawString(font,
                                   "X: " + temp.getPos().X + " Y: " + temp.getPos().Y,
                                   new Vector2(20, 40),
                                   Color.White);
        }
    }
}
