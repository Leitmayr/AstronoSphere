using AstroSim.Core.Coordinates;
using AstroSim.Core.Site;
using AstroSim.Core.Time;

namespace AstroSim.Projection.Projections
{
    public interface IMapProjection<TCoord>
    {
        MapPoint01 Project(TCoord coord, ObservationSite site, AstroTimeUT time);
    }
}
