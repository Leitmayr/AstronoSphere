namespace EphemerisRegression.Domain
{
    public sealed class NodeReferenceModel
    {
        public string Planet { get; set; } = "";
        public string TestSuite { get; set; } = "";
        public string EventName { get; set; } = "";
        public string CorrectionLevel { get; set; } = "";

        // bewusst object, damit keine Typbindung entsteht
        public object Metadata { get; set; } = new();

        public NodeEvent Node { get; set; } = new();
    }

    public sealed class NodeEvent
    {
        public double JulianDate { get; set; }

        public StateVector Before { get; set; } = default!;
        public StateVector At { get; set; } = default!;
        public StateVector After { get; set; } = default!;
    }
}
