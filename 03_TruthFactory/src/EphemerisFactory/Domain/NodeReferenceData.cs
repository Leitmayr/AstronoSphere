using System.Collections.Generic;

namespace EphemerisRegression.Domain
{
    public sealed class NodeReferenceData
    {
        public string Planet { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string Frame { get; set; } = string.Empty;

        public List<NodeEventReference> Events { get; set; }
            = new();
    }

    public sealed class NodeEventReference
    {
        public string Type { get; set; } = string.Empty;
        public double JulianDate { get; set; }

        public StateVector Before { get; set; }
        public StateVector After { get; set; }
    }
}
