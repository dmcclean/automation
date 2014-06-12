using System;

using T = System.Single;
using System.Collections.Generic;

namespace AutomationLibrary.Mathematics
{
    [Serializable]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = sizeof(T))]
    [AutomationLibrary.Controllers.PlcTypeName("ST_Vector2Real")]
    public struct Vector2F
        : IEquatable<Vector2F>
    {
        #region Fields

        private readonly T x, y;

        #endregion

        #region Constructors

        public Vector2F(T x, T y)
        {
            this.x = x;
            this.y = y;
        }

        #endregion

        #region Special Values

        public static readonly Vector2F Zero  = new Vector2F(0, 0);
        public static readonly Vector2F UnitX = new Vector2F(1, 0);
        public static readonly Vector2F UnitY = new Vector2F(0, 1);

        #endregion

        #region Factory Methods

        public static Vector2F UnitVector(T angleRadians)
        {
            return new Vector2F((T)Math.Cos(angleRadians), (T)Math.Sin(angleRadians));
        }

        public static Vector2F FromArray(T[] array)
        {
            return FromArray(array, 0);
        }

        public static Vector2F FromArray(T[] array, int offset)
        {
            if (array == null) throw new ArgumentNullException();
            if (offset < 0) throw new IndexOutOfRangeException();
            if (array.Length < checked(offset + 2)) throw new IndexOutOfRangeException();

            var x = array[offset];
            var y = array[offset + 1];

            return new Vector2F(x, y);
        }

        public static Vector2F FromRadiusAndAngle(T radius, T angleRadians)
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

        public Vector2F XComponent { get { return new Vector2F(x, 0); } }

        public Vector2F YComponent { get { return new Vector2F(0, y); } }

        public bool IsNaN
        {
            get
            {
                return T.IsNaN(x) || T.IsNaN(y);
            }
        }

        #endregion

        public bool TryNormalize(out Vector2F result)
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

        public Vector2F Normalize()
        {
            return this / Length;
        }

        public static T DotProduct(Vector2F a, Vector2F b)
        {
            return (a.x * b.x) + (a.y * b.y);
        }

        public static T DistanceBetweenPoints(Vector2F a, Vector2F b)
        {
            return (a - b).Length;
        }

        public static T AngleBetweenVectorsInRadians(Vector2F a, Vector2F b)
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

        public static Vector2F Centroid(IEnumerable<Vector2F> vectors)
        {
            int count = 0;
            var result = Vector2F.Zero;
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
            if (obj is Vector2F) return Equals((Vector2F)obj);
            else return false;
        }

        public bool Equals(Vector2F other)
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

        public static Vector2F operator +(Vector2F a, Vector2F b)
        {
            return new Vector2F(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2F operator -(Vector2F a, Vector2F b)
        {
            return new Vector2F(a.X - b.X, a.Y - b.Y);
        }

        public static Vector2F operator -(Vector2F value)
        {
            return -1 * value;
        }

        public static Vector2F operator *(T scale, Vector2F value)
        {
            return new Vector2F(scale * value.X, scale * value.Y);
        }

        public static Vector2F operator *(Vector2F value, T scale)
        {
            return scale * value;
        }

        public static Vector2F operator /(Vector2F value, T scale)
        {
            return (1 / scale) * value;
        }

        public static implicit operator Vector2(Vector2F value)
        {
            return new Vector2(value.x, value.y);
        }

        public static explicit operator Vector2F(Vector2 value)
        {
            return new Vector2F((T)value.X, (T)value.Y);
        }

        #endregion
    }
}
