namespace Astronometria.Core.Coordinates;


public readonly struct HorizontalCoord
{
    public double AzDeg { get; }   // [0..360)
    public double AltDeg { get; }  // [-90..+90]

    public HorizontalCoord(double azDeg, double altDeg)
    {
        AzDeg = azDeg;
        AltDeg = altDeg;
    }

    public override string ToString() => $"Az={AzDeg}°, Alt={AltDeg}°";
}
