// ============================================================
// FILE: Program.cs
// STATUS: UPDATE
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
                var runner = new FactoryRunner();
                runner.Run();

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
    }
}