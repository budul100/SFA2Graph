using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using ProgressWatcher.Interfaces;
using SFA2Graph.Extensions;
using SFA2Graph.Models;
using StringExtensions;
using System.Collections.Generic;
using System.Linq;

namespace SFA2Graph.Factories
{
    internal class ArcFactory
    {
        #region Private Fields

        private const int IndexStart = 0;
        private const int LevelDefault = 2;
        private const int RoadclassDefault = 1;
        private const int TypDefault = 0;
        private const int VerticesDistanceMin = 5;

        private readonly Dictionary<string, Arc> arcs = new();
        private readonly int decimalPoints;
        private readonly string delimiter;

        private int index = IndexStart;

        #endregion Private Fields

        #region Public Constructors

        public ArcFactory(int decimalPoints)
        {
            delimiter = Vertice.VerticesDelimiter.ToString();
            this.decimalPoints = decimalPoints;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEnumerable<Arc> Contents => arcs.Values;

        #endregion Public Properties

        #region Public Methods

        public void Load(IEnumerable<Feature> lines, IPackage parentPackage)
        {
            using var infoPackage = parentPackage.GetPackage(
                items: lines,
                status: "Convert lines to arcs.");

            foreach (var line in lines)
            {
                var geometries = line.GetGeometries().ToArray();

                foreach (var geometry in geometries)
                {
                    var vertices = geometry.GetVertices(
                        verticesDistanceMin: VerticesDistanceMin).ToArray();

                    var verticesText = vertices
                        .Select(v => v.AsText(decimalPoints))
                        .Join(delimiter: delimiter);

                    if (!arcs.ContainsKey(verticesText))
                    {
                        var lastCoordinate = vertices.LastOrDefault()?.Coordinate
                            ?? geometry.Coordinates[0];
                        var lastDistance = vertices.LastOrDefault()?.Distance
                            ?? 0;

                        var arc = GetArc(
                            geometry: geometry,
                            lastCoordinate: lastCoordinate,
                            lastDistance: lastDistance,
                            verticesText: verticesText);

                        arcs.Add(
                            key: verticesText,
                            value: arc);
                    }
                }

                infoPackage.NextStep();
            }
        }

        #endregion Public Methods

        #region Private Methods

        private Arc GetArc(Geometry geometry, Coordinate lastCoordinate, double lastDistance,
            string verticesText)
        {
            var length = lastDistance + lastCoordinate.GetDistance(geometry.Coordinates[^1]);

            var result = new Arc
            {
                ArcID = ++index,
                FromX = geometry.Coordinates[0].X.ToStringDecimal(decimalPoints),
                FromY = geometry.Coordinates[0].Y.ToStringDecimal(decimalPoints),
                Length = length.ToStringInt(),
                Level = LevelDefault,
                RoadClass = RoadclassDefault,
                ToX = geometry.Coordinates[^1].X.ToStringDecimal(decimalPoints),
                ToY = geometry.Coordinates[^1].Y.ToStringDecimal(decimalPoints),
                Typ = TypDefault,
                Vertices = verticesText,
            };

            return result;
        }

        #endregion Private Methods
    }
}