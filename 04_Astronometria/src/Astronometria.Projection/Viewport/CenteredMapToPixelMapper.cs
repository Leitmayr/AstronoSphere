using Astronometria.Core.Coordinates;

namespace Astronometria.Projection.Viewport
{
    /// <summary>
    /// MapPoint01: (0.5,0.5) = Zentrum.
    /// Y nach oben positiv in Map, Pixel-Y nach unten -> wir invertieren Y.
    /// </summary>
    public sealed class CenteredMapToPixelMapper : IMapToPixelMapper
    {
        public PixelPoint Map01ToPixel(MapPoint01 p, MapViewport vp)
        {
            double cx = vp.WidthPx / 2.0;
            double cy = vp.HeightPx / 2.0;

            // Map01 in [-0.5..+0.5] um Zentrum
            double mx = (p.X - 0.5) * vp.WidthPx;
            double my = (p.Y - 0.5) * vp.HeightPx;

            // Zoom + Pan, Y invertieren
            double x = cx + mx * vp.Scale + vp.PanXPx;
            double y = cy - my * vp.Scale + vp.PanYPx;

            return new PixelPoint(x, y);
        }
    }
}
