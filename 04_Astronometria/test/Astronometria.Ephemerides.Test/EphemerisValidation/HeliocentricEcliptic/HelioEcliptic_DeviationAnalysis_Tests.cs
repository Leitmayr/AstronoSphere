using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using NUnit.Framework;
using Astronometria.Core.Time;
using Astronometria.Core.Bodies;
using Astronometria.Ephemerides.VSOP;
using Astronometria.Ephemerides.Test.EphemerisValidation.Common;
using Astronometria.Ephemerides.Test;
using Astronometria.Time.Astro;

namespace Astronometria.Ephemerides.Test.EphemerisValidation.HeliocentricEcliptic
{
    [TestFixture]
    public class HelioEcliptic_DeviationAnalysis_Tests
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
        public void L0_TSA_GenerateHelioDeviationStatistics()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var dataPath = Path.Combine(baseDir,
                "EphemerisValidation", "TestData", "Helio");

            var repo = new VsopRepository(_vsopPath);
            var provider = new VsopProvider(repo);

            var results = new List<DeviationStatEntry>();

            foreach (var file in Directory.GetFiles(dataPath, "*.json", SearchOption.AllDirectories))
            {
                var json = File.ReadAllText(file);
                var reference = JsonSerializer.Deserialize<ReferenceData>(json)!;

                var deltasX = new List<double>();
                var deltasY = new List<double>();
                var deltasZ = new List<double>();

                foreach (var vector in reference.Vectors)
                {
                    var time = new TTInstant(vector.JulianDate);

                    var state = provider.GetHeliocentricState(
                        ParsePlanet(reference.Planet), time);

                    deltasX.Add(state.Position.X - vector.X);
                    deltasY.Add(state.Position.Y - vector.Y);
                    deltasZ.Add(state.Position.Z - vector.Z);
                }

                results.Add(new DeviationStatEntry
                {
                    Planet = reference.Planet,
                    EventType = reference.Event,
                    MaxX = deltasX.Max(d => Math.Abs(d)),
                    MaxY = deltasY.Max(d => Math.Abs(d)),
                    MaxZ = deltasZ.Max(d => Math.Abs(d)),
                    RmsX = Rms(deltasX),
                    RmsY = Rms(deltasY),
                    RmsZ = Rms(deltasZ)
                });
            }

            WriteCsv(results, baseDir);

            Assert.Pass("Helio deviation statistics generated.");
        }

        private static double Rms(IEnumerable<double> values)
        {
            var arr = values.ToArray();
            return Math.Sqrt(arr.Sum(v => v * v) / arr.Length);
        }

        private static void WriteCsv(List<DeviationStatEntry> stats, string baseDir)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Planet,Event,MaxX,MaxY,MaxZ,RmsX,RmsY,RmsZ");

            foreach (var s in stats)
            {
                sb.AppendLine($"{s.Planet},{s.EventType},{s.MaxX},{s.MaxY},{s.MaxZ},{s.RmsX},{s.RmsY},{s.RmsZ}");
            }

            var path = Path.Combine(baseDir, "Helio_Deviation_Statistics.csv");
            File.WriteAllText(path, sb.ToString());
        }

        private static PlanetId ParsePlanet(string name)
        {
            return Enum.Parse<PlanetId>(name);
        }
    }


}
