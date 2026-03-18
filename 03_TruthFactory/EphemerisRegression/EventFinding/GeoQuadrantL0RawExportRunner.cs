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
    public sealed class GeoQuadrantL0RawExportRunner
    {
        public async Task RunAsync(IEnumerable<HelioEvent> events)
        {
            var solutionRoot = ProjectPathResolver.GetSolutionRoot();

            string rawDir = Path.Combine(
                solutionRoot,
                "EphemerisRegression",
                "Horizons",
                "Geo",
                "TS-A",
                "Raw");

            Directory.CreateDirectory(rawDir);

            var config = new HorizonsGeoLevel0Config();
            var factory = new HorizonsApiRequestFactory(config);
            var client = new HorizonsApiClient();

            foreach (var e in events)
            {
                Console.WriteLine($"RAW Export GEO TS-A: {e.Planet} {e.EventName}");

                var request = factory.Create(e);
                var result = await client.ExecuteAsync(request);

                string fileName =
                    $"{e.Planet}_{e.TestSuite}_{e.EventName}_L0_Geo.csv";

                string path = Path.Combine(rawDir, fileName);

                await File.WriteAllTextAsync(path, result);

                await Task.Delay(500);
            }

            Console.WriteLine("Geo Quadrant L0 RAW export complete.");
        }
    }
}
