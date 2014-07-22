using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Mathematics.Geometry
{
    [Serializable]
    public sealed class Plane3
    {
        private readonly Vector3 _normal;
        private readonly double _signedDistanceFromOrigin;

        public Plane3(Vector3 normal, double signedDistanceFromOrigin)
        {
            if (!normal.TryNormalize(out _normal)) throw new IllConditionedProblemException();
            if (double.IsNaN(signedDistanceFromOrigin) || double.IsInfinity(signedDistanceFromOrigin)) throw new ArgumentOutOfRangeException();

            _signedDistanceFromOrigin = signedDistanceFromOrigin;
        }

        public static readonly Plane3 XY = new Plane3(Vector3.UnitZ, 0);
        public static readonly Plane3 XZ = new Plane3(Vector3.UnitY, 0);
        public static readonly Plane3 YZ = new Plane3(Vector3.UnitX, 0);

        public static Plane3 FromCoefficientsOfStandardForm(double a, double b, double c, double d)
        {
            Vector3 dir = new Vector3(a, b, c);
            Vector3 normal;
            if (!dir.TryNormalize(out normal)) throw new IllConditionedProblemException();
            var signedDistanceFromOrigin = d / dir.Length; // we need to normalize the d coefficient as we did with a,b,c

            return new Plane3(normal, signedDistanceFromOrigin);
        }

        public static Plane3 FromThreePoints(Vector3 a, Vector3 b, Vector3 c)
        {
            var dir = Vector3.CrossProduct(b - a, c - a);
            Vector3 normal;
            if (!dir.TryNormalize(out normal)) throw new IllConditionedProblemException();
            var signedDistanceFromOrigin = Vector3.DotProduct(normal, a); // TODO: is this negated?

            return new Plane3(normal, signedDistanceFromOrigin);
        }

        public Vector3 Normal { get { return _normal; } }

        public Vector3 PointNearestOrigin { get { return _normal * _signedDistanceFromOrigin; } }

        public double SignedDistanceFromPlane(Vector3 point)
        {
            return Vector3.DotProduct(_normal, point) + _signedDistanceFromOrigin;
        }

        public double DistanceFromPlane(Vector3 point)
        {
            return Math.Abs(SignedDistanceFromPlane(point));
        }

        public Plane3 FlipNormal()
        {
            return new Plane3(-_normal, -_signedDistanceFromOrigin);
        }
    }
}
