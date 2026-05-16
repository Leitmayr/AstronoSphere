using System;
using System.Globalization;
using System.IO;
using Astronometria.Core.ScientificRun.Models;

namespace Astronometria.Core.ScientificRun.IO
{
    public static class ScientificSimulationOutputPathBuilder
    {
        public static string BuildRunFilePath(
            string runFolder,
            ExperimentInputModel experiment,
            TerminalNodeDescriptor terminalNode)
        {
            Directory.CreateDirectory(runFolder);

            var humanPart = ExtractHumanPart(experiment.SourceFile);
            var statePart = BuildStatePart(experiment, terminalNode);
            var simulationPart = "VSOP87-VEC-L0";

            var fileName =
                $"{experiment.CatalogNumber}__{humanPart}__{statePart}__{simulationPart}.json";

            return Path.Combine(runFolder, fileName);
        }

        private static string ExtractHumanPart(string sourceFile)
        {
            var name = Path.GetFileNameWithoutExtension(sourceFile);
            var parts = name.Split("__", StringSplitOptions.None);

            if (parts.Length < 3)
                throw new InvalidOperationException(
                    $"Cannot derive human filename part from experiment source file '{sourceFile}'.");

            return parts[1];
        }

        private static string BuildStatePart(
            ExperimentInputModel experiment,
            TerminalNodeDescriptor terminalNode)
        {
            return string.Join(
                "-",
                terminalNode.Origin,
                terminalNode.Plane,
                experiment.Core.Frame.Epoch,
                experiment.Core.Time.TimeScale,
                FormatIdNumber(experiment.Core.Time.StartJD),
                FormatIdNumber(experiment.Core.Time.StopJD),
                experiment.Core.Time.Step);
        }

        private static string FormatIdNumber(double value)
        {
            return Math.Truncate(value)
                .ToString("0", CultureInfo.InvariantCulture);
        }
    }
}