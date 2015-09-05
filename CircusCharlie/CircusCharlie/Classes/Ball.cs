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
    class Ball : Actor
    {
        private Texture2D tex;

        private const float BALLSIZE = 0.5f;
        private float YSPEED = 1 / 30f;
        private const float XSPEED = 1 / 16f;
        private const float YSPEEDADDSPEED = 0.1f;

        private float xSpeed = 0f;
        private float ySpeed = 1 / 27f;

        private float boostSpeed;

        private float yDirection = 1f;

        private float       fuel;
        private bool        fuelCharging = false;
        private float       fuelDrainSpeed  = 0.015f;
        private float       fuelChargeDelay = 0f;
        private const float FUELCHARGEDELAY = 5f;
        private bool        fuelReset = false;

        private float rotX = 0f;
        private float rotY = 0f;

        public Model model;

        private Vector3 startPos;

        public float Fuel
        {
            get
            {
                return fuel;
            }
        }

        public Ball(Texture2D _tex)
            : base()
        {
            tex = _tex;
            startPos = pos;

            fuel = 1.0f;
            boostSpeed = YSPEED * 4f;

            AddTrig(new ColCircle(new Vector2(pos.X, pos.Y),
                                  new Vector2(0, 0),
                                  BALLSIZE));

            model = MainGame.content.Load<Model>("Sprites/cube");


        }

        public float GetYspeed()
        {
            return ySpeed;
        }

        public Vector3 GetPos()
        {
            return pos;
        }

        public bool isFuelCharging()
        {
            return fuelCharging;
        }

        public void SetPos(IntVector2D _pos)
        {
            pos = new Vector3(_pos.X, _pos.Y, 0f);
            UpdateCol(new Vector2(pos.X, pos.Y));
            startPos = pos;
        }

        private void Boost()
        {
            ////////////
            // YSPEED //
            ////////////
            if (Keyboard.GetState().IsKeyUp(Keys.LeftShift) || Keyboard.GetState().IsKeyUp(Keys.LeftControl))
            {
                if (fuelReset)
                {
                    fuelReset = false;
                    fuelChargeDelay = FUELCHARGEDELAY;
                }
            }
            if (!fuelCharging && Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                fuelReset = true;

                // Slow it down.
                ySpeed *= 0.965f;

                // Drain fuel (normal speed)
                if (fuel < 0f)
                {
                    fuel = 0f;
                    fuelChargeDelay = FUELCHARGEDELAY;
                    fuelCharging = true;
                }
                else
                {
                    fuel -= fuelDrainSpeed;
                }
            }
            else if (!fuelCharging && Keyboard.GetState().IsKeyDown(Keys.LeftControl))
            {
                fuelReset = true;

                // Accelerate towards max speed.
                float temp = boostSpeed - Math.Abs(ySpeed);
                ySpeed += temp*0.5f*yDirection;

                // Drain fuel (double speed)
                if (fuel < 0f)
                {
                    fuel = 0f;
                    fuelChargeDelay = FUELCHARGEDELAY;
                    fuelCharging = true;
                }
                else
                {
                    fuel -= fuelDrainSpeed*2f;
                }
            }
            else
            {
                // Accelerate towards normal.
                if (Math.Abs(ySpeed) != YSPEED && Math.Abs(Math.Abs(ySpeed) - YSPEED) > 0.0001f)
                {
                    float temp = YSPEED - Math.Abs(ySpeed);
                    ySpeed += temp * 0.5f * yDirection;
                }
                else
                {
                    ySpeed = YSPEED * yDirection;
                }

                // Wait until the charge is drained.
                if (fuelChargeDelay > 0f) fuelChargeDelay -= 0.1f;
                else
                {
                    if (fuel < 1f)
                    {
                        fuel += fuelDrainSpeed;
                    }
                    else if (fuelCharging || fuel > 1f)
                    {
                        fuel = 1.0f;
                        fuelCharging = false;
                    }
                }
            }
        }

        private void Move()
        {
            // Check collisions
            Vector2 collision = MainGame.room.CheckCol(this);

            if (Keyboard.GetState().IsKeyDown(Keys.Space)) return;

            // Move first
            if (pos.X > BALLSIZE / 3f &&
                collision.X >= 0f &&
                Keyboard.GetState().IsKeyDown(Keys.A))
            {
                if (xSpeed > -XSPEED)
                {
                    xSpeed -= 0.01f;
                }
            }
            else if (pos.X < MainGame.room.mapWidth - BALLSIZE / 3f &&
                     collision.X <= 0f &&
                     Keyboard.GetState().IsKeyDown(Keys.D))
            {
                if (xSpeed < XSPEED)
                {
                    xSpeed += 0.01f;
                }
            }
            else
            {
                // Softly push the ball out of the wall.
                // Important to leave a bit of the wall still inside to prevent the ball from jittering.
                // Crud we still get the clip bounce...
                if (Math.Abs(collision.X) >= 0.01f)
                {
                    pos.X -= (collision.X / Global.gridSize) / 100f;
                }
                xSpeed = 0f;
            }

            pos += new Vector3(xSpeed, 0, 0);

            Boost();

            // Collision below
            if (collision.Y > 0f) { yDirection = -1f; ySpeed = Math.Abs(ySpeed) * yDirection; }
            // Collision above
            else if (collision.Y < 0f) { yDirection = 1f; ySpeed = Math.Abs(ySpeed) * yDirection; }

            pos += new Vector3(0, ySpeed, 0);

            if (Math.Abs(rotX) < 0.5f && xSpeed != 0f)
            {
                if (xSpeed > 0f)
                {
                    if (ySpeed > 0f) rotX -= 0.1f;
                    else rotX += 0.1f;
                }
                else if (xSpeed < 0f)
                {
                    if (ySpeed > 0f) rotX += 0.1f;
                    else rotX -= 0.1f;
                }
            }
            else if (Math.Abs(xSpeed) < 0.1f)
            {
                rotX *= 0.85f;
            }

            rotY += ySpeed;

            UpdateCol(new Vector2(pos.X, pos.Y));
        }

        public override void Draw()
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = Matrix.CreateRotationZ(rotX);
                    effect.World *= Matrix.CreateRotationX(rotY);
                    effect.World *= Matrix.CreateTranslation(new Vector3(pos.X,
                                                                        pos.Y,
                                                                        0f));

                    effect.View = MainGame.matrixView;
                    effect.Projection = MainGame.matrixProj;

                    effect.TextureEnabled = true;
                    effect.Texture = tex;


                    effect.EnableDefaultLighting();
                    //effect.LightingEnabled = true; // turn on the lighting subsystem.
                    effect.AmbientLightColor = new Vector3(0.2f, 0.2f, 0.2f);
                    effect.EmissiveColor = new Vector3(1, 0, 0);
                }

                mesh.Draw();
            }


            DrawCol(Editor.colorDebug2);

            // Check for trigger collisions with the ball.
            MainGame.room.CheckTrig(this);
            Move();

            UpdateCol();
        }

        public override float GetValue(string name)
        {
            if (name == "yspeed") return ySpeed;

            return 0f;
        }

        public override void Reset()
        {
            base.Reset();

            pos = startPos;
        }

        // Prevent the ball hitting it's own Head.
        protected override void ActorCol(Actor other, Vector2 collision)
        {
            if (other.GetType() == typeof(EnemyStatue))
            {
                Reset();
            }
        }

        public override void PassMsg(string msg)
        {
            // React to an instant kill
            if (msg == "dmgMax")
            {
                Reset();
            }

            return;
        }
    }
}
