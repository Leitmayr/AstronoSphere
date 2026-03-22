using Astronometria.Core.Bodies;

namespace Astronometria.Ephemerides.Test.EphemerisValidation.Common
{
    public static class RegressionTolerances
    {
        // ------------------------------------------------------------
        // GEO – Position (AU) - Toleranz in geo ecliptical = geo equatorial 
        // ------------------------------------------------------------
        public static double GetGeoPositionTolerance(PlanetId planet)
        {
            return planet switch
            {
                PlanetId.Mercury => 1e-5,
                PlanetId.Venus => 1e-5,
                PlanetId.Earth => 1e-5,
                PlanetId.Mars => 2e-5,

                PlanetId.Jupiter => 1e-4,
                PlanetId.Saturn => 2e-4,

                PlanetId.Uranus => 1e-3,
                PlanetId.Neptune => 1e-3,

                _ => 2e-5
            };
        }

        // ------------------------------------------------------------
        // GEO – Velocity (AU/day) - Toleranz in geo ecliptical = geo equatorial 
        // ------------------------------------------------------------
        public static double GetGeoVelocityTolerance(PlanetId planet)
        {
            return planet switch
            {
                PlanetId.Mercury => 3e-7,
                PlanetId.Venus => 3e-7,
                PlanetId.Earth => 3e-7,
                PlanetId.Mars => 2e-7,

                PlanetId.Jupiter => 2e-7,
                PlanetId.Saturn => 2e-7,
                PlanetId.Uranus => 2e-7,
                PlanetId.Neptune => 2e-7,

                _ => 2e-7
            };
        }

        // ------------------------------------------------------------
        // HELIO – Position (AU)
        // ------------------------------------------------------------
        public static double GetHelioPositionTolerance(PlanetId planet)
        {
            return planet switch
            {
                PlanetId.Mercury => 5e-6,
                PlanetId.Venus => 5e-6,
                PlanetId.Earth => 5e-6,
                PlanetId.Mars => 1e-5,

                PlanetId.Jupiter => 5e-5,
                PlanetId.Saturn => 1e-4,

                PlanetId.Uranus => 5e-4,
                PlanetId.Neptune => 7e-4,

                _ => 1e-5
            };
        }

        // ------------------------------------------------------------
        // HELIO – Velocity (AU/day)
        // ------------------------------------------------------------
        public static double GetHelioVelocityTolerance(PlanetId planet)
        {
            return planet switch
            {
                PlanetId.Mercury => 2e-7,
                PlanetId.Venus => 1e-7,
                PlanetId.Earth => 1e-7,
                PlanetId.Mars => 1e-7,

                PlanetId.Jupiter => 1e-7,
                PlanetId.Saturn => 1e-7,
                PlanetId.Uranus => 1e-7,
                PlanetId.Neptune => 1e-7,

                _ => 1e-7
            };
        }
    }
}
