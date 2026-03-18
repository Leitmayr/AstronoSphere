namespace AstroSim.Projection.Viewport
{
    public sealed class CenteredRadiusToPixelMapper : ICenteredRadiusToPixelMapper
    {
        public PixelPoint CenteredToPixel(CenteredRadiusPoint p, double centerXPx, double centerYPx, double radiusPx)
        {
            double x = centerXPx + radiusPx * p.X;
            double y = centerYPx + radiusPx * p.Y;
            return new PixelPoint(x, y);
        }
    }
}
