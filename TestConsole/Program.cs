﻿using System;
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
            ProfileTest();
            //TestClick();
            //TestEclipsePipe();
        }

        static void TestClick()
        {
            var factory = new AutomationLibrary.Controllers.Modbus.ModbusControllerConnectionFactory(AutomationLibrary.Controllers.Modbus.ModbusSerialProtocol.RTU, new AutomationLibrary.Controllers.Modbus.SlaveAddress(1), "com12", 38400, System.IO.Ports.Parity.Odd, 8, System.IO.Ports.StopBits.One);
            var conn = factory.Connect(TimeSpan.FromSeconds(3));

            var clickSpace = new MassBayEngineering.Interop.AutomationDirect.ClickDeclarationSpace(conn.RootNamespace);

            var hand = clickSpace.X(101);
            var greenLight = clickSpace.Y(105);
            var aCoil = clickSpace.C(7);
            var aTimer = clickSpace.T(103);
            var aTimerValue = clickSpace.TD(103);
            var aChar = clickSpace.TXT(37);
            var aFloat = clickSpace.DF(300);
            var anInt = clickSpace.DD(300);

            bool state = greenLight.Read();
            while (true)
            {
                Console.ReadLine();
                state = !state;
                greenLight.SynchronousWrite(state);

                Console.WriteLine("C7: {0}", aCoil.Read());
                Console.WriteLine("T103: {0}", aTimer.Read());
                Console.WriteLine("TD103: {0}", aTimerValue.Read());
                Console.WriteLine("TXT37: {0}", aChar.Read());
                Console.WriteLine("DF300: {0}", aFloat.Read());
                Console.WriteLine("DD300: {0}", anInt.Read());

                aChar.SynchronousWrite('$');
            }
        }

        static void TestEclipsePipe()
        {
            using(var reader = System.IO.File.OpenText(@"M:\Superior Energy\Test Runs\data_4.dat"))
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

                    if (encoder < 32000) encoder += 65536;  // to allow for rotating wrong way, move things first to account for counter rollover
                    encoder %= EncoderPPR;
                    if (encoder < 0) encoder += EncoderPPR;

                    var theta = (2 * Math.PI * encoder) / EncoderPPR;

                    //const double LaserVoltageAtEdgeOfHolder = 1.3612; // as measured by 1-2-3 block method
                    const double LaserVoltageAtEdgeOfHolder = 1.381; // as empirically derived by assuming theoretical scale factor and pipe size
                    const double LaserScaleFactorInchesPerVolt = 12.0 / 10.0; // theoretical scale factor from datasheet
                    const double LaserHolderHalfWidth = 2.3125; // theoretical from drawing, part measures perfectly for width

                    var r = ((laser - LaserVoltageAtEdgeOfHolder) * LaserScaleFactorInchesPerVolt) + LaserHolderHalfWidth;

                    polarPoints.Add(new Vector2(theta, r));
                }

                var cartesianPoints = new List<Vector2>(polarPoints.Count);
                foreach (var polar in polarPoints)
                    cartesianPoints.Add(new Vector2(polar.Y * Math.Cos(polar.X), polar.Y * Math.Sin(polar.X)));

                var circle = GeometricFits.FitCircle(cartesianPoints);

                Console.WriteLine("Point Count: {0}", cartesianPoints.Count);
                Console.WriteLine();

                Console.WriteLine("Radius: {0:f3}", circle.Radius);
                Console.WriteLine("Diameter: {0:f3}", circle.Diameter);
                Console.WriteLine("Center X: {0:f3}", circle.Center.X);
                Console.WriteLine("Center Y: {0:f3}", circle.Center.Y);
                Console.WriteLine("Center Offset: {0:f3}", circle.Center.Length);
                Console.WriteLine();

                var ellipse = GeometricFits.FitEllipse(cartesianPoints);

                Console.WriteLine("Elliptical Major Diameter: {0:f3}", ellipse.MajorAxisLength);
                Console.WriteLine("Elliptical Minor Diameter: {0:f3}", ellipse.MinorAxisLength);
                Console.WriteLine("Elliptical Eccentricity: {0:f4}", ellipse.Eccentricity);

                Console.ReadLine();

                var censoredCenteredCartesian = new List<Vector2>();
                foreach(var point in cartesianPoints)
                {
                    var centered = point - circle.Center;
                    if (centered.Length > 2.95) censoredCenteredCartesian.Add(point);
                }
                var circle2 = GeometricFits.FitCircle(censoredCenteredCartesian);
                Console.WriteLine("Radius: {0:f3}", circle2.Radius);
                Console.WriteLine("Diameter: {0:f3}", circle2.Diameter);
                Console.WriteLine("Center X: {0:f3}", circle2.Center.X);
                Console.WriteLine("Center Y: {0:f3}", circle2.Center.Y);
                Console.WriteLine("Center Offset: {0:f3}", circle2.Center.Length);
                Console.WriteLine();

                Console.ReadLine();

                var function = AutomationLibrary.Mathematics.Curves.CircularFunction.FromCartesianPoints(censoredCenteredCartesian);
                function = function.SavitzkyGolaySmooth(7, 31);
                var area = function.ComputeArea();

                Console.WriteLine("Circular area: {0:f2}", circle.Radius * circle.Radius * Math.PI);
                Console.WriteLine("Smoothed function area: {0:f2}", area);

                Console.ReadLine();

                using (var writer = System.IO.File.CreateText(@"M:\Superior Energy\Test Runs\data_4.csv"))
                {
                    writer.WriteLine("X, Y, R, Theta");
                    foreach (var point in cartesianPoints)
                    {
                        var centered = point - circle.Center;

                        writer.WriteLine("{0},{1},{2},{3}", centered.X, centered.Y, centered.Length, centered.AngleFromXAxis);
                    }
                }
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
            var spec = PartSpecification.Parse(@"F:\Profiles\511955-002.xml");


            foreach (var segment in spec.ProfileSegments)
            {
                if (segment.IsMasterProfile)
                {
                    var boundary = segment.ToleranceBoundary(0);
                    boundary.Add(boundary[0]);

                    using (var writer = System.IO.File.CreateText(@"F:\tolerance.csv"))
                    {
                        writer.WriteLine("N, X, Y");
                        int n = 0;
                        foreach (var point in boundary)
                        {
                            writer.WriteLine("\"{0}\",{1:f5},{2:f5}", n++, point.X, point.Y);
                        }
                    }
                    break;
                }
            }


            List<Vector2> points = new List<Vector2>();

            using (var reader = System.IO.File.OpenText(@"F:\Profile378.csv"))
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

            var simplified = AutomationLibrary.Mathematics.Curves.Polyline.Simplify(points, 0.0005);

            // identify "blip" if there is one
            var findBlip = true;
            LineSegment2 blip = null;
            if (findBlip)
            {
                var candidates = new List<Tuple<LineSegment2, double>>(); // candidate segment and score

                // let the blip be a short horizontal section much above its surroundings)
                foreach (var segment in simplified.AsLineSegments())
                {
                    if (segment.Length > .2) continue;
                    var angle = Math.Abs(segment.Offset.AngleFromXAxis) * 180 / Math.PI;
                    if (angle > 5) continue;

                    var midX = segment.Midpoint.X;
                    var midY = segment.Midpoint.Y;
                    var minX = Math.Min(segment.Origin.X, segment.Destination.X);
                    var maxX = Math.Max(segment.Origin.X, segment.Destination.X);
                    var averageBackgroundY = (from p in points
                                             where ((midX - 1) < p.X && p.X < (midX + 1)) // must be within 1 inch of candidate midpoint
                                                   && (p.X < minX || maxX < p.X) // must not be within segment itself
                                                   && (p.Y > 0.001) // must not be off the wire
                                             select p.Y).Average();
                    var score = midY - averageBackgroundY;
                    if (score > 0) candidates.Add(Tuple.Create(segment, score));
                }

                // order candidates by decreasing score
                candidates.Sort((c1, c2) => -c1.Item2.CompareTo(c2.Item2));

                // limit candidates to a reasonable number to avoid complexity blowup in failure cases
                candidates = candidates.Take(10).ToList();

                // merge adjacent candidates into one
                var result = new List<LineSegment2>();
                while (candidates.Count > 0)
                {
                    var newResult = candidates[0].Item1;
                    candidates.RemoveAt(0);

                    // we may wish to extend it by merging it with any other top candidates that are adjacent
                    int i = 0;
                    while(true)
                    {
                        if (i >= candidates.Count) break;
                        var extensionCandidate = candidates[i].Item1;

                        if (extensionCandidate.Destination.Equals(newResult.Origin))
                        {
                            candidates.RemoveAt(i);
                            newResult = LineSegment2.FromOriginAndDestination(extensionCandidate.Origin, newResult.Destination);
                        }
                        else if (newResult.Destination.Equals(extensionCandidate.Origin))
                        {
                            candidates.RemoveAt(i);
                            newResult = LineSegment2.FromOriginAndDestination(newResult.Origin, extensionCandidate.Destination);
                        }
                        else i++;
                    }

                    result.Add(newResult);
                }

                if (candidates.Count == 0) blip = null;
                else
                {
                    blip = candidates[0].Item1; // hooray, we found the blip
                    candidates.RemoveAt(0);
                    
                    // we may wish to extend it by merging it with any other top candidates that are adjacent
                    while (candidates.Count > 0)
                    {
                        var extensionCandidate = candidates[0].Item1;
                        candidates.RemoveAt(0);

                        if (extensionCandidate.Destination.Equals(blip.Origin))
                        {
                            blip = LineSegment2.FromOriginAndDestination(extensionCandidate.Origin, blip.Destination);
                        }
                        else if (blip.Destination.Equals(extensionCandidate.Origin))
                        {
                            blip = LineSegment2.FromOriginAndDestination(blip.Origin, extensionCandidate.Destination);
                        }
                        else break;
                    }
                }
            }

            var resimplified = new List<LineSegment2>();
            foreach (var seg in simplified.AsLineSegments())
            {
                var startX = seg.Origin.X;
                var endX = seg.Destination.X;

                var blipStartX = blip.Origin.X - .05;
                var blipEndX = blip.Destination.X + .05;

                if (startX < blipStartX || blipEndX < endX) resimplified.Add(seg);
            }
            resimplified.Add(blip);

            var resimplifiedPoints = new List<Vector2>();
            foreach (var seg in resimplified)
            {
                if (!resimplifiedPoints.Contains(seg.Origin)) resimplifiedPoints.Add(seg.Origin);
                if (!resimplifiedPoints.Contains(seg.Destination)) resimplifiedPoints.Add(seg.Destination);
            }
            resimplifiedPoints.Sort((p1, p2) => p1.X.CompareTo(p2.X));

            var fitLines = new List<Tuple<double,double,Line2>>();

            for (int i = 0; i < resimplifiedPoints.Count - 1; i++)
            {
                var start = resimplifiedPoints[i];
                var end = resimplifiedPoints[i + 1];

                var segPoints = SelectPointsBetween(points, start.X, end.X);
                var n = segPoints.Count;

                int skip = 0;
                if (n > 20) skip = n / 10;
                else if (n > 4) skip = n / 4;

                var notNearEnds = segPoints.SkipBothEnds(skip);

                var segFineLine = GeometricFits.FitLine(notNearEnds.ToList());

                fitLines.Add(Tuple.Create(start.X, end.X, segFineLine));
            }

            using (var writer = System.IO.File.CreateText(@"F:\profile-fit-lines.csv"))
            {
                writer.WriteLine("X,Y");
                foreach (var point in resimplifiedPoints)
                {
                    writer.WriteLine("{0},{1}", point.X, point.Y);
                }
                /*
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
                 */
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
