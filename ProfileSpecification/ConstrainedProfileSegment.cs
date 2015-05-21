using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomationLibrary.Collections;

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

        private struct VisitPoint { public double MinXPosition; public double MaxXPosition; public DiameterConstraint Diameter; }

        public List<Vector2> ToleranceBoundary(double parentProfileDiameter)
        {
            List<Vector2> result = new List<Vector2>();
            
            // visit order is going to start at the origin and go around clockwise in the ordinary XY plane
            Stack<VisitPoint> visited = new Stack<VisitPoint>();
            
            // so let's pick out the origin and start there
            var origin = _constraints.Single(c => c is OriginProfileConstraint) as OriginProfileConstraint;
            result.Add(new Vector2(0, origin.Diameter.DrawingMinimumDiameter(parentProfileDiameter)));
            visited.Push(new VisitPoint { MinXPosition = 0, MaxXPosition = 0, Diameter = origin.Diameter });

            // now let's go to all the positive points
            var positivePoints = _constraints.After(c => c is OriginProfileConstraint).ToArray();
            var positivePointCount = positivePoints.Count();
            double currentMinX = 0;
            double currentMaxX = 0;
            for (int i = 0; i < positivePointCount; i++)
            {
                var point = positivePoints[i];
                if (point is AbsolutelyPositionedPointProfileConstraint)
                {
                    var minX = ((AbsolutelyPositionedPointProfileConstraint)point).MinimumPosition;
                    var maxX = ((AbsolutelyPositionedPointProfileConstraint)point).MaximumPosition;

                    var minY = ((AbsolutelyPositionedPointProfileConstraint)point).Diameter.DrawingMinimumDiameter(parentProfileDiameter);

                    // on the first one, include both
                    if (i == 0)
                    {
                        result.Add(new Vector2(minX, minY));
                    }
                    result.Add(new Vector2(maxX, minY));

                    visited.Push(new VisitPoint { MinXPosition = minX, MaxXPosition = maxX, Diameter = ((AbsolutelyPositionedPointProfileConstraint)point).Diameter });

                    currentMinX = minX;
                    currentMaxX = maxX;
                }
                else if (point is RelativelyPositionedPointProfileConstraint)
                {
                    var minDelX = ((RelativelyPositionedPointProfileConstraint)point).MinimumRelativePosition;
                    var maxDelX = ((RelativelyPositionedPointProfileConstraint)point).MaximumRelativePosition;

                    var minY = ((RelativelyPositionedPointProfileConstraint)point).Diameter.DrawingMinimumDiameter(parentProfileDiameter);

                    var minX = currentMinX + minDelX;
                    var maxX = currentMaxX + maxDelX;

                    var resultingMinX = Math.Max(currentMaxX, minX);

                    // on the first one, include both
                    if (i == 0)
                    {
                        result.Add(new Vector2(resultingMinX, minY));
                    }
                    result.Add(new Vector2(maxX, minY));

                    visited.Push(new VisitPoint { MinXPosition = resultingMinX, MaxXPosition = maxX, Diameter = ((RelativelyPositionedPointProfileConstraint)point).Diameter });

                    currentMinX = resultingMinX;
                    currentMaxX = maxX;
                }
                else
                {
                    // TODO:
                }
            }

            // now let's dequeue all the positive points (and the origin)
            bool first = true;
            while (visited.Count > 0)
            {
                var popped = visited.Pop();

                if (first)
                {
                    result.Add(new Vector2(popped.MaxXPosition, popped.Diameter.DrawingMaximumDiameter(parentProfileDiameter)));
                    first = false;
                }
                result.Add(new Vector2(popped.MinXPosition, popped.Diameter.DrawingMaximumDiameter(parentProfileDiameter)));
            }

            // now let's go to all the negative points
            var negativePoints = _constraints.Before(c => c is OriginProfileConstraint).Reverse().ToArray();
            var negativePointsCount = negativePoints.Count();
            for(int i = 0; i < negativePointsCount; i++)
            {
                var point = negativePoints[i];
                if (point is AbsolutelyPositionedPointProfileConstraint)
                {
                    var minX = ((AbsolutelyPositionedPointProfileConstraint)point).MinimumPosition;
                    var maxX = ((AbsolutelyPositionedPointProfileConstraint)point).MaximumPosition;

                    var maxY = ((AbsolutelyPositionedPointProfileConstraint)point).Diameter.DrawingMaximumDiameter(parentProfileDiameter);

                    result.Add(new Vector2(maxX, maxY));
                    // on the last one, include both
                    if (i == negativePointsCount - 1)
                    {
                        result.Add(new Vector2(minX, maxY));
                    }

                    visited.Push(new VisitPoint { MinXPosition = minX, MaxXPosition = maxX, Diameter = ((AbsolutelyPositionedPointProfileConstraint)point).Diameter });
                }
                else
                {
                    // TODO:
                }
            }

            // now let's dequeue all the negative points)
            first = true;
            while (visited.Count > 0)
            {
                var popped = visited.Pop();

                if (first)
                {
                    result.Add(new Vector2(popped.MinXPosition, popped.Diameter.DrawingMinimumDiameter(parentProfileDiameter)));
                    first = false;
                }
                result.Add(new Vector2(popped.MaxXPosition, popped.Diameter.DrawingMinimumDiameter(parentProfileDiameter)));
            }

            return result;
        }
    }
}
