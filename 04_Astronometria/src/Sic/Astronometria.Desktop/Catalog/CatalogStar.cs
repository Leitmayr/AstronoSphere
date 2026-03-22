using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Astronometria
{
    public struct CatalogStar
    {
        public int HarvardRevisedNumber;   // HR
        public string Name;
        public string HD;
        public double VisualMagnitude;
        public string SpectralTypeShort;
        public string ConstellationShort;
        public string ConstellationLong;
        public string ConstellationGerman;
        public string GreekLetter;
        public double RAdeg;            //deg in grad
        public double DECdeg;           //deg in grad
    }



    public static class StarCatalog
         
    {

        private static List<CatalogStar> _allStars;

        public static IReadOnlyList<CatalogStar> AllStars => _allStars;

        public static void Load(string filePath)
        {
            _allStars = new List<CatalogStar>();

            var culture = CultureInfo.InvariantCulture;


            foreach (var rawLine in File.ReadLines(filePath))
            {
                var line = rawLine.Trim();

                // Kommentare und leere Zeilen überspringen
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (line.StartsWith("//")) continue;
                if (!line.StartsWith("{")) continue;

                // { ... },
                line = line.TrimStart('{').TrimEnd('}', ',');

                // Spalten trennen
                string[] f = line.Split(',');

                try
                {
                    var star = new CatalogStar
                    {
                        HarvardRevisedNumber = int.Parse(f[1].Trim().Trim('"')),
                        Name = f[2].Trim().Trim('"'),
                        HD = f[3].Trim().Trim('"'),

                        VisualMagnitude = double.Parse(f[6], culture),

                        SpectralTypeShort = f[9].Trim().Trim('"'),

                        ConstellationShort = f[10].Trim().Trim('"'),
                        ConstellationLong = f[11].Trim().Trim('"'),
                        ConstellationGerman = f[12].Trim().Trim('"'),

                        GreekLetter = f[14].Trim().Trim('"'),
                        // ✅ KORREKT
                        RAdeg = double.Parse(f[21], culture),
                        DECdeg = double.Parse(f[23], culture)
                    };

                    _allStars.Add(star);
                }
                catch (Exception ex)
                {
                    // optional: Debug-Ausgabe
                    // Debug.WriteLine("Fehlerhafte Zeile: " + ex.Message);
                }
            }
        }
    }
}
