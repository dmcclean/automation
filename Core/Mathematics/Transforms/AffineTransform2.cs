using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Mathematics.Transforms
{
    // represents an affine transform in 2-dimensional space
    [Serializable]
    public sealed class AffineTransform2
    {
        private readonly double _x11, _x12, _x13, _x21, _x22, _x23;

        private AffineTransform2(double x11, double x12, double x13, double x21, double x22, double x23)
        {
            _x11 = x11;
            _x12 = x12;
            _x13 = x13;
            _x21 = x21;
            _x22 = x22;
            _x23 = x23;
        }

        private AffineTransform2(double[,] x)
        {
            if (x == null) throw new ArgumentNullException();
            if (x.GetLength(0) != 3) throw new ArgumentException();
            if (x.GetLength(1) != 3) throw new ArgumentException();

            if (x[2, 0] != 0) throw new ArgumentException();
            if (x[2, 1] != 0) throw new ArgumentException();
            if (x[2, 2] != 1) throw new ArgumentException();

            _x11 = x[0, 0];
            _x12 = x[0, 1];
            _x13 = x[0, 2];
            _x21 = x[1, 0];
            _x22 = x[1, 1];
            _x23 = x[1, 2];
        }

        public static readonly AffineTransform2 Identity = Scaling(1);

        public static AffineTransform2 Compose(AffineTransform2 first, AffineTransform2 second)
        {
            var a = second;
            var b = first;
            
            var x11 = b._x11 * a._x11 + b._x12 * a._x21 + b._x13 * 0;
            var x12 = b._x11 * a._x12 + b._x12 * a._x22 + b._x13 * 0;
            var x13 = b._x11 * a._x13 + b._x12 * a._x23 + b._x13 * 1;

            var x21 = b._x21 * a._x11 + b._x22 * a._x21 + b._x23 * 0;
            var x22 = b._x21 * a._x12 + b._x22 * a._x22 + b._x23 * 0;
            var x23 = b._x21 * a._x13 + b._x22 * a._x23 + b._x23 * 1;

            return new AffineTransform2(x11, x12, x13, x21, x22, x23);
        }

        public static AffineTransform2 Rotation(double angleRadians)
        {
            var cos = Math.Cos(angleRadians);
            var sin = Math.Sin(angleRadians);

            return new AffineTransform2(cos, sin, 0, -sin, cos, 0);
        }

        public static AffineTransform2 RotationAt(Vector2 center, double angleRadians)
        {
            var first = Translation(-center);
            var second = Rotation(angleRadians);
            var third = Translation(center);

            return Compose(Compose(first, second), third);
        }

        public static AffineTransform2 Translation(Vector2 offset)
        {
            return new AffineTransform2(1, 0, offset.X, 0, 1, offset.Y);
        }

        public static AffineTransform2 Scaling(double scale)
        {
            return Scaling(scale, scale);
        }

        public static AffineTransform2 Scaling(double scaleX, double scaleY)
        {
            return new AffineTransform2(scaleX, 0, 0, 0, scaleY, 0);
        }

        public AffineTransform2 Rotate(double angleRadians)
        {
            return Compose(this, Rotation(angleRadians));
        }

        public AffineTransform2 RotateAt(Vector2 center, double angleRadians)
        {
            return Compose(this, RotationAt(center, angleRadians));
        }

        public AffineTransform2 Translate(Vector2 offset)
        {
            return Compose(this, Translation(offset));
        }

        public AffineTransform2 Scale(double scale)
        {
            return Compose(this, Scaling(scale));
        }

        public AffineTransform2 Scale(double scaleX, double scaleY)
        {
            return Compose(this, Scaling(scaleX, scaleY));
        }

        public Vector2 Apply(Vector2 x)
        {
            var newX = _x11 * x.X + _x12 * x.Y + _x13;
            var newY = _x21 * x.X + _x22 * x.Y + _x23;

            return new Vector2(newX, newY);
        }

        public Vector2F Apply(Vector2F x)
        {
            return (Vector2F)Apply((Vector2)x);
        }
    }
}
