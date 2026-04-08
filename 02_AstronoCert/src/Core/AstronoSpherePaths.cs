using System.IO;

namespace AstronoCert
{
    public static class AstronoSpherePaths
    {
        public static string GetRepoRoot()
        {
            var current = Directory.GetCurrentDirectory();
            return Path.GetFullPath(Path.Combine(current, @"..\..\..\..\.."));
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
    }
}