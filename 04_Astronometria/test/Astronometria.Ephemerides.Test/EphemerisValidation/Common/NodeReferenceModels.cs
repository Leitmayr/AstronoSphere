namespace Astronometria.Ephemerides.Test.EphemerisValidation.Common
{
    public sealed class NodeReferenceModel
    {
        public string Planet { get; set; } = "";
        public string TestSuite { get; set; } = "";
        public string EventName { get; set; } = "";
        public string CorrectionLevel { get; set; } = "";

        public object Metadata { get; set; } = new();

        public NodeEvent Node { get; set; } = new();
    }
}

