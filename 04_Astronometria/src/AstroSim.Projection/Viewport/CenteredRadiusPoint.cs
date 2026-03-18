namespace AstroSim.Projection.Viewport
{
    /// <summary>
    /// Normierte Kartenkoordinate relativ zum Kartenradius.
    /// X/Y typischerweise im Bereich [-1..+1] (oder [0..1], je nach Definition).
    /// </summary>
    public readonly struct CenteredRadiusPoint
    {
        public double X { get; }
        public double Y { get; }

        public CenteredRadiusPoint(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
