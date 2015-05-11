using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CircusCharlie.Classes
{
    class Tile
    {
        private Sprite spr {get; set;}

        private Vector2 off {get; set;}
        private Vector2 pos { get; set; }

        public Vector2 getPos() { return pos; }
        public void setPos(Vector2 _pos) { pos = _pos; }

        Cube cube {get; set;}

        public Tile(Sprite _spr, Vector2 _off, Vector2 _pos)
        {
            spr = _spr;
            off = _off;
            pos = _pos;

            cube = new Cube(spr.GetTexture(),
                            new Vector3(pos.X, pos.Y, -0.5f),
                            new Vector3(1f, 1f, 1f),
                            _off,
                            Vector2.One * 0.25f);
        }

        public void SetNeighbours(bool t, bool r, bool b, bool l)
        {
            cube.SetNeighbours(t, r, b, l);
        }

        public Tile(Tile other)
        {
            spr = other.spr;
            off = other.off;
            pos = other.pos;
            cube = other.cube;
        }

        public Vector2 GetOff()
        {
            return off;
        }

        public void Draw(IntVector2D _pos)
        {
            Texture2D tex = spr.GetTexture();

            // Get offset.
            Vector2 temp = new Vector2
                                (
                                    (int)(tex.Width / 4 * off.X),
                                    (int)(tex.Height / 4 * off.Y)
                                );


            spr.DrawView(new Vector2(pos.X, pos.Y) * Global.gridSize,
                         Vector2.One*Global.gridSize,
                         temp,
                         new Vector2(tex.Width / 4, tex.Height / 4),
                         Color.White);
        }

        public void Draw3D()
        {
            /********/
            /*      */
            /*  3D  */
            /*      */
            /********/

            cube.Draw();
        }
    }
}
