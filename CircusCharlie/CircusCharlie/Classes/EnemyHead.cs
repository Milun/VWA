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

        int mode = 0;

        bool stunned = false;

        float gravity = 0.008f;

        int interval = 0;
        int silly = 0;
        int sillyMode = 0;
        float minDistance = 1.0f;

        int faceDirection = 0;
        int faceSpeed = 4;

        public EnemyHead(Vector2 _pos, Sprite _spr, Sprite _spr2, float _flipY)
            : base(_pos, _spr, -1f, _flipY, new Vector2(1.5f, 2f), new Vector2(0.33f, 0.25f), new Vector2(0f, 0.5f), 2.0f)
        {
            bullet = new EnemyBullet(new Vector2(pos.X, pos.Y - 1f), _spr2, this);

            AddCol(new ColSquare(pos,
                                 new Vector2(-0.3f, -1.7f),
                                 new Vector2(0.6f, 1.7f)));
        }

        public void WaitToShoot()
        {
            //mode = 0;
        }

        private void Face(int i)
        {
            if (i * faceSpeed < faceDirection && faceDirection != -faceSpeed)
            {
                faceDirection--;
            }
            else if (i * faceSpeed > faceDirection && faceDirection != faceSpeed)
            {
                faceDirection++;
            }
        }

        public override void Draw()
        {
            // Look at the ball            
            Vector2 ball = MainGame.ball.GetPos();
            Vector2 ballPos = pos - ball;

            bill.SetFrame(faceDirection / faceSpeed + 1 + mode * 3);

            // If you're not stunned, look at the player.
            if (!stunned)
            { 
                if (ballPos.X < -minDistance)
                {
                    Face(-1);
                }
                else if (ballPos.X > minDistance)
                {
                    Face(1);
                }

                bill.SetFrame(faceDirection / faceSpeed + 1 + mode * 3);
            }

            // Fall!
            if (stunned)
            {
                ySpeed += gravity;
                pos.Y += ySpeed;
                mode = 2;

                Vector2 col = MainGame.room.CheckCol(new Vector2(pos.X, pos.Y + 0.2f), new Vector2(1.5f, 0.01f));

                MainGame.room.MsgCol(
                    new Vector2(pos.X - 0.3f, pos.Y),
                    new Vector2(0.6f, 0.1f), "dmgMax");

                if (col != Vector2.Zero)
                {
                    stunned = false;

                    pos.Y += col.Y + 0.2f;
                    ySpeed = 0f;
                }

                bill.UpdatePos(pos);
                UpdateCol();
            }
            // Silly
            else if (silly > 0 && sillyMode == 0)
            {
                silly--;
                mode = 3;

                if (silly != 0)
                { 
                    bill.UpdatePos(new Vector2(pos.X + (float)((silly/5) % 2) * 0.01f, pos.Y));
                }
                else
                {
                    bill.UpdatePos(pos);
                }
            }
            // Idle
            else if (interval < 140)
            {
                mode = 0;
                bool inRange = (ballPos.Length() < minDistance * 7f);

                if (interval < 110)
                {
                    interval++;
                }
                // You need to be in range for it to complete firing.
                else if (inRange)
                {
                    interval++;
                }
                else
                {
                    interval -= 40;
                }

                // If out of range, do silly things.
                if (!inRange && interval == 100)
                {
                    // 1 in 10 chance of silly.
                    if (Editor.random.Next(10) == 0)
                    {
                        silly = 50;
                        sillyMode = 0;
                    }
                }
            }
            else if (interval < 220)
            {
                int shake = 220 - interval;

                interval++;
                mode = 1;

                if (interval != 220)
                {
                    bill.UpdatePos(new Vector2(pos.X + (float)((shake / 2) % 2) / ((shake + 7)), pos.Y));
                }
                else
                {
                    bill.UpdatePos(pos);

                    // 1 in 4 chance of silly.
                    if (Editor.random.Next(6) == 0)
                    {
                        sillyMode = 1;
                    }
                }
            }
            else if (interval < 260)
            {
                if (interval == 220 && sillyMode != 1)
                {
                    bullet.ShootBullet(new Vector2(pos.X - 0.2f - 0.2f * (faceDirection/faceSpeed),
                                                   pos.Y - 1f));
                }

                interval++;

                // Stick your tongue out instead of shooting.
                if (sillyMode == 1)
                {
                    mode = 3;

                    if (interval != 260)
                    {
                        bill.UpdatePos(new Vector2(pos.X + (float)(((interval - 220) / 5) % 2) * 0.01f, pos.Y));
                    }
                    else
                    {
                        bill.UpdatePos(pos);
                        sillyMode = 0;
                    }
                }
                else
                {
                    mode = 2;
                }
            }
            else if (interval < 280)
            {
                interval = 0;
                mode = 0;
            }

            bullet.Draw();
            bill.Draw();

            DrawCol(Editor.colorDebug2);
        }

        public override void PassMsg(string msg)
        {
            // Get stunned from a block being hit.
            if (msg == "stun-above" || msg == "stun-below")
            {
                stunned = true;

                bill.SetFrame(7);
                mode = 0;
                interval = 0;

                // Fly/fall into the air.
                ySpeed = -0.1f * flipY;
            }
            // Fall if they somehow manage to break the block from the other side.
            else if (msg == "gone-above" || msg == "gone-below")
            {
                stunned = true;

                bill.SetFrame(7);
                mode = 0;
                interval = 0;
            }

            return;
        }
    }
}
