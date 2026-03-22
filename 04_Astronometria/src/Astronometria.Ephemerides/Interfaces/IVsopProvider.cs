using Astronometria.Core.Bodies;
using Astronometria.Core.Geometry;
using Astronometria.Time.Astro;   // <-- NEU

namespace Astronometria.Ephemerides.Interfaces
{
    public interface IVsopProvider
    {
        StateVector GetHeliocentricState(
            PlanetId planet,
            TTInstant time);   // <-- geändert
    }
}