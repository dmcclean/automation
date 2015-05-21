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

        private enum Shape
        {
            Unknown,
            Convex,
            Concave            
        }

        private struct VisitPoint { public double MinXPosition; public double MaxXPosition; public DiameterConstraint Diameter; public Shape Shape; }

        public List<Vector2> ToleranceBoundary(double parentProfileDiameter)
        {
            // We will make a list of all the "visit points" (encoding min/max positions in both directions, sequence, and shape)
            var visitPoints = new List<VisitPoint>();
            
            // so let's pick out the origin and start there
            var origin = _constraints.Single(c => c is OriginProfileConstraint) as OriginProfileConstraint;

            visitPoints.Add(new VisitPoint { MinXPosition = 0, MaxXPosition = 0, Diameter = origin.Diameter, Shape = Shape.Unknown });

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

                    visitPoints.Add(new VisitPoint { MinXPosition = minX, MaxXPosition = maxX, Diameter = ((AbsolutelyPositionedPointProfileConstraint)point).Diameter, Shape = Shape.Unknown });

                    currentMinX = minX;
                    currentMaxX = maxX;
                }
                else if (point is RelativelyPositionedPointProfileConstraint)
                {
                    var minDelX = ((RelativelyPositionedPointProfileConstraint)point).MinimumRelativePosition;
                    var maxDelX = ((RelativelyPositionedPointProfileConstraint)point).MaximumRelativePosition;

                    var minX = currentMinX + minDelX;
                    var maxX = currentMaxX + maxDelX;

                    var resultingMinX = Math.Max(currentMaxX, minX);

                    visitPoints.Add(new VisitPoint { MinXPosition = resultingMinX, MaxXPosition = maxX, Diameter = ((RelativelyPositionedPointProfileConstraint)point).Diameter, Shape = Shape.Unknown});

                    currentMinX = resultingMinX;
                    currentMaxX = maxX;
                }
                else
                {
                    // ignore between constraints
                }
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

                    visitPoints.Insert(0, new VisitPoint { MinXPosition = minX, MaxXPosition = maxX, Diameter = ((AbsolutelyPositionedPointProfileConstraint)point).Diameter, Shape = Shape.Unknown });

                    currentMinX = minX;
                    currentMaxX = maxX;
                }
                else if (point is RelativelyPositionedPointProfileConstraint)
                {
                    var minDelX = ((RelativelyPositionedPointProfileConstraint)point).MinimumRelativePosition;
                    var maxDelX = ((RelativelyPositionedPointProfileConstraint)point).MaximumRelativePosition;

                    var minX = currentMinX - maxDelX;
                    var maxX = currentMaxX - minDelX;

                    var resultingMaxX = Math.Min(currentMinX, maxX);

                    visitPoints.Insert(0, new VisitPoint { MinXPosition = minX, MaxXPosition = resultingMaxX, Diameter = ((RelativelyPositionedPointProfileConstraint)point).Diameter, Shape = Shape.Unknown });

                    currentMinX = minX;
                    currentMaxX = resultingMaxX;
                }
                else
                {
                    // ignore between constraints
                }
            }

            // Create a list to hold the result
            var result = new List<Vector2>();

            // We have now computed the total list of visit points, ordered by increasing x. Next we compute their convexity.
            // First we will visit them all in forward order, emitting the minimum size tolerance
            var currentMinY = visitPoints[0].Diameter.DrawingMinimumDiameter(parentProfileDiameter);
            for (int i = 0; i < visitPoints.Count; i++)
            {
                var point = visitPoints[i];
                var last = i == (visitPoints.Count - 1);

                var minY = point.Diameter.DrawingMinimumDiameter(parentProfileDiameter);

                bool minWentDown = minY <= currentMinY;
                var bindingX = minWentDown ? point.MinXPosition : point.MaxXPosition;

                result.Add(new Vector2(bindingX, minY));
                if (last && minWentDown) result.Add(new Vector2(point.MaxXPosition, minY));

                currentMinY = minY;
            }

            // We now visit them all in reverse order, emitting the maximum size tolerance
            var currentMaxY = visitPoints[visitPoints.Count - 1].Diameter.DrawingMaximumDiameter(parentProfileDiameter);
            for (int i = visitPoints.Count - 1; i >= 0; i--)
            {
                var point = visitPoints[i];
                var last = i == 0;

                var maxY = point.Diameter.DrawingMaximumDiameter(parentProfileDiameter);

                bool maxWentUp = maxY >= currentMaxY;
                var bindingX = maxWentUp ? point.MaxXPosition : point.MinXPosition;

                result.Add(new Vector2(bindingX, maxY));

                if (last && maxWentUp) result.Add(new Vector2(point.MinXPosition, maxY));

                currentMaxY = maxY;
            }

            return result;
        }
    }
}
