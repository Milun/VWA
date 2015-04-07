using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CircusCharlie.Classes
{
    class Actor
    {
        protected bool destroyed = false;

        public List<Col> cols;

        public Actor()
        {
            cols = new List<Col>();
        }

        protected void AddCol(Col col)
        {
            if (cols == null) return;
            cols.Add(col);
        }

        protected void UpdateCol(Vector2 _pos)
        {
            foreach (Col e in cols)
            {
                e.UpdatePos(_pos);
            }
        }

        public bool IsAlive()
        {
            return !destroyed;
        }

        public void Destroy()
        {
            destroyed = true;
        }

        public void UnDestroy()
        {
            destroyed = false;
        }

        public virtual void Draw()
        {

        }

        public List<Col> GetCol()
        {
            return cols;
        }

        public virtual Vector2 CheckCol(Actor other)
        {
            if (!other.IsAlive() || !IsAlive()) return Vector2.Zero;

            Vector2 output = Vector2.Zero;

            foreach (Col e in cols)
            {
                foreach (Col f in other.GetCol())
                {
                    output += f.CheckCol(e);
                }
            }

            return output;
        }

        public virtual Vector2 CheckCol(Col other)
        {
            Vector2 output = Vector2.Zero;

            foreach (Col e in cols)
            {
                output += e.CheckCol(other);
            }

            return output;
        }

        public void DrawCol()
        {
            foreach (Col e in cols)
            {
                e.DrawDebug();
            }
        }
    }
}
