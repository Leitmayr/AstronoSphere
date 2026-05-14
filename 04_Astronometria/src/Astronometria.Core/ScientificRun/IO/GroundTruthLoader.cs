using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Astronometria.Core.ScientificRun.Models;

namespace Astronometria.Core.ScientificRun.IO
{
    public static class GroundTruthLoader
    {
        public static GroundTruthInputModel ResolveSingleBaseline(
            string repositoryRoot,
            ExperimentInputModel experiment)
        {
            if (string.IsNullOrWhiteSpace(repositoryRoot))
                throw new ArgumentException("Repository root must not be empty.", nameof(repositoryRoot));

            if (experiment == null)
                throw new ArgumentNullException(nameof(experiment));

            var baselineFolder = Path.Combine(
                repositoryRoot,
                "AstronoData",
                "03_GroundTruth",
                "Ephemeris",
                "Horizons",
                "Baseline");

            if (!Directory.Exists(baselineFolder))
                throw new DirectoryNotFoundException(
                    $"GroundTruth Baseline folder not found: {baselineFolder}");

            var matches = Directory
                .GetFiles(baselineFolder, "*.json")
                .Where(path => FileContainsExperimentId(path, experiment.ExperimentID))
                .OrderBy(path => path, StringComparer.Ordinal)
                .ToList();

            if (matches.Count == 0)
                throw new FileNotFoundException(
                    $"No matching GroundTruth baseline found for ExperimentID '{experiment.ExperimentID}'.");

            if (matches.Count > 1)
                throw new InvalidOperationException(
                    $"Multiple matching GroundTruth baselines found for ExperimentID '{experiment.ExperimentID}'.");

            var sourceFile = matches[0];
            var json = File.ReadAllText(sourceFile);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var groundTruth = JsonSerializer.Deserialize<GroundTruthInputModel>(json, options);

            if (groundTruth == null)
                throw new InvalidOperationException(
                    $"Could not deserialize GroundTruth file: {sourceFile}");

            groundTruth.SourceFile = Path.GetFileName(sourceFile);

            ValidateGroundTruth(experiment, groundTruth, sourceFile);

            return groundTruth;
        }

        private static bool FileContainsExperimentId(string path, string experimentId)
        {
            var text = File.ReadAllText(path);
            return text.Contains(experimentId, StringComparison.Ordinal);
        }

        private static void ValidateGroundTruth(
            ExperimentInputModel experiment,
            GroundTruthInputModel groundTruth,
            string sourceFile)
        {
            if (!string.Equals(
                    groundTruth.ExperimentRef.ExperimentID,
                    experiment.ExperimentID,
                    StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    $"GroundTruth ExperimentID mismatch in '{sourceFile}'.");
            }

            if (!string.Equals(
                    groundTruth.ExperimentRef.CatalogNumber,
                    experiment.CatalogNumber,
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    $"GroundTruth CatalogNumber mismatch in '{sourceFile}'.");
            }

            if (!string.Equals(
                    groundTruth.ExperimentRef.CoreHash,
                    experiment.CoreHash,
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    $"GroundTruth CoreHash mismatch in '{sourceFile}'.");
            }

            if (string.IsNullOrWhiteSpace(groundTruth.DatasetHeader.DatasetID))
                throw new InvalidOperationException($"GroundTruth DatasetID missing in '{sourceFile}'.");

            if (string.IsNullOrWhiteSpace(groundTruth.DatasetHeader.TruthMetadata.RequestHash))
                throw new InvalidOperationException($"GroundTruth RequestHash missing in '{sourceFile}'.");

            if (groundTruth.Data.Count == 0)
                throw new InvalidOperationException($"GroundTruth contains no data samples in '{sourceFile}'.");
        }
    }
}