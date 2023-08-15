using NetTopologySuite.Geometries;
using SFA2Graph.Models;
using System.Collections.Generic;

namespace SFA2Graph.Extensions
{
    internal static class GeometryExtensions
    {
        #region Public Methods

        public static IEnumerable<IEnumerable<Vertice>> GetVerticesGroups(this Geometry geometry, int arcLengthMin)
        {
            var distance = 0.0;
            var last = default(Coordinate);
            var result = new List<Vertice>();

            foreach (var coordinate in geometry.Coordinates)
            {
                distance += last?.GetDistance(coordinate) ?? 0.0;

                var current = new Vertice
                {
                    Coordinate = coordinate,
                    Distance = distance,
                };

                result.Add(current);

                if (distance >= arcLengthMin
                    && result.Count > 1)
                {
                    yield return result;

                    distance = 0.0;
                    result = new List<Vertice>();

                    current = new Vertice
                    {
                        Coordinate = coordinate,
                        Distance = distance,
                    };

                    result.Add(current);
                }

                last = coordinate;
            }

            if (result.Count > 1)
            {
                yield return result;
            }
        }

        #endregion Public Methods
    }
}