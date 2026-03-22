
using Astronometria.Core.Coordinates;

namespace Astronometria.Projection.Viewport
{
    public interface IMapToPixelMapper
    {
        PixelPoint Map01ToPixel(MapPoint01 p, MapViewport vp);
    }
}
