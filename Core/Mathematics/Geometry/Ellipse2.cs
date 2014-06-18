using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Mathematics.Geometry
{
    [Serializable]
    public sealed class Ellipse2
    {
        private readonly Vector2 _center;
        private readonly double _semiMajorAxis;
        private readonly double _semiMinorAxis;
        private readonly double _angleOfSemiMajorAxis;

        public Ellipse2(Vector2 center, double semiMajorAxis, double semiMinorAxis, double angle)
        {
            if (semiMinorAxis <= 0) throw new ArgumentOutOfRangeException();
            if (semiMajorAxis < semiMinorAxis) throw new ArgumentOutOfRangeException();

            _center = center;
            _semiMajorAxis = semiMajorAxis;
            _semiMinorAxis = semiMinorAxis;
            _angleOfSemiMajorAxis = angle;
        }

        public double FocalDistance
        {
            get
            {
                return Math.Sqrt((_semiMajorAxis * _semiMajorAxis) - (_semiMinorAxis * _semiMinorAxis));
            }
        }

        private Vector2 DirectionToFirstFocus
        {
            get
            {
                return Vector2.UnitVector(_angleOfSemiMajorAxis);
            }
        }

        public Vector2 Center { get { return _center; } }
        public Vector2 FirstFocus { get { return _center + (FocalDistance * DirectionToFirstFocus); } }
        public Vector2 SecondFocus { get { return _center - (FocalDistance * DirectionToFirstFocus); } }

        public double SemiMajorAxisLength { get { return _semiMajorAxis; } }
        public double SemiMinorAxisLength { get { return _semiMinorAxis; } }
        public double MajorAxisLength { get { return 2 * _semiMajorAxis; } }
        public double MinorAxisLength { get { return 2 * _semiMinorAxis; } }
        public double DistanceBetweenFoci { get { return 2 * FocalDistance; } }

        public double Eccentricity
        {
            get
            {
                return FocalDistance / _semiMajorAxis;
            }
        }

        public double Area
        {
            get
            {
                return Math.PI * _semiMajorAxis * _semiMinorAxis;
            }
        }
    }
}
