// ============================================================
// FILE: /Runner/GeoDecNodeL0ExportRunner.cs
// STATUS: TS-C JSON Export (Vector + Observer)
// ============================================================

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
    public sealed class GeoDecNodeL0ExportRunner
    {
        public async Task RunAsync(IEnumerable<HelioEvent> events)
        {
            var solutionRoot = ProjectPathResolver.GetSolutionRoot();

            string rawDir = Path.Combine(
                solutionRoot,
                "EphemerisRegression",
                "Horizons",
                "Geo",
                "TS-C",
                "Raw");

            string jsonDir = Path.Combine(
                solutionRoot,
                "EphemerisRegression",
                "Horizons",
                "Geo",
                "TS-C",
                "Json");

            Directory.CreateDirectory(jsonDir);

            var vectorParser = new HorizonsVectorParser();
            var observerParser = new HorizonsObserverParser();
            var writer = new JsonReferenceWriter(JsonOptionsFactory.Create());

            foreach (var e in events)
            {
                Console.WriteLine($"JSON Export GEO DEC: {e.Planet} {e.EventName}");

                bool ascending = e.EventName.Contains("Ascending", StringComparison.OrdinalIgnoreCase);

                // =====================================================
                // VECTOR (ICRF x/y/z)  -> sign change in Z at equator
                // =====================================================

                string vectorFile =
                    $"{e.Planet}_{e.TestSuite}_{e.EventName}_L0_Vector.csv";

                var vectorPath = Path.Combine(rawDir, vectorFile);
                if (!File.Exists(vectorPath))
                    throw new FileNotFoundException($"Missing raw vector file: {vectorPath}");

                var rawVector = await File.ReadAllTextAsync(vectorPath);
                var vectors = vectorParser.Parse(rawVector).ToList();

                var vectorNode = FindVectorNode(vectors, ascending);

                var vectorRef = new GeoDecVectorNodeReferenceModel
                {
                    Planet = e.Planet,
                    TestSuite = e.TestSuite,
                    EventName = e.EventName,
                    CorrectionLevel = "L0",
                    Metadata = BuildMetadata(e, ephemType: "VECTORS"),
                    Node = vectorNode
                };

                await writer.WriteAsync(
                    Path.Combine(jsonDir, vectorFile.Replace(".csv", ".json")),
                    vectorRef);

                // =====================================================
                // OBSERVER (RA/DEC)  -> sign change in DEC at equator
                // =====================================================

                string observerFile =
                    $"{e.Planet}_{e.TestSuite}_{e.EventName}_L0_Observer.csv";

                var observerPath = Path.Combine(rawDir, observerFile);
                if (!File.Exists(observerPath))
                    throw new FileNotFoundException($"Missing raw observer file: {observerPath}");

                var rawObserver = await File.ReadAllTextAsync(observerPath);
                var rows = observerParser.Parse(rawObserver).ToList();

                var observerNode = FindObserverNode(rows, ascending);

                var observerRef = new GeoDecObserverNodeReferenceModel
                {
                    Planet = e.Planet,
                    TestSuite = e.TestSuite,
                    EventName = e.EventName,
                    CorrectionLevel = "L0",
                    Metadata = BuildMetadata(e, ephemType: "OBSERVER"),
                    Node = observerNode
                };

                await writer.WriteAsync(
                    Path.Combine(jsonDir, observerFile.Replace(".csv", ".json")),
                    observerRef);
            }

            Console.WriteLine("Geo DEC Node L0 JSON export complete.");
        }

        // =========================================================
        // Node finding (same pattern as TS-B style: prev + current + next)
        // =========================================================

        private static NodeEvent FindVectorNode(
            List<StateVector> vectors,
            bool ascending)
        {
            if (vectors.Count < 3)
                throw new InvalidOperationException("Vector list too short to detect node crossing.");

            for (int i = 1; i < vectors.Count - 1; i++)
            {
                var prev = vectors[i - 1];
                var current = vectors[i];
                var next = vectors[i + 1];

                // Ascending: Z- -> Z+
                if (ascending && prev.Z < 0 && current.Z >= 0)
                {
                    return new NodeEvent
                    {
                        JulianDate = current.JulianDate,
                        Before = prev,
                        At = current,
                        After = next
                    };
                }

                // Descending: Z+ -> Z-
                if (!ascending && prev.Z > 0 && current.Z <= 0)
                {
                    return new NodeEvent
                    {
                        JulianDate = current.JulianDate,
                        Before = prev,
                        At = current,
                        After = next
                    };
                }
            }

            throw new InvalidOperationException(
                $"No {(ascending ? "ascending" : "descending")} vector node found.");
        }

        private static ObserverNodeEvent FindObserverNode(
            List<ObserverRow> rows,
            bool ascending)
        {
            if (rows.Count < 3)
                throw new InvalidOperationException("Observer list too short to detect node crossing.");

            for (int i = 1; i < rows.Count - 1; i++)
            {
                var prev = rows[i - 1];
                var current = rows[i];
                var next = rows[i + 1];

                // Ascending: DEC- -> DEC+
                if (ascending && prev.Dec < 0 && current.Dec >= 0)
                    return BuildObserverNode(prev, current, next);

                // Descending: DEC+ -> DEC-
                if (!ascending && prev.Dec > 0 && current.Dec <= 0)
                    return BuildObserverNode(prev, current, next);
            }

            throw new InvalidOperationException(
                $"No {(ascending ? "ascending" : "descending")} observer node found.");
        }

        private static ObserverNodeEvent BuildObserverNode(
            ObserverRow prev,
            ObserverRow current,
            ObserverRow next)
        {
            return new ObserverNodeEvent
            {
                JulianDate = current.JulianDate,
                Before = new ObserverState
                {
                    JulianDate = prev.JulianDate,
                    Ra = prev.Ra,
                    Dec = prev.Dec
                },
                At = new ObserverState
                {
                    JulianDate = current.JulianDate,
                    Ra = current.Ra,
                    Dec = current.Dec
                },
                After = new ObserverState
                {
                    JulianDate = next.JulianDate,
                    Ra = next.Ra,
                    Dec = next.Dec
                }
            };
        }

        // =========================================================
        // Metadata: request canonical + hash (vector/observer use their own configs)
        // =========================================================

        private static object BuildMetadata(HelioEvent e, string ephemType)
        {
            // TS-C uses *two* different configs:
            // - VECTORS: equatorial (ICRF) vector output
            // - OBSERVER: RA/DEC output (ICRF), TIME_TYPE=TT
            HorizonsLevelBaseConfig cfg = ephemType == "VECTORS"
                ? new HorizonsGeoEqVectorLevel0Config()
                : new HorizonsGeoObserverLevel0Config();

            var factory = new HorizonsApiRequestFactory(cfg);
            var request = factory.Create(e);

            var parameters = request.ToParameterDictionary();
            var canonical = CanonicalRequestBuilder.Build(parameters);
            var hash = HashCalculator.ComputeSha256(canonical);

            return new
            {
                CanonicalRequest = canonical,
                RequestHash = hash,
                HorizonsUrl = "",
                EngineVersion = "1.0.0",
                CorrectionLevel = "L0",
                Mode = "GEO-DEC",
                EphemType = ephemType,
                GeneratedAtUtc = DateTime.UtcNow
            };
        }
    }
}