using Astronometria.Core.Bodies;
using AstroSim.Ephemerides.Test.EphemerisValidation.HorizonsValidation.Common;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Astronometria.Ephemerides.Test.EphemerisValidation.HorizonsValidation.Common
{
    public sealed class HorizonsValidationTestData
    {
        public string RepoRoot { get; }
        public string Vsop87APath { get; }
        public string ExperimentsRoot { get; }
        public string GroundTruthRoot { get; }

        public HorizonsValidationTestData()
        {
            RepoRoot = FindRepoRoot();

            Vsop87APath = Path.Combine(
                RepoRoot,
                "04_Astronometria",
                "src",
                "Astronometria.Ephemerides",
                "VSOP",
                "Data",
                "87A");

            ExperimentsRoot = Path.Combine(
                RepoRoot,
                "AstronoData",
                "02_Experiments",
                "Released");

            GroundTruthRoot = Path.Combine(
                RepoRoot,
                "AstronoData",
                "03_GroundTruth",
                "Ephemeris",
                "Horizons",
                "Baseline");
        }

        public ExperimentFile ReadExperimentByCatalog(string catalogNumber)
        {
            foreach (var file in Directory.GetFiles(ExperimentsRoot, "*.json"))
            {
                var experiment = ReadJson<ExperimentFile>(file);

                if (experiment.CatalogNumber == catalogNumber)
                    return experiment;
            }

            throw new InvalidOperationException($"Experiment not found for CatalogNumber: {catalogNumber}");
        }

        public GroundTruthFile ReadGroundTruthByCatalog(string catalogNumber)
        {
            foreach (var file in Directory.GetFiles(GroundTruthRoot, "*.json"))
            {
                var groundTruth = ReadJson<GroundTruthFile>(file);

                if (groundTruth.ExperimentRef.CatalogNumber == catalogNumber)
                    return groundTruth;
            }

            throw new InvalidOperationException($"GroundTruth not found for CatalogNumber: {catalogNumber}");
        }

        public static PlanetId GetSingleTargetPlanet(ExperimentFile experiment)
        {
            var target = experiment.Core.ObservedObject.Targets.Single();
            return Enum.Parse<PlanetId>(target);
        }

        private static T ReadJson<T>(string path)
        {
            var json = File.ReadAllText(path);

            return JsonSerializer.Deserialize<T>(
                       json,
                       new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                   ?? throw new InvalidOperationException($"Could not deserialize JSON file: {path}");
        }

        private static string FindRepoRoot()
        {
            var current = new DirectoryInfo(TestContext.CurrentContext.TestDirectory);

            while (current != null)
            {
                var hasAstronoData = Directory.Exists(Path.Combine(current.FullName, "AstronoData"));
                var hasAstronometria = Directory.Exists(Path.Combine(current.FullName, "04_Astronometria"));

                if (hasAstronoData && hasAstronometria)
                    return current.FullName;

                current = current.Parent;
            }

            throw new DirectoryNotFoundException(
                $"Could not find AstronoSphere repo root above test directory: {TestContext.CurrentContext.TestDirectory}");
        }
    }

    public sealed class ExperimentFile
    {
        public string ExperimentID { get; set; } = string.Empty;
        public string CatalogNumber { get; set; } = string.Empty;
        public string CoreHash { get; set; } = string.Empty;
        public ExperimentCore Core { get; set; } = new();
        public ExperimentEvent Event { get; set; } = new();
    }

    public sealed class ExperimentCore
    {
        public ExperimentTime Time { get; set; } = new();
        public ExperimentObserver Observer { get; set; } = new();
        public ExperimentObservedObject ObservedObject { get; set; } = new();
        public ExperimentFrame Frame { get; set; } = new();
    }

    public sealed class ExperimentTime
    {
        public double StartJD { get; set; }
        public double StopJD { get; set; }
        public string Step { get; set; } = string.Empty;
        public string TimeScale { get; set; } = string.Empty;
    }

    public sealed class ExperimentObserver
    {
        public string Type { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }

    public sealed class ExperimentObservedObject
    {
        public string BodyClass { get; set; } = string.Empty;
        public string[] Targets { get; set; } = Array.Empty<string>();
    }

    public sealed class ExperimentFrame
    {
        public string Type { get; set; } = string.Empty;
        public string Epoch { get; set; } = string.Empty;
    }

    public sealed class ExperimentEvent
    {
        public string Category { get; set; } = string.Empty;
        public string Qualifier { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}