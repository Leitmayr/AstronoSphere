// ============================================================
// FILE: 03_TruthFactory/src/EphemerisFactory/Core/AstronoSpherePaths.cs
// STATUS: ÄNDERUNG (Rollback + minimal Fix)
// ============================================================

using System;
using System.IO;

namespace EphemerisFactory.Core
{
    public static class AstronoSpherePaths
    {
        public static string GetAstronoDataRoot()
        {
            var baseDir = AppContext.BaseDirectory;

            var root = Path.GetFullPath(
                Path.Combine(baseDir, @"..\..\..\..\..\..\"));

            return Path.Combine(root, "AstronoData");
        }

        public static string GetReferenceDataRoot()
        {
            return Path.Combine(GetAstronoDataRoot(), "03_ReferenceData");
        }

        // =====================================================
        // FIX: "Runs" ergänzt (einzige echte Korrektur)
        // =====================================================

        public static string GetReferenceDataRunRoot()
        {
            return Path.Combine(GetReferenceDataRoot(), "Runs", "Run");
        }

        public static string GetReferenceDataLastRunRoot()
        {
            return Path.Combine(GetReferenceDataRoot(), "Runs", "LastRun");
        }

        // =====================================================
        // WICHTIG: bestehende Methoden NICHT anfassen
        // =====================================================

        public static string GetObservationCatalogReleasedFolder()
        {
            return Path.Combine(GetAstronoDataRoot(), "02_ObservationCatalog", "Released");
        }

        public static void PrintPaths()
        {
            Console.WriteLine("=== PATH DEBUG ===");
            Console.WriteLine($"AstronoData Root : {GetAstronoDataRoot()}");
            Console.WriteLine($"ReferenceData    : {GetReferenceDataRoot()}");
            Console.WriteLine($"Run              : {GetReferenceDataRunRoot()}");
            Console.WriteLine($"LastRun          : {GetReferenceDataLastRunRoot()}");
            Console.WriteLine("==================");
        }
    }
}