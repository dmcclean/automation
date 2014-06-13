using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Mathematics.Geometry.Voronoi
{
    public class VoronoiEdge
    {
        internal bool Done = false;
        public Vector2 RightData { get; internal set; }
        public Vector2 LeftData { get; internal set; }
        public Vector2 VVertexA { get; internal set; }
        public Vector2 VVertexB { get; internal set; }
        
        public VoronoiEdge() 
        {
            VVertexA = VoronoiDiagram.VVUnkown;
            VVertexB = VoronoiDiagram.VVUnkown;
        }

        internal void AddVertex(Vector2 V)
        {
            if (VVertexA.IsNaN)
                VVertexA = V;
            else if (VVertexB.IsNaN)
                VVertexB = V;
            else throw new Exception("Tried to add third vertex!");
        }

        private static bool IsVVInfinite(Vector2 point)
        {
            return double.IsPositiveInfinity(point.X) && double.IsPositiveInfinity(point.Y);
        }

        public bool IsInfinite
        {
            get { return IsVVInfinite(VVertexA) && IsVVInfinite(VVertexB); }
        }
        public bool IsPartlyInfinite
        {
            get { return IsVVInfinite(VVertexA) || IsVVInfinite(VVertexB); }
        }
        public Vector2 FixedPoint
        {
            get
            {
                if (IsInfinite)
                    return 0.5 * (LeftData + RightData);
                if (IsVVInfinite(VVertexA))
                    return VVertexA;
                return VVertexB;
            }
        }
        public Vector2 DirectionVector
        {
            get
            {
                if (!IsPartlyInfinite)
                    return (VVertexB - VVertexA) * (1.0 / Vector2.DistanceBetweenPoints(VVertexA, VVertexB));
                if (LeftData.X == RightData.X)
                {
                    if (LeftData.Y < RightData.Y)
                        return new Vector2(-1, 0);
                    return new Vector2(1, 0);
                }
                Vector2 Erg = new Vector2(-(RightData.Y - LeftData.Y) / (RightData.X - LeftData.X), 1);
                if (RightData.X < LeftData.X)
                    Erg *= -1;
                Erg = Erg.Normalize();
                return Erg;
            }
        }
        public double Length
        {
            get
            {
                if (IsPartlyInfinite)
                    return double.PositiveInfinity;
                return Vector2.DistanceBetweenPoints(VVertexA, VVertexB);
            }
        }
    }
}
