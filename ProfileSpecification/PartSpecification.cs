using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Collections.ObjectModel;

namespace AutomationLibrary.Mathematics.ProfileSpecification
{
    public sealed class PartSpecification
    {
        private readonly string _name;
        private readonly string _version;
        private readonly string _sourcePath; // where this was loaded from, optional
        private readonly DateTime? _sourceLastModifiedTime; // timestamp on where this was loaded from, optional
        private readonly double _minimumOverallLength;
        private readonly double _maximumOverallLength;
        private readonly List<ConstrainedProfileSegment> _profileSegments;

        public PartSpecification(string name, string version, string sourcePath, DateTime? sourceLastModifiedTime, double minimumOverallLength, double maximumOverallLength, IEnumerable<ConstrainedProfileSegment> profileSegments)
        {
            if (minimumOverallLength < 0) throw new ArgumentOutOfRangeException();
            if (maximumOverallLength < minimumOverallLength) throw new ArgumentException();
            if (profileSegments == null) throw new ArgumentNullException();

            _name = (name ?? "Unnamed Part").Trim();
            _version = (version ?? "None").Trim();
            if (sourcePath != null) _sourcePath = sourcePath.Trim();
            _sourceLastModifiedTime = sourceLastModifiedTime;
            _minimumOverallLength = minimumOverallLength;
            _maximumOverallLength = maximumOverallLength;

            _profileSegments = profileSegments.ToList();
        }

        #region Parsing

        private static readonly string XmlNamespace = @"http://schema.massbayengineering.com/2013/DiameterProfileSpecification/";
        private static readonly XName RootNodeName = XName.Get("PartSpecification", XmlNamespace);
        private static readonly XName NameNodeName = XName.Get("Name", XmlNamespace);
        private static readonly XName RevisionNodeName = XName.Get("Revision", XmlNamespace);
        private static readonly XName GeometryNodeName = XName.Get("Geometry", XmlNamespace);
        private static readonly XName ProfileNodeName = XName.Get("Profile", XmlNamespace);

        public static PartSpecification Parse(string path)
        {
            DateTime? lastModifiedTime = null;
            try
            {
                lastModifiedTime = System.IO.File.GetLastWriteTime(path);
            }
            catch { }

            var document = XDocument.Load(path, LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);

            return Parse(document, path, lastModifiedTime);
        }

        public static PartSpecification Parse(XDocument document, string sourcePath, DateTime? sourceLastModifiedTime)
        {
            var rootNode = document.Root;
            if (rootNode.Name != RootNodeName) throw new Exception(string.Format("Could not locate root {0} node. (Possibly missing XML namespace declaration?)", RootNodeName.LocalName));

            return Parse(rootNode, sourcePath, sourceLastModifiedTime);
        }

        private static PartSpecification Parse(XElement rootElement, string sourcePath, DateTime? sourceLastModifiedTime)
        {
            string name = null;
            string revision = null;

            var nameElements = rootElement.Elements(NameNodeName);
            if (nameElements.Count() == 1) name = nameElements.Single().Value;
            else if(nameElements.Count() > 1) throw new Exception("Multiple name elements encountered.");

            var revisionElements = rootElement.Elements(RevisionNodeName);
            if (revisionElements.Count() == 1) revision = revisionElements.Single().Value;
            else if(revisionElements.Count() > 1) throw new Exception("Multiple revision elements encountered.");

            var geometryElements = rootElement.Elements(GeometryNodeName);
            if (geometryElements.Count() <= 0) throw new Exception("No geometry node found.");
            else if (geometryElements.Count() > 1) throw new Exception("Multiple geometry elements encountered.");

            var geometryElement = geometryElements.Single();

            var unitNameAttribute = geometryElement.Attribute("unit");
            if (unitNameAttribute == null) throw new Exception("Unit not specified on geometry element.");
            var unitName = unitNameAttribute.Value;
            LengthUnit unit;
            if (!TryParseEnum<LengthUnit>(unitName, out unit)) throw new Exception("Unrecognized unit specified on geometry element.");

            double scaleFactorToInches;
            switch (unit)
            {
                case LengthUnit.Inch:
                    scaleFactorToInches = 1;
                    break;
                case LengthUnit.Millimeter:
                    scaleFactorToInches = .03937;
                    break;
                default:
                    throw new Exception("Unrecognized unit specified on geometry element.");
            }

            double minLength;
            var minLengthAttribute = geometryElement.Attribute("minLength");
            if (minLengthAttribute == null) minLength = 0;
            else
            {
                if (!TryParseDouble(minLengthAttribute.Value, out minLength)) throw new Exception("Minimum length attribute on geometry element could not be parsed.");
            }

            double maxLength;
            var maxLengthAttribute = geometryElement.Attribute("maxLength");
            if (maxLengthAttribute == null) maxLength = double.PositiveInfinity;
            else
            {
                if (!TryParseDouble(maxLengthAttribute.Value, out maxLength)) throw new Exception("Maximum length attribute on geometry element could not be parsed.");
            }

            minLength *= scaleFactorToInches;
            maxLength *= scaleFactorToInches;


            var profileElements = geometryElement.Elements(ProfileNodeName);
            List<ConstrainedProfileSegment> profileSegments = new List<ConstrainedProfileSegment>();

            foreach (var profileElement in profileElements)
            {
                var profileSegment = ParseProfileSegment(profileElement, scaleFactorToInches);
                if (profileSegments.Count == 0 && !profileSegment.IsMasterProfile) throw new Exception(string.Format("The first profile listed (at line {0}) must be the master profile, and so it may not have posMin and posMax attributes.", GetLine(profileElement)));
                // TODO: reinstate this condition in a revised form
                // if (profileSegments.Count >= 1 && profileSegment.IsMasterProfile) throw new Exception(string.Format("Only the first profile listed may be the master profile. The profile at line {0} must have posMin and posMax attributes.", GetLine(profileElement)));
                profileSegments.Add(profileSegment);
            }

            // TODO: validate that segments don't conflict with each other? seems complicated and unneccessary
            // conflicts could happen if a child profile is too long or improperly placed to fit in the parent profile

            return new PartSpecification(name, revision, sourcePath, sourceLastModifiedTime, minLength, maxLength, profileSegments);
        }

        private static bool TryParseDouble(string s, out double result)
        {
            try
            {
                result = double.Parse(s);
                return true;
            }
            catch
            {
                result = double.NaN;
                return false;
            }
        }

        private static bool TryParseEnum<T>(string s, out T result)
            where T : struct
        {
            try
            {
                result = (T)Enum.Parse(typeof(T), s, false); // regard case
                return true;
            }
            catch
            {
                result = default(T);
                return false;
            }
        }

        private static ConstrainedProfileSegment ParseProfileSegment(XElement profileElement, double scaleFactorToInches)
        {
            string name = ParseOptionalStringAttribute(profileElement, "name");

            var minPos = ParseOptionalDoubleAttribute(profileElement, "posMin");
            var maxPos = ParseOptionalDoubleAttribute(profileElement, "posMax");

            if (minPos.HasValue != maxPos.HasValue) throw new Exception(string.Format("A child profile at line {0} must specify both a minimum and maximum position of its origin in the frame of its parent.", GetLine(profileElement)));

            int positionNumber = 1;
            var constraints = new List<ProfileConstraint>();
            foreach (var element in profileElement.Elements())
            {
                constraints.Add(ParseProfileConstraint(element, scaleFactorToInches, positionNumber));
                positionNumber += 1;
            }

            if (minPos.HasValue) return new ConstrainedProfileSegment(name, constraints, minPos.Value, maxPos.Value);
            else return new ConstrainedProfileSegment(name, constraints);
        }

        private static ProfileConstraint ParseProfileConstraint(XElement constraintElement, double scaleFactorToInches, int positionNumber)
        {
            if (constraintElement.Name.Namespace.NamespaceName != XmlNamespace) throw new ArgumentException(string.Format("Unrecognized profile constraint at line {0}.", GetLine(constraintElement)));

            string name = ParseOptionalStringAttribute(constraintElement, "name");

            name = name ?? string.Format("Point_{0}", positionNumber);

            switch (constraintElement.Name.LocalName)
            {
                case "Origin":
                    {
                        var diameter = ParseDiameterConstraint(constraintElement, scaleFactorToInches);
                        return new OriginProfileConstraint(name, diameter);
                    }
                case "Absolute":
                    {
                        var diameter = ParseDiameterConstraint(constraintElement, scaleFactorToInches);

                        double minPos = scaleFactorToInches * ParseRequiredDoubleAttribute(constraintElement, "posMin");
                        double maxPos = scaleFactorToInches * ParseRequiredDoubleAttribute(constraintElement, "posMax");

                        return new AbsolutelyPositionedPointProfileConstraint(name, diameter, minPos, maxPos);
                    }
                case "Relative":
                    {
                        var diameter = ParseDiameterConstraint(constraintElement, scaleFactorToInches);

                        double minPos = scaleFactorToInches * ParseRequiredDoubleAttribute(constraintElement, "posMin");
                        double maxPos = scaleFactorToInches * ParseRequiredDoubleAttribute(constraintElement, "posMax");

                        return new RelativelyPositionedPointProfileConstraint(name, diameter, minPos, maxPos);
                    }
                case "Between":
                    {
                        string fromName = ParseRequiredStringAttribute(constraintElement, "from");
                        string toName = ParseRequiredStringAttribute(constraintElement, "to");
                        double minDiff = scaleFactorToInches * ParseOptionalDoubleAttribute(constraintElement, "minDiff", 0);
                        double maxDiff = scaleFactorToInches * ParseOptionalDoubleAttribute(constraintElement, "maxDiff", double.PositiveInfinity);

                        return new DistanceBetweenPointsProfileConstraint(fromName, toName, minDiff, maxDiff);
                    }
                default:
                    throw new ArgumentException(string.Format("Unrecognized profile constraint at line {0}.", GetLine(constraintElement)));
            }
        }

        private static string ParseOptionalStringAttribute(XElement element, string name)
        {
            var attr = element.Attribute(name);
            
            if (attr == null) return null;
            else return attr.Value;
        }

        private static string ParseRequiredStringAttribute(XElement element, string name)
        {
            var attr = element.Attribute(name);
            if (attr == null) throw new Exception(string.Format("Unable to find required attribute {0} at line {1}.", name, GetLine(element)));

            return attr.Value;
        }

        private static double? ParseOptionalDoubleAttribute(XElement element, string name)
        {
            var attr = element.Attribute(name);
            if (attr == null) return null;

            double result;
            if (TryParseDouble(attr.Value, out result)) return result;
            else throw new Exception(string.Format("Unable to parse dimension at line {0}.", GetLine(attr)));
        }

        private static double ParseOptionalDoubleAttribute(XElement element, string name, double defaultValue)
        {
            var attr = element.Attribute(name);
            if (attr == null) return defaultValue;

            double result;
            if (TryParseDouble(attr.Value, out result)) return result;
            else throw new Exception(string.Format("Unable to parse dimension at line {0}.", GetLine(attr)));
        }

        private static double ParseRequiredDoubleAttribute(XElement element, string name)
        {
            var attr = element.Attribute(name);
            if (attr == null) throw new Exception(string.Format("Unable to find required attribute {0} at line {1}.", name, GetLine(element)));
            
            double result;
            if (TryParseDouble(attr.Value, out result)) return result;
            else throw new Exception(string.Format("Unable to parse dimension at line {0}.", GetLine(attr)));
        }

        private static DiameterConstraint ParseDiameterConstraint(XElement element, double scaleFactorToInches)
        {
            var minAttribute = element.Attribute("min");
            var maxAttribute = element.Attribute("max");
            var nominalAttribute = element.Attribute("nominal");

            if (nominalAttribute != null && minAttribute == null && maxAttribute == null)
            {
                double nominalValue;
                if (TryParseDouble(nominalAttribute.Value, out nominalValue))
                {
                    nominalValue *= scaleFactorToInches;
                    return DiameterConstraint.FromNominalValue(nominalValue);
                }
                else
                {
                    if (nominalAttribute.Value == "Parent") return DiameterConstraint.IntersectionWithParentProfile;
                    else throw new Exception(string.Format("Unrecognized nominal diameter value at line {0}.", GetLine(nominalAttribute)));
                }
            }
            else if (minAttribute != null || maxAttribute != null)
            {
                double min = 0;
                double max = double.PositiveInfinity;
                if (minAttribute != null)
                {
                    if (!TryParseDouble(minAttribute.Value, out min)) throw new Exception(string.Format("Could not parse minimum diameter value at line {0}.", GetLine(minAttribute)));
                }
                if (maxAttribute != null)
                {
                    if (!TryParseDouble(maxAttribute.Value, out max)) throw new Exception(string.Format("Could not parse maximum diameter value at line {0}.", GetLine(maxAttribute)));
                }

                min *= scaleFactorToInches;
                max *= scaleFactorToInches;

                // TODO: parse an informative nominal diameter that may have also been present

                return DiameterConstraint.FromMinimumAndMaximum(min, max);
            }
            else
            {
                throw new Exception(string.Format("Conflicting diameter constraints at line {0}. Provide a nominal dimension or min/max dimensions.", GetLine(element)));
            }
        }

        private static string GetLine(IXmlLineInfo lineInfo)
        {
            if (lineInfo == null) return "[Unknown]";
            return lineInfo.LineNumber.ToString();
        }

        #endregion

        #region Properties

        public string Name { get { return _name; } }
        public string Version { get { return _version; } }
        public string SourcePath { get { return _sourcePath; } }
        public DateTime? SourceLastModifiedTime { get { return _sourceLastModifiedTime; } }
        public ReadOnlyCollection<ConstrainedProfileSegment> ProfileSegments { get { return new ReadOnlyCollection<ConstrainedProfileSegment>(_profileSegments); } }

        #endregion
    }
}
