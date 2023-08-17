using NetTopologySuite.Features;
using ProgressWatcher.Interfaces;
using SFA2Graph.Converter.Extensions;
using SFA2Graph.Converter.Models;
using StringExtensions;
using System.Collections.Generic;
using System.Linq;

namespace SFA2Graph.Converter.Factories
{
    internal class ArcFactory
    {
        #region Private Fields

        private const int IndexStart = 0;
        private const int LevelDefault = 2;
        private const int RoadclassDefault = 1;
        private const int TypDefault = 0;

        private readonly int arcLengthMin;
        private readonly Dictionary<HashSet<Vertice>, Arc> arcs = new();
        private readonly int decimalPoints;
        private readonly string delimiter;

        private int index = IndexStart;

        #endregion Private Fields

        #region Public Constructors

        public ArcFactory(int decimalPoints, int arcLengthMin)
        {
            this.decimalPoints = decimalPoints;
            this.arcLengthMin = arcLengthMin;

            delimiter = Vertice.VerticesDelimiter.ToString();
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
                status: "Convert lines into arcs.");

            foreach (var line in lines)
            {
                ConvertFeature(
                    feature: line,
                    parentPackage: infoPackage);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void ConvertFeature(Feature feature, IPackage parentPackage)
        {
            var geometries = feature.GetGeometries().ToArray();

            using var infoPackage = parentPackage.GetPackage(
                items: geometries,
                status: "Convert feature geometries into arcs.");

            foreach (var geometry in geometries)
            {
                var verticesGroups = geometry.GetVerticesGroups(
                    arcLengthMin: arcLengthMin).ToArray();

                foreach (var verticesGroup in verticesGroups)
                {
                    var key = new HashSet<Vertice>(verticesGroup);

                    if (!arcs.ContainsKey(key))
                    {
                        var result = GetArc(
                            vertices: verticesGroup);

                        arcs.Add(
                            key: key,
                            value: result);
                    }
                }

                infoPackage.NextStep();
            }
        }

        private Arc GetArc(IEnumerable<Vertice> vertices)
        {
            var from = vertices.First();
            var to = vertices.Last();

            var verticesText = vertices
                .Skip(1).SkipLast(1)
                .Select(v => v.AsText(decimalPoints))
                .Join(delimiter: delimiter);

            var result = new Arc
            {
                ArcID = ++index,
                FromX = from.Coordinate.X.ToStringDecimal(decimalPoints),
                FromY = from.Coordinate.Y.ToStringDecimal(decimalPoints),
                Length = to.Distance.ToStringInt(),
                Level = LevelDefault,
                RoadClass = RoadclassDefault,
                ToX = to.Coordinate.X.ToStringDecimal(decimalPoints),
                ToY = to.Coordinate.Y.ToStringDecimal(decimalPoints),
                Typ = TypDefault,
                Vertices = verticesText,
            };

            return result;
        }

        #endregion Private Methods
    }
}