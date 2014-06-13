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

        public static Circle2 FromThreePoints(Vector2 a, Vector2 b, Vector2 c)
        {
            // form the three chords
            var ab = LineSegment2.FromOriginAndDestination(a, b);
            var bc = LineSegment2.FromOriginAndDestination(b, c);
            var ca = LineSegment2.FromOriginAndDestination(c, a);

            // for numerical reasons, use the longest two
            var chords = new LineSegment2[] { ab, bc, ca };
            chords = chords.OrderByDescending(chord => chord.SquaredLength).Take(2).ToArray();

            // take the perpendicular bisectors of these two chords
            var p1 = chords[0].PerpendicularBisector;
            var p2 = chords[1].PerpendicularBisector;

            // where they intersect is the center
            var maybeCenter = Line2.Intersection(p1, p2);
            if (maybeCenter == null) throw new IllConditionedProblemException();

            var center = maybeCenter.Value;

            // find the radius as the distance from one of the points to the center
            // for symmetry we will do all three and average
            var r = (Vector2.DistanceBetweenPoints(center, a) + Vector2.DistanceBetweenPoints(center, b) + Vector2.DistanceBetweenPoints(center, c)) / 3;

            return new Circle2(center, r);
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
