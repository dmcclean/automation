using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Mathematics.Geometry
{
    public sealed class PointCloud2
        : IEnumerable<Vector2>
    {
        private readonly List<Vector2> _points;

        public PointCloud2()
        {
            _points = new List<Vector2>();
        }

        public PointCloud2(IEnumerable<Vector2> points)
        {
            _points = new List<Vector2>(points);
        }

        public Vector2 Centroid 
        {
            get 
            {
                return Vector2.Centroid(_points);
            }
        }

        public Circle2 ComputeMinimumCircumscribingCircle()
        {
            var n = _points.Count;

            if (n == 0) return new Circle2(Vector2.Zero, 0);
            else if (n == 1) return new Circle2(_points[0], 0);
            else if (n == 2)
            {
                var segment = LineSegment2.FromOriginAndDestination(_points[0], _points[1]);

                return new Circle2(segment.Midpoint, segment.Length / 2);
            }
            else if (n == 3) return Circle2.FromThreePoints(_points[0], _points[1], _points[2]);
            else
            {
                // brute force!
                Circle2 incumbent = new Circle2(Vector2.Zero, double.PositiveInfinity);
                double incumbentRadius = double.PositiveInfinity;
                Circle2 incumbentMiss = new Circle2(Vector2.Zero, double.PositiveInfinity);
                double incumbentMissPenalty = double.PositiveInfinity;

                for (int i = 0; i < n - 2; i++)
                {
                    for (int j = i + 1; j < n - 1; j++)
                    {
                        for (int k = j + 1; k < n; k++)
                        {
                            var candidate = Circle2.FromThreePoints(_points[i], _points[j], _points[k]);

                            // the order of these tests is very important because one is much cheaper than the other
                            if (candidate.Radius < incumbentRadius)
                            {
                                if (double.IsInfinity(incumbentRadius))
                                {
                                    // we have not yet found one that includes all points, so we might still need to update the incumbent miss
                                    var candidateMissPenalty = _points.Max(p => candidate.SignedDistanceFromCircle(p));
                                    if (candidateMissPenalty <= 0)
                                    {
                                        // all points pass strict inclusion, this is the new incumbent
                                        incumbent = candidate;
                                        incumbentRadius = candidate.Radius;
                                    }

                                    if (double.IsInfinity(incumbentRadius) && candidateMissPenalty < incumbentMissPenalty)
                                    {
                                        // this one doesn't fit, but it's the best we've seen so far so we might need it if things go poorly numerically
                                        incumbentMiss = candidate;
                                        incumbentMissPenalty = candidateMissPenalty;
                                    }
                                }
                                else
                                {
                                    // since we don't care about updating the miss, we can instead use short-circuiting while evaluating inclusion
                                    if (_points.All(p => candidate.Contains(p)))
                                    {
                                        incumbent = candidate;
                                        incumbentRadius = candidate.Radius;
                                    }
                                }
                            }
                        }
                    }
                }

                if (double.IsInfinity(incumbentRadius))
                {
                    // we didn't find one that numerically included all the points
                    // instead return the closest
                    return incumbentMiss;
                }
                else return incumbent;
            }
        }

        public IEnumerator<Vector2> GetEnumerator()
        {
            return _points.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (System.Collections.IEnumerator)this.GetEnumerator();
        }
    }
}
