using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CircusCharlie.Classes
{
    class BlockTape : Block
    {
        private Quad reel1, reel2, back;
        private float reel1Rot, reel2Rot;

        public BlockTape(Vector2 _pos, Sprite _spr) : base(_pos, _spr)
        {
            cube = new Cube(_spr.GetTexture(),
                            new Vector3(pos.X, pos.Y, -0.25f),
                            new Vector3(2f, 1f, 0.5f),

                            new Vector2(0, 35f/128f),

                            new Vector2(171f/256f, 0f),
                            new Vector2(171f/256f, 34/128f),
                            new Vector2(0f, 0f),
                            new Vector2(171f/256f, 34/128f),

                            new Vector2(171f/256f, 93f/128f),

                            new Vector2(85f/256f, 34f/128f),
                            new Vector2(34f/256f, 94f/128f),
                            new Vector2(171f/256f, 34f/128f),
                            new Vector2(34f/256f, 94f/128f));

            reel1 = new Quad(
                                new Vector3(pos.X+0.5f, pos.Y+0.5f, -0.2f),
                                Vector3.Backward,
                                Vector3.Down,
                                0.9f,
                                0.9f,
                                new Vector2(205f/256f, 34f/128f),
                                new Vector2(51f/256f, 51f/128f),
                                _spr.GetTexture()
                            );
            reel2 = new Quad(
                                new Vector3(pos.X+1.5f, pos.Y+0.5f, -0.2f),
                                Vector3.Backward,
                                Vector3.Down,
                                0.9f,
                                0.9f,
                                new Vector2(205f / 256f, 34f / 128f),
                                new Vector2(51f / 256f, 51f / 128f),
                                _spr.GetTexture()
                            );

            back = new Quad(
                                new Vector3(pos.X + 1f, pos.Y + 0.5f, 0.25f),
                                Vector3.Backward,
                                Vector3.Down,
                                2f,
                                1f,
                                new Vector2(205f/256f, 85f/128f),
                                new Vector2(51f/256f, 43f/128f),
                                _spr.GetTexture()
                            );

            reel1Rot = (float)Editor.random.Next(360);
            reel2Rot = (float)Editor.random.Next(360);
        }

        public override void Draw()
        {
            reel1Rot += 0.5f;
            reel2Rot += 0.5f;

            if (reel1Rot >= 360f) reel1Rot -= 360f;
            if (reel2Rot >= 360f) reel2Rot -= 360f;

            reel1.RotateZ(reel1Rot);
            reel2.RotateZ(reel2Rot);

            Game1.AddQuad(ref back);

            cube.DrawSides();

            //Game1.AddQuad(ref reel2);

            Game1.AddQuad(ref reel1);
            Game1.AddQuad(ref reel2);

            cube.DrawTop();
        }

    }
}
