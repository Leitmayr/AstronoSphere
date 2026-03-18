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
    public sealed class GeoEventGenerationRunner
    {
        private readonly HorizonsApiClient _client;
        private readonly HorizonsApiRequestFactory _factory;

        public GeoEventGenerationRunner()
        {
            var config = new HorizonsGeoLevel0Config();
            _factory = new HorizonsApiRequestFactory(config);
            _client = new HorizonsApiClient();
        }

        public async Task<IEnumerable<HelioEvent>> GenerateAsync(
            DateTime start,
            DateTime stop,
            bool fastMode = true)
        {
            var generator = new GeoNodeEventGenerator(_client, _factory);

            var planets = fastMode
                ? PlanetSelection.GeoNodesFast
                : PlanetSelection.GeoNodesFull;

            var result = new List<HelioEvent>();

            foreach (var planet in planets)
            {
                Console.WriteLine();
                Console.WriteLine($"--- {planet.Key} (GEO) ---");

                var generatedEvents = await generator.GenerateForPlanetAsync(
                    planet.Value,
                    start,
                    stop);

                foreach (var evt in generatedEvents)
                {
                    Console.WriteLine($"{planet.Key} {evt.EventName} -> JD {evt.JD:F9}");

                    var geoEvent = new HelioEvent
                    {
                        Planet = planet.Key,
                        CommandCode = planet.Value,
                        TestSuite = "TS-B",
                        EventName = evt.EventName,
                        JulianDate = evt.JD,
                        WindowDays = 1,
                        StepSize = "1h"
                    };

                    result.Add(geoEvent);
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Total GEO events generated: {result.Count}");

            return result;
        }
    }
}
