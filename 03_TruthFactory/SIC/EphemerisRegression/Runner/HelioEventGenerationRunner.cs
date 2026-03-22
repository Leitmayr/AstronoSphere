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
    public sealed class HelioEventGenerationRunner
    {
        private readonly HorizonsApiClient _client;
        private readonly HorizonsApiRequestFactory _factory;

        public HelioEventGenerationRunner()
        {
            var config = new HorizonsHelioLevel0Config();
            _factory = new HorizonsApiRequestFactory(config);
            _client = new HorizonsApiClient();
        }

        public async Task<IEnumerable<HelioEvent>> GenerateAsync(
            DateTime start,
            DateTime stop,
            bool fastMode = true)
        {
            var generator = new HelioQuadrantEventGenerator(_client, _factory);

            var planets = fastMode
                ? PlanetSelection.Fast
                : PlanetSelection.Full;

            var result = new List<HelioEvent>();

            foreach (var planet in planets)
            {
                Console.WriteLine();
                Console.WriteLine($"--- {planet.Key} ---");

                var generatedEvents = await generator.GenerateForPlanetAsync(
                    planet.Value,
                    start,
                    stop);

                foreach (var evt in generatedEvents)
                {
                    Console.WriteLine($"{planet.Key} {evt.EventName} -> JD {evt.JD:F9}");

                    var helioEvent = new HelioEvent
                    {
                        Planet = planet.Key,
                        CommandCode = planet.Value,
                        TestSuite = "TS-A",
                        EventName = evt.EventName,
                        JulianDate = evt.JD,
                        WindowDays = 3,
                        StepSize = "1h"
                    };

                    result.Add(helioEvent);
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Total events generated: {result.Count}");

            return result;
        }
    }
}
