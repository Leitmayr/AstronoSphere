// ============================================================
// FILE: /EventFinding/GeoDecNodeSeedData.cs
// STATUS: NEU
// ============================================================

using System;
using System.Collections.Generic;

namespace EphemerisRegression.EventFinding
{
    public sealed class GeoDecNodeSeed
    {
        public DateTime AscendingStart { get; init; }
        public DateTime AscendingEnd { get; init; }

        public DateTime DescendingStart { get; init; }
        public DateTime DescendingEnd { get; init; }

        public double OrbitalPeriodDays { get; init; }
    }

    public static class GeoDecNodeSeedData
    {
        private static readonly Dictionary<string, GeoDecNodeSeed> _data =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["Mercury"] = new GeoDecNodeSeed
                {
                    AscendingStart = new DateTime(2026, 4, 15),
                    AscendingEnd = new DateTime(2026, 4, 20),
                    DescendingStart = new DateTime(2026, 9, 10),
                    DescendingEnd = new DateTime(2026, 9, 15),
                    OrbitalPeriodDays = 87.969
                },

                ["Venus"] = new GeoDecNodeSeed
                {
                    AscendingStart = new DateTime(2026, 3, 5),
                    AscendingEnd = new DateTime(2026, 3, 10),
                    DescendingStart = new DateTime(2026, 8, 5),
                    DescendingEnd = new DateTime(2026, 8, 10),
                    OrbitalPeriodDays = 224.701
                },

                ["Mars"] = new GeoDecNodeSeed
                {
                    AscendingStart = new DateTime(2026, 4, 10),
                    AscendingEnd = new DateTime(2026, 4, 15),
                    DescendingStart = new DateTime(2025, 8, 5),
                    DescendingEnd = new DateTime(2025, 8, 10),
                    OrbitalPeriodDays = 686.980
                },

                ["Jupiter"] = new GeoDecNodeSeed
                {
                    AscendingStart = new DateTime(2023, 1, 10),
                    AscendingEnd = new DateTime(2023, 1, 20),
                    DescendingStart = new DateTime(2022, 5, 25),
                    DescendingEnd = new DateTime(2022, 5, 31),
                    OrbitalPeriodDays = 4332.59
                },

                ["Saturn"] = new GeoDecNodeSeed
                {
                    AscendingStart = new DateTime(2026, 3, 15),
                    AscendingEnd = new DateTime(2026, 3, 31),
                    DescendingStart = new DateTime(2010, 8, 31),
                    DescendingEnd = new DateTime(2010, 9, 15),
                    OrbitalPeriodDays = 10759.22
                },

                ["Uranus"] = new GeoDecNodeSeed
                {
                    AscendingStart = new DateTime(2011, 3, 31),
                    AscendingEnd = new DateTime(2011, 4, 30),
                    DescendingStart = new DateTime(2012, 1, 15),
                    DescendingEnd = new DateTime(2012, 2, 15),
                    OrbitalPeriodDays = 30685.4
                },

                ["Neptune"] = new GeoDecNodeSeed
                {
                    AscendingStart = new DateTime(2026, 4, 30),
                    AscendingEnd = new DateTime(2026, 5, 15),
                    DescendingStart = new DateTime(1944, 3, 1),
                    DescendingEnd = new DateTime(1944, 3, 31),
                    OrbitalPeriodDays = 60190.0
                }
            };

        public static GeoDecNodeSeed Get(string planet)
        {
            if (_data.TryGetValue(planet, out var seed))
                return seed;

            throw new ArgumentException(
                $"No GeoDecNode seed data defined for planet '{planet}'.");
        }
    }
}