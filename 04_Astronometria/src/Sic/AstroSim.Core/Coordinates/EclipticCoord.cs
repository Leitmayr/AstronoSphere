namespace AstroSim.Core.Coordinates;

public readonly struct EclipticCoord
{
    public double LambdaDeg { get; } // ekliptische Länge
    public double BetaDeg { get; }   // ekliptische Breite

    public EclipticCoord(double lambdaDeg, double betaDeg)
    {
        LambdaDeg = lambdaDeg;
        BetaDeg = betaDeg;
    }

    public override string ToString() => $"λ={LambdaDeg}°, β={BetaDeg}°";
}
