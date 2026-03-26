using System;
using System.IO;

namespace EphemerisFactory.Core
{
    public static class AstronoSpherePaths
    {
        // =========================================================
        // ROOT RESOLUTION
        // =========================================================

        public static string GetRepoRoot()
        {
            var current = Directory.GetCurrentDirectory();

            var dir = new DirectoryInfo(current);

            while (dir != null)
            {
                if (Directory.Exists(Path.Combine(dir.FullName, "AstronoData")))
                {
                    return dir.FullName;
                }

                dir = dir.Parent;
            }

            throw new Exception("AstronoSphere root not found (AstronoData folder missing).");
        }

        // =========================================================
        // CORE ROOTS
        // =========================================================

        public static string GetAstronoDataRoot()
        {
            return Path.Combine(GetRepoRoot(), "AstronoData");
        }

        // =========================================================
        // OBSERVATION CATALOG
        // =========================================================

        public static string GetObservationCatalogRoot()
        {
            return Path.Combine(GetAstronoDataRoot(), "02_ObservationCatalog");
        }

        public static string GetObservationCatalogReleasedFolder()
        {
            return Path.Combine(GetObservationCatalogRoot(), "Released");
        }

        public static string GetObservationCatalogCreatedFolder()
        {
            return Path.Combine(GetObservationCatalogRoot(), "Created");
        }

        // =========================================================
        // REFERENCE DATA
        // =========================================================

        public static string GetReferenceDataRoot()
        {
            return Path.Combine(GetAstronoDataRoot(), "03_ReferenceData");
        }

        public static string GetReferenceDataRunRoot()
        {
            return Path.Combine(GetReferenceDataRoot(), "Run");
        }

        public static string GetReferenceDataRunFolder(string level)
        {
            return Path.Combine(GetReferenceDataRunRoot(), level);
        }

        public static string GetReferenceDataBaselineFolder(string level)
        {
            return Path.Combine(GetReferenceDataRoot(), "Baseline", level);
        }

        public static string GetReferenceDataLastRunFolder(string level)
        {
            return Path.Combine(GetReferenceDataRoot(), "LastRun", level);
        }

        // =========================================================
        // DEBUG HELPER (optional, aber extrem nützlich)
        // =========================================================

        public static void PrintPaths()
        {
            Console.WriteLine("=== AstronoSphere Paths ===");
            Console.WriteLine($"RepoRoot: {GetRepoRoot()}");
            Console.WriteLine($"AstronoData: {GetAstronoDataRoot()}");
            Console.WriteLine($"ObservationCatalog/Released: {GetObservationCatalogReleasedFolder()}");
            Console.WriteLine($"ReferenceData/Run: {GetReferenceDataRunRoot()}");
            Console.WriteLine("===========================");
        }
    }
}