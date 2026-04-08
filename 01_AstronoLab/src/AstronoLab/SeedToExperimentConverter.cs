using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;
using System.Globalization;

namespace AstronoLab
{
    public static class SeedToExperimentConverter
    {
        private static readonly string OldScenarioPath =
            @"C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData\02_ObservationCatalog\Released";

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

                // --- Core ---
                var core = new
                {
                    Time = new
                    {
                        StartJD = seed.GetProperty("Core").GetProperty("Time").GetProperty("StartJD").GetDouble(),
                        StopJD = seed.GetProperty("Core").GetProperty("Time").GetProperty("StopJD").GetDouble(),
                        Step = seed.GetProperty("Core").GetProperty("Time").GetProperty("Step").GetString(),
                        TimeScale = seed.GetProperty("Core").GetProperty("Time").GetProperty("TimeScale").GetString()
                    },
                    Observer = new
                    {
                        Type = seed.GetProperty("Core").GetProperty("Observer").GetProperty("Type").GetString(),
                        Body = seed.GetProperty("Core").GetProperty("Observer").GetProperty("Body").GetString()
                    },
                    ObservedObject = new
                    {
                        BodyClass = seed.GetProperty("Core").GetProperty("ObservedObject").GetProperty("BodyClass").GetString(),
                        Targets = seed.GetProperty("Core").GetProperty("ObservedObject").GetProperty("Targets")
                            .EnumerateArray()
                            .Select(x => x.GetString())
                            .ToArray()
                    },
                    Frame = new
                    {
                        Type = seed.GetProperty("Core").GetProperty("Frame").GetProperty("Type").GetString(),
                        Epoch = seed.GetProperty("Core").GetProperty("Frame").GetProperty("Epoch").GetString()
                    }
                };

                // --- ExperimentID ---
                var startJD = Math.Floor(core.Time.StartJD);
                var stopJD = Math.Floor(core.Time.StopJD);
                var step = core.Time.Step;

                var experimentId = $"HELIO-J2000-TDB-{startJD}-{stopJD}-{step}".ToUpper();

                // --- CatalogNumber ---
                var fileName = Path.GetFileNameWithoutExtension(file);
                var catalogNumber = fileName.Replace("SCN_", "AS-");

                // --- Event ---
                object descriptionValue;
                var descProp = seed.GetProperty("Event").GetProperty("Description");

                if (descProp.ValueKind == JsonValueKind.Number)
                    descriptionValue = descProp.GetDouble();
                else if (descProp.ValueKind == JsonValueKind.Null)
                    descriptionValue = null;
                else
                    descriptionValue = descProp.GetString();

                var eventObj = new
                {
                    Category = seed.GetProperty("Event").GetProperty("Category").GetString(),
                    Qualifier = seed.GetProperty("Event").GetProperty("Qualifier").GetString(),
                    Description = descriptionValue
                };

                // --- Metadata ---
                var metadata = new
                {
                    Author = seed.GetProperty("Metadata").GetProperty("Author").GetString(),
                    Priority = seed.GetProperty("Metadata").GetProperty("Priority").GetInt32(),
                    Status = new
                    {
                        Maturity = seed.GetProperty("Metadata").GetProperty("Status").GetProperty("Maturity").GetString(),
                        Visibility = seed.GetProperty("Metadata").GetProperty("Status").GetProperty("Visibility").GetString()
                    }
                };

                // --- Step2_1: load old scenario ---
                var oldFile = Path.Combine(OldScenarioPath, catalogNumber + ".json");

                object scenarioCitation = null;
                object datasetHeader = null;

                if (File.Exists(oldFile))
                {
                    using var oldDoc = JsonDocument.Parse(File.ReadAllText(oldFile));

                    if (oldDoc.RootElement.TryGetProperty("ScenarioCitation", out var sc))
                        scenarioCitation = JsonSerializer.Deserialize<object>(sc.GetRawText());

                    if (oldDoc.RootElement.TryGetProperty("DatasetHeader", out var dh))
                        datasetHeader = JsonSerializer.Deserialize<object>(dh.GetRawText());
                }

                // --- Final Experiment ---
                var experiment = new
                {
                    SchemaVersion = "1.0",
                    ExperimentID = experimentId,
                    CatalogNumber = catalogNumber,
                    CoreHash = "TO_BE_REPLACED",

                    Core = core,
                    Event = eventObj,
                    Metadata = metadata,
                    Notes = seed.GetProperty("Notes").GetString(),

                    ScenarioCitation = scenarioCitation,
                    DatasetHeader = datasetHeader
                };

                // --- Serializer OPTIONS (FIXED DOUBLE FORMAT) ---
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.Never,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };

                
                var jsonOut = JsonSerializer.Serialize(experiment, options);

                // --- FORCE 2 SPACE + CRLF ---
                var formatted = ConvertToTwoSpaceIndent(jsonOut);

                var outFile = Path.Combine(outputFolder, fileName + ".json");

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

//    // --- DOUBLE FORMAT FIX (6 DECIMALS, NO STRING BREAKAGE) ---
//    public class FixedDoubleConverter : JsonConverter<double>
//    {
//        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//        {
//            return reader.GetDouble();
//        }

//        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
//        {
//            writer.WriteRawValue(value.ToString("0.000000", CultureInfo.InvariantCulture));
//        }
//    }
}