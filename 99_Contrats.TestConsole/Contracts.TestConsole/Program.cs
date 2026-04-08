using System;
using System.IO;
using System.Text.Json;
using AstronoData.Contracts.Hashing;

namespace Contracts.TestConsole
{
    class Program
    {
        static void Main()
        {
            IHashService hashService = new HashService();

            string path = @"C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData\01_Seeds\Prepared\SCN_000023.json";

            var json = File.ReadAllText(path);

            var root = JsonSerializer.Deserialize<Root>(json);

            var core = root.Core;

            var canonical = hashService.BuildCanonical(core);
            var hash = hashService.ComputeHash(canonical);

            Console.WriteLine("===== CANONICAL =====");
            Console.WriteLine(canonical);
            Console.WriteLine("=====================");

            Console.WriteLine($"CORE HASH: {hash}");
        }
    }

    public class Root
    {
        public Core Core { get; set; }
    }
}