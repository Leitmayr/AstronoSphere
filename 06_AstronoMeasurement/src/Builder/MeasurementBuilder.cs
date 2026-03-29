// ============================================================
// FILE: 06_AstronoMeasurement/src/AstronoMeasurement/Builder/MeasurementBuilder.cs
// STATUS: NEU
// ============================================================

using System.Collections.Generic;
using AstronoMeasurement.Definitions;
using AstronoMeasurement.Keys;

namespace AstronoMeasurement.Builder
{
    /// <summary>
    /// Builds concrete measurement instances from definitions.
    /// 
    /// KISS (M1):
    /// - Only L0 supported
    /// - Type is always VEC
    /// </summary>
    public sealed class MeasurementBuilder
    {
        /// <summary>
        /// Builds MeasurementKeys from definitions
        /// </summary>
        public List<MeasurementKey> Build(List<MeasurementDefinition> definitions)
        {
            var result = new List<MeasurementKey>();

            foreach (var def in definitions)
            {
                // M1: fixed mapping
                var key = new MeasurementKey(def.Level, "VEC");

                result.Add(key);
            }

            return result;
        }
    }
}