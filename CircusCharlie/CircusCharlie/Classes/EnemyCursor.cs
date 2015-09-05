using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CircusCharlie.Classes
{
    class EnemyCursor : Enemy
    {
        public EnemyCursor(Vector3 _pos, Sprite _spr)
            : base(_pos, _spr, 1f, 1f,
                   new Vector2(17f/24f, 32f/24f),
                   new Vector2(17f/64f, 1f),
                   Vector2.Zero, 0f)
        {
            pos = Vector3.One * 3f;
        }

        public override void Draw()
        {
            bill.UpdatePos(pos);
            bill.Draw();
        }
    }
}
