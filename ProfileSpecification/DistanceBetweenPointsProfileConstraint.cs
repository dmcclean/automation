using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Mathematics.ProfileSpecification
{
    public sealed class DistanceBetweenPointsProfileConstraint
        : ProfileConstraint
    {
        private readonly string _pointAName;
        private readonly string _pointBName;
        private readonly double _minimumDistance;
        private readonly double _maximumDistance;

        public DistanceBetweenPointsProfileConstraint(string pointAName, string pointBName, double minimumDistance, double maximumDistance)
        {
            if (string.IsNullOrWhiteSpace(pointAName)) throw new ArgumentException();
            if (string.IsNullOrWhiteSpace(pointBName)) throw new ArgumentException();
            if (pointAName == pointBName) throw new ArgumentException();
            if (double.IsNaN(minimumDistance) || double.IsNaN(maximumDistance)) throw new ArgumentException();
            if (minimumDistance < 0) throw new ArgumentOutOfRangeException();
            if (maximumDistance < minimumDistance) throw new ArgumentOutOfRangeException();

            _pointAName = pointAName;
            _pointBName = pointBName;
            _minimumDistance = minimumDistance;
            _maximumDistance = maximumDistance;
        }

        public override string ReasonForIncompatibilityWith(IEnumerable<ProfileConstraint> others)
        {
            bool foundA = false;
            bool foundB = false;

            foreach (var constraint in others)
            {
                var point = constraint as PointProfileConstraint;

                if (constraint == this) continue;
                if (point != null)
                {
                    if (_pointAName == point.Name) foundA = true;
                    if (_pointAName == point.Name) foundB = true;
                }
            }

            if (!foundA) return string.Format("Unable to locate a point named {0} referred to in a Between constraint.", _pointAName);
            else if (!foundB) return string.Format("Unable to locate a point named {0} referred to in a Between constraint.", _pointBName);
            else return null;

        }
    }
}
