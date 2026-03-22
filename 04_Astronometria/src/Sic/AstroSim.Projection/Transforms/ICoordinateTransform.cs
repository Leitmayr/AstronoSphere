using AstroSim.Core.Coordinates;
using AstroSim.Core.Site;
using AstroSim.Core.Time;

namespace AstroSim.Projection.Transforms
{
    public interface ICoordinateTransform
    {
        HorizontalCoord EquToHor(EquatorialCoord eq, AstroTimeUT time, ObservationSite site);
        EclipticCoord EquToEcl(EquatorialCoord eq, AstroTimeUT time);
        EquatorialCoord EclToEqu(EclipticCoord ecl, AstroTimeUT time);
    }
}