using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CircusCharlie.Classes
{
    class Global
    {
        public static float     viewZoom = 1f;
        public static Vector2   viewCenter = Vector2.Zero;
        public static Vector2   viewSize = Vector2.Zero;

        public static float deltaTime = 0f;

        public static readonly int spriteScale = 3;    // Each pixel corresponds to this many pixels.
        public static readonly int tileSize = 8;       // Size of individual tiles in file.

        public static readonly int gridSize = 24;

        public static Vector2 Get3DRatio(Vector2 pos)
        {
            float x = (viewCenter.X + viewSize.X - pos.X) / (Game1.SCREENWIDTH / 2f);
            float y = (viewCenter.Y + viewSize.Y - pos.Y) / (Game1.SCREENHEIGHT / 2f);

            return new Vector2(x, y);
        }

        public static void SetViewCenter(Vector2 pos)
        {
            viewCenter = pos - viewSize;
        }

        public static float Random(float min, float max)
        {
            int r = Editor.random.Next((int)(min * 100f), (int)(max * 100f));

            return r/100f;
        }

    }
}
