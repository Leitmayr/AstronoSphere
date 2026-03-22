using NUnit.Framework;
using System;
using System.IO;
using Astronometria.Core.Time;
using Astronometria.Ephemerides.VSOP.Parsing;
using Astronometria.Ephemerides.VSOP.Calculation;
using Astronometria.Ephemerides.Test;
using Astronometria.Time.Astro;

namespace Astronometria.Ephemerides.Test.VSOP
{
    [TestFixture]
    public class Vsop87A_Earth_Tests
    {
        private const double Tolerance = 1e-10;

        private string GetEarthFilePath()
        {

            var solutionRoot = SolutionPathResolver.GetSolutionRoot();

            return Path.Combine(
                solutionRoot,
                "src",
                "Astronometria.Ephemerides",
                "VSOP",
                "Data",
                "87A",
                "VSOP87A_earth.dat");
        }

        [TestCase(2451545.0, -.1771354586, .9672416237, -.0000039000)]
        [TestCase(2415020.0, -.1883079649, .9650688844, .0002150325)]
        [TestCase(2378495.0, -.1993918002, .9627974368, .0004307602)]
        [TestCase(2341970.0, -.2104654652, .9603579954, .0006472929)]
        [TestCase(2305445.0, -.2214982928, .9578483181, .0008568250)]
        [TestCase(2268920.0, -.2324780153, .9551975793, .0010692878)]
        [TestCase(2232395.0, -.2435134343, .9524373311, .0012871020)]
        [TestCase(2195870.0, -.2544603371, .9495904257, .0014962103)]
        [TestCase(2159345.0, -.2654547156, .9465233602, .0017037737)]
        [TestCase(2122820.0, -.2763146784, .9433985307, .0019115387)]
        public void Earth_Position_Should_Match_Reference(
            double jdTT,
            double refX,
            double refY,
            double refZ)
        {
            // Arrange
            string filePath = GetEarthFilePath();
            Assert.That(File.Exists(filePath), $"VSOP data file not found: {filePath}");

            var earth = Vsop87Parser.Parse(filePath);
            var time = new TTInstant(jdTT);

            double T = time.JulianMillenniaSinceJ2000();

            // Act
            var result = VsopCalculator.Compute(earth, T);

            double X = result[0];
            double Y = result[1];
            double Z = result[2];

            TestContext.WriteLine($"JD = {jdTT}");
            TestContext.WriteLine($"X = {X:G17}");
            TestContext.WriteLine($"Y = {Y:G17}");
            TestContext.WriteLine($"Z = {Z:G17}");

            // Assert
            Assert.That(X, Is.EqualTo(refX).Within(Tolerance));
            Assert.That(Y, Is.EqualTo(refY).Within(Tolerance));
            Assert.That(Z, Is.EqualTo(refZ).Within(Tolerance));
        }
    }
}