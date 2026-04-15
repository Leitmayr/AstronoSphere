// ============================================================
// FILE: StateVector.cs
// STATUS: FIX (M1.9 + JD RAW SUPPORT)
// ============================================================

namespace EphemerisRegression.Domain
{
    public sealed record StateVector(
        double JulianDate,
        double X,
        double Y,
        double Z,
        double VX,
        double VY,
        double VZ,
        string JulianDateRaw
    );
}