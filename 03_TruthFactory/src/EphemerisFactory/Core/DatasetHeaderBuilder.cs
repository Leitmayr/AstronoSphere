// ============================================================
// FILE: DatasetHeaderBuilder.cs
// STATUS: FIX (JsonElement-based, no dynamic)
// ============================================================

using EphemerisRegression.Api;
using EphemerisRegression.Infrastructure;
using System.Text;
using System.Text.Json;

namespace EphemerisFactory.Core
{
    public static class DatasetHeaderBuilder
    {
        public static string Populate(string json)
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var scenarioId = root.GetProperty("ScenarioID").GetString();

            var core = root.GetProperty("Core");

            var request = BuildRequest(core);

            var parameters = request.ToParameterDictionary();

            var canonical = CanonicalRequestBuilder.Build(parameters);
            var hash = HashCalculator.ComputeSha256(canonical);

            return InjectDatasetHeader(json, scenarioId!, canonical, hash, core);
        }

        // =====================================================
        // BUILD REQUEST
        // =====================================================

        private static HorizonsApiRequest BuildRequest(JsonElement core)
        {
            var observer = core.GetProperty("Observer");
            var time = core.GetProperty("Time");
            var targets = core.GetProperty("Targets");

            var targetName = targets[0].GetString()!;
            int command = PlanetMapper.ToCommand(targetName);

            string observerType = observer.GetProperty("Type").GetString()!;

            string center = observerType switch
            {
                "Heliocentric" => "@10",
                "Geocentric" => "500@399",
                _ => throw new Exception($"Unsupported observer type: {observerType}")
            };

            double start = time.GetProperty("StartJD").GetDouble();
            double stop = time.GetProperty("StopJD").GetDouble();
            double step = time.GetProperty("StepDays").GetDouble();

            return new HorizonsApiRequest
            {
                Command = command,
                Center = center,
                StartTime = $"JD{F(start)}",
                StopTime = $"JD{F(stop)}",
                StepSize = $"{F(step)}D",
                RefPlane = "ECLIPTIC",
                RefSystem = "ICRF",
                OutputUnits = "AU-D",
                EphemType = "VECTORS",
                CsvFormat = "NO"
            };
        }

        // =====================================================
        // INJECT INTO JSON (simple but safe for pilot)
        // =====================================================

        private static string InjectDatasetHeader(
            string original,
            string scenarioId,
            string canonical,
            string hash,
            JsonElement core)
        {
            var sb = new StringBuilder(original);

            // VERY SIMPLE APPROACH (pilot!):
            // replace placeholder fields

            string datasetId = $"{scenarioId}--EPH-HORIZONS-DE440";

            sb = ReplaceFullLine(sb, "DatasetID", $"\"DatasetID\": \"{datasetId}\"");
            sb.Replace("\"CanonicalRequest\": null", $"\"CanonicalRequest\": \"{Escape(canonical)}\"");
            sb.Replace("\"RequestHash\": null", $"\"RequestHash\": \"{hash}\"");

            sb.Replace("\"FactoryName\": null", "\"FactoryName\": \"EphemerisFactory\"");
            sb.Replace("\"Source\": null", "\"Source\": \"JPL_Horizons\"");

            string mode = core.GetProperty("Observer").GetProperty("Type").GetString() == "Heliocentric"
                ? "HELIO"
                : "GEO";

            sb.Replace("\"Mode\": null", $"\"Mode\": \"{mode}\"");

            return sb.ToString();
        }

        private static string F(double v) =>
            v.ToString("0.########", System.Globalization.CultureInfo.InvariantCulture);

        private static string Escape(string s) =>
            s.Replace("\"", "\\\"").Replace("\n", "\\n");


        private static StringBuilder ReplaceFullLine(StringBuilder sb, string key, string newLine)
        {
            var lines = sb.ToString().Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].TrimStart().StartsWith($"\"{key}\""))
                {
                    lines[i] = "    " + newLine + ",";
                    break;
                }
            }

            return new StringBuilder(string.Join("\n", lines));
        }
    }
}