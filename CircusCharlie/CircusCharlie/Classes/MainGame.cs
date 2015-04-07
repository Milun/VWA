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

        SpriteBatch spriteBatch;
        Ball ball;
        public static Room room;

        // Constructor
        public MainGame(SpriteBatch _spriteBatch, ContentManager Content)
        {
            spriteBatch = _spriteBatch;
            ball = new Ball(new Sprite(Content.Load<Texture2D>("Sprites/spr_ball"),
                            ref spriteBatch));

            texBG = Content.Load<Texture2D>("Sprites/spr_bg");
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
            Global.viewCenter = Vector2.Zero;
            Editor.showDebug = false;
        }

        public void DrawBackdrop()
        {
            // Draw black backdrop
            spriteBatch.Draw(texBG, new Rectangle((int)(ball.GetPos().X/-2f)-50,
                                                  (int)(ball.GetPos().Y/-2f)-50,
                                                  1392,
                                                  1056), Color.White);
        }

        public void Draw()
        {
            if (room == null) return;

            //room.DrawShadow(new IntVector2D(0, 0));

            // DRAW THE SPRITES DIFFERENTLY STUPID! DON'T CHANGE ANYTHING ACTUALLY FUNCTIONAL DUMMY!
            //afjdk

            room.DrawShadow(new IntVector2D(0, 0));
            room.Draw(new IntVector2D(0,0));
            ball.Draw();

            Global.viewCenter = ball.GetPos() - new Vector2(250, 140);
        }
    }
}
