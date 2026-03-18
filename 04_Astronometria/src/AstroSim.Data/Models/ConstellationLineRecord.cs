using AstroSim.Core.Coordinates;

namespace AstroSim.Data.Models
{
    public sealed class ConstellationLineRecord
    {
        public string ConstellationIAU3 { get; set; } = "";
        public EquatorialCoord Start { get; set; }
        public EquatorialCoord End { get; set; }
    }
}
