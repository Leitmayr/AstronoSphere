using System;
using System.IO;

namespace AstronoLab
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var inputFolder = @"C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData\01_Seeds\Incoming";
            var outputFolder = @"C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData\01_Seeds\Prepared";

            if (args.Length == 1)
            {
                // 👉 Single File Mode (z.B. SCN_000023)
                var file = Path.Combine(inputFolder, args[0] + ".json");

                SeedToExperimentConverter.RunSingle(file, outputFolder);
            }
            else
            {
                // 👉 Default: alle
                SeedToExperimentConverter.Run(inputFolder, outputFolder);
            }
        }
    }
}