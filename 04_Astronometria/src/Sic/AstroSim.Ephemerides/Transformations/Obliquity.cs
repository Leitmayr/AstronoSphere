using System;

namespace AstroSim.Ephemerides.Transformations
{
    /// <summary>
    /// Mean obliquity of the ecliptic (J2000).
    /// Used for transformation from ecliptic to equatorial frame.
    /// </summary>
    internal static class Obliquity
    {
        // Mean obliquity at J2000 in degrees
        private const double Epsilon0Arcseconds = 84381.448;

        /// <summary>
        /// Mean obliquity at J2000 in radians.
        /// </summary>
        public static readonly double Epsilon0 =
            Epsilon0Arcseconds / 3600.0 * System.Math.PI / 180.0;
    }
}

