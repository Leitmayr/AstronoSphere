using System;
using System.Collections.Generic;
using AstronoData.Contracts.Domain;
using Astronometria.Core.Bodies;
using Astronometria.Ephemerides.Planetary;
using Astronometria.Ephemerides.Test.EphemerisValidation.Common;
using Astronometria.Ephemerides.Test.EphemerisValidation.HorizonsValidation.Common;
using Astronometria.Ephemerides.VSOP;
using Astronometria.Time.Astro;
using NUnit.Framework;

namespace Astronometria.Ephemerides.Test.EphemerisValidation.HorizonsValidation.Mesh
{
    [TestFixture]
    [Category("M2.2")]
    [Category("L0")]
    [Category("Mesh")]
    [Category("MeshTesting")]
    [Category("VSOP")]
    [Category("Horizons")]
    [Category("Position")]
    public sealed class L0_MeshTesting_Vsop_Horizons_Position_Tests
    {
        private static IEnumerable<TestCaseData> MVH1Cases()
        {
            for (var number = 274; number <= 338; number++)
            {
                var catalogNumber = $"AS-{number:000000}";
                yield return new TestCaseData(catalogNumber)
                    .SetName($"{catalogNumber} MVH1 L0 Position");
            }
        }

        private static IEnumerable<TestCaseData> MVH2Cases()
        {
            for (var number = 339; number <= 360; number++)
            {
                var catalogNumber = $"AS-{number:000000}";
                yield return new TestCaseData(catalogNumber)
                    .SetName($"{catalogNumber} MVH2 L0 Position");
            }
        }

        private static IEnumerable<TestCaseData> MVH3Cases()
        {
            for (var number = 361; number <= 374; number++)
            {
                var catalogNumber = $"AS-{number:000000}";
                yield return new TestCaseData(catalogNumber)
                    .SetName($"{catalogNumber} MVH3 L0 Position");
            }
        }

        [TestCaseSource(nameof(MVH1Cases))]
        [Category("MVH1")]
        public void MVH1_L0_Position_Matches_Horizons_Baseline(string catalogNumber)
        {
            RunMeshValidation(catalogNumber, MeshType.MVH1);
        }

        [TestCaseSource(nameof(MVH2Cases))]
        [Category("MVH2")]
        public void MVH2_L0_Position_Matches_Horizons_Baseline(string catalogNumber)
        {
            RunMeshValidation(catalogNumber, MeshType.MVH2);
        }

        [TestCaseSource(nameof(MVH3Cases))]
        [Category("MVH3")]
        public void MVH3_L0_Position_Matches_Horizons_Baseline(string catalogNumber)
        {
            RunMeshValidation(catalogNumber, MeshType.MVH3);
        }

        private static void RunMeshValidation(string catalogNumber, MeshType expectedMeshType)
        {
            Assert.That(
                ExperimentSetMapper.Map(catalogNumber),
                Is.EqualTo(ExperimentSet.Mesh),
                $"{catalogNumber} is not mapped to Mesh.");

            var testData = new HorizonsValidationTestData();

            var experiment = testData.ReadExperimentByCatalog(catalogNumber);
            var groundTruth = testData.ReadGroundTruthByCatalog(catalogNumber);

            var meshType = MeshTypeMapper.Map(experiment.Event.Description);

            Assert.That(
                meshType,
                Is.EqualTo(expectedMeshType),
                $"{catalogNumber} is not mapped to {expectedMeshType}.");

            Assert.That(
                MeshTypeMapper.IsHorizonsValidationMesh(meshType),
                Is.True,
                $"{catalogNumber} is not a Horizons validation mesh.");

            Assert.That(groundTruth.ExperimentRef.CatalogNumber, Is.EqualTo(experiment.CatalogNumber));
            Assert.That(groundTruth.ExperimentRef.ExperimentID, Is.EqualTo(experiment.ExperimentID));
            Assert.That(groundTruth.DatasetHeader.Measurement.Level, Is.EqualTo("L0"));
            Assert.That(groundTruth.DatasetHeader.Measurement.Type, Is.EqualTo("VEC"));
            Assert.That(groundTruth.Data, Is.Not.Empty);

            var planetId = HorizonsValidationTestData.GetSingleTargetPlanet(experiment);
            var frameType = experiment.Core.Frame.Type;

            var repo = new VsopRepository(testData.Vsop87APath);
            var provider = new VsopProvider(repo);
            var positionService = new PlanetPositionService(provider);

            var tolerance = GetPositionTolerance(frameType, planetId);

            foreach (var sample in groundTruth.Data)
            {
                var time = new TTInstant(sample.JD);
                var state = GetEngineState(frameType, planetId, time, provider, positionService);

                var dx = Math.Abs(state.Position.X - sample.Position.X);
                var dy = Math.Abs(state.Position.Y - sample.Position.Y);
                var dz = Math.Abs(state.Position.Z - sample.Position.Z);
                var dMax = Math.Max(dx, Math.Max(dy, dz));

                TestContext.WriteLine(
                    $"{catalogNumber} | {meshType} | {frameType} | JD={sample.JD} | " +
                    $"ΔX={dx:E3} ΔY={dy:E3} ΔZ={dz:E3} Δmax={dMax:E3} | tol={tolerance:E3}");

                Assert.Multiple(() =>
                {
                    Assert.That(
                        state.Position.X,
                        Is.EqualTo(sample.Position.X).Within(tolerance),
                        $"{catalogNumber} {meshType} {frameType} X mismatch at JD {sample.JD}");

                    Assert.That(
                        state.Position.Y,
                        Is.EqualTo(sample.Position.Y).Within(tolerance),
                        $"{catalogNumber} {meshType} {frameType} Y mismatch at JD {sample.JD}");

                    Assert.That(
                        state.Position.Z,
                        Is.EqualTo(sample.Position.Z).Within(tolerance),
                        $"{catalogNumber} {meshType} {frameType} Z mismatch at JD {sample.JD}");
                });
            }
        }

        private static double GetPositionTolerance(string frameType, PlanetId planetId)
        {
            return frameType switch
            {
                "HelioEcliptic" => RegressionTolerances.GetHelioPositionTolerance(planetId),
                "GeoEcliptic" => RegressionTolerances.GetGeoPositionTolerance(planetId),
                _ => throw new NotSupportedException($"Unsupported Mesh frame type: {frameType}")
            };
        }

        private static dynamic GetEngineState(
            string frameType,
            PlanetId planetId,
            TTInstant time,
            VsopProvider provider,
            PlanetPositionService positionService)
        {
            return frameType switch
            {
                "HelioEcliptic" => provider.GetHeliocentricState(planetId, time),
                "GeoEcliptic" => positionService.GetGeocentricEclipticState(planetId, time),
                _ => throw new NotSupportedException($"Unsupported Mesh frame type: {frameType}")
            };
        }
    }
}