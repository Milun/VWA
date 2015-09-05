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
    class Block : Actor
    {
        protected Sprite spr;
        protected Cube cube;

        protected Texture2D tex;

        public Block(Vector3 _pos, Sprite _spr)
            : base()
        {
            pos = _pos;
            spr = _spr;
            tex = spr.GetTexture();

            AddCol(new ColSquare(new Vector2(pos.X, pos.Y), Vector2.Zero, new Vector2(2f, 1f)));
        }

        public override void DrawEditor()
        {
            spr.DrawView
            (
                new Vector2(pos.X, pos.Y) * Global.gridSize,
                new Vector2(48, 24),
                new Vector2(0f, 0.5f),
                Vector2.One * 0.5f,
                Color.White
            );
        }

        public override void Draw()
        {
            // If the block is alive, draw a single cube (save memory).
            if (IsAlive())
            {
                if (cube != null) cube.Draw();
                DrawCol(Editor.colorDebug);
            }


            // Particles may lag the game. Only draw if HD is on.
            /*if (MainGame.HD)
            { 
                if (destroyAnimation < 4f)
                {
                    particle[1].Draw();
                    particle[2].Draw();
                    particle[0].Draw();
                }
            }*/

        }

        /*public override void Reset()
        {
            base.Reset();
        */
            /*destroyAnimation = 0f;

            particle = new Particle[3];

            particle[0] = new Particle(new Vector2(pos.X + 0.9f, pos.Y + 0.45f),
                                       -0.8f,
                                       tex,
                                       Vector2.One * 2.3f,
                                       10,
                                       Vector2.Zero,
                                       Vector2.One * 0.008f,
                                       (float)(Editor.random.Next(0, 10)) - 5f,
                                       4f);

            particle[1] = new Particle(new Vector2(pos.X + 0.6f, pos.Y + 0.7f),
                                       -0.3f,
                                       tex,
                                       Vector2.One * 1.3f,
                                       14,
                                       Vector2.Zero,
                                       new Vector2(-0.025f, 0.01f),
                                       (float)(Editor.random.Next(0, 10)) - 5f,
                                       4f);

            particle[2] = new Particle(new Vector2(pos.X + 1.45f, pos.Y + 0.55f),
                                       -0.5f,
                                       tex,
                                       Vector2.One * 1.7f,
                                       15,
                                       Vector2.Zero,
                                       new Vector2(0.02f, 0.005f),
                                       (float)(Editor.random.Next(0, 10)) - 5f,
                                       4f);
            */
           /* 
            cube = new Cube(tex,
                            new Vector3(pos.X, pos.Y, -0.25f),
                            new Vector3(2f, 1f, 0.5f),
                            new Vector2(0, 0.5f),
                            new Vector2(0, 0f),
                            new Vector2(0, 0.5f),
                            new Vector2(0.5f, 0f),
                            new Vector2(0, 0.5f),

                            Vector2.One * 0.5f,
                            Vector2.One * 0.5f,
                            Vector2.One * 0.5f,
                            Vector2.One * 0.5f,
                            Vector2.One * 0.5f);

            //cubeFrag = new Cube[4];

        }*/

        protected override void ActorCol(Actor other, Vector2 collision)
        {
            // If it collides with the ball, destroy the block!
            if (other.GetType() == typeof(Ball))
            {
                if ((collision.Y > 0f && other.GetValue("yspeed") < 0f) ||
                    (collision.Y < 0f && other.GetValue("yspeed") > 0f))
                {
                    // If hit from below, stun things above.
                    if (collision.Y > 0f)
                    {
                        MainGame.room.MsgCol(new Vector2(pos.X, pos.Y - 0.2f),
                                             new Vector2(2f, 0.2f),
                                             "stun-below");

                        MainGame.room.MsgCol(new Vector2(pos.X, pos.Y + 1f),
                                             new Vector2(2f, 0.2f),
                                             "gone-above");
                    }
                    else if (collision.Y < 0f)
                    {
                        MainGame.room.MsgCol(new Vector2(pos.X, pos.Y + 1f),
                                             new Vector2(2f, 0.2f),
                                             "stun-above");

                        MainGame.room.MsgCol(new Vector2(pos.X, pos.Y - 0.2f),
                                             new Vector2(2f, 0.2f),
                                             "gone-below");
                    }

                    Destroy();
                    return;
                }
            }
        }

    }
}
