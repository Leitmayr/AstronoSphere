// ============================================================
// FILE: 06_AstronoMeasurement/Contracts/IInstrument.cs
// STATUS: NEW
// ============================================================

namespace AstronoMeasurement.Contracts
{
    /// <summary>
    /// PURPOSE:
    /// Defines a measurement instrument in AstronoSphere.
    ///
    /// CONTEXT:
    /// Instruments describe how a scenario is measured, independent of:
    /// - scenario definition
    /// - factory implementation
    /// - engine computation
    ///
    /// CONSTRAINTS:
    /// - Must be deterministic
    /// - Must not contain any physics or computation logic
    /// - Pure semantic definition only
    /// </summary>
    public interface IInstrument
    {
        /// <summary>
        /// Unique instrument identifier (e.g. "L0", "L1")
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Human readable name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Description of the measurement semantics
        /// </summary>
        string Description { get; }
    }
}