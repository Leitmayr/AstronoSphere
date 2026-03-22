using Astronometria.Core.Bodies;
using Astronometria.Core.Time;
using Astronometria.Ephemerides;
using Astronometria.Ephemerides.Planetary;
using Astronometria.Ephemerides.Test.EphemerisValidation.Common;
using Astronometria.Ephemerides.VSOP;
using NUnit.Framework;
using System;
using System.IO;
using Astronometria.Ephemerides.Test;
using Astronometria.Time.Astro;

namespace Astronometria.Ephemerides.Test.EphemerisValidation.Robustness
{
    [TestFixture]
    public class Quadrant_RATests
    {
        private string _vsopPath;

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

        [Test]
        public void Mars_RANormalized_Example()
        {
            var time = new TTInstant(2451600.0);

            var repo = new VsopRepository(_vsopPath);
            var provider = new VsopProvider(repo);
            var service = new PlanetPositionService(provider);

            var state = service.GetGeocentricEquatorialState(
                PlanetId.Mars,
                time);

            double ra = Math.Atan2(state.Position.Y, state.Position.X) * 180.0 / Math.PI;
            if (ra < 0) ra += 360.0;


            var tol = RegressionTolerances.GetGeoPositionTolerance(PlanetId.Mars);


            Assert.That(state.Position.X, Is.EqualTo(2.11215845998386).Within(tol));
            Assert.That(state.Position.Y, Is.EqualTo(0.354650864026278).Within(tol));
            Assert.That(state.Position.Z, Is.EqualTo(0.139252155827254).Within(tol));
        }


        [Test]
        public void Venus_RANormalized_Example()
        {
            var time = new TTInstant(2451990.0);

            var repo = new VsopRepository(_vsopPath);
            var provider = new VsopProvider(repo);
            var service = new PlanetPositionService(provider);

            var state = service.GetGeocentricEquatorialState(
                PlanetId.Venus,
                time);

            double ra = Math.Atan2(state.Position.Y, state.Position.X) * 180.0 / Math.PI;
            if (ra < 0) ra += 360.0;

            var tol = RegressionTolerances.GetGeoPositionTolerance(PlanetId.Venus);

            Assert.That(state.Position.X, Is.EqualTo(0.279852884703374).Within(tol));
            Assert.That(state.Position.Y, Is.EqualTo(0.0499416980029664).Within(tol));
            Assert.That(state.Position.Z, Is.EqualTo(0.0675576899129624).Within(tol));
        }



    }
}
