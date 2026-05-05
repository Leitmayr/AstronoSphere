using Astronometria.Core.Bodies;
using Astronometria.Core.Geometry;
using Astronometria.Ephemerides.Interfaces;
using Astronometria.Ephemerides.Transformations;
using Astronometria.Ephemerides.VSOP.Model;
using Astronometria.Time.Astro;

namespace Astronometria.Ephemerides.Planetary
{
    /// <summary>
    /// Provides planetary state vectors derived from VSOP heliocentric states.
    /// </summary>
    public sealed class PlanetPositionService
    {
        private readonly IVsopProvider _vsopProvider;

        public PlanetPositionService(IVsopProvider vsopProvider)
        {
            _vsopProvider = vsopProvider;
        }

        /// <summary>
        /// Returns geocentric ecliptic J2000 state vector.
        /// </summary>
        public StateVector GetGeocentricEclipticState(
            PlanetId planet,
            TTInstant time)
        {
            StateVector helioPlanet =
                _vsopProvider.GetHeliocentricState(planet, time);

            StateVector helioEarth =
                _vsopProvider.GetHeliocentricState(PlanetId.Earth, time);

            Vector3 geoPosEcl =
                helioPlanet.Position - helioEarth.Position;

            Vector3 geoVelEcl =
                helioPlanet.Velocity - helioEarth.Velocity;

            return new StateVector(geoPosEcl, geoVelEcl);
        }

        /// <summary>
        /// Returns geocentric equatorial J2000 state vector.
        /// </summary>
        public StateVector GetGeocentricEquatorialState(
            PlanetId planet,
            TTInstant time)
        {
            StateVector geoEcl =
                GetGeocentricEclipticState(planet, time);

            Vector3 geoPosEqu =
                CoordinateTransform.EclipticToEquatorial(geoEcl.Position);

            Vector3 geoVelEqu =
                CoordinateTransform.EclipticToEquatorial(geoEcl.Velocity);

            return new StateVector(geoPosEqu, geoVelEqu);
        }
    }
}