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

        private readonly HashSet<Arc> arcs = new();

        #endregion Private Fields

        #region Public Methods

        public void Write(IEnumerable<Feature> lines, string path, IPackage parentPackage)
        {
            using var infoPackage = parentPackage.GetPackage(
                steps: 2,
                status: "Create routing graph");

            LoadArcs(
                lines: lines,
                parentPackage: infoPackage);

            WriteRouting(
                path: path,
                parentPackage: infoPackage);
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadArcs(IEnumerable<Feature> lines, IPackage parentPackage)
        {
            using var infoPackage = parentPackage.GetPackage(
                items: lines,
                status: "Convert lines to arcs.");

            var index = IndexStart;
            var delimiter = Vertice.VerticesDelimiter.ToString();

            foreach (var line in lines)
            {
                var geometries = line.GetGeometries().ToArray();

                foreach (var geometry in geometries)
                {
                    var vertices = geometry.GetVertices().ToArray();
                    var verticesText = vertices
                        .Select(v => v.ToString())
                        .Join(delimiter: delimiter);

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

                    arcs.Add(arc);
                }

                infoPackage.NextStep();
            }
        }

        private void WriteRouting(string path, IPackage parentPackage)
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