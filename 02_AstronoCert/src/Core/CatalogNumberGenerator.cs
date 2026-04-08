// ============================================================
// FILE: CatalogNumberGenerator.cs
// STATUS: UPDATE (v3.1)
// ============================================================

using System.IO;
using System.Linq;

namespace AstronoCert
{
    public static class CatalogNumberGenerator
    {
        public static int GetNextStart(string releasedFolder)
        {
            if (!Directory.Exists(releasedFolder))
                return 1;

            var files = Directory.GetFiles(releasedFolder, "AS-*.json");

            if (!files.Any())
                return 1;

            var max = files
                .Select(f => Path.GetFileNameWithoutExtension(f))
                .Select(name => name.Replace("AS-", ""))
                .Select(int.Parse)
                .Max();

            return max + 1;
        }
    }
}