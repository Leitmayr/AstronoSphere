using System.Collections.Generic;
using EphemerisRegression.Domain;

namespace EphemerisRegression.Config
{
    public static class PlanetSelection
    {
        public static IReadOnlyDictionary<string, int> Fast =>
            new Dictionary<string, int>
            {
                { "Mercury", PlanetCodes.Mercury },
                { "Venus", PlanetCodes.Venus },
                { "Earth", PlanetCodes.Earth },
                { "Mars", PlanetCodes.Mars },
                { "Jupiter", PlanetCodes.Jupiter },
                { "Saturn", PlanetCodes.Saturn }
            };

        public static IReadOnlyDictionary<string, int> Full =>
            new Dictionary<string, int>
            {
                { "Mercury", PlanetCodes.Mercury },
                { "Venus", PlanetCodes.Venus },
                { "Earth", PlanetCodes.Earth },
                { "Mars", PlanetCodes.Mars },
                { "Jupiter", PlanetCodes.Jupiter },
                { "Saturn", PlanetCodes.Saturn },
                { "Uranus", PlanetCodes.Uranus },
                { "Neptune", PlanetCodes.Neptune }
            };

        // 🔹 Geo Node FAST (keine Earth, keine Uranus/Neptune)
        public static IReadOnlyDictionary<string, int> GeoNodesFast =>
            new Dictionary<string, int>
            {
                { "Mercury", PlanetCodes.Mercury },
                { "Venus", PlanetCodes.Venus },
                { "Mars", PlanetCodes.Mars },
                { "Jupiter", PlanetCodes.Jupiter },
                { "Saturn", PlanetCodes.Saturn }
            };

        // 🔹 Geo Node FULL (keine Earth, aber inkl. Uranus/Neptune)
        public static IReadOnlyDictionary<string, int> GeoNodesFull =>
            new Dictionary<string, int>
            {
                { "Mercury", PlanetCodes.Mercury },
                { "Venus", PlanetCodes.Venus },
                { "Mars", PlanetCodes.Mars },
                { "Jupiter", PlanetCodes.Jupiter },
                { "Saturn", PlanetCodes.Saturn },
                { "Uranus", PlanetCodes.Uranus },
                { "Neptune", PlanetCodes.Neptune }
            };
    }
}
