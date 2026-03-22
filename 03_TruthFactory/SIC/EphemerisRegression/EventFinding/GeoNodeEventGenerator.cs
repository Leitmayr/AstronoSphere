using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EphemerisRegression.Api;
using EphemerisRegression.Parsing;

namespace EphemerisRegression.EventFinding
{
    public sealed class GeoNodeEventGenerator
    {
        private readonly HorizonsApiClient _client;
        private readonly HorizonsApiRequestFactory _factory;
        private readonly HorizonsVectorParser _parser;

        public GeoNodeEventGenerator(
            HorizonsApiClient client,
            HorizonsApiRequestFactory factory)
        {
            _client = client;
            _factory = factory;
            _parser = new HorizonsVectorParser();
        }

        public async Task<List<(string EventName, double JD)>>
            GenerateForPlanetAsync(int commandCode, DateTime start, DateTime stop)
        {
            DateTime currentStart = start;
            DateTime currentStop = stop;

            double? ascending = null;
            double? descending = null;

            while (currentStart < start.AddYears(200))
            {
                var request = _factory.CreateCustom(
                    commandCode,
                    currentStart,
                    currentStop,
                    "1d");

                var raw = await _client.ExecuteAsync(request);
                var vectors = _parser.Parse(raw).ToList();

                for (int i = 1; i < vectors.Count; i++)
                {
                    var prev = vectors[i - 1];
                    var curr = vectors[i];

                    // Ascending Node (Z- → Z+)
                    if (ascending == null && prev.Z < 0 && curr.Z > 0)
                        ascending = await RefineCrossing(commandCode, prev.JulianDate);

                    // Descending Node (Z+ → Z-)
                    if (descending == null && prev.Z > 0 && curr.Z < 0)
                        descending = await RefineCrossing(commandCode, prev.JulianDate);

                    if (ascending != null && descending != null)
                        return new List<(string, double)>
                        {
                            ("GeoAscendingNode", ascending.Value),
                            ("GeoDescendingNode", descending.Value)
                        };
                }

                currentStart = currentStop;
                currentStop = currentStop.AddYears(1);
            }

            throw new Exception("Could not find both geocentric node crossings within search window.");
        }

        private async Task<double> RefineCrossing(int commandCode, double jd)
        {
            double start = jd - 3.0;
            double stop = jd + 3.0;

            var zoomRequest = _factory.CreateCustom(
                commandCode,
                DateTimeFromJulian(start),
                DateTimeFromJulian(stop),
                "1h");

            var raw = await _client.ExecuteAsync(zoomRequest);
            var vectors = _parser.Parse(raw).ToList();

            for (int i = 1; i < vectors.Count; i++)
            {
                var prev = vectors[i - 1];
                var curr = vectors[i];

                if (prev.Z * curr.Z < 0)
                {
                    return Interpolate(
                        prev.JulianDate,
                        curr.JulianDate,
                        prev.Z,
                        curr.Z);
                }
            }

            return jd; // fallback
        }

        private static double Interpolate(
            double t1, double t2,
            double v1, double v2)
        {
            return t1 + (-v1) / (v2 - v1) * (t2 - t1);
        }

        private static DateTime DateTimeFromJulian(double jd)
        {
            double unixEpochJD = 2440587.5;
            double seconds = (jd - unixEpochJD) * 86400.0;
            return DateTimeOffset.FromUnixTimeSeconds((long)seconds).UtcDateTime;
        }
    }
}
