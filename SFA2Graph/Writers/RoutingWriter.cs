using CsvHelper;
using CsvHelper.Configuration;
using ProgressWatcher.Interfaces;
using SFA2Graph.Models;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace SFA2Graph.Writers
{
    internal class RoutingWriter
    {
        #region Private Fields

        private CsvConfiguration writerConfiguration;

        #endregion Private Fields

        #region Public Constructors

        public RoutingWriter()
        {
            writerConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = "\t",
                Encoding = Encoding.UTF8,
                HasHeaderRecord = true,
            };
        }

        #endregion Public Constructors

        #region Public Methods

        public void Write(IEnumerable<Arc> arcs, string path, IPackage parentPackage)
        {
            using var infoPackage = parentPackage.GetPackage(
                status: "Write routing graph");

            using var streamWriter = new StreamWriter(
                path: path);

            using var csvWriter = new CsvWriter(
                writer: streamWriter,
                configuration: writerConfiguration);

            csvWriter.WriteRecords(arcs);
        }

        #endregion Public Methods
    }
}