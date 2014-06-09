using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Mathematics
{
    public enum InterpolationBehavior
    {
        Linear,
        UseValueOfLesserArgument,
        UseValueOfGreaterArgument,
        UseLesserValue,
        UseGreaterValue,
    }
}
