// ============================================================
// FILE: Program.cs
// STATUS: UPDATE (Single Run Support)
// ============================================================

using System;
using EphemerisFactory.Core;

namespace EphemerisFactory
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== EphemerisFactory v1 ===");

            try
            {
                string? experimentId = ParseArgument(args, "--experiment");
                string? numericId = ParseArgument(args, "--id");

                var runner = new FactoryRunner();

                if (!string.IsNullOrWhiteSpace(experimentId))
                {
                    Console.WriteLine($"Running single experiment: {experimentId}");
                    runner.RunSingle(experimentId);
                }
                else if (!string.IsNullOrWhiteSpace(numericId))
                {
                    Console.WriteLine($"Running single experiment (numeric): {numericId}");
                    runner.RunSingleByNumber(int.Parse(numericId));
                }
                else
                {
                    runner.Run();
                }

                Console.WriteLine("Factory completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR during Factory execution:");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Press any key to exit...");
            //Console.ReadKey();
        }

        private static string? ParseArgument(string[] args, string key)
        {
            for (int i = 0; i < args.Length - 1; i++)
            {
                if (args[i].Equals(key, StringComparison.OrdinalIgnoreCase))
                    return args[i + 1];
            }
            return null;
        }
    }
}