using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Mathematics.ProfileSpecification
{
    public sealed class ConstrainedProfileSegment
    {
        private readonly string _name;
        private readonly double _minimumDatumPosition;
        private readonly double _maximumDatumPosition;
        private readonly List<ProfileConstraint> _constraints;
        private readonly Dictionary<string, PointProfileConstraint> _pointConstraints;

        public ConstrainedProfileSegment(string name, IEnumerable<ProfileConstraint> constraints)
            : this(name, constraints, double.NaN, double.NaN)
        {
        }

        public ConstrainedProfileSegment(string name, IEnumerable<ProfileConstraint> constraints, double minimumDatumPosition, double maximumDatumPosition)
        {
            if (double.IsNaN(minimumDatumPosition) != double.IsNaN(maximumDatumPosition)) throw new ArgumentException();
            else if (!double.IsNaN(minimumDatumPosition))
            {
                if (maximumDatumPosition < minimumDatumPosition) throw new ArgumentException();
            }

            if (constraints == null) throw new ArgumentNullException();

            // name is purely descriptive for user interface
            _name = (name ?? string.Empty).Trim();

            // NaN signals that this is a master profile
            _minimumDatumPosition = minimumDatumPosition;
            _maximumDatumPosition = maximumDatumPosition;

            _constraints = new List<ProfileConstraint>();
            _pointConstraints = new Dictionary<string, PointProfileConstraint>();
            foreach (var constraint in constraints)
            {
                if (constraint == null) throw new ArgumentNullException();

                _constraints.Add(constraint);
                var namedPoint = constraint as PointProfileConstraint;
                if (namedPoint != null) _pointConstraints.Add(namedPoint.Name, namedPoint);
            }

            ValidateConstraints();
        }

        private void ValidateConstraints()
        {
            if (_pointConstraints.Count < 1) throw new Exception("This profile doesn't contain any control points.");
            if (_constraints.Count(c => c is OriginProfileConstraint) < 1) throw new Exception("This profile doesn't contain an origin point.");

            bool haveSeenFirstGlobal = false;
            foreach (var constraint in _constraints)
            {
                if (constraint is PointProfileConstraint)
                {
                    if (haveSeenFirstGlobal) throw new Exception("This profile contains global constraints followed by individual point constraints. All point constraints must precede the first global constraint.");
                }
                else haveSeenFirstGlobal = true;
            }

            foreach (var constraint in _constraints)
            {
                var complaint = constraint.ReasonForIncompatibilityWith(_constraints);
                if (complaint != null) throw new Exception(complaint);
            }
        }

        public bool IsMasterProfile { get { return double.IsNaN(_minimumDatumPosition); } }

        public string Name { get { return _name; } }
    }
}
