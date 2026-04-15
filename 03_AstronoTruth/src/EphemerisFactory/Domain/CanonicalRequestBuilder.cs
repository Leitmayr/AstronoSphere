using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EphemerisRegression.Infrastructure
{
    public static class CanonicalRequestBuilder
    {
        public static string Build(IDictionary<string, string> parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            // Normalisierung + Sortierung
            var normalized = parameters
                .Where(p => !string.IsNullOrWhiteSpace(p.Key))
                .Select(p => new KeyValuePair<string, string>(
                    NormalizeKey(p.Key),
                    NormalizeValue(p.Value)))
                .OrderBy(p => p.Key, StringComparer.Ordinal);

            var sb = new StringBuilder();

            foreach (var pair in normalized)
            {
                sb.Append(pair.Key);
                sb.Append('=');
                sb.Append(pair.Value);
                sb.Append('\n');
            }

            return sb.ToString().TrimEnd('\n');
        }

        private static string NormalizeKey(string key)
        {
            return key.Trim().ToUpperInvariant();
        }

        private static string NormalizeValue(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return value.Trim().ToUpperInvariant();
        }
    }
}
