using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace CircusCharlie.Classes
{
    class Block : Actor
    {
        private Sprite spr;

        public Block(Vector2 _pos, Sprite _spr) : base()
        {
            pos = _pos;
            spr = _spr;
            AddCol(new ColSquare(new Vector2(pos.X * Global.gridSize, pos.Y * Global.gridSize), Vector2.Zero, new Vector2(48, 24)));
        }

        public override void Draw()
        {
            if (destroyed) return;

            spr.DrawView(pos * Global.gridSize, new IntVector2D(48, 24), Color.Aquamarine);
            DrawCol();
        }

        protected override void ActorCol(Actor other, Vector2 collision)
        {
            if (!IsAlive()) return;

            // If it collides with the ball, destroy the block!
            if (other.GetType() == typeof(Ball))
            {
                if ((collision.Y > 0f && other.GetValue("yspeed") < 0f) ||
                    (collision.Y < 0f && other.GetValue("yspeed") > 0f))
                {
                    Destroy();
                    return;
                }
            }
        }

    }
}
