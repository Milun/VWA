using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CircusCharlie.Classes
{
    class Billboard
    {
        private Texture2D tex;

        private Vector2 size;
        private Vector3 pos;
        private Vector3 origin;

        private Vector2 sizeUV;

        private Quad quad;

        private int start = 0;
        private int end = 0;
        private int frame = 0;

        float animTimer = 0f;
        float animSpeed = 0.05f;

        private IntVector2D tileMap;


        public Billboard(Texture2D _tex,
                         Vector3 _pos,
                         Vector2 _origin,
                         Vector2 _size,
                         Vector2 _uvOff,
                         Vector2 _uvSize)
        {
            tex = _tex;
            pos = _pos;
            size = _size;

            sizeUV = _uvSize;

            tileMap = new IntVector2D((int)Math.Floor(1f / _uvSize.X),
                                      (int)Math.Floor(1f / _uvSize.Y));

            origin = new Vector3(size.X * _origin.X, size.Y * _origin.Y, 0.0f);

            quad = new Classes.Quad
                       (
                            pos + origin,
                            Vector3.Backward,
                            Vector3.Down,
                            size.X,
                            size.Y,
                            _uvOff,
                            sizeUV,
                            tex
                       );

            // LOOK OVER THIS LATER
            quad.Z = pos.Z;
            quad.Order = (int)(pos.Z * 100f);

            animSpeed = 0;
            frame = 1;
        }

        public Billboard(Texture2D _tex,
                         Vector3 _pos,
                         Vector2 _origin,
                         Vector2 _size,
                         int _frame,
                         Vector2 _uvSize)
        {
            tex = _tex;
            pos = _pos;
            size = _size;

            sizeUV = _uvSize;

            tileMap = new IntVector2D((int)Math.Floor(1f / _uvSize.X),
                                      (int)Math.Floor(1f / _uvSize.Y));

            origin = new Vector3(size.X * _origin.X, size.Y * _origin.Y, 0.0f);

            quad = new Classes.Quad
                       (
                            pos + origin,
                            Vector3.Backward,
                            Vector3.Down,
                            size.X,
                            size.Y,
                            Vector2.Zero * sizeUV,
                            sizeUV,
                            tex
                       );

            // LOOK OVER THIS LATER
            quad.Z = pos.Z;
            quad.Order = (int)(pos.Z * 100f);

            FrameToUV(_frame);
            frame = _frame;
        }

        private void FrameToUV(int _frame)
        {
            quad.SetUV(new Vector2((float)(_frame % tileMap.X), (float)(_frame / tileMap.X) )*sizeUV);
        }

        public void SetUV(Vector2 uv)
        {
            quad.SetUV(uv);
        }


        public void Flip(bool x, bool y)
        {
            quad.FlipUV(x, y);
        }

        public void UpdatePos(Vector3 _pos)
        {
            pos = _pos;
            quad.SetPos(pos + origin);

            quad.Order = (int)(_pos.Z * 100f);
        }

        public int GetFrame()
        {
            return frame;
        }

        public void SetAlpha(float a)
        {
            quad.Alpha = a;
        }

        public void SetRotation(float r, float r2 = 0.0f)
        {
            quad.RotateZ(r, r2);
        }

        public void UpdateVerts(float width, float height)
        {
            quad.SetVerts(width, height);
        }

        public void SetAnim(int s, int e, int f = -1)
        {
            if (f == -1) frame = s;
            else         frame = f;
            start = s;
            end = e;

            FrameToUV(frame);

            animTimer = 0f;
        }

        public void SetFrame(int f)
        {
            frame = start = end = f;
            FrameToUV(frame);
            animSpeed = 0f;
            animTimer = 0f;
        }

        public void SetAnimationSpeed(float speed)
        {
            animSpeed = speed;
        }
        
        // Returns -1 if inbetween frames.
        public int Animate()
        {
            if (start == end || animSpeed == 0f) return -1;

            if (animTimer >= 1.0f)
            {
                frame++;
                if (frame > end) frame = start;
                FrameToUV(frame);

                animTimer = 0f;

                return frame;
            }
            else
            {
                animTimer += animSpeed;
            }

            return -1;
        }

        public void Draw()
        {
            Game1.AddQuadTrans(ref quad);
        }
    }
}
