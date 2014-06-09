using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Mathematics.ProfileSpecification
{
    public sealed class DiameterConstraint
    {
        private readonly DiameterConstraintType _type;
        private readonly double _minimumDiameter;
        private readonly double _maximumDiameter;

        private DiameterConstraint(DiameterConstraintType type, double minimumDiameter, double maximumDiameter)
        {
            _type = type;
            _minimumDiameter = minimumDiameter;
            _maximumDiameter = maximumDiameter;
        }

        public static readonly DiameterConstraint IntersectionWithParentProfile = new DiameterConstraint(DiameterConstraintType.IntersectionWithParentProfile, double.MinValue, double.MaxValue);

        public static DiameterConstraint FromNominalValue(double nominal)
        {
            if (double.IsNaN(nominal) || double.IsInfinity(nominal)) throw new ArgumentException();
            if (nominal < 0) throw new ArgumentOutOfRangeException();

            return new DiameterConstraint(DiameterConstraintType.Nominal, nominal, nominal);
        }

        public static DiameterConstraint FromMinimumAndMaximum(double minimumDiameter, double maximumDiameter)
        {
            if (double.IsInfinity(minimumDiameter) || double.IsNaN(minimumDiameter)) throw new ArgumentException();
            if (minimumDiameter < 0) throw new ArgumentOutOfRangeException();
            if (double.IsNaN(maximumDiameter)) throw new ArgumentException();
            if (maximumDiameter < minimumDiameter) throw new ArgumentOutOfRangeException();

            return new DiameterConstraint(DiameterConstraintType.Limits, minimumDiameter, maximumDiameter);
        }
    }
}
