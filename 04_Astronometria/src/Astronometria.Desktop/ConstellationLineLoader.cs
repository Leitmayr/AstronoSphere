using System.Collections.Generic;
using System.Linq;
using Astronometria.Data.Parsers.Constellations;

namespace Astronometria
{
    public static class ConstellationLineLoader
    {
        public static Dictionary<string, List<EquatorialLine>> Load(string filePath)
        {
            var parser = new ConstellationLineCsvParser();

            return parser.Parse(filePath)
                .GroupBy(r => r.ConstellationIAU3)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => new EquatorialLine(x.Start, x.End)).ToList());
        }
    }
}