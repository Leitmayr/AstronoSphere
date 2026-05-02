// ============================================================
// FILE: Program.cs
// STATUS: UPDATE (M2.1 CatalogNumber direct run support)
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
                string? catalogNumber = ParseArgument(args, "--catalog");
                string? experimentId = ParseArgument(args, "--experiment");
                string? numericId = ParseArgument(args, "--id");

                if (catalogNumber == null && args.Length == 1 && args[0].StartsWith("AS-", StringComparison.OrdinalIgnoreCase))
                    catalogNumber = args[0];

                var runner = new FactoryRunner();

                if (!string.IsNullOrWhiteSpace(catalogNumber))
                {
                    Console.WriteLine($"Running single catalog number: {catalogNumber}");
                    runner.RunSingle(catalogNumber);
                }
                else if (!string.IsNullOrWhiteSpace(experimentId))
                {
                    Console.WriteLine($"Running single experiment/catalog argument: {experimentId}");
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