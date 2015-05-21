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

        private const double NominalTolerance = 0.15;

        public double DrawingMinimumDiameter(double parentProfileIntersectionDiameter)
        {
            switch (_type)
            {
                case DiameterConstraintType.IntersectionWithParentProfile:
                    return parentProfileIntersectionDiameter;
                case DiameterConstraintType.Limits:
                    return _minimumDiameter;
                case DiameterConstraintType.Nominal:
                    return _minimumDiameter * (1 - NominalTolerance);
                default:
                    throw new ApplicationException("Unexpected constraint type.");
            }
        }

        public double DrawingMaximumDiameter(double parentProfileIntersectionDiameter)
        {
            switch (_type)
            {
                case DiameterConstraintType.IntersectionWithParentProfile:
                    return parentProfileIntersectionDiameter;
                case DiameterConstraintType.Limits:
                    return _maximumDiameter;
                case DiameterConstraintType.Nominal:
                    return _maximumDiameter * (1 + NominalTolerance);
                default:
                    throw new ApplicationException("Unexpected constraint type.");

            }
        }
    }
}
