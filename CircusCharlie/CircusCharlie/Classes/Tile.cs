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

        // Used for 3D
        bool nTop = false;
        bool nBottom = false;
        bool nLeft = false;
        bool nRight = false;

        Cube cube {get; set;}

        /*public Tile()
        {
            spr = null;
            off = new IntVector2D(0,0);
            pos = new IntVector2D(0, 0);

            color = 0;
        }*/
        public Tile(Sprite _spr, IntVector2D _off, IntVector2D _pos)
        {
            spr = _spr;
            off = _off;
            pos = _pos;
            rotation = 0;

            color = 0;

            //cube = new Cube(spr, pos, Vector2.One*Global.gridSize, 1f, 10f);
        }

        public void SetNeighbours(bool t, bool r, bool b, bool l)
        {
            nTop = t;
            nRight = r;
            nBottom = b;
            nLeft = l;
        }

        public Tile(Tile other)
        {
            spr = other.spr;
            off = other.off;
            pos = other.pos;
            rotation = other.rotation;
            color = other.color;
            cube = other.cube;
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

            spr.DrawView(new Vector2(pos.X, pos.Y) * Global.gridSize, off * Global.tileSize, rotation, 3f, Editor.COLORS[color]);

        }

        public void DrawEditor()
        {
            Rotate(pos * Global.gridSize, new IntVector2D(Global.gridSize, Global.gridSize));

            spr.Draw(new IntVector2D(pos.X, pos.Y) * Global.gridSize, off * Global.tileSize, rotation, 3f, Editor.COLORS[color]);

        }

        public void Draw(IntVector2D _pos)
        {
            Rotate(_pos + pos * Global.gridSize, new IntVector2D(Global.gridSize, Global.gridSize));

            spr.DrawView(new Vector2(_pos.X + pos.X, _pos.Y + pos.Y) * Global.gridSize
                         /*+ Global.Get3DRatio(new Vector2(pos.X, pos.Y) * Global.gridSize) * new Vector2(20f, 20f) * Global.viewZoom*/,

                         off * Global.tileSize, rotation, 3f, Editor.COLORS[color]);
        }

        public void DrawShadow(IntVector2D _pos)
        {
            /*************************/






            // First: Move the main image. Do NOT use the "Get3DRatio". That is wrong. Just move the main image faster than the normal view moves.
            // Then, do the 3D BASED on the movement of the main image.


            // IVE GOT IT!
            // The part which it jutting into the camera needs to be scaled LARGER based on the speed SLOWER that it moves. This means more gaps between tiles cos they're technically "larger".
            // See Blender. Run some tests. Perhaps the same scale factor up can be the slowdown factor.






            /*************************/







            // Get the perspective of the block.
            Vector2 pers = Global.Get3DRatio(new Vector2(pos.X, pos.Y) * Global.gridSize) * new Vector2(12f, 12f) * Global.viewZoom;
            SpriteBatch spriteBatch = spr.GetSpriteBatch();
            Texture2D tex = spr.GetTexture();

            Vector3 temp = Editor.COLORS[color].ToVector3();

            Vector2 posOff = Vector2.Zero;//Global.Get3DRatio(new Vector2(pos.X, pos.Y) * Global.gridSize) * new Vector2(20f, 20f) * Global.viewZoom;

            if (!nBottom && pers.Y > 0f)
            {
                Color c = new Color(temp * 0.5f);

                int segmentCount = Math.Abs((int)pers.Y);
                float segmentIndent = pers.X / pers.Y;
                float segmentSize = 8f / pers.Y;
                float segmentCurrent = 0f;
                float segmentCurrentIndent = segmentIndent;

                for (int i = 0; i < segmentCount; i++)
                {
                    spriteBatch.Draw(tex, new Rectangle((int)(((pos.X) * Global.gridSize) * Global.viewZoom - Global.viewCenter.X * Global.viewZoom + segmentCurrentIndent + posOff.X),
                                                        (int)(((pos.Y + 1) * Global.gridSize) * Global.viewZoom - Global.viewCenter.Y * Global.viewZoom + i + posOff.Y),
                                                        (int)(Global.gridSize * Global.viewZoom),
                                                        (int)1),
                                          new Rectangle((int)(off.X * Global.tileSize),
                                                        (int)(off.Y * Global.tileSize + segmentCurrent),
                                                        (int)(8),
                                                        (int)(1)),
                                                        c);

                    segmentCurrent += segmentSize;
                    segmentCurrentIndent += segmentIndent;
                }
            }

            if (!nRight && pers.X > 0f)
            {
                Color c = new Color(temp * 0.8f);

                int segmentCount    = Math.Abs((int)pers.X);
                float segmentSize   = 8f / pers.X;
                float segmentIndent = pers.Y / pers.X;
                float segmentCurrent = 0f;
                float segmentCurrentIndent = segmentIndent;

                for (int i = 0; i < segmentCount; i++)
                {
                    spriteBatch.Draw(tex, new Rectangle((int)(((pos.X+1)*Global.gridSize) * Global.viewZoom - Global.viewCenter.X * Global.viewZoom + i + posOff.X),
                                                        (int)(((pos.Y) * Global.gridSize) * Global.viewZoom - Global.viewCenter.Y * Global.viewZoom + segmentCurrentIndent + posOff.Y),
                                                        (int)1,
                                                        (int)(Global.gridSize * Global.viewZoom)),
                                          new Rectangle((int)(off.X*Global.tileSize + segmentCurrent),
                                                        (int)(off.Y*Global.tileSize),
                                                        (int)(1),
                                                        (int)(8)),              
                                                        c);

                    segmentCurrent += segmentSize;
                    segmentCurrentIndent += segmentIndent;
                }
            }

            // Draw the left/right side in 3D.

            //Rotate(_pos + pos * Global.gridSize, new IntVector2D(Global.gridSize, Global.gridSize));

            //Color shadow = new Color((int)Editor.COLORS[color].R / 8, (int)Editor.COLORS[color].G / 8, (int)Editor.COLORS[color].B / 8, 170);

            //spr.DrawView(new Vector2(_pos.X + pos.X, _pos.Y + pos.Y) * Global.gridSize + shadowMove, off * Global.tileSize, rotation, 3f, shadow);
        }
    }
}
