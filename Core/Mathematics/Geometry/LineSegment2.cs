using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Mathematics.Geometry
{
    [Serializable]
    public sealed class LineSegment2
    {
        private readonly Vector2 _origin;
        private readonly Vector2 _destination;

        private LineSegment2(Vector2 origin, Vector2 destination)
        {
            _origin = origin;
            _destination = destination;
        }

        public static LineSegment2 FromOriginAndDestination(Vector2 origin, Vector2 destination)
        {
            return new LineSegment2(origin, destination);
        }

        public static LineSegment2 FromOriginAndOffset(Vector2 origin, Vector2 offset)
        {
            return FromOriginAndDestination(origin, origin + offset);
        }

        public Vector2 Origin { get { return _origin; } }
        public Vector2 Destination { get { return _destination; } }

        public Vector2 Midpoint { get { return (_origin + _destination) / 2; } }

        public Line2 PerpendicularBisector
        {
            get
            {
                return Line2.FromPointAndDirection(Midpoint, Offset.Rotate90Positive());
            }
        }

        public Vector2 Offset { get { return _destination - _origin; } }
        public double SquaredLength { get { return Offset.SquaredLength; } }
        public double Length { get { return Offset.Length; } }
    }
}
