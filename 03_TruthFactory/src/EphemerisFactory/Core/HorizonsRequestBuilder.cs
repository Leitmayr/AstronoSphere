// ============================================================
// FILE: HorizonsRequestBuilder.cs
// STATUS: NEW
// ============================================================

using EphemerisRegression.Api;
using System;
using System.Globalization;

namespace EphemerisFactory.Core
{
    public static class HorizonsRequestBuilder
    {
        public static HorizonsApiRequest Build(dynamic scenario)
        {
            var core = scenario.Core;

            var target = core.Targets[0];
            int command = PlanetMapper.ToCommand(target);

            string center = core.Observer.Type switch
            {
                "Heliocentric" => "@10",
                "Geocentric" => "500@399",
                _ => throw new Exception($"Unsupported observer type: {core.Observer.Type}")
            };

            return new HorizonsApiRequest
            {
                Command = command,
                Center = center,

                StartTime = $"JD{F(core.Time.StartJD)}",
                StopTime = $"JD{F(core.Time.StopJD)}",

                StepSize = $"{F(core.Time.StepDays)}D",

                RefPlane = "ECLIPTIC",
                RefSystem = "ICRF",
                OutputUnits = "AU-D",
                EphemType = "VECTORS",
                CsvFormat = "NO"
            };
        }

        private static string F(double v) =>
            v.ToString("0.########", CultureInfo.InvariantCulture);
    }
}