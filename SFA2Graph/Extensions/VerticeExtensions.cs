using SFA2Graph.Models;
using System.Text;

namespace SFA2Graph.Extensions
{
    internal static class VerticeExtensions
    {
        #region Public Methods

        public static string AsText(this Vertice vertice, int decimalPoints)
        {
            var result = new StringBuilder();

            result.Append(vertice.Coordinate.X.ToStringDecimal(decimalPoints));

            result.Append(Vertice.VerticesDelimiter);

            result.Append(vertice.Coordinate.Y.ToStringDecimal(decimalPoints));

            result.Append(Vertice.VerticesDelimiter);

            result.Append(vertice.Distance.ToStringInt());

            return result.ToString();
        }

        #endregion Public Methods
    }
}