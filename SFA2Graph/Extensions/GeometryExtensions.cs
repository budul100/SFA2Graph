using NetTopologySuite.Geometries;
using SFA2Graph.Models;
using System.Collections.Generic;
using System.Linq;

namespace SFA2Graph.Extensions
{
    internal static class GeometryExtensions
    {
        #region Public Methods

        public static IEnumerable<Vertice> GetVertices(this Geometry geometry,
            int verticesDistanceMin)
        {
            var relevants = geometry?.Coordinates?
                .Skip(1).SkipLast(1).ToArray();

            if (relevants.Length > 1)
            {
                var lastCoordinate = geometry.Coordinates[0];
                var distance = 0.0;

                foreach (var relevant in relevants)
                {
                    var currentDistance = lastCoordinate.GetDistance(relevant);

                    if (!relevant.Equals(lastCoordinate)
                        && currentDistance > verticesDistanceMin)
                    {
                        distance += currentDistance;

                        var result = new Vertice
                        {
                            Coordinate = relevant,
                            Distance = distance,
                        };

                        yield return result;

                        lastCoordinate = relevant;
                    }
                }
            }
        }

        #endregion Public Methods
    }
}