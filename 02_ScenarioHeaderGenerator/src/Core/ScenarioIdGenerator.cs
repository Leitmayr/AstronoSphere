// ============================================================
// FILE: ScenarioIdGenerator.cs
// STATUS: UPDATE
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

            return $"{origin}-{time.TimeScale}-{Format(time.StartJD)}-{Format(time.StopJD)}-{time.StepDays}";
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

        private static string Format(double value)
        {
            return value.ToString("0.########", CultureInfo.InvariantCulture);
        }
    }
}