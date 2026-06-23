using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Astronometria.Core.ScientificRun.Models;

namespace Astronometria.Core.ScientificRun.IO
{
    public static class ExperimentLoader
    {
        public static ExperimentInputModel LoadByCatalogNumber(
            string repositoryRoot,
            string catalogNumber)
        {
            if (string.IsNullOrWhiteSpace(repositoryRoot))
                throw new ArgumentException("Repository root must not be empty.", nameof(repositoryRoot));

            if (string.IsNullOrWhiteSpace(catalogNumber))
                throw new ArgumentException("Catalog number must not be empty.", nameof(catalogNumber));

            var releasedFolder = Path.Combine(
                repositoryRoot,
                "AstronoData",
                "02_Experiments",
                "Released");

            if (!Directory.Exists(releasedFolder))
                throw new DirectoryNotFoundException(
                    $"Experiment Released folder not found: {releasedFolder}");

            var matches = Directory
                .GetFiles(releasedFolder, $"{catalogNumber}__*.json")
                .OrderBy(path => path, StringComparer.Ordinal)
                .ToList();

            if (matches.Count == 0)
                throw new FileNotFoundException(
                    $"No released experiment found for catalog number '{catalogNumber}'.");

            if (matches.Count > 1)
                throw new InvalidOperationException(
                    $"Multiple released experiments found for catalog number '{catalogNumber}'.");

            var sourceFile = matches[0];
            var json = File.ReadAllText(sourceFile);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var experiment = JsonSerializer.Deserialize<ExperimentInputModel>(json, options);

            if (experiment == null)
                throw new InvalidOperationException(
                    $"Could not deserialize experiment file: {sourceFile}");

            experiment.SourceFile = Path.GetFileName(sourceFile);

            ValidateExperimentIdentity(experiment, catalogNumber, sourceFile);

            return experiment;
        }

        private static void ValidateExperimentIdentity(
            ExperimentInputModel experiment,
            string expectedCatalogNumber,
            string sourceFile)
        {
            if (!string.Equals(
                    experiment.CatalogNumber,
                    expectedCatalogNumber,
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    $"CatalogNumber mismatch in '{sourceFile}'. Expected '{expectedCatalogNumber}', actual '{experiment.CatalogNumber}'.");
            }

            if (string.IsNullOrWhiteSpace(experiment.ExperimentID))
                throw new InvalidOperationException($"ExperimentID missing in '{sourceFile}'.");

            if (string.IsNullOrWhiteSpace(experiment.CoreHash))
                throw new InvalidOperationException($"CoreHash missing in '{sourceFile}'.");

            if (experiment.Core.ObservedObject.Targets.Count != 1)
                throw new InvalidOperationException(
                    $"ScientificRun requires exactly one target. File: {sourceFile}");
        }
    }
}