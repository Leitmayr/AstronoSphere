using System;
using System.Collections.Generic;
using System.Globalization;

namespace EphemerisFactory.Core
{
    public static class HorizonsStateParser
    {
        public static List<StateVector> Parse(string raw)
        {
            var result = new List<StateVector>();

            var lines = raw.Split('\n');

            bool inData = false;

            double jd = 0;
            double x = 0, y = 0, z = 0;
            double vx = 0, vy = 0, vz = 0;

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

                if (!inData)
                    continue;

                // JD
                if (l.Contains("=") && l.StartsWith("2"))
                {
                    var jdStr = l.Split('=')[0].Trim();
                    jd = double.Parse(jdStr, CultureInfo.InvariantCulture);
                }
                // Position
                else if (l.StartsWith("X ="))
                {
                    var parts = l.Replace("=", "")
                                 .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    x = double.Parse(parts[1], CultureInfo.InvariantCulture);
                    y = double.Parse(parts[3], CultureInfo.InvariantCulture);
                    z = double.Parse(parts[5], CultureInfo.InvariantCulture);
                }
                // Velocity → vollständiger Datensatz
                else if (l.StartsWith("VX"))
                {
                    var parts = l.Replace("=", "")
                                 .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    vx = double.Parse(parts[1], CultureInfo.InvariantCulture);
                    vy = double.Parse(parts[3], CultureInfo.InvariantCulture);
                    vz = double.Parse(parts[5], CultureInfo.InvariantCulture);

                    result.Add(new StateVector
                    {
                        JD = jd,
                        Position = new Vector3
                        {
                            X = x,
                            Y = y,
                            Z = z
                        },
                        Velocity = new Vector3
                        {
                            X = vx,
                            Y = vy,
                            Z = vz
                        }
                    });
                }
            }

            return result;
        }
    }
}