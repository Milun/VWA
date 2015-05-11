using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CircusCharlie.Classes
{
    class Enemy : Actor
    {
        protected Billboard bill;
        private Quad shadow;

        protected float xSpeed = 0.0f;
        protected float ySpeed = 0.0f;

        protected float flipX = 1f;
        protected float flipY = 1f;

        // Used for editor.
        private Sprite spr;
        private Vector2 frameSize;
        private Vector2 billSize;

        protected Vector2 startPos;
        private Vector2 origin;

        public Enemy(Vector2 _pos, Sprite _spr, float _flipX, float _flipY, Vector2 _billSize, Vector2 _frameSize, Vector2 _origin, float shadowSize = 0f, float z = 0f)
        {
            pos = _pos;
            startPos = pos;

            flipX = _flipX;
            flipY = _flipY;

            billSize = _billSize;

            spr = _spr;
            frameSize = _frameSize;

            origin = _origin;

            bill = new Billboard(spr.GetTexture(),
                                 pos,
                                 new Vector2(origin.X*flipX, -origin.Y*flipY),
                                 _billSize,
                                 6,
                                 frameSize,
                                 z);
            
            if (shadowSize > 0f)
            { 
                shadow = new Quad
                           (
                                new Vector3(pos.X,
                                            pos.Y-flipY*0.05f,
                                            z),
                                Vector3.Up,
                                Vector3.Forward,
                                shadowSize,
                                shadowSize,
                                Vector2.Zero,
                                Vector2.One,
                                MainGame.sprBall.GetTexture()
                           );
            }

            bill.Flip((flipX != 1f), (flipY != 1f));
        }

        protected void DrawShadow()
        {
            shadow.SetPos(new Vector3(pos.X, pos.Y, 0f));
            Game1.AddQuad(ref shadow);
        }

        public override void DrawEditor()
        {
            spr.DrawView(new Vector2(pos.X * Global.gridSize - Global.gridSize*(billSize.X*0.5f+billSize.X*origin.X*flipX),
                                     pos.Y * Global.gridSize - Global.gridSize*(billSize.Y*0.5f+billSize.Y*origin.Y*flipY) ),
                         billSize * Global.gridSize,
                         Vector2.Zero,
                         new Vector2(spr.GetTexture().Width * frameSize.X,
                                     spr.GetTexture().Height * frameSize.Y),
                         Color.White,
                         true,
                         (flipY == -1f));
        }

        public override float GetValue(string name)
        {
            if (name == "flipY") return flipY;

            return 0f;
        }
    }
}
