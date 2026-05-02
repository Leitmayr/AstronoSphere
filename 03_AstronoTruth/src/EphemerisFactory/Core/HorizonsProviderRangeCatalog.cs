// ============================================================
// FILE: 03_AstronoTruth/src/EphemerisFactory/Core/HorizonsProviderRangeCatalog.cs
// STATUS: NEW (M2.1 AstronoDiag provider range catalog)
// ============================================================

using System;

namespace EphemerisFactory.Core
{
    public static class HorizonsProviderRangeCatalog
    {
        public static (double ProviderMinJD, double ProviderMaxJD) GetRange(string target)
        {
            return target.ToUpperInvariant() switch
            {
                "MERCURY" => (0.5, 5373482.5),
                "VENUS" => (0.5, 5373482.5),
                "EARTH" => (0.5, 5373482.5),
                "MARS" => (2305448.5, 2670690.5),
                "JUPITER" => (2305457.5, 2524601.5),
                "SATURN" => (2360233.5, 2542859.5),
                "URANUS" => (2305451.5, 2597625.5),
                "NEPTUNE" => (2305451.5, 2597641.5),

                _ => throw new NotSupportedException(
                    $"No Horizons provider range defined for target: {target}")
            };
        }
    }
}