using CommandLine;
using NetTopologySuite.Geometries;
using System.Collections.Generic;

namespace SFA2Graph.Models
{
    public class Options
    {
        #region Public Properties

        [Option(
            shortName: 'i',
            longName: "inputpaths",
            HelpText = "Pathes of the GeoJSON files to be input.",
            Required = true,
            Separator = ',')]
        public IEnumerable<string> InputPaths { get; set; }

        [Option(
            longName: "lineattrfilter",
            HelpText = "Key value pairs of attributes where one must exist to be considered as line. The value must be a regular expression. " +
                "Keys, values, and pairs must be split by a comma.",
            Separator = ',',
            Required = false,
            Default = new string[]
            {
                "type", "route",
                "type", "route_master",
            })]
        public IEnumerable<string> LineAttributesFilter { get; set; }

        [Option(
            longName: "lineattrkey",
            HelpText = "Attributes where one must exist to be considered as locations and which is used to group the lines. " +
                "The attributes must be split by a comma.",
            Separator = ',',
            Required = false)]
        public IEnumerable<string> LineAttributesKey { get; set; }

        [Option(
            longName: "linefilter",
            HelpText = "One or multiple values of the line key attributes which the line input should be filtered for.",
            Separator = ',',
            Required = false)]
        public IEnumerable<string> LineFilters { get; set; }

        [Option(
            longName: "linetypes",
            HelpText = "The geometry types to be considered as lines. " +
                "See <https://nettopologysuite.github.io/NetTopologySuite/api/NetTopologySuite.Geometries.OgcGeometryType.html> for possible values.",
            Required = false,
            Default = new OgcGeometryType[]
            {
                OgcGeometryType.LineString,
                OgcGeometryType.MultiLineString,
            })]
        public IEnumerable<OgcGeometryType> LineTypes { get; set; }

        [Option(
            shortName: 'o',
            longName: "outputpath",
            HelpText = "Path of the resulting txt routing graph file.",
            Required = true)]
        public string OutputPath { get; set; }

        #endregion Public Properties
    }
}