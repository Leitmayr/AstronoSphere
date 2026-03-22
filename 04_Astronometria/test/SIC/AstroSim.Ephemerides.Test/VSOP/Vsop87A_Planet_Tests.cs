
using AstroSim.Core.Time;
using AstroSim.Ephemerides.VSOP.Calculation;
using AstroSim.Ephemerides.VSOP.Parsing;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using AstroSim.Time.Astro;

namespace AstroSim.Ephemerides.Test.VSOP
{
    [TestFixture]
    public class Vsop87A_Position_Tests
    {
        private const double Tolerance = 1e-10;

        private static readonly string[] SupportedPlanets =
        {
            "mercury",
            "venus",
            "earth",
            "mars",
            "jupiter",
            "saturn",
            "uranus",
            "neptune"
        };

        private string GetPlanetFilePath(string planet)
        {
            var normalized = planet.ToLowerInvariant();

            if (!SupportedPlanets.Contains(normalized))
                throw new ArgumentException($"Unsupported planet: {planet}");

            var fullPath = Path.Combine(
                AppContext.BaseDirectory,
                "VSOP",
                "Data",
                "87A",
                $"VSOP87A_{normalized}.dat");

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException(
                    $"VSOP file not found: {fullPath}");
            }

            return fullPath;
        }

        [TestCaseSource(typeof(Vsop87ATestSource), nameof(Vsop87ATestSource.PositionCases))]
        public void Position_Should_Match_Reference(
            string planet,
            double jdTT,
            double refX,
            double refY,
            double refZ)
        {
            string filePath = GetPlanetFilePath(planet);

            var parsed = Vsop87Parser.Parse(filePath);
            var time = new TTInstant(jdTT);

            double T = time.JulianMillenniaSinceJ2000();
            var result = VsopCalculator.Compute(parsed, T);

            Assert.Multiple(() =>
            {
                Assert.That(result[0], Is.EqualTo(refX).Within(Tolerance));
                Assert.That(result[1], Is.EqualTo(refY).Within(Tolerance));
                Assert.That(result[2], Is.EqualTo(refZ).Within(Tolerance));
            });
        }
    }
}
