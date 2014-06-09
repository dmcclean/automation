using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomationLibrary.Controllers;
using System.Runtime.InteropServices;

namespace AutomationLibrary.Mathematics
{
    [PlcTypeName("ST_RationalFunction")]
    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public struct RationalFunction
    {
        private const int MaxOrder = 32;

        private RationalFunctionForm FunctionForm;
        private ushort Order;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MaxOrder)]
        private float[] W;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MaxOrder)]
        private float[] X;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MaxOrder)]
        private float[] Y;
        private float SY;

        public static RationalFunction BuildBarycentric(float[] w, float[] x, float[] y, float sy)
        {
            if (w == null || x == null || y == null) throw new ArgumentNullException();
            if (w.Length != x.Length || x.Length != y.Length) throw new ArgumentException("Mismatched array lengths.");
            if (!IsSorted(x)) throw new ArgumentException("Points are not sorted by x coordinate.");
            if (!w.All(wi => Math.Abs(wi) <= 1)) throw new ArgumentException("Weight values are not normalized to -1 to 1 range.");
            if (!y.All(yi => Math.Abs(yi) <= 1)) throw new ArgumentException("Y values are not normalized to -1 to 1 range.");

            var result = new RationalFunction();
            result.FunctionForm = RationalFunctionForm.BarycentricForm;
            result.Order = (ushort)w.Length;
            result.W = new float[MaxOrder];
            w.CopyTo(result.W, 0);
            result.X = new float[MaxOrder];
            x.CopyTo(result.X, 0);
            result.Y = new float[MaxOrder];
            y.CopyTo(result.Y, 0);
            result.SY = sy;

            return result;
        }

        private static bool IsSorted(float[] x)
        {
            if (x.Length <= 1) return true;
            var current = x[0];
            for (int i = 1; i < x.Length; i++)
            {
                if (current > x[i]) return false;
                current = x[i];
            }
            return true;
        }

        public static RationalFunction BuildPolynomial(params float[] coefficients)
        {
            if (coefficients == null) throw new ArgumentNullException();
            if (coefficients.Length > MaxOrder) throw new ArgumentException("Maximum supported order exceeded.");

            var result = new RationalFunction();
            result.FunctionForm = RationalFunctionForm.PolynomialForm;
            result.Order = (ushort)coefficients.Length;
            result.W = new float[MaxOrder];
            coefficients.CopyTo(result.W, 0);
            result.SY = 1.0f;

            return result;
        }

        public float Evaluate(float argument)
        {
            switch (FunctionForm)
            {
                case RationalFunctionForm.PolynomialForm:
                    return EvaluatePolynomial(argument);
                case RationalFunctionForm.BarycentricForm:
                    return EvaluateBarycentric(argument);
                default:
                    return float.NaN;
            }
        }

        private float EvaluatePolynomial(float argument)
        {
            float result = 0;
            for (int i = Order - 1; i >= 0; i--)
            {
                result *= argument;
                result += this.W[i];
            }
            return result;
        }

        private float EvaluateBarycentric(float argument)
        {
            float result = 0;
            float s1 = 0;
            float s2 = 0;
            float s = 0;
            float v = 0;
            int i = 0;

            //
            // special case: NaN
            //
            if (float.IsNaN(argument))
            {
                result = float.NaN;
                return result;
            }

            //
            // special case: N=1
            //
            if (this.Order == 1)
            {
                result = this.SY * this.Y[0];
                return result;
            }

            //
            // Here we assume that task is normalized, i.e.:
            // 1. abs(Y[i])<=1
            // 2. abs(W[i])<=1
            // 3. X[] is ordered
            //
            s = Math.Abs(argument - this.X[0]);
            for (i = 0; i <= this.Order - 1; i++)
            {
                v = this.X[i];
                if ((double)(v) == (double)(argument))
                {
                    result = this.SY * this.Y[i];
                    return result;
                }
                v = Math.Abs(argument - v);
                if ((double)(v) < (double)(s))
                {
                    s = v;
                }
            }
            s1 = 0;
            s2 = 0;
            for (i = 0; i <= this.Order - 1; i++)
            {
                v = s / (argument - this.X[i]);
                v = v * this.W[i];
                s1 = s1 + v * this.Y[i];
                s2 = s2 + v;
            }
            result = this.SY * s1 / s2;
            return result;
        }
    }
}
