using System.IO;
using System.Text.Json;
using Astronometria.Core.Bodies;
using Astronometria.Core.Time;
using Astronometria.Ephemerides.Planetary;
using Astronometria.Ephemerides.VSOP;
using Astronometria.Ephemerides.Test.Planetary.TestData;
using NUnit.Framework;
using Astronometria.Time.Astro;

namespace Astronometria.Ephemerides.Test.Planetary
{
    public class PlanetPositionServiceTests
    {
        [Test]
        public void Mars_Geocentric_Equatorial_J2000_Matches_Reference()
        {
            // Arrange
            var json = File.ReadAllText(
                "Planetary/TestData/Mars_Geocentric_2025.json");

            var reference = JsonSerializer.Deserialize<PlanetReference>(json);

            var time = new TTInstant(reference.EpochTT);

            var solutionRoot = SolutionPathResolver.GetSolutionRoot();

            var vsopPath = Path.Combine(
                solutionRoot,
                "src",
                "Astronometria.Ephemerides",
                "VSOP",
                "Data",
                "87A");

            var repo = new VsopRepository(vsopPath);

            var provider = new VsopProvider(repo);

            var service = new PlanetPositionService(provider);

            PlanetId planet =
                Enum.Parse<PlanetId>(reference.Planet);

            // Act
            var state =
                service.GetGeocentricEquatorialState(planet, time);

            // Assert
            Assert.That(state.Position.X,
                Is.EqualTo(reference.X).Within(5e-7));
            Assert.That(state.Position.Y,
                Is.EqualTo(reference.Y).Within(5e-7));
            Assert.That(state.Position.Z,
                Is.EqualTo(reference.Z).Within(5e-7));
        }
    }
}
