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

        public static readonly int spriteScale = 3;    // Each pixel corresponds to this many pixels.
        public static readonly int tileSize = 8;       // Size of individual tiles in file.

        public static readonly int gridSize = 24;

    }
}
