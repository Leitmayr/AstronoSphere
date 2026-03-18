using AstroSim.Core.Bodies;
using AstroSim.Core.Time;
using AstroSim.Ephemerides.VSOP;
using NUnit.Framework;
using AstroSim.Ephemerides.Test;
using AstroSim.Time.Astro;


namespace AstroSim.Ephemerides.Test.VSOP
{
    [TestFixture]
    public class Geocentric_Ecliptic_Tests
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
        public void Mars_Geocentric_Ecliptic_J2000_JD2451545()
        {
            var time = new TTInstant(2451545.0);

            var repo = new VsopRepository(_vsopPath);
            var provider = new VsopProvider(repo);

            var mars = provider.GetHeliocentricState(
                PlanetId.Mars, time);

            var earth = provider.GetHeliocentricState(
                PlanetId.Earth, time);

            var geo = mars.Position - earth.Position;

            Assert.That(geo.X,
                Is.EqualTo(1.5678513850).Within(1e-10));
            Assert.That(geo.Y,
                Is.EqualTo(-0.9806573280).Within(1e-10));
            Assert.That(geo.Z,
                Is.EqualTo(-0.0344638967).Within(1e-10));
        }
    }
}

