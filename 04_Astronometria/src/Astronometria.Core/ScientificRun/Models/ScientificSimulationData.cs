using System.Collections.Generic;

namespace Astronometria.Core.ScientificRun.Models
{
    public sealed class ScientificSimulationData
    {
        public ScientificRunClassification RunClassification { get; init; } = new();

        public ScientificExperimentRef ExperimentRef { get; init; } = new();

        public ScientificMeasurement Measurement { get; init; } = new();

        public ScientificGroundTruthRef GroundTruthRef { get; init; } = new();

        public ScientificEngine Engine { get; init; } = new();

        public ScientificObservationScene ObservationScene { get; init; } = new();
    }

    public sealed class ScientificRunClassification
    {
        public string RunType { get; init; } = "ScientificRun";
        public string InputType { get; init; } = "CertifiedExperiment";
        public string TargetCardinality { get; init; } = "SingleTarget";
    }

    public sealed class ScientificExperimentRef
    {
        public string CatalogNumber { get; init; } = string.Empty;
        public string ExperimentID { get; init; } = string.Empty;
        public string CoreHash { get; init; } = string.Empty;
        public string SourceFile { get; init; } = string.Empty;
    }

    public sealed class ScientificMeasurement
    {
        public string Domain { get; init; } = "Ephemeris";
        public string Instrument { get; init; } = "VEC";
        public string CorrectionLevel { get; init; } = "L0";
        public string TimeScale { get; init; } = "TDB";
    }

    public sealed class ScientificGroundTruthRef
    {
        public string Provider { get; init; } = "Horizons";
        public string DatasetID { get; init; } = string.Empty;
        public string RequestHash { get; init; } = string.Empty;
        public string SourceFile { get; init; } = string.Empty;
    }

    public sealed class ScientificEngine
    {
        public string Name { get; init; } = "Astronometria";
        public ScientificSimulationModel SimulationModel { get; init; } = new();
        public ScientificBuild Build { get; init; } = new();
    }

    public sealed class ScientificSimulationModel
    {
        public string Family { get; init; } = "VSOP";
        public string Type { get; init; } = "VSOP87A";
        public string Truncation { get; init; } = "none";
        public string TimeDomain { get; init; } = "TT";
    }

    public sealed class ScientificBuild
    {
        public string GitCommit { get; init; } = string.Empty;
        public string GitBranch { get; init; } = string.Empty;
    }

    public sealed class ScientificObservationScene
    {
        public ScientificSceneContext SceneContext { get; init; } = new();
        public List<ScientificTargetSimulation> TargetSimulations { get; init; } = new();
    }

    public sealed class ScientificSceneContext
    {
        public string TemporalMode { get; init; } = "ObservationTime";
        public ScientificTime Time { get; init; } = new();
        public ScientificObserver Observer { get; init; } = new();
        public ScientificFrame Frame { get; init; } = new();
    }

    public sealed class ScientificTime
    {
        public double StartJD { get; init; }
        public double StopJD { get; init; }
        public string Step { get; init; } = string.Empty;
        public string TimeScale { get; init; } = string.Empty;
    }

    public sealed class ScientificObserver
    {
        public string Type { get; init; } = string.Empty;
        public string Body { get; init; } = string.Empty;
    }

    public sealed class ScientificFrame
    {
        public string Origin { get; init; } = string.Empty;
        public string Plane { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public string Epoch { get; init; } = string.Empty;
        public string RefSystem { get; init; } = string.Empty;
    }

    public sealed class ScientificTargetSimulation
    {
        public ScientificTarget Target { get; init; } = new();
        public ScientificTerminalNode TerminalNode { get; init; } = new();
    }

    public sealed class ScientificTarget
    {
        public string BodyClass { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
    }

    public sealed class ScientificTerminalNode
    {
        public string NodeId { get; init; } = string.Empty;
        public string NodeType { get; init; } = string.Empty;
        public string NodeRole { get; init; } = "TerminalNode";
        public string Status { get; init; } = "Completed";

        public string StateHash { get; set; } = string.Empty;
        public string DataHash { get; set; } = string.Empty;

        public List<ScientificDataSample> Data { get; init; } = new();
    }

    public sealed class ScientificDataSample
    {
        public double JD { get; init; }
        public ScientificVector Position { get; init; } = new();
        public ScientificVector Velocity { get; init; } = new();
    }

    public sealed class ScientificVector
    {
        public double X { get; init; }
        public double Y { get; init; }
        public double Z { get; init; }
    }
}