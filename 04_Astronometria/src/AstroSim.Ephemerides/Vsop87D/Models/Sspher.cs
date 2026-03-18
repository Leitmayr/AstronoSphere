namespace AstroSim.Ephemerides.Vsop87D.Models
{
    public readonly struct Sspher
    {
        public double Ldeg { get; }
        public double Bdeg { get; }
        public double R_au { get; }

        public Sspher(double ldeg, double bdeg, double r_au)
        {
            Ldeg = ldeg;
            Bdeg = bdeg;
            R_au = r_au;
        }
    }
}
