// ============================================================
// FILE: 03_TruthFactory/src/EphemerisFactory/Core/FactoryRunner.cs
// STATUS: ÄNDERUNG (gitkeep handling)
// ============================================================

using AstronoMeasurement.Builder;
using AstronoMeasurement.Defaults;
using AstronoMeasurement.Keys;
using EphemerisRegression.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace EphemerisFactory.Core
{
    public sealed class FactoryRunner
    {
        private readonly string _inputFolder =
            AstronoSpherePaths.GetObservationCatalogReleasedFolder();

        private readonly string _runFolder =
            Path.Combine(
                AstronoSpherePaths.GetAstronoDataRoot(),
                "03_ReferenceData",
                "Runs",
                "Run");

        private readonly string _lastRunFolder =
            Path.Combine(
                AstronoSpherePaths.GetAstronoDataRoot(),
                "03_ReferenceData",
                "Runs",
                "LastRun");

        public void Run()
        {
            Console.WriteLine("=== EphemerisFactory v1 ===");
            Console.WriteLine("EphemerisFactory started...");

            Directory.CreateDirectory(_runFolder);
            Directory.CreateDirectory(_lastRunFolder);

            // ============================================================
            // 1) RESET: Run → LastRun
            // ============================================================

            ResetRunFolder();

            var files = Directory.GetFiles(_inputFolder, "*.json");

            Console.WriteLine($"Scenarios found: {files.Length}");

            var definitions = MeasurementDefaults.GetDefault();
            var measurementBuilder = new MeasurementBuilder();
            List<MeasurementKey> measurements = measurementBuilder.Build(definitions);

            var measurement = measurements[0];
            var level = measurement.Level;

            foreach (var file in files)
            {
                var json = File.ReadAllText(file);

                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                ValidateStatus(root, file);

                var scenarioId = root.GetProperty("ScenarioID").GetString()!;

                Console.WriteLine($"Processing: {scenarioId}");

                var request = HorizonsRequestBuilder.Build(root);

                var parameters = request.ToParameterDictionary();
                var canonical = CanonicalRequestBuilder.Build(parameters);

                var hash = HashCalculator.ComputeSha256(canonical);
                var epochHash = HashCalculator.ComputeSha256(hash);

                var url = BuildUrl(canonical);

                var client = new HorizonsApiClient();
                var raw = client.ExecuteAsync(request).Result;

                var csvFile = Path.Combine(_runFolder, $"{scenarioId}_{level}.csv");
                var jsonFile = Path.Combine(_runFolder, $"{scenarioId}_{level}.json");

                File.WriteAllText(csvFile, raw);

                var dataset = DatasetBuilder.Build(
                    json,
                    canonical,
                    hash,
                    epochHash,
                    level,
                    url,
                    raw);

                File.WriteAllText(jsonFile, dataset);

                Console.WriteLine($"  -> written: {Path.GetFileName(jsonFile)}");
            }

            Console.WriteLine("Factory completed successfully.");
        }

        // ============================================================
        // RESET LOGIC (gitkeep FIX)
        // ============================================================

        private void ResetRunFolder()
        {
            Console.WriteLine("Resetting Run folder...");

            var runFiles = Directory.GetFiles(_runFolder);

            int moved = 0;

            foreach (var file in runFiles)
            {
                var fileName = Path.GetFileName(file);

                // 🔥 FIX: .gitkeep ignorieren
                if (fileName.Equals(".gitkeep", StringComparison.OrdinalIgnoreCase))
                    continue;

                var target = Path.Combine(_lastRunFolder, fileName);

                File.Copy(file, target, overwrite: true);
                moved++;
            }

            if (moved == 0)
            {
                Console.WriteLine("Run folder empty → nothing to move.");
            }
            else
            {
                Console.WriteLine($"Moved {moved} files to LastRun.");
            }

            // 🔥 Run leeren (ohne .gitkeep)
            foreach (var file in runFiles)
            {
                var fileName = Path.GetFileName(file);

                if (fileName.Equals(".gitkeep", StringComparison.OrdinalIgnoreCase))
                    continue;

                File.Delete(file);
            }

            Console.WriteLine("Run folder cleared.");
        }

        // ============================================================
        // VALIDATION
        // ============================================================

        private static void ValidateStatus(JsonElement root, string file)
        {
            if (!root.TryGetProperty("Status", out var status))
                throw new Exception($"Missing Status in scenario: {file}");

            var maturity = status.GetProperty("maturity").GetString();

            if (maturity != "released")
                throw new Exception($"Scenario not released: {file}");
        }

        // ============================================================
        // URL BUILDER
        // ============================================================

        private static string BuildUrl(string canonical)
        {
            var encoded = Uri.EscapeDataString(canonical);
            return $"https://ssd.jpl.nasa.gov/api/horizons.api?format=text&input={encoded}";
        }
    }
}