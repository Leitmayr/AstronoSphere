using System;
using System.IO;
using Astronometria.Core.Bodies;
using Astronometria.Core.Time;
using Astronometria.Ephemerides.VSOP;
using NUnit.Framework;
using Astronometria.Ephemerides.Test;
using Astronometria.Time.Astro;

namespace Astronometria.Ephemerides.Test.EphemerisValidation.GeocentricEcliptic
{
    [TestFixture]
    public class GeoEcliptic_Tests
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
        public void Mars_JD2451545_GeocentricEcliptic()
        {
            var time = new TTInstant(2451545.0);

            var repo = new VsopRepository(_vsopPath);
            var provider = new VsopProvider(repo);

            var mars = provider.GetHeliocentricState(PlanetId.Mars, time);
            var earth = provider.GetHeliocentricState(PlanetId.Earth, time);

            var geo = mars.Position - earth.Position;

            Assert.That(geo.X, Is.EqualTo(1.56785138502339).Within(1e-9));
            Assert.That(geo.Y, Is.EqualTo(-0.9806573279870529).Within(1e-9));
            Assert.That(geo.Z, Is.EqualTo(-0.034463896622636256).Within(1e-9));
        }


        [Test]
        public void Mars_JD2415020_GeocentricEcliptic()
        {
            var time = new TTInstant(2415020.0);

            var repo = new VsopRepository(_vsopPath);
            var provider = new VsopProvider(repo);

            var mars = provider.GetHeliocentricState(PlanetId.Mars, time);
            var earth = provider.GetHeliocentricState(PlanetId.Earth, time);

            var geo = mars.Position - earth.Position;

            Assert.That(geo.X, Is.EqualTo(0.616741212299999).Within(1e-9));
            Assert.That(geo.Y, Is.EqualTo(-2.3203043094).Within(1e-9));
            Assert.That(geo.Z, Is.EqualTo(-0.039180053).Within(1e-9));
        }

        [Test]
        public void Mars_JD2378495_GeocentricEcliptic()
        {
            var time = new TTInstant(2378495.0);

            var repo = new VsopRepository(_vsopPath);
            var provider = new VsopProvider(repo);

            var mars = provider.GetHeliocentricState(PlanetId.Mars, time);
            var earth = provider.GetHeliocentricState(PlanetId.Earth, time);

            var geo = mars.Position - earth.Position;

            Assert.That(geo.X, Is.EqualTo(-0.912530161900001).Within(1e-9));
            Assert.That(geo.Y, Is.EqualTo(-2.05912373819999).Within(1e-9));
            Assert.That(geo.Z, Is.EqualTo(0.00449009049999999).Within(1e-9));
        }



        [Test]
        public void Jupiter_JD2378495_GeocentricEcliptic()
        {
            var time = new TTInstant(2378495.0);

            var repo = new VsopRepository(_vsopPath);
            var provider = new VsopProvider(repo);

            var jupiter = provider.GetHeliocentricState(PlanetId.Jupiter, time);
            var earth = provider.GetHeliocentricState(PlanetId.Earth, time);

            var geo = jupiter.Position - earth.Position;

            Assert.That(geo.X, Is.EqualTo(0.181352799799999).Within(1e-9));
            Assert.That(geo.Y, Is.EqualTo(4.1689774471).Within(1e-9));
            Assert.That(geo.Z, Is.EqualTo(-0.0204756092).Within(1e-9));
        }


        [Test]
        public void Jupiter_JD2451545_GeocentricEcliptic()
        {
            var time = new TTInstant(2451545.0);

            var repo = new VsopRepository(_vsopPath);
            var provider = new VsopProvider(repo);

            var jupiter = provider.GetHeliocentricState(PlanetId.Jupiter, time);
            var earth = provider.GetHeliocentricState(PlanetId.Earth, time);

            var geo = jupiter.Position - earth.Position;

            Assert.That(geo.X, Is.EqualTo(4.1783094854).Within(1e-9));
            Assert.That(geo.Y, Is.EqualTo(1.971339384000).Within(1e-9));
            Assert.That(geo.Z, Is.EqualTo(-0.101779850100).Within(1e-9));
        }


    }


}

