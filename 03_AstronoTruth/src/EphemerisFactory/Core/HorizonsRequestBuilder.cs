// ============================================================
// FILE: HorizonsRequestBuilder.cs
// STATUS: FINAL (M1.9 + Build + ParameterHash helper)
// ============================================================

using EphemerisRegression.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace EphemerisFactory.Core
{
    public static class HorizonsRequestBuilder
    {
        // ============================================================
        // BUILD HORIZONS REQUEST FROM EXPERIMENT JSON
        // ============================================================
        public static HorizonsApiRequest Build(JsonElement root)
        {
            var core = root.GetProperty("Core");

            var observer = core.GetProperty("Observer");
            var time = core.GetProperty("Time");

            var observedObject = core.GetProperty("ObservedObject");
            var targets = observedObject.GetProperty("Targets");

            var targetName = targets[0].GetString()!;
            int command = PlanetMapper.ToCommand(targetName);

            string observerType = observer.GetProperty("Type").GetString()!;

            string center = observerType switch
            {
                "Heliocentric" => "@10",
                "Geocentric" => "500@399",
                _ => throw new Exception($"Unsupported observer type: {observerType}")
            };

            // =====================================================
            // DEBUG: RAW INPUT VALUES
            // =====================================================

            string startRaw = time.GetProperty("StartJD").GetRawText();
            string stopRaw = time.GetProperty("StopJD").GetRawText();
            string step = time.GetProperty("Step").GetString()!;

            Console.WriteLine("=== HORIZONS INPUT DEBUG ===");
            Console.WriteLine($"StartJD (raw) : {startRaw}");
            Console.WriteLine($"StopJD  (raw) : {stopRaw}");
            Console.WriteLine($"Step          : {step}");
            Console.WriteLine("================================");

            // =====================================================
            // BUILD REQUEST
            // =====================================================

            string start = $"JD{startRaw}";
            string stop = $"JD{stopRaw}";

            var request = new HorizonsApiRequest
            {
                Command = command,
                Center = center,
                StartTime = start,
                StopTime = stop,
                StepSize = step,
                RefPlane = "ECLIPTIC",
                RefSystem = "ICRF",
                OutputUnits = "AU-D",
                EphemType = "VECTORS",
                CsvFormat = "YES",
                VectorCorrection = null
            };

            Console.WriteLine("=== FINAL REQUEST VALUES ===");
            Console.WriteLine($"START_TIME = {request.StartTime}");
            Console.WriteLine($"STOP_TIME  = {request.StopTime}");
            Console.WriteLine("================================");

            return request;
        }

        // ============================================================
        // CANONICAL REQUEST + PARAMETER-BASED HASH
        // ============================================================
        public static (string canonicalRequest, string requestHash) BuildCanonicalAndHash(IDictionary<string, string> parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            // Sortiert, stabil, webtool-kompatibel
            var ordered = parameters
                .Where(kv => !string.IsNullOrWhiteSpace(kv.Key))
                .Select(kv => new KeyValuePair<string, string>(
                    kv.Key.Trim().ToUpperInvariant(),
                    NormalizeValue(kv.Value)))
                .OrderBy(kv => kv.Key, StringComparer.Ordinal)
                .ToArray();

            // Multi-line canonical for console/header
            var canonicalRequest = string.Join(
                "\n",
                ordered.Select(kv => $"{kv.Key}={kv.Value}")
            );

            Console.WriteLine();
            Console.WriteLine("===== CANONICAL REQUEST =====");
            Console.WriteLine(canonicalRequest);
            Console.WriteLine("=============================");
            Console.WriteLine();

            // Parameter-hash input: formatting independent from JSON escaping
            var hashInput = string.Join(
                "|",
                ordered.Select(kv => $"{kv.Key}={kv.Value}")
            );

            Console.WriteLine("===== HASH INPUT (PARAMETER) =====");
            Console.WriteLine(hashInput);
            Console.WriteLine("==================================");
            Console.WriteLine();

            var requestHash = ComputeSha256(hashInput);

            return (canonicalRequest, requestHash);
        }

        private static string NormalizeValue(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return value.Trim().ToUpperInvariant();
        }

        private static string ComputeSha256(string input)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha.ComputeHash(bytes);

            var sb = new StringBuilder(hash.Length * 2);
            foreach (var b in hash)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}