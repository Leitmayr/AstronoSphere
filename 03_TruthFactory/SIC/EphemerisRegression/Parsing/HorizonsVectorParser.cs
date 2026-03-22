using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EphemerisRegression.Domain;

namespace EphemerisRegression.Parsing
{
    public sealed class HorizonsVectorParser
    {
        public IEnumerable<StateVector> Parse(string rawText)
        {
            var lines = rawText.Split(
                new[] { "\r\n", "\n" },
                StringSplitOptions.None);

            var vectors = new List<StateVector>();

            bool insideDataBlock = false;

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Trim();

                if (line == "$$SOE")
                {
                    insideDataBlock = true;
                    continue;
                }

                if (line == "$$EOE")
                {
                    break;
                }

                if (!insideDataBlock)
                    continue;

                // JD line
                if (double.TryParse(
                        line.Split(' ')[0],
                        NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out double jd))
                {
                    // Next lines must be:
                    // X/Y/Z
                    // VX/VY/VZ

                    var posLine = lines[++i].Trim();
                    var velLine = lines[++i].Trim();

                    var (x, y, z) = ParseXYZLine(posLine);
                    var (vx, vy, vz) = ParseXYZLine(velLine);

                    vectors.Add(new StateVector(
                        jd,
                        x, y, z,
                        vx, vy, vz));
                }
            }

            return vectors;
        }

        private static (double X, double Y, double Z) ParseXYZLine(string line)
        {
            // Example:
            // X = 2.322003149478238E-01 Y = 2.158827608782743E-01 Z =-3.655411625819229E-03
            // or:
            // VX=-2.470957324320873E-02 VY=...

            var tokens = line
                .Replace("=", " ")
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // tokens will look like:
            // [X, 2.322E-01, Y, 2.158E-01, Z, -3.65E-03]

            double x = double.Parse(tokens[1], CultureInfo.InvariantCulture);
            double y = double.Parse(tokens[3], CultureInfo.InvariantCulture);
            double z = double.Parse(tokens[5], CultureInfo.InvariantCulture);

            return (x, y, z);
        }
    }
}

