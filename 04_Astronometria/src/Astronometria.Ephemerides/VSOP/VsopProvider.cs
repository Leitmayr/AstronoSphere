using Astronometria.Core.Bodies;
using Astronometria.Core.Geometry;
using Astronometria.Ephemerides.Interfaces;
using Astronometria.Ephemerides.VSOP.Calculation;
using Astronometria.Ephemerides.VSOP.Model;
using Astronometria.Time.Astro;   // <-- NEU

namespace Astronometria.Ephemerides.VSOP
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