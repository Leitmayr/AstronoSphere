namespace EphemerisRegression.Domain
{
    public sealed record StateVector(
        double JulianDate,
        double X,
        double Y,
        double Z,
        double VX,
        double VY,
        double VZ);
}

