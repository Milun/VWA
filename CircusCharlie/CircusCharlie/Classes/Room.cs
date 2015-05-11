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
    class Room
    {
        private const int mapWidth = 29;
        private const int mapHeight = 22;

        private Dictionary<IntVector2D, Tile> tiles;
        private List<Col> cols;
        private Dictionary<IntVector2D, Actor> actors;

        private string description = "";

        public Room()
        {
            tiles = new Dictionary<IntVector2D, Tile>();
            actors = new Dictionary<IntVector2D, Actor>();

            cols = new List<Col>();

            
        }

        public Vector2 GetRoomSize()
        {
            return new Vector2(mapWidth * Global.gridSize, mapHeight * Global.gridSize);
        }

        public Tile GetTile(int x, int y)
        {
            return tiles[new IntVector2D(x, y)];
        }

        public void SetActor(IntVector2D v, Actor actor)
        {
            if (actor != null)
            {
                actors[v] = actor;
            }
            else
            {
                actors.Remove(v);
            }
        }

        public void SetTile(IntVector2D v, Tile tile)
        {
            if (tile != null)
            {
                tiles[v] = tile;
            }
            else
            {
                tiles.Remove(v);
            }
        }

        public void MsgCol(Vector2 pos, Vector2 size, string msg)
        {
            ColSquare a = new ColSquare(pos, Vector2.Zero, Vector2.One * size);

            a.DrawDebug(Editor.colorDebug3);

            foreach (KeyValuePair<IntVector2D, Actor> e in actors)
            {
                if (e.Value.CheckCol(a) != Vector2.Zero)
                {
                    e.Value.PassMsg(msg);
                }
            }
        }

        public bool CheckCol(Vector2 pos)
        {
            return CheckCol(pos, Vector2.One*0.2f);
        }

        public bool CheckCol(Vector2 pos, Vector2 size)
        {
            ColSquare a = new ColSquare(pos, Vector2.One * -size/2f, Vector2.One * size);

            a.DrawDebug(Editor.colorDebug3);

            foreach (Col e in cols)
            {
                if (e.CheckCol(a) != Vector2.Zero) return true;
            }


            foreach (KeyValuePair<IntVector2D, Actor> e in actors)
            {
                if (e.Value.CheckCol(a) != Vector2.Zero) return true;
            }

            return false;
        }

        public Vector2 CheckCol(Actor other)
        {
            // Lag caused by large amount of sprites.

            Vector2 output = Vector2.Zero;

            foreach (Col e in cols)
            {
                output -= other.CheckCol(e);
            }

            
            foreach (KeyValuePair<IntVector2D, Actor> e in actors)
            {
                Vector2 col = e.Value.CheckCol(other);

                if (col != Vector2.Zero)
                {
                    output -= col;
                }
            }

            return output;
        }

        public bool IsLevelComplete()
        {
            foreach (KeyValuePair<IntVector2D, Actor> e in actors)
            {
                if (e.Value.IsAlive()) return false;
            }

            return true;
        }

        public void InitRoom()
        {
            // Maybe have a parent class for ALL ACTORS which has a common draw and 
            // check collision function which returns a vector.
            // This way the ACTORS can check the type of thing they're colliding with (you pass the other actor to them).


            //// TO DO LIST!!!
            // Make it sort the collisions from top left to bottom right.
            // Do an int for down and right. First go DOWN (if possible) then add 1 to down, then go right and check.
            // Remove as you go. At the end just add a collision for the amount of down and right you have x 24.


            List<IntVector2D> temp = new List<IntVector2D>();
            foreach (KeyValuePair<IntVector2D, Tile> e in tiles)
            {
                temp.Add(e.Key);

                e.Value.SetNeighbours(tiles.ContainsKey(new IntVector2D(e.Key.X, e.Key.Y - 1)),
                                      tiles.ContainsKey(new IntVector2D(e.Key.X + 1, e.Key.Y)),
                                      tiles.ContainsKey(new IntVector2D(e.Key.X, e.Key.Y + 1)),
                                      tiles.ContainsKey(new IntVector2D(e.Key.X - 1, e.Key.Y)));
            }

            while (temp.Count > 0)
            {
                Vector2 pos = new Vector2(temp[0].X, temp[0].Y);
                Vector2 size = new Vector2(1f, 1f);

                bool horizontal = true;
                bool vertical = true;

                IntVector2D start = temp[0] + new IntVector2D(1,0);
                temp.RemoveAt(0);

                while (temp.Contains(start))
                {
                    size += new Vector2(1f, 0f);
                    temp.Remove(start);
                    start += new IntVector2D(1, 0);
                }

                cols.Add(new ColSquare(pos,
                                       Vector2.Zero,
                                       size));
            }

            //foreach (IntVector2D e in temp)
            {


                /*cols.Add(new ColSquare(new Vector2(e.Key.X * 24, e.Key.Y * 24),
                                       Vector2.Zero,
                                       new Vector2(24, 24)));*/
            }
        }

        //private void 

        public void StopRoom()
        {
            cols.Clear();
            foreach (KeyValuePair<IntVector2D, Actor> e in actors)
            {
                e.Value.Reset();
            }
        }

        public void SaveRoom(string filename)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
            {
                // Write the amount of tiles in the whole room
                writer.Write(tiles.Count);

                // Then write each tile in one by one
                foreach (KeyValuePair<IntVector2D, Tile> e in tiles)
                {
                    writer.Write(e.Key.X);
                    writer.Write(e.Key.Y);
                    writer.Write(e.Value.GetOff().X);
                    writer.Write(e.Value.GetOff().Y);
                }

                // Need to add info for all the actors in the scene
                writer.Write(actors.Count);

                foreach (KeyValuePair<IntVector2D, Actor> e in actors)
                {
                    // Normal breakable block
                    if (e.Value.GetType() == typeof(Block))
                    {
                        writer.Write((int)(0));
                        writer.Write(e.Key.X);
                        writer.Write(e.Key.Y);
                        continue;
                    }

                    // Statue
                    if (e.Value.GetType() == typeof(EnemyStatue))
                    {
                        writer.Write((int)(1));
                        writer.Write(e.Key.X);
                        writer.Write(e.Key.Y);

                        // Enemies have different pos's to where they're stored.
                        writer.Write(e.Value.GetPos().X);
                        writer.Write(e.Value.GetPos().Y);

                        // Write whether the statue is upside down.
                        writer.Write((e.Value.GetValue("flipY") == -1f));
                        continue;
                    }

                    // Head
                    if (e.Value.GetType() == typeof(EnemyHead))
                    {
                        writer.Write((int)(2));
                        writer.Write(e.Key.X);
                        writer.Write(e.Key.Y);

                        // Enemies have different pos's to where they're stored.
                        writer.Write(e.Value.GetPos().X);
                        writer.Write(e.Value.GetPos().Y);

                        // Write whether the head is upside down.
                        writer.Write((e.Value.GetValue("flipY") == -1f));
                        continue;
                    }

                    
                }
            }
        }
        public void LoadRoom(string filename, ref Sprite spr, ref Sprite _block)
        {
            if (File.Exists(filename))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
                {
                    tiles.Clear();

                    // Read how many tiles are in this room
                    int count = reader.ReadInt32();

                    // Read the tiles into the room
                    for (int i = 0; i < count; i++)
                    {
                        int posX = reader.ReadInt32();
                        int posY = reader.ReadInt32();
                        float offX = reader.ReadSingle();
                        float offY = reader.ReadSingle();
                        //int rotation = reader.ReadInt32();
                        //int color = reader.ReadInt32();

                        Tile tile = new Tile(spr,
                                             new Vector2(offX, offY),
                                             new Vector2(posX, posY));

                        tiles.Add(new IntVector2D(posX, posY), tile);
                    }

                    count = reader.ReadInt32();

                    for (int i = 0; i < count; i++)
                    {
                        // Need to read the type first.
                        int type = reader.ReadInt32();

                        // All actors have a position.
                        int posX = reader.ReadInt32();
                        int posY = reader.ReadInt32();

                        // Make block
                        if (type == 0)
                        {
                            Actor block = new Block(new Vector2(posX, posY), _block);
                            actors.Add(new IntVector2D(posX, posY), block);
                            continue;
                        }

                        // Pretty much everything else reads the real position.
                        float X = reader.ReadSingle();
                        float Y = reader.ReadSingle();

                        bool readFlipY = false;
                        float writeFlipY = 1f;

                        switch (type)
                        {
                            // Statue
                            case 1:
                                readFlipY = reader.ReadBoolean();
                                if (readFlipY) writeFlipY = -1f;

                                Actor statue = new EnemyStatue(new Vector2(X, Y), Editor.sprStatue, -1f, writeFlipY);
                                actors.Add(new IntVector2D(posX, posY), statue);
                                break;

                            // Head
                            case 2:
                                readFlipY = reader.ReadBoolean();
                                if (readFlipY) writeFlipY = -1f;

                                Actor head = new EnemyHead(new Vector2(X, Y), Editor.sprHead, writeFlipY);
                                actors.Add(new IntVector2D(posX, posY), head);
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
        }

        public void Draw(IntVector2D _pos)
        {

            foreach (KeyValuePair<IntVector2D, Tile> e in tiles)
            {
                e.Value.Draw(_pos);
            }

            foreach (Col e in cols)
            {
                e.DrawDebug(Editor.colorDebug2);
            }

            foreach (KeyValuePair<IntVector2D, Actor> e in actors)
            {
                e.Value.DrawEditor();
            }
        }

        public void Draw3D(IntVector2D _pos)
        {

            foreach (KeyValuePair<IntVector2D, Tile> e in tiles)
            {
                e.Value.Draw3D();
            }

            foreach (Col e in cols)
            {
                e.DrawDebug(Editor.colorDebug);
            }

            foreach (KeyValuePair<IntVector2D, Actor> e in actors)
            {
                e.Value.Draw();
            }
        }

    }
}
