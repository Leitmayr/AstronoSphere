using Astronometria.Core.Coordinates;
using Astronometria.Core.Site;
using Astronometria.Core.Time;

namespace Astronometria.Projection.Transforms
{
    public interface ICoordinateTransform
    {
        HorizontalCoord EquToHor(EquatorialCoord eq, AstroTimeUT time, ObservationSite site);
        EclipticCoord EquToEcl(EquatorialCoord eq, AstroTimeUT time);
        EquatorialCoord EclToEqu(EclipticCoord ecl, AstroTimeUT time);
    }
}