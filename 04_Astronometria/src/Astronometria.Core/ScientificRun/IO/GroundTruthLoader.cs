using System;
using System.Collections.Generic;
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
            {
                throw new DirectoryNotFoundException(
                    $"GroundTruth Baseline folder not found: {baselineFolder}");
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var matches = new List<(string Path, GroundTruthInputModel Model)>();

            foreach (var file in Directory.GetFiles(baselineFolder, "*.json"))
            {
                var json = File.ReadAllText(file);

                var gt = JsonSerializer.Deserialize<GroundTruthInputModel>(
                    json,
                    options);

                if (gt == null)
                    continue;

                var measurementMatches =
                    string.Equals(
                        gt.DatasetHeader.Measurement.Type,
                        "VEC",
                        StringComparison.OrdinalIgnoreCase)
                    &&
                    string.Equals(
                        gt.DatasetHeader.Measurement.Level,
                        "L0",
                        StringComparison.OrdinalIgnoreCase);

                var catalogMatches =
                    string.Equals(
                        gt.ExperimentRef.CatalogNumber,
                        experiment.CatalogNumber,
                        StringComparison.OrdinalIgnoreCase);

                var coreHashMatches =
                    string.Equals(
                        gt.ExperimentRef.CoreHash,
                        experiment.CoreHash,
                        StringComparison.OrdinalIgnoreCase);

                if (measurementMatches
                    && catalogMatches
                    && coreHashMatches)
                {
                    matches.Add((file, gt));
                }
            }

            if (matches.Count == 0)
            {
                throw new GroundTruthResolutionException(
                    "040.009",
                    $"No matching GroundTruth baseline found for CatalogNumber '{experiment.CatalogNumber}' and CoreHash '{experiment.CoreHash}'.");
            }

            if (matches.Count > 1)
            {
                throw new GroundTruthResolutionException(
                    "040.002",
                    $"Multiple matching GroundTruth baselines found for CatalogNumber '{experiment.CatalogNumber}' and CoreHash '{experiment.CoreHash}'.",
                    matches
                        .Select(x => Path.GetFileName(x.Path))
                        .ToList());
            }

            var match = matches[0];

            match.Model.SourceFile = Path.GetFileName(match.Path);

            ValidateGroundTruth(
                experiment,
                match.Model,
                match.Path);

            return match.Model;
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

            if (string.IsNullOrWhiteSpace(
                    groundTruth.DatasetHeader.DatasetID))
            {
                throw new InvalidOperationException(
                    $"GroundTruth DatasetID missing in '{sourceFile}'.");
            }

            if (string.IsNullOrWhiteSpace(
                    groundTruth.DatasetHeader.TruthMetadata.RequestHash))
            {
                throw new InvalidOperationException(
                    $"GroundTruth RequestHash missing in '{sourceFile}'.");
            }

            if (groundTruth.Data.Count == 0)
            {
                throw new InvalidOperationException(
                    $"GroundTruth contains no data samples in '{sourceFile}'.");
            }
        }
    }

    public sealed class GroundTruthResolutionException : Exception
    {
        public GroundTruthResolutionException(
            string diagnosticCode,
            string message,
            IReadOnlyList<string>? matchingFiles = null)
            : base(message)
        {
            DiagnosticCode = diagnosticCode;
            MatchingFiles = matchingFiles ?? Array.Empty<string>();
        }

        public string DiagnosticCode { get; }

        public IReadOnlyList<string> MatchingFiles { get; }
    }
}