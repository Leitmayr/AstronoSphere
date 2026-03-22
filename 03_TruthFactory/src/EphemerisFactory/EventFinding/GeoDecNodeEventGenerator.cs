// ============================================================
// FILE: /EventFinding/GeoDecNodeEventGenerator.cs
// STATUS: ÄNDERUNG
// ============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EphemerisRegression.Api;
using EphemerisRegression.Parsing;

namespace EphemerisRegression.EventFinding
{
    public sealed class GeoDecNodeEventGenerator
    {
        private readonly HorizonsApiClient _client;
        private readonly HorizonsApiRequestFactory _factory;
        private readonly HorizonsVectorParser _parser;

        public GeoDecNodeEventGenerator(
            HorizonsApiClient client,
            HorizonsApiRequestFactory factory)
        {
            _client = client;
            _factory = factory;
            _parser = new HorizonsVectorParser();
        }

        public async Task<List<(string EventName, double JD)>>
            GenerateForPlanetAsync(
                string planetName,
                int commandCode)
        {
            var seed = GeoDecNodeSeedData.Get(planetName);

            double? ascending = null;
            double? descending = null;

            // ============================================================
            // === ASCENDING WINDOW ======================================
            // ============================================================

            ascending = await SearchWithinWindow(
                commandCode,
                seed.AscendingStart,
                seed.AscendingEnd,
                ascending: true);

            if (ascending == null)
            {
                ascending = await SearchWithPeriodFallback(
                    commandCode,
                    seed.AscendingStart,
                    seed.OrbitalPeriodDays,
                    ascending: true);
            }

            // ============================================================
            // === DESCENDING WINDOW =====================================
            // ============================================================

            descending = await SearchWithinWindow(
                commandCode,
                seed.DescendingStart,
                seed.DescendingEnd,
                ascending: false);

            if (descending == null)
            {
                descending = await SearchWithPeriodFallback(
                    commandCode,
                    seed.DescendingStart,
                    seed.OrbitalPeriodDays,
                    ascending: false);
            }

            if (ascending == null || descending == null)
                throw new Exception(
                    $"Could not find declination node crossings for {planetName}.");

            return new List<(string, double)>
            {
                ("GeoDecAscendingNode", ascending.Value),
                ("GeoDecDescendingNode", descending.Value)
            };
        }

        // ============================================================
        // === WINDOW SEARCH (TS-B PATTERN) ============================
        // ============================================================

        private async Task<double?> SearchWithinWindow(
            int commandCode,
            DateTime start,
            DateTime end,
            bool ascending)
        {
            var request = _factory.CreateCustom(
                commandCode,
                start,
                end,
                "1d");

            // IMPORTANT:
            // We want equatorial ICRF vectors. In ICRF, sign(Z) matches sign(Dec).
            // Ensure ref plane/system and TDB.
            request = new HorizonsApiRequest
            {
                Command = request.Command,
                Center = request.Center,
                StartTime = request.StartTime,
                StopTime = request.StopTime,
                StepSize = request.StepSize,

                RefPlane = "FRAME",
                RefSystem = "ICRF",
                OutputUnits = request.OutputUnits,
                CsvFormat = "NO",          // keep classic format for HorizonsVectorParser
                EphemType = "VECTORS",
                VectorCorrection = request.VectorCorrection,
                AdditionalParameters = new Dictionary<string, string>
                {
                    ["TIME_TYPE"] = "TDB"
                }
            };

            var raw = await _client.ExecuteAsync(request);
            var vectors = _parser.Parse(raw).ToList();

            for (int i = 1; i < vectors.Count; i++)
            {
                var prev = vectors[i - 1];
                var curr = vectors[i];

                // Ascending node: Z- -> Z+
                if (ascending && prev.Z < 0 && curr.Z > 0)
                    return await RefineCrossing(commandCode, prev.JulianDate);

                // Descending node: Z+ -> Z-
                if (!ascending && prev.Z > 0 && curr.Z < 0)
                    return await RefineCrossing(commandCode, prev.JulianDate);
            }

            return null;
        }

        // ============================================================
        // === PERIODIC FALLBACK ======================================
        // ============================================================

        private async Task<double?> SearchWithPeriodFallback(
            int commandCode,
            DateTime initial,
            double orbitalPeriodDays,
            bool ascending)
        {
            DateTime currentStart = initial;
            double step = orbitalPeriodDays / 2.0;

            for (int i = 0; i < 20; i++)
            {
                DateTime currentEnd = currentStart.AddDays(step);

                var result = await SearchWithinWindow(
                    commandCode,
                    currentStart,
                    currentEnd,
                    ascending);

                if (result != null)
                    return result;

                currentStart = currentStart.AddDays(step);
            }

            return null;
        }

        // ============================================================
        // === REFINEMENT (TS-B STYLE) ================================
        // ============================================================

        private async Task<double> RefineCrossing(
            int commandCode,
            double jd)
        {
            double start = jd - 3.0;
            double stop = jd + 3.0;

            var zoomRequest = _factory.CreateCustom(
                commandCode,
                DateTimeFromJulian(start),
                DateTimeFromJulian(stop),
                "1h");

            zoomRequest = new HorizonsApiRequest
            {
                Command = zoomRequest.Command,
                Center = zoomRequest.Center,
                StartTime = zoomRequest.StartTime,
                StopTime = zoomRequest.StopTime,
                StepSize = zoomRequest.StepSize,

                RefPlane = "FRAME",
                RefSystem = "ICRF",
                OutputUnits = zoomRequest.OutputUnits,
                CsvFormat = "NO",
                EphemType = "VECTORS",
                VectorCorrection = zoomRequest.VectorCorrection,
                AdditionalParameters = new Dictionary<string, string>
                {
                    ["TIME_TYPE"] = "TDB"
                }
            };

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