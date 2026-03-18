using AstroSim.Core.Bodies;
using AstroSim.Core.Geometry;
using AstroSim.Time.Astro;   // <-- NEU

namespace AstroSim.Ephemerides.Interfaces
{
    public interface IVsopProvider
    {
        StateVector GetHeliocentricState(
            PlanetId planet,
            TTInstant time);   // <-- geändert
    }
}