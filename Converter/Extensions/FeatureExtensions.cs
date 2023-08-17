using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using StringExtensions;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SFA2Graph.Converter.Extensions
{
    internal static class FeatureExtensions
    {
        #region Public Methods

        public static string GetAttribute(this Feature feature, string key)
        {
            var result = default(string);

            if (!key.IsEmpty())
            {
                result = feature.Attributes?
                    .GetOptionalValue(key)?.ToString();
            }

            return result;
        }

        public static IEnumerable<Geometry> GetGeometries(this Feature feature)
        {
            var length = feature.Geometry.NumGeometries;

            for (var index = 0; index < length; index++)
            {
                var result = feature.Geometry.GetGeometryN(index);

                if (!result.IsEmpty
                    && result.Coordinates[0] != result.Coordinates.Last())
                {
                    yield return result;
                }
            }
        }

        public static bool IsValid(this Feature feature, IEnumerable<string> lineFilters, IEnumerable<string> attributesKey)
        {
            var result = (lineFilters?.Any() != true) || lineFilters.Any(f => Regex.IsMatch(
                input: feature.GetAttribute(attributesKey),
                pattern: f));

            return result;
        }

        #endregion Public Methods

        #region Private Methods

        private static string GetAttribute(this Feature feature, IEnumerable<string> keys)
        {
            foreach (var key in keys)
            {
                var result = feature.GetAttribute(key);

                if (!result.IsEmpty())
                {
                    return result;
                }
            }

            return default;
        }

        #endregion Private Methods
    }
}