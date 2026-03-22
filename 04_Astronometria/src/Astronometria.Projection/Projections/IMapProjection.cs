using Astronometria.Core.Coordinates;
using Astronometria.Core.Site;
using Astronometria.Core.Time;

namespace Astronometria.Projection.Projections
{
    public interface IMapProjection<TCoord>
    {
        MapPoint01 Project(TCoord coord, ObservationSite site, AstroTimeUT time);
    }
}
