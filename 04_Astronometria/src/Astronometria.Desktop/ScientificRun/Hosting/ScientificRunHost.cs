using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Astronometria.Core.ScientificRun.Build;
using Astronometria.Core.ScientificRun.Diagnostics;
using Astronometria.Core.ScientificRun.Hashing;
using Astronometria.Core.ScientificRun.IO;
using Astronometria.Core.ScientificRun.Models;
using Astronometria.Core.ScientificRun.Planning;
using Astronometria.ScientificRun.Cli;
using Astronometria.ScientificRun.Execution;

namespace Astronometria.ScientificRun.Hosting
{
    public static class ScientificRunHost
    {
        public static int Run(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var cli = CliParser.Parse(args);

            if (cli.ShowHelp)
            {
                PrintHelp();
                return 0;
            }

            Console.WriteLine("=== Astronometria ScientificRun ===");

            var buildInfo = BuildInfoService.CreateOrUpdateBuildInfo();

            Console.WriteLine($"GitCommit : {buildInfo.GitCommit}");
            Console.WriteLine($"GitBranch : {buildInfo.GitBranch}");
            Console.WriteLine($"Root      : {buildInfo.RepositoryRoot}");
            Console.WriteLine();

            if (cli.RunAll)
            {
                return RunAll(buildInfo);
            }

            if (cli.IsSingleExperimentRun)
            {
                Console.WriteLine("Mode      : Single Experiment");
                Console.WriteLine($"Catalog   : {cli.CatalogNumber}");
                Console.WriteLine();

                var runFolder = ScientificRunFolderManager.PrepareRunFolder(
                    buildInfo.RepositoryRoot,
                    rotateExistingRun: true);

                var diagRunFolder = ScientificRunDiagnosticFolderManager.PrepareRunFolder(
                    buildInfo.RepositoryRoot,
                    rotateExistingRun: true);

                return ExecuteSingleExperiment(
                    buildInfo,
                    cli.CatalogNumber!,
                    runFolder,
                    diagRunFolder,
                    printDeltas: false);
            }

            PrintHelp();
            return 0;
        }

        private static int RunAll(BuildInfo buildInfo)
        {
            Console.WriteLine("Mode      : Full Batch Run");
            Console.WriteLine();

            var releasedFolder = Path.Combine(
                buildInfo.RepositoryRoot,
                "AstronoData",
                "02_Experiments",
                "Released");

            var runFolder = ScientificRunFolderManager.PrepareRunFolder(
                buildInfo.RepositoryRoot,
                rotateExistingRun: true);

            var diagRunFolder = ScientificRunDiagnosticFolderManager.PrepareRunFolder(
                buildInfo.RepositoryRoot,
                rotateExistingRun: true);

            var files = Directory
                .GetFiles(releasedFolder, "*.json")
                .OrderBy(x => x, StringComparer.Ordinal)
                .ToList();

            foreach (var file in files)
            {
                var catalogNumber =
                    Path.GetFileName(file)
                        .Split("__")[0];

                Console.WriteLine($"=== Batch item: {catalogNumber} ===");

                ExecuteSingleExperiment(
                    buildInfo,
                    catalogNumber,
                    runFolder,
                    diagRunFolder,
                    printDeltas: false);

                Console.WriteLine();
            }

            Console.WriteLine("Full Batch Run complete.");
            Console.WriteLine($"Processed files: {files.Count}");

            return 0;
        }

        private static int ExecuteSingleExperiment(
            BuildInfo buildInfo,
            string catalogNumber,
            string runFolder,
            string diagRunFolder,
            bool printDeltas)
        {
            var experiment = ExperimentLoader.LoadByCatalogNumber(
                buildInfo.RepositoryRoot,
                catalogNumber);

            Console.WriteLine("=== Experiment loaded ===");
            Console.WriteLine($"CatalogNumber : {experiment.CatalogNumber}");
            Console.WriteLine($"ExperimentID  : {experiment.ExperimentID}");
            Console.WriteLine($"CoreHash      : {experiment.CoreHash}");
            Console.WriteLine($"SourceFile    : {experiment.SourceFile}");
            Console.WriteLine($"Target        : {experiment.Core.ObservedObject.Targets[0]}");
            Console.WriteLine($"Frame         : {experiment.Core.Frame.Type}");
            Console.WriteLine($"Epoch         : {experiment.Core.Frame.Epoch}");
            Console.WriteLine($"TimeScale     : {experiment.Core.Time.TimeScale}");
            Console.WriteLine($"StartJD       : {FormatDouble(experiment.Core.Time.StartJD)}");
            Console.WriteLine($"StopJD        : {FormatDouble(experiment.Core.Time.StopJD)}");
            Console.WriteLine($"Step          : {experiment.Core.Time.Step}");
            Console.WriteLine();

            if (!string.Equals(
                    experiment.Metadata.Status.Maturity,
                    "Released",
                    StringComparison.OrdinalIgnoreCase))
            {
                var message =
                    $"Invalid ExperimentMaturity '{experiment.Metadata.Status.Maturity}' for CatalogNumber '{experiment.CatalogNumber}'.";

                var diagPath = ScientificRunDiagnosticWriter.WriteResolutionDiagnostic(
                    diagRunFolder,
                    experiment,
                    "040.003",
                    message,
                    Array.Empty<string>());

                Console.WriteLine("=== ScientificRun diagnostic written ===");
                Console.WriteLine("Code       : 040.003");
                Console.WriteLine($"Message    : {message}");
                Console.WriteLine($"DiagFile   : {diagPath}");
                Console.WriteLine();

                return 0;
            }

            if (!string.Equals(
                    experiment.Core.Frame.Type,
                    "HelioEcliptic",
                    StringComparison.OrdinalIgnoreCase)
                &&
                !string.Equals(
                    experiment.Core.Frame.Type,
                    "GeoEcliptic",
                    StringComparison.OrdinalIgnoreCase))
            {
                var message =
                    $"Unsupported ScientificRun configuration. Frame.Type='{experiment.Core.Frame.Type}' is not supported in M2.3/M2.4.";

                var diagPath = ScientificRunDiagnosticWriter.WriteResolutionDiagnostic(
                    diagRunFolder,
                    experiment,
                    "040.008",
                    message,
                    Array.Empty<string>());

                Console.WriteLine("=== ScientificRun diagnostic written ===");
                Console.WriteLine("Code       : 040.008");
                Console.WriteLine($"Message    : {message}");
                Console.WriteLine($"DiagFile   : {diagPath}");
                Console.WriteLine();

                return 0;
            }

            GroundTruthInputModel groundTruth;

            try
            {
                groundTruth = GroundTruthLoader.ResolveSingleBaseline(
                    buildInfo.RepositoryRoot,
                    experiment);
            }
            catch (GroundTruthResolutionException ex)
            {
                var diagPath = ScientificRunDiagnosticWriter.WriteResolutionDiagnostic(
                    diagRunFolder,
                    experiment,
                    ex.DiagnosticCode,
                    ex.Message,
                    ex.MatchingFiles);

                Console.WriteLine("=== ScientificRun diagnostic written ===");
                Console.WriteLine($"Code       : {ex.DiagnosticCode}");
                Console.WriteLine($"Message    : {ex.Message}");
                Console.WriteLine($"DiagFile   : {diagPath}");
                Console.WriteLine();

                return 0;
            }

            Console.WriteLine("=== GroundTruth resolved ===");
            Console.WriteLine("Provider      : Horizons");
            Console.WriteLine($"DatasetID     : {groundTruth.DatasetHeader.DatasetID}");
            Console.WriteLine($"RequestHash   : {groundTruth.DatasetHeader.TruthMetadata.RequestHash}");
            Console.WriteLine($"SourceFile    : {groundTruth.SourceFile}");
            Console.WriteLine($"Samples       : {groundTruth.Data.Count}");
            Console.WriteLine($"FirstJD       : {FormatDouble(groundTruth.Data[0].JD)}");
            Console.WriteLine($"LastJD        : {FormatDouble(groundTruth.Data[^1].JD)}");
            Console.WriteLine();

            var terminalNode = TerminalNodeDeriver.Derive(
                experiment,
                groundTruth);

            Console.WriteLine("=== TerminalNode derived ===");
            Console.WriteLine($"Target        : {terminalNode.TargetName}");
            Console.WriteLine($"Abbreviation  : {terminalNode.TargetAbbreviation}");
            Console.WriteLine($"NodeId        : {terminalNode.NodeId}");
            Console.WriteLine($"NodeType      : {terminalNode.NodeType}");
            Console.WriteLine($"NodeRole      : {terminalNode.NodeRole}");
            Console.WriteLine($"Status        : {terminalNode.Status}");
            Console.WriteLine($"Origin        : {terminalNode.Origin}");
            Console.WriteLine($"Plane         : {terminalNode.Plane}");
            Console.WriteLine($"RefSystem     : {terminalNode.RefSystem}");
            Console.WriteLine($"TimeScale     : {terminalNode.TimeScale}");
            Console.WriteLine($"Output        : {terminalNode.Output}");
            Console.WriteLine($"Level         : {terminalNode.CorrectionLevel}");
            Console.WriteLine();

            var simulationData = ScientificRunSimulationExecutor.Execute(
                buildInfo.RepositoryRoot,
                buildInfo,
                experiment,
                groundTruth,
                terminalNode);

            var stateHash = ScientificHashService.ComputeStateHash(
                simulationData);

            var dataHash = ScientificHashService.ComputeDataHash(
                simulationData);

            simulationData
                .ObservationScene
                .TargetSimulations[0]
                .TerminalNode
                .StateHash = stateHash;

            simulationData
                .ObservationScene
                .TargetSimulations[0]
                .TerminalNode
                .DataHash = dataHash;

            Console.WriteLine("=== Hashes ===");
            Console.WriteLine($"StateHash    : {stateHash}");
            Console.WriteLine($"DataHash     : {dataHash}");
            Console.WriteLine();

            var outputPath = ScientificSimulationOutputPathBuilder.BuildRunFilePath(
                runFolder,
                experiment,
                terminalNode);

            ScientificSimulationJsonWriter.Write(
                outputPath,
                simulationData);

            Console.WriteLine("=== SimulationData written ===");
            Console.WriteLine($"OutputFile    : {outputPath}");
            Console.WriteLine($"Samples       : {simulationData.ObservationScene.TargetSimulations[0].TerminalNode.Data.Count}");
            Console.WriteLine();

            if (printDeltas)
            {
                PrintDeltas(
                    experiment,
                    groundTruth,
                    simulationData);
            }

            return 0;
        }

        private static void PrintDeltas(
            ExperimentInputModel experiment,
            GroundTruthInputModel groundTruth,
            ScientificSimulationData simulationData)
        {
            Console.WriteLine("=== Delta VSOP - Horizons ===");

            var samples =
                simulationData
                    .ObservationScene
                    .TargetSimulations[0]
                    .TerminalNode
                    .Data;

            for (var i = 0; i < samples.Count; i++)
            {
                var sim = samples[i];
                var gt = groundTruth.Data[i];

                var dx = Math.Abs(sim.Position.X - gt.Position.X);
                var dy = Math.Abs(sim.Position.Y - gt.Position.Y);
                var dz = Math.Abs(sim.Position.Z - gt.Position.Z);

                var dmax = Math.Max(dx, Math.Max(dy, dz));

                Console.WriteLine(
                    $"{experiment.CatalogNumber} | " +
                    $"{experiment.Core.Frame.Type} | " +
                    $"JD={FormatDeltaNumber(sim.JD)} | " +
                    $"ΔX={FormatScientific(dx)} " +
                    $"ΔY={FormatScientific(dy)} " +
                    $"ΔZ={FormatScientific(dz)} " +
                    $"Δmax={FormatScientific(dmax)} | " +
                    $"tol={FormatScientific(0.000005)}");
            }

            Console.WriteLine();
        }

        private static string FormatDouble(double value)
        {
            return value.ToString(
                "0.###############",
                CultureInfo.InvariantCulture);
        }

        private static string FormatDeltaNumber(double value)
        {
            return value.ToString(
                "G17",
                CultureInfo.GetCultureInfo("de-DE"));
        }

        private static string FormatScientific(double value)
        {
            return value.ToString(
                "0.000E+000",
                CultureInfo.GetCultureInfo("de-DE"));
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Astronometria ScientificRun CLI");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("  Astronometria.Desktop.exe --catalog AS-000003");
            Console.WriteLine("  Astronometria.Desktop.exe --all");
            Console.WriteLine();
            Console.WriteLine("M2.3/M2.4 currently supports ScientificRun only.");
        }
    }
}