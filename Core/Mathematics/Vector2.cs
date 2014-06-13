using System;

using T = System.Double;
using System.Collections.Generic;

namespace AutomationLibrary.Mathematics
{
    [Serializable]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = sizeof(T))]
    [AutomationLibrary.Controllers.PlcTypeName("ST_Vector2LReal")]
    public struct Vector2
        : IEquatable<Vector2>
    {
        #region Fields

        private readonly T x, y;

        #endregion

        #region Constructors

        public Vector2(T x, T y)
        {
            this.x = x;
            this.y = y;
        }

        #endregion

        #region Special Values

        public static readonly Vector2 Zero  = new Vector2(0, 0);
        public static readonly Vector2 UnitX = new Vector2(1, 0);
        public static readonly Vector2 UnitY = new Vector2(0, 1);

        #endregion

        #region Factory Methods

        public static Vector2 UnitVector(T angleRadians)
        {
            return new Vector2((T)Math.Cos(angleRadians), (T)Math.Sin(angleRadians));
        }

        public static Vector2 FromArray(T[] array)
        {
            return FromArray(array, 0);
        }

        public static Vector2 FromArray(T[] array, int offset)
        {
            if (array == null) throw new ArgumentNullException();
            if (offset < 0) throw new IndexOutOfRangeException();
            if (array.Length < checked(offset + 2)) throw new IndexOutOfRangeException();

            var x = array[offset];
            var y = array[offset + 1];

            return new Vector2(x, y);
        }

        public static Vector2 FromRadiusAndAngle(T radius, T angleRadians)
        {
            return radius * UnitVector(angleRadians);
        }

        #endregion

        #region Properties

        public T X { get { return x; } }
        public T Y { get { return y; } }

        public T Length
        {
            get
            {
                return (T)Math.Sqrt(SquaredLength);
            }
        }

        public T SquaredLength
        {
            get
            {
                return (x * x) + (y * y);
            }
        }

        public T AngleFromXAxis
        {
            get
            {
                return (T)Math.Atan2(y, x);
            }
        }

        public Vector2 XComponent { get { return new Vector2(x, 0); } }

        public Vector2 YComponent { get { return new Vector2(0, y); } }

        public bool IsNaN
        {
            get
            {
                return T.IsNaN(x) || T.IsNaN(y);
            }
        }

        #endregion

        public bool TryNormalize(out Vector2 result)
        {
            var norm = Length;
            if (norm < 1e-6)
            {
                result = this;
                return false;
            }
            else
            {
                result = this / norm;
                return true;
            }
        }

        public Vector2 Normalize()
        {
            return this / Length;
        }

        public Vector2 Rotate90Positive()
        {
            return new Vector2(-y, x);
        }

        public Vector2 Rotate90Negative()
        {
            return new Vector2(y, -x);
        }

        public static T DotProduct(Vector2 a, Vector2 b)
        {
            return (a.x * b.x) + (a.y * b.y);
        }

        public static T DistanceBetweenPoints(Vector2 a, Vector2 b)
        {
            return (a - b).Length;
        }

        public static T AngleBetweenVectorsInRadians(Vector2 a, Vector2 b)
        {
            var cosine = DotProduct(a, b) / (a.Length * b.Length);

            return (T)Math.Acos(cosine);
        }

        public T[] ToArray()
        {
            return new T[] { X, Y };
        }

        public void CopyToArray(T[] array, int offset)
        {
            if (array == null) throw new ArgumentNullException();
            if (offset < 0) throw new IndexOutOfRangeException();
            if (array.Length < checked(offset + 2)) throw new IndexOutOfRangeException();

            array[offset] = X;
            array[offset + 1] = Y;
        }

        public static Vector2 Centroid(IEnumerable<Vector2> vectors)
        {
            int count = 0;
            var result = Vector2.Zero;
            foreach (var vector in vectors)
            {
                result += vector;
                count += 1;
            }

            if (count > 0) result /= count;
            return result;
        }

        #region Object Overrides

        public override bool Equals(object obj)
        {
            if (obj is Vector2) return Equals((Vector2)obj);
            else return false;
        }

        public bool Equals(Vector2 other)
        {
            return (X == other.X) && (Y == other.Y);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}", X, Y);
        }

        #endregion

        #region Operator Overloads

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }

        public static Vector2 operator -(Vector2 value)
        {
            return -1 * value;
        }

        public static Vector2 operator *(T scale, Vector2 value)
        {
            return new Vector2(scale * value.X, scale * value.Y);
        }

        public static Vector2 operator *(Vector2 value, T scale)
        {
            return scale * value;
        }

        public static Vector2 operator /(Vector2 value, T scale)
        {
            return (1 / scale) * value;
        }

        #endregion
    }
}
