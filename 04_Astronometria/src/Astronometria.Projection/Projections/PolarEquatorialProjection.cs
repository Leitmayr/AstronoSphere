using System;
using Astronometria.Core.Coordinates;
using Astronometria.Core.Site;
using Astronometria.Core.Time;
using Astronometria.Projection.Transforms;

namespace Astronometria.Projection.Projections
{
    /// <summary>
    /// Polarprojektion (Polaris im Zentrum), basierend auf:
    /// r = (90 - Dec) / (180 - geoLat)
    /// gamma = 15° * Stundenwinkel
    /// Map01: (0.5,0.5) ist Zentrum
    /// </summary>
    public sealed class PolarEquatorialProjection : IEquatorialMapProjection
    {
        private readonly IHourAngleCalculator _hourAngle;

        public PolarEquatorialProjection(IHourAngleCalculator hourAngle)
        {
            _hourAngle = hourAngle;
        }

        public MapPoint01 Project(EquatorialCoord eq, ObservationSite site, AstroTimeUT time)
        {
            // Radius in "Kartenradien"
            double r = (90.0 - eq.Decdeg) / (180.0 - site.LatitudeDeg);

            // gamma aus Stundenwinkel
            double haHours = _hourAngle.GetHourAngleHours(time, eq, site);
            double gammaDeg = 15.0 * haHours;

            // centered cartesian
            double xCentered = r * Math.Sin(Math.PI / 180.0 * gammaDeg);
            double yCentered = r * Math.Cos(Math.PI / 180.0 * gammaDeg);

            // Map01 (du hast bestätigt: Y muss gespiegelt werden)
            double mapX = 0.5 + xCentered / 2.0;
            double mapY = 0.5 - yCentered / 2.0;

            return new MapPoint01(mapX, mapY);
        }
    }
}