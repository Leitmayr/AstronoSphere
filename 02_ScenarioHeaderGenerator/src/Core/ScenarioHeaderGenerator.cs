// ============================================================
// FILE: ScenarioHeaderGenerator.cs
// STATUS: UPDATED (StepDays as string, passt 1:1 durch)
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

                var json = BuildScenarioJson(candidate, scenarioId, catalogNumber, coreHash);

                var filePath = Path.Combine(_createdFolder, $"{catalogNumber}.json");

                File.WriteAllText(filePath, json, Encoding.UTF8);

                counter++;
            }
        }

        // =========================================================
        // DEFAULTS
        // =========================================================

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

        // =========================================================
        // BUILD FULL SCENARIO
        // =========================================================

        private static string BuildScenarioJson(
            ScenarioCandidate candidate,
            string scenarioId,
            string catalogNumber,
            string coreHash)
        {
            var core = candidate.Core;
            var ev = candidate.Event;

            var sb = new StringBuilder();

            sb.AppendLine("{");

            // =====================================================
            // HEADER
            // =====================================================

            sb.AppendLine("  \"SchemaVersion\": \"1.0\",");
            sb.AppendLine();
            sb.AppendLine($"  \"ScenarioID\": \"{scenarioId}\",");
            sb.AppendLine($"  \"CatalogNumber\": \"{catalogNumber}\",");
            sb.AppendLine($"  \"CoreHash\": \"{coreHash}\",");
            sb.AppendLine();
            sb.AppendLine("  \"Author\": null,");
            sb.AppendLine();
            sb.AppendLine("  \"Extensions\": \"placeholder for future needs\",");
            sb.AppendLine();
            sb.AppendLine("  \"Status\": {");
            sb.AppendLine("    \"maturity\": null,");
            sb.AppendLine("    \"visibility\": null");
            sb.AppendLine("  },");
            sb.AppendLine();
            sb.AppendLine($"  \"ScenarioType\": null,");
            sb.AppendLine($"  \"ScenarioCategory\": \"{ev.Category}\",");
            sb.AppendLine($"  \"EventComment\": \"{Escape(ev.Comment)}\",");
            sb.AppendLine();
            sb.AppendLine("  \"Description\": null,");
            sb.AppendLine();
            sb.AppendLine("  \"Rationale\": {");
            sb.AppendLine("    \"Phenomenon\": null,");
            sb.AppendLine("    \"NumericalRisk\": null,");
            sb.AppendLine("    \"Validates\": []");
            sb.AppendLine("  },");
            sb.AppendLine();
            sb.AppendLine("  \"ScientificPurpose\": null,");
            sb.AppendLine();
            sb.AppendLine("  \"Priority\": null,");
            sb.AppendLine();
            sb.AppendLine("  \"ScenarioCitation\": {");
            sb.AppendLine($"    \"Author\": \"{Escape(ev.Source)}\",");
            sb.AppendLine("    \"Source\": null,");
            sb.AppendLine("    \"Citation\": null");
            sb.AppendLine("  },");

            // =====================================================
            // CORE
            // =====================================================

            sb.AppendLine();
            sb.AppendLine("  \"Core\": {");

            sb.AppendLine("    \"Time\": {");
            sb.AppendLine($"      \"StartJD\": {F(core.Time.StartJD)},");
            sb.AppendLine($"      \"StopJD\": {F(core.Time.StopJD)},");

            // 🔥 WICHTIG: StepDays ist jetzt STRING → 1:1 durchreichen
            sb.AppendLine($"      \"StepDays\": \"{core.Time.StepDays}\",");

            sb.AppendLine($"      \"TimeScale\": \"{core.Time.TimeScale}\"");
            sb.AppendLine("    },");

            sb.AppendLine();
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

            sb.AppendLine();
            sb.AppendLine("    \"Targets\": [");
            for (int i = 0; i < core.Targets.Length; i++)
            {
                var comma = i < core.Targets.Length - 1 ? "," : "";
                sb.AppendLine($"      \"{core.Targets[i]}\"{comma}");
            }
            sb.AppendLine("    ],");

            sb.AppendLine();
            sb.AppendLine("    \"Frame\": {");
            sb.AppendLine($"      \"Type\": \"{core.Frame.Type}\",");
            sb.AppendLine($"      \"Epoch\": \"{core.Frame.Epoch}\"");
            sb.AppendLine("    },");

            sb.AppendLine();
            sb.AppendLine("    \"Corrections\": {");
            sb.AppendLine($"      \"LightTime\": {Bool(core.Corrections.LightTime)},");
            sb.AppendLine($"      \"Aberration\": {Bool(core.Corrections.Aberration)},");
            sb.AppendLine($"      \"Precession\": {Bool(core.Corrections.Precession)},");
            sb.AppendLine($"      \"Nutation\": {Bool(core.Corrections.Nutation)}");
            sb.AppendLine("    }");

            sb.AppendLine("  },");

            // =====================================================
            // DATASET HEADER (Skeleton)
            // =====================================================

            sb.AppendLine();
            sb.AppendLine("  \"DatasetHeader\": {");

            sb.AppendLine($"    \"DatasetID\": \"{scenarioId}--EPH-PLACEHOLDER\",");
            sb.AppendLine();

            sb.AppendLine("    \"TruthMetadata\": {");
            sb.AppendLine("      \"CanonicalRequest\": null,");
            sb.AppendLine("      \"RequestHash\": null,");
            sb.AppendLine("      \"EpochHash\": null,");
            sb.AppendLine("      \"Requests\": null,");
            sb.AppendLine("      \"TruthProviderUrl\": null,");
            sb.AppendLine("      \"GeneratedAtUtc\": null");
            sb.AppendLine("    },");

            sb.AppendLine();
            sb.AppendLine("    \"FactoryMetadata\": {");
            sb.AppendLine("      \"FactoryName\": null,");
            sb.AppendLine("      \"FactoryVersion\": null,");
            sb.AppendLine("      \"Source\": null,");
            sb.AppendLine("      \"ReferenceEphemeris\": null,");
            sb.AppendLine("      \"Mode\": null,");
            sb.AppendLine("      \"EphemType\": null,");
            sb.AppendLine("      \"CorrectionLevel\": null,");
            sb.AppendLine("      \"TimeScale\": null");
            sb.AppendLine("    },");

            sb.AppendLine();
            sb.AppendLine("    \"TruthCitation\": {");
            sb.AppendLine("      \"Provider\": null,");
            sb.AppendLine("      \"Source\": null,");
            sb.AppendLine("      \"Citation\": null");
            sb.AppendLine("    },");

            sb.AppendLine();
            sb.AppendLine("    \"Provenance\": {");
            sb.AppendLine("      \"ScenarioFactory\": null,");
            sb.AppendLine("      \"TruthFactory\": null,");
            sb.AppendLine("      \"ValidationTarget\": {");
            sb.AppendLine("        \"Software\": null,");
            sb.AppendLine("        \"GitCommit\": null,");
            sb.AppendLine("        \"GitBranch\": null,");
            sb.AppendLine("        \"GitTag\": null");
            sb.AppendLine("      }");
            sb.AppendLine("    },");

            sb.AppendLine();
            sb.AppendLine("    \"EngineCitation\": {");
            sb.AppendLine("      \"Author\": null,");
            sb.AppendLine("      \"Software\": null,");
            sb.AppendLine("      \"Citation\": null");
            sb.AppendLine("    },");

            sb.AppendLine();
            sb.AppendLine("    \"ValidationFingerprint\": null");

            sb.AppendLine("  }");

            sb.AppendLine("}");

            return sb.ToString();
        }

        // =========================================================
        // HELPERS
        // =========================================================

        private static string F(double v) =>
            v.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture);

        private static string Bool(bool b) => b ? "true" : "false";

        private static string Escape(string s) =>
            s?.Replace("\"", "\\\"") ?? "";
    }
}