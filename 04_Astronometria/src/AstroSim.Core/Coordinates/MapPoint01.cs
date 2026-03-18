namespace AstroSim.Core.Coordinates;

/// <summary>
/// Kartenkoordinate normiert auf [0..1] in X und Y.
/// </summary>
public readonly struct MapPoint01
{
    public double X { get; } // [0..1]
    public double Y { get; } // [0..1]

    public MapPoint01(double x, double y)
    {
        X = x;
        Y = y;
    }

    public override string ToString() => $"({X}, {Y})";
}