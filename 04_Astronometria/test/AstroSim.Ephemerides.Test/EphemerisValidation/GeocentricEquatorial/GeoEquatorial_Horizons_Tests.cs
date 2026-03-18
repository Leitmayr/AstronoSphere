using AstroSim.Core.Bodies;
using AstroSim.Core.Time;
using AstroSim.Ephemerides;
using AstroSim.Ephemerides.Planetary;
using AstroSim.Ephemerides.Test.EphemerisValidation.Common;
using AstroSim.Ephemerides.VSOP;
using NUnit.Framework;
using System;
using System.IO;
using AstroSim.Ephemerides.Test;
using AstroSim.Time.Astro;

namespace AstroSim.Ephemerides.Test.EphemerisValidation.GeocentricEquatorial
{
    [TestFixture]
    public class GeoEquatorial_Horizons_Tests
    {
        private string _vsopPath;

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

        [Test]
        public void Mars_JD2451545_GeocentricEquatorial_JPL()
        {
            var time = new TTInstant(2451545.0);

            var repo = new VsopRepository(_vsopPath);
            var provider = new VsopProvider(repo);
            var service = new PlanetPositionService(provider);

            var state = service.GetGeocentricEquatorialState(
                PlanetId.Mars,
                time);


            var tol = RegressionTolerances.GetGeoPositionTolerance(PlanetId.Mars);

            Assert.That(state.Position.X, Is.EqualTo(1.567851021019061).Within(tol));
            Assert.That(state.Position.Y, Is.EqualTo(-0.886027304697501).Within(tol));
            Assert.That(state.Position.Z, Is.EqualTo(-0.4217030662836571).Within(tol));
        }


        [Test]
        public void Mars_JD2415020_GeocentricEquatorial_JPL()
        {
            var time = new TTInstant(2415020.0);

            var repo = new VsopRepository(_vsopPath);
            var provider = new VsopProvider(repo);
            var service = new PlanetPositionService(provider);

            var state = service.GetGeocentricEquatorialState(
                PlanetId.Mars,
                time);

            var tol = RegressionTolerances.GetGeoPositionTolerance(PlanetId.Mars);

            Assert.That(state.Position.X, Is.EqualTo(6.167401595495362E-01).Within(tol));
            Assert.That(state.Position.Y, Is.EqualTo(-2.113253066189190E+00).Within(tol));
            Assert.That(state.Position.Z, Is.EqualTo(-9.589107242171878E-01).Within(tol));
        }


        [Test]
        public void Mars_JD2378495_GeocentricEquatorial_JPL()
        {
            var time = new TTInstant(2378495.0);

            var repo = new VsopRepository(_vsopPath);
            var provider = new VsopProvider(repo);
            var service = new PlanetPositionService(provider);

            var state = service.GetGeocentricEquatorialState(
                PlanetId.Mars,
                time);

            var tol = RegressionTolerances.GetGeoPositionTolerance(PlanetId.Mars);

            Assert.That(state.Position.X, Is.EqualTo(-9.125308437184837E-01).Within(tol));
            Assert.That(state.Position.Y, Is.EqualTo(-1.890995250777256E+00).Within(tol));
            Assert.That(state.Position.Z, Is.EqualTo(-8.149526531513910E-01).Within(tol));
        }


        [Test]
        public void Venus_JD2451545_GeocentricEquatorial_JPL()
        {
            var time = new TTInstant(2451545.0);

            var repo = new VsopRepository(_vsopPath);
            var provider = new VsopProvider(repo);
            var service = new PlanetPositionService(provider);

            var state = service.GetGeocentricEquatorialState(
                PlanetId.Venus,
                time);

            var tol = RegressionTolerances.GetGeoPositionTolerance(PlanetId.Venus);

            Assert.That(state.Position.X, Is.EqualTo(-5.411671970726792E-01).Within(tol));
            Assert.That(state.Position.Y, Is.EqualTo(-9.337027690276324E-01).Within(tol));
            Assert.That(state.Position.Z, Is.EqualTo(-3.601022606327784E-01).Within(tol));
        }




        [Test]
        public void Venus_JD2415020_GeocentricEquatorial_JPL()
        {
            var time = new TTInstant(2415020.0);

            var repo = new VsopRepository(_vsopPath);
            var provider = new VsopProvider(repo);
            var service = new PlanetPositionService(provider);

            var state = service.GetGeocentricEquatorialState(
                PlanetId.Venus,
                time);

            var tol = RegressionTolerances.GetGeoPositionTolerance(PlanetId.Venus);

            Assert.That(state.Position.X, Is.EqualTo(8.854501841196145E-01).Within(tol));
            Assert.That(state.Position.Y, Is.EqualTo(-1.054818002026842E+00).Within(tol));
            Assert.That(state.Position.Z, Is.EqualTo(-5.044429066348386E-01).Within(tol));
        }


        [Test]
        public void Jupiter_JD2378495_GeocentricEquatorial_JPL()
        {
            var time = new TTInstant(2378495.0);

            var repo = new VsopRepository(_vsopPath);
            var provider = new VsopProvider(repo);
            var service = new PlanetPositionService(provider);

            var state = service.GetGeocentricEquatorialState(
                PlanetId.Jupiter,
                time);

            var tol = RegressionTolerances.GetGeoPositionTolerance(PlanetId.Jupiter);

            Assert.That(state.Position.X, Is.EqualTo(1.813467989135781E-01).Within(tol));
            Assert.That(state.Position.Y, Is.EqualTo(3.833107165698300E+00).Within(tol));
            Assert.That(state.Position.Z, Is.EqualTo(1.639537317296447E+00).Within(tol));
        }


        [Test]
        public void Jupiter_JD2451545_GeocentricEquatorial_JPL()
        {
            var time = new TTInstant(2451545.0);

            var repo = new VsopRepository(_vsopPath);
            var provider = new VsopProvider(repo);
            var service = new PlanetPositionService(provider);

            var state = service.GetGeocentricEquatorialState(
                PlanetId.Jupiter,
                time);

            var tol = RegressionTolerances.GetGeoPositionTolerance(PlanetId.Jupiter);

            Assert.That(state.Position.X, Is.EqualTo(4.178312534862136E+00).Within(tol));
            Assert.That(state.Position.Y, Is.EqualTo(1.849149906689260E+00).Within(tol));
            Assert.That(state.Position.Z, Is.EqualTo(6.907692464036385E-01).Within(tol));
        }
    }
}
