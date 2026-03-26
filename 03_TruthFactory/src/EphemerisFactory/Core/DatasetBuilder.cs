using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;
using EphemerisRegression.Domain;

namespace EphemerisFactory.Core
{
    public static class DatasetBuilder
    {
        public static string Build(
            string scenarioJson,
            string canonical,
            string requestHash,
            string epochHash,
            string level,
            string url,
            string rawCsv)
        {
            using var doc = JsonDocument.Parse(scenarioJson);
            var root = doc.RootElement;

            var scenarioId = root.GetProperty("ScenarioID").GetString()!;
            var coreHash = root.GetProperty("CoreHash").GetString()!;
            var catalog = root.GetProperty("CatalogNumber").GetString()!;

            var sb = new StringBuilder(scenarioJson);

            // =====================================================
            // 1) SCENARIO HEADER KOMPLETT ENTFERNEN
            // =====================================================

            RemoveScenarioBlock(sb);

            // =====================================================
            // 2) SCENARIO REF EINSETZEN
            // =====================================================

            InsertScenarioRef(sb, scenarioId, coreHash, catalog);

            // =====================================================
            // 3) DATASET HEADER ERGÄNZEN (NICHT ZERSTÖREN!)
            // =====================================================

            ReplaceDatasetId(sb, level);

            ReplaceIfNull(sb, "CanonicalRequest",
                $"\"CanonicalRequest\": \"{Escape(canonical)}\"");

            ReplaceIfNull(sb, "RequestHash",
                $"\"RequestHash\": \"{requestHash}\"");

            ReplaceIfNull(sb, "EpochHash",
                $"\"EpochHash\": \"{epochHash}\"");

            ReplaceIfNull(sb, "TruthProviderUrl",
                $"\"TruthProviderUrl\": \"{url}\"");

            ReplaceIfNull(sb, "Requests",
                $"\"Requests\": [{{\"CanonicalRequest\": \"{Escape(canonical)}\", \"RequestHash\": \"{requestHash}\", \"HorizonsUrl\": \"{url}\"}}]");

            // =====================================================
            // 4) DATA PARSEN
            // =====================================================

            var data = ParseStateVectors(rawCsv);

            ReplaceData(sb, data);

            return sb.ToString();
        }

        // =====================================================
        // CSV -> STATEVECTOR
        // =====================================================

        private static List<StateVector> ParseStateVectors(string rawCsv)
        {
            var rows = HorizonsCsvParser.ParseRaw(rawCsv);
            var result = new List<StateVector>(rows.Count);

            foreach (var row in rows)
            {
                if (row.Length < 7)
                    continue;

                result.Add(new StateVector(
                    row[0], // JulianDate
                    row[1], // X
                    row[2], // Y
                    row[3], // Z
                    row[4], // VX
                    row[5], // VY
                    row[6]  // VZ
                ));
            }

            return result;
        }

        // =====================================================
        // SCENARIO ENTFERNEN
        // =====================================================

        private static void RemoveScenarioBlock(StringBuilder sb)
        {
            var content = sb.ToString();

            int datasetHeaderIndex = content.IndexOf("\"DatasetHeader\":", StringComparison.Ordinal);
            if (datasetHeaderIndex < 0)
                return;

            content = content.Substring(datasetHeaderIndex);

            sb.Clear();
            sb.Append(content);
        }

        private static void InsertScenarioRef(
            StringBuilder sb,
            string id,
            string hash,
            string catalog)
        {
            var content = sb.ToString();

            var refBlock =
$@"{{
  ""ScenarioRef"": {{
    ""ScenarioID"": ""{id}"",
    ""CoreHash"": ""{hash}"",
    ""CatalogNumber"": ""{catalog}""
  }},";

            content = refBlock + "\n" + content;

            sb.Clear();
            sb.Append(content);
        }

        // =====================================================
        // DATASET ID
        // =====================================================

        private static void ReplaceDatasetId(StringBuilder sb, string level)
        {
            var content = sb.ToString();

            content = content.Replace(
                "--EPH-PLACEHOLDER",
                $"--EPH-HORIZONS-DE440-{level}",
                StringComparison.Ordinal);

            sb.Clear();
            sb.Append(content);
        }

        // =====================================================
        // NULL ONLY
        // =====================================================

        private static void ReplaceIfNull(StringBuilder sb, string key, string value)
        {
            var search = $"\"{key}\": null";

            if (!sb.ToString().Contains(search, StringComparison.Ordinal))
                return;

            sb.Replace(search, value);
        }

        // =====================================================
        // DATA
        // =====================================================

        private static void ReplaceData(StringBuilder sb, List<StateVector> data)
        {
            var content = sb.ToString();
            var dataBlock = BuildDataBlock(data);

            int idx = content.IndexOf("\"Data\":", StringComparison.Ordinal);
            if (idx >= 0)
            {
                content = content.Substring(0, idx) + dataBlock;
            }
            else
            {
                content = content.TrimEnd('}', '\n', '\r') + ",\n" + dataBlock + "\n}";
            }

            sb.Clear();
            sb.Append(content);
        }

        private static string BuildDataBlock(List<StateVector> data)
        {
            var sb = new StringBuilder();

            sb.AppendLine("\"Data\": [");

            for (int i = 0; i < data.Count; i++)
            {
                var d = data[i];
                var comma = i < data.Count - 1 ? "," : "";

                sb.AppendLine("  {");
                sb.AppendLine($"    \"JD\": {F(d.JulianDate)},");
                sb.AppendLine($"    \"Position\": {{ \"X\": {F(d.X)}, \"Y\": {F(d.Y)}, \"Z\": {F(d.Z)} }},");
                sb.AppendLine($"    \"Velocity\": {{ \"X\": {F(d.VX)}, \"Y\": {F(d.VY)}, \"Z\": {F(d.VZ)} }}");
                sb.AppendLine($"  }}{comma}");
            }

            sb.AppendLine("]");

            return sb.ToString();
        }

        // =====================================================
        // HELPERS
        // =====================================================

        private static string F(double v) =>
            v.ToString("0.########", CultureInfo.InvariantCulture);

        private static string Escape(string s) =>
            s.Replace("\\", "\\\\")
             .Replace("\"", "\\\"")
             .Replace("\n", "\\n");
    }
}