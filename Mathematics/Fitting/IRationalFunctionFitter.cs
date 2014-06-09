using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Mathematics.Fitting
{
    public interface IRationalFunctionFitter
    {
        RationalFunction FitFunction(Vector2F[] points);
    }
}
