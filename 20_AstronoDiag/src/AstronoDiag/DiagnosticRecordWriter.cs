using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AstronoDiag
{
    public sealed class DiagnosticRecordWriter
    {
        private readonly string _runFolder;

        public DiagnosticRecordWriter(string runFolder)
        {
            _runFolder = runFolder;
        }

        public void Write(DiagnosticRecord record, string human)
        {
            Directory.CreateDirectory(_runFolder);

            var fileName = BuildFileName(record, human);
            var path = Path.Combine(_runFolder, fileName);

            if (File.Exists(path))
                throw new InvalidOperationException($"DiagnosticRecord already exists: {path}");

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            options.Converters.Add(new JsonStringEnumConverter());

            var json = JsonSerializer.Serialize(record, options);
            File.WriteAllText(path, json);
        }

        private static string BuildFileName(DiagnosticRecord record, string human)
        {
            return $"DiagMsg__{record.CatalogNumber}__{human}__{record.Code}.json";
        }
    }
}