using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Mathematics.ProfileSpecification
{
    public sealed class AbsolutelyPositionedPointProfileConstraint
        : PointProfileConstraint
    {
        private readonly DiameterConstraint _diameterConstraint;
        private readonly double _minimumPosition;
        private readonly double _maximumPosition;

        public AbsolutelyPositionedPointProfileConstraint(string name, DiameterConstraint diameterConstraint, double minimumPosition, double maximumPosition)
            : base(name)
        {
            if (double.IsNaN(minimumPosition)) throw new ArgumentException();
            if (double.IsNaN(maximumPosition)) throw new ArgumentException();
            if (maximumPosition < minimumPosition) throw new ArgumentOutOfRangeException();
            if (Math.Sign(minimumPosition) == -Math.Sign(maximumPosition)) throw new ArgumentException("Absolute position must definitely lie to one side of the origin or to the other side.");

            _diameterConstraint = diameterConstraint;
            _minimumPosition = minimumPosition;
            _maximumPosition = maximumPosition;
        }

        public double MinimumPosition { get { return _minimumPosition; } }
        public double MaximumPosition { get { return _maximumPosition; } }

        public DiameterConstraint Diameter { get { return _diameterConstraint; } }

        public override string ReasonForIncompatibilityWith(IEnumerable<ProfileConstraint> others)
        {
            return null;
        }
    }
}
