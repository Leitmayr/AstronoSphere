using EphemerisRegression.Domain;
using EphemerisRegression.Infrastructure;
using EphemerisRegression.Parsing;
using EphemerisRegression.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EphemerisRegression.Export
{
    public sealed class MeshDataL0JsonExportRunner
    {
        public async Task RunAsync(
            string planetName,
            string epochName,
            IEnumerable<string> rawChunkPaths,
            IEnumerable<MeshRequestInfo> requestInfos,
            string engineVersion = "1.1.0")
        {
            if (rawChunkPaths == null) throw new ArgumentNullException(nameof(rawChunkPaths));
            if (requestInfos == null) throw new ArgumentNullException(nameof(requestInfos));

            var chunkList = rawChunkPaths.Where(p => !string.IsNullOrWhiteSpace(p)).ToList();
            var infoList = requestInfos.ToList();

            var solutionRoot = ProjectPathResolver.GetSolutionRoot();

            string jsonDir = Path.Combine(
                solutionRoot,
                "EphemerisRegression",
                "Horizons",
                "Mesh",
                "Helio",
                "L0",
                "Json");

            Directory.CreateDirectory(jsonDir);

            var allVectors = new List<StateVector>();

            if (chunkList.Count > 0)
            {
                if (chunkList.Count != infoList.Count)
                    throw new InvalidOperationException(
                        $"rawChunkPaths ({chunkList.Count}) and requestInfos ({infoList.Count}) must have same length.");

                var parser = new HorizonsVectorParser();

                foreach (var rawPath in chunkList)
                {
                    if (!File.Exists(rawPath))
                        throw new FileNotFoundException($"RAW file not found: {rawPath}");

                    var rawContent = await File.ReadAllTextAsync(rawPath);
                    var vectors = parser.Parse(rawContent);

                    allVectors.AddRange(vectors);
                }
            }

            string epochHash = HashCalculator.ComputeSha256(
                string.Join("\n", infoList.Select(i => i.RequestHash)));

            var jsonModel = new MeshEpochReferenceJson
            {
                Planet = planetName,
                TestSuite = "TS-D",
                Epoch = epochName,
                CorrectionLevel = "L0",
                Mode = "HELIO",
                Metadata = new MeshEpochMetadata
                {
                    EngineVersion = engineVersion,
                    GeneratedAtUtc = DateTime.UtcNow,
                    EpochHash = epochHash,
                    Requests = infoList
                },
                StateVectors = allVectors
            };

            string fileName = $"{planetName}_TS-D_L0_{epochName}.json";
            string jsonPath = Path.Combine(jsonDir, fileName);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(jsonModel, options);
            await File.WriteAllTextAsync(jsonPath, json);

            Console.WriteLine($"Saved JSON: {Path.GetFileName(jsonPath)}");
        }

        private sealed class MeshEpochReferenceJson
        {
            public string Planet { get; set; } = "";
            public string TestSuite { get; set; } = "";
            public string Epoch { get; set; } = "";
            public string CorrectionLevel { get; set; } = "";
            public string Mode { get; set; } = "";
            public MeshEpochMetadata Metadata { get; set; } = new();
            public List<StateVector> StateVectors { get; set; } = new();
        }

        private sealed class MeshEpochMetadata
        {
            public string EngineVersion { get; set; } = "";
            public DateTime GeneratedAtUtc { get; set; }
            public string EpochHash { get; set; } = "";
            public List<MeshRequestInfo> Requests { get; set; } = new();
        }
    }

    public sealed class MeshRequestInfo
    {
        public string CanonicalRequest { get; set; } = "";
        public string RequestHash { get; set; } = "";
        public string HorizonsUrl { get; set; } = "";
    }
}