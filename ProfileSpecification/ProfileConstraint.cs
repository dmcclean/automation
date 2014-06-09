using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Mathematics.ProfileSpecification
{
    public abstract class ProfileConstraint
    {
        protected ProfileConstraint()
        {
        }

        public abstract string ReasonForIncompatibilityWith(IEnumerable<ProfileConstraint> others);
    }
}
