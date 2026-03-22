namespace AstroSim.Data.Models
{
    /// <summary>
    /// Rohdatensatz aus einem Sternkatalog (z. B. Bright Star Catalog).
    /// </summary>
    public sealed class StarRecord
    {
        public int HarvardRevisedNumber { get; set; }
        public string? Name { get; set; }
        public string? HD { get; set; }

        public double VisualMagnitude { get; set; }
        public string? SpectralTypeShort { get; set; }

        public string? ConstellationShort { get; set; }
        public string? ConstellationLong { get; set; }
        public string? ConstellationGerman { get; set; }

        public string? GreekLetter { get; set; }

        public double RightAscensionDeg { get; set; } // RAdeg
        public double DeclinationDeg { get; set; }    // DECdeg
    }
}
