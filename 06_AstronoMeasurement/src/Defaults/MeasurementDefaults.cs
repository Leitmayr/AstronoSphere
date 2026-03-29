// ============================================================
// FILE: 06_AstronoMeasurement/src/AstronoMeasurement/Defaults/MeasurementDefaults.cs
// STATUS: NEU
// ============================================================

using System.Collections.Generic;
using AstronoMeasurement.Definitions;

namespace AstronoMeasurement.Defaults
{
    /// <summary>
    /// Default measurement configuration for M1.
    /// 
    /// KISS:
    /// - Hardcoded DefaultLevels = [L0]
    /// - No config, no external dependency
    /// </summary>
    public static class MeasurementDefaults
    {
        public static List<MeasurementDefinition> GetDefault()
        {
            return new List<MeasurementDefinition>
            {
                new MeasurementDefinition("L0")
            };
        }
    }
}
