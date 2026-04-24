// ============================================================
// FILE: MeshGenRunner.cs
// STATUS: NEW (M2.1 One-Shot Mesh Import Runner)
// ============================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using AstronoData.Contracts.Domain;

namespace AstronoLab
{
    public enum MeshGenRunMode
    {
        Full,
        Gmss
    }

    /// <summary>
    /// PURPOSE
    /// One-shot M2.1 runner for converting MeshGenerator seed files
    /// from AstronoData/01_Seeds/Incoming into prepared experiment candidates.
    ///
    /// CONTEXT
    /// This runner implements:
    /// - SPEC_MeshGenerator_AstronoLab_SortSpec
    /// - SPEC_MeshGenerator_AstronoLab_MappingSpec
    /// - VAL_MeshGenRunner_M2.1 GMSS subset mode
    ///
    /// CONSTRAINTS
    /// - This runner is not part of the future AstronoLab productive workflow.
    /// - Processing order is derived from seed content, not file system order.
    /// - Catalog numbering starts at AS-000143 by specification.
    /// - GMSS mode filters only after full deterministic sorting and catalog assignment.
    /// - Core is copied 1:1 from the incoming seed.
    /// </summary>
    public static class MeshGenRunner
    {
        private const int FirstCatalogNumber = 143;
        private const string NotesSeparator = "<_|_>";

        private static readonly HashSet<string> GmssCatalogNumbers = new(StringComparer.OrdinalIgnoreCase)
        {
            "AS-000143",
            "AS-000203",
            "AS-000221",
            "AS-000254",
            "AS-000283",
            "AS-000340",
            "AS-000342",
            "AS-000363"
        };

        private static readonly string[] MeshOrder =
        {
            "MCRE",
            "MXT1",
            "MXT2",
            "MVH1",
            "MVH2",
            "MVH3"
        };

        private static readonly string[] PlanetOrder =
        {
            "Mercury",
            "Venus",
            "Earth",
            "Mars",
            "Jupiter",
            "Saturn",
            "Uranus",
            "Neptune"
        };

        public static void Run(string inputFolder, string outputFolder, MeshGenRunMode mode)
        {
            Console.WriteLine("AstronoLab MeshGenRunner");
            Console.WriteLine("------------------------");
            Console.WriteLine($"Mode  : {mode}");
            Console.WriteLine($"Input : {inputFolder}");
            Console.WriteLine($"Output: {outputFolder}");
            Console.WriteLine();

            Directory.CreateDirectory(outputFolder);

            var files = Directory.GetFiles(inputFolder, "Planet-*.json", SearchOption.TopDirectoryOnly);

            if (files.Length == 0)
            {
                Console.WriteLine("No MeshGenerator files found.");
                return;
            }

            var sortedInputs = files
                .Select(ReadMeshInput)
                .OrderBy(x => x.SortKey.MeshRank)
                .ThenBy(x => x.SortKey.EpochNumber)
                .ThenBy(x => x.SortKey.SubEpochNumber)
                .ThenBy(x => x.SortKey.PlanetRank)
                .ThenBy(x => x.SortKey.StartJD)
                .ThenBy(x => x.SortKey.StopJD)
                .ThenBy(x => x.ResultId, StringComparer.Ordinal)
                .ToList();

            var catalogedInputs = sortedInputs
                .Select((input, index) => new CatalogedMeshInput(
                    Input: input,
                    CatalogNumber: $"AS-{FirstCatalogNumber + index:000000}"))
                .ToList();

            var selectedInputs = mode == MeshGenRunMode.Gmss
                ? catalogedInputs.Where(x => GmssCatalogNumbers.Contains(x.CatalogNumber)).ToList()
                : catalogedInputs;

            Console.WriteLine($"Mesh files found       : {sortedInputs.Count}");
            Console.WriteLine($"First catalog no.      : AS-{FirstCatalogNumber:000000}");
            Console.WriteLine($"Selected output count  : {selectedInputs.Count}");
            Console.WriteLine();

            if (mode == MeshGenRunMode.Gmss)
            {
                ValidateGmssSelection(selectedInputs);
                Console.WriteLine("GMSS selection:");
                foreach (var item in selectedInputs)
                {
                    Console.WriteLine(
                        $"  {item.CatalogNumber} | {item.Input.MeshAbbreviation} | {item.Input.SubEpochLabel} | {item.Input.Target}");
                }

                Console.WriteLine();
            }

            foreach (var item in selectedInputs)
            {
                WritePreparedSeed(item.Input, item.CatalogNumber, outputFolder);
            }

            Console.WriteLine();
            Console.WriteLine($"Done. Prepared mesh experiment candidates: {selectedInputs.Count}");
        }

        private static void ValidateGmssSelection(List<CatalogedMeshInput> selectedInputs)
        {
            if (selectedInputs.Count != GmssCatalogNumbers.Count)
            {
                throw new Exception(
                    $"GMSS selection failed. Expected {GmssCatalogNumbers.Count} files, got {selectedInputs.Count}.");
            }

            var found = selectedInputs
                .Select(x => x.CatalogNumber)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var missing = GmssCatalogNumbers
                .Where(x => !found.Contains(x))
                .OrderBy(x => x, StringComparer.Ordinal)
                .ToList();

            if (missing.Count > 0)
            {
                throw new Exception("GMSS selection failed. Missing: " + string.Join(", ", missing));
            }
        }

        private static MeshInput ReadMeshInput(string filePath)
        {
            var json = File.ReadAllText(filePath);
            using var doc = JsonDocument.Parse(json);

            var generatedSeed = doc.RootElement
                .GetProperty("GeneratedSeeds")[0];

            var seed = generatedSeed.GetProperty("SeedCandidate");
            var seedOrigin = generatedSeed.GetProperty("SeedOrigin");

            var core = seed.GetProperty("Core");
            var time = core.GetProperty("Time");
            var observedObject = core.GetProperty("ObservedObject");
            var eventNode = seed.GetProperty("Event");

            var meshAbbreviation = eventNode.GetProperty("Description").GetString()
                ?? throw new Exception($"Missing Event.Description in {filePath}");

            var qualifier = eventNode.GetProperty("Qualifier").GetString()
                ?? throw new Exception($"Missing Event.Qualifier in {filePath}");

            var target = observedObject.GetProperty("Targets")[0].GetString()
                ?? throw new Exception($"Missing target in {filePath}");

            var startJD = time.GetProperty("StartJD").GetDouble();
            var stopJD = time.GetProperty("StopJD").GetDouble();

            var resultId = seedOrigin.GetProperty("ResultID").GetString()
                ?? throw new Exception($"Missing SeedOrigin.ResultID in {filePath}");

            var subEpoch = ParseSubEpoch(qualifier, filePath);

            var sortKey = BuildSortKey(
                filePath,
                meshAbbreviation,
                subEpoch,
                target,
                startJD,
                stopJD);

            return new MeshInput(
                FilePath: filePath,
                Core: core.Clone(),
                Event: eventNode.Clone(),
                Metadata: seed.GetProperty("Metadata").Clone(),
                InputNotes: seed.GetProperty("Notes").GetString() ?? string.Empty,
                ResultId: resultId,
                MeshAbbreviation: meshAbbreviation,
                Target: target,
                StartJD: startJD,
                StopJD: stopJD,
                SubEpochLabel: $"SubEpoch{subEpoch.EpochNumber}.{subEpoch.SubEpochNumber}",
                SortKey: sortKey);
        }

        private static MeshSortKey BuildSortKey(
            string filePath,
            string meshAbbreviation,
            SubEpochInfo subEpoch,
            string target,
            double startJD,
            double stopJD)
        {
            var meshRank = Array.IndexOf(MeshOrder, meshAbbreviation);
            if (meshRank < 0)
                throw new Exception($"Unknown mesh abbreviation '{meshAbbreviation}' in {filePath}");

            var planetRank = Array.IndexOf(PlanetOrder, target);
            if (planetRank < 0)
                throw new Exception($"Unknown planet '{target}' in {filePath}");

            return new MeshSortKey(
                MeshRank: meshRank,
                EpochNumber: subEpoch.EpochNumber,
                SubEpochNumber: subEpoch.SubEpochNumber,
                PlanetRank: planetRank,
                StartJD: startJD,
                StopJD: stopJD);
        }

        private static SubEpochInfo ParseSubEpoch(string qualifier, string filePath)
        {
            var match = Regex.Match(
                qualifier,
                @"^(Simulation|Validation)_SubEpoch(?<epoch>\d+)\.(?<sub>\d+)_(Core|Extended|Outer)$");

            if (!match.Success)
                throw new Exception($"Cannot parse SubEpoch from qualifier '{qualifier}' in {filePath}");

            return new SubEpochInfo(
                EpochNumber: int.Parse(match.Groups["epoch"].Value),
                SubEpochNumber: int.Parse(match.Groups["sub"].Value));
        }

        private static void WritePreparedSeed(
            MeshInput input,
            string catalogNumber,
            string outputFolder)
        {
            var experimentId = BuildExperimentId(input.Core);
            var category = MapCategory(input.MeshAbbreviation);

            var eventObj = new
            {
                Category = category,
                Qualifier = input.Event.GetProperty("Qualifier").GetString(),
                Description = input.Event.GetProperty("Description").GetString()
            };

            var notes = string.IsNullOrWhiteSpace(input.InputNotes)
                ? input.ResultId
                : $"{input.ResultId}{NotesSeparator}{input.InputNotes}";

            var experiment = new
            {
                SchemaVersion = "1.0",
                ExperimentID = experimentId,
                CatalogNumber = catalogNumber,
                CoreHash = "TO_BE_REPLACED",
                Core = JsonSerializer.Deserialize<object>(input.Core.GetRawText()),
                Event = eventObj,
                Metadata = JsonSerializer.Deserialize<object>(input.Metadata.GetRawText()),
                Notes = notes
            };

            var categoryAbbreviation = CategoryMapper.ToAbbreviation(category);

            var observedObject = input.Core.GetProperty("ObservedObject");
            var bodyClass = observedObject.GetProperty("BodyClass").GetString()?.ToUpperInvariant()
                ?? throw new Exception($"Missing BodyClass in {input.FilePath}");
            var target = observedObject.GetProperty("Targets")[0].GetString()?.ToUpperInvariant()
                ?? throw new Exception($"Missing Target in {input.FilePath}");

            var human = $"{bodyClass}-{target}-{categoryAbbreviation}";
            var fileName = $"{catalogNumber}__{human}__{experimentId}.json";
            var outputPath = Path.Combine(outputFolder, fileName);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.Never,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            var jsonOut = JsonSerializer.Serialize(experiment, options);
            var formatted = ConvertToTwoSpaceIndent(jsonOut);

            File.WriteAllText(outputPath, formatted, Encoding.UTF8);

            Console.WriteLine($"Created {outputPath}");
        }

        private static string BuildExperimentId(JsonElement core)
        {
            var time = core.GetProperty("Time");

            var startJD = Math.Floor(time.GetProperty("StartJD").GetDouble());
            var stopJD = Math.Floor(time.GetProperty("StopJD").GetDouble());
            var step = time.GetProperty("Step").GetString()
                ?? throw new Exception("Missing Core.Time.Step.");

            return $"HELIO-J2000-TDB-{startJD}-{stopJD}-{step}".ToUpperInvariant();
        }

        private static string MapCategory(string meshAbbreviation)
        {
            return meshAbbreviation switch
            {
                "MCRE" => "Mesh Simulation Core",
                "MXT1" => "Mesh Simulation Extended",
                "MXT2" => "Mesh Simulation Outer",
                "MVH1" => "Mesh Validation Horizons Core",
                "MVH2" => "Mesh Validation Horizons Extended",
                "MVH3" => "Mesh Validation Horizons Outer",
                _ => throw new Exception($"Unknown mesh abbreviation: {meshAbbreviation}")
            };
        }

        private static string ConvertToTwoSpaceIndent(string input)
        {
            var lines = input.Split('\n');
            var indent = 0;
            var result = new List<string>();

            foreach (var raw in lines)
            {
                var line = raw.Trim();

                if (line.StartsWith("}") || line.StartsWith("]"))
                    indent--;

                result.Add(new string(' ', indent * 2) + line);

                if (line.EndsWith("{") || line.EndsWith("["))
                    indent++;
            }

            return string.Join("\r\n", result);
        }

        private sealed record CatalogedMeshInput(
            MeshInput Input,
            string CatalogNumber);

        private sealed record MeshInput(
            string FilePath,
            JsonElement Core,
            JsonElement Event,
            JsonElement Metadata,
            string InputNotes,
            string ResultId,
            string MeshAbbreviation,
            string Target,
            double StartJD,
            double StopJD,
            string SubEpochLabel,
            MeshSortKey SortKey);

        private sealed record MeshSortKey(
            int MeshRank,
            int EpochNumber,
            int SubEpochNumber,
            int PlanetRank,
            double StartJD,
            double StopJD);

        private sealed record SubEpochInfo(
            int EpochNumber,
            int SubEpochNumber);
    }
}