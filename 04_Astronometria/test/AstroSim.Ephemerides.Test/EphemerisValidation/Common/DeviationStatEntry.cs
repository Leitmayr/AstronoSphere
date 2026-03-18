namespace AstroSim.Ephemerides.Test.EphemerisValidation.Common
{
    public sealed class DeviationStatEntry
    {
        public string Planet { get; set; } = "";
        public string Suite { get; set; } = "";
        public string EventType { get; set; } = "";

        public double MaxX { get; set; }
        public double MaxY { get; set; }
        public double MaxZ { get; set; }

        public double RmsX { get; set; }
        public double RmsY { get; set; }
        public double RmsZ { get; set; }
    }
}

