using System;
using Astronometria.Core.ScientificRun.Models;
using Astronometria.Core.ScientificRun.StateTree;

namespace Astronometria.Core.ScientificRun.Planning
{
    public static class TerminalNodeDeriver
    {
        public static TerminalNodeDescriptor Derive(
            ExperimentInputModel experiment,
            GroundTruthInputModel groundTruth)
        {
            if (experiment == null)
                throw new ArgumentNullException(nameof(experiment));

            if (groundTruth == null)
                throw new ArgumentNullException(nameof(groundTruth));

            var targetName = GetSingleTarget(experiment);
            var targetAbbreviation = GetPlanetAbbreviation(targetName);

            var origin = GetOrigin(experiment.Core.Frame.Type);
            var plane = GetPlane(experiment.Core.Frame.Type);
            var refSystem = GetRefSystem(experiment.Core.Frame.Epoch);
            var timeScale = experiment.Core.Time.TimeScale;
            var output = NormalizeOutput(groundTruth.DatasetHeader.Measurement.Type);
            var correctionLevel = NormalizeCorrectionLevel(groundTruth.DatasetHeader.Measurement.Level);

            var physicsNodeType = MeasurementDefinitionToNodeTypeMapper.Map(
                experiment,
                groundTruth);

            PhysicsStateTreeRegistry.ResolvePath(physicsNodeType);

            var nodeType =
                $"VSOP87.{correctionLevel}.{origin}.{plane}.{refSystem}.{timeScale}.{output}";

            return new TerminalNodeDescriptor
            {
                TargetName = targetName,
                TargetAbbreviation = targetAbbreviation,
                NodeId = $"{targetAbbreviation}_NODE_001",
                NodeType = nodeType,
                PhysicsNodeType = physicsNodeType.Value,
                NodeRole = "TerminalNode",
                Status = "Planned",
                Origin = origin,
                Plane = plane,
                RefSystem = refSystem,
                TimeScale = timeScale,
                Output = output,
                CorrectionLevel = correctionLevel
            };
        }

        private static string GetSingleTarget(ExperimentInputModel experiment)
        {
            if (experiment.Core.ObservedObject.Targets.Count != 1)
                throw new InvalidOperationException(
                    "ScientificRun requires exactly one target.");

            var target = experiment.Core.ObservedObject.Targets[0];

            if (string.IsNullOrWhiteSpace(target))
                throw new InvalidOperationException("Target must not be empty.");

            return target;
        }

        private static string GetPlanetAbbreviation(string targetName)
        {
            return targetName.Trim().ToUpperInvariant() switch
            {
                "MERCURY" => "MER",
                "VENUS" => "VEN",
                "EARTH" => "EAR",
                "MARS" => "MAR",
                "JUPITER" => "JUP",
                "SATURN" => "SAT",
                "URANUS" => "URA",
                "NEPTUNE" => "NEP",
                _ => throw new NotSupportedException(
                    $"Unsupported target for M2.4.0 ScientificRun: '{targetName}'.")
            };
        }

        private static string GetOrigin(string frameType)
        {
            return frameType switch
            {
                "HelioEcliptic" => "HELIO",
                "GeoEcliptic" => "GEO",
                "GeoEquatorial" => "GEO",
                _ => throw new NotSupportedException(
                    $"Unsupported frame type for M2.4.0 ScientificRun: '{frameType}'.")
            };
        }

        private static string GetPlane(string frameType)
        {
            return frameType switch
            {
                "HelioEcliptic" => "ECL",
                "GeoEcliptic" => "ECL",
                "GeoEquatorial" => "EQU",
                _ => throw new NotSupportedException(
                    $"Unsupported frame type for M2.4.0 ScientificRun: '{frameType}'.")
            };
        }

        private static string GetRefSystem(string epoch)
        {
            return epoch switch
            {
                "J2000" => "J2000",
                _ => throw new NotSupportedException(
                    $"Unsupported reference system for M2.4.0 ScientificRun: '{epoch}'.")
            };
        }

        private static string NormalizeOutput(string measurementType)
        {
            return measurementType switch
            {
                "VEC" => "VEC",
                "VECTORS" => "VEC",
                _ => throw new NotSupportedException(
                    $"Unsupported measurement type for M2.4.0 ScientificRun: '{measurementType}'.")
            };
        }

        private static string NormalizeCorrectionLevel(string level)
        {
            return level switch
            {
                "L0" => "L0",
                _ => throw new NotSupportedException(
                    $"Unsupported correction level for M2.4.0 ScientificRun: '{level}'.")
            };
        }
    }
}