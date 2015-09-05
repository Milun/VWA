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
        private Quad reel1, reel2, back, sticker;
        private float reel1Rot, reel2Rot;

        protected Texture2D tex2;

        int health = 0;

        public BlockTape(Vector3 _pos, Sprite _spr, Sprite _spr2, int _health = 0) : base(_pos, _spr)
        {
            reel1Rot = (float)Editor.random.Next(360);
            reel2Rot = (float)Editor.random.Next(360);

            tex2 = _spr2.GetTexture();
            health = _health;
            Reset();
        }

        public override void Reset()
        {
            float off = ((float)health) * 0.25f;

            cube = new Cube(tex,
                            new Vector3(pos.X, pos.Y, -0.25f),
                            new Vector3(2f, 1f, 0.5f),

                            new Vector2(0, 35f / 512f + off),

                            new Vector2(171f / 256f, 0f + off),
                            new Vector2(171f / 256f, 34 / 512f + off),
                            new Vector2(0f, 0f + off),
                            new Vector2(171f / 256f, 34 / 512f + off),

                            new Vector2(171f / 256f, 93f / 512f),

                            new Vector2(85f / 256f, 34f / 512f),
                            new Vector2(34f / 256f, 94f / 512f),
                            new Vector2(171f / 256f, 34f / 512f),
                            new Vector2(34f / 256f, 94f / 512f));

            reel1 = new Quad(
                                new Vector3(pos.X + 0.5f, pos.Y + 0.5f, -0.2f),
                                Vector3.Backward,
                                Vector3.Down,
                                0.9f,
                                0.9f,
                                new Vector2(205f / 256f, 34f / 512f),
                                new Vector2(51f / 256f, 51f / 512f),
                                tex
                            );
            reel2 = new Quad(
                                new Vector3(pos.X + 1.5f, pos.Y + 0.5f, -0.2f),
                                Vector3.Backward,
                                Vector3.Down,
                                0.9f,
                                0.9f,
                                new Vector2(205f / 256f, 34f / 512f),
                                new Vector2(51f / 256f, 51f / 512f),
                                tex
                            );

            back = new Quad(
                                new Vector3(pos.X + 1f, pos.Y + 0.5f, 0.25f),
                                Vector3.Backward,
                                Vector3.Down,
                                2f,
                                1f,
                                new Vector2(205f / 256f, 85f / 512f + off),
                                new Vector2(51f / 256f, 43f / 512f),
                                tex
                            );

            sticker = new Quad(
                                new Vector3(pos.X + 1f, pos.Y + 0.535f, -0.26f),
                                Vector3.Backward,
                                Vector3.Down,
                                (71f * 2f) / 171f,
                                43f / 94f,
                                new Vector2(Editor.random.Next(2) * 0.5f, Editor.random.Next(4) * 0.25f),
                                new Vector2(0.5f, 0.25f),
                                tex2
                            );
        }

        public override float GetValue(string name)
        {
            if (name == "health")
            {
                return health;
            }

            return 0f;
        }

        public override void Draw()
        {
            if (!IsAlive()) return;

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

            Game1.AddQuad(ref sticker);

            DrawCol(Editor.colorDebug);
        }

        public override void DrawEditor()
        {
            int off = health * 128;

            spr.DrawView
            (
                new Vector2(pos.X, pos.Y) * Global.gridSize,
                new Vector2(48, 24),
                new Vector2(0, 35+off),
                new Vector2(171,  93),
                Color.White
            );
        }
    }
}
