using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomationLibrary.Collections;
using AutomationLibrary.Mathematics;
using AutomationLibrary.Mathematics.Fitting;
using AutomationLibrary.Mathematics.Geometry;
using AutomationLibrary.Mathematics.ProfileSpecification;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            TestEclipsePipe();
        }

        static void TestEclipsePipe()
        {
            using(var reader = System.IO.File.OpenText(@"M:\Eclipse\Test Runs\data_3.dat"))
            {
                for (int i = 0; i < 8; i++)
                    reader.ReadLine(); // skip header

                var polarPoints = new List<Vector2>();
                while(true)
                {
                    var line = reader.ReadLine();
                    if (line == null) break;
                    var nums = line.Split('\t');
                    var laser = double.Parse(nums[1]);
                    var encoder = (int)double.Parse(nums[2]);

                    const int EncoderPPR = 4000; // encoder PPR is 1000, but it is a 4x subsampling quadrature encoder

                    if (encoder < 32000) encoder -= 65536;  // to allow for rotating wrong way, move things first to account for counter rollover
                    encoder %= EncoderPPR;
                    if (encoder < 0) encoder += EncoderPPR;

                    var theta = 2 * Math.PI * encoder / EncoderPPR;

                    // const double LaserVoltageAtEdgeOfHolder = 1.3612; as measured by 1-2-3 block method
                    const double LaserVoltageAtEdgeOfHolder = 1.37; // as empirically derived by assuming theoretical scale factor and pipe size
                    const double LaserScaleFactorInchesPerVolt = 12.0 / 10.0; // theoretical scale factor from datasheet
                    const double LaserHolderHalfWidth = 2.3125; // theoretical from drawing, part measures perfectly for width

                    var r = ((laser - LaserVoltageAtEdgeOfHolder) * LaserScaleFactorInchesPerVolt) + LaserHolderHalfWidth;

                    polarPoints.Add(new Vector2(theta, r));
                }

                var cartesianPoints = new List<Vector2>(polarPoints.Count);
                foreach (var polar in polarPoints)
                    cartesianPoints.Add(new Vector2(polar.Y * Math.Cos(polar.X), polar.Y * Math.Sin(polar.X)));

                var circle = GeometricFits.FitCircle(cartesianPoints);

                Console.WriteLine("Radius: {0:f3}", circle.Radius);
                Console.WriteLine("Diameter: {0:f3}", circle.Diameter);
                Console.WriteLine("Center X: {0:f3}", circle.Center.X);
                Console.WriteLine("Center Y: {0:f3}", circle.Center.Y);
                Console.WriteLine("Center Offset: {0:f3}", circle.Center.Length);
                Console.ReadLine();

                var ellipse = GeometricFits.FitEllipse(cartesianPoints);

                Console.WriteLine("Elliptical Major Diameter: {0:f3}", ellipse.MajorAxisLength);
                Console.WriteLine("Elliptical Minor Diameter: {0:f3}", ellipse.MinorAxisLength);
                Console.WriteLine("Elliptical Eccentricity: {0:f4}", ellipse.Eccentricity);

                Console.ReadLine();
            }
        }

        static void ClowWater() 
        {
            List<Vector3> p3 = new List<Vector3>();
            p3.Add(new Vector3(1.1, 0.9, 1.0));
            p3.Add(new Vector3(6.9, 7.1, 7.0));

            var line3fit = AutomationLibrary.Mathematics.Fitting.GeometricFits.FitLine(p3);

            var line3 = Line3.FromPointAndDirection(new Vector3(0.5, 0.5, 0.5), new Vector3(1, 1, 1));
            var nearest = line3.GetClosestPoint(new Vector3(27, -1.4, 19));

            List<Vector2> points = new List<Vector2>();

            using (var reader = System.IO.File.OpenText(@"C:\Users\douglas\desktop\pipe.csv"))
            {
                reader.ReadLine(); // skip header
                while (true)
                {
                    var line = reader.ReadLine();
                    if (line == null) break;
                    var nums = line.Split(',');
                    var values = nums.Select(n => double.Parse(n)).ToArray();
                    points.Add(new Vector2(values[0], values[1]));
                }
            }

            var ellipse = AutomationLibrary.Mathematics.Fitting.GeometricFits.FitEllipse(points);

            var ellipseFunc = AutomationLibrary.Mathematics.Curves.CircularFunction.FromCartesianPoints(ellipse.Center, points);
            var smoothEllipseFunc = ellipseFunc.SavitzkyGolaySmooth(3, 21);


            points.Clear();
            points.AddRange(GeneratePointsOnEllipticalArc(new Vector2(0.37, -2.4), 21, 24.26, 96.3 * Math.PI / 180.0, .020, -95.0 * Math.PI / 180.0, 97.0 * Math.PI / 180.0).Take(1000));


            var pointSet = new PointCloud2(points);

            var voronoi = AutomationLibrary.Mathematics.Geometry.Voronoi.VoronoiDiagram.ComputeForPoints(points);
            voronoi = voronoi.Filter(0); // build map of nearest points

            var centersOfInfiniteCells = new HashSet<Vector2>();
            foreach (var edge in voronoi.Edges)
            {
                if (edge.IsPartlyInfinite)
                {
                    centersOfInfiniteCells.Add(edge.LeftData);
                    centersOfInfiniteCells.Add(edge.RightData);
                }
            }

            var pointSet2 = new PointCloud2(centersOfInfiniteCells);

            var mcc = pointSet2.ComputeMinimumCircumscribingCircle();
            var mic = ComputeMaximumInscribedCircle(pointSet, voronoi, mcc);
            var lsc = GeometricFits.FitCircle(points);

            using (var writer = System.IO.File.CreateText(@"C:\users\douglas\desktop\circlepoints.csv"))
            {
                writer.WriteLine("X,Y");
                foreach (var point in pointSet)
                {
                    writer.WriteLine("{0},{1}", point.X, point.Y);
                }
            }

            Console.WriteLine("n = {0}", points.Count);
            Console.WriteLine("MIC @ ({0}), r = {1}", mic.Center, mic.Radius);
            Console.WriteLine("LSC @ ({0}), r = {1}", lsc.Center, lsc.Radius);
            Console.WriteLine("MCC @ ({0}), r = {1}", mcc.Center, mcc.Radius);

            Console.WriteLine();

            Console.WriteLine("draw.circle({0}, {1}, {2}, border='{3}')", mic.Center.X, mic.Center.Y, mic.Radius, "red");
            Console.WriteLine("draw.circle({0}, {1}, {2}, border='{3}')", lsc.Center.X, lsc.Center.Y, lsc.Radius, "blue");
            Console.WriteLine("draw.circle({0}, {1}, {2}, border='{3}')", mcc.Center.X, mcc.Center.Y, mcc.Radius, "green");

            Console.ReadLine();
        }

        private static IEnumerable<Vector2> GeneratePointsOnEllipticalArc(Vector2 center, double a, double b, double phi, double noise, double minT, double maxT)
        {
            // requires: a > 0, b > 0, 0 <= phi <= 2 pi, 0 <= minT < maxT <= 2 pi, noise > 0
            var r = new Random();

            while (true)
            {
                var t = minT + (r.NextDouble() * (maxT - minT));
                var noiseX = (r.NextDouble() - 0.5) * 2 * noise;
                var noiseY = (r.NextDouble() - 0.5) * 2 * noise;
                var noiseVec = new Vector2(noiseX, noiseY);

                var x = center.X + a * Math.Cos(t) * Math.Cos(phi) - b * Math.Sin(t) * Math.Sin(phi);
                var y = center.Y + a * Math.Cos(t) * Math.Sin(phi) + b * Math.Sin(t) * Math.Cos(phi);

                var noiseless = new Vector2(x, y);

                yield return noiseless + noiseVec;
            }
        }

        private static Circle2 ComputeMaximumInscribedCircle(PointCloud2 points, AutomationLibrary.Mathematics.Geometry.Voronoi.VoronoiDiagram voronoi, Circle2 mcc)
        {
            var candidateCenters = new List<Vector2>();
            candidateCenters.AddRange(voronoi.Vertices.Keys);
            // TODO: add intersections between voronoi edges and convex hull of points

            Circle2 incumbent = new Circle2(points.First(), 0);

            foreach (var candidate in candidateCenters)
            {
                foreach (var neighbor in voronoi.Vertices[candidate])
                {
                    var candidateRadius = Vector2.DistanceBetweenPoints(candidate, neighbor);

                    if (candidateRadius > incumbent.Radius && mcc.Contains(candidate))
                    {
                        incumbent = new Circle2(candidate, candidateRadius);
                    }
                }
            }

            return incumbent;
        }

        static void ProfileTest() 
        {
            var spec = PartSpecification.Parse(@"F:\ExampleGuideWireProfileSpec.xml");




            List<Vector2> points = new List<Vector2>();

            using (var reader = System.IO.File.OpenText(@"F:\wire-profile.csv"))
            {
                reader.ReadLine(); // skip header
                
                while (true)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) break;

                    var segments = line.Split(',');

                    double x, y;
                    if (!double.TryParse(segments[0], out x)) throw new ApplicationException();
                    if (!double.TryParse(segments[1], out y)) throw new ApplicationException();

                    points.Add(new Vector2(x, y));
                }
            }

            var simplified = AutomationLibrary.Mathematics.Curves.Polyline.Simplify(points, 0.0003);

            var fitLines = new List<Tuple<double,double,Line2>>();

            for (int i = 0; i < simplified.Count - 1; i++)
            {
                var start = simplified[i];
                var end = simplified[i + 1];

                //if ((end.X - start.X) < .2) break;

                var segPoints = SelectPointsBetween(points, start.X, end.X);
                var segRoughLine = GeometricFits.FitLine(segPoints);
                var segFineLine = segRoughLine;

                var n = segPoints.Count;

                if (n > 20)
                {
                    var skip = n / 20;
                    var residuals = from p in segPoints
                                    select Tuple.Create(segRoughLine.DistanceFromLine(p), p);
                    var residualsNotNearEnds = residuals.Skip(skip).Take(n - 2 * skip);
                    var nonOutliers = from r in residualsNotNearEnds
                                      orderby r.Item1 descending
                                      select r.Item2;
                    var nonOutlierPoints = nonOutliers.Skip(skip).ToArray();

                    segFineLine = GeometricFits.FitLine(nonOutlierPoints);
                }

                fitLines.Add(Tuple.Create(start.X, end.X, segFineLine));
            }

            fitLines.RemoveAt(12);
            fitLines.RemoveAt(7);

            using (var writer = System.IO.File.CreateText(@"F:\profile-fit-lines.csv"))
            {
                writer.WriteLine("X,Y");
                writer.WriteLine("{0},{1}", fitLines[0].Item1, fitLines[0].Item3.Intercept + fitLines[0].Item3.Slope * fitLines[0].Item1);

                for (int i = 0; i < fitLines.Count - 1; i++)
                {
                    var cross = Line2.Intersection(fitLines[i].Item3, fitLines[i + 1].Item3);
                    if (cross.HasValue)
                    {
                        writer.WriteLine("{0},{1}", cross.Value.X, cross.Value.Y);
                    }
                    else throw new Exception();
                }

                var lastIdx = fitLines.Count - 1;
                writer.WriteLine("{0},{1}", fitLines[lastIdx].Item2, fitLines[lastIdx].Item3.Intercept + fitLines[lastIdx].Item3.Slope * fitLines[lastIdx].Item2);
            }

            Console.WriteLine("Profile processed.");
            Console.WriteLine();

            Console.ReadLine();
        }

        static List<Vector2> SelectPointsBetween(IList<Vector2> points, double left, double right)
        {
            return (from p in points
                    where (p.X >= left) && (p.X <= right)
                    select p).ToList();
        }

        static void PlaneFittingTest()
        {
            var points = new List<Vector3>();

            var actualNormal = new Vector3(2, 7, -4).Normalize();
            var actualDistanceFromOrigin = 417.3;

            Console.WriteLine("Actual: {0}", actualNormal);
            Console.WriteLine("Actual: {0}", actualDistanceFromOrigin);
            Console.WriteLine();

            var rand = new Random();

            var planeCenterPoint = actualNormal * actualDistanceFromOrigin;
            var yAxis = Vector3.CrossProduct(Vector3.UnitX, actualNormal).Normalize();
            var xAxis = Vector3.CrossProduct(yAxis, actualNormal).Normalize();


            for (int i = 0; i < 50000; i++)
            {
                var xOnPlane = rand.NextDouble() * 50 - 25;
                var yOnPlane = rand.NextDouble() * 50 - 25;

                var noiseOffPlane = rand.NextDouble() * 10 - 5;

                var point = planeCenterPoint + (xOnPlane * xAxis) + (yOnPlane * yAxis) + (noiseOffPlane * actualNormal);
                points.Add(point);
            }
            

            var plane = GeometricFits.FitPlane(points);

            Console.WriteLine("Fit: {0}", plane.Normal);
            Console.WriteLine("Fit: {0}", plane.SignedDistanceFromPlane(Vector3.Zero));
        }
    }
}
