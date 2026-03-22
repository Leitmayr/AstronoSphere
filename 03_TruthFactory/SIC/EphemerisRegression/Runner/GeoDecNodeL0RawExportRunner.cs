// ============================================================
// FILE: /Runner/GeoDecNodeL0RawExportRunner.cs
// STATUS: ERWEITERT (TS-C Dual Mode, Vector = ICRF)
// ============================================================

using EphemerisRegression.Api;
using EphemerisRegression.Config;
using EphemerisRegression.Event;
using EphemerisRegression.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace EphemerisRegression.Runner
{
    public sealed class GeoDecNodeL0RawExportRunner
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

            Directory.CreateDirectory(rawDir);

            // ==============================
            // SEPARATE FACTORIES
            // ==============================

            // 🔴 NEU: Vector in ICRF / FRAME (äquatorial)
            var vectorFactory = new HorizonsApiRequestFactory(
                new HorizonsGeoEqVectorLevel0Config());

            // 🟢 Observer unverändert
            var observerFactory = new HorizonsApiRequestFactory(
                new HorizonsGeoObserverLevel0Config());

            var client = new HorizonsApiClient();

            foreach (var e in events)
            {
                Console.WriteLine($"RAW Export GEO DEC: {e.Planet} {e.EventName}");

                // ============================
                // VECTOR (ICRF)
                // ============================

                var vectorRequest = vectorFactory.Create(e);
                var vectorResult = await client.ExecuteAsync(vectorRequest);

                string vectorFile =
                    $"{e.Planet}_{e.TestSuite}_{e.EventName}_L0_Vector.csv";

                await File.WriteAllTextAsync(
                    Path.Combine(rawDir, vectorFile),
                    vectorResult);

                await Task.Delay(500);

                // ============================
                // OBSERVER (RA/DEC)
                // ============================

                var observerRequest = observerFactory.Create(e);
                var observerResult = await client.ExecuteAsync(observerRequest);

                string observerFile =
                    $"{e.Planet}_{e.TestSuite}_{e.EventName}_L0_Observer.csv";

                await File.WriteAllTextAsync(
                    Path.Combine(rawDir, observerFile),
                    observerResult);

                await Task.Delay(500);
            }

            Console.WriteLine("Geo DEC Node L0 RAW export complete.");
        }
    }
}