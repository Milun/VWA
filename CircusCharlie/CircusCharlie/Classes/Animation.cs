using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CircusCharlie.Classes
{
    class Animation
    {
        public Sprite spr;
        public int start = 0;
        public int end = 0;

        public float timer = 0f;
        public float speed = 1f;
        public int frame = 0;

        public Animation(Sprite _spr)
        {
            spr = _spr;

            
        }

        public void SetAnimation(int _start, int _end)
        {
            start = _start;
            end = _end;
        }

        private void Animate()
        {
            timer -= 0.1f;
            if (timer < 0f)
            {
                timer = speed;
                frame++;

                if (frame > end) frame = start;
            }
        }

        public void DrawView(Vector2 pos, float scale = 1f)
        {
            Animate();
            //spr.DrawView(pos, new IntVector2D(spr.Width*frame, 0), 0, scale, Color.White);
        }

    }
}
