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
    public sealed class HelioQuadrantL0RawExportRunner
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

            Directory.CreateDirectory(rawDir);

            var config = new HorizonsHelioLevel0Config();
            var factory = new HorizonsApiRequestFactory(config);
            var client = new HorizonsApiClient();

            foreach (var e in events)
            {
                Console.WriteLine($"RAW Export: {e.Planet} {e.EventName}");

                var request = factory.Create(e);
                var result = await client.ExecuteAsync(request);

                string fileName =
                    $"{e.Planet}_{e.TestSuite}_{e.EventName}_L0.csv";

                string path = Path.Combine(rawDir, fileName);

                await File.WriteAllTextAsync(path, result);

                await Task.Delay(500); // Rate limit protection
            }

            Console.WriteLine("Helio Quadrant L0 RAW export complete.");
        }
    }
}
