using Astronometria.Time.Astro;
using System;
using System.IO;
using Astronometria.Core.Bodies;
using Astronometria.Core.Time;
using Astronometria.Ephemerides.VSOP;
using NUnit.Framework;
using Astronometria.Ephemerides.Test;

namespace Astronometria.Ephemerides.Test.VSOP
{
    [TestFixture]
    public class Vsop87A_Helio_ReferenceTests
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
        public void Mars_Helio_J2000_JD2451545_Matches_IMCCE()
        {
            // JD2451545.0  (01/01/2000 12h TDB)
            var time = new TTInstant(2451545.0);

            var repo = new VsopRepository(_vsopPath);
            var provider = new VsopProvider(repo);

            var state = provider.GetHeliocentricState(
                PlanetId.Mars,
                time);

            Assert.That(state.Position.X,
                Is.EqualTo(1.3907159264).Within(1e-10));
            Assert.That(state.Position.Y,
                Is.EqualTo(-0.0134157043).Within(1e-10));
            Assert.That(state.Position.Z,
                Is.EqualTo(-0.0344677967).Within(1e-10));
        }
    }
}
