using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Mathematics.Geometry
{
    [Serializable]
    public sealed class Line2
    {
        private readonly Vector2 _normal;
        private readonly double _signedDistanceFromOrigin;

        public Line2(Vector2 normal, double signedDistanceFromOrigin)
        {
            if (!normal.TryNormalize(out _normal)) throw new IllConditionedProblemException();
            if (double.IsNaN(signedDistanceFromOrigin) || double.IsInfinity(signedDistanceFromOrigin)) throw new ArgumentOutOfRangeException();

            _signedDistanceFromOrigin = signedDistanceFromOrigin;
        }

        public static Line2 FromCoefficientsOfStandardForm(double a, double b, double c)
        {
            Vector2 dir = new Vector2(a, b);
            Vector2 normal;
            if (!dir.TryNormalize(out normal)) throw new IllConditionedProblemException();
            var signedDistanceFromOrigin = c / dir.Length; // we need to normalize the c coefficient as we did with a,b

            return new Line2(normal, signedDistanceFromOrigin);
        }

        public static Line2 FromSlopeAndIntercept(double slope, double intercept)
        {
            var a = slope;
            var b = -1;
            var c = intercept;

            return Line2.FromCoefficientsOfStandardForm(a, b, c);
        }

        public static Line2 FromPointAndDirection(Vector2 point, Vector2 direction)
        {
            var normal = direction.Rotate90Positive().Normalize();

            var distanceFromOriginToPointProjectedOnNormal = Vector2.DotProduct(point, normal);
            return new Line2(normal, distanceFromOriginToPointProjectedOnNormal);
        }

        public static Line2 FromTwoPoints(Vector2 a, Vector2 b)
        {
            var offset = b - a;

            return FromPointAndDirection(a, offset);
        }

        public Vector2 Normal { get { return _normal; } }
        public double Slope { get { return -_normal.X / _normal.Y; } }
        public double Intercept { get { return _signedDistanceFromOrigin / _normal.Y; } }

        public double SignedDistanceFromLine(Vector2 point)
        {
            return Vector2.DotProduct(_normal, point) + _signedDistanceFromOrigin;
        }

        public double DistanceFromLine(Vector2 point)
        {
            return Math.Abs(SignedDistanceFromLine(point));
        }

        public Line2 FlipNormal()
        {
            return new Line2(-_normal, -_signedDistanceFromOrigin);
        }

        public static Vector2? Intersection(Line2 a, Line2 b)
        {
            // http://en.wikipedia.org/wiki/Line-line_intersection#Using_homogeneous_coordinates

            var aPrime = new Vector3(a._normal.X, a._normal.Y, a._signedDistanceFromOrigin);
            var bPrime = new Vector3(b._normal.X, b._normal.Y, b._signedDistanceFromOrigin);
            var c = Vector3.CrossProduct(aPrime, bPrime);

            if (Math.Abs(c.Z) < 1e-10) return null;
            else return new Vector2(-c.X / c.Z, -c.Y / c.Z);
        }
    }
}
