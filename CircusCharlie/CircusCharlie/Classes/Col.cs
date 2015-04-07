using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CircusCharlie.Classes
{
    class Col
    {
        protected Vector2 tL = new Vector2(-1.0f, -1.0f);
        protected Vector2 bR = new Vector2(1.0f, 1.0f);
        protected Vector2 pos = Vector2.Zero;

        public virtual void Flip()
        {
            return;
        }

        public virtual void UpdatePos(Vector2 _pos)
        {
            pos = _pos;
        }

        public virtual void DrawDebug()
        {
            return;
        }

        public virtual void GlobalScale(float scale)
        {
            return;
        }

        public virtual Vector2 Pos
        {
            get
            {
                return pos;
            }

            set
            {
                pos = value;
            }
        }

        public virtual Vector2 TL
        {
            get
            {
                return tL;
            }
        }

        public virtual Vector2 BR
        {
            get
            {
                return bR;
            }
        }

        protected bool CheckColBounds(Col other)
        {
            if (TL.X > other.BR.X) return false;
            if (BR.X < other.TL.X) return false;
            if (TL.Y > other.BR.Y) return false;
            if (BR.Y < other.TL.Y) return false;

            return true;
        }

        public virtual Vector2 CheckColCircle(ColCircle other)
        {
            return Vector2.Zero;
        }

        public virtual Vector2 CheckColSquare(ColSquare other)
        {
            return Vector2.Zero;
        }

        public Vector2 CheckCol(Col other)
        {
            if (other.GetType() == typeof(ColCircle))
            {
                return this.CheckColCircle((ColCircle)other);
            }
            else if (other.GetType() == typeof(ColSquare))
            {
                return this.CheckColSquare((ColSquare)other);
            }

            return Vector2.Zero;
        }
    }
}
