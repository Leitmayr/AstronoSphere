namespace Astronometria.Ephemerides.Test.EphemerisValidation.Common
{
    public sealed class NodeEvent
    {
        public double JulianDate { get; set; }

        public VectorEntry Before { get; set; } = new();
        public VectorEntry At { get; set; } = new();
        public VectorEntry After { get; set; } = new();
    }
}
