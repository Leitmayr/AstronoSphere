// ============================================================
// FILE: /Runner/GeoDecNodeEventRunner.cs
// STATUS: RESET
// ============================================================

using EphemerisRegression.Api;
using EphemerisRegression.Config;
using EphemerisRegression.Domain;
using EphemerisRegression.Event;
using EphemerisRegression.EventFinding;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EphemerisRegression.Runner
{
    public sealed class GeoDecNodeEventRunner
    {
        private readonly HorizonsApiClient _client;
        private readonly HorizonsApiRequestFactory _factory;

        public GeoDecNodeEventRunner()
        {
            var config = new HorizonsGeoLevel0Config();
            _factory = new HorizonsApiRequestFactory(config);
            _client = new HorizonsApiClient();
        }

        public async Task<IEnumerable<HelioEvent>> GenerateAsync(
            bool fastMode = true)
        {
            var generator =
                new GeoDecNodeEventGenerator(_client, _factory);

            var planets = fastMode
                ? PlanetSelection.GeoNodesFast
                : PlanetSelection.GeoNodesFull;

            var result = new List<HelioEvent>();

            foreach (var planet in planets)
            {
                Console.WriteLine();
                Console.WriteLine($"--- {planet.Key} (GEO DEC) ---");

                var generatedEvents =
                    await generator.GenerateForPlanetAsync(
                        planet.Key,
                        planet.Value);

                foreach (var evt in generatedEvents)
                {
                    Console.WriteLine(
                        $"{planet.Key} {evt.EventName} -> JD {evt.JD:F9}");

                    result.Add(new HelioEvent
                    {
                        Planet = planet.Key,
                        CommandCode = planet.Value,
                        TestSuite = "TS-C",
                        EventName = evt.EventName,
                        JulianDate = evt.JD,
                        WindowDays = 1,
                        StepSize = "1h"
                    });
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Total GEO DEC events generated: {result.Count}");

            return result;
        }
    }
}