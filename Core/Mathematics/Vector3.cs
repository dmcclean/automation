using System;

using T = System.Double;
using System.Collections.Generic;

namespace AutomationLibrary.Mathematics
{
    [Serializable]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = sizeof(T))]
    [AutomationLibrary.Controllers.PlcTypeName("ST_Vector3LReal")]
    public struct Vector3
        : IEquatable<Vector3>
    {
        #region Fields

        private readonly T x, y, z;

        #endregion

        #region Constructors

        public Vector3(T x, T y, T z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        #endregion

        #region Special Values

        public static readonly Vector3 Zero = new Vector3(0, 0, 0);
        public static readonly Vector3 UnitX = new Vector3(1, 0, 0);
        public static readonly Vector3 UnitY = new Vector3(0, 1, 0);
        public static readonly Vector3 UnitZ = new Vector3(0, 0, 1);

        #endregion

        #region Factory Methods

        public static Vector3 FromArray(T[] array)
        {
            return FromArray(array, 0);
        }

        public static Vector3 FromArray(T[] array, int offset)
        {
            if (array == null) throw new ArgumentNullException();
            if (offset < 0) throw new IndexOutOfRangeException();
            if (array.Length < checked(offset + 3)) throw new IndexOutOfRangeException();

            var x = array[offset];
            var y = array[offset + 1];
            var z = array[offset + 2];

            return new Vector3(x, y, z);
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

        public Vector3 XComponent { get { return new Vector3(x, 0, 0); } }

        public Vector3 YComponent { get { return new Vector3(0, y, 0); } }

        public Vector3 ZComponent { get { return new Vector3(0, 0, z); } }

        #endregion

        public bool TryNormalize(out Vector3 result)
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

        public Vector3 Normalize()
        {
            return this / Length;
        }

        public static T DotProduct(Vector3 a, Vector3 b)
        {
            return (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
        }

        public static Vector3 CrossProduct(Vector3 a, Vector3 b)
        {
            var x = (a.y * b.z) - (a.z * b.y);
            var y = (a.z * b.x) - (a.x * b.z);
            var z = (a.x * b.y) - (a.y * b.x);

            return new Vector3(x, y, z);
        }

        public static T DistanceBetweenPoints(Vector3 a, Vector3 b)
        {
            return (a - b).Length;
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

        public static Vector3 Centroid(IEnumerable<Vector3> vectors)
        {
            int count = 0;
            var result = Vector3.Zero;
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
            if (obj is Vector3) return Equals((Vector3)obj);
            else return false;
        }

        public bool Equals(Vector3 other)
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

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vector3 operator -(Vector3 value)
        {
            return -1 * value;
        }

        public static Vector3 operator *(T scale, Vector3 value)
        {
            return new Vector3(scale * value.X, scale * value.Y, scale * value.Z);
        }

        public static Vector3 operator *(Vector3 value, T scale)
        {
            return scale * value;
        }

        public static Vector3 operator /(Vector3 value, T scale)
        {
            return (1 / scale) * value;
        }

        #endregion
    }
}
