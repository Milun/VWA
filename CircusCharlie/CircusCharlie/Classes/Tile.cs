using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CircusCharlie.Classes
{
    class Tile
    {
        private const int tileSize = 8; // Size of the tiles sprite.
        private const int tileScale = 3; // Visible scale.

        private Sprite spr {get; set;}
        private int rotation {get; set;}

        private IntVector2D off {get; set;}
        private IntVector2D pos { get; set; }
        private int color { get; set; }

        private bool pressed = false;

        public IntVector2D getPos() { return pos; }
        public void setPos(IntVector2D _pos) { pos = _pos; }

        public Tile()
        {
            spr = null;
            off = new IntVector2D(0,0);
            pos = new IntVector2D(0, 0);

            color = 0;
        }
        public Tile(Sprite _spr, IntVector2D _off, IntVector2D _pos)
        {
            spr = _spr;
            off = _off;
            pos = _pos;
            rotation = 0;

            color = 0;
        }

        public Tile(Tile other)
        {
            spr = other.spr;
            off = other.off;
            pos = other.pos;
            rotation = other.rotation;
            color = other.color;
        }

        public IntVector2D GetOff()
        {
            return off;
        }

        public int GetRotation()
        {
            return rotation;
        }
        public void SetRotation(int val)
        {
            rotation = val;
        }

        public void SetColor(int _color)
        {
            color = _color;
        }

        public int GetColor()
        {
            return color;
        }

        private void Rotate(IntVector2D p, IntVector2D s)
        {

            if (!Keyboard.GetState().IsKeyDown(Keys.R))
            {
                pressed = false;
                return;
            }
            else if (pressed)
            {
                return;
            }

            pressed = true;

            if (Mouse.GetState().X > p.X &&
                Mouse.GetState().X < p.X + s.X &&
                Mouse.GetState().Y > p.Y &&
                Mouse.GetState().Y < p.Y + s.Y
                )
            {
                if (rotation < 3)
                    rotation++;
                else
                    rotation = 0;

                
            }
        }

        public void Draw()
        {
            Rotate(pos * Global.gridSize, new IntVector2D(Global.gridSize, Global.gridSize));

            spr.DrawView(pos * Global.gridSize, off * Global.tileSize, rotation, 3f, Editor.COLORS[color]);

        }

        public void Draw(IntVector2D _pos)
        {
            Rotate(_pos + pos * Global.gridSize, new IntVector2D(Global.gridSize, Global.gridSize));

            spr.DrawView(_pos + pos * Global.gridSize, off * Global.tileSize, rotation, 3f, Editor.COLORS[color]);
        }

        public void DrawShadow(IntVector2D _pos)
        {
            Rotate(_pos + pos * Global.gridSize, new IntVector2D(Global.gridSize, Global.gridSize));

            Color shadow = new Color((int)Editor.COLORS[color].R / 8, (int)Editor.COLORS[color].G / 8, (int)Editor.COLORS[color].B / 8, 170);

            spr.DrawView(_pos + pos * Global.gridSize + new IntVector2D(2, 2), off * Global.tileSize, rotation, 3f, shadow);
        }
    }
}
