﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Mathematics.Fitting
{
    public sealed class FloaterHormannRationalFunctionFitter
        : IRationalFunctionFitter
    {
        private int _order;

        public FloaterHormannRationalFunctionFitter(int order)
        {
            if (order <= 0) throw new ArgumentOutOfRangeException();

            _order = order;
        }

        public RationalFunction FitFunction(Vector2F[] points)
        {
            alglib.barycentricinterpolant function;
            alglib.barycentricfitreport report;

            var x = (from point in points
                     select (double)point.X).ToArray();

            var y = (from point in points
                     select (double)point.Y).ToArray();

            int resultCode;

            int n = x.Length;

            alglib.barycentricfitfloaterhormann(x, y, n, _order, out resultCode, out function, out report);

            if (resultCode == -4) throw new IllConditionedProblemException();
            if (resultCode < 0) throw new ApplicationException();
            
            return function.ConvertToRationalFunction();
        }
    }
}
