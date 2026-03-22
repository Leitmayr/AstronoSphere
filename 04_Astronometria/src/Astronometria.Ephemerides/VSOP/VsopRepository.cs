using System;
using System.Collections.Generic;
using System.IO;
using Astronometria.Core.Bodies;
using Astronometria.Ephemerides.VSOP.Model;
using Astronometria.Ephemerides.VSOP.Parsing;

namespace Astronometria.Ephemerides.VSOP
{
    /// <summary>
    /// Central repository holding all VSOP planetary data.
    /// Loads and stores parsed VSOP files.
    /// </summary>
    public sealed class VsopRepository
    {
        private readonly Dictionary<PlanetId, VsopPlanet> _planets;

        public VsopRepository(string dataDirectory)
        {
            _planets = new Dictionary<PlanetId, VsopPlanet>();

            LoadPlanet(dataDirectory, PlanetId.Mercury);
            LoadPlanet(dataDirectory, PlanetId.Venus);
            LoadPlanet(dataDirectory, PlanetId.Earth);
            LoadPlanet(dataDirectory, PlanetId.Mars);
            LoadPlanet(dataDirectory, PlanetId.Jupiter);
            LoadPlanet(dataDirectory, PlanetId.Saturn);
            LoadPlanet(dataDirectory, PlanetId.Uranus);
            LoadPlanet(dataDirectory, PlanetId.Neptune);
        }

        private void LoadPlanet(string dir, PlanetId id)
        {
            string fileName = $"VSOP87A_{id.ToString().ToLower()}.dat";

            string file = Path.Combine(dir, fileName);

            var planet = Vsop87Parser.Parse(file);

            _planets[id] = planet;
        }
        public VsopPlanet GetPlanet(PlanetId id)
        {
            return _planets[id];
        }
    }
}

