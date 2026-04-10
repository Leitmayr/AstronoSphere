// ============================================================
// FILE: 03_TruthFactory/src/EphemerisFactory/Core/FactoryRunner.cs
// STATUS: UPDATE (Remove AstronoMeasurement dependency - KISS)
// ============================================================

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
            Console.WriteLine("EphemerisFactory started...");

            Directory.CreateDirectory(_runFolder);
            Directory.CreateDirectory(_lastRunFolder);

            ResetRunFolder();

            var allFiles = Directory.GetFiles(_inputFolder, "*.json");
            Console.WriteLine($"Scenarios found (total): {allFiles.Length}");

            Execute(allFiles.ToList());

            Console.WriteLine("Factory completed successfully.");
        }

        public void RunSingle(string experimentId)
        {
            Console.WriteLine($"Single run: {experimentId}");

            Directory.CreateDirectory(_runFolder);
            Directory.CreateDirectory(_lastRunFolder);

            ResetRunFolder();

            var file = Directory
                .GetFiles(_inputFolder, "*.json")
                .FirstOrDefault(f =>
                {
                    var json = File.ReadAllText(f);
                    using var doc = JsonDocument.Parse(json);
                    var root = doc.RootElement;

                    if (!root.TryGetProperty("CatalogNumber", out var cat))
                        return false;

                    return string.Equals(
                        cat.GetString(),
                        experimentId,
                        StringComparison.OrdinalIgnoreCase);
                });

            if (file == null)
                throw new Exception($"Experiment not found: {experimentId}");

            Execute(new List<string> { file });
        }

        public void RunSingleByNumber(int id)
        {
            RunSingle($"AS-{id:D6}");
        }

        private void Execute(List<string> files)
        {
            // 🔥 FIX: Measurement entfernt (M1 = konstant)
            var level = "L0";

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

                var url = BuildUrl(request);

                var client = new HorizonsApiClient();
                var raw = client.ExecuteAsync(request).Result;

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
        }

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

        private void ResetRunFolder()
        {
            Console.WriteLine("Resetting Run folder...");

            var runFiles = Directory.GetFiles(_runFolder);

            foreach (var file in runFiles)
            {
                var fileName = Path.GetFileName(file);

                if (fileName.Equals(".gitkeep", StringComparison.OrdinalIgnoreCase))
                    continue;

                var target = Path.Combine(_lastRunFolder, fileName);
                File.Copy(file, target, overwrite: true);
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

        private static void ValidateStatus(JsonElement root, string file)
        {
            if (!root.TryGetProperty("Status", out var status))
                throw new Exception($"Missing Status in scenario: {file}");

            var maturity = status.GetProperty("maturity").GetString();

            if (maturity != "released")
                throw new Exception($"Scenario not released: {file}");
        }

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
    }
}