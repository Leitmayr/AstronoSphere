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
    public sealed class HelioQuadrantL0ExportRunner
    {
        public async Task RunAsync(IEnumerable<HelioEvent> events)
        {
            var solutionRoot = ProjectPathResolver.GetSolutionRoot();

            string rawDir = Path.Combine(
                solutionRoot,
                "EphemerisRegression",
                "Horizons",
                "Helio",
                "TS-A",
                "Raw");

            string jsonDir = Path.Combine(
                solutionRoot,
                "EphemerisRegression",
                "Horizons",
                "Helio",
                "TS-A",
                "Json");

            Directory.CreateDirectory(jsonDir);

            var parser = new HorizonsVectorParser();
            var writer = new JsonReferenceWriter(JsonOptionsFactory.Create());

            foreach (var e in events)
            {
                string fileName =
                    $"{e.Planet}_{e.TestSuite}_{e.EventName}_L0.csv";

                string rawPath = Path.Combine(rawDir, fileName);

                if (!File.Exists(rawPath))
                {
                    Console.WriteLine($"RAW file not found: {fileName}");
                    continue;
                }

                Console.WriteLine($"JSON Export: {e.Planet} {e.EventName}");

                var rawContent = await File.ReadAllTextAsync(rawPath);
                var vectors = parser.Parse(rawContent).ToList();

                var factory = new HorizonsApiRequestFactory(
                    new HorizonsHelioLevel0Config());

                var request = factory.Create(e);

                var parameters = request.ToParameterDictionary();
                var canonical = CanonicalRequestBuilder.Build(parameters);
                var hash = HashCalculator.ComputeSha256(canonical);

                var metadata = new ReferenceMetadata
                {
                    CanonicalRequest = canonical,
                    RequestHash = hash,
                    HorizonsUrl = "",
                    EngineVersion = "1.0.0",
                    CorrectionLevel = "L0",
                    Mode = "HELIO",
                    EphemType = "VECTORS",
                    GeneratedAtUtc = DateTime.UtcNow
                };

                var reference = new ReferenceStateVector(
                    e.Planet,
                    e.TestSuite,
                    e.EventName,
                    "L0",
                    vectors,
                    metadata);

                string jsonPath = Path.Combine(
                    jsonDir,
                    fileName.Replace(".csv", ".json"));

                await writer.WriteAsync(jsonPath, reference);

                Console.WriteLine($"Saved JSON: {Path.GetFileName(jsonPath)}");
            }

            Console.WriteLine("Helio Quadrant L0 JSON export complete.");
        }
    }
}
