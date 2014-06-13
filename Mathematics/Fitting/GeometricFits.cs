using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomationLibrary.Mathematics.Geometry;

namespace AutomationLibrary.Mathematics.Fitting
{
    public static class GeometricFits
    {
        public static Circle2 FitCircle(IList<Vector2> points)
        {
            var n = points.Count;
            
            if (n == 3) return Circle2.FromThreePoints(points[0], points[1], points[2]);
            else return FitCircle(points, p => p.X, p => p.Y, p => 1); // equal weights
        }

        public static Circle2 FitCircleToWeightedPoints(IList<Vector3> points)
        {
            return FitCircle(points, p => p.X, p => p.Y, p => p.Z); // weighted by Z values
        }

        private static Circle2 FitCircle<T>(IList<T> points, Func<T, double> xSelector, Func<T, double> ySelector, Func<T, double> wSelector)
        {
            var n = points.Count;
            if (n < 3) throw new ArgumentException("At least three points are required to fit a circle.");

            var x = new double[n, 2];
            var y = new double[n];
            var w = new double[n];

            var minX = double.PositiveInfinity;
            var maxX = double.NegativeInfinity;
            var minY = double.PositiveInfinity;
            var maxY = double.NegativeInfinity;

            for (int i = 0; i < n; i++)
            {
                x[i, 0] = xSelector(points[i]);
                x[i, 1] = ySelector(points[i]);
                y[i] = 0; // this point is ideally 0 distance from the fit circle
                w[i] = wSelector(points[i]);

                minX = Math.Min(minX, x[i,0]);
                maxX = Math.Max(maxX, x[i,0]);
                minY = Math.Min(minY, x[i,1]);
                maxY = Math.Max(maxY, x[i,1]);
            }

            var guessCenterX = (minX + maxX) / 2;
            var guessCenterY = (minY + maxY) / 2;
            var guessRadius = Math.Max(maxX - minX, maxY - minY) / 2;

            var c = new double[] { guessCenterX, guessCenterY, guessRadius };

            alglib.lsfitstate fitState;

            var diffStep = 0.0001; // TODO: not sure how to optimally choose this value

            alglib.lsfitcreatewf(x, y, w, c, diffStep, out fitState);
            alglib.lsfitsetcond(fitState, 0, 0, 0); // choose stopping conditions automatically
            alglib.lsfitfit(fitState, CircleDistanceFunction, null, null);

            int info;
            alglib.lsfitreport report;
            alglib.lsfitresults(fitState, out info, out c, out report);
            /*
             *        Info    -   completion code:
                        * -7    gradient verification failed.
                                See LSFitSetGradientCheck() for more information.
                        *  1    relative function improvement is no more than
                                EpsF.
                        *  2    relative step is no more than EpsX.
                        *  4    gradient norm is no more than EpsG
                        *  5    MaxIts steps was taken
                        *  7    stopping conditions are too stringent,
                                further improvement is impossible
             * 
             */
            if (info <= 0) throw new IllConditionedProblemException();

            return new Circle2(new Vector2(c[0], c[1]), c[2]);
        }

        private static void CircleDistanceFunction(double[] c, double[] x, ref double func, object obj)
        {
            var circle = new Circle2(new Vector2(c[0], c[1]), c[2]);
            var point = new Vector2(x[0], x[1]);
            func = circle.DistanceFromCircle(point);
        }

        public static Line2 FitLine(IList<Vector2> points)
        {
            var n = points.Count;
            if (n < 2) throw new ArgumentException("At least two points are required to fit a line.");

            var x = new double[n, 2];
            var centroid = Vector2.Centroid(points);

            for (int i = 0; i < n; i++)
            {
                x[i, 0] = points[i].X - centroid.X;
                x[i, 1] = points[i].Y - centroid.Y;
            }

            double[] w;
            double[,] u, vt;
            const int uNeeded = 0; // don't need left singular vectors
            const int vtNeeded = 1; // need first set of right singular vectors
            const int extraMemoryAllowedSetting = 2; // it's ok to use more memory for performance
            alglib.rmatrixsvd(x, n, 2, uNeeded, vtNeeded, extraMemoryAllowedSetting, out w, out u, out vt);


            // because alglib.rmatrixsvd promises to return singular values, w, in descending order, we can be sure that w[1] is the least
            const int IndexOfLeastSingularValue = 1;

            var a = vt[IndexOfLeastSingularValue, 0];
            var b = vt[IndexOfLeastSingularValue, 1];

            var normal = new Vector2(a, b);

            var c = Vector2.DotProduct(centroid, normal);

            return new Line2(normal, c);
        }

        public static Plane3 FitPlane(IList<Vector3> points)
        {
            var n = points.Count;
            if (n < 3) throw new ArgumentException("At least three points are required to fit a plane.");

            var x = new double[n, 3];
            var centroid = Vector3.Centroid(points);

            for (int i = 0; i < n; i++)
            {
                x[i, 0] = points[i].X - centroid.X;
                x[i, 1] = points[i].Y - centroid.Y;
                x[i, 2] = points[i].Z - centroid.Z;
            }

            double[] w;
            double[,]  u, vt;
            const int uNeeded = 0; // don't need left singular vectors
            const int vtNeeded = 1; // need first set of right singular vectors
            const int extraMemoryAllowedSetting = 2; // it's ok to use more memory for performance
            alglib.rmatrixsvd(x, n, 3, uNeeded, vtNeeded, extraMemoryAllowedSetting, out w, out u, out vt);


            // because alglib.rmatrixsvd promises to return singular values, w, in descending order, we can be sure that w[2] is the least
            const int IndexOfLeastSingularValue = 2;

            var a = vt[IndexOfLeastSingularValue, 0];
            var b = vt[IndexOfLeastSingularValue, 1];
            var c = vt[IndexOfLeastSingularValue, 2];

            var normal = new Vector3(a, b, c);

            var d = Vector3.DotProduct(centroid, normal);

            return new Plane3(normal, d);
        }
    }
}
