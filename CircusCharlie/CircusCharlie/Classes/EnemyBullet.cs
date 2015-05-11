using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CircusCharlie.Classes
{
    class EnemyBullet : Enemy
    {
        float charge = 0f;        // Needs to charge up.
        EnemyHead head;           // Reference to the head which shot ya.

        float minDistance = 5.0f;
        Vector2 moveVector = Vector2.Zero;

        public EnemyBullet(Vector2 _pos, Sprite _spr, EnemyHead _head)
            : base(_pos, _spr, 1f, 1f, Vector2.One * 0.8f, Vector2.One, Vector2.Zero, 0.0f, -0.1f)
        {
            head = _head;

            AddCol(new ColCircle(pos,
                                 Vector2.Zero,
                                 0.8f));
        }

        public override void Reset()
        {
            base.Reset();

            pos = startPos;
            moveVector = Vector2.Zero;

            bill.UpdatePos(pos);
            UpdateCol();

            // Set some time before the bullet respawns.
            charge = 0f;
            bill.UpdateVerts(0f, 0f);
        }

        public override void Draw()
        {
            // Charge the bullet up.
            if (charge < 1f)
            {
                charge += 0.02f;

                bill.UpdateVerts(0.8f * charge, 0.8f * charge);

                // When you've stopped charging, fire in the direction of the ball.
                if (charge >= 1f)
                {
                    Vector2 ball = MainGame.ball.GetPos();
                    moveVector = new Vector2(ball.X - pos.X, ball.Y - pos.Y);
                    moveVector.Normalize();
                }

            }
            else
            {
                // Get vector to ball.
                Vector2 ball = MainGame.ball.GetPos();
                Vector2 vec = new Vector2(ball.X - pos.X, ball.Y - pos.Y);

                float length = vec.Length();

                // Check if the bullet has hit something.

                // First the player
                if (length < 0.7f)
                {
                    Reset();
                    head.WaitToShoot();

                    // Reset the bullet (temporary)
                    MainGame.ball.Reset();

                    return;
                }

                // Second, a wall
                Vector2 collision = MainGame.room.CheckCol(this);

                // Reset on hit (go back to the start)
                if (collision != Vector2.Zero)
                {
                    Reset();
                    // Tell the head to wait for the player to come in range again.
                    head.WaitToShoot();
                    return;
                }

                // If the ball is whithin range of the bullet, home in.
                if (length <= minDistance)
                {
                    moveVector = vec;
                    moveVector.Normalize();
                }

                // Move
                pos += moveVector * 0.05f;
                bill.UpdatePos(pos);
                UpdateCol();
            }

            // Don't draw an uncharged bullet
            if (charge > 0f) bill.Draw();

            DrawCol(Editor.colorDebug2);
        }
    }
}
