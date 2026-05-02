// ============================================================
// FILE: 20_AstronoDiag/src/AstronoDiag/DiagnosticRunFolderManager.cs
// STATUS: UPDATE (Fix: LastRun must NEVER be deleted)
// ============================================================

using System.IO;

namespace AstronoDiag
{
    public static class DiagnosticRunFolderManager
    {
        public static void ResetRunToLastRun(string runFolder, string lastRunFolder)
        {
            Directory.CreateDirectory(runFolder);
            Directory.CreateDirectory(lastRunFolder);

            // 1) Copy Run → LastRun (overwrite allowed)
            foreach (var file in Directory.GetFiles(runFolder))
            {
                var name = Path.GetFileName(file);

                if (name == ".gitkeep")
                    continue;

                File.Copy(file, Path.Combine(lastRunFolder, name), true);
            }

            // 2) Clear Run (LastRun untouched!)
            foreach (var file in Directory.GetFiles(runFolder))
            {
                var name = Path.GetFileName(file);

                if (name == ".gitkeep")
                    continue;

                File.Delete(file);
            }
        }
    }
}