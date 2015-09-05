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
        EnemyHead head;           // Reference to the head which shot ya.

        float minDistance = 5.0f;
        Vector2 moveVector = Vector2.Zero;

        float diam = 0.8f;

        bool explode = false;

        public EnemyBullet(Vector3 _pos, Sprite _spr, EnemyHead _head)
            : base(_pos, _spr, 1f, 1f, Vector2.One*0.8f, Vector2.One * 0.25f, Vector2.Zero, 0.0f)
        {
            head = _head;

            Die();
            explode = false;

            AddTrig(new ColCircle(new Vector2(pos.X, pos.Y),
                                  Vector2.Zero,
                                  diam * 0.7f));
        }

        public override void Reset()
        {
            base.Reset();

            explode = false;

            pos = startPos;
            moveVector = Vector2.Zero;

            bill.UpdatePos(pos);
            UpdateCol();

            // Set some time before the bullet respawns.
            bill.UpdateVerts(0f, 0f);

            bill.SetAnim(0, 8);
            bill.SetAnimationSpeed(0.3f);

            UnDestroy();
        }

        private void Die()
        {
            explode = true;
            bill.SetAnim(9, 16, 9);
            Destroy();
        }

        public void ShootBullet(Vector3 newStartPos)
        {
            startPos = newStartPos;

            Reset();

            // Get vector to ball.
            Vector2 ball = new Vector2(MainGame.ball.GetPos().X, MainGame.ball.GetPos().Y);
            Vector2 vec = new Vector2(ball.X - pos.X, ball.Y - pos.Y);
            vec.Normalize();

            moveVector = vec * 0.05f;
        }

        public override void Draw()
        {
            if (explode)
            {
                if (bill.Animate() != 16)
                {
                    bill.Draw();
                    return;
                }

                // You collided. Make the head wait to shoot again.
                Destroy();
                explode = false;
                head.WaitToShoot();

                return;
            }
            else
            {
                // Check OOB
                if (pos.X > MainGame.room.mapWidth+diam ||
                    pos.X < -diam ||
                    pos.Y > MainGame.room.mapHeight+diam ||
                    pos.Y < -diam)
                {
                    Destroy();
                }

                if (destroyed)
                {
                    return;
                }
            }

            // Check if the bullet has hit something.
            // Get vector to ball.
            Vector3 ball = MainGame.ball.GetPos();
            Vector2 vec = new Vector2(ball.X - pos.X, ball.Y - pos.Y);

            // First the player
            if (vec.Length() < diam / 2f)
            {
                MainGame.ball.Reset();
                Die();

                return;
            }

            // Second, a wall
            Vector2 collision = MainGame.room.CheckCol(this);

            // Move
            pos += new Vector3(moveVector.X, moveVector.Y, 0f);

            bill.UpdatePos(pos);
            bill.Animate();
            bill.Draw();
            
            UpdateCol();

            DrawCol(Editor.colorDebug2);
        }

        // Prevent the ball hitting it's own Head.
        protected override void ActorCol(Actor other, Vector2 collision)
        {
            // Ignore what made you.
            if (other == head)
            {
                return;
            }
            else
            {
                Die();
            }
        }

        protected override void ActorColGeneric(Vector2 collision)
        {
            Die();
        }
    }
}
