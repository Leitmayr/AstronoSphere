// ============================================================
// FILE: SeedToExperimentConverter.cs
// STATUS: FINAL (M1.9 + CategoryMapper + Prefix + Description FIX)
// ============================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;
using AstronoData.Contracts.Domain;

namespace AstronoLab
{
    public static class SeedToExperimentConverter
    {
        public static void Run(string inputFolder, string outputFolder)
        {
            var files = Directory.GetFiles(inputFolder, "SCN_*.json");
            RunInternal(files, outputFolder);
        }

        public static void RunSingle(string file, string outputFolder)
        {
            RunInternal(new[] { file }, outputFolder);
        }

        private static void RunInternal(string[] files, string outputFolder)
        {
            foreach (var file in files)
            {
                Console.WriteLine($"Processing {file}...");

                var json = File.ReadAllText(file);
                using var doc = JsonDocument.Parse(json);

                var seed = doc.RootElement
                    .GetProperty("GeneratedSeeds")[0]
                    .GetProperty("SeedCandidate");

                var core = seed.GetProperty("Core");

                var startJD = Math.Floor(core.GetProperty("Time").GetProperty("StartJD").GetDouble());
                var stopJD = Math.Floor(core.GetProperty("Time").GetProperty("StopJD").GetDouble());
                var step = core.GetProperty("Time").GetProperty("Step").GetString();

                var experimentId = $"HELIO-J2000-TDB-{startJD}-{stopJD}-{step}".ToUpper();

                var fileName = Path.GetFileNameWithoutExtension(file);
                var catalogNumber = fileName.Replace("SCN_", "AS-");

                var eventNode = seed.GetProperty("Event");
                var category = eventNode.GetProperty("Category").GetString();

                var categoryAbbr = CategoryMapper.ToAbbreviation(category);

                var observedObject = core.GetProperty("ObservedObject");

                var bodyClass = observedObject.GetProperty("BodyClass").GetString().ToUpper();
                var target = observedObject.GetProperty("Targets")[0].GetString().ToUpper();

                var human = $"{bodyClass}-{target}-{categoryAbbr}";

                var finalFileName = $"{catalogNumber}__{human}__{experimentId}.json";

                // =====================================================
                // 🔥 FIX: Description IMMER als STRING
                // =====================================================

                var descProp = eventNode.GetProperty("Description");

                string description;

                if (descProp.ValueKind == JsonValueKind.Number)
                {
                    // JD exakt erhalten
                    description = descProp.GetRawText();
                }
                else if (descProp.ValueKind == JsonValueKind.String)
                {
                    description = descProp.GetString();
                }
                else if (descProp.ValueKind == JsonValueKind.Null)
                {
                    description = null;
                }
                else
                {
                    throw new Exception($"Unsupported Description type: {descProp.ValueKind}");
                }

                var eventObj = new
                {
                    Category = eventNode.GetProperty("Category").GetString(),
                    Qualifier = eventNode.GetProperty("Qualifier").GetString(),
                    Description = description
                };

                var experiment = new
                {
                    SchemaVersion = "1.0",
                    ExperimentID = experimentId,
                    CatalogNumber = catalogNumber,
                    CoreHash = "TO_BE_REPLACED",

                    Core = JsonSerializer.Deserialize<object>(core.GetRawText()),
                    Event = eventObj,
                    Metadata = JsonSerializer.Deserialize<object>(seed.GetProperty("Metadata").GetRawText()),
                    Notes = seed.GetProperty("Notes").GetString()
                };

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.Never,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };

                var jsonOut = JsonSerializer.Serialize(experiment, options);
                var formatted = ConvertToTwoSpaceIndent(jsonOut);

                var outFile = Path.Combine(outputFolder, finalFileName);

                File.WriteAllText(outFile, formatted, Encoding.UTF8);

                Console.WriteLine($"Created {outFile}");
            }
        }

        private static string ConvertToTwoSpaceIndent(string input)
        {
            var lines = input.Split('\n');
            int indent = 0;
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
    }
}