
using AstroSim.Core.Coordinates;

namespace AstroSim.Projection.Viewport
{
    public interface IMapToPixelMapper
    {
        PixelPoint Map01ToPixel(MapPoint01 p, MapViewport vp);
    }
}
