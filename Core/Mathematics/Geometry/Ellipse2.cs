using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Mathematics.Geometry
{
    [Serializable]
    public sealed class Ellipse2
    {
        private readonly Vector2 _center;
        private readonly double _semiMajorAxis;
        private readonly double _semiMinorAxis;
        private readonly double _angleOfSemiMajorAxis;

        public Ellipse2(Vector2 center, double semiMajorAxis, double semiMinorAxis, double angle)
        {
            _center = center;
            _semiMajorAxis = semiMajorAxis;
            _semiMinorAxis = semiMinorAxis;
            _angleOfSemiMajorAxis = angle;
        }
    }
}
