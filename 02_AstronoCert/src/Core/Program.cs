using System;
using AstronoCert.Core;

namespace AstronoCert
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== AstronoCert ===");

            try
            {
                var runner = new AstronoCertRunner();
                runner.Run();

                Console.WriteLine("AstronoCert completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}