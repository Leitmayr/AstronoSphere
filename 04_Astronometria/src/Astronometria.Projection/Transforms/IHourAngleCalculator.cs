using Astronometria.Core.Coordinates;
using Astronometria.Core.Site;
using Astronometria.Core.Time;

namespace Astronometria.Projection.Transforms
{
    /// <summary>
    /// Liefert Stundenwinkel in Stunden (0..24 bzw. auch negative Werte je nach Konvention).
    /// </summary>
    public interface IHourAngleCalculator
    {
        double GetHourAngleHours(AstroTimeUT time, EquatorialCoord eq, ObservationSite site);
    }
}