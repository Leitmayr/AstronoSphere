using System;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Astronometria.Core.Coordinates;
using Astronometria.Data.Models;

namespace Astronometria.Data.Parsers.Constellations
{
    /// <summary>
    /// Parser für CSV:
    /// constellation, ra1_deg, dec1_deg, ra2_deg, dec2_deg
    /// mit Punkt als Dezimaltrennzeichen (InvariantCulture).
    /// </summary>
    public sealed class ConstellationLineCsvParser : IDataParser<ConstellationLineRecord>
    {
        public IEnumerable<ConstellationLineRecord> Parse(string filePath)
        {
            var culture = CultureInfo.InvariantCulture;

            int lineNo = 0;
            foreach (var raw in File.ReadLines(filePath))
            {
                lineNo++;

                if (lineNo == 1) // Header
                    continue;

                var line = raw.Trim();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(',');
                if (parts.Length < 5)
                    continue;

                string constellation = parts[0].Trim();

                if (!double.TryParse(parts[1], NumberStyles.Float, culture, out var ra1)) continue;
                if (!double.TryParse(parts[2], NumberStyles.Float, culture, out var dec1)) continue;
                if (!double.TryParse(parts[3], NumberStyles.Float, culture, out var ra2)) continue;
                if (!double.TryParse(parts[4], NumberStyles.Float, culture, out var dec2)) continue;

                yield return new ConstellationLineRecord
                {
                    ConstellationIAU3 = constellation,
                    Start = new EquatorialCoord(ra1, dec1),
                    End = new EquatorialCoord(ra2, dec2)
                };
            }
        }
    }
}
