// ============================================================
// FILE: 03_TruthFactory/src/EphemerisFactory/Core/AstronoSpherePaths.cs
// STATUS: UPDATE (M1.9 PATH FREEZE)
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

        // =====================================================
        // INPUT (M1.9 FINAL)
        // =====================================================

        public static string GetExperimentsReleasedFolder()
        {
            return Path.Combine(GetAstronoDataRoot(), "02_Experiments", "Released");
        }

        // =====================================================
        // OUTPUT (M1.9 FINAL)
        // =====================================================

        public static string GetGroundTruthRunFolder()
        {
            return Path.Combine(
                GetAstronoDataRoot(),
                "03_GroundTruth",
                "Ephemeris",
                "Horizons",
                "Run");
        }

        public static string GetGroundTruthLastRunFolder()
        {
            return Path.Combine(
                GetAstronoDataRoot(),
                "03_GroundTruth",
                "Ephemeris",
                "Horizons",
                "LastRun");
        }

        public static void PrintPaths()
        {
            Console.WriteLine("=== PATH DEBUG ===");
            Console.WriteLine($"AstronoData Root : {GetAstronoDataRoot()}");
            Console.WriteLine($"Experiments      : {GetExperimentsReleasedFolder()}");
            Console.WriteLine($"Run              : {GetGroundTruthRunFolder()}");
            Console.WriteLine($"LastRun          : {GetGroundTruthLastRunFolder()}");
            Console.WriteLine("==================");
        }
    }
}