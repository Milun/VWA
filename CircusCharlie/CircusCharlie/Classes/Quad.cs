#region File Description
//-----------------------------------------------------------------------------
// Quad.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace CircusCharlie.Classes
{
    public struct Quad
    {
        public Vector3 Origin;
        public Vector3 UpperLeft;
        public Vector3 LowerLeft;
        public Vector3 UpperRight;
        public Vector3 LowerRight;
        public Vector3 Normal;
        public Vector3 Up;
        public Vector3 Left;

        Vector2 textureUpperLeft;
        Vector2 textureUpperRight;
        Vector2 textureLowerLeft;
        Vector2 textureLowerRight;

        public VertexPositionNormalTexture[] Vertices;
        public short[] Indexes;

        public Texture2D Tex;

        private float Width;
        private float Height;
        private bool flipX;
        private bool flipY;

        public Vector2 uvSize;
        public float Alpha;
        public float RotX;
        public float RotY;
        public float RotZ;

        public float Z;

        public int Order;

        public Quad( Vector3 origin, Vector3 normal, Vector3 up, 
            float width, float height, Vector2 UVPos, Vector2 UVSize, Texture2D tex )
        {
            Vertices = new VertexPositionNormalTexture[4];
            Indexes = new short[6];
            Origin = origin;
            Normal = normal;
            Up = up;
            Tex = tex;
            uvSize = UVSize;
            Width = width;
            Height = height;
            Alpha = 1.0f;

            Z = 0f;
            Order = 0;

            flipX = false;
            flipY = false;

            RotX = RotY = RotZ = 0.0f;

            if (uvSize.X > 0f && uvSize.Y > 0f)
            { 
                textureUpperRight = UVPos;
                textureUpperLeft = new Vector2(UVPos.X + Math.Abs(UVSize.X), UVPos.Y);
                textureLowerRight = new Vector2(UVPos.X, UVPos.Y + Math.Abs(UVSize.Y));
                textureLowerLeft = UVPos + new Vector2(Math.Abs(UVSize.X), Math.Abs(UVSize.Y));
            }
            else if (uvSize.X <= 0f && uvSize.Y > 0f)
            {
                textureUpperLeft = UVPos;
                textureUpperRight = new Vector2(UVPos.X + Math.Abs(UVSize.X), UVPos.Y);
                textureLowerLeft = new Vector2(UVPos.X, UVPos.Y + Math.Abs(UVSize.Y));
                textureLowerRight = UVPos + new Vector2(Math.Abs(UVSize.X), Math.Abs(UVSize.Y));
            }
            else if (uvSize.X > 0f && uvSize.Y <= 0f)
            {
                textureLowerRight = UVPos;
                textureLowerLeft = new Vector2(UVPos.X + Math.Abs(UVSize.X), UVPos.Y);
                textureUpperRight = new Vector2(UVPos.X, UVPos.Y + Math.Abs(UVSize.Y));
                textureUpperLeft = UVPos + new Vector2(Math.Abs(UVSize.X), Math.Abs(UVSize.Y));
            }
            else
            {
                textureLowerLeft = UVPos;
                textureLowerRight = new Vector2(UVPos.X + Math.Abs(UVSize.X), UVPos.Y);
                textureUpperLeft = new Vector2(UVPos.X, UVPos.Y + Math.Abs(UVSize.Y));
                textureUpperRight = UVPos + new Vector2(Math.Abs(UVSize.X), Math.Abs(UVSize.Y));
            }

            // Calculate the quad corners
            Left = Vector3.Cross( normal, Up );
            Vector3 uppercenter = (Up * height / 2) + origin;
            UpperLeft = uppercenter + (Left * width / 2);
            UpperRight = uppercenter - (Left * width / 2);
            LowerLeft = UpperLeft - (Up * height);
            LowerRight = UpperRight - (Up * height);

            FillVertices();
        }

        public void SetVerts(float width, float height)
        {
            // Calculate the quad corners
            Left = Vector3.Cross(Normal, Up);
            Vector3 uppercenter = (Up * height / 2) + Origin;
            UpperLeft = uppercenter + (Left * width / 2);
            UpperRight = uppercenter - (Left * width / 2);
            LowerLeft = UpperLeft - (Up * height);
            LowerRight = UpperRight - (Up * height);

            FillVertices();
        }

        public void SetPos(Vector3 pos)
        {
            Origin = pos;

            // Calculate the quad corners
            Left = Vector3.Cross(Normal, Up);
            Vector3 uppercenter = (Up * Height / 2) + Origin;
            UpperLeft = uppercenter + (Left * Width / 2);
            UpperRight = uppercenter - (Left * Width / 2);
            LowerLeft = UpperLeft - (Up * Height);
            LowerRight = UpperRight - (Up * Height);

            FillVertices();
        }

        public void RotateZ(float r, float r2 = 0.0f)
        {
            RotZ = r;
            Up = new Vector3((float)Math.Sin(r*3.14159/180f),
                             -(float)Math.Cos(r*3.14159/180f),
                             0.0f);

            Normal = new Vector3((float)Math.Sin(r2 * 3.14159 / 180f),
                                0.0f,
                                -(float)Math.Cos(r2 * 3.14159 / 180f));

            // Calculate the quad corners
            Left = Vector3.Cross(Normal, Up);

            Vector3 uppercenter = (Up * Height / 2) + Origin;
            UpperLeft = uppercenter + (Left * Width / 2);
            UpperRight = uppercenter - (Left * Width / 2);
            LowerLeft = UpperLeft - (Up * Height);
            LowerRight = UpperRight - (Up * Height);

            FillVertices();
        }

        public void FlipUV(bool x, bool y)
        {
            flipX = x;
            flipY = y;

            FillVertices();
        }

        public void SetUV(Vector2 UVPos)
        {
            textureUpperRight = UVPos;
            textureUpperLeft = new Vector2(UVPos.X + uvSize.X, UVPos.Y);
            textureLowerRight = new Vector2(UVPos.X, UVPos.Y + uvSize.Y);
            textureLowerLeft = UVPos + uvSize;

            FillVertices();
        }
        
        private void FillVertices()
        {
            // Provide a normal for each vertex
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].Normal = Normal;
            }

            // Set the position and texture coordinate for each
            // vertex

            Vertices[0].Position = LowerLeft;
            Vertices[1].Position = UpperLeft;
            Vertices[2].Position = LowerRight;
            Vertices[3].Position = UpperRight;

            if (!flipX && !flipY)
            { 
                Vertices[0].TextureCoordinate = textureLowerLeft;
                Vertices[1].TextureCoordinate = textureUpperLeft;
                Vertices[2].TextureCoordinate = textureLowerRight;
                Vertices[3].TextureCoordinate = textureUpperRight;
            }
            else if (flipX && !flipY)
            {
                Vertices[0].TextureCoordinate = textureLowerRight;
                Vertices[1].TextureCoordinate = textureUpperRight;
                Vertices[2].TextureCoordinate = textureLowerLeft;
                Vertices[3].TextureCoordinate = textureUpperLeft;
            }
            else if (!flipX && flipY)
            {
                Vertices[0].TextureCoordinate = textureUpperLeft;
                Vertices[1].TextureCoordinate = textureLowerLeft;
                Vertices[2].TextureCoordinate = textureUpperRight;
                Vertices[3].TextureCoordinate = textureLowerRight;
            }
            else if (flipX && flipY)
            {
                Vertices[0].TextureCoordinate = textureUpperRight;
                Vertices[1].TextureCoordinate = textureLowerRight;
                Vertices[2].TextureCoordinate = textureUpperLeft;
                Vertices[3].TextureCoordinate = textureLowerLeft;
            }

            // Set the index buffer for each vertex, using
            // clockwise winding
            Indexes[0] = 0;
            Indexes[1] = 1;
            Indexes[2] = 2;
            Indexes[3] = 2;
            Indexes[4] = 1;
            Indexes[5] = 3;
        }
    }
}