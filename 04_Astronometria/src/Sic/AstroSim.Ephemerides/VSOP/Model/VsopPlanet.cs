namespace AstroSim.Ephemerides.VSOP.Model
{
    /// <summary>
    /// Represents one VSOP planet.
    /// Always contains exactly 3 coordinates.
    /// </summary>
    public sealed class VsopPlanet
    {
        public VsopCoordinate[] Coordinates { get; } = new VsopCoordinate[3];

        public string Name { get; }

        public VsopPlanet(string name)
        {
            Name = name;

            for (int i = 0; i < 3; i++)
                Coordinates[i] = new VsopCoordinate();
        }
    }
}
