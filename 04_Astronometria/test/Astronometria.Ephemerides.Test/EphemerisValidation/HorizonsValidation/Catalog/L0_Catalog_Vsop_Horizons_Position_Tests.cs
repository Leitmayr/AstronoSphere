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

namespace Astronometria.Ephemerides.Test.EphemerisValidation.HorizonsValidation.Catalog
{
    [TestFixture]
    [Category("M2.2")]
    [Category("L0")]
    [Category("Catalog")]
    [Category("VSOP")]
    [Category("Horizons")]
    [Category("Position")]
    public sealed class L0_Catalog_Vsop_Horizons_Position_Tests
    {
        private static IEnumerable<TestCaseData> CatalogCases()
        {
            for (var number = 13; number <= 72; number++)
            {
                // Deprecated / excluded catalog entries
                if (number is 20 or 33 or 34 or 48)
                    continue;

                // GeoEquatorial catalog entries:
                // GroundTruth currently generated with REF_PLANE=ECLIPTIC.
                // Deferred until GeoEquatorial GroundTruth is regenerated correctly.
                if (number >= 59 && number <= 72)
                    continue;

                var catalogNumber = $"AS-{number:000000}";
                yield return new TestCaseData(catalogNumber)
                    .SetName($"{catalogNumber} Catalog L0 Position");
            }

            yield return new TestCaseData("AS-000145")
                .SetName("AS-000145 Catalog L0 Position");
        }

        [TestCaseSource(nameof(CatalogCases))]
        public void Catalog_L0_Position_Matches_Horizons_Baseline(string catalogNumber)
        {
            Assert.That(
                ExperimentSetMapper.Map(catalogNumber),
                Is.EqualTo(ExperimentSet.Catalog),
                $"{catalogNumber} is not mapped to Catalog.");

            var testData = new HorizonsValidationTestData();

            var experiment = testData.ReadExperimentByCatalog(catalogNumber);
            var groundTruth = testData.ReadGroundTruthByCatalog(catalogNumber);

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
                    $"{catalogNumber} | Catalog | {frameType} | JD={sample.JD} | " +
                    $"ΔX={dx:E3} ΔY={dy:E3} ΔZ={dz:E3} Δmax={dMax:E3} | tol={tolerance:E3}");

                Assert.Multiple(() =>
                {
                    Assert.That(
                        state.Position.X,
                        Is.EqualTo(sample.Position.X).Within(tolerance),
                        $"{catalogNumber} Catalog {frameType} X mismatch at JD {sample.JD}");

                    Assert.That(
                        state.Position.Y,
                        Is.EqualTo(sample.Position.Y).Within(tolerance),
                        $"{catalogNumber} Catalog {frameType} Y mismatch at JD {sample.JD}");

                    Assert.That(
                        state.Position.Z,
                        Is.EqualTo(sample.Position.Z).Within(tolerance),
                        $"{catalogNumber} Catalog {frameType} Z mismatch at JD {sample.JD}");
                });
            }
        }

        private static double GetPositionTolerance(string frameType, PlanetId planetId)
        {
            return frameType switch
            {
                "HelioEcliptic" => RegressionTolerances.GetHelioPositionTolerance(planetId),
                "GeoEcliptic" => RegressionTolerances.GetGeoPositionTolerance(planetId),
                _ => throw new NotSupportedException($"Unsupported Catalog frame type: {frameType}")
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
                _ => throw new NotSupportedException($"Unsupported Catalog frame type: {frameType}")
            };
        }
    }
}