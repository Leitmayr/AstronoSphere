// ============================================================
// FILE: 06_AstronoMeasurement/src/AstronoMeasurement/Definitions/MeasurementDefinition.cs
// STATUS: NEU
// ============================================================

namespace AstronoMeasurement.Definitions
{
    /// <summary>
    /// Represents a semantic measurement definition (Instrument).
    /// 
    /// KISS (M1):
    /// - Only Level (L0)
    /// - Type is implicitly VEC (fixed for M1)
    /// 
    /// Future (M2+):
    /// - Extend with correction flags or config
    /// </summary>
    public sealed class MeasurementDefinition
    {
        /// <summary>
        /// Measurement level (e.g. L0, L1, L2)
        /// </summary>
        public string Level { get; }

        public MeasurementDefinition(string level)
        {
            Level = level;
        }
    }
}