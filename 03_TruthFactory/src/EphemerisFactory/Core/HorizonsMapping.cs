// ============================================================
// FILE: 03_TruthFactory/src/EphemerisFactory/Core/HorizonsMapping.cs
// STATUS: NEU
// ============================================================

using System;

namespace EphemerisFactory.Core
{
    /// <summary>
    /// Central mapping between Measurement (Level + Type)
    /// and Horizons API parameters.
    ///
    /// KISS (M1):
    /// - Only L0-VEC supported
    /// - No corrections
    /// </summary>
    public static class HorizonsMapping
    {
        public static string GetEphemType(string level, string type)
        {
            // M1: only vector output
            if (type == "VEC")
                return "VECTORS";

            throw new Exception($"Unsupported measurement type: {type}");
        }

        public static string? GetVectorCorrection(string level)
        {
            // M1: no corrections
            if (level == "L0")
                return null;

            throw new Exception($"Unsupported level: {level}");
        }
    }
}