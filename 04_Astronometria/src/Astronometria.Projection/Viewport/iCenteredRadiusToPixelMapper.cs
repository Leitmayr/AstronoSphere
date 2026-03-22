namespace Astronometria.Projection.Viewport
{
    public interface ICenteredRadiusToPixelMapper
    {
        PixelPoint CenteredToPixel(CenteredRadiusPoint p, double centerXPx, double centerYPx, double radiusPx);
    }
}