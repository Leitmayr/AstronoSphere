using System;
using System.Globalization;
using System.IO;
using AstroSim.Ephemerides.VSOP.Model;

namespace AstroSim.Ephemerides.VSOP.Parsing
{
    /// <summary>
    /// Parser für originale IMCCE VSOP87A-Dateien (VERSION A1 Format).
    /// 
    /// Erwartetes Headerformat:
    /// VSOP87 VERSION A1 EARTH VARIABLE 1 (XYZ) *T**0 843 TERMS
    /// 
    /// Datenzeilen enthalten viele Spalten, aber nur die letzten 3 sind relevant:
    /// A  B  C
    /// </summary>
    public static class Vsop87Parser
    {
        public static VsopPlanet Parse(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"VSOP file not found: {filePath}");

            string planetName =
                Path.GetFileNameWithoutExtension(filePath);

            var planet = new VsopPlanet(planetName);

            int currentCoordinate = -1;  // 0=X, 1=Y, 2=Z
            int currentOrder = -1;       // 0..5

            foreach (var rawLine in File.ReadLines(filePath))
            {
                string line = rawLine.Trim();

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // ---------------------------------------------------------
                // HEADER
                // ---------------------------------------------------------
                if (line.StartsWith("VSOP87"))
                {
                    ParseHeader(line, out currentCoordinate, out currentOrder);
                    continue;
                }

                // ---------------------------------------------------------
                // DATA LINE
                // ---------------------------------------------------------
                ParseTerm(line, planet, currentCoordinate, currentOrder);
            }

            return planet;
        }

        // -------------------------------------------------------------
        // HEADER PARSING
        // -------------------------------------------------------------
        private static void ParseHeader(
            string line,
            out int coordinateIndex,
            out int order)
        {
            coordinateIndex = -1;
            order = -1;

            var tokens = line.Split(
                new[] { ' ' },
                StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < tokens.Length; i++)
            {
                if (tokens[i] == "VARIABLE")
                {
                    // VARIABLE 1 → X
                    coordinateIndex = int.Parse(tokens[i + 1]) - 1;
                }

                if (tokens[i].StartsWith("*T**"))
                {
                    // *T**0 → Ordnung 0
                    string orderString = tokens[i].Substring(4);
                    order = int.Parse(orderString);
                }
            }

            if (coordinateIndex < 0 || coordinateIndex > 2)
                throw new InvalidDataException("Invalid coordinate index in header.");

            if (order < 0 || order > 5)
                throw new InvalidDataException("Invalid series order in header.");
        }

        // -------------------------------------------------------------
        // TERM PARSING
        // -------------------------------------------------------------
        private static void ParseTerm(
            string line,
            VsopPlanet planet,
            int coordinateIndex,
            int order)
        {
            var tokens = line.Split(
                new[] { ' ' },
                StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length < 3)
                return;

            // Die letzten 3 Spalten sind immer A, B, C
            double A = double.Parse(
                tokens[tokens.Length - 3],
                CultureInfo.InvariantCulture);

            double B = double.Parse(
                tokens[tokens.Length - 2],
                CultureInfo.InvariantCulture);

            double C = double.Parse(
                tokens[tokens.Length - 1],
                CultureInfo.InvariantCulture);

            planet.Coordinates[coordinateIndex]
                  .Series[order]
                  .Terms.Add(new VsopTerm(A, B, C));
        }
    }
}