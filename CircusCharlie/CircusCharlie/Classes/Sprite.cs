using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CircusCharlie.Classes
{
    class Sprite
    {
        private Texture2D texture { get; set; }
        private int width, height = 0;

        private SpriteBatch spriteBatch;

        // For tile sheets
        public Sprite(Texture2D _texture, ref SpriteBatch _spriteBatch, int _width, int _height)
        {
            texture = _texture;
            spriteBatch = _spriteBatch;
            width = _width;
            height = _height;
        }

        public int Width
        {
            get
            {
                return width;
            }
        }

        public int Height
        {
            get
            {
                return height;
            }
        }

        public Texture2D GetTexture()
        {
            return texture;
        }

        public SpriteBatch GetSpriteBatch()
        {
            return spriteBatch;
        }

        // For a single sprite.
        public Sprite(Texture2D _texture, ref SpriteBatch _spriteBatch)
        {
            texture = _texture;
            spriteBatch = _spriteBatch;
            width = texture.Width;
            height = texture.Height;
        }

        public void Draw(IntVector2D pos, Color color)
        {
            if (texture == null) return;

            spriteBatch.Draw(texture, new Rectangle(pos.X, pos.Y, width, height), color);
        }
        
        public void Draw(Rectangle pos, Rectangle mask, Color color)
        {
            spriteBatch.Draw(texture, pos, mask, color);
        }

        public void Draw(IntVector2D pos, IntVector2D size, Color color)
        {
            if (texture == null) return;

            spriteBatch.Draw(texture, new Rectangle(pos.X,
                                                    pos.Y,
                                                    size.X,
                                                    size.Y), color);
        }

        public void DrawView(Vector2 pos, Vector2 size, Vector2 off, Vector2 offSize, Color color)
        {
            if (texture == null) return;

            spriteBatch.Draw(texture, new Rectangle((int)(pos.X * Global.viewZoom - Global.viewCenter.X * Global.viewZoom),
                                                    (int)(pos.Y * Global.viewZoom - Global.viewCenter.Y * Global.viewZoom),
                                                    (int)(size.X * Global.viewZoom),
                                                    (int)(size.Y * Global.viewZoom)),
                                      new Rectangle((int)(off.X),
                                                    (int)(off.Y),
                                                    (int)(offSize.X),
                                                    (int)(offSize.Y)),
                                                    color);
        }

        public void DrawView(Vector2 pos, Vector2 size, Vector2 off, Vector2 offSize, Color color, bool flipX, bool flipY)
        {
            if (texture == null) return;

            SpriteEffects se = SpriteEffects.None;
            float rotation = 0f;
            Vector2 addPos = Vector2.Zero;

            if (flipX && flipY)
            {
                rotation = 180f*3.14159f/180f;
                addPos = Vector2.One;
            }
            else if (flipX)
            {
                se = SpriteEffects.FlipHorizontally;
            }
            else if (flipY)
            {
                se = SpriteEffects.FlipVertically;
            }

            spriteBatch.Draw(texture, new Rectangle((int)((pos.X) * Global.viewZoom - Global.viewCenter.X * Global.viewZoom + size.X*Global.viewZoom*addPos.X),
                                                    (int)((pos.Y) * Global.viewZoom - Global.viewCenter.Y * Global.viewZoom + size.Y*Global.viewZoom*addPos.Y),
                                                    (int)(size.X * Global.viewZoom),
                                                    (int)(size.Y * Global.viewZoom)),
                                      new Rectangle((int)(off.X),
                                                    (int)(off.Y),
                                                    (int)(offSize.X),
                                                    (int)(offSize.Y)),
                                                    color,
                                                    rotation,
                                                    Vector2.Zero,
                                                    se,
                                                    0f
                                                    );
        }

        public void DrawView(Vector2 pos, Color color)
        {
            if (texture == null) return;

            spriteBatch.Draw(texture, new Rectangle((int)(pos.X * Global.viewZoom - Global.viewCenter.X * Global.viewZoom),
                                                    (int)(pos.Y * Global.viewZoom - Global.viewCenter.Y * Global.viewZoom),
                                                    (int)(width * Global.viewZoom),
                                                    (int)(height *Global.viewZoom)), color);
        }

        /*public void Draw3D(Vector2 pos, Color color, float z)
        {
            if (texture == null) return;

            spriteBatch.Draw(texture, new Rectangle((int)(pos.X * Global.viewZoom - Global.viewCenter.X * Global.viewZoom + Global.Get3DRatio(pos).X * z * 500f - (width - width * z) / 2f),
                                                    (int)(pos.Y * Global.viewZoom - Global.viewCenter.Y * Global.viewZoom + Global.Get3DRatio(pos).Y * z * 500f - (height - height * z) / 2f),
                                                    (int)((width -width *z) * Global.viewZoom),
                                                    (int)((height-height*z) * Global.viewZoom)), color);
        }*/

        public void DrawView(Vector2 pos, IntVector2D size, Color color)
        {
            if (texture == null) return;

            spriteBatch.Draw(texture, new Rectangle((int)(pos.X * Global.viewZoom - Global.viewCenter.X * Global.viewZoom),
                                                    (int)(pos.Y * Global.viewZoom - Global.viewCenter.Y * Global.viewZoom),
                                                    (int)(size.X * Global.viewZoom),
                                                    (int)(size.Y * Global.viewZoom)), color);
        }
    }
}
