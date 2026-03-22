using System.Collections.Generic;
using System.Linq;
using Astronometria.Data.Models;

namespace Astronometria.Data.Parsers.BrightStarCatalog
{
    public static class StarCatalogLoader
    {
        public static List<StarRecord> LoadToList(string filePath)
        {
            var parser = new BrightStarCatalogParser();
            return parser.Parse(filePath).ToList();
        }
    }
}
