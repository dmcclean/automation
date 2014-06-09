using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomationLibrary.Controllers;

namespace AutomationLibrary.Mathematics
{
    [PlcTypeName("E_RationalFunctionForm")]
    public enum RationalFunctionForm
        : ushort
    {
        PolynomialForm,
        BarycentricForm
    }
}
