﻿using System;
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

        public static Circle2 FitCircleOfKnownRadius(double radius, IList<Vector2> points)
        {
            return FitCircleOfKnownRadius(radius, points, p => p.X, p => p.Y, p => 1);
        }

        public static Circle2 FitCircleOfKnownRadiusToWeightedPoints(double radius, IList<Vector3> points)
        {
            return FitCircleOfKnownRadius(radius, points, p => p.X, p => p.Y, p => p.Z);
        }

        private static Circle2 FitCircleOfKnownRadius<T>(double radius, IList<T> points, Func<T, double> xSelector, Func<T, double> ySelector, Func<T, double> wSelector)
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

                minX = Math.Min(minX, x[i, 0]);
                maxX = Math.Max(maxX, x[i, 0]);
                minY = Math.Min(minY, x[i, 1]);
                maxY = Math.Max(maxY, x[i, 1]);
            }

            var guessCenterX = (minX + maxX) / 2;
            var guessCenterY = (minY + maxY) / 2;

            var c = new double[] { guessCenterX, guessCenterY };

            alglib.lsfitstate fitState;

            var diffStep = 0.0001; // TODO: not sure how to optimally choose this value

            var distanceFunction = CreateCircleOfKnownRadiusDistanceFunction(radius);

            alglib.lsfitcreatewf(x, y, w, c, diffStep, out fitState);
            alglib.lsfitsetcond(fitState, 0, 0, 0); // choose stopping conditions automatically
            alglib.lsfitfit(fitState, distanceFunction, null, null);

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

            return new Circle2(new Vector2(c[0], c[1]), radius);
        }

        private static Circle2 FitCircle<T>(IList<T> points, Func<T, double> xSelector, Func<T, double> ySelector, Func<T, double> wSelector)
        {
            var n = points.Count;
            if (n < 3) return null; // At least three points are required to fit a circle

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

            var cx = c[0];
            var cy = c[1];
            var r = c[2];

            if (r <= 0) throw new IllConditionedProblemException(); // negative radius

            return new Circle2(new Vector2(cx, cy), r);
        }

        private static void CircleDistanceFunction(double[] c, double[] x, ref double func, object obj)
        {
            var cx = c[0];
            var cy = c[1];
            var r = c[2];

            if (r < 0)
            {
                func = double.PositiveInfinity; // negative radius
            }
            else
            {
                var circle = new Circle2(new Vector2(c[0], c[1]), c[2]);
                var point = new Vector2(x[0], x[1]);
                func = circle.DistanceFromCircle(point);
            }
        }

        private static alglib.ndimensional_pfunc CreateCircleOfKnownRadiusDistanceFunction(double radius)
        {
            return delegate(double[] c, double[] x, ref double func, object obj)
            {
                var circle = new Circle2(new Vector2(c[0], c[1]), radius);
                var point = new Vector2(x[0], x[1]);
                func = circle.DistanceFromCircle(point);
            };
        }

        private delegate void OptimizableFunction(double[] c, double[] x, ref double func, object obj);

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

        public static Ellipse2 FitEllipse(IList<Vector2> points)
        {
            int numPoints = points.Count;

            var D1 = new double[numPoints, 3];
            var D2 = new double[numPoints, 3];
            var S1 = new double[3, 3];
            var S2 = new double[3, 3];
            var S3 = new double[3, 3];
            var T = new double[3, 3];
            var M = new double[3, 3];
            var M2 = new double[3, 3];
            var C1 = new double[3, 3];
            var a1 = new double[3, 1];
            var a2 = new double[3, 1];

            C1[0, 0] = 0;
            C1[0, 1] = 0;
            C1[0, 2] = 0.5;
            C1[1, 0] = 0;
            C1[1, 1] = -1;
            C1[1, 2] = 0;
            C1[2, 0] = 0.5;
            C1[2, 1] = 0;
            C1[2, 2] = 0;

            //2 D1 = [x .ˆ 2, x .* y, y .ˆ 2]; % quadratic part of the design matrix
            //3 D2 = [x, y, ones(size(x))]; % linear part of the design matrix
            for (int xx = 0; xx < points.Count; xx++)
            {
                var p = points[xx];
                D1[xx, 0] = p.X * p.X;
                D1[xx, 1] = p.X * p.Y;
                D1[xx, 2] = p.Y * p.Y;

                D2[xx, 0] = p.X;
                D2[xx, 1] = p.Y;
                D2[xx, 2] = 1;
            }

            var opTypeNone = 0;
            var opTypeTranspose = 1;

            //4 S1 = D1’ * D1; % quadratic part of the scatter matrix
            //temp = D1.Transpose() * D1;
            var temp = new double[3, 3];
            alglib.rmatrixgemm(3, 3, numPoints, 1, D1, 0, 0, opTypeTranspose, D1, 0, 0, opTypeNone, 0, ref temp, 0, 0);
            for (int xx = 0; xx < 3; xx++)
                for (int yy = 0; yy < 3; yy++)
                    S1[xx, yy] = temp[xx, yy];

            //5 S2 = D1’ * D2; % combined part of the scatter matrix
            //temp = D1.Transpose() * D2;
            alglib.rmatrixgemm(3, 3, numPoints, 1, D1, 0, 0, opTypeTranspose, D2, 0, 0, opTypeNone, 0, ref temp, 0, 0);
            for (int xx = 0; xx < 3; xx++)
                for (int yy = 0; yy < 3; yy++)
                    S2[xx, yy] = temp[xx, yy];

            //6 S3 = D2’ * D2; % linear part of the scatter matrix
            //temp = D2.Transpose() * D2;
            alglib.rmatrixgemm(3, 3, numPoints, 1, D2, 0, 0, opTypeTranspose, D2, 0, 0, opTypeNone, 0, ref temp, 0, 0);
            for (int xx = 0; xx < 3; xx++)
                for (int yy = 0; yy < 3; yy++)
                    S3[xx, yy] = temp[xx, yy];

            //7 T = - inv(S3) * S2’; % for getting a2 from a1
            // T = -1 * S3.Inverse() * S2.Transpose();
            int info;
            alglib.matinvreport report;
            alglib.rmatrixinverse(ref S3, out info, out report);

            alglib.rmatrixgemm(3, 3, 3, -1, S3, 0, 0, opTypeNone, S2, 0, 0, opTypeTranspose, 0, ref T, 0, 0);

            //8 M = S1 + S2 * T; % reduced scatter matrix
            // M = S1 + S2 * T;
            alglib.rmatrixgemm(3, 3, 3, 1, S2, 0, 0, opTypeNone, T, 0, 0, opTypeNone, 0, ref temp, 0, 0);
            for (int xx = 0; xx < 3; xx++)
                for (int yy = 0; yy < 3; yy++)
                    M[xx, yy] = S1[xx, yy] + temp[xx, yy];

            //9 M = [M(3, <span class="wp-smiley emoji emoji-smile" title=":)">:)</span> ./ 2; - M(2, <span class="wp-smiley emoji emoji-smile" title=":)">:)</span>; M(1, <span class="wp-smiley emoji emoji-smile" title=":)">:)</span> ./ 2]; % premultiply by inv(C1)
            //M2 = C1 * M;
            alglib.rmatrixgemm(3, 3, 3, 1, C1, 0, 0, opTypeNone, M, 0, 0, opTypeNone, 0, ref M2, 0, 0);

            //10 [evec, eval] = eig(M2); % solve eigensystem
            //ComplexEigensystem eigenSystem = M2.Eigensystem();
            const int RightEigenvectorsNeeded = 1;
            double[] wr, wi;
            double[,] vl, vr;
            var converged = alglib.rmatrixevd(M2, 3, RightEigenvectorsNeeded, out wr, out wi, out vl, out vr);

            //11 cond = 4 * evec(1, <span class="wp-smiley emoji emoji-smile" title=":)">:)</span> .* evec(3, <span class="wp-smiley emoji emoji-smile" title=":)">:)</span> - evec(2, <span class="wp-smiley emoji emoji-smile" title=":)">:)</span> .ˆ 2; % evaluate a’Ca
            //12 a1 = evec(:, find(cond > 0)); % eigenvector for min. pos. eigenvalue
            //for (int xx = 0; xx < eigenSystem.Dimension; xx++)
            //{
            //    Vector<Complex> vector = eigenSystem.Eigenvector(xx);
            //    Complex condition = 4 * vector[0] * vector[2] - vector[1] * vector[1];
            //    if (condition.Im == 0 && condition.Re > 0)
            //    {
            //        // Solution is found
            //        Console.WriteLine("\nSolution Found!");
            //        for (int yy = 0; yy < vector.Count(); yy++)
            //        {
            //            Console.Write("{0}, ", vector[yy]);
            //            a1[yy, 0] = vector[yy].Re;
            //        }
            //    }
            //}
            var incumbentCondition = double.PositiveInfinity;
            for (int xx = 0; xx < wr.Length; xx++)
            {
                var vec = vr;

                var condition = 4 * vec[0, xx] * vec[2, xx] - vec[1, xx] * vec[1, xx];
                if (condition > 0 && condition < incumbentCondition)
                {
                    incumbentCondition = condition;

                    // solution is found
                    for (int yy = 0; yy < vec.GetLength(1); yy++)
                    {
                        a1[yy, 0] = vec[yy, xx];
                    }
                }
            }

            if (double.IsInfinity(incumbentCondition)) return null; // no solution found

            // normalize a1
            var a1vec = new Vector3(a1[0, 0], a1[1, 0], a1[2, 0]).Normalize();
            a1[0, 0] = a1vec.X;
            a1[1, 0] = a1vec.Y;
            a1[2, 0] = a1vec.Z;

            //13 a2 = T * a1; % ellipse coefficients
            //a2 = T * a1;
            alglib.rmatrixgemm(3, 1, 3, 1, T, 0, 0, opTypeNone, a1, 0, 0, opTypeNone, 0, ref a2, 0, 0);

            //14 a = [a1; a2]; % ellipse coefficients
            var f = new double[6];
            f[0] = a1[0, 0];
            f[1] = a1[1, 0];
            f[2] = a1[2, 0];
            f[3] = a2[0, 0];
            f[4] = a2[1, 0];
            f[5] = a2[2, 0];

            var equation = string.Format("{0:f6} * x^2 + {1:f6} * x * y + {2:f6} * y^2 + {3:f6} * x + {4:f6} * y + {5:f6} = 0", f[0], f[1], f[2], f[3], f[4], f[5]);

            // calculate the ellipse parameters after https://stat.ethz.ch/pipermail/r-help/2010-September/254560.html
            var b2 = (f[1]*f[1]) / 4;
            var cx = (f[2] * f[3] / 2 - b2 * f[4]) / (b2 - f[0] * f[2]);
            var cy = (f[0] * f[4] / 2 - f[1] * f[3] / 4) / (b2 - f[0] * f[2]);

            //num <- 2 * (f[1] * f[5]^2 / 4 + f[3] * f[4]^2 / 4 + f[6] * b2 - f[2]*f[4]*f[5]/4 - f[1]*f[3]*f[6])
            var num = 2 * (f[0] * f[4] * f[4] / 4 + f[2] * f[3] * f[3] / 4 + f[5] * b2 - f[1] * f[3] * f[4] / 4 - f[0] * f[2] * f[5]);

            //den1 <- (b2 - f[1]*f[3])
            var den1 = (b2 - f[0] * f[2]);
            //den2 <- sqrt((f[1] - f[3])^2 + 4*b2)
            var den2 = Math.Sqrt((f[0] - f[2]) * (f[0] - f[2]) + 4 * b2);
            //den3 <- f[1] + f[3]
            var den3 = f[0] + f[2];

            var semiMajor = Math.Sqrt(num / (den1 * (den2 - den3)));
            var semiMinor = Math.Sqrt(num / (den1 * (-den2 - den3)));
            
            // calculate angle of rotation
            // term <- (f[1] - f[3]) / f[2]
            var term = (f[0] - f[2]) / f[1];
            // angle <- atan(1 / term) / 2
            var angle = Math.Atan(1 / term) / 2;

            if (double.IsNaN(cx) || double.IsNaN(cy) || double.IsNaN(semiMajor) || double.IsNaN(semiMinor) || double.IsNaN(angle)) return null;
            if (semiMinor == 0 || semiMajor == 0) return null;

            // convert angle to our convention
            angle += Math.PI / 2;

            return new Ellipse2(new Vector2(cx, cy), semiMajor, semiMinor, angle);
        }
    }
}
