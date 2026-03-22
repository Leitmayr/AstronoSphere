// ============================================================
// FILE: ScenarioHeaderGenerator.cs
// STATUS: UPDATE (v3.1 FIXES)
// ============================================================

using AstronoData.ScenarioCandidates;
using ScenarioHeaderGenerator.ScenarioCandidates;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ScenarioHeaderGenerator
{
    public sealed class ScenarioHeaderGenerator
    {
        private readonly string _createdFolder =
            AstronoSpherePaths.GetObservationCatalogCreatedFolder();

        private readonly string _releasedFolder =
            AstronoSpherePaths.GetObservationCatalogReleasedFolder();

        public void Run(IEnumerable<ScenarioCandidate> candidates)
        {
            Directory.CreateDirectory(_createdFolder);

            int counter = CatalogNumberGenerator.GetNextStart(_releasedFolder);

            foreach (var candidate in candidates)
            {
                var core = candidate.Core;

                ApplyDefaults(core);

                var scenarioId = ScenarioIdGenerator.Generate(core);
                var catalogNumber = $"AS-{counter:D6}";
                var coreHash = CoreHashGenerator.Generate(core);

                var json = BuildPrettyJson(scenarioId, catalogNumber, coreHash, core);

                var filePath = Path.Combine(_createdFolder, $"{catalogNumber}.json");

                File.WriteAllText(filePath, json, Encoding.UTF8);

                counter++;
            }
        }

        private static void ApplyDefaults(CoreDefinition core)
        {
            if (core.Observer.Location == null)
            {
                core.Observer.Location = new LocationDefinition
                {
                    Lat = 0.0,
                    Lon = 0.0,
                    Elevation = 0.0,
                    SiteName = "CenterOfObservingBody"
                };
            }
        }

        private static string BuildPrettyJson(
            string scenarioId,
            string catalogNumber,
            string coreHash,
            CoreDefinition core)
        {
            var sb = new StringBuilder();

            sb.AppendLine("{");
            sb.AppendLine("  \"SchemaVersion\": \"1.0\",");
            sb.AppendLine($"  \"ScenarioID\": \"{scenarioId}\",");
            sb.AppendLine($"  \"CatalogNumber\": \"{catalogNumber}\",");
            sb.AppendLine($"  \"CoreHash\": \"{coreHash}\",");

            sb.AppendLine("  \"Core\": {");

            sb.AppendLine("    \"Time\": {");
            sb.AppendLine($"      \"StartJD\": {F(core.Time.StartJD)},");
            sb.AppendLine($"      \"StopJD\": {F(core.Time.StopJD)},");
            sb.AppendLine($"      \"StepDays\": {F(core.Time.StepDays)},");
            sb.AppendLine($"      \"TimeScale\": \"{core.Time.TimeScale}\"");
            sb.AppendLine("    },");

            sb.AppendLine("    \"Observer\": {");
            sb.AppendLine($"      \"Type\": \"{core.Observer.Type}\",");
            sb.AppendLine($"      \"Body\": \"{core.Observer.Body}\",");

            sb.AppendLine("      \"Location\": {");
            sb.AppendLine($"        \"Lat\": {F(core.Observer.Location.Lat)},");
            sb.AppendLine($"        \"Lon\": {F(core.Observer.Location.Lon)},");
            sb.AppendLine($"        \"Elevation\": {F(core.Observer.Location.Elevation ?? 0.0)},");
            sb.AppendLine($"        \"SiteName\": \"{core.Observer.Location.SiteName}\"");
            sb.AppendLine("      }");

            sb.AppendLine("    },");

            sb.AppendLine("    \"Targets\": [");
            for (int i = 0; i < core.Targets.Length; i++)
            {
                var comma = i < core.Targets.Length - 1 ? "," : "";
                sb.AppendLine($"      \"{core.Targets[i]}\"{comma}");
            }
            sb.AppendLine("    ],");

            sb.AppendLine("    \"Frame\": {");
            sb.AppendLine($"      \"Type\": \"{core.Frame.Type}\",");
            sb.AppendLine($"      \"Epoch\": \"{core.Frame.Epoch}\"");
            sb.AppendLine("    },");

            sb.AppendLine("    \"Corrections\": {");
            sb.AppendLine($"      \"LightTime\": {core.Corrections.LightTime.ToString().ToLower()},");
            sb.AppendLine($"      \"Aberration\": {core.Corrections.Aberration.ToString().ToLower()},");
            sb.AppendLine($"      \"Precession\": {core.Corrections.Precession.ToString().ToLower()},");
            sb.AppendLine($"      \"Nutation\": {core.Corrections.Nutation.ToString().ToLower()}");
            sb.AppendLine("    }");

            sb.AppendLine("  }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private static string F(double v) =>
            v.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture);
    }
}