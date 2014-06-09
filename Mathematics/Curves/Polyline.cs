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
    }
}
