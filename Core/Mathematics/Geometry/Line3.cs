using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Mathematics.Geometry
{
    [Serializable]
    public sealed class Line3
    {
        private readonly Vector3 _point;
        private readonly Vector3 _direction;

        private Line3(Vector3 point, Vector3 direction)
        {
            _point = point;
            _direction = direction;
        }

        public static Line3 FromPointAndDirection(Vector3 point, Vector3 direction)
        {
            Vector3 normalDirection;
            if (!direction.TryNormalize(out normalDirection)) throw new ArgumentException();

            var draft = new Line3(point, normalDirection);
            var nearestOrigin = draft.GetClosestPoint(Vector3.Zero);
            return new Line3(nearestOrigin, normalDirection);
        }

        public Vector3 GetClosestPoint(Vector3 point)
        {
            var c = point - _point;
            var t = Vector3.DotProduct(_direction, c);

            return _point + (t * _direction);
        }

        public double DistanceTo(Vector3 point)
        {
            var closest = GetClosestPoint(point);
            return (point - closest).Length;
        }

        public Vector3 Direction
        {
            get
            {
                return _direction;
            }
        }

        public Vector3 PiercePointOnPlane(Plane3 plane)
        {
            throw new NotImplementedException();
        }
    }
}
