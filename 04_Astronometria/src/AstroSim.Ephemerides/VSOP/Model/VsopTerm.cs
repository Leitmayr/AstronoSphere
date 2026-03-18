namespace AstroSim.Ephemerides.VSOP.Model
{
    /// <summary>
    /// Represents one VSOP term:
    /// A * cos(B + C * T)
    /// </summary>
    public readonly struct VsopTerm
    {
        public readonly double A;
        public readonly double B;
        public readonly double C;

        public VsopTerm(double a, double b, double c)
        {
            A = a;
            B = b;
            C = c;
        }
    }
}