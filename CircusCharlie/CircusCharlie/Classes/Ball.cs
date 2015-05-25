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
        private Sprite spr;

        private const float BALLSIZE = 0.5f;
        private float YSPEED = 1/27f;
        private const float XSPEED = 1/16f;

        private float xSpeed = 0f;
        private float ySpeed = 1/27f;
        private float ySpeedAdd = 0f;

        public Model model;

        private Vector2 startPos;

        public Ball(Sprite _spr) : base()
        {
            spr = _spr;
            startPos = pos;

            AddTrig(new ColCircle(pos,
                                  new Vector2(0, 0),
                                  BALLSIZE));

            model = MainGame.content.Load<Model>("Sprites/cube");
        }

        public float GetYspeed()
        {
            return ySpeed;
        }

        public Vector2 GetPos()
        {
            return pos;
        }

        public void SetPos(IntVector2D _pos)
        {
            pos = new Vector2(_pos.X, _pos.Y);
            UpdateCol(pos);
            startPos = pos;
        }



        private void Move()
        {
            // Check collisions
            Vector2 collision = MainGame.room.CheckCol(this);

            if (Keyboard.GetState().IsKeyDown(Keys.Space)) return;

            // Move first
            if (pos.X > BALLSIZE/3f && 
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
                    pos.X -= (collision.X/Global.gridSize)/100f;
                }
                xSpeed = 0f;
            }

            pos += new Vector2(xSpeed, 0);

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                ySpeedAdd = -YSPEED / 2f;
            }
            else
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                ySpeedAdd = YSPEED / 2f;
            }
            else
            {
                ySpeedAdd = 0f;
            }

            // Collision below
            if (collision.Y > 0f) ySpeed = -YSPEED;
            // Collision above
            else if (collision.Y < 0f) ySpeed = YSPEED;

            pos += new Vector2(0, ySpeed+ySpeedAdd);

            UpdateCol(pos);
        }

        public override void Draw()
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = Matrix.CreateTranslation(new Vector3(pos.X,
                                                                        pos.Y,
                                                                        0f));
                    effect.View = MainGame.matrixView;
                    effect.Projection = MainGame.matrixProj;

                    //effect.TextureEnabled = true;
                    //effect.Texture = spr.GetTexture();
                    

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
