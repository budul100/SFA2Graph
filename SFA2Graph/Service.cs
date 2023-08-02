using ProgressWatcher;
using ProgressWatcher.Interfaces;
using SFA2Graph.Extensions;
using SFA2Graph.Models;
using SFA2Graph.Repositories;
using SFA2Graph.Writers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA2Graph
{
    public class Service
    {
        #region Private Fields

        private const double StatusWeightLoadingFiles = 0.7;

        private readonly FeatureRepository lineRepository;
        private readonly Action<double, string> onProgressChange;
        private readonly Options options;
        private readonly Watcher progressWatcher;

        #endregion Private Fields

        #region Public Constructors

        public Service(Options options, Action<double, string> onProgressChange)
        {
            this.options = options;
            this.onProgressChange = onProgressChange;

            progressWatcher = new Watcher();
            progressWatcher.PropertyChanged += OnProgressChanged;
            progressWatcher.ProgressCompleted += OnProgressCompleted;

            var lineAttributesFilter = options.LineAttributesFilter
                .GetKeyValuePairs().ToArray();

            lineRepository = new FeatureRepository(
                types: options.LineTypes,
                attributesKey: options.LineAttributesKey,
                attributesFilter: lineAttributesFilter);
        }

        #endregion Public Constructors

        #region Public Methods

        public void Run()
        {
            using var infoPackage = progressWatcher.Initialize(
                allSteps: 2,
                status: "Convert SFA data.");

            LoadFiles(
                inputPaths: options.InputPaths,
                parentPackage: infoPackage);

            var lines = lineRepository.Features.Where(l => l.IsValid(
                lineFilters: options.LineFilters,
                attributesKey: options.LineAttributesKey)).ToArray();

            var routingWriter = new RoutingWriter();

            routingWriter.Write(
                lines: lines,
                path: options.OutputPath,
                parentPackage: infoPackage);
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadFeatures(string inputPath, IPackage parentPackage)
        {
            using var infoPackage = parentPackage.GetPackage(
                steps: 2,
                status: "Loading features.");

            var collectionRepository = new CollectionRepository();

            collectionRepository.Load(
                file: inputPath,
                parentPackage: infoPackage);

            lineRepository.Load(
                collection: collectionRepository.Collection,
                parentPackage: infoPackage);
        }

        private void LoadFiles(IEnumerable<string> inputPaths, IPackage parentPackage)
        {
            using var infoPackage = parentPackage.GetPackage(
                items: inputPaths,
                status: "Loading files.",
                weight: StatusWeightLoadingFiles);

            foreach (var inputPath in inputPaths)
            {
                LoadFeatures(
                    inputPath: inputPath,
                    parentPackage: infoPackage);
            }
        }

        private void OnProgressChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (onProgressChange != default)
            {
                var text = $"{progressWatcher.Status} ({progressWatcher.ProgressTip * 100:0}%)";

                onProgressChange.Invoke(
                    arg1: progressWatcher.ProgressAll,
                    arg2: text);
            }
        }

        private void OnProgressCompleted(object sender, EventArgs e)
        {
            if (onProgressChange != default)
            {
                progressWatcher.PropertyChanged -= OnProgressChanged;

                onProgressChange.Invoke(
                    arg1: 1,
                    arg2: default);
            }
        }

        #endregion Private Methods
    }
}