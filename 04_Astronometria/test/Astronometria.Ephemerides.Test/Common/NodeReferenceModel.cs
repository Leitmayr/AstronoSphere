namespace Astronometria.Ephemerides.Test.Common
{
    public class NodeReferenceModel
    {
        public NodeEvent Ascending { get; set; }
        public NodeEvent Descending { get; set; }
    }

    public class NodeEvent
    {
        public double JulianDate { get; set; }
        public double Value { get; set; }
    }
}