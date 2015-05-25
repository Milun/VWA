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
        private Vector2 pos;
        private Vector3 origin;

        private Vector2 sizeUV;

        private Quad quad;

        private int start = 0;
        private int end = 0;
        private int frame = 0;

        float animTimer = 0f;
        float animSpeed = 0.05f;

        private float z = 0.0f;

        private IntVector2D tileMap;

        public Billboard(Texture2D _tex,
                         Vector2 _pos,
                         Vector2 _origin,
                         Vector2 _size,
                         int _frame,
                         Vector2 _uvSize,
                         float _z = 0.0f)
        {
            tex = _tex;
            pos = _pos;
            size = _size;
            z = _z;

            sizeUV = _uvSize;

            tileMap = new IntVector2D((int)Math.Floor(1f / _uvSize.X),
                                      (int)Math.Floor(1f / _uvSize.Y));

            origin = new Vector3(size.X * _origin.X, size.Y * _origin.Y, 0.0f);

            quad = new Classes.Quad
                       (
                            new Vector3(pos.X, pos.Y, z) + origin,
                            Vector3.Backward,
                            Vector3.Down,
                            size.X,
                            size.Y,
                            Vector2.Zero * sizeUV,
                            sizeUV,
                            tex
                       );
            quad.Z = z;
            quad.Order = (int)(z * 100f);

            FrameToUV(_frame);
            frame = _frame;
        }

        private void FrameToUV(int _frame)
        {
            quad.SetUV(new Vector2((float)(_frame % tileMap.X), (float)(_frame / tileMap.X) )*sizeUV);
        }

        public void Flip(bool x, bool y)
        {
            quad.FlipUV(x, y);
        }

        public void UpdatePos(Vector2 _pos, float _z = 0f)
        {
            pos = _pos;
            quad.SetPos(new Vector3(pos.X, pos.Y, z+_z) + origin);
        }

        public int GetFrame()
        {
            return frame;
        }

        public void SetAlpha(float a)
        {
            quad.Alpha = a;
        }

        public void SetRotation(float r)
        {
            quad.RotateZ(r);
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
