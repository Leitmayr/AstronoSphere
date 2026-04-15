// ============================================================
// FILE: FactoryRunner.cs
// STATUS: FINAL (M1.9 + CategoryMapper integration + PARAMETER HASH)
// ============================================================

using AstronoData.Contracts.Domain;
using EphemerisRegression.Api;
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
            AstronoSpherePaths.GetExperimentsReleasedFolder();

        private readonly string _runFolder =
            AstronoSpherePaths.GetGroundTruthRunFolder();

        private readonly string _lastRunFolder =
            AstronoSpherePaths.GetGroundTruthLastRunFolder();

        public void Run()
        {
            Console.WriteLine("EphemerisFactory started...");

            Directory.CreateDirectory(_runFolder);
            Directory.CreateDirectory(_lastRunFolder);

            ResetRunFolder();

            var allFiles = Directory.GetFiles(_inputFolder, "*.json");
            Console.WriteLine($"Experiments found: {allFiles.Length}");

            Execute(allFiles.ToList());

            Console.WriteLine("Factory completed successfully.");
        }

        public void RunSingleByNumber(int id)
        {
            RunSingle($"AS-{id:D6}");
        }

        public void RunSingle(string catalogNumber)
        {
            Console.WriteLine($"Single run: {catalogNumber}");

            Directory.CreateDirectory(_runFolder);
            Directory.CreateDirectory(_lastRunFolder);

            ResetRunFolder();

            var file = Directory
                .GetFiles(_inputFolder, "*.json")
                .FirstOrDefault(f =>
                {
                    var json = File.ReadAllText(f);
                    using var doc = JsonDocument.Parse(json);

                    return doc.RootElement
                        .GetProperty("CatalogNumber")
                        .GetString() == catalogNumber;
                });

            if (file == null)
                throw new Exception($"Experiment not found: {catalogNumber}");

            Execute(new List<string> { file });
        }

        private void Execute(List<string> files)
        {
            var level = "L0";

            foreach (var file in files)
            {
                var json = File.ReadAllText(file);

                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                ValidateStatus(root, file);

                var experimentId = root.GetProperty("ExperimentID").GetString()!;
                var catalogNumber = root.GetProperty("CatalogNumber").GetString()!;

                Console.WriteLine($"Processing: {catalogNumber} | {experimentId}");

                // =====================================================
                // HUMAN NAME (M1.9 FINAL)
                // =====================================================

                var core = root.GetProperty("Core");
                var observedObject = core.GetProperty("ObservedObject");

                var bodyClass = observedObject.GetProperty("BodyClass").GetString()!;
                var target = observedObject.GetProperty("Targets")[0].GetString()!;

                var eventNode = root.GetProperty("Event");
                var category = eventNode.GetProperty("Category").GetString()!;

                var categoryAbbr = CategoryMapper.ToAbbreviation(category);
                var human = $"{bodyClass.ToUpperInvariant()}-{target.ToUpperInvariant()}-{categoryAbbr}";

                // =====================================================
                // REQUEST + CANONICAL + PARAMETER HASH
                // =====================================================

                var request = HorizonsRequestBuilder.Build(root);
                var parameters = request.ToParameterDictionary();

                var (canonical, requestHash) =
                    HorizonsRequestBuilder.BuildCanonicalAndHash(parameters);

                // EpochHash bleibt bewusst unverändert
                var epochHash = EphemerisRegression.Infrastructure.HashCalculator.ComputeSha256(requestHash);

                // =====================================================
                // CALL
                // =====================================================

                var client = new HorizonsApiClient();
                var raw = client.ExecuteAsync(request).Result;

                if (IsInvalidResponse(raw))
                {
                    Console.WriteLine($"[SKIP] Invalid ephemeris for {experimentId}");
                    continue;
                }

                var parsed = HorizonsCsvParser.ParseRaw(raw);

                if (parsed.Count == 0)
                {
                    Console.WriteLine($"[SKIP] No data returned for {experimentId}");
                    continue;
                }

                // =====================================================
                // FILENAME (M1.9 FINAL)
                // =====================================================

                var datasetSuffix = $"EPH-HORIZONS-VEC-{level}";
                var fileName = $"{human}__{experimentId}__{datasetSuffix}";

                var csvFile = Path.Combine(_runFolder, fileName + ".csv");
                var jsonFile = Path.Combine(_runFolder, fileName + ".json");

                File.WriteAllText(csvFile, raw);

                var dataset = DatasetBuilder.Build(
                    json,
                    canonical,
                    requestHash,
                    epochHash,
                    level,
                    BuildUrl(request),
                    raw);

                File.WriteAllText(jsonFile, dataset);

                Console.WriteLine($"-> written: {Path.GetFileName(jsonFile)}");
            }
        }

        // =====================================================
        // HELPERS
        // =====================================================

        private static bool IsInvalidResponse(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return true;
            if (raw.Contains("No ephemeris", StringComparison.OrdinalIgnoreCase)) return true;
            if (!raw.Contains("$$SOE")) return true;
            return false;
        }

        private void ResetRunFolder()
        {
            Console.WriteLine("Resetting Run folder...");

            var runFiles = Directory.GetFiles(_runFolder);

            foreach (var file in runFiles)
            {
                var name = Path.GetFileName(file);
                if (name == ".gitkeep") continue;

                File.Copy(file, Path.Combine(_lastRunFolder, name), true);
            }

            foreach (var file in runFiles)
            {
                var name = Path.GetFileName(file);
                if (name == ".gitkeep") continue;

                File.Delete(file);
            }
        }

        private static void ValidateStatus(JsonElement root, string file)
        {
            var maturity = root
                .GetProperty("Metadata")
                .GetProperty("Status")
                .GetProperty("Maturity")
                .GetString();

            if (!string.Equals(maturity, "Released", StringComparison.OrdinalIgnoreCase))
                throw new Exception($"Experiment not released: {file}");
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