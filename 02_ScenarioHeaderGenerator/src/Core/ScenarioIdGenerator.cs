// ============================================================
// FILE: ScenarioIdGenerator.cs
// STATUS: UPDATE (RC1 – Scenario Precision: truncate JD)
// ============================================================

using AstronoData.ScenarioCandidates;
using ScenarioHeaderGenerator.ScenarioCandidates;
using System;
using System.Globalization;

namespace ScenarioHeaderGenerator
{
    public static class ScenarioIdGenerator
    {
        public static string Generate(CoreDefinition core)
        {
            var origin = MapOrigin(core.Observer.Type);
            var time = core.Time;

            return $"{origin}-{time.TimeScale}-{FormatJD(time.StartJD)}-{FormatJD(time.StopJD)}-{time.StepDays}";
        }

        private static string MapOrigin(string type)
        {
            return type switch
            {
                "Heliocentric" => "HELIO",
                "Geocentric" => "GEO",
                "Topocentric" => "TOPO",
                _ => throw new Exception($"Invalid Observer.Type: {type}")
            };
        }

        // ============================================================
        // RC1 – JD FORMAT (TRUNCATE, NOT ROUND)
        // ============================================================

        private static string FormatJD(double value)
        {
            var truncated = Math.Truncate(value * 1000.0) / 1000.0;
            return truncated.ToString("0.000", CultureInfo.InvariantCulture);
        }
    }
}