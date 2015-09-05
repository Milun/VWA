using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CircusCharlie.Classes
{
    class BlockDoom : Block
    {
        public BlockDoom(Vector3 _pos, Sprite _spr) : base(_pos, _spr)
        {

        }

        protected override void ActorCol(Actor other, Vector2 collision)
        {
            // If it collides with the ball, kill the ball!
            if (other.GetType() == typeof(Ball))
            {
                if (collision.Y != 0f)
                {
                    MainGame.ball.Reset();
                }
            }
        }
    }
}
