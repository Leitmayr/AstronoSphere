using System;
using System.IO;
using System.Linq;

namespace Astronometria.Core.ScientificRun.Diagnostics
{
    public static class ScientificRunDiagnosticFolderManager
    {
        public static string PrepareRunFolder(
            string repositoryRoot,
            bool rotateExistingRun)
        {
            var baseFolder = Path.Combine(
                repositoryRoot,
                "AstronoData",
                "04_Simulations",
                "DiagMessages");

            var runFolder = Path.Combine(baseFolder, "Run");
            var lastRunFolder = Path.Combine(baseFolder, "LastRun");

            Directory.CreateDirectory(baseFolder);
            Directory.CreateDirectory(runFolder);
            Directory.CreateDirectory(lastRunFolder);

            if (rotateExistingRun)
            {
                CopyRunToLastRunAndClearRun(
                    runFolder,
                    lastRunFolder);
            }

            return runFolder;
        }

        private static void CopyRunToLastRunAndClearRun(
            string runFolder,
            string lastRunFolder)
        {
            var runFiles = Directory
                .GetFiles(runFolder, "*.json")
                .OrderBy(x => x, StringComparer.Ordinal)
                .ToList();

            foreach (var file in runFiles)
            {
                var target = Path.Combine(
                    lastRunFolder,
                    Path.GetFileName(file));

                File.Copy(
                    file,
                    target,
                    overwrite: true);
            }

            foreach (var file in runFiles)
            {
                File.Delete(file);
            }
        }
    }
}