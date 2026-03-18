using EphemerisRegression.Domain;

namespace EphemerisRegression.Event
{
    public sealed class NodeEvent
    {
        public string Planet { get; init; } = "";
        public string NodeType { get; init; } = "";   // Ascending / Descending
        public double JulianDate { get; init; }

        public StateVector Before { get; set; } = null!;
        public StateVector At { get; set; } = null!;
        public StateVector After { get; set; } = null!;
    }
}
