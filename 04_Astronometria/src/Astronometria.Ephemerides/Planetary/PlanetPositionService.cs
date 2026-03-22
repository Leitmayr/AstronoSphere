using Astronometria.Core.Bodies;
using Astronometria.Core.Geometry;
using Astronometria.Ephemerides.Interfaces;
using Astronometria.Ephemerides.Transformations;
using Astronometria.Ephemerides.VSOP.Model;
using Astronometria.Time.Astro;   // <-- NEU

namespace Astronometria.Ephemerides.Planetary
{
    /// <summary>
    /// Provides geocentric planetary state vectors
    /// in equatorial J2000 reference frame.
    /// </summary>
    public sealed class PlanetPositionService
    {
        private readonly IVsopProvider _vsopProvider;

        public PlanetPositionService(IVsopProvider vsopProvider)
        {
            _vsopProvider = vsopProvider;
        }

        /// <summary>
        /// Returns geocentric equatorial J2000 state vector.
        /// </summary>
        public StateVector GetGeocentricEquatorialState(
            PlanetId planet,
            TTInstant time)   // <-- geändert
        {
            // Heliocentric planet
            StateVector helioPlanet =
                _vsopProvider.GetHeliocentricState(planet, time);

            // Heliocentric Earth
            StateVector helioEarth =
                _vsopProvider.GetHeliocentricState(PlanetId.Earth, time);

            // Geocentric ecliptic
            Vector3 geoPosEcl =
                helioPlanet.Position - helioEarth.Position;

            Vector3 geoVelEcl =
                helioPlanet.Velocity - helioEarth.Velocity;

            // Rotate to equatorial J2000
            Vector3 geoPosEqu =
                CoordinateTransform.EclipticToEquatorial(geoPosEcl);

            Vector3 geoVelEqu =
                CoordinateTransform.EclipticToEquatorial(geoVelEcl);

            return new StateVector(geoPosEqu, geoVelEqu);
        }
    }
}