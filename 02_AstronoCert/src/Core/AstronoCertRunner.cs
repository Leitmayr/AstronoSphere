// ============================================================
// FILE: AstronoCertRunner.cs
// STATUS: FINAL (M1.9 + CategoryMapper + Prefix + StatusFilter)
// ============================================================

using AstronoCert.Core;
using AstronoData.Contracts.Domain;
using AstronoData.Contracts.Hashing;
using System;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AstronoCert
{
    public sealed class AstronoCertRunner
    {
        private readonly string _inputFolder =
            AstronoSpherePaths.GetSeedsPreparedFolder();

        private readonly string _outputFolder =
            AstronoSpherePaths.GetExperimentsReleasedFolder();

        private readonly string _processedFolder =
            AstronoSpherePaths.GetSeedsProcessedFolder();

        public void Run()
        {
            Console.WriteLine("=== AstronoCert ===");

            if (!Directory.Exists(_inputFolder))
            {
                Console.WriteLine("Input folder not found.");
                return;
            }

            Directory.CreateDirectory(_outputFolder);
            Directory.CreateDirectory(_processedFolder);

            var files = Directory.GetFiles(_inputFolder, "*.json")
                .OrderBy(f => f)
                .ToArray();

            if (!files.Any())
            {
                Console.WriteLine("No input files found.");
                return;
            }

            int catalogNumber = CatalogNumberGenerator.GetNextStart(_outputFolder);

            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var hashService = new HashService();

            foreach (var file in files)
            {
                Console.WriteLine($"Processing {file}...");

                var json = File.ReadAllText(file);
                var experiment = JsonSerializer.Deserialize<Experiment>(json);

                if (experiment == null)
                    throw new Exception($"Failed to deserialize: {file}");

                // =====================================================
                // 🔥 FIX: Status via JsonDocument lesen (wie Event!)
                // =====================================================
                using var statusDoc = JsonDocument.Parse(json);

                var maturity = statusDoc.RootElement
                    .GetProperty("Metadata")
                    .GetProperty("Status")
                    .GetProperty("Maturity")
                    .GetString();

                if (!string.Equals(maturity, "Released", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"[SKIP] Not Released: {file}");
                    continue;
                }
                // =====================================================

                experiment.DatasetHeader = null;
                experiment.ScenarioCitation = null;

                var canonical = hashService.BuildCanonical(experiment.Core);

                Console.WriteLine("[AstronoCert] CoreCanonical:");
                Console.WriteLine(canonical);

                var fullHash = hashService.ComputeHash(canonical);
                var shortHash = fullHash.Substring(0, 8);

                experiment.CoreHash = shortHash;

                experiment.ExperimentID = ExperimentIdGenerator.Generate(experiment.Core);

                if (string.IsNullOrWhiteSpace(experiment.CatalogNumber))
                {
                    experiment.CatalogNumber = $"AS-{catalogNumber:D6}";
                    catalogNumber++;
                }

                var core = experiment.Core;

                var bodyClass = core.ObservedObject.BodyClass.ToUpperInvariant();
                var target = core.ObservedObject.Targets[0].ToUpperInvariant();

                var eventJson = JsonSerializer.Serialize(experiment.Event);
                using var eventDoc = JsonDocument.Parse(eventJson);

                var category = eventDoc.RootElement.GetProperty("Category").GetString();

                var categoryAbbr = CategoryMapper.ToAbbreviation(category);

                var human = $"{bodyClass}-{target}-{categoryAbbr}";

                var fileName = $"{experiment.CatalogNumber}__{human}__{experiment.ExperimentID}.json";

                var outputPath = Path.Combine(_outputFolder, fileName);

                var outputJson = JsonSerializer.Serialize(experiment, jsonOptions);
                File.WriteAllText(outputPath, outputJson);

                Console.WriteLine($"Created {outputPath}");

                var processedPath = Path.Combine(_processedFolder, Path.GetFileName(file));

                if (File.Exists(processedPath))
                    File.Delete(processedPath);

                File.Move(file, processedPath);
            }

            Console.WriteLine("AstronoCert completed successfully.");
        }
    }
}