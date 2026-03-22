using System.Text.Json;

namespace EphemerisRegression.Export
{
    public static class JsonOptionsFactory
    {
        public static JsonSerializerOptions Create()
        {
            return new JsonSerializerOptions
            {
                WriteIndented = true
            };
        }
    }
}
