// ============================================================
// FILE: 03_AstronoTruth/src/EphemerisFactory/Core/FactoryRunner.cs
// STATUS: UPDATE (Fix: LastRun must NEVER be deleted + Parse fix)
// ============================================================

using AstronoData.Contracts.Domain;
using AstronoDiag;
using EphemerisRegression.Api;
using EphemerisRegression.Domain;
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

        private readonly string _diagRunFolder =
            AstronoSpherePaths.GetGroundTruthDiagMessagesRunFolder();

        private readonly string _diagLastRunFolder =
            AstronoSpherePaths.GetGroundTruthDiagMessagesLastRunFolder();

        private readonly DiagnosticRecordWriter _diagnosticWriter;

        public FactoryRunner()
        {
            _diagnosticWriter = new DiagnosticRecordWriter(_diagRunFolder);
        }

        public void Run()
        {
            Console.WriteLine("EphemerisFactory started...");

            ResetRunFolder();
            DiagnosticRunFolderManager.ResetRunToLastRun(_diagRunFolder, _diagLastRunFolder);

            var allFiles = Directory.GetFiles(_inputFolder, "*.json");
            Console.WriteLine($"Experiments found: {allFiles.Length}");

            Execute(allFiles.ToList());

            Console.WriteLine("Factory completed successfully.");
        }

        public void RunSingle(string catalogNumber)
        {
            Console.WriteLine($"Single run: {catalogNumber}");

            ResetRunFolder();
            DiagnosticRunFolderManager.ResetRunToLastRun(_diagRunFolder, _diagLastRunFolder);

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

                var experimentId = root.GetProperty("ExperimentID").GetString()!;
                var catalogNumber = root.GetProperty("CatalogNumber").GetString()!;

                Console.WriteLine($"Processing: {catalogNumber} | {experimentId}");

                var human = BuildHumanName(root);

                var preRequestDiagnostic = AstronoTruthDiagnosticGuard.EvaluatePreRequest(root);

                if (preRequestDiagnostic != null)
                {
                    _diagnosticWriter.Write(preRequestDiagnostic, human);
                    Console.WriteLine($"[DIAG] {preRequestDiagnostic.Code}: {catalogNumber}");
                    continue;
                }

                var request = HorizonsRequestBuilder.Build(root);
                var parameters = request.ToParameterDictionary();

                var (canonical, requestHash) =
                    HorizonsRequestBuilder.BuildCanonicalAndHash(parameters);

                var epochHash =
                    EphemerisRegression.Infrastructure.HashCalculator.ComputeSha256(requestHash);

                var requestUrl = BuildUrl(request);

                var client = new HorizonsApiClient();

                string raw;

                try
                {
                    raw = client.ExecuteAsync(request).Result;
                }
                catch (Exception ex)
                {
                    var diagnostic = AstronoTruthDiagnosticGuard.BuildRequestFailed(
                        root,
                        requestUrl,
                        "HttpRequestException",
                        ex.Message);

                    _diagnosticWriter.Write(diagnostic, human);
                    continue;
                }

                if (IsInvalidResponse(raw))
                {
                    var diagnostic = AstronoTruthDiagnosticGuard.BuildRequestFailed(
                        root,
                        requestUrl,
                        "InvalidHorizonsResponse",
                        BuildSnippet(raw));

                    _diagnosticWriter.Write(diagnostic, human);
                    continue;
                }

                var parsed = default(List<CsvRow>);

                try
                {
                    parsed = HorizonsCsvParser.ParseRaw(raw);
                }
                catch (Exception ex)
                {
                    var diagnostic = AstronoTruthDiagnosticGuard.BuildParseFailed(
                        root,
                        requestUrl,
                        "CSV",
                        ex.Message);

                    _diagnosticWriter.Write(diagnostic, human);
                    continue;
                }

                if (parsed.Count == 0)
                {
                    var diagnostic = AstronoTruthDiagnosticGuard.BuildParseFailed(
                        root,
                        requestUrl,
                        "CSV",
                        "No state vectors parsed.");

                    _diagnosticWriter.Write(diagnostic, human);
                    continue;
                }

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
                    requestUrl,
                    raw);

                File.WriteAllText(jsonFile, dataset);

                Console.WriteLine($"-> written: {Path.GetFileName(jsonFile)}");
            }
        }

        private void ResetRunFolder()
        {
            Console.WriteLine("Resetting GroundTruth Run folder...");

            Directory.CreateDirectory(_runFolder);
            Directory.CreateDirectory(_lastRunFolder);

            var runFiles = Directory.GetFiles(_runFolder);

            // Copy Run → LastRun (overwrite allowed)
            foreach (var file in runFiles)
            {
                var name = Path.GetFileName(file);
                if (name == ".gitkeep") continue;

                File.Copy(file, Path.Combine(_lastRunFolder, name), true);
            }

            // Clear Run ONLY
            foreach (var file in runFiles)
            {
                var name = Path.GetFileName(file);
                if (name == ".gitkeep") continue;

                File.Delete(file);
            }
        }

        private static string BuildHumanName(JsonElement root)
        {
            var core = root.GetProperty("Core");
            var observedObject = core.GetProperty("ObservedObject");

            var bodyClass = observedObject.GetProperty("BodyClass").GetString()!;
            var target = observedObject.GetProperty("Targets")[0].GetString()!;

            var eventNode = root.GetProperty("Event");
            var category = eventNode.GetProperty("Category").GetString()!;

            var categoryAbbr = CategoryMapper.ToAbbreviation(category);

            return $"{bodyClass.ToUpperInvariant()}-{target.ToUpperInvariant()}-{categoryAbbr}";
        }

        private static bool IsInvalidResponse(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return true;
            if (raw.Contains("No ephemeris", StringComparison.OrdinalIgnoreCase)) return true;
            if (!raw.Contains("$$SOE")) return true;
            return false;
        }

        private static string BuildSnippet(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return "<empty>";

            return raw.Length <= 500
                ? raw
                : raw.Substring(0, 500);
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

        public void RunSingleByNumber(int id)
        {
            RunSingle($"AS-{id:D6}");
        }
    }
}