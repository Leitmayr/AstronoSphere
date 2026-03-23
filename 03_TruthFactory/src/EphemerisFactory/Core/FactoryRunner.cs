// ============================================================
// FILE: FactoryRunner.cs
// STATUS: FIX (JsonElement handling)
// ============================================================

using System;
using System.IO;
using System.Text.Json;

namespace EphemerisFactory.Core
{
    public sealed class FactoryRunner
    {
        public void Run()
        {
            var folder = AstronoSpherePaths.GetObservationCatalogReleasedFolder();

            if (!Directory.Exists(folder))
                throw new DirectoryNotFoundException($"Folder not found: {folder}");

            var files = Directory.GetFiles(folder, "AS-*.json");

            foreach (var file in files)
            {
                var json = File.ReadAllText(file);

                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                ValidateStatus(root, file);

                var updated = DatasetHeaderBuilder.Populate(json);

                File.WriteAllText(file, updated);

                Console.WriteLine($"Updated: {Path.GetFileName(file)}");
            }
        }

        // =====================================================
        // STATUS VALIDATION (ROBUST)
        // =====================================================

        private static void ValidateStatus(JsonElement root, string file)
        {
            if (!root.TryGetProperty("Status", out var status))
                throw new Exception($"Missing Status in scenario: {file}");

            if (!status.TryGetProperty("maturity", out var maturityProp))
                throw new Exception($"Missing Status.maturity in scenario: {file}");

            var maturity = maturityProp.GetString();

            if (maturity != "released")
            {
                throw new Exception(
                    $"Scenario not released: {file} (maturity={maturity})");
            }
        }
    }
}