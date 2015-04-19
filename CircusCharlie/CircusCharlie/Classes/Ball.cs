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
    class Ball : Actor
    {
        private Sprite spr;

        private const float BALLSIZE = 16f;
        private float YSPEED = 2f;
        private const float XSPEED = 1.5f;

        private float xSpeed = 1f;
        private float ySpeed = 2f;
        private float ySpeedAdd = 0f;

        public Ball(Sprite _spr) : base()
        {
            spr = _spr;

            AddCol(new ColCircle(pos,
                                 new Vector2(0, 0),
                                 BALLSIZE));
        }

        public float GetYspeed()
        {
            return ySpeed;
        }

        public Vector2 GetPos()
        {
            return pos;
        }

        public void SetPos(IntVector2D _pos)
        {
            pos = new Vector2(_pos.X, _pos.Y);
            UpdateCol(pos);
        }



        private void Move()
        {
            // Check collisions
            Vector2 collision = MainGame.room.CheckCol(this);


            if (Keyboard.GetState().IsKeyDown(Keys.Space)) return;

            // Move first
            if (collision.X >= 0f && Keyboard.GetState().IsKeyDown(Keys.A))
            {
                pos -= new Vector2(XSPEED, 0);
            }
            if (collision.X <= 0f &&  Keyboard.GetState().IsKeyDown(Keys.D))
            {
                pos += new Vector2(XSPEED, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                ySpeedAdd = -YSPEED / 2f;
            }
            else
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                ySpeedAdd = YSPEED / 2f;
            }
            else
            {
                ySpeedAdd = 0f;
            }

            // Collision below
            if (collision.Y > 0f) ySpeed = -YSPEED;
            // Collision above
            else if (collision.Y < 0f) ySpeed = YSPEED;

            pos += new Vector2(0, ySpeed+ySpeedAdd);

            UpdateCol(pos);
        }

        public override void Draw()
        {
            Color shadow = new Color(0, 0, 0, 100);

            /*spr.DrawView(new IntVector2D((int)(pos.X + 2 - BALLSIZE / 2f), (int)(pos.Y + 2 - BALLSIZE / 2f)),
                     new IntVector2D((int)BALLSIZE + 1, (int)BALLSIZE+1), shadow);
            */

            spr.DrawView(new Vector2(pos.X - BALLSIZE / 2f,
                                     pos.Y - BALLSIZE / 2f),
                         new IntVector2D((int)BALLSIZE,
                                         (int)BALLSIZE), Color.White);

            
            DrawCol();

            Move();

            UpdateCol();
        }

        public override float GetValue(string name)
        {
            if (name == "yspeed") return ySpeed;

            return 0f;
        }

    }
}
