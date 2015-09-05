using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CircusCharlie.Classes
{
    class BlockBurger : Block
    {
        List<Particle> particles;

        private Model   meatModel;
        private float   meatRot     = 0f;
        private float   meatRotAmm  = 0f;
        private Vector3 meatSpeed   = Vector3.Zero;
        private Vector3 meatPos     = Vector3.Zero;
        private float meatAlpha     = 10f;

        public BlockBurger(Vector3 _pos, Sprite _spr)
            : base(_pos, _spr)
        {
            Reset();

            meatModel = MainGame.content.Load<Model>("Sprites/burgerMeat");
        }

        public override void Draw()
        {
            base.Draw();

            if (destroyed)
            {
                foreach(Particle e in particles)
                {
                    DrawMeat();
                    e.Draw();                    
                }
            }
        }

        public override void Reset()
        {
            particles = new List<Particle>();

            /*sauce1 = new Particle(pos + new Vector2(1f, 0.5f),
                                  0.4f,
                                  tex,
                                  Vector2.One * 4f,
                                  new Vector2(0.5f, 0f),
                                  new Vector2(0.25f, 0.5f),
                                  Vector2.Zero,
                                  0f,
                                  10f);
            */

            Particle temp = new Particle(tex,
                                  pos + new Vector3(1f, -1f, 0.4f),
                                  Vector2.One * 3f,
                                  new Vector2(0f, 0.5f),
                                  new Vector2(0.5f, 0f),
                                  new Vector2(0.25f, 0.5f),
                                  38f);

            float scale = (float)(Editor.random.Next(25, 40))/10.0f;

            temp.addKeyRot(0f, Editor.random.Next(-20, 20));

            temp.addKeyVel  (0f, new Vector3(0f, 0f, -1.2f));
            temp.addKeyScale(0f, Vector2.One * scale * 0.2f);

            //temp.addKeyScale(0.3f, Vector2.One * scale);

            temp.addKeyScale(1.5f, Vector2.One * scale * 0.8f);
            temp.addKeyVel  (1.5f, Vector3.Zero);

            //temp.addKeyScale(1.6f, Vector2.One * scale * 0.8f);



            temp.addKeyVel(12f, Vector3.Zero);
            temp.addKeyScale(12f, Vector2.One * scale * 0.8f);
            temp.addKeyAlpha(10f, 1.0f);

            temp.addKeyVel(38f,     new Vector3(0f, 0.15f, 0));
            temp.addKeyScale(38f,   new Vector2(scale * 0.5f, scale * 2.25f));
            temp.addKeyAlpha(38f,   0f);

            particles.Add(temp);

            particles.Add(CreateContent(Vector2.One * 0.75f, Vector2.One * 0.75f));
            particles.Add(CreateContent(Vector2.One * 0.75f, Vector2.One * 0.75f));
            //particles.Add(CreateContent(Vector2.One * 0.75f, Vector2.One * 0.75f));
            particles.Add(CreateContent(Vector2.One*1.5f, new Vector2(0.75f, 0.25f)));
            //particles.Add(CreateContent(Vector2.One*1.5f, new Vector2(0.75f, 0.25f)));
            //particles.Add(CreateContent(Vector2.One*2f, new Vector2(0.75f, 0.5f)));

            cube = new Cube(tex,
                            new Vector3(pos.X, pos.Y, -0.25f),
                            new Vector3(2f, 1f, 0.5f),

                            new Vector2(0, 0.25f),
                            new Vector2(0, 0f),
                            new Vector2(0.125f, 0.25f),
                            new Vector2(0.0f, 0.5f),
                            new Vector2(0f, 0.25f),

                            new Vector2(0.25f, 0.25f),
                            new Vector2(0.25f, 0.25f),
                            new Vector2(-0.125f, 0.25f),
                            new Vector2(0.25f, 0.25f),
                            new Vector2(-0.125f, 0.25f));
        }

        private Particle CreateContent(Vector2 size, Vector2 offPos)
        {
            Particle pickle;

            float velX = Global.Random(-1f, 1f);
            float velY = Global.Random(-0.06f, -0.02f);
            float velZ = Global.Random(-0.1f, 0.3f);

            pickle = new Particle(tex,
                                  pos + new Vector3(1f + velX, 0.5f + Global.Random(-0.8f, 0.8f), Global.Random(-0.3f, 0.3f)),
                                  size,
                                  Vector2.Zero,
                                  offPos,
                                  new Vector2(0.125f, 0.25f),
                                  20f);
            
            pickle.setRot2(Global.Random(-2.0f, 2.0f));

            pickle.addKeyVel(0f, new Vector3(velX / 50f, velY, velZ));

            /*pickle.addKeyUV(1f, new Vector2(0.875f, 0.25f));
            pickle.addKeyUV(2f, new Vector2(0.75f, 0.25f));
            pickle.addKeyUV(3f, new Vector2(0.875f, 0.25f));
            */

            pickle.addKeyAlpha(16f, 1f);
            pickle.addKeyAlpha(20f, 0f);

            pickle.addKeyVel(20f, new Vector3(velX / 50f, velY+0.15f, velZ));

            pickle.addKeyRot(40f, 360f * Global.Random(-1f, 1f));

            return pickle;
        }

        private void DrawMeat()
        {
            meatRot     += 0.003f;
            meatSpeed   -= Vector3.Down*0.00002f;
            meatPos     += meatSpeed;

            foreach (ModelMesh mesh in meatModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = Matrix.CreateRotationZ(meatRot * meatRotAmm);
                    effect.World *= Matrix.CreateRotationX(meatRot);
                    effect.World *= Matrix.CreateTranslation(meatPos);

                    effect.View = MainGame.matrixView;
                    effect.Projection = MainGame.matrixProj;

                    effect.TextureEnabled = true;
                    effect.Texture = tex;

                    effect.FogEnabled = true;
                    effect.FogColor = Vector3.Zero;
                    effect.FogStart = 20f;
                    effect.FogEnd = 50f;

                    effect.EnableDefaultLighting();
                    //effect.LightingEnabled = true; // turn on the lighting subsystem.
                    effect.AmbientLightColor = new Vector3(0.2f, 0.2f, 0.2f);
                    effect.EmissiveColor = new Vector3(1, 0, 0);
                }

                mesh.Draw();
            }
        }





        public override void DrawEditor()
        {
            spr.DrawView
            (
                new Vector2(pos.X, pos.Y) * Global.gridSize,
                new Vector2(48, 24),
                new Vector2(0, 35),
                new Vector2(171, 93),
                Color.White
            );
        }

        public override void Destroy()
        {
            // Set the meat particle effects stats
            if (IsAlive())
            {
                meatSpeed = new Vector3(Global.Random(-0.012f,0.012f),
                                        Global.Random(-0.02f,-0.01f),
                                        Global.Random(-0.015f, 0.05f));

                meatPos = pos + new Vector3(1.0f, 0.5f, -1.0f);
                meatRotAmm = Global.Random(0.2f, 1.5f);
            }

            base.Destroy();
        }
    }
}
