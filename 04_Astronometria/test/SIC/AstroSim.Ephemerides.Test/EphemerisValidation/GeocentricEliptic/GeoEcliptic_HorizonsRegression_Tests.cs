using AstroSim.Core.Bodies;
using AstroSim.Core.Time;
using AstroSim.Ephemerides.Test.EphemerisValidation.Common;
using AstroSim.Ephemerides.VSOP;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using AstroSim.Time.Astro;

namespace AstroSim.Ephemerides.Test.EphemerisValidation.GeocentricEcliptic
{
    [TestFixture]
    public class L0_TSB_GeoEcliptic_HorizonsRegression_Tests
    {
        private string _vsopPath = string.Empty;

        [OneTimeSetUp]
        public void Setup()
        {
            var solutionRoot = SolutionPathResolver.GetSolutionRoot();

            _vsopPath = Path.Combine(
                solutionRoot,
                "src",
                "AstroSim.Ephemerides",
                "VSOP",
                "Data",
                "87A");
        }

        public static IEnumerable<string> GetJsonFiles()
        {
            var solutionRoot = SolutionPathResolver.GetSolutionRoot();

            var path = Path.Combine(
                solutionRoot,
                "test",
                "AstroSim.Ephemerides.Test",
                "EphemerisValidation",
                "TestData",
                "Geo",
                "TS-A");

            if (!Directory.Exists(path))
                return Enumerable.Empty<string>();

            return Directory.GetFiles(path, "*.json", SearchOption.AllDirectories);
        }

        [TestCaseSource(nameof(GetJsonFiles)), Category("Position")]
        public void L0_TSA_Geo_Position_Regression(string jsonFile)
        {
            var json = File.ReadAllText(jsonFile);
            var reference = JsonSerializer.Deserialize<ReferenceData>(json)!;

            var repo = new VsopRepository(_vsopPath);
            var provider = new VsopProvider(repo);

            var planetId = Enum.Parse<PlanetId>(reference.Planet);
            var tol = RegressionTolerances.GetGeoPositionTolerance(planetId);

            foreach (var vector in reference.Vectors)
            {
                var time = new TTInstant(vector.JulianDate);

                var planetState = provider.GetHeliocentricState(planetId, time);
                var earthState = provider.GetHeliocentricState(PlanetId.Earth, time);

                var geoPosition = planetState.Position - earthState.Position;

                Assert.That(geoPosition.X, Is.EqualTo(vector.X).Within(tol));
                Assert.That(geoPosition.Y, Is.EqualTo(vector.Y).Within(tol));
                Assert.That(geoPosition.Z, Is.EqualTo(vector.Z).Within(tol));
            }
        }
    }
}