using AstroSim.Core.Bodies;
using AstroSim.Core.Geometry;
using AstroSim.Ephemerides.Interfaces;
using AstroSim.Ephemerides.VSOP.Calculation;
using AstroSim.Ephemerides.VSOP.Model;
using AstroSim.Time.Astro;   // <-- NEU

namespace AstroSim.Ephemerides.VSOP
{
    /// <summary>
    /// VSOP87A-based heliocentric provider.
    /// Returns positions in ecliptic J2000 frame.
    /// Velocity currently set to zero (M1).
    /// </summary>
    public sealed class VsopProvider : IVsopProvider
    {
        private readonly VsopRepository _repository;

        public VsopProvider(VsopRepository repository)
        {
            _repository = repository;
        }

        public StateVector GetHeliocentricState(
            PlanetId planet,
            TTInstant time)   // <-- geändert
        {
            var vsopPlanet = _repository.GetPlanet(planet);

            double T = time.JulianMillenniaSinceJ2000();
            double[] xyz = VsopCalculator.Compute(vsopPlanet, T);

            var position = new Vector3(xyz[0], xyz[1], xyz[2]);

            return new StateVector(position, Vector3.Zero);
        }
    }
}