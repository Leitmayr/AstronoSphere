// ============================================================
// FILE: Program.cs
// PROJECT: ScenarioHeaderGenerator
// STATUS: NEW
// ============================================================

using System;
using AstronoData.ScenarioCandidates;
using ScenarioHeaderGenerator;
using ScenarioHeaderGenerator.ScenarioCandidates;


namespace ScenarioHeaderGenerator.Core
{
    /// <summary>
    /// PURPOSE:
    /// Entry point for SHG pipeline pilot.
    ///
    /// CONTEXT:
    /// - Loads ScenarioCandidates from AstronoData
    /// - Runs ScenarioHeaderGenerator (SHG)
    /// - Writes scenarios to ObservationCatalog
    ///
    /// CONSTRAINTS:
    /// - No arguments
    /// - Deterministic execution
    /// - Fail fast on errors
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Scenario Header Generator (SHG) ===");
            
            try
            {
                // 1. Load candidates
                var loader = new ScenarioCandidateLoader();
                var candidates = loader.LoadAll();

                Console.WriteLine($"Loaded {candidates.Count} candidates.");

                // 2. Run SHG
                var shg = new ScenarioHeaderGenerator();
                shg.Run(candidates);

                Console.WriteLine("SHG completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR during SHG execution:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}