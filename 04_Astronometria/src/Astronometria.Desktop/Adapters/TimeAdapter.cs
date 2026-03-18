using AstroSim.Core.Time;

namespace Astronometria.Adapters
{
    public static class TimeAdapter
    {
        public static czeit ToCzeit(AstroTimeUT t)
        {
            // Platzhalter – hier deine echte Konvertierung einsetzen
            // z.B. new czeit(t.JulianDay) oder czeit.FromJulianDay(t.JulianDay) etc.
            return new czeit(t.JulianDay);
        }
    }
}
