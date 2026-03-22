// ============================================================
// FILE: /Parsing/HorizonsObserverParser.cs
// STATUS: NEU
// ============================================================

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace EphemerisRegression.Parsing
{
    public sealed class HorizonsObserverParser
    {
        private static readonly CultureInfo Culture =
            CultureInfo.InvariantCulture;

        public IEnumerable<ObserverRow> Parse(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                yield break;

            var lines = raw.Split('\n')
                           .Select(l => l.Trim())
                           .ToList();

            bool insideData = false;

            foreach (var line in lines)
            {
                if (line.StartsWith("$$SOE"))
                {
                    insideData = true;
                    continue;
                }

                if (line.StartsWith("$$EOE"))
                    yield break;

                if (!insideData)
                    continue;

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(
                    new[] { ' ' },
                    StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length < 3)
                    continue;

                if (!double.TryParse(parts[0], NumberStyles.Float, Culture, out double jd))
                    continue;

                if (!double.TryParse(parts[1], NumberStyles.Float, Culture, out double ra))
                    continue;

                if (!double.TryParse(parts[2], NumberStyles.Float, Culture, out double dec))
                    continue;

                yield return new ObserverRow
                {
                    JulianDate = jd,
                    Ra = ra,
                    Dec = dec
                };
            }
        }
    }

    public sealed class ObserverRow
    {
        public double JulianDate { get; init; }
        public double Ra { get; init; }
        public double Dec { get; init; }
    }
}