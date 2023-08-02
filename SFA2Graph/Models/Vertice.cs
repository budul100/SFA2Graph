using NetTopologySuite.Geometries;
using System.Globalization;
using System.Text;

namespace SFA2Graph.Models
{
    internal class Vertice
    {
        #region Public Fields

        public const char VerticesDelimiter = ' ';

        #endregion Public Fields

        #region Private Fields

        private readonly NumberFormatInfo numberFormat;

        #endregion Private Fields

        #region Public Constructors

        public Vertice()
        {
            numberFormat = new NumberFormatInfo
            {
                NumberDecimalSeparator = "."
            };
        }

        #endregion Public Constructors

        #region Public Properties

        public Coordinate Coordinate { get; set; }

        public double Distance { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            var result = new StringBuilder();

            result.Append(Coordinate.X.ToString(numberFormat));

            result.Append(VerticesDelimiter);

            result.Append(Coordinate.Y.ToString(numberFormat));

            result.Append(VerticesDelimiter);

            result.Append(Distance.ToString(numberFormat));

            return result.ToString();
        }

        #endregion Public Methods
    }
}