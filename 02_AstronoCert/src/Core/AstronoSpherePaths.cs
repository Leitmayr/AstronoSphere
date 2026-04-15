using System;
using System.IO;

namespace AstronoCert
{
    public static class AstronoSpherePaths
    {
        public static string GetRepoRoot()
        {
            var baseDir = AppContext.BaseDirectory;

            var dir = new DirectoryInfo(baseDir);

            while (dir != null && dir.Name != "AstronoSphere")
            {
                dir = dir.Parent;
            }

            if (dir == null)
                throw new Exception("AstronoSphere root not found.");

            return dir.FullName;
        }

        public static string GetAstronoDataRoot()
        {
            return Path.Combine(GetRepoRoot(), "AstronoData");
        }

        public static string GetSeedsPreparedFolder()
        {
            return Path.Combine(GetAstronoDataRoot(), "01_Seeds", "Prepared");
        }

        public static string GetSeedsProcessedFolder()
        {
            return Path.Combine(GetAstronoDataRoot(), "01_Seeds", "Processed");
        }

        public static string GetExperimentsReleasedFolder()
        {
            return Path.Combine(GetAstronoDataRoot(), "02_Experiments", "Released");
        }

        public static void PrintPaths()
        {
            Console.WriteLine("=== PATH DEBUG ===");
            Console.WriteLine($"RepoRoot   : {GetRepoRoot()}");
            Console.WriteLine($"DataRoot   : {GetAstronoDataRoot()}");
            Console.WriteLine($"Prepared   : {GetSeedsPreparedFolder()}");
            Console.WriteLine($"Processed  : {GetSeedsProcessedFolder()}");
            Console.WriteLine($"Experiments: {GetExperimentsReleasedFolder()}");
            Console.WriteLine("==================");
        }
    }
}