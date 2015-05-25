using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CircusCharlie.Classes
{
    class EnemyStatue : Enemy
    {
        private float gravity = 0.004f;

        // States:
        private bool stunned = false;
        private bool turning = false;

        public EnemyStatue(Vector2 _pos, Sprite _spr, float _flipX, float _flipY) : base(_pos, _spr, _flipX, _flipY, Vector2.One*2.5f, Vector2.One*0.125f, new Vector2(0f, 0.5f), 0.8f)
        {
            Reset();

            AddTrig(new ColSquare(pos,
                                  new Vector2(-0.25f, -1.2f - 1.2f * flipY),
                                  new Vector2(0.5f, 2.4f)));
        }

        public override void Draw()
        {
            if (destroyed) return;

            if (stunned)
            {
                // Check for landing. Don't do it first frame.
                if (ySpeed > 0.1f &&
                    MainGame.room.CheckCol(new Vector2(pos.X, pos.Y),
                                           new Vector2(0.5f, 0.2f)) != Vector2.Zero)
                {
                    //Destroy();
                }

                ySpeed += gravity * flipY;
                pos.X += xSpeed * flipX;
                pos.Y += ySpeed;

                bill.UpdatePos(pos);
                UpdateCol();
            }
            else if (turning)
            {
                // Stop turning when the animation is done.
                if (bill.Animate() == 15)
                {
                    turning = false;
                    bill.SetAnimationSpeed(0.25f);
                    bill.SetAnim(0, 14, 12);
                    bill.Flip((flipX > 0f), (flipY == -1f));
                }
            }
            else
            {
                // Check for edges on the right.
                // Only do this every animation frame (looks better).
                if (flipX > 0f)
                {
                    // There's floor ahead, and there's no wall ahead
                    if (
                        MainGame.room.CheckCol(new Vector2(pos.X + 0.75f, pos.Y + 0.1f * flipY)) &&
                        MainGame.room.CheckCol(new Vector2(pos.X + 0.75f, pos.Y - 1.5f * flipY), new Vector2(0.2f, 2f)) == Vector2.Zero
                       )
                    {
                        pos += new Vector2(xSpeed, 0.0f);
                        bill.UpdatePos(pos);
                        UpdateCol();
                    }
                    // Turn around
                    else
                    {
                        flipX = -1f;

                        turning = true;
                        bill.SetAnimationSpeed(0.4f);
                        bill.SetAnim(15, 18);
                    }
                }
                else if (flipX < 0f)
                {
                    // There's floor ahead
                    if (
                        MainGame.room.CheckCol(new Vector2(pos.X - 0.75f, pos.Y + 0.1f * flipY)) &&
                        MainGame.room.CheckCol(new Vector2(pos.X - 0.75f, pos.Y - 1.5f * flipY), new Vector2(0.2f, 2f)) == Vector2.Zero
                       )
                    {
                        pos -= new Vector2(xSpeed, 0.0f);
                        bill.UpdatePos(pos);
                        UpdateCol();
                    }
                    // Turn around
                    else
                    {
                        flipX = 1f;

                        turning = true;
                        bill.SetAnimationSpeed(0.4f);
                        bill.SetAnim(15, 18);
                    }
                }

                bill.Animate();
            }

            DrawShadow();
            bill.Draw();

            //spr.DrawView(pos, new IntVector2D(0,0), 0, 1f, Color.White);
            DrawCol(Editor.colorDebug2);
        }

        public override void PassMsg(string msg)
        {
            // Get stunned from a block being hit.
            if (msg == "stun-above" || msg == "stun-below")
            {
                stunned = true;

                // Fly/fall into the air.
                ySpeed = -0.1f * flipY;

                // Stop animating.
                bill.SetAnimationSpeed(0);
            }
            // Fall if they somehow manage to break the block from the other side.
            else if (msg == "gone-above" || msg == "gone-below")
            {
                stunned = true;
                // Stop animating.
                bill.SetAnimationSpeed(0);
            }

            return;
        }

        public override void Reset()
        {
            base.Reset();

            // Start on a random walking frame because it looks cool.
            bill.SetAnim(0, 14, Editor.random.Next(0, 14));
            bill.SetAnimationSpeed(0.25f);

            stunned = false;
            turning = false;

            flipX = 1f;

            pos = startPos;
            xSpeed = 0.02f;
        }
    }
}
