using AstroSim.Core.Coordinates;
using AstroSim.Core.Site;
using AstroSim.Core.Time;
using AstroSim.Projection.Transforms;

namespace Astronometria.Adapters
{
    public sealed class ObsSituationHourAngleCalculator : IHourAngleCalculator
    {
        public double GetHourAngleHours(AstroTimeUT time, EquatorialCoord eq, ObservationSite site)
        {
            // TODO: AstroTimeUT -> czeit
            // Du hast bestimmt eine Möglichkeit, aus deiner Zeitklasse einen Julian Day / UT abzuleiten.
            // Ich schreibe es bewusst als Platzhalter:
            czeit myTime = TimeAdapter.ToCzeit(time);

            // ObsSituation erwartet RA in Stunden (du hattest RA/15)
            var sit = new ObsSituation(myTime, eq.RAdeg / 15.0, eq.Decdeg, site.LongitudeDeg, site.LatitudeDeg);
            return sit.getHourAngle;
        }
    }
}