using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Astronometria.Core.ScientificRun.Models;

namespace Astronometria.Core.ScientificRun.Diagnostics
{
    public static class ScientificRunDiagnosticWriter
    {
        public static string WriteResolutionDiagnostic(
            string diagRunFolder,
            ExperimentInputModel experiment,
            string code,
            string message,
            IReadOnlyList<string> matchingFiles)
        {
            Directory.CreateDirectory(diagRunFolder);

            var fileName =
                $"DiagMsg__{experiment.CatalogNumber}__{code}.json";

            var path = Path.Combine(diagRunFolder, fileName);

            var record = new ScientificRunDiagnosticRecord
            {
                Code = code,
                Severity = "Warning",
                SourceSystem = "Astronometria",
                SubSourceSystem = "ScientificRun",
                CatalogNumber = experiment.CatalogNumber,
                ExperimentID = experiment.ExperimentID,
                CoreHash = experiment.CoreHash,
                Message = message,
                MatchingFiles = matchingFiles
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            var json = JsonSerializer.Serialize(record, options);

            File.WriteAllText(
                path,
                json,
                new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));

            return path;
        }
    }

    public sealed class ScientificRunDiagnosticRecord
    {
        public string Code { get; init; } = string.Empty;

        public string Severity { get; init; } = string.Empty;

        public string SourceSystem { get; init; } = string.Empty;

        public string SubSourceSystem { get; init; } = string.Empty;

        public string CatalogNumber { get; init; } = string.Empty;

        public string ExperimentID { get; init; } = string.Empty;

        public string CoreHash { get; init; } = string.Empty;

        public string Message { get; init; } = string.Empty;

        public IReadOnlyList<string> MatchingFiles { get; init; } = Array.Empty<string>();
    }
}