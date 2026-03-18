using System.Collections.Generic;

namespace AstroSim.Ephemerides.Test.EphemerisValidation.Common
{
    public sealed class ReferenceData
    {
        public string Planet { get; set; } = "";
        public string TestSuite { get; set; } = "";
        public string Event { get; set; } = "";
        public string CorrectionLevel { get; set; } = "";
        public List<VectorEntry> Vectors { get; set; } = new();
    }

    public sealed class VectorEntry
    {
        public double JulianDate { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double VX { get; set; }
        public double VY { get; set; }
        public double VZ { get; set; }
    }
}
