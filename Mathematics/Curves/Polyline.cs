using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Mathematics.Curves
{
    public static class Polyline
    {
        public static List<Vector2> Simplify(IList<Vector2> points, double tolerance)
        {
            var x = points.Select(p => p.X).ToArray();
            var y = points.Select(p => p.Y).ToArray();
            var n = x.Length;

            double[] xPrime, yPrime;
            int nSegments;

            alglib.lstfitpiecewiselinearrdp(x, y, n, tolerance, out xPrime, out yPrime, out nSegments);

            if (xPrime == null || yPrime == null || xPrime.Length != nSegments + 1 || yPrime.Length != nSegments + 1) throw new ApplicationException("Unexpected result encountered during polyline simplification.");

            var result = new List<Vector2>(xPrime.Length);
            for (int i = 0; i < xPrime.Length; i++)
            {
                result.Add(new Vector2(xPrime[i], yPrime[i]));
            }

            return result;
        }

        public static List<Vector2> LargestTriangleThreeBuckets(List<Vector2> data, int threshold)
        {
            int dataLength = data.Count;
            if (threshold >= dataLength || threshold == 0)
                return data; // Nothing to do

            var sampled = new List<Vector2>(threshold);

            // Bucket size. Leave room for start and end data points
            double every = (double)(dataLength - 2) / (threshold - 2);

            int a = 0;
            var maxAreaPoint = new Vector2(0, 0);
            int nextA = 0;

            sampled.Add(data[a]); // Always add the first point

            for (int i = 0; i < threshold - 2; i++)
            {
                // Calculate point average for next bucket (containing c)
                double avgX = 0;
                double avgY = 0;
                int avgRangeStart = (int)(Math.Floor((i + 1) * every) + 1);
                int avgRangeEnd = (int)(Math.Floor((i + 2) * every) + 1);
                avgRangeEnd = avgRangeEnd < dataLength ? avgRangeEnd : dataLength;

                int avgRangeLength = avgRangeEnd - avgRangeStart;

                for (; avgRangeStart < avgRangeEnd; avgRangeStart++)
                {
                    avgX += data[avgRangeStart].X; // * 1 enforces Number (value may be Date)
                    avgY += data[avgRangeStart].Y;
                }
                avgX /= avgRangeLength;

                avgY /= avgRangeLength;

                // Get the range for this bucket
                int rangeOffs = (int)(Math.Floor((i + 0) * every) + 1);
                int rangeTo = (int)(Math.Floor((i + 1) * every) + 1);

                // Point a
                double pointAx = data[a].X; // enforce Number (value may be Date)
                double pointAy = data[a].Y;

                double maxArea = -1;

                for (; rangeOffs < rangeTo; rangeOffs++)
                {
                    // Calculate triangle area over three buckets
                    double area = Math.Abs((pointAx - avgX) * (data[rangeOffs].Y - pointAy) -
                                           (pointAx - data[rangeOffs].X) * (avgY - pointAy)
                                      ) * 0.5;
                    if (area > maxArea)
                    {
                        maxArea = area;
                        maxAreaPoint = data[rangeOffs];
                        nextA = rangeOffs; // Next a is this b
                    }
                }

                sampled.Add(maxAreaPoint); // Pick this point from the bucket
                a = nextA; // This a is the next a (chosen b)
            }

            sampled.Add(data[dataLength - 1]); // Always add last

            return sampled;
        }
    }
}
