using AstroSim.Core.Coordinates;
using AstroSim.Core.Site;
using AstroSim.Core.Time;

namespace AstroSim.Projection.Transforms
{
    /// <summary>
    /// Liefert Stundenwinkel in Stunden (0..24 bzw. auch negative Werte je nach Konvention).
    /// </summary>
    public interface IHourAngleCalculator
    {
        double GetHourAngleHours(AstroTimeUT time, EquatorialCoord eq, ObservationSite site);
    }
}