using EphemerisRegression.Api;
using EphemerisRegression.Config;
using EphemerisRegression.Domain;
using EphemerisRegression.Export;
using EphemerisRegression.Infrastructure;
using EphemerisRegression.Parsing;
using EphemerisRegression.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EphemerisRegression.Runner
{
    public sealed class MeshDataGenerationRunner
    {
        private const int MaxStepsPerChunk = 2000;

        private readonly HorizonsApiClient _apiClient = new();
        private readonly MeshHorizonsApiRequestFactory _meshFactory;

        public MeshDataGenerationRunner()
        {
            var config = new HorizonsHelioLevel0Config();
            _meshFactory = new MeshHorizonsApiRequestFactory(config);
        }

        public async Task RunAllPlanetsAsync(TsDMeshRunConfig config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (config.Planets == null || config.Planets.Count == 0)
                throw new ArgumentException("Config.Planets must not be empty.", nameof(config));

            Console.WriteLine("TS-D Mesh automatic run started...");
            Console.WriteLine($"Planets: {string.Join(", ", config.Planets.Select(p => p.Name))}");
            Console.WriteLine();

            var solutionRoot = ProjectPathResolver.GetSolutionRoot();

            string rawDir = Path.Combine(
                solutionRoot,
                "EphemerisRegression",
                "Horizons",
                "Mesh",
                "Helio",
                "L0",
                "Raw");

            string jsonDir = Path.Combine(
                solutionRoot,
                "EphemerisRegression",
                "Horizons",
                "Mesh",
                "Helio",
                "L0",
                "Json");

            Directory.CreateDirectory(rawDir);
            Directory.CreateDirectory(jsonDir);

            var epochs = new[]
            {
                ("Core",     1600, 1, 1, 2400, 1, 1,  30.0),
                ("Extended",    0, 1, 1, 4000, 1, 1, 180.0),
                ("Extreme",  -4000, 1, 1, 8000, 1, 1, 730.0)
            };

            foreach (var planet in config.Planets)
            {
                foreach (var e in epochs)
                {
                    await RunPlanetEpochAsync(
                        planet,
                        epochName: e.Item1,
                        startY: e.Item2, startM: e.Item3, startD: e.Item4,
                        stopY: e.Item5, stopM: e.Item6, stopD: e.Item7,
                        stepDays: e.Item8,
                        rawDir: rawDir,
                        jsonDir: jsonDir,
                        overwrite: config.OverwriteExistingFiles);
                }
            }

            Console.WriteLine("TS-D Mesh automatic run complete.");
        }

        private async Task RunPlanetEpochAsync(
            PlanetInfo planet,
            string epochName,
            int startY, int startM, int startD,
            int stopY, int stopM, int stopD,
            double stepDays,
            string rawDir,
            string jsonDir,
            bool overwrite)
        {
            Console.WriteLine($"Planet: {planet.Name} | Epoch: {epochName}");

            double epochStart = ToJulianDayProlepticGregorian(startY, startM, startD);
            double epochStop = ToJulianDayProlepticGregorian(stopY, stopM, stopD);

            // --- CLIPPING ---
            double effectiveStart = Math.Max(epochStart, planet.MinJulianDay);
            double effectiveStop = Math.Min(epochStop, planet.MaxJulianDay);

            if (effectiveStart > effectiveStop)
            {
                Console.WriteLine("  No overlap with planet availability. Skipped.");
                Console.WriteLine();
                return;
            }

            var allVectors = new List<StateVector>();
            var allRequests = new List<(string Canonical, string Hash)>();

            int chunkIndex = 1;

            for (double chunkStartJD = effectiveStart;
                 chunkStartJD <= effectiveStop;
                 chunkStartJD += MaxStepsPerChunk * stepDays, chunkIndex++)
            {
                double chunkStopJD = chunkStartJD + (MaxStepsPerChunk - 1) * stepDays;
                if (chunkStopJD > effectiveStop)
                    chunkStopJD = effectiveStop;

                double lastPointJD = chunkStartJD +
                                     Math.Floor((chunkStopJD - chunkStartJD) / stepDays) * stepDays;

                var request = _meshFactory.Create(
                    planet.CommandCode,
                    chunkStartJD,
                    chunkStopJD,
                    stepDays);

                Console.WriteLine(
                    $"  Chunk {chunkIndex:00}: " +
                    $"{request.StartTime} -> {request.StopTime} | " +
                    $"JD {chunkStartJD:F1} -> {chunkStopJD:F1} | " +
                    $"LastJD={lastPointJD:F1} | STEP={stepDays}d");

                string rawContent = await _apiClient.ExecuteAsync(request);

                string rawFileName =
                    $"{planet.Name}_TS-D_L0_{epochName}_Chunk_{chunkIndex:00}.csv";

                string rawPath = Path.Combine(rawDir, rawFileName);

                await File.WriteAllTextAsync(rawPath, rawContent);

                var parser = new HorizonsVectorParser();
                var vectors = parser.Parse(rawContent).ToList();

                if (vectors.Count == 0)
                    throw new InvalidOperationException(
                        $"0 vectors: {planet.Name} {epochName} chunk {chunkIndex:00}. RAW={rawPath}");

                allVectors.AddRange(vectors);

                var parameters = request.ToParameterDictionary();
                var canonical = CanonicalRequestBuilder.Build(parameters);
                var hash = HashCalculator.ComputeSha256(canonical);

                allRequests.Add((canonical, hash));
            }

            string epochHash = HashCalculator.ComputeSha256(
                string.Join("|", allRequests.Select(r => r.Hash)));

            var metadata = new ReferenceMetadata
            {
                EpochHash = epochHash,
                Requests = allRequests.Select(r => new RequestInfo
                {
                    CanonicalRequest = r.Canonical,
                    RequestHash = r.Hash,
                    HorizonsUrl = ""
                }).ToList(),
                HorizonsUrl = "",
                EngineVersion = "1.1.0",
                CorrectionLevel = "L0",
                Mode = "HELIO",
                EphemType = "VECTORS",
                GeneratedAtUtc = DateTime.UtcNow
            };

            var reference = new ReferenceStateVector(
                planet.Name,
                "TS-D",
                epochName,
                "L0",
                allVectors,
                metadata);

            var writer = new JsonReferenceWriter(JsonOptionsFactory.Create());

            string jsonPath =
                Path.Combine(jsonDir, $"{planet.Name}_TS-D_L0_{epochName}.json");

            await writer.WriteAsync(jsonPath, reference);

            Console.WriteLine($"  JSON saved: {Path.GetFileName(jsonPath)} | vectors={allVectors.Count}");
            Console.WriteLine();
        }

        private static double ToJulianDayProlepticGregorian(int year, int month, int day)
        {
            int a = (14 - month) / 12;
            int y = year + 4800 - a;
            int m = month + 12 * a - 3;

            int jdn = day
                + (153 * m + 2) / 5
                + 365 * y
                + y / 4
                - y / 100
                + y / 400
                - 32045;

            return jdn - 0.5;
        }

        public sealed class TsDMeshRunConfig
        {
            public IReadOnlyList<PlanetInfo> Planets { get; init; } = PlanetCatalog.AllPlanets;

            public bool OverwriteExistingFiles { get; init; } = true;

            public static TsDMeshRunConfig OnlyEarth() =>
                new TsDMeshRunConfig
                {
                    Planets = new[] { PlanetCatalog.Earth }
                };
        }
    }
}