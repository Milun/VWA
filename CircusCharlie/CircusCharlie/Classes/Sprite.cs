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

        public void Draw(IntVector2D pos, IntVector2D off, int rotate, float scale, Color color)
        {
            Rectangle mask = new Rectangle(off.X, off.Y, width, height);

            if (texture != null) spriteBatch.Draw(texture,
                                                  new Vector2(pos.X + width / 2 * scale, pos.Y + height / 2 * scale),
                                                  mask,
                                                  color,
                                                  (float)(rotate) * 90 * 3.14159f / 180.0f,
                                                  new Vector2(width / 2, height / 2),
                                                  scale,
                                                  SpriteEffects.None,
                                                  1);
        }

        public void DrawView(Vector2 pos, Color color)
        {
            if (texture == null) return;

            spriteBatch.Draw(texture, new Rectangle((int)(pos.X * Global.viewZoom - Global.viewCenter.X * Global.viewZoom),
                                                    (int)(pos.Y * Global.viewZoom - Global.viewCenter.Y * Global.viewZoom),
                                                    (int)(width * Global.viewZoom),
                                                    (int)(height *Global.viewZoom)), color);
        }

        public void Draw3D(Vector2 pos, Color color, float z)
        {
            if (texture == null) return;

            spriteBatch.Draw(texture, new Rectangle((int)(pos.X * Global.viewZoom - Global.viewCenter.X * Global.viewZoom + Global.Get3DRatio(pos).X * z * 500f - (width - width * z) / 2f),
                                                    (int)(pos.Y * Global.viewZoom - Global.viewCenter.Y * Global.viewZoom + Global.Get3DRatio(pos).Y * z * 500f - (height - height * z) / 2f),
                                                    (int)((width -width *z) * Global.viewZoom),
                                                    (int)((height-height*z) * Global.viewZoom)), color);
        }

        public void DrawView(Vector2 pos, IntVector2D size, Color color)
        {
            if (texture == null) return;

            spriteBatch.Draw(texture, new Rectangle((int)(pos.X * Global.viewZoom - Global.viewCenter.X * Global.viewZoom),
                                                    (int)(pos.Y * Global.viewZoom - Global.viewCenter.Y * Global.viewZoom),
                                                    (int)(size.X * Global.viewZoom),
                                                    (int)(size.Y * Global.viewZoom)), color);
        }

        public void DrawView(Vector2 pos, IntVector2D off, int rotate, float scale, Color color)
        {
            Rectangle mask = new Rectangle(off.X, off.Y, width, height);

            if (texture != null) spriteBatch.Draw(texture,
                                                  new Vector2(pos.X * Global.viewZoom + width / 2 * scale * Global.viewZoom - Global.viewCenter.X * Global.viewZoom,
                                                              pos.Y * Global.viewZoom + height / 2 * scale * Global.viewZoom - Global.viewCenter.Y * Global.viewZoom),
                                                  mask,
                                                  color,
                                                  (float)(rotate) * 90 * 3.14159f / 180.0f,
                                                  new Vector2(width / 2, height / 2),
                                                  scale * Global.viewZoom,
                                                  SpriteEffects.None,
                                                  1);
        }
    }
}
