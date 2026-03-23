// ============================================================
// FILE: AstronoSpherePaths.cs
// STATUS: NEW
// ============================================================

using System.IO;

namespace EphemerisFactory.Core
{
    public static class AstronoSpherePaths
    {
        public static string GetRepoRoot()
        {
            var current = Directory.GetCurrentDirectory();

            // von bin/Debug/... hoch bis AstronoSphere
            return Path.GetFullPath(Path.Combine(current, @"..\..\..\..\..\.."));
        }

        public static string GetAstronoDataRoot()
        {
            return Path.Combine(GetRepoRoot(), "AstronoData");
        }

        public static string GetObservationCatalogReleasedFolder()
        {
            return Path.Combine(
                GetAstronoDataRoot(),
                "02_ObservationCatalog",
                "Released");
        }
    }
}