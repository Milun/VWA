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
        private Sprite spr;
        private Cube cube;

        private Cube[] cubeFrag; // Fragments of the block which break apart.
        private Particle[] particle; // Effect which plays when the block is hit.

        Texture2D tex;

        float destroyAnimation = 0f;

        public Block(Vector2 _pos, Sprite _spr) : base()
        {
            pos = _pos;
            spr = _spr;
            AddCol(new ColSquare(new Vector2(pos.X, pos.Y), Vector2.Zero, new Vector2(2f, 1f)));

            tex = MainGame.content.Load<Texture2D>("Sprites/spr_burger");

            Reset();
        }

        public override void DrawEditor()
        {
            spr.DrawView
            (
                pos * Global.gridSize,
                new IntVector2D(48, 24),
                Color.Aquamarine
            );
        }

        public override void Draw()
        {
            // If the block is alive, draw a single cube (save memory).
            if (!destroyed)
            {
                cube.Draw();
            }
            else
            // Otherwise, show it being destroyed.
            {
                if (destroyAnimation < 4f)
                {
                    destroyAnimation += 0.1f;

                    float move = destroyAnimation*(float)Math.Cos((double)destroyAnimation / 4.2f);

                    cubeFrag[0].SetPos(new Vector3(pos.X,
                                                   pos.Y - move,
                                                   -1f + move / 4f));

                    cubeFrag[1].SetPos(new Vector3(pos.X + 1f + move / 2f,
                                                   pos.Y + 0.3f - move / 16f,
                                                   -1f + move / 8f));

                    cubeFrag[2].SetPos(new Vector3(pos.X,
                                                   pos.Y + 0.7f + move / 4f,
                                                   -1f - move / 3f));

                    cubeFrag[3].SetPos(new Vector3(pos.X - move / 2f,
                                                   pos.Y + 0.3f - move / 14f,
                                                   -1f + move / 12f));
                }

                if (destroyAnimation < 1.5f)
                {
                    //cubeFrag[0].SetAlpha(1f - destroyAnimation - 0.5f);
                    cubeFrag[0].Draw();
                }
                if (destroyAnimation < 1f)
                {
                    //cubeFrag[1].SetAlpha(1f - destroyAnimation);
                    cubeFrag[1].Draw();
                }
                if (destroyAnimation < 2.1f)
                {
                    //cubeFrag[2].SetAlpha(1f - destroyAnimation - 1.1f);
                    cubeFrag[2].Draw();
                }
                if (destroyAnimation < 4f)
                {
                    //cubeFrag[3].SetAlpha(1f - destroyAnimation - 3f);
                    cubeFrag[3].Draw();
                }

                // Particles may lag the game. Only draw if HD is on.
                if (MainGame.HD)
                { 
                    if (destroyAnimation < 4f)
                    {
                        particle[1].Draw();
                        particle[2].Draw();
                        particle[0].Draw();
                    }
                }
            }

            DrawCol(Editor.colorDebug);
        }

        public override void Reset()
        {
            base.Reset();

            destroyAnimation = 0f;

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

            cubeFrag = new Cube[4];

            cubeFrag[0] = new Cube(tex,
                                   new Vector3(pos.X, pos.Y, -1f),
                                   new Vector3(2f, 0.3f, 2f),
                                   new Vector2(0, 0.5f),
                                   new Vector2(0, 0f),
                                   new Vector2(0, 0.5f),
                                   new Vector2(0.5f, 0f),
                                   new Vector2(0, 0.5f),

                                   new Vector2(0.5f, 0.15f),
                                   Vector2.One * 0.5f,
                                   new Vector2(0.5f, 0.15f),
                                   Vector2.One * 0.5f,
                                   new Vector2(0.5f, 0.15f));

            cubeFrag[1] = new Cube(tex,
                                   new Vector3(pos.X + 1f, pos.Y + 0.3f, -1f),
                                   new Vector3(1f, 0.4f, 2f),
                                   new Vector2(0.25f, 0.65f),
                                   new Vector2(0, 0f),
                                   new Vector2(0.25f, 0.65f),
                                   new Vector2(0.5f, 0f),
                                   new Vector2(0.25f, 0.65f),

                                   new Vector2(0.25f, 0.2f),
                                   Vector2.One * 0.5f,
                                   new Vector2(0.25f, 0.2f),
                                   Vector2.One * 0.5f,
                                   new Vector2(0.25f, 0.2f));

            cubeFrag[2] = new Cube(tex,
                                   new Vector3(pos.X, pos.Y + 0.7f, -1f),
                                   new Vector3(2f, 0.3f, 2f),
                                   new Vector2(0, 0.85f),
                                   new Vector2(0, 0f),
                                   new Vector2(0, 0.85f),
                                   new Vector2(0.5f, 0f),
                                   new Vector2(0, 0.85f),

                                   new Vector2(0.5f, 0.15f),
                                   Vector2.One * 0.5f,
                                   new Vector2(0.5f, 0.15f),
                                   Vector2.One * 0.5f,
                                   new Vector2(0.5f, 0.15f));

            cubeFrag[3] = new Cube(tex,
                                   new Vector3(pos.X, pos.Y + 0.3f, -1f),
                                   new Vector3(1f, 0.4f, 2f),
                                   new Vector2(0f, 0.65f),
                                   new Vector2(0, 0f),
                                   new Vector2(0f, 0.65f),
                                   new Vector2(0.5f, 0f),
                                   new Vector2(0f, 0.65f),

                                   new Vector2(0.25f, 0.2f),
                                   Vector2.One * 0.5f,
                                   new Vector2(0.25f, 0.2f),
                                   Vector2.One * 0.5f,
                                   new Vector2(0.25f, 0.2f));
        }

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
                        MainGame.room.MsgCol(new Vector2(pos.X, pos.Y-0.2f),
                                             new Vector2(2f, 0.2f),
                                             "stun-below");

                        MainGame.room.MsgCol(new Vector2(pos.X, pos.Y+1f),
                                             new Vector2(2f, 0.2f),
                                             "gone-above");
                    }
                    else if (collision.Y < 0f)
                    {
                        MainGame.room.MsgCol(new Vector2(pos.X, pos.Y+1f),
                                             new Vector2(2f, 0.2f),
                                             "stun-above");

                        MainGame.room.MsgCol(new Vector2(pos.X, pos.Y-0.2f),
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
