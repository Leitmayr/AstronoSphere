using System.Collections.Generic;
using System.Linq;
using AstroSim.Data.Models;

namespace Astronometria.Adapters
{
    public static class StarRecordAdapter
    {
        public static List<Star> ToStars(
            IEnumerable<StarRecord> records,
            observationPoint obs,
            czeit time)
        {
            // Star-Konstruktor: (RA, Dec, mag, specType, obsPoint, time)
            return records
                .Select(r => new Star(
                    r.RightAscensionDeg,
                    r.DeclinationDeg,
                    r.VisualMagnitude,
                    r.SpectralTypeShort ?? string.Empty,
                    obs,
                    time))
                .ToList();
        }
    }
}
