using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace CircusCharlie.Classes
{
    class MainGame
    {
        Texture2D texBG;
        Texture2D texMonitor;
        Sprite sprBall;

        SpriteBatch spriteBatch;
        Ball ball;
        public static Room room;

        float temp = 0f;
        bool bounce = false;

        // Constructor
        public MainGame(SpriteBatch _spriteBatch, ContentManager Content)
        {
            spriteBatch = _spriteBatch;
            sprBall = new Sprite(Content.Load<Texture2D>("Sprites/spr_ball"), ref spriteBatch);
            ball = new Ball(sprBall);

            texBG = Content.Load<Texture2D>("Sprites/spr_bg");
            texMonitor = Content.Load<Texture2D>("Sprites/spr_monitor");

            Global.viewSize = new Vector2(Game1.SCREENWIDTH / 4f, Game1.SCREENHEIGHT / 4f);
        }

        public void InitGame(ref Room _room, IntVector2D startPos)
        {
            room = _room;
            room.InitRoom();
            ball.SetPos(startPos);
            Global.viewZoom = 2f;
        }

        public void StopGame()
        {
            if (room == null) return;
            room.StopRoom();
            room = null;
            Global.viewZoom = 1f;
            //Global.viewCenter = Vector2.Zero;
            Editor.showDebug = false;
        }

        public void DrawBackdrop()
        {
            // Draw black backdrop
            spriteBatch.Draw(texBG, new Rectangle((int)(ball.GetPos().X/-2f)-50,
                                                  (int)(ball.GetPos().Y/-2f)-50,
                                                  1392,
                                                  1056), Color.White);

            if (bounce)
            {
                temp += 0.003f;
                if (temp >= 0.8f) bounce = false;
            }
            else
            {
                temp -= 0.003f;
                if (temp <= 0.0f) bounce = true;
            }
            sprBall.Draw3D(Vector2.One * 100f, new Color(1f-temp, 1f-temp, 1f-temp), temp);

            /*
            spriteBatch.Draw(texMonitor, new Rectangle((int)(-Global.viewCenter.X * Global.viewZoom) + 330,
                                                       (int)(-Global.viewCenter.Y * Global.viewZoom) - 190,
                                                       740,
                                                       510),
                                         new Rectangle(170,
                                                       0,
                                                       170,
                                                       170), Color.White);

            spriteBatch.Draw(texMonitor, new Rectangle((int)(-Global.viewCenter.X * Global.viewZoom) + 330,
                                                       (int)(-Global.viewCenter.Y * Global.viewZoom) + 1000,
                                                       740,
                                                       510),
                                         new Rectangle(170,
                                                       342,
                                                       170,
                                                       170), Color.White);


            spriteBatch.Draw(texMonitor, new Rectangle((int)(-Global.viewCenter.X * Global.viewZoom) + 1060,
                                                       (int)(-Global.viewCenter.Y * Global.viewZoom) - 190,
                                                       510,
                                                       510),
                                         new Rectangle(342,
                                                       0,
                                                       170,
                                                       170), Color.White);

            spriteBatch.Draw(texMonitor, new Rectangle((int)(-Global.viewCenter.X * Global.viewZoom) + 1060,
                                                       (int)(-Global.viewCenter.Y * Global.viewZoom) + 320,
                                                       510,
                                                       680),
                                         new Rectangle(342,
                                                       170,
                                                       170,
                                                       170), Color.White);

            spriteBatch.Draw(texMonitor, new Rectangle((int)(-Global.viewCenter.X * Global.viewZoom) + 1060,
                                                       (int)(-Global.viewCenter.Y * Global.viewZoom) + 1000,
                                                       510,
                                                       510),
                                         new Rectangle(342,
                                                       342,
                                                       170,
                                                       170), Color.White);






            spriteBatch.Draw(texMonitor, new Rectangle((int)(-Global.viewCenter.X * Global.viewZoom)-180,
                                                       (int)(-Global.viewCenter.Y * Global.viewZoom)-190,
                                                       510,
                                                       510),
                                         new Rectangle(0,
                                                       0,
                                                       170,
                                                       170), Color.White);

            spriteBatch.Draw(texMonitor, new Rectangle((int)(-Global.viewCenter.X * Global.viewZoom) - 180,
                                                       (int)(-Global.viewCenter.Y * Global.viewZoom) + 320,
                                                       510,
                                                       680),
                                         new Rectangle(0,
                                                       170,
                                                       170,
                                                       170), Color.White);

            spriteBatch.Draw(texMonitor, new Rectangle((int)(-Global.viewCenter.X * Global.viewZoom) - 180,
                                                       (int)(-Global.viewCenter.Y * Global.viewZoom) + 1000,
                                                       510,
                                                       510),
                                         new Rectangle(0,
                                                       342,
                                                       170,
                                                       170), Color.White);
             */
        }

        private void DrawCheckerboard()
        {
            float width = 350f;
            int invertAmm = 50;
            int invertCurrent = invertAmm;
            float invert = 0f;

            float y = 300f;

            for (int i = 0; i < 300; i++)
            {
                float height = 1f + (1f-GetBallPercent().Y)/2f;
                

                Color c = new Color(1.0f, 1.0f, 1.0f);

                if (invertCurrent == 0)
                {
                    invertAmm -= 4;

                    if (invertAmm <= 0) invertAmm = 1;

                    if (invert == 0f) invert = 1f;
                    else invert = 0f;

                    invertCurrent = invertAmm;
                }
                invertCurrent--;

                float _width = width - i;
                

                for (int j = 0; j < 10; j++)
                {
                    float x = (float)(j)*_width*2f + invert*_width;
                    x -= ((float)i) * ball.GetPos().X / 300f;

                    

                    Editor.sprDebug.Draw(new IntVector2D((int)(x),
                                                         (int)(y)),
                                         new IntVector2D((int)(_width),
                                                         (int)(height)),
                                         c*(1f-((float)i/300f))); // PROPER ALPHA
                }

                y -= height;
            }


            /*DrawCheck(5, 2);
            DrawCheck(7, 2);
            DrawCheck(9, 2);

            DrawCheck(5, 3);
            DrawCheck(7, 3);
            DrawCheck(9, 3);

            DrawCheck(5, 4);
            DrawCheck(7, 4);
            DrawCheck(9, 4);*/
        }

        private void DrawCheck(float _x, float _y)
        {
            float width = 60f + _y*20f;
            float height = 5f;

            for (int i = 0; i < 20; i++)
            {
                Editor.sprDebug.Draw(new IntVector2D((int)(_x*width-i*0.5f),
                                                     (int)(_y*100f+i*5f)),
                                     new IntVector2D((int)(width+i),
                                                     (int)(5f)),
                                     Color.White);
            }
        }

        private Vector2 GetBallPercent()
        {
            if (room == null) return Vector2.Zero;

            return new Vector2(ball.GetPos().X / room.GetRoomSize().X,
                               ball.GetPos().Y / room.GetRoomSize().Y);
        }

        public void Draw()
        {
            if (room == null) return;

            //room.DrawShadow(new IntVector2D(0, 0));

            // DRAW THE SPRITES DIFFERENTLY STUPID! DON'T CHANGE ANYTHING ACTUALLY FUNCTIONAL DUMMY!
            //afjdk

            //DrawCheckerboard();

            room.DrawShadow(new IntVector2D(0, 0));
            room.Draw(new IntVector2D(0,0));
            ball.Draw();

            Global.SetViewCenter(ball.GetPos());
        }
    }
}
