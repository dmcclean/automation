using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Mathematics
{
    public static class AlgLibHelpers
    {
        public static RationalFunction ConvertToRationalFunction(this alglib.barycentricinterpolant function)
        {
            if (function == null) throw new ArgumentNullException();

            var n = function.innerobj.n;

            if (n <= 2)
            {
                // linear function, let's convert it to that form for simplicity
                double[] coefficients;
                alglib.polynomialbar2pow(function, out coefficients);

                return RationalFunction.BuildPolynomial(coefficients.Select(x => (float)x).ToArray());
            }
            else
            {
                // maintain barycentric representation
                var w = ConvertToFloatArray(function.innerobj.w, n);
                var x = ConvertToFloatArray(function.innerobj.x, n);
                var y = ConvertToFloatArray(function.innerobj.y, n);

                return RationalFunction.BuildBarycentric(w, x, y, (float)function.innerobj.sy);
            }
        }

        private static float[] ConvertToFloatArray(double[] array, int length)
        {
            return array.Take(length).Select(x => (float)x).ToArray();
        }
    }
}
