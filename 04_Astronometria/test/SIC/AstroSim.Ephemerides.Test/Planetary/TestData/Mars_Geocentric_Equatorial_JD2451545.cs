using AstroSim.Core.Bodies;
using AstroSim.Core.Time;
using AstroSim.Ephemerides.Planetary;
using AstroSim.Ephemerides.VSOP;
using NUnit.Framework;
using AstroSim.Ephemerides.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AstroSim.Time.Astro;

namespace AstroSim.Ephemerides.Test.Planetary.TestData
{

    
        [TestFixture]
    public class Mars_Geocentric_Equatorial_JD2451545
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
            public void Mars_Geocentric_Equatorial_J2000_JD2451545()
            {
                var time = new TTInstant(2451545.0);

                var repo = new VsopRepository(_vsopPath);
                var provider = new VsopProvider(repo);

                var service = new PlanetPositionService(provider);

                var state = service.GetGeocentricEquatorialState(
                    PlanetId.Mars,
                    time);

            double raRad = Math.Atan2(state.Position.Y, state.Position.X);
            double raDeg = raRad * 180.0 / Math.PI;

            if (raDeg < 0)
                raDeg += 360.0;

            TestContext.WriteLine($"RA (deg) = {raDeg}");

            // Ausgabe im Testfenster
            TestContext.WriteLine($"X = {state.Position.X}");
            TestContext.WriteLine($"Y = {state.Position.Y}");
            TestContext.WriteLine($"Z = {state.Position.Z}");


            var marsHelio = provider.GetHeliocentricState(PlanetId.Mars, time);
            var earthHelio = provider.GetHeliocentricState(PlanetId.Earth, time);


            double lambdaRad = Math.Atan2(
                earthHelio.Position.Y,
                earthHelio.Position.X);

            double lambdaDeg = lambdaRad * 180.0 / Math.PI;

            if (lambdaDeg < 0)
                lambdaDeg += 360.0;

            TestContext.WriteLine($"Earth heliocentric ecliptic longitude (deg) = {lambdaDeg}");


            var geoEcl = marsHelio.Position - earthHelio.Position;

            TestContext.WriteLine($"ECL X = {geoEcl.X}");
            TestContext.WriteLine($"ECL Y = {geoEcl.Y}");
            TestContext.WriteLine($"ECL Z = {geoEcl.Z}");

            Assert.That(state.Position.X,
                    Is.EqualTo(1.567851021019061E+00).Within(1e-6));
                Assert.That(state.Position.Y,
                    Is.EqualTo(-8.860273046975010E-01).Within(1e-6));
                Assert.That(state.Position.Z,
                    Is.EqualTo(-4.217030662836571E-01).Within(1e-6));
            }


    }
}
