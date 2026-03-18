using System.IO;
using System.Text.Json;

namespace EphemerisRegression.Export
{
    public sealed class JsonReferenceWriter
    {
        private readonly JsonSerializerOptions _options;

        public JsonReferenceWriter(JsonSerializerOptions options)
        {
            _options = options;
        }

        public async Task WriteAsync<T>(
            string path,
            T model)
        {
            var json = JsonSerializer.Serialize(model, _options);
            await File.WriteAllTextAsync(path, json);
        }
    }
}