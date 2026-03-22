using Astronometria.Core.Bodies;
using Astronometria.Core.Time;
using Astronometria.Ephemerides.Test.EphemerisValidation.Common;
using Astronometria.Ephemerides.VSOP;
using Astronometria.Core.Geometry;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Astronometria.Time.Astro;

namespace Astronometria.Ephemerides.Test.EphemerisValidation.GeocentricEcliptic
{
    [TestFixture]
    public class L0_TSB_GeoNodes_Tests
    {
        private string _vsopPath = string.Empty;

        [OneTimeSetUp]
        public void Setup()
        {
            var solutionRoot = SolutionPathResolver.GetSolutionRoot();

            _vsopPath = Path.Combine(
                solutionRoot,
                "src",
                "Astronometria.Ephemerides",
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
                "Astronometria.Ephemerides.Test",
                "EphemerisValidation",
                "TestData",
                "Geo",
                "TS-B");

            if (!Directory.Exists(path))
                return Enumerable.Empty<string>();

            return Directory.GetFiles(path, "*.json", SearchOption.TopDirectoryOnly);
        }

        [TestCaseSource(nameof(GetJsonFiles))]
        public void L0_TSB_Geo_Node_Z_Sign_Regression(string jsonFile)
        {
            var json = File.ReadAllText(jsonFile);
            var reference = JsonSerializer.Deserialize<NodeReferenceModel>(json)!;

            var planetId = Enum.Parse<PlanetId>(reference.Planet);

            var repo = new VsopRepository(_vsopPath);
            var provider = new VsopProvider(repo);

            var node = reference.Node;
            var tol = RegressionTolerances.GetGeoPositionTolerance(planetId);

            var before = Compute(provider, planetId, node.Before.JulianDate);
            var at = Compute(provider, planetId, node.At.JulianDate);
            var after = Compute(provider, planetId, node.After.JulianDate);

            Assert.Multiple(() =>
            {
                Assert.That(before.Z, Is.EqualTo(node.Before.Z).Within(tol));
                Assert.That(at.Z, Is.EqualTo(node.At.Z).Within(tol));
                Assert.That(after.Z, Is.EqualTo(node.After.Z).Within(tol));
            });
        }

        private static Vector3 Compute(
            VsopProvider provider,
            PlanetId planetId,
            double jd)
        {
            var time = new TTInstant(jd);

            var planetState = provider.GetHeliocentricState(planetId, time);
            var earthState = provider.GetHeliocentricState(PlanetId.Earth, time);

            return planetState.Position - earthState.Position;
        }
    }
}