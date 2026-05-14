using System;
using System.IO;
using System.Linq;

namespace Astronometria.Core.ScientificRun.IO
{
    public static class ScientificRunFolderManager
    {
        public static string PrepareRunFolder(string repositoryRoot)
        {
            var baseFolder = Path.Combine(
                repositoryRoot,
                "AstronoData",
                "04_Simulations",
                "Scientific",
                "Ephemeris");

            var runFolder = Path.Combine(baseFolder, "Run");
            var lastRunFolder = Path.Combine(baseFolder, "LastRun");

            Directory.CreateDirectory(baseFolder);
            Directory.CreateDirectory(runFolder);
            Directory.CreateDirectory(lastRunFolder);

            RotateRunToLastRun(runFolder, lastRunFolder);

            Directory.CreateDirectory(runFolder);

            return runFolder;
        }

        private static void RotateRunToLastRun(string runFolder, string lastRunFolder)
        {
            foreach (var file in Directory.GetFiles(runFolder, "*.json").OrderBy(x => x, StringComparer.Ordinal))
            {
                var target = Path.Combine(lastRunFolder, Path.GetFileName(file));

                if (File.Exists(target))
                    File.Delete(target);

                File.Move(file, target);
            }

            foreach (var directory in Directory.GetDirectories(runFolder).OrderBy(x => x, StringComparer.Ordinal))
            {
                Directory.Delete(directory, recursive: true);
            }
        }
    }
}
