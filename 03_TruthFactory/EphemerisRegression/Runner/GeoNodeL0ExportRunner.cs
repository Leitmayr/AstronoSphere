using EphemerisRegression.Api;
using EphemerisRegression.Config;
using EphemerisRegression.Domain;
using EphemerisRegression.Event;
using EphemerisRegression.Export;
using EphemerisRegression.Infrastructure;
using EphemerisRegression.Parsing;
using EphemerisRegression.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EphemerisRegression.Runner
{
    public sealed class GeoNodeL0ExportRunner
    {
        public async Task RunAsync(IEnumerable<HelioEvent> events)
        {
            var solutionRoot = ProjectPathResolver.GetSolutionRoot();

            string rawDir = Path.Combine(
                solutionRoot,
                "EphemerisRegression",
                "Horizons",
                "Geo",
                "TS-B",
                "Raw");

            string jsonDir = Path.Combine(
                solutionRoot,
                "EphemerisRegression",
                "Horizons",
                "Geo",
                "TS-B",
                "Json");

            Directory.CreateDirectory(jsonDir);

            var parser = new HorizonsVectorParser();
            var writer = new JsonReferenceWriter(JsonOptionsFactory.Create());

            foreach (var e in events)
            {
                string fileName =
                    $"{e.Planet}_{e.TestSuite}_{e.EventName}_L0_Geo.csv";

                string rawPath = Path.Combine(rawDir, fileName);

                if (!File.Exists(rawPath))
                {
                    Console.WriteLine($"RAW file not found: {fileName}");
                    continue;
                }

                Console.WriteLine($"JSON Export GEO NODE: {e.Planet} {e.EventName}");

                var rawContent = await File.ReadAllTextAsync(rawPath);
                var vectors = parser.Parse(rawContent).ToList();

                bool ascending = e.EventName.Contains("Ascending");

                var node = FindNode(vectors, ascending);

                // --- Metadata ---
                var factory = new HorizonsApiRequestFactory(
                    new HorizonsGeoLevel0Config());

                var request = factory.Create(e);

                var parameters = request.ToParameterDictionary();
                var canonical = CanonicalRequestBuilder.Build(parameters);
                var hash = HashCalculator.ComputeSha256(canonical);

                var metadata = new
                {
                    CanonicalRequest = canonical,
                    RequestHash = hash,
                    HorizonsUrl = "",
                    EngineVersion = "1.0.0",
                    CorrectionLevel = "L0",
                    Mode = "GEO",
                    EphemType = "VECTORS",
                    GeneratedAtUtc = DateTime.UtcNow
                };

                var reference = new NodeReferenceModel
                {
                    Planet = e.Planet,
                    TestSuite = e.TestSuite,
                    EventName = e.EventName,
                    CorrectionLevel = "L0",
                    Metadata = metadata,
                    Node = node
                };

                string jsonPath = Path.Combine(
                    jsonDir,
                    fileName.Replace(".csv", ".json"));

                await writer.WriteAsync(jsonPath, reference);

                Console.WriteLine($"Saved JSON: {Path.GetFileName(jsonPath)}");
            }

            Console.WriteLine("Geo Node L0 JSON export complete.");
        }

        // ==============================================================
        // === NODE DETECTION ===========================================
        // ==============================================================

        private static NodeEvent FindNode(
            List<StateVector> vectors,
            bool ascending)
        {
            for (int i = 1; i < vectors.Count - 1; i++)
            {
                var prev = vectors[i - 1];
                var current = vectors[i];
                var next = vectors[i + 1];

                bool signChange =
                    Math.Sign(prev.Z) != Math.Sign(current.Z);

                if (!signChange)
                    continue;

                bool correctDirection =
                    ascending
                        ? prev.Z < 0 && current.Z >= 0
                        : prev.Z > 0 && current.Z <= 0;

                if (!correctDirection)
                    continue;

                return new NodeEvent
                {
                    JulianDate = current.JulianDate,
                    Before = prev,
                    At = current,
                    After = next
                };
            }

            throw new InvalidOperationException(
                $"No {(ascending ? "ascending" : "descending")} node found in data.");
        }
    }
}