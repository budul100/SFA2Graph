using NetTopologySuite.Geometries;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;

namespace SFA2Graph.Extensions
{
    internal static class CoordinateExtensions
    {
        #region Public Methods

        public static double GetDistance(this Coordinate left, Coordinate right)
        {
            var ctfac = new CoordinateTransformationFactory();

            var from = GeographicCoordinateSystem.WGS84;
            var to = ProjectedCoordinateSystem.WebMercator;

            var trans = ctfac.CreateFromCoordinateSystems(
                sourceCS: from,
                targetCS: to);
            var mathTransform = trans.MathTransform;

            var (leftX, leftY) = mathTransform.Transform(
                x: left.X,
                y: left.Y);
            var (rightX, rightY) = mathTransform.Transform(
                x: right.X,
                y: right.Y);

            var leftCoordinate = new GeoAPI.Geometries.Coordinate(
                x: leftX,
                y: leftY);
            var rightCoordinate = new GeoAPI.Geometries.Coordinate(
                x: rightX,
                y: rightY);

            return leftCoordinate.Distance(rightCoordinate);
        }

        #endregion Public Methods
    }
}