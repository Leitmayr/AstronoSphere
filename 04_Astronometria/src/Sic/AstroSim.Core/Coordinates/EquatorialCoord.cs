namespace AstroSim.Core.Coordinates;

public readonly struct EquatorialCoord
{
    public double RAdeg { get; }   // [0..360)
    public double Decdeg { get; }  // [-90..+90]

    public EquatorialCoord(double raDeg, double decDeg)
    {
        RAdeg = raDeg;
        Decdeg = decDeg;
    }

    public override string ToString() => $"RA={RAdeg}°, Dec={Decdeg}°";
}
