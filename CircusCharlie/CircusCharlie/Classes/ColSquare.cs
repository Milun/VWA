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

        private const float leeway = 1f;

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

            Editor.sprDebug.Draw(new IntVector2D((int)tL.X+1, (int)tL.Y+1),
                                 new IntVector2D((int)size.X-2, (int)size.Y-2), Editor.colorDebug);
        }

        public Vector2 Center
        {
            get
            {
                return origin + pos + size/2f;
            }
        }

        public override Vector2 CheckColCircle(ColCircle other)
        {
            if (CheckColBounds(other))
            {
                if (other.Center.X > this.TL.X &&
                    other.Center.X < this.BR.X)
                {
                    if (other.Center.Y > this.Center.Y)
                    {
                        return new Vector2(0.0f, other.Center.Y - other.Rad - this.TL.Y);
                    }
                    if (other.Center.Y < this.Center.Y)
                    {
                        return new Vector2(0.0f, other.Center.Y + other.Rad - this.BR.Y);
                    }
                }
                if (other.Center.Y < this.BR.Y + leeway && other.Center.Y > this.TL.Y - leeway)
                {
                    if (other.Center.X > this.Center.X)
                    {
                        return new Vector2(other.Center.X - other.Rad - this.TL.X, 0.0f);
                    }
                    if (other.Center.X < this.Center.X)
                    {
                        return new Vector2(other.Center.X + other.Rad - this.BR.X, 0.0f);
                    }
                }

                Vector2 dist;
                dist = new Vector2(TL.X + leeway, BR.Y + leeway) - other.Center;
                float mag = dist.Length();
                dist.Normalize();
                if (mag < other.Rad) return -dist * (other.Rad - mag);

                dist = new Vector2(BR.X - leeway, BR.Y + leeway) - other.Center;
                mag = dist.Length();
                dist.Normalize();
                if (mag < other.Rad) return -dist * (other.Rad - mag);

                dist = new Vector2(TL.X + leeway, TL.Y - leeway) - other.Center;
                mag = dist.Length();
                dist.Normalize();
                if (mag < other.Rad) return -dist * (other.Rad - mag);

                dist = new Vector2(BR.X - leeway, TL.Y - leeway) - other.Center;
                mag = dist.Length();
                dist.Normalize();
                if (mag < other.Rad) return -dist * (other.Rad - mag);
            }

            return Vector2.Zero;
        }

        public override Vector2 CheckColSquare(ColSquare other)
        {
            if (!CheckColBounds(other)) return Vector2.Zero;

            float left = Math.Abs(other.TL.X - BR.X);
            float right = Math.Abs(other.BR.X - TL.X);
            float down = Math.Abs(other.TL.Y - BR.Y);
            float up = Math.Abs(other.BR.Y - TL.Y);

            if (left <= right && left <= up && left <= down) return new Vector2(-left, 0.0f);
            if (right <= left && right <= up && right <= down) return new Vector2(right, 0.0f);
            if (up <= right && up <= left && up <= down) return new Vector2(0.0f, up);
            if (down <= right && down <= left && down <= up) return new Vector2(0.0f, -down);
            
            return Vector2.Zero;
        }
    }
}
