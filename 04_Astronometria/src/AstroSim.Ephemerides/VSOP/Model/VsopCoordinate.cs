namespace AstroSim.Ephemerides.VSOP.Model
{
    /// <summary>
    /// Represents one coordinate (X,Y,Z or L,B,R).
    /// Contains up to 6 series (T^0 .. T^5).
    /// </summary>
    public sealed class VsopCoordinate
    {
        public VsopSeries[] Series { get; } = new VsopSeries[6];

        public VsopCoordinate()
        {
            for (int i = 0; i < 6; i++)
                Series[i] = new VsopSeries();
        }
    }
}
