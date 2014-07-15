using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomationLibrary.Mathematics.Fitting;

namespace AutomationLibrary.Mathematics
{
    public sealed class PiecewiseFunction
    {
        private readonly Vector2[] _definedPoints;
        private readonly InterpolationBehavior _interpolation;
        private readonly ExtrapolationBehavior _leftExtrapolation;
        private readonly ExtrapolationBehavior _rightExtrapolation;
        private readonly double _period;

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
            _period = double.NaN;
        }

        public PiecewiseFunction(double period, IEnumerable<Vector2> definedPoints) // TODO: allow other interpolation styles
        {
            if (double.IsNaN(period) || double.IsInfinity(period) || period <= 0) throw new ArgumentOutOfRangeException();
            _definedPoints = definedPoints.Select(p => new Vector2(NormalizePeriodicArgument(period, p.X), p.Y)).ToArray();
            if (_definedPoints.Length <= 0) throw new ArgumentException("At least one defined value must be supplied.");
            Array.Sort(_definedPoints, (a, b) => a.X.CompareTo(b.X));
            _interpolation = InterpolationBehavior.Linear;
            _leftExtrapolation = ExtrapolationBehavior.NotDefined;
            _rightExtrapolation = ExtrapolationBehavior.NotDefined;
            _period = period;
        }

        private static double NormalizePeriodicArgument(double period, double argument)
        {
            var residue = argument % period;
            if (residue < 0) residue += period;
            return residue;
        }

        public double Evaluate(double argument)
        {
            if (double.IsNaN(_period)) return EvaluateNonPeriodic(argument);
            else return EvaluatePeriodic(argument);
        }

        private double EvaluatePeriodic(double argument)
        {
            var clamped = NormalizePeriodicArgument(_period, argument);

            var argPoint = clamped * Vector2.UnitX;
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
                
                Vector2 leftPoint;
                Vector2 rightPoint;

                if (indexOfFirstGreaterValue == 0)
                {
                    leftPoint = _definedPoints.Last() - new Vector2(_period, 0);
                    rightPoint = _definedPoints[0];
                }
                else if (indexOfFirstGreaterValue >= _definedPoints.Length)
                {
                    leftPoint = _definedPoints.Last();
                    rightPoint = _definedPoints[0] + new Vector2(_period, 0);
                }
                else
                {
                    // interpolation, again this should be a common occurence
                    leftPoint = _definedPoints[indexOfFirstGreaterValue - 1];
                    rightPoint = _definedPoints[indexOfFirstGreaterValue];
                }

                return LinearlyInterpolate(clamped, leftPoint, rightPoint);
            }
        }

        private static double LinearlyInterpolate(double argument, Vector2 leftPoint, Vector2 rightPoint)
        {
            var intervalWidth = rightPoint.X - leftPoint.X;
            var intervalHeight = rightPoint.Y - leftPoint.Y;
            var fromLeft = argument - leftPoint.X;
            return leftPoint.Y + (fromLeft / intervalWidth) * intervalHeight;
        }

        private double EvaluateNonPeriodic(double argument)
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
                            return LinearlyInterpolate(argument, leftPoint, rightPoint);
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

        public PiecewiseFunction SavitzkyGolaySmooth(int filterOrder, int filterLength, Func<int, IRationalFunctionFitter> createPolynomialFitter)
        {
            if (filterLength < 1) throw new ArgumentOutOfRangeException();
            if (filterLength % 2 != 1) throw new ArgumentException("Filter length must be odd.");
            if (filterOrder < 2) throw new ArgumentOutOfRangeException();
            if (filterOrder > filterLength) throw new ArgumentOutOfRangeException();
            if (filterLength > _definedPoints.Length) throw new ArgumentOutOfRangeException("Filter too long for available data points.");

            if (double.IsNaN(_period)) return SavitzkyGolaySmoothNonPeriodic(filterOrder, filterLength, createPolynomialFitter);
            else return SavitzkyGolaySmoothPeriodic(filterOrder, filterLength, createPolynomialFitter);
        }

        private PiecewiseFunction SavitzkyGolaySmoothNonPeriodic(int filterOrder, int filterLength, Func<int, IRationalFunctionFitter> createPolynomialFitter)
        {
            throw new NotImplementedException();
        }

        private PiecewiseFunction SavitzkyGolaySmoothPeriodic(int filterOrder, int filterLength, Func<int, IRationalFunctionFitter> createPolynomialFitter)
        {
            var fitter = createPolynomialFitter(filterOrder);

            var halfLength = (filterLength - 1) / 2;
            var pointsToFit = new Vector2F[filterLength]; // we will create and reuse this one array

            var newDefinedPoints = new List<Vector2>();

            for (int i = 0; i < _definedPoints.Length; i++)
            {
                // we need points from [i - halfLength, i + halfLength]
                for (int j = 0; j < filterLength; j++)
                {
                    var index = i + j - halfLength;
                    if (index < 0) index += _definedPoints.Length;
                    if (index >= _definedPoints.Length) index -= _definedPoints.Length;

                    pointsToFit[j] = (Vector2F)_definedPoints[index];
                }

                var fitFunction = fitter.FitFunction(pointsToFit);
                var smoothedY = fitFunction.Evaluate((float)_definedPoints[i].X);
                newDefinedPoints.Add(new Vector2(_definedPoints[i].X, smoothedY));
            }

            return new PiecewiseFunction(_period, newDefinedPoints);
        }
    }
}
