using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CircusCharlie.Classes
{
    class Cube
    {
        private Texture2D   tex;
        //private Vector2     uvSize = Vector2.Zero; // 0-1
        //private Vector2     uvOff = Vector2.Zero;  // Multiplier

        private Vector3 size = Vector3.One;
        private Vector3 pos = Vector3.Zero;

        // Neighbours
        private bool nTop       = false;
        private bool nBottom    = false;
        private bool nLeft      = false;
        private bool nRight     = false;

        Quad [] quads;

        public Cube(Texture2D _tex,
                    Vector3 _pos,
                    Vector3 _size,
                    Vector2 uvOff,
                    Vector2 uvSize)
        {
            tex = _tex;
            pos = _pos;
            size = _size;

            //uvOff = _uvOff;
            //uvSize = _uvSize;

            quads = new Quad[5];

            // 0 - Main
            // 1 - Top
            // 2 - Right
            // 3 - Bottom
            // 4 - Left

            quads[0] = new Classes.Quad
                       (
                            new Vector3(pos.X + size.X / 2f, pos.Y + size.Y / 2f, pos.Z),
                            Vector3.Backward,
                            Vector3.Down,
                            size.X,
                            size.Y,
                            uvOff * uvSize,
                            uvSize,
                            tex
                       );

            quads[1] = new Classes.Quad
                       (
                            new Vector3(pos.X + size.X / 2f, pos.Y, pos.Z + size.Z / 2f),
                            Vector3.Up,
                            Vector3.Forward,
                            size.X,
                            size.Z,
                            uvOff * uvSize,
                            uvSize,
                            tex
                       );

            quads[2] = new Classes.Quad
                       (
                            new Vector3(pos.X + size.X, pos.Y + size.Y / 2f, pos.Z + size.Z / 2f),
                            Vector3.Left,
                            Vector3.Down,
                            size.Z,
                            size.Y,
                            uvOff * uvSize,
                            uvSize,
                            tex
                       );

            quads[3] = new Classes.Quad
                       (
                            new Vector3(pos.X + size.X / 2f, pos.Y + size.Y, pos.Z + size.Z / 2f),
                            Vector3.Down,
                            Vector3.Forward,
                            size.X,
                            size.Z,
                            uvOff * uvSize,
                            uvSize,
                            tex
                       );

            quads[4] = new Classes.Quad
                       (
                            new Vector3(pos.X, pos.Y + size.Y / 2f, pos.Z + size.Z / 2f),
                            Vector3.Right,
                            Vector3.Down,
                            size.Z,
                            size.Y,
                            uvOff * uvSize,
                            uvSize,
                            tex
                       );
        }

        public Cube(Texture2D _tex,
                    Vector3 _pos,
                    Vector3 _size,
                    Vector2 uvOff0,
                    Vector2 uvOff1,
                    Vector2 uvOff2,
                    Vector2 uvOff3,
                    Vector2 uvOff4,
                    Vector2 uvSize0,
                    Vector2 uvSize1,
                    Vector2 uvSize2,
                    Vector2 uvSize3,
                    Vector2 uvSize4)
        {
            tex = _tex;
            pos = _pos;
            size = _size;

            //uvOff = _uvOff;
            //uvSize = _uvSize;

            quads = new Quad[5];

            // 0 - Main
            // 1 - Top
            // 2 - Right
            // 3 - Bottom
            // 4 - Left

            quads[0] = new Classes.Quad
                       (
                            new Vector3(pos.X+size.X/2f, pos.Y+size.Y/2f, pos.Z),
                            Vector3.Backward,
                            Vector3.Down,
                            size.X,
                            size.Y,
                            uvOff0,
                            uvSize0,
                            tex
                       );

            quads[1] = new Classes.Quad
                       (
                            new Vector3(pos.X+size.X/2f, pos.Y, pos.Z + size.Z / 2f),
                            Vector3.Up,
                            Vector3.Forward,
                            size.X,
                            size.Z,
                            uvOff1,
                            uvSize1,
                            tex
                       );

            quads[2] = new Classes.Quad
                       (
                            new Vector3(pos.X + size.X, pos.Y+size.Y/2f, pos.Z + size.Z / 2f),
                            Vector3.Left,
                            Vector3.Down,
                            size.Z,
                            size.Y,
                            uvOff2,
                            uvSize2,
                            tex
                       );

            quads[3] = new Classes.Quad
                       (
                            new Vector3(pos.X+size.X/2f, pos.Y+size.Y, pos.Z + size.Z / 2f),
                            Vector3.Down,
                            Vector3.Forward,
                            size.X,
                            size.Z,
                            uvOff3,
                            uvSize3,
                            tex
                       );

            quads[4] = new Classes.Quad
                       (
                            new Vector3(pos.X, pos.Y+size.Y/2f, pos.Z + size.Z / 2f),
                            Vector3.Right,
                            Vector3.Down,
                            size.Z,
                            size.Y,
                            uvOff4,
                            uvSize4,
                            tex
                       );
        }

        public void SetPos(Vector3 _pos)
        {
            pos = _pos;

            quads[0].SetPos(new Vector3(pos.X + size.X / 2f, pos.Y + size.Y / 2f, pos.Z));
            quads[1].SetPos(new Vector3(pos.X + size.X / 2f, pos.Y, pos.Z + size.Z / 2f));
            quads[2].SetPos(new Vector3(pos.X + size.X, pos.Y + size.Y / 2f, pos.Z + size.Z / 2f));
            quads[3].SetPos(new Vector3(pos.X + size.X / 2f, pos.Y + size.Y, pos.Z + size.Z / 2f));
            quads[4].SetPos(new Vector3(pos.X, pos.Y + size.Y / 2f, pos.Z + size.Z / 2f));
        }

        public void SetAlpha(float a)
        {
            for (int i = 0; i < quads.Length; i++)
            {
                quads[i].Alpha = a;
            }
        }

        /*public void SetSize(Vector3 _size)
        {
            size = _size;

            quads[0].SetVerts(size.X, size.Y);
            quads[1].SetVerts(size.X, size.Y);
            quads[2].SetVerts(size.X, size.Y);
            quads[3].SetVerts(size.X, size.Y);
            quads[4].SetVerts(size.X, size.Y);
        }*/

        public void SetNeighbours(bool t, bool r, bool b, bool l)
        {
            nTop = t;
            nRight = r;
            nBottom = b;
            nLeft = l;
        }

        public void Draw()
        {
            Game1.AddQuad(ref quads[0]);

            if (!MainGame.HD) return;

            if (!nTop)    Game1.AddQuad(ref quads[1]);
            if (!nRight)  Game1.AddQuad(ref quads[2]);
            if (!nBottom) Game1.AddQuad(ref quads[3]);
            if (!nLeft)   Game1.AddQuad(ref quads[4]);
        }

        public void DrawTop()
        {
            Game1.AddQuad(ref quads[0]);
        }

        public void DrawSides()
        {
            if (!MainGame.HD) return;

            if (!nTop) Game1.AddQuad(ref quads[1]);
            if (!nRight) Game1.AddQuad(ref quads[2]);
            if (!nBottom) Game1.AddQuad(ref quads[3]);
            if (!nLeft) Game1.AddQuad(ref quads[4]);
        }
    }
}
