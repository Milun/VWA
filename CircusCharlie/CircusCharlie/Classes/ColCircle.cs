using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CircusCharlie.Classes
{
    class ColCircle : Col
    {
        private Vector2 origin; // Relative
        private float diamater;   // Relative

        private const float leeway = 1/24f;

        public ColCircle(Vector2 _pos, Vector2 _origin, float _diameter)
        {
            pos = _pos;             // Actual position on screen.
            origin = _origin;       // Plus the origin (to offset).
            diamater = _diameter;   // Box is this big.

            tL = pos + origin - new Vector2(diamater/2f, diamater/2f);
            bR = pos + origin + new Vector2(diamater/2f, diamater/2f);
        }

        public override void DrawDebug(Color color)
        {
            if (!Editor.showDebug) return;

            Editor.sprCircle.Draw(new IntVector2D((int)(tL.X*Global.gridSize), (int)(tL.Y*Global.gridSize)),
                                  new IntVector2D((int)(diamater * Global.gridSize), (int)(diamater * Global.gridSize)), color);

            Editor.sprDebug.Draw(new IntVector2D((int)(tL.X * Global.gridSize-1), (int)(tL.Y * Global.gridSize-1)),
                                  new IntVector2D(2, 2), Color.Silver);

            Editor.sprDebug.Draw(new IntVector2D((int)(bR.X * Global.gridSize-1), (int)(bR.Y * Global.gridSize-1)),
                                  new IntVector2D(2, 2), Color.Silver);
        }

        public Vector2 Center
        {
            get
            {
                return origin + pos;
            }
        }

        public float Rad
        {
            get
            {
                return diamater / 2f;
            }
        }

        public override void UpdatePos(Vector2 _pos)
        {
            base.UpdatePos(_pos);

            // Update bounding boxes.
            tL = pos + origin - new Vector2(diamater / 2f, diamater / 2f);
            bR = pos + origin + new Vector2(diamater / 2f, diamater / 2f);
        }

        public override Vector2 CheckColSquare(ColSquare other)
        {
            return other.CheckColCircle(this);
        }

    }
}
