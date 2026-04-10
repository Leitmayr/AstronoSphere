using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EphemerisRegression.Api
{
    public sealed class HorizonsApiClient
    {
        private static readonly HttpClient _http = new()
        {
            Timeout = TimeSpan.FromMinutes(5)
        };

        public async Task<string> ExecuteAsync(HorizonsApiRequest request)
        {
            string url = BuildUrl(request);

            Console.WriteLine();
            Console.WriteLine("============================================");
            Console.WriteLine("REQUEST URL:");
            Console.WriteLine(url);
            Console.WriteLine("============================================");
            Console.WriteLine();

            await Task.Delay(2000); // Throttle

            var response = await _http.GetAsync(
                url,
                HttpCompletionOption.ResponseHeadersRead);

            if (!response.IsSuccessStatusCode)
            {
                var errorText = await response.Content.ReadAsStringAsync();

                Console.WriteLine("HTTP ERROR:");
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(errorText);
            }

            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);

            return await reader.ReadToEndAsync();
        }

        private string BuildUrl(HorizonsApiRequest request)
        {
            var parameters = request.ToParameterDictionary();

            var sb = new StringBuilder();
            sb.Append("https://ssd.jpl.nasa.gov/api/horizons.api?format=text");

            foreach (var kv in parameters)
            {
                sb.Append("&");
                sb.Append(kv.Key);
                sb.Append("=");
                sb.Append(Uri.EscapeDataString(kv.Value));
            }

            return sb.ToString();
        }
    }
}