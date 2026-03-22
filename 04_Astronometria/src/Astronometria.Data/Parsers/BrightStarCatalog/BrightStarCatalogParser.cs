using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Astronometria.Data.Models;

namespace Astronometria.Data.Parsers.BrightStarCatalog
{
    public sealed class BrightStarCatalogParser : IDataParser<StarRecord>
    {
        public IEnumerable<StarRecord> Parse(string filePath)
        {
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
                
                StarRecord? star = null;
                
                try
                {
                    star = new StarRecord
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

                        RightAscensionDeg = double.Parse(f[21], culture),
                        DeclinationDeg = double.Parse(f[23], culture)
                    };

                 
                }
                catch
                {
                    // bewusst still: wie vorher
                }

                if (star != null)
                {
                    yield return star;
                }
            }
        }
    }
}
