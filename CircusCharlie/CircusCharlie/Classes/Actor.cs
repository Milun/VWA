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
        protected Vector3 pos = Vector3.Zero;

        public List<Col> cols;
        public List<Col> trigs;

        public Actor()
        {
            cols = new List<Col>();
            trigs = new List<Col>();
        }

        protected void AddCol(Col col)
        {
            if (cols == null) return;
            cols.Add(col);
        }

        protected void AddTrig(Col col)
        {
            if (trigs == null) return;
            trigs.Add(col);
        }

        protected void UpdateCol(Vector2 _pos)
        {
            foreach (Col e in cols)
            {
                e.UpdatePos(_pos);
            }

            foreach (Col e in trigs)
            {
                e.UpdatePos(_pos);
            }
        }

        public bool IsAlive()
        {
            return !destroyed;
        }

        public virtual void Destroy()
        {
            destroyed = true;
        }

        public virtual void UnDestroy()
        {
            destroyed = false;
        }

        public virtual void Reset()
        {
            destroyed = false;
        }

        public virtual void Draw()
        {

        }

        public Vector3 GetPos()
        {
            return pos;
        }

        public virtual void DrawEditor()
        {

        }

        public List<Col> GetCol()
        {
            return cols;
        }

        public List<Col> GetTrigs()
        {
            return trigs;
        }

        public virtual Vector2 CheckCol(Actor other)
        {
            if (!other.IsAlive() || !IsAlive()) return Vector2.Zero;

            Vector2 output = Vector2.Zero;

            foreach (Col e in trigs)
            {
                foreach (Col f in other.GetCol())
                {
                    output += f.CheckCol(e);
                }
            }

            // Make both act on a collision if it occurs.
            if (output != Vector2.Zero)
            {
                ActorCol(other, output);
                other.ActorCol(this, output);
            }

            return output;
        }

        public virtual Vector2 CheckTrig(Actor other)
        {
            if (!other.IsAlive() || !IsAlive()) return Vector2.Zero;

            Vector2 output = Vector2.Zero;

            foreach (Col e in trigs)
            {
                foreach (Col f in other.GetTrigs())
                {
                    output += f.CheckCol(e);
                }
            }

            // Make both act on a collision if it occurs.
            if (output != Vector2.Zero)
            {
                ActorCol(other, output);
                other.ActorCol(this, output);
            }

            return output;
        }

        protected virtual void ActorColGeneric(Vector2 collision)
        {
            return;
        }

        protected virtual void ActorCol(Actor other, Vector2 collision)
        {
            return;
        }

        public virtual float GetValue(string name)
        {
            return 0f;
        }

        public virtual void PassMsg(string msg)
        {
            return;
        }

        public virtual Vector2 CheckCol(Col other)
        {
            if (!IsAlive()) return Vector2.Zero;

            Vector2 output = Vector2.Zero;

            foreach (Col e in cols)
            {
                output += e.CheckCol(other);
            }

            if (output != Vector2.Zero)
            {
                ActorColGeneric(output);
            }

            return output;
        }

        public virtual Vector2 CheckTrig(Col other)
        {
            if (!IsAlive()) return Vector2.Zero;

            Vector2 output = Vector2.Zero;

            foreach (Col e in trigs)
            {
                /*Console.WriteLine(other.TL.X + "," + other.TL.Y + " - " + other.BR.X + "," + other.BR.Y + " +++ " +
                                  e.TL.X + "," + e.TL.Y + " - " + e.BR.X + "," + e.BR.Y);
                */
                output += e.CheckCol(other);
            }

            if (output != Vector2.Zero)
            {
                ActorColGeneric(output);
            }

            return output;
        }

        public void DrawCol(Color color)
        {
            foreach (Col e in cols)
            {
                e.DrawDebug(color);
            }

            foreach (Col e in trigs)
            {
                e.DrawDebug(Color.BlueViolet);
            }
        }

        public void UpdateCol()
        {
            foreach (Col e in cols)
            {
                e.UpdatePos(new Vector2(pos.X, pos.Y));
            }

            foreach (Col e in trigs)
            {
                e.UpdatePos(new Vector2(pos.X, pos.Y));
            }
        }
    }
}
