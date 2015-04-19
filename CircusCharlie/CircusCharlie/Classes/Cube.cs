using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CircusCharlie.Classes
{
    class Cube
    {
        private Sprite spr;
        private Vector2 pos = Vector2.Zero;
        private Vector2 size = Vector2.Zero;
        private float z = 1;        // Larger = closer to camera.
        private float depth = 10f;  // Length of the cube in z.

        public Cube(Sprite _spr, Vector2 _pos, Vector2 _size, float _z, float _depth)
        {
            spr = _spr;
            pos = _pos;
            size = _size;
            z = _z;
            depth = _depth;
        }

        public void DrawDepth()
        {

        }

        public void DrawMain()
        {
            spr.DrawView(pos, Color.White);
        }
    }
}
