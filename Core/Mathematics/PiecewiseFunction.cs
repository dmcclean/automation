using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Mathematics
{
    public sealed class PiecewiseFunction
    {
        private readonly Vector2[] _definedPoints;
        private readonly InterpolationBehavior _interpolation;
        private readonly ExtrapolationBehavior _leftExtrapolation;
        private readonly ExtrapolationBehavior _rightExtrapolation;

        private static VectorComparisonOnX _vectorComparisonOnX = new VectorComparisonOnX();

        private sealed class VectorComparisonOnX : IComparer<Vector2>
        {
            public int Compare(Vector2 x, Vector2 y)
            {
                return x.X.CompareTo(y.X);
            }
        }

        public PiecewiseFunction(IEnumerable<Vector2> definedPoints, InterpolationBehavior interpolation, ExtrapolationBehavior leftExtrapolation, ExtrapolationBehavior rightExtrapolation)
        {
            _definedPoints = definedPoints.ToArray();
            if (_definedPoints.Length <= 0) throw new ArgumentException("At least one defined value must be supplied.");
            Array.Sort(_definedPoints, (a, b) => a.X.CompareTo(b.X));
            _interpolation = interpolation;
            _leftExtrapolation = leftExtrapolation;
            _rightExtrapolation = rightExtrapolation;
        }

        public double Evaluate(double argument)
        {
            var argPoint = argument * Vector2.UnitX;
            int index = Array.BinarySearch<Vector2>(_definedPoints, argPoint, _vectorComparisonOnX);

            if (index >= 0)
            {
                // the exact value was found, so return it's Y value
                return _definedPoints[index].Y;
            }
            else
            {
                // the exact value was not found (this should be a common occurrence)
                var indexOfFirstGreaterValue = ~index;
                if (indexOfFirstGreaterValue == 0)
                {
                    return ExtrapolateLeft(argument);
                }
                else if (indexOfFirstGreaterValue >= _definedPoints.Length)
                {
                    return ExtrapolateRight(argument);
                }
                else
                {
                    // interpolation, again this should be a common occurence
                    var leftPoint = _definedPoints[indexOfFirstGreaterValue - 1];
                    var rightPoint = _definedPoints[indexOfFirstGreaterValue];

                    switch (_interpolation)
                    {
                        case InterpolationBehavior.UseValueOfLesserArgument:
                            return leftPoint.Y;
                        case InterpolationBehavior.UseValueOfGreaterArgument:
                            return rightPoint.Y;
                        case InterpolationBehavior.UseGreaterValue:
                            return Math.Max(leftPoint.Y, rightPoint.Y);
                        case InterpolationBehavior.UseLesserValue:
                            return Math.Min(leftPoint.Y, rightPoint.Y);
                        case InterpolationBehavior.Linear:
                        default:
                            var intervalWidth = rightPoint.X - leftPoint.X;
                            var intervalHeight = rightPoint.Y - leftPoint.Y;
                            var fromLeft = argument - leftPoint.X;
                            return leftPoint.Y + (fromLeft / intervalWidth) * intervalHeight;
                    }
                }
            }
        }

        private double ExtrapolateLeft(double argument)
        {
            switch (_leftExtrapolation)
            {
                case ExtrapolationBehavior.UseExtremeValue:
                    return _definedPoints[0].Y;
                case ExtrapolationBehavior.NotDefined:
                default:
                    return double.NaN;
            }
        }

        private double ExtrapolateRight(double argument)
        {
            switch (_rightExtrapolation)
            {
                case ExtrapolationBehavior.UseExtremeValue:
                    return _definedPoints[_definedPoints.Length - 1].Y;
                case ExtrapolationBehavior.NotDefined:
                default:
                    return double.NaN;
            }
        }
    }
}
