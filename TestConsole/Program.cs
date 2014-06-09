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
            List<Vector2> points = new List<Vector2>();
            using (var reader = System.IO.File.OpenText(@"M:\Kirwan\Grinding Robot\Dropbox\scanner-survey_4_0_neg34.txt"))
            {
                reader.ReadLine(); // skip header
                while (true)
                {
                    var line = reader.ReadLine();
                    if (line == null) break;

                    var tokens = line.Split(' ', ',', '\t');
                    double x, z;
                    if (double.TryParse(tokens[0], out x) && double.TryParse(tokens[1], out z))
                    {
                        points.Add(new Vector2(x, z));
                    }
                }
            }

            var circle = GeometricFits.FitCircle(points);

            Console.ReadLine();
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
