// ============================================================
// FILE: 03_TruthFactory/src/EphemerisFactory/Core/HorizonsMetadataProvider.cs
// STATUS: NEW (M1.9 central metadata provider)
// ============================================================

namespace EphemerisFactory.Core
{
    public sealed class HorizonsMetadataProvider
    {
        public FactoryMetadataModel CreateFactoryMetadata(string mode, string level, string timeScale)
        {
            return new FactoryMetadataModel
            {
                FactoryName = "EphemerisFactory",
                FactoryVersion = "1.0.0",
                Source = "JPL Horizons https://ssd.jpl.nasa.gov/horizons/app.html#/",
                ReferenceEphemeris = "DE440",
                Mode = mode,
                EphemType = "VECTORS",
                CorrectionLevel = level,
                TimeScale = timeScale
            };
        }

        public TruthCitationModel CreateTruthCitation()
        {
            return new TruthCitationModel
            {
                Provider = "NASA - Jet Propulsion Laboratory, California Institute of Technology",
                Source = "https://ssd.jpl.nasa.gov/horizons/",
                Citation = "PL Solar System Dynamics Group. JPL Horizons On-Line Ephemeris System. California Institute of Technology. Accessed: 2026-03-23. https://ssd.jpl.nasa.gov/horizons/"
            };
        }

        public ProvenanceModel CreateProvenance()
        {
            return new ProvenanceModel
            {
                ScenarioFactory = "AstronoSphere | MeeusScenarioFactory",
                TruthFactory = "HorizonsTruthFactory",
                ValidationTarget = new ValidationTargetModel
                {
                    Software = "AstronoSphere",
                    GitCommit = null,
                    GitBranch = null,
                    GitTag = null
                }
            };
        }

        public EngineCitationModel CreateEngineCitation()
        {
            return new EngineCitationModel
            {
                Author = null,
                Software = null,
                Citation = null
            };
        }
    }

    public sealed class FactoryMetadataModel
    {
        public string FactoryName { get; set; } = "";
        public string FactoryVersion { get; set; } = "";
        public string Source { get; set; } = "";
        public string ReferenceEphemeris { get; set; } = "";
        public string Mode { get; set; } = "";
        public string EphemType { get; set; } = "";
        public string CorrectionLevel { get; set; } = "";
        public string TimeScale { get; set; } = "";
    }

    public sealed class TruthCitationModel
    {
        public string Provider { get; set; } = "";
        public string Source { get; set; } = "";
        public string Citation { get; set; } = "";
    }

    public sealed class ProvenanceModel
    {
        public string ScenarioFactory { get; set; } = "";
        public string TruthFactory { get; set; } = "";
        public ValidationTargetModel ValidationTarget { get; set; } = new();
    }

    public sealed class ValidationTargetModel
    {
        public string Software { get; set; } = "";
        public string? GitCommit { get; set; }
        public string? GitBranch { get; set; }
        public string? GitTag { get; set; }
    }

    public sealed class EngineCitationModel
    {
        public string? Author { get; set; }
        public string? Software { get; set; }
        public string? Citation { get; set; }
    }
}