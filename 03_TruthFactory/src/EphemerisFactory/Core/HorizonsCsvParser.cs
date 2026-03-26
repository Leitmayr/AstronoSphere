using System;
using System.Collections.Generic;
using System.Globalization;

namespace EphemerisFactory.Core
{
    public static class HorizonsCsvParser
    {
        public static List<double[]> ParseRaw(string raw)
        {
            var result = new List<double[]>();

            var lines = raw.Split('\n');

            bool inData = false;

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

                // wir brauchen mindestens:
                // JD + (skip calendar) + X Y Z VX VY VZ
                if (parts.Length < 8)
                    continue;

                try
                {
                    var values = new double[7];

                    values[0] = Parse(parts[0]); // JD
                    values[1] = Parse(parts[2]); // X
                    values[2] = Parse(parts[3]); // Y
                    values[3] = Parse(parts[4]); // Z
                    values[4] = Parse(parts[5]); // VX
                    values[5] = Parse(parts[6]); // VY
                    values[6] = Parse(parts[7]); // VZ

                    result.Add(values);
                }
                catch
                {
                    // bewusst ignorieren → robust
                }
            }

            return result;
        }

        private static double Parse(string s) =>
            double.Parse(s.Trim(), CultureInfo.InvariantCulture);
    }
}