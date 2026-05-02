// ============================================================
// FILE: 03_AstronoTruth/src/EphemerisFactory/Core/AstronoSpherePaths.cs
// STATUS: UPDATE (M2.1 AstronoDiag paths)
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

        public static string GetExperimentsReleasedFolder()
        {
            return Path.Combine(GetAstronoDataRoot(), "02_Experiments", "Released");
        }

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

        public static string GetGroundTruthDiagMessagesRunFolder()
        {
            return Path.Combine(
                GetAstronoDataRoot(),
                "03_GroundTruth",
                "DiagMessages",
                "Run");
        }

        public static string GetGroundTruthDiagMessagesLastRunFolder()
        {
            return Path.Combine(
                GetAstronoDataRoot(),
                "03_GroundTruth",
                "DiagMessages",
                "LastRun");
        }

        public static void PrintPaths()
        {
            Console.WriteLine("=== PATH DEBUG ===");
            Console.WriteLine($"AstronoData Root       : {GetAstronoDataRoot()}");
            Console.WriteLine($"Experiments            : {GetExperimentsReleasedFolder()}");
            Console.WriteLine($"GroundTruth Run        : {GetGroundTruthRunFolder()}");
            Console.WriteLine($"GroundTruth LastRun    : {GetGroundTruthLastRunFolder()}");
            Console.WriteLine($"DiagMessages Run       : {GetGroundTruthDiagMessagesRunFolder()}");
            Console.WriteLine($"DiagMessages LastRun   : {GetGroundTruthDiagMessagesLastRunFolder()}");
            Console.WriteLine("==================");
        }
    }
}