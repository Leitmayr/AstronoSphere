using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using EphemerisRegression.Api;
using EphemerisRegression.Parsing;


namespace EphemerisRegression.EventFinding
{


    public sealed class HelioQuadrantEventGenerator
    {
        private readonly HorizonsApiClient _client;
        private readonly HorizonsApiRequestFactory _factory;
        private readonly HorizonsVectorParser _parser;

        public HelioQuadrantEventGenerator(
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

            double? l0 = null;
            double? l6 = null;
            double? l12 = null;
            double? l18 = null;

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

                    if (l0 == null && prev.Y < 0 && curr.Y > 0)
                        l0 = await RefineCrossing(commandCode, prev.JulianDate);

                    if (l12 == null && prev.Y > 0 && curr.Y < 0)
                        l12 = await RefineCrossing(commandCode, prev.JulianDate);

                    if (l6 == null && prev.X > 0 && curr.X < 0)
                        l6 = await RefineCrossing(commandCode, prev.JulianDate);

                    if (l18 == null && prev.X < 0 && curr.X > 0)
                        l18 = await RefineCrossing(commandCode, prev.JulianDate);

                    if (l0 != null && l6 != null && l12 != null && l18 != null)
                        return new List<(string, double)>
                {
                    ("L18", l18.Value),
                    ("L0",  l0.Value),
                    ("L6",  l6.Value),
                    ("L12", l12.Value)
                };
                }

                // Fenster erweitern um 1 Jahr
                currentStart = currentStop;
                currentStop = currentStop.AddYears(1);
            }

            throw new Exception("Could not find all quadrant crossings within 10 years.");
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

                // Y sign change
                if (prev.Y * curr.Y < 0)
                    return Interpolate(prev.JulianDate, curr.JulianDate,
                                       prev.Y, curr.Y);

                // X sign change
                if (prev.X * curr.X < 0)
                    return Interpolate(prev.JulianDate, curr.JulianDate,
                                       prev.X, curr.X);
            }

            return jd; // fallback (should never happen)
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
