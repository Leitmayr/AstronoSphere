using System;

namespace AstronoCert.Core
{
    public static class ExperimentIdGenerator
    {
        public static string Generate(CoreDefinition core)
        {
            // --- JD truncation (NO decimals, NO rounding) ---
            long start = (long)Math.Truncate(core.Time.StartJD);
            long stop = (long)Math.Truncate(core.Time.StopJD);

            // --- Fixed components (M1.9 spec) ---
            string observer = MapObserver(core.Observer.Type);
            string epoch = core.Frame.Epoch;          // J2000
            string timeScale = core.Time.TimeScale;   // TDB
            string step = core.Time.Step;             // e.g. 1H

            var id = $"{observer}-{epoch}-{timeScale}-{start}-{stop}-{step}";

            return id.ToUpperInvariant();
        }

        private static string MapObserver(string type)
        {
            return type switch
            {
                "Heliocentric" => "HELIO",
                "Geocentric" => "GEO",
                _ => type.ToUpperInvariant()
            };
        }
    }
}