using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CircusCharlie.Classes
{
    class Enemy : Actor
    {
        public Animation anim;

        public Enemy(Vector2 _pos, Sprite spr)
        {
            anim = new Animation(spr);
            anim.SetAnimation(1, 9);

            pos = _pos;

            AddCol(new ColCircle(pos,
                                 new Vector2(0, 0),
                                 16));
        }

        public override void Draw()
        {
            if (destroyed) return;

            anim.DrawView(pos-new Vector2(12,12), 0.5f);

            //spr.DrawView(pos, new IntVector2D(0,0), 0, 1f, Color.White);
            DrawCol();
        }

    }
}
