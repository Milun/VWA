using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CircusCharlie.Classes
{
    class ColSquare : Col
    {
        private Vector2 origin; // Relative
        private Vector2 size;   // Relative

        public ColSquare(Vector2 _pos, Vector2 _origin, Vector2 _size)
        {
            pos = _pos;         // Actual position on screen.
            origin = _origin;   // Plus the origin (to offset).
            size = _size;       // Box is this big.

            tL = pos + origin;
            bR = pos + origin + size;
        }

        public override void UpdatePos(Vector2 _pos)
        {
            base.UpdatePos(_pos);

            // Update bounding boxes.
            tL = pos + origin;
            bR = pos + origin + size;
        }

        public override void GlobalScale(float scale)
        {
            pos *= scale;
            origin *= scale;
            size *= scale;

            tL = pos + origin;
            bR = pos + origin + size;
        }

        public override void DrawDebug()
        {
            if (!Editor.showDebug) return;

            Editor.sprDebug.Draw(new IntVector2D((int)tL.X, (int)tL.Y),
                                 new IntVector2D((int)size.X, (int)size.Y), Editor.colorDebug);
        }

        /*public override Vector2 CheckColCircle(TwoColCircle other)
        {
            if (CheckColBounds(other))
            {
                print("homestar");

                if (other.Center.x > this.BL.x && other.Center.x < this.TR.x)
                {
                    if (other.Center.y > this.Center.y) return new Vector2(0.0f, other.Center.y - other.Rad - this.TR.y);
                    if (other.Center.y < this.Center.y) return new Vector2(0.0f, other.Center.y + other.Rad - this.BL.y);
                }
                if (other.Center.y > this.BL.y && other.Center.y < this.TR.y)
                {
                    if (other.Center.x > this.Center.x) return new Vector2(other.Center.x - other.Rad - this.TR.x, 0.0f);
                    if (other.Center.x < this.Center.x) return new Vector2(other.Center.x + other.Rad - this.BL.x, 0.0f);
                }



                Vector2 dist;
                dist = new Vector2(BL.x, TR.y) - other.Center;
                if (dist.magnitude < other.Rad) return dist.normalized * (other.Rad - dist.magnitude);

                dist = BL - other.Center;
                if (dist.magnitude < other.Rad) return dist.normalized * (other.Rad - dist.magnitude);

                dist = TR - other.Center;
                if (dist.magnitude < other.Rad) return dist.normalized * (other.Rad - dist.magnitude);

                dist = new Vector2(TR.x, BL.y) - other.Center;
                if (dist.magnitude < other.Rad) return dist.normalized * (other.Rad - dist.magnitude);
            }

            return Vector2.zero;
        }*/

        public override Vector2 CheckColSquare(ColSquare other)
        {
            if (!CheckColBounds(other)) return Vector2.Zero;

            float left  = Math.Abs(other.TL.X - BR.X);
            float right = Math.Abs(other.BR.X - TL.X);
            float down =  Math.Abs(other.TL.Y - BR.Y);
            float up =    Math.Abs(other.BR.Y - TL.Y);

            if (left <= right && left <= up   && left <= down)  return new Vector2(-left, 0.0f);
            if (right <= left && right <= up  && right <= down) return new Vector2(right, 0.0f);
            if (up <= right   && up <= left   && up <= down)    return new Vector2(0.0f, up);
            if (down <= right && down <= left && down <= up)    return new Vector2(0.0f, -down);
            
            return Vector2.Zero;
        }
    }
}
