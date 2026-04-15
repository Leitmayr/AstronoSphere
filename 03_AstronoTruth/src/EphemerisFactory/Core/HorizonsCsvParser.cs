// ============================================================
// FILE: HorizonsCsvParser.cs
// STATUS: UPDATE (M1.9 JD STRING FIX - MINIMAL)
// ============================================================

using System;
using System.Collections.Generic;
using System.Globalization;

namespace EphemerisFactory.Core
{
    public sealed class CsvRow
    {
        public string JdRaw { get; set; } = "";
        public double[] Values { get; set; } = new double[7];
    }

    public static class HorizonsCsvParser
    {
        public static List<CsvRow> ParseRaw(string raw)
        {
            var result = new List<CsvRow>();

            var lines = raw.Split('\n');

            bool inData = false;
            bool first = true;

            foreach (var line in lines)
            {
                var l = line.Trim();

                if (l.StartsWith("$$SOE"))
                {
                    inData = true;
                    continue;
                }

                if (l.StartsWith("$$EOE"))
                    break;

                if (!inData || string.IsNullOrWhiteSpace(l))
                    continue;

                var parts = l.Split(',');

                if (parts.Length < 8)
                    continue;

                try
                {
                    var row = new CsvRow();

                    // 🔥 JD RAW STRING (NEU)
                    row.JdRaw = parts[0].Trim();

                    if (first)
                    {
                        Console.WriteLine("=== CSV DEBUG (FIRST ROW) ===");
                        Console.WriteLine($"RAW LINE: {l}");
                        Console.WriteLine($"JD STRING: {row.JdRaw}");
                    }

                    var values = new double[7];

                    // weiterhin double für Berechnungen
                    values[0] = Parse(parts[0]);

                    if (first)
                    {
                        Console.WriteLine($"JD PARSED: {values[0]:G17}");
                        Console.WriteLine("=============================");
                        first = false;
                    }

                    values[1] = Parse(parts[2]);
                    values[2] = Parse(parts[3]);
                    values[3] = Parse(parts[4]);
                    values[4] = Parse(parts[5]);
                    values[5] = Parse(parts[6]);
                    values[6] = Parse(parts[7]);

                    row.Values = values;

                    result.Add(row);
                }
                catch
                {
                }
            }

            return result;
        }

        private static double Parse(string s) =>
            double.Parse(s.Trim(), CultureInfo.InvariantCulture);
    }
}