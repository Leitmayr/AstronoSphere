using EphemerisRegression.Infrastructure;
using System;
using System.IO;
using System.Text.Json;

namespace EphemerisFactory.Core
{
    public sealed class FactoryRunner
    {
        private readonly string _inputFolder =
            AstronoSpherePaths.GetObservationCatalogReleasedFolder();

        private readonly string _outputFolder =
            Path.Combine(
                AstronoSpherePaths.GetAstronoDataRoot(),
                "03_ReferenceData",
                "Runs",
                "Run"
            );

        public void Run()
        {
            Console.WriteLine("=== EphemerisFactory v1 ===");
            Console.WriteLine("=== EphemerisFactory L0 ===");

            Directory.CreateDirectory(_outputFolder);

            var files = Directory.GetFiles(_inputFolder, "*.json");

            Console.WriteLine($"Scenarios found: {files.Length}");

            foreach (var file in files)
            {
                var json = File.ReadAllText(file);
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                ValidateStatus(root, file);

                var scenarioId = root.GetProperty("ScenarioID").GetString()!;

                Console.WriteLine($"Processing: {scenarioId}");

                // =====================================================
                // REQUEST BUILD
                // =====================================================

                var request = HorizonsRequestBuilder.Build(root);

                var parameters = request.ToParameterDictionary();
                var canonical = CanonicalRequestBuilder.Build(parameters);

                var hash = HashCalculator.ComputeSha256(canonical);
                var epochHash = HashCalculator.ComputeSha256(hash);

                var url = BuildUrl(canonical);

                // =====================================================
                // HORIZONS CALL
                // =====================================================

                var client = new HorizonsApiClient();

                var raw = client.ExecuteAsync(request).Result;


                Console.WriteLine("----- RAW DEBUG START -----");

                // erste 30 Zeilen anzeigen
                var lines = raw.Split('\n');

                for (int i = 0; i < Math.Min(30, lines.Length); i++)
                {
                    Console.WriteLine(lines[i]);
                }

                Console.WriteLine("----- RAW DEBUG END -----");

                // CSV speichern
                var csvFile = Path.Combine(_outputFolder, $"{scenarioId}_L0.csv");
                File.WriteAllText(csvFile, raw);

                // Dataset bauen (JETZT MIT DATA!)
                var dataset = DatasetBuilder.Build(
                    json,
                    canonical,
                    hash,
                    epochHash,
                    "L0",
                    url,
                    raw
                );

                var jsonFile = Path.Combine(_outputFolder, $"{scenarioId}_L0.json");
                File.WriteAllText(jsonFile, dataset);

                Console.WriteLine($"  → written: {Path.GetFileName(jsonFile)}");
                Console.WriteLine($"  → raw: {Path.GetFileName(csvFile)}");
            }

            Console.WriteLine("Factory completed successfully.");
        }

        // =====================================================
        // VALIDATION
        // =====================================================

        private static void ValidateStatus(JsonElement root, string file)
        {
            if (!root.TryGetProperty("Status", out var status))
                throw new Exception($"Missing Status in scenario: {file}");

            var maturity = status.GetProperty("maturity").GetString();

            if (maturity != "released")
                throw new Exception($"Scenario not released: {file}");
        }

        // =====================================================
        // URL BUILDER
        // =====================================================

        private static string BuildUrl(string canonical)
        {
            var encoded = Uri.EscapeDataString(canonical);
            return $"https://ssd.jpl.nasa.gov/api/horizons.api?format=text&input={encoded}";
        }
    }
}