// ============================================================
// FILE: 03_TruthFactory/src/EphemerisFactory/Core/FactoryRunner.cs
// STATUS: GEÄNDERT (RC7 – TruthProviderUrl Fix)
// ============================================================

using AstronoMeasurement.Builder;
using AstronoMeasurement.Defaults;
using AstronoMeasurement.Keys;
using EphemerisRegression.Api;
using EphemerisRegression.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

            // ============================================================
            // 2) FILTER
            // ============================================================

            var options = ParseCommandLineArgs();
            PrintFilterInfo(options);

            var allFiles = Directory.GetFiles(_inputFolder, "*.json");
            Console.WriteLine($"Scenarios found (total): {allFiles.Length}");

            var files = ApplyCatalogFilter(allFiles, options);
            Console.WriteLine($"Scenarios selected     : {files.Count}");

            // ============================================================
            // 3) MEASUREMENT (M1: L0)
            // ============================================================

            var definitions = MeasurementDefaults.GetDefault();
            var measurementBuilder = new MeasurementBuilder();
            List<MeasurementKey> measurements = measurementBuilder.Build(definitions);

            var measurement = measurements[0];
            var level = measurement.Level;

            // ============================================================
            // 4) MAIN LOOP
            // ============================================================

            foreach (var file in files)
            {
                Console.WriteLine($"[DEBUG] Reading file: {file}");

                var json = File.ReadAllText(file);

                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                ValidateStatus(root, file);

                var scenarioId = root.GetProperty("ScenarioID").GetString()!;
                var catalogNumber = root.GetProperty("CatalogNumber").GetString()!;

                Console.WriteLine($"Processing: {catalogNumber} | {scenarioId}");

                var request = HorizonsRequestBuilder.Build(root);

                var parameters = request.ToParameterDictionary();
                var canonical = CanonicalRequestBuilder.Build(parameters);

                var hash = HashCalculator.ComputeSha256(canonical);
                var epochHash = HashCalculator.ComputeSha256(hash);

                // RC7 FIX:
                // TruthProviderUrl muss identisch zum echten API-Call sein.
                var url = BuildUrl(request);

                var client = new HorizonsApiClient();
                var raw = client.ExecuteAsync(request).Result;

                // ============================================================
                // RC6 – INVALID RESPONSE DETECTION
                // ============================================================

                if (IsInvalidResponse(raw))
                {
                    Console.WriteLine($"[SKIP] Invalid ephemeris for {scenarioId}");
                    continue;
                }

                var parsed = HorizonsCsvParser.ParseRaw(raw);

                if (parsed.Count == 0)
                {
                    Console.WriteLine($"[SKIP] No data returned for {scenarioId}");
                    continue;
                }

                // ============================================================
                // WRITE OUTPUT
                // ============================================================

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
        // RC6 – HELPER
        // ============================================================

        private static bool IsInvalidResponse(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return true;

            if (raw.Contains("No ephemeris", StringComparison.OrdinalIgnoreCase))
                return true;

            if (!raw.Contains("$$SOE"))
                return true;

            return false;
        }

        // ============================================================
        // FILTER
        // ============================================================

        private static CatalogFilterOptions ParseCommandLineArgs()
        {
            var options = new CatalogFilterOptions();

            var args = Environment.GetCommandLineArgs();

            foreach (var arg in args)
            {
                if (arg.StartsWith("--catalog-from=", StringComparison.OrdinalIgnoreCase))
                {
                    options.CatalogFrom = NormalizeCatalogNumber(
                        arg.Substring("--catalog-from=".Length));
                }
                else if (arg.StartsWith("--catalog-to=", StringComparison.OrdinalIgnoreCase))
                {
                    options.CatalogTo = NormalizeCatalogNumber(
                        arg.Substring("--catalog-to=".Length));
                }
            }

            if (options.CatalogFrom is not null && options.CatalogTo is not null)
            {
                if (string.CompareOrdinal(options.CatalogFrom, options.CatalogTo) > 0)
                    throw new Exception(
                        $"Invalid catalog range: from '{options.CatalogFrom}' is greater than to '{options.CatalogTo}'.");
            }

            return options;
        }

        private static void PrintFilterInfo(CatalogFilterOptions options)
        {
            if (options.CatalogFrom is null && options.CatalogTo is null)
            {
                Console.WriteLine("Catalog filter         : none");
                return;
            }

            Console.WriteLine(
                $"Catalog filter         : from={options.CatalogFrom ?? "-"} to={options.CatalogTo ?? "-"}");
        }

        private static List<string> ApplyCatalogFilter(
            string[] files,
            CatalogFilterOptions options)
        {
            var result = new List<(string File, string CatalogNumber)>();

            foreach (var file in files)
            {
                Console.WriteLine($"[DEBUG] Filtering file: {file}");

                var json = File.ReadAllText(file);

                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (!root.TryGetProperty("CatalogNumber", out var catalogElement))
                    throw new Exception($"Missing CatalogNumber in scenario: {file}");

                var catalogNumberRaw = catalogElement.GetString();
                if (string.IsNullOrWhiteSpace(catalogNumberRaw))
                    throw new Exception($"Empty CatalogNumber in scenario: {file}");

                var catalogNumber = NormalizeCatalogNumber(catalogNumberRaw);

                if (!IsInCatalogRange(catalogNumber, options))
                    continue;

                ValidateStatus(root, file);

                result.Add((file, catalogNumber));
            }

            return result
                .OrderBy(x => x.CatalogNumber, StringComparer.Ordinal)
                .Select(x => x.File)
                .ToList();
        }

        private static bool IsInCatalogRange(
            string catalogNumber,
            CatalogFilterOptions options)
        {
            if (options.CatalogFrom is not null &&
                string.CompareOrdinal(catalogNumber, options.CatalogFrom) < 0)
            {
                return false;
            }

            if (options.CatalogTo is not null &&
                string.CompareOrdinal(catalogNumber, options.CatalogTo) > 0)
            {
                return false;
            }

            return true;
        }

        private static string NormalizeCatalogNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new Exception("CatalogNumber must not be empty.");

            var trimmed = value.Trim().ToUpperInvariant();

            if (!trimmed.StartsWith("AS-", StringComparison.Ordinal))
                throw new Exception(
                    $"Invalid CatalogNumber '{value}'. Expected format AS-XXXXXX.");

            var numberPart = trimmed.Substring(3);

            if (numberPart.Length != 6)
                throw new Exception(
                    $"Invalid CatalogNumber '{value}'. Expected exactly 6 digits.");

            foreach (var c in numberPart)
            {
                if (!char.IsDigit(c))
                    throw new Exception(
                        $"Invalid CatalogNumber '{value}'. Expected digits only after AS-.");
            }

            return $"AS-{numberPart}";
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
        // RC7 – URL BUILDER
        // Muss identisch zum tatsächlichen API-Call sein.
        // ============================================================

        private static string BuildUrl(HorizonsApiRequest request)
        {
            var parameters = request.ToParameterDictionary();

            var sb = new StringBuilder();
            sb.Append("https://ssd.jpl.nasa.gov/api/horizons.api?format=text");

            foreach (var kv in parameters)
            {
                sb.Append("&");
                sb.Append(kv.Key);
                sb.Append("=");
                sb.Append(Uri.EscapeDataString(kv.Value));
            }

            return sb.ToString();
        }

        private sealed class CatalogFilterOptions
        {
            public string? CatalogFrom { get; set; }
            public string? CatalogTo { get; set; }
        }
    }
}