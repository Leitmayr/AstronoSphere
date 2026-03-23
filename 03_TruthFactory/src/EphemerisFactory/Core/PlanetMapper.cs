// ============================================================
// FILE: PlanetMapper.cs
// STATUS: NEW
// ============================================================

using System;
using System.Collections.Generic;

namespace EphemerisFactory.Core
{
    public static class PlanetMapper
    {
        private static readonly Dictionary<string, int> Map = new()
        {
            ["Mercury"] = 199,
            ["Venus"] = 299,
            ["Earth"] = 399,
            ["Mars"] = 499,
            ["Jupiter"] = 599,
            ["Saturn"] = 699,
            ["Uranus"] = 799,
            ["Neptune"] = 899
        };

        public static int ToCommand(string name)
        {
            if (!Map.TryGetValue(name, out var code))
                throw new Exception($"Unknown target: {name}");

            return code;
        }
    }
}
