using NetTopologySuite.Geometries;

namespace SFA2Graph.Converter.Models
{
    internal class Vertice
    {
        #region Public Fields

        public const char VerticesDelimiter = ' ';

        #endregion Public Fields

        #region Public Properties

        public Coordinate Coordinate { get; set; }

        public double Distance { get; set; }

        #endregion Public Properties
    }
}