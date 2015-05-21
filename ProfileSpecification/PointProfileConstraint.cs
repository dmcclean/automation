using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Mathematics.ProfileSpecification
{
    public abstract class PointProfileConstraint
        : ProfileConstraint
    {
        private readonly string _name;

        protected PointProfileConstraint(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException();
            _name = name;
        }

        public string Name { get { return _name; } }
    }
}
