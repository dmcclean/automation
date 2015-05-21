using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Mathematics.ProfileSpecification
{
    public sealed class OriginProfileConstraint
        : PointProfileConstraint
    {
        private readonly DiameterConstraint _diameter;

        public OriginProfileConstraint(string name, DiameterConstraint diameter)
            : base(name)
        {
            _diameter = diameter;
        }

        public DiameterConstraint Diameter { get { return _diameter; } }

        public override string ReasonForIncompatibilityWith(IEnumerable<ProfileConstraint> others)
        {
            foreach (var constraint in others)
            {
                if (constraint == this) continue;
                if(constraint is OriginProfileConstraint) return "Multiple origin points cannot exist in the same profile.";
            }
            return null;
        }
    }
}
