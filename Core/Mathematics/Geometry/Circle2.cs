using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Mathematics.Geometry
{
    [Serializable]
    public sealed class Circle2
    {
        private readonly Vector2 _center;
        private readonly double _radius;

        public Circle2(Vector2 center, double radius)
        {
            if (radius < 0) throw new ArgumentOutOfRangeException();
            _center = center;
            _radius = radius;
        }

        public Vector2 Center { get { return _center; } }
        public double Radius { get { return _radius; } }

        public double Diameter { get { return _radius * 2; } }
        public double Circumference { get { return Math.PI * Diameter; } }

        public double DistanceFromCircle(Vector2 point)
        {
            return Math.Abs(SignedDistanceFromCircle(point));
        }

        public double SignedDistanceFromCircle(Vector2 point)
        {
            return (point - _center).Length - _radius;
        }

        public bool Contains(Vector2 point)
        {
            return SignedDistanceFromCircle(point) <= 0;
        }
    }
}
