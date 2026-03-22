namespace Astronometria.Projection.Viewport
{
    public sealed class MapViewport
    {
        public double WidthPx { get; set; }
        public double HeightPx { get; set; }

        // Zoom & Pan (in Pixeln). Default: kein Zoom, kein Pan
        public double Scale { get; set; } = 1.0;
        public double PanXPx { get; set; } = 0.0;
        public double PanYPx { get; set; } = 0.0;
    }
}
