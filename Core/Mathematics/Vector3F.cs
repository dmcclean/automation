using System;

using T = System.Single;
using System.Collections.Generic;

namespace AutomationLibrary.Mathematics
{
    [Serializable]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = sizeof(T))]
    [AutomationLibrary.Controllers.PlcTypeName("ST_Vector3Real")]
    public struct Vector3F
        : IEquatable<Vector3F>
    {
        #region Fields

        private readonly T x, y, z;

        #endregion

        #region Constructors

        public Vector3F(T x, T y, T z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        #endregion

        #region Special Values

        public static readonly Vector3F Zero  = new Vector3F(0, 0, 0);
        public static readonly Vector3F UnitX = new Vector3F(1, 0, 0);
        public static readonly Vector3F UnitY = new Vector3F(0, 1, 0);
        public static readonly Vector3F UnitZ = new Vector3F(0, 0, 1);

        #endregion

        #region Factory Methods

        public static Vector3F FromArray(T[] array)
        {
            return FromArray(array, 0);
        }

        public static Vector3F FromArray(T[] array, int offset)
        {
            if (array == null) throw new ArgumentNullException();
            if (offset < 0) throw new IndexOutOfRangeException();
            if (array.Length < checked(offset + 3)) throw new IndexOutOfRangeException();

            var x = array[offset];
            var y = array[offset + 1];
            var z = array[offset + 2];

            return new Vector3F(x, y, z);
        }

        #endregion

        #region Properties

        public T X { get { return x; } }
        public T Y { get { return y; } }
        public T Z { get { return z; } }

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
                return (x * x) + (y * y) + (z * z);
            }
        }

        public Vector3F XComponent { get { return new Vector3F(x, 0, 0); } }

        public Vector3F YComponent { get { return new Vector3F(0, y, 0); } }

        public Vector3F ZComponent { get { return new Vector3F(0, 0, z); } }

        #endregion

        public bool TryNormalize(out Vector3F result)
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

        public Vector3F Normalize()
        {
            return this / Length;
        }

        public static T DotProduct(Vector3F a, Vector3F b)
        {
            return (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
        }

        public static Vector3F CrossProduct(Vector3F a, Vector3F b)
        {
            var x = (a.y * b.z) - (a.z * b.y);
            var y = (a.z * b.x) - (a.x * b.z);
            var z = (a.x * b.y) - (a.y * b.x);

            return new Vector3F(x, y, z);
        }

        public static T DistanceBetweenPoints(Vector3F a, Vector3F b)
        {
            return (a - b).Length;
        }

        public static T AngleBetweenVectorsInRadians(Vector3F a, Vector3F b)
        {
            var cosine = DotProduct(a, b) / (a.Length * b.Length);

            return (T)Math.Acos(cosine);
        }

        public T[] ToArray()
        {
            return new T[] { X, Y, Z };
        }

        public void CopyToArray(T[] array, int offset)
        {
            if (array == null) throw new ArgumentNullException();
            if (offset < 0) throw new IndexOutOfRangeException();
            if (array.Length < checked(offset + 3)) throw new IndexOutOfRangeException();

            array[offset] = X;
            array[offset + 1] = Y;
            array[offset + 2] = Z;
        }

        public static Vector3F Centroid(IEnumerable<Vector3F> vectors)
        {
            int count = 0;
            var result = Vector3F.Zero;
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
            if (obj is Vector3F) return Equals((Vector3F)obj);
            else return false;
        }

        public bool Equals(Vector3F other)
        {
            return (X == other.X) && (Y == other.Y) && (Z == other.Z);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}, Z: {2}", X, Y, Z);
        }

        #endregion

        #region Operator Overloads

        public static Vector3F operator +(Vector3F a, Vector3F b)
        {
            return new Vector3F(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vector3F operator -(Vector3F a, Vector3F b)
        {
            return new Vector3F(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vector3F operator -(Vector3F value)
        {
            return -1 * value;
        }

        public static Vector3F operator *(T scale, Vector3F value)
        {
            return new Vector3F(scale * value.X, scale * value.Y, scale * value.Z);
        }

        public static Vector3F operator *(Vector3F value, T scale)
        {
            return scale * value;
        }

        public static Vector3F operator /(Vector3F value, T scale)
        {
            return (1 / scale) * value;
        }

        public static implicit operator Vector3(Vector3F value)
        {
            return new Vector3(value.x, value.y, value.z);
        }

        public static explicit operator Vector3F(Vector3 value)
        {
            return new Vector3F((T)value.X, (T)value.Y, (T)value.Z);
        }

        #endregion
    }
}
