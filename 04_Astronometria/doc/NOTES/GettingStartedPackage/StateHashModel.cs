namespace Astronometria.Core.ScientificRun.Hashing
{
    public sealed class StateHashModel
    {
        public string EngineName { get; init; } = string.Empty;

        public string SimulationFamily { get; init; } = string.Empty;

        public string SimulationType { get; init; } = string.Empty;

        public string SimulationTruncation { get; init; } = string.Empty;

        public string SimulationTimeDomain { get; init; } = string.Empty;

        public string TemporalMode { get; init; } = string.Empty;

        public double StartJD { get; init; }

        public double StopJD { get; init; }

        public string Step { get; init; } = string.Empty;

        public string TimeScale { get; init; } = string.Empty;

        public string ObserverType { get; init; } = string.Empty;

        public string ObserverBody { get; init; } = string.Empty;

        public string FrameOrigin { get; init; } = string.Empty;

        public string FramePlane { get; init; } = string.Empty;

        public string FrameType { get; init; } = string.Empty;

        public string FrameEpoch { get; init; } = string.Empty;

        public string RefSystem { get; init; } = string.Empty;

        public string TargetBodyClass { get; init; } = string.Empty;

        public string TargetName { get; init; } = string.Empty;

        public string TargetAbbreviation { get; init; } = string.Empty;

        public string NodeId { get; init; } = string.Empty;

        public string NodeType { get; init; } = string.Empty;

        public string Output { get; init; } = string.Empty;

        public string CorrectionLevel { get; init; } = string.Empty;

        public string NumericalPolicy { get; init; } = "STRICT_9_TRUNCATE";
    }
}