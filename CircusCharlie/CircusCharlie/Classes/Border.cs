using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CircusCharlie.Classes
{
    class Border
    {
        IntVector2D point1;
        IntVector2D point2;
        IntVector2D topLeft;
        IntVector2D bottomRight;
        int size = 256;

        SpriteBatch spriteBatch;

        private Texture2D texBrowser;

        public Border( SpriteBatch sb,
                       Texture2D tex,
                       IntVector2D _point1,
                       IntVector2D _point2,
                       IntVector2D _topLeft,
                       IntVector2D _bottomRight)
        {
            point1 = _point1;
            point2 = _point2;
            topLeft = _topLeft;
            bottomRight = _bottomRight;

            spriteBatch = sb;
            texBrowser = tex;
        }

        public void Draw()
        {
            // Left Top
            spriteBatch.Draw(texBrowser,
                             new Rectangle(topLeft.X, topLeft.Y, point1.X, point1.Y),
                             new Rectangle(0, 0, point1.X, point1.Y),
                             Color.White);

            // Mid Top
            spriteBatch.Draw(texBrowser,
                             new Rectangle(topLeft.X + point1.X, topLeft.Y, bottomRight.X - point1.X - (256 - point2.X) - topLeft.X, point1.Y),
                             new Rectangle(point1.X, 0, point2.X - point1.X, point1.Y),
                             Color.White);

            // Right Top
            spriteBatch.Draw(texBrowser,
                             new Rectangle(bottomRight.X - point1.X, topLeft.Y, point1.X, point1.Y),
                             new Rectangle(point2.X, 0, size-point2.X, point1.Y),
                             Color.White);



            // Left Bottom
            spriteBatch.Draw(texBrowser,
                             new Rectangle(topLeft.X, bottomRight.Y-point1.X, point1.X, point1.Y),
                             new Rectangle(0, point2.Y, point1.X, point1.Y),
                             Color.White);

            // Mid Bottom
            spriteBatch.Draw(texBrowser,
                             new Rectangle(topLeft.X + point1.X, bottomRight.Y - point1.X, bottomRight.X - point1.X - (256 - point2.X) - topLeft.X, point1.Y),
                             new Rectangle(point1.X, point2.Y, point2.X - point1.X, point1.Y),
                             Color.White);

            // Right Bottom
            spriteBatch.Draw(texBrowser,
                             new Rectangle(bottomRight.X - point1.X, bottomRight.Y - point1.X, point1.X, point1.Y),
                             new Rectangle(point2.X, point2.Y, size - point2.X, point1.Y),
                             Color.White);


            // Left Mid
            spriteBatch.Draw(texBrowser,
                             new Rectangle(topLeft.X, topLeft.Y + point1.Y, point1.X, bottomRight.Y - (point2.Y - point1.Y) - point1.Y - topLeft.Y),
                             new Rectangle(0, point1.Y, point1.X, point2.Y-point1.Y),
                             Color.White);

            // Right Mid
            spriteBatch.Draw(texBrowser,
                             new Rectangle(bottomRight.X-point1.X, topLeft.Y + point1.Y, point1.X, bottomRight.Y - (point2.Y - point1.Y) - point1.Y - topLeft.Y),
                             new Rectangle(point2.X, point1.Y, point1.X, point2.Y-point1.Y),
                             Color.White);

            // Black
            spriteBatch.Draw(texBrowser,
                             new Rectangle(0, 0, topLeft.X, bottomRight.Y),
                             new Rectangle(0, 0, 1, 1),
                             Color.Black);

            spriteBatch.Draw(texBrowser,
                             new Rectangle(bottomRight.X, 0, topLeft.X, bottomRight.Y),
                             new Rectangle(0, 0, 1, 1),
                             Color.Black);

            /*spriteBatch.Draw(texBrowser, new Rectangle(0,
                                                       0,
                                                       leftCorner,
                                                       Game1.SCREENHEIGHT),
                                         new Rectangle(0,
                                                       0,
                                                       1,
                                                       1),
                                                       Color.Black);

            spriteBatch.Draw(texBrowser, new Rectangle(798,
                                                       0,
                                                       leftCorner,
                                                       Game1.SCREENHEIGHT),
                                         new Rectangle(0,
                                                       0,
                                                       1,
                                                       1),
                                                       Color.Black);

            ///

            spriteBatch.Draw(texBrowser, new Rectangle(leftCorner,
                                                       0,
                                                       size,
                                                       size),
                                         new Rectangle(0,
                                                       0,
                                                       size,
                                                       size),
                                                       Color.White);

            spriteBatch.Draw(texBrowser, new Rectangle(leftCorner,
                                                       Game1.SCREENHEIGHT - size,
                                                       size,
                                                       size),
                                         new Rectangle(0,
                                                       256 - size,
                                                       size,
                                                       size),
                                                       Color.White);

            spriteBatch.Draw(texBrowser, new Rectangle(leftCorner + size,
                                                       Game1.SCREENHEIGHT - size,
                                                       427,
                                                       size),
                                         new Rectangle(size,
                                                       256 - size,
                                                       256 - size * 2,
                                                       size),
                                                       Color.White);

            spriteBatch.Draw(texBrowser, new Rectangle(leftCorner + size,
                                                       0,
                                                       427,
                                                       size),
                                         new Rectangle(size,
                                                       0,
                                                       256 - size * 2,
                                                       size),
                                                       Color.White);

            spriteBatch.Draw(texBrowser, new Rectangle(720,
                                                       0,
                                                       size,
                                                       size),
                                         new Rectangle(256 - size,
                                                       0,
                                                       size,
                                                       size),
                                                       Color.White);

            spriteBatch.Draw(texBrowser, new Rectangle(720,
                                                       Game1.SCREENHEIGHT - size,
                                                       size,
                                                       size),
                                         new Rectangle(256 - size,
                                                       256 - size,
                                                       size,
                                                       size),
                                                       Color.White);*/
        }
    }
}
