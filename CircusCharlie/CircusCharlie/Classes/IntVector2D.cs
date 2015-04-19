using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CircusCharlie.Classes
{
    class IntVector2D
    {
        public int X, Y = 0;

        public IntVector2D()
        {
            X = 0;
            Y = 0;
        }
        public IntVector2D(int _X, int _Y)
        {
            X = _X;
            Y = _Y;
        }


        public static IntVector2D operator *(IntVector2D value, int scaleFactor)
        {
            return new IntVector2D(value.X * scaleFactor, value.Y * scaleFactor);
        }
        public static IntVector2D operator *(int scaleFactor, IntVector2D value)
        {
            return new IntVector2D(value.X * scaleFactor, value.Y * scaleFactor);
        }

        public static IntVector2D operator *(IntVector2D value, float scaleFactor)
        {
            return new IntVector2D((int)(value.X * scaleFactor), (int)(value.Y * scaleFactor));
        }
        public static IntVector2D operator *(float scaleFactor, IntVector2D value)
        {
            return new IntVector2D((int)(value.X * scaleFactor), (int)(value.Y * scaleFactor));
        }

        public static IntVector2D operator +(IntVector2D value1, IntVector2D value2)
        {
            return new IntVector2D(value1.X + value2.X, value1.Y + value2.Y);
        }
        public static IntVector2D operator -(IntVector2D value1, IntVector2D value2)
        {
            return new IntVector2D(value1.X - value2.X, value1.Y - value2.Y);
        }

        public static bool operator ==(IntVector2D value1, IntVector2D value2)
        {
            return (value1.X == value2.X && value1.Y == value2.Y);
        }
        public static bool operator !=(IntVector2D value1, IntVector2D value2)
        {
            return (value1.X != value2.X || value1.Y != value2.Y);
        }

        public override bool Equals(System.Object other)
        {
            // If parameter is null return false.
            if (other == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            IntVector2D v = other as IntVector2D;
            if ((System.Object)v == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (X == v.X) && (Y == v.Y);
        }

        public bool Equals(IntVector2D v)
        {
            // If parameter is null return false:
            if ((object)v == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (X == v.X) && (Y == v.Y);
        }

        public override int GetHashCode()
        {
            return X ^ Y;
        }
    }
}
