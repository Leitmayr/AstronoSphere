using NUnit.Framework;
using System.Globalization;

namespace Astronometria.Ephemerides.Test.VSOP
{
    public static class Vsop87ATestSource
    {
        private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

        private static readonly HashSet<string> SupportedPlanets =
            new(StringComparer.OrdinalIgnoreCase)
            {
                "MERCURY",
                "VENUS",
                "EARTH",
                "MARS",
                "JUPITER",
                "SATURN",
                "URANUS",
                "NEPTUNE"
            };

        private static string GetVariantDirectory()
        {
            return Path.Combine(
                AppContext.BaseDirectory,
                "VSOP",
                "Data",
                "VSOP87A");
        }

        // ------------------------------------------------------------
        // POSITION TESTS
        // ------------------------------------------------------------

        public static IEnumerable<TestCaseData> PositionCases()
        {
            var results = new List<TestCaseData>();

            var dir = Path.Combine(
                AppContext.BaseDirectory,
                "VSOP",
                "Data",
                "VSOP87A");

            if (!Directory.Exists(dir))
                return results;

            var files = Directory.GetFiles(dir, "*_Positions.csv");

            foreach (var file in files)
            {
                string planet = Path.GetFileName(file)
                    .Split('_')[0]
                    .ToUpperInvariant();

                if (!SupportedPlanets.Contains(planet))
                    continue;

                try
                {
                    var lines = File.ReadAllLines(file);

                    for (int i = 1; i < lines.Length; i++)
                    {
                        if (string.IsNullOrWhiteSpace(lines[i]))
                            continue;

                        var parts = lines[i].Split(',');

                        if (parts.Length < 4)
                            continue;

                        if (!double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double jd) ||
                            !double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double x) ||
                            !double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out double y) ||
                            !double.TryParse(parts[3], NumberStyles.Float, CultureInfo.InvariantCulture, out double z))
                            continue;

                        results.Add(
                            new TestCaseData(
                                planet,
                                jd,
                                x,
                                y,
                                z)
                            .SetName($"{planet}_VSOP87A_Pos_{jd}")
                        );
                    }
                }
                catch
                {
                    // bewusst schlucken – nur für Discovery-Stabilität
                }
            }

            Console.WriteLine($"TOTAL GENERATED CASES: {results.Count}");

            return results;
        }

        // ------------------------------------------------------------
        // VELOCITY TESTS
        // ------------------------------------------------------------

        public static IEnumerable<TestCaseData> VelocityCases()
        {
            var dir = GetVariantDirectory();

            if (!Directory.Exists(dir))
                yield break;

            var files = Directory.GetFiles(dir, "*_Velocity.csv");

            foreach (var file in files)
            {
                string planet = Path.GetFileName(file)
                    .Split('_')[0]
                    .ToUpperInvariant();

                if (!SupportedPlanets.Contains(planet))
                    continue;

                foreach (var testCase in LoadVelocityFile(file, planet))
                    yield return testCase;
            }
        }

        // ------------------------------------------------------------
        // CSV LOADERS (DISCOVERY-SAFE)
        // ------------------------------------------------------------

        private static IEnumerable<TestCaseData> LoadPositionFile(string file, string planet)
        {
            var lines = File.ReadAllLines(file);

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;

                var parts = lines[i].Split(',');

                if (parts.Length < 4)
                    continue;

                if (!double.TryParse(parts[0], NumberStyles.Float, Culture, out double jd) ||
                    !double.TryParse(parts[1], NumberStyles.Float, Culture, out double x) ||
                    !double.TryParse(parts[2], NumberStyles.Float, Culture, out double y) ||
                    !double.TryParse(parts[3], NumberStyles.Float, Culture, out double z))
                {
                    continue;
                }

                yield return new TestCaseData(
                        planet,
                        jd,
                        x,
                        y,
                        z)
                    .SetName($"{planet}_VSOP87A_Pos_{jd}");
            }
        }

        private static IEnumerable<TestCaseData> LoadVelocityFile(string file, string planet)
        {
            var lines = File.ReadAllLines(file);

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;

                var parts = lines[i].Split(',');

                if (parts.Length < 4)
                    continue;

                if (!double.TryParse(parts[0], NumberStyles.Float, Culture, out double jd) ||
                    !double.TryParse(parts[1], NumberStyles.Float, Culture, out double xd) ||
                    !double.TryParse(parts[2], NumberStyles.Float, Culture, out double yd) ||
                    !double.TryParse(parts[3], NumberStyles.Float, Culture, out double zd))
                {
                    continue;
                }

                yield return new TestCaseData(
                        planet,
                        jd,
                        xd,
                        yd,
                        zd)
                    .SetName($"{planet}_VSOP87A_Vel_{jd}");
            }
        }
    }
}
