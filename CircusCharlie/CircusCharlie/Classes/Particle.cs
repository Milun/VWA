using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CircusCharlie.Classes
{
    class Particle
    {
        private Billboard bill;

        private Vector2 move;
        private float rotation;
        private float rotate = 0f;

        private Vector2 pos;
        private float time = 0.0f;
        private float lifetime;

        public Particle(Vector2 _pos, float z, Texture2D tex, Vector2 size, int frame, Vector2 offSize, Vector2 _move, float _rotate, float _lifetime)
        {
            pos = _pos;
            move = _move;

            lifetime = _lifetime;
            rotate = _rotate;
            rotation = Editor.random.Next(0, 360);

            bill = new Billboard(tex, pos, Vector2.Zero, size, frame, Vector2.One * 0.25f, z);
        }

        public void Draw()
        {
            // As long as the particle is alive, animate it.
            if (time < lifetime)
            {
                time += 0.1f;
                rotation += rotate;
                pos += move;

                bill.SetRotation(rotation);
                bill.SetAlpha(1f - (time / lifetime));
                bill.UpdatePos(pos);

                bill.Draw();
            }
        }
    }
}
