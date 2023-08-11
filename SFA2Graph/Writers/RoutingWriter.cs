using CsvHelper;
using CsvHelper.Configuration;
using NetTopologySuite.Features;
using ProgressWatcher.Interfaces;
using SFA2Graph.Extensions;
using SFA2Graph.Models;
using StringExtensions;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace SFA2Graph.Writers
{
    internal class RoutingWriter
    {
        #region Private Fields

        private const int IndexStart = 0;
        private const int LevelDefault = 2;
        private const int RoadclassDefault = 1;
        private const int TypDefault = 0;

        #endregion Private Fields

        #region Public Methods

        public void Write(IEnumerable<Feature> lines, string path, IPackage parentPackage)
        {
            using var infoPackage = parentPackage.GetPackage(
                steps: 2,
                status: "Create routing graph");

            var arcs = GetArcs(
                lines: lines,
                parentPackage: infoPackage);

            WriteRouting(
                arcs: arcs,
                path: path,
                parentPackage: infoPackage);
        }

        #endregion Public Methods

        #region Private Methods

        private IEnumerable<Arc> GetArcs(IEnumerable<Feature> lines, IPackage parentPackage)
        {
            using var infoPackage = parentPackage.GetPackage(
                items: lines,
                status: "Convert lines to arcs.");

            var index = IndexStart;
            var delimiter = Vertice.VerticesDelimiter.ToString();

            var result = new Dictionary<string, Arc>();

            foreach (var line in lines)
            {
                var geometries = line.GetGeometries().ToArray();

                foreach (var geometry in geometries)
                {
                    var vertices = geometry.GetVertices().ToArray();
                    var verticesText = vertices
                        .Select(v => v.ToString())
                        .Join(delimiter: delimiter);

                    if (!result.ContainsKey(verticesText))
                    {
                        var lastCoordinate = vertices.LastOrDefault()?.Coordinate
                            ?? geometry.Coordinates[0];
                        var lastDistance = vertices.LastOrDefault()?.Distance
                            ?? 0;
                        var length = lastDistance + lastCoordinate.GetDistance(geometry.Coordinates[^1]);

                        var arc = new Arc
                        {
                            ArcID = ++index,
                            FromX = geometry.Coordinates[0].X,
                            FromY = geometry.Coordinates[0].Y,
                            Length = length,
                            Level = LevelDefault,
                            RoadClass = RoadclassDefault,
                            ToX = geometry.Coordinates[^1].X,
                            ToY = geometry.Coordinates[^1].Y,
                            Typ = TypDefault,
                            Vertices = verticesText,
                        };

                        result.Add(
                            key: verticesText,
                            value: arc);
                    }
                }

                infoPackage.NextStep();
            }

            return result.Values;
        }

        private void WriteRouting(IEnumerable<Arc> arcs, string path, IPackage parentPackage)
        {
            using var infoPackage = parentPackage.GetPackage(
                status: "Write file.");

            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = "\t",
                Encoding = Encoding.UTF8,
                HasHeaderRecord = true,
            };

            using var streamWriter = new StreamWriter(path);
            using var csvWriter = new CsvWriter(
                writer: streamWriter,
                configuration: configuration);

            csvWriter.WriteRecords(arcs);
        }

        #endregion Private Methods
    }
}