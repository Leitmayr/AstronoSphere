using System;
using Astronometria.Core.ScientificRun.Models;

namespace Astronometria.Core.ScientificRun.StateTree
{
    /// <summary>
    /// PURPOSE:
    /// Maps M2.4 MeasurementDefinition semantics to the internal PHYS.* terminal node type.
    ///
    /// CONTEXT:
    /// MeasurementDefinition is engine-neutral. The StateTree node type is the
    /// Astronometria internal execution target derived from that measurement.
    ///
    /// CONSTRAINTS:
    /// M2.4.0 supports only Ephemeris / VEC / L0 / J2000 / TDB for
    /// HELIO-ECL, GEO-ECL and GEO-EQU.
    /// </summary>
    public static class MeasurementDefinitionToNodeTypeMapper
    {
        public static PhysicsStateNodeType Map(
            ExperimentInputModel experiment,
            GroundTruthInputModel groundTruth)
        {
            if (experiment == null)
                throw new ArgumentNullException(nameof(experiment));

            if (groundTruth == null)
                throw new ArgumentNullException(nameof(groundTruth));

            var correctionLevel = NormalizeCorrectionLevel(groundTruth.DatasetHeader.Measurement.Level);
            var output = NormalizeOutput(groundTruth.DatasetHeader.Measurement.Type);
            var epoch = NormalizeEpoch(experiment.Core.Frame.Epoch);
            var timeScale = NormalizeTimeScale(experiment.Core.Time.TimeScale);

            if (correctionLevel != "L0")
                throw new NotSupportedException(
                    $"Unsupported correction level for M2.4.0 StateTree: '{correctionLevel}'.");

            if (output != "VEC")
                throw new NotSupportedException(
                    $"Unsupported measurement output for M2.4.0 StateTree: '{output}'.");

            if (epoch != "J2000")
                throw new NotSupportedException(
                    $"Unsupported epoch for M2.4.0 StateTree: '{epoch}'.");

            if (timeScale != "TDB")
                throw new NotSupportedException(
                    $"Unsupported measurement time scale for M2.4.0 StateTree: '{timeScale}'.");

            return experiment.Core.Frame.Type switch
            {
                "HelioEcliptic" => PhysicsStateNodeType.HelioEclJ2000VecL0,
                "GeoEcliptic" => PhysicsStateNodeType.GeoEclJ2000VecL0,
                "GeoEquatorial" => PhysicsStateNodeType.GeoEquJ2000VecL0,
                _ => throw new NotSupportedException(
                    $"Unsupported frame type for M2.4.0 StateTree: '{experiment.Core.Frame.Type}'.")
            };
        }

        private static string NormalizeCorrectionLevel(string level)
        {
            return level switch
            {
                "L0" => "L0",
                _ => throw new NotSupportedException(
                    $"Unsupported correction level for M2.4.0 StateTree: '{level}'.")
            };
        }

        private static string NormalizeOutput(string measurementType)
        {
            return measurementType switch
            {
                "VEC" => "VEC",
                "VECTORS" => "VEC",
                _ => throw new NotSupportedException(
                    $"Unsupported measurement type for M2.4.0 StateTree: '{measurementType}'.")
            };
        }

        private static string NormalizeEpoch(string epoch)
        {
            return epoch switch
            {
                "J2000" => "J2000",
                _ => throw new NotSupportedException(
                    $"Unsupported epoch for M2.4.0 StateTree: '{epoch}'.")
            };
        }

        private static string NormalizeTimeScale(string timeScale)
        {
            return timeScale switch
            {
                "TDB" => "TDB",
                _ => throw new NotSupportedException(
                    $"Unsupported time scale for M2.4.0 StateTree: '{timeScale}'.")
            };
        }
    }
}