using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using NUnit.Framework;
using AstroSim.Core.Time;
using AstroSim.Core.Bodies;
using AstroSim.Ephemerides.VSOP;
using AstroSim.Ephemerides.Test.EphemerisValidation.Common;

namespace AstroSim.Ephemerides.Test.EphemerisValidation.GeocentricEcliptic
{
    [TestFixture]
    public class L0_TSB_GeoNodes_DeviationAnalysis_Tests
    {
        private string _vsopPath;

        [OneTimeSetUp]
        public void Setup()
        {
            var baseDir = TestContext.CurrentContext.TestDirectory;

            _vsopPath = Path.GetFullPath(
                Path.Combine(baseDir,
                    @"..\..\..\..\AstroSim.Ephemerides\VSOP\Data\87A"));
        }

        [Test]
        public void L0_TSB_GenerateDeviationStatistics()
        {
            var baseDir = TestContext.CurrentContext.TestDirectory;

            var dataPath = Path.Combine(baseDir,
                "EphemerisValidation", "TestData", "Geo", "TS-B");

            var repo = new VsopRepository(_vsopPath);
            var provider = new VsopProvider(repo);

            var results = new List<DeviationStatEntry>();

            foreach (var file in Directory.GetFiles(dataPath, "*.json", SearchOption.AllDirectories))
            {
                var json = File.ReadAllText(file);
                var reference = JsonSerializer.Deserialize<NodeReferenceModel>(json)!;


                var deltasX = new List<double>();
                var deltasY = new List<double>();
                var deltasZ = new List<double>();

                foreach (var vector in Expand(reference))
                {
                    var time = new TTInstant(vector.JulianDate);

                    var planetState = provider.GetHeliocentricState(
                        ParsePlanet(reference.Planet), time);

                    var earthState = provider.GetHeliocentricState(
                        PlanetId.Earth, time);

                    var geo = planetState.Position - earthState.Position;

                    deltasX.Add(geo.X - vector.X);
                    deltasY.Add(geo.Y - vector.Y);
                    deltasZ.Add(geo.Z - vector.Z);
                }

                results.Add(new DeviationStatEntry
                {
                    Planet = reference.Planet,
                    MaxX = deltasX.Max(d => Math.Abs(d)),
                    MaxY = deltasY.Max(d => Math.Abs(d)),
                    MaxZ = deltasZ.Max(d => Math.Abs(d)),
                    RmsX = Rms(deltasX),
                    RmsY = Rms(deltasY),
                    RmsZ = Rms(deltasZ)
                });
            }

            WriteCsv(results, baseDir);

            Assert.Pass("Geo Node TS-B deviation statistics generated.");
        }

        private static IEnumerable<VectorEntry> Expand(NodeReferenceModel reference)

        {
            yield return reference.Ascending.Before;
            yield return reference.Ascending.At;
            yield return reference.Ascending.After;

            yield return reference.Descending.Before;
            yield return reference.Descending.At;
            yield return reference.Descending.After;
        }

        private static double Rms(IEnumerable<double> values)
        {
            var arr = values.ToArray();
            return Math.Sqrt(arr.Sum(v => v * v) / arr.Length);
        }

        private static void WriteCsv(List<DeviationStatEntry> stats, string baseDir)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Planet,MaxX,MaxY,MaxZ,RmsX,RmsY,RmsZ");

            foreach (var s in stats)
            {
                sb.AppendLine($"{s.Planet},{s.MaxX},{s.MaxY},{s.MaxZ},{s.RmsX},{s.RmsY},{s.RmsZ}");
            }

            var path = Path.Combine(baseDir, "GeoNodes_TS_B_Deviation_Statistics.csv");
            File.WriteAllText(path, sb.ToString());
        }

        private static PlanetId ParsePlanet(string name)
        {
            return Enum.Parse<PlanetId>(name);
        }
    }

}
