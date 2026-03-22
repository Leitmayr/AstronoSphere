// ============================================================
// FILE: AstronoSpherePaths.cs
// STATUS: UPDATE
// ============================================================

using System.IO;

namespace ScenarioHeaderGenerator
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

        public static string GetCandidateReleasedFolder()
        {
            return Path.Combine(GetAstronoDataRoot(), "01_CandidateData", "Released");
        }

        public static string GetObservationCatalogCreatedFolder()
        {
            return Path.Combine(GetAstronoDataRoot(), "02_ObservationCatalog", "Created");
        }

        public static string GetObservationCatalogReleasedFolder()
        {
            return Path.Combine(GetAstronoDataRoot(), "02_ObservationCatalog", "Released");
        }
    }
}