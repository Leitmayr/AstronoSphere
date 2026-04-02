// ============================================================
// FILE: ScenarioHeaderMerger.cs
// STATUS: FINAL (Sollstruktur + Encoding + Stable Merge)
// ============================================================

using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;

class Program
{
    static void Main()
    {
        var createdPath = @"C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData\02_ObservationCatalog\Created";
        var lastReleasedPath = @"C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData\02_ObservationCatalog\Released\LastReleased";
        var outputPath = @"C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData\02_ObservationCatalog\Released";

        Directory.CreateDirectory(outputPath);

        var files = Directory.GetFiles(createdPath, "*.json");

        Console.WriteLine($"Found {files.Length} files.");

        int success = 0;
        int missing = 0;

        foreach (var newFile in files)
        {
            var fileName = Path.GetFileName(newFile);
            var oldFile = Path.Combine(lastReleasedPath, fileName);

            if (!File.Exists(oldFile))
            {
                Console.WriteLine($"[WARN] Missing old file: {fileName}");
                missing++;
                continue;
            }

            var newJson = JsonNode.Parse(File.ReadAllText(newFile))!.AsObject();
            var oldJson = JsonNode.Parse(File.ReadAllText(oldFile))!.AsObject();

            var merged = Merge(newJson, oldJson);

            var outFile = Path.Combine(outputPath, fileName);

            File.WriteAllText(outFile,
                merged.ToJsonString(new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                }));

            Console.WriteLine($"[OK] {fileName}");
            success++;
        }

        Console.WriteLine("====================================");
        Console.WriteLine($"Merged:  {success}");
        Console.WriteLine($"Missing: {missing}");
        Console.WriteLine("Done.");
    }

    static JsonObject Merge(JsonObject newObj, JsonObject oldObj)
    {
        var result = new JsonObject();

        // ============================================================
        // 1) FIXED ORDER (Sollstruktur)
        // ============================================================

        // 1 SchemaVersion (ALT)
        AddFromOld(oldObj, result, "SchemaVersion");

        // 2 ScenarioID (NEW)
        AddFromNew(newObj, result, "ScenarioID");

        // 3 CatalogNumber (ALT)
        AddFromOld(oldObj, result, "CatalogNumber");

        // 4 CoreHash (NEW)
        AddFromNew(newObj, result, "CoreHash");

        // 5 Core (NEW)
        AddFromNew(newObj, result, "Core");

        // ============================================================
        // 2) RESTLICHER HEADER (ALT, feste Reihenfolge)
        // ============================================================

        AddFromOld(oldObj, result, "Author");
        AddFromOld(oldObj, result, "Extensions");
        AddFromOld(oldObj, result, "Status");
        AddFromOld(oldObj, result, "ScenarioType");
        AddFromOld(oldObj, result, "ScenarioCategory");
        AddFromOld(oldObj, result, "EventComment");
        AddFromOld(oldObj, result, "Description");
        AddFromOld(oldObj, result, "Rationale");
        AddFromOld(oldObj, result, "ScientificPurpose");
        AddFromOld(oldObj, result, "Priority");
        AddFromOld(oldObj, result, "ScenarioCitation");

        // ============================================================
        // 3) DatasetHeader (ALT, immer am Ende)
        // ============================================================

        AddFromOld(oldObj, result, "DatasetHeader");

        return result;
    }

    // ============================================================
    // HELPERS
    // ============================================================

    static void AddFromNew(JsonObject source, JsonObject target, string key)
    {
        if (source.ContainsKey(key))
            target[key] = Clone(source[key]);
    }

    static void AddFromOld(JsonObject source, JsonObject target, string key)
    {
        if (source.ContainsKey(key))
            target[key] = Clone(source[key]);
    }

    static JsonNode? Clone(JsonNode? node)
    {
        if (node == null)
            return null;

        return JsonNode.Parse(node.ToJsonString());
    }
}