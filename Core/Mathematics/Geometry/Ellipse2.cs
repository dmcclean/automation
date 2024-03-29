﻿using System;
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
            if (semiMinorAxis <= 0) throw new ArgumentOutOfRangeException("semiMinorAxis");
            if (semiMajorAxis < semiMinorAxis)
            {
                var temp = semiMajorAxis;
                semiMajorAxis = semiMinorAxis;
                semiMinorAxis = temp;
                angle += Math.PI / 2;
            }

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

        public double AngleOfMajorAxis { get { return _angleOfSemiMajorAxis; } }

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

        public double Circumference
        {
            get
            {
                // using Ramanujan's approximation
                // x = (a-b)/(a+b)
                // C ~= pi * (a+b)(1+3x^2/[10+sqrt(4-3x^2)])
                var a = _semiMajorAxis;
                var b = _semiMinorAxis;

                var x = (a - b) / (a + b);
                var x2 = x * x;

                return Math.PI * (a + b) * (1 + (3 * x2 / (10 + Math.Sqrt(4 - (3 * x2)))));
            }
        }

        public Vector2 PointAtAngularPosition(double angleRadians)
        {
            var effectiveRadius = EffectiveRadiusAtAngularPosition(angleRadians);
            var offset = Vector2.FromRadiusAndAngle(effectiveRadius, angleRadians);

            return _center + offset;
        }

        public Vector2 NearestPointOfApproach(Line2 line)
        {
            Vector2 incumbent = _center;
            double incumbentDistance = double.MaxValue;

            for (var theta = 0.0; theta <= 2 * Math.PI; theta += .001)
            {
                var candidate = PointAtAngularPosition(theta);
                var distance = line.DistanceFromLine(candidate);
                if (distance < incumbentDistance)
                {
                    incumbent = candidate;
                    incumbentDistance = distance;
                }
            }

            return incumbent;
        }

        public double EffectiveRadiusAtAngularPosition(double angleRadians)
        {
            // http://en.wikipedia.org/wiki/Ellipse#Polar_form_relative_to_center
            var theta = angleRadians - _angleOfSemiMajorAxis;
            
            var a = _semiMajorAxis;
            var b = _semiMinorAxis;
            
            var num = a * b;
            var bTerm = b * Math.Cos(theta);
            var aTerm = a * Math.Sin(theta);
            var denom = Math.Sqrt(bTerm * bTerm + aTerm * aTerm);

            return num / denom;
        }
    }
}
