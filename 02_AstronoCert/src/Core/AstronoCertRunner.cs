// ============================================================
// FILE: AstronoCertRunner.cs
// STATUS: FIX (JSON Encoding - Special Characters)
// ============================================================

using AstronoCert.Core;
using AstronoData.Contracts.Hashing;
using System;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;

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

            var files = Directory.GetFiles(_inputFolder, "SCN_*.json");

            if (!files.Any())
            {
                Console.WriteLine("No input files found.");
                return;
            }

            int catalogNumber = CatalogNumberGenerator.GetNextStart(_outputFolder);

            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            var hashService = new HashService();

            foreach (var file in files.OrderBy(f => f))
            {
                var json = File.ReadAllText(file);
                var experiment = JsonSerializer.Deserialize<Experiment>(json);

                if (experiment == null)
                    throw new Exception($"Failed to deserialize: {file}");

                // --- Core Hash ---
                var canonical = hashService.BuildCanonical(experiment.Core);

                Console.WriteLine("[AstronoCert] CoreCanonical:");
                Console.WriteLine(canonical);

                var fullHash = hashService.ComputeHash(canonical);
                var shortHash = fullHash.Substring(0, 8);

                Console.WriteLine("[AstronoCert] CoreHash:");
                Console.WriteLine(shortHash);

                experiment.CoreHash = shortHash;

                // --- Experiment ID ---
                experiment.ExperimentID = ExperimentIdGenerator.Generate(experiment.Core);

                // --- Catalog Number ---
                experiment.CatalogNumber = $"AS-{catalogNumber:D6}";
                catalogNumber++;

                // --- Write Output ---
                var outputFileName = $"{experiment.CatalogNumber}.json";
                var outputPath = Path.Combine(_outputFolder, outputFileName);

                var outputJson = JsonSerializer.Serialize(experiment, jsonOptions);
                File.WriteAllText(outputPath, outputJson);

                // --- Move processed seed ---
                var fileName = Path.GetFileName(file);
                var processedPath = Path.Combine(_processedFolder, fileName);

                if (File.Exists(processedPath))
                    File.Delete(processedPath);

                File.Move(file, processedPath);
            }

            Console.WriteLine("AstronoCert completed successfully.");
        }
    }
}