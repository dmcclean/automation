using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomationLibrary.Mathematics.Fitting;

namespace AutomationLibrary.Mathematics.Curves
{
    public sealed class CircularFunction
    {
        private readonly PiecewiseFunction _function;

        private CircularFunction(PiecewiseFunction pointsInPolarCoordinates)
        {
            _function = pointsInPolarCoordinates;
        }

        public static CircularFunction FromCartesianPoints(IEnumerable<Vector2> points)
        {
            // convert to polar coordinates
            var polarPoints = from p in points
                              select new Vector2(p.AngleFromXAxis, p.Length);

            var function = new PiecewiseFunction(2 * Math.PI, polarPoints);

            return new CircularFunction(function);
        }

        public static CircularFunction FromCartesianPoints(Vector2 center, IEnumerable<Vector2> points)
        {
            // remove the center point
            return FromCartesianPoints(points.Select(p => p - center));
        }

        public CircularFunction SavitzkyGolaySmooth(int filterOrder, int filterLength)
        {
            var function = _function.SavitzkyGolaySmooth(filterOrder, filterLength, n => new PolynomialFunctionFitter(n));

            return new CircularFunction(function);
        }

        public double EvaluateRadiusAtAngle(double angleRadians)
        {
            return _function.Evaluate(angleRadians);
        }

        public double ComputeCircumference()
        {
            return ComputeCircumference(offsetOutwards: 0.0);
        }

        public double ComputeCircumference(double offsetOutwards)
        {
            double thetaStep = 0.01;
            
            var lastPointPolar = new Vector2(0, _function.Evaluate(0));
            var lastPointCartesian = Vector2.FromRadiusAndAngle(radius: lastPointPolar.Y + offsetOutwards, angleRadians: lastPointPolar.X);
            var firstPointCartesian = lastPointCartesian; // save the first one so that we can close the curve

            var result = 0.0;
            for (double theta = thetaStep; theta < 2 * Math.PI; theta += thetaStep)
            {
                var currentPointPolar = new Vector2(theta, _function.Evaluate(theta));
                var currentPointCartesian = Vector2.FromRadiusAndAngle(radius: currentPointPolar.Y + offsetOutwards, angleRadians: currentPointPolar.X);

                var distance = Vector2.DistanceBetweenPoints(lastPointCartesian, currentPointCartesian);
                result += distance;

                lastPointPolar = currentPointPolar;
                lastPointCartesian = currentPointCartesian;
            }

            // close curve back to initial point in case of discontinuity
            var closeDistance = Vector2.DistanceBetweenPoints(lastPointCartesian, firstPointCartesian);
            result += closeDistance;

            return result;
        }
    }
}
