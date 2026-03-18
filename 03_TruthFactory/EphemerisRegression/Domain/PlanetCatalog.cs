using System;
using System.Collections.Generic;
using System.Linq;

namespace EphemerisRegression.Domain
{
    public sealed record PlanetInfo(
        string Name,
        int CommandCode,
        double MinJulianDay,
        double MaxJulianDay)
    {
        public override string ToString() =>
            $"{Name} ({CommandCode}) JD[{MinJulianDay} .. {MaxJulianDay}]";
    }

    public static class PlanetCatalog
    {
        // Horizons availability table (TDB)
        // Values taken EXACTLY from specification table (Min JD / Max JD)

        public static readonly IReadOnlyList<PlanetInfo> AllPlanets =
            new List<PlanetInfo>
            {
                // Inner planets: full JD range supported by Horizons
                new("Mercury", 199, 0.5,        5373482.5),
                new("Venus",   299, 0.5,        5373482.5),
                new("Earth",   399, 0.5,        5373482.5),

                // Mars
                new("Mars",    499, 2305448.5,  2670690.5),

                // Jupiter
                new("Jupiter", 599, 2305457.5,  2524601.5),

                // Saturn
                new("Saturn",  699, 2360233.5,  2542859.5),

                // Uranus
                new("Uranus",  799, 2305451.5,  2597625.5),

                // Neptune
                new("Neptune", 899, 2305451.5,  2597641.5)
            };

        public static PlanetInfo Earth =>
            AllPlanets.Single(p => p.Name == "Earth");

        public static IReadOnlyList<PlanetInfo> ParseSelection(string? selection)
        {
            if (string.IsNullOrWhiteSpace(selection))
                return AllPlanets;

            var wanted = selection
                .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var result = AllPlanets
                .Where(p => wanted.Contains(p.Name))
                .ToList();

            if (result.Count != wanted.Count)
                throw new ArgumentException(
                    "Unknown planet(s). Known: " +
                    string.Join(", ", AllPlanets.Select(p => p.Name)));

            return result;
        }
    }
}