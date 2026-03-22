using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using NUnit.Framework;
using AstroSim.Core.Time;
using AstroSim.Core.Bodies;
using AstroSim.Ephemerides.VSOP;
using AstroSim.Ephemerides.Test.EphemerisValidation.Common;
using AstroSim.Time.Astro;

namespace AstroSim.Ephemerides.Test.EphemerisValidation.HeliocentricEcliptic
{
    [TestFixture]
    public class L0_TSA_HelioEclipticQuadrant_HorizonsRegression_Tests
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
                "Helio",
                "TS-A");

            if (!Directory.Exists(path))
                return Enumerable.Empty<string>();

            return Directory.GetFiles(path, "*.json", SearchOption.AllDirectories);
        }

        [TestCaseSource(nameof(GetJsonFiles)), Category("Position")]
        public void L0_TSA_Helio_Position_Regression(string jsonFile)
        {
            var json = File.ReadAllText(jsonFile);
            var reference = JsonSerializer.Deserialize<ReferenceData>(json)!;

            var repo = new VsopRepository(_vsopPath);
            var provider = new VsopProvider(repo);

            var planetId = Enum.Parse<PlanetId>(reference.Planet);
            var tol = RegressionTolerances.GetHelioPositionTolerance(planetId);

            foreach (var vector in reference.Vectors)
            {
                var time = new TTInstant(vector.JulianDate);
                var state = provider.GetHeliocentricState(planetId, time);

                Assert.That(state.Position.X, Is.EqualTo(vector.X).Within(tol));
                Assert.That(state.Position.Y, Is.EqualTo(vector.Y).Within(tol));
                Assert.That(state.Position.Z, Is.EqualTo(vector.Z).Within(tol));
            }
        }
    }
}