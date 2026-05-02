using System;
using System.IO;

namespace AstronoLab
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var inputFolder = Path.Combine(GetAstronoDataRoot(), "01_Seeds", "Incoming");
            var outputFolder = Path.Combine(GetAstronoDataRoot(), "01_Seeds", "Prepared");

            if (args.Length == 1 && args[0].Equals("--meshgen", StringComparison.OrdinalIgnoreCase))
            {
                MeshGenRunner.Run(inputFolder, outputFolder, MeshGenRunMode.Full);
                return;
            }

            if (args.Length == 1 && args[0].Equals("--meshgen-gmss", StringComparison.OrdinalIgnoreCase))
            {
                MeshGenRunner.Run(inputFolder, outputFolder, MeshGenRunMode.Gmss);
                return;
            }

            if (args.Length == 1)
            {
                var file = Path.Combine(inputFolder, args[0] + ".json");
                SeedToExperimentConverter.RunSingle(file, outputFolder);
                return;
            }

            SeedToExperimentConverter.Run(inputFolder, outputFolder);
        }

        private static string GetRepoRoot()
        {
            var baseDir = AppContext.BaseDirectory;
            var dir = new DirectoryInfo(baseDir);

            while (dir != null && dir.Name != "AstronoSphere")
            {
                dir = dir.Parent;
            }

            if (dir == null)
                throw new Exception("AstronoSphere root not found.");

            return dir.FullName;
        }

        private static string GetAstronoDataRoot()
        {
            return Path.Combine(GetRepoRoot(), "AstronoData");
        }
    }
}