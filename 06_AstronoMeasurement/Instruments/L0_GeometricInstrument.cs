// ============================================================
// FILE: 06_AstronoMeasurement/Instruments/L0_Geometric/L0_GeometricInstrument.cs
// STATUS: NEW
// ============================================================

using AstronoMeasurement.Contracts;

namespace AstronoMeasurement.Instruments.L0_Geometric
{
    /// <summary>
    /// PURPOSE:
    /// Defines the L0 measurement instrument (pure geometric state).
    ///
    /// CONTEXT:
    /// L0 represents the baseline measurement:
    /// - no light-time correction
    /// - no aberration
    /// - no relativistic effects
    ///
    /// This corresponds to the geometric pipeline output of Astronometria.
    ///
    /// CONSTRAINTS:
    /// - no physics implementation
    /// - no dependency on engine or factory
    /// - purely declarative definition
    /// </summary>
    public sealed class L0_GeometricInstrument : IInstrument
    {
        public string Id => "L0";

        public string Name => "Geometric";

        public string Description =>
            "Pure geometric state vectors without physical corrections (baseline measurement).";
    }
}