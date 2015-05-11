using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CircusCharlie.Classes
{
    class EnemyHead : Enemy
    {
        EnemyBullet bullet; // The bullet the head shoots.
        bool wait = true;
        int interval = 0;
        float minDistance = 10.0f;

        public EnemyHead(Vector2 _pos, Sprite _spr, float _flipY) : base(_pos, _spr, -1f, _flipY, Vector2.One*3f, Vector2.One, new Vector2(0f, 0.5f), 2.0f)
        {
            bullet = new EnemyBullet(new Vector2(pos.X, pos.Y - 1f), MainGame.sprBall, this);
        }

        public void WaitToShoot()
        {
            wait = true;
        }

        public override void Draw()
        {
            DrawShadow();

            bill.Draw();
            DrawCol(Editor.colorDebug2);            
            
            // Only shoot if in range (and at intervals)
            if (wait)
            {
                if (interval >= 30)
                {
                    interval = 0;

                    // Get vector to ball.
                    Vector2 ball = MainGame.ball.GetPos();
                    Vector2 vec = new Vector2(ball.X - pos.X, ball.Y - pos.Y);

                    // If the ball is in range, shoot.
                    if (vec.Length() <= minDistance) wait = false;
                }
                else
                {
                    interval++;
                }
            }
            else 
            {
                bullet.Draw();
            }
        }
    }
}
