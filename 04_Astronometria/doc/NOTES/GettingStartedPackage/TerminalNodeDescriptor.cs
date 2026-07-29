namespace Astronometria.Core.ScientificRun.Models
{
    public sealed class TerminalNodeDescriptor
    {
        public string TargetName { get; init; } = string.Empty;

        public string TargetAbbreviation { get; init; } = string.Empty;

        public string NodeId { get; init; } = string.Empty;

        public string NodeType { get; init; } = string.Empty;

        public string NodeRole { get; init; } = "TerminalNode";

        public string Status { get; init; } = "Planned";

        public string Origin { get; init; } = string.Empty;

        public string Plane { get; init; } = string.Empty;

        public string RefSystem { get; init; } = string.Empty;

        public string TimeScale { get; init; } = string.Empty;

        public string Output { get; init; } = string.Empty;

        public string CorrectionLevel { get; init; } = string.Empty;
    }
}