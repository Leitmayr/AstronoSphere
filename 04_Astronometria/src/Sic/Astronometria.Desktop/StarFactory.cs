using System;
using System.Collections.Generic;
using System.Text;

namespace Astronometria
{
    public static class StarFactory
    {
        public static List<Star> CreateStars(
            observationPoint obs,
            czeit time,
            double maxMag)
        {
            var result = new List<Star>();

            foreach (var cs in StarCatalog.AllStars)
            {
                if (cs.VisualMagnitude > maxMag)
                    continue;

                var star = new Star(
                    cs.RAdeg,
                    cs.DECdeg,
                    cs.VisualMagnitude,
                    cs.SpectralTypeShort,
                    obs,
                    time
                );

                result.Add(star);
            }

            return result;
        }
    }
}
