using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace Astronometria.Core.ScientificRun.Build
{
    public static class BuildInfoService
    {
        private const string Unknown = "UNKNOWN";

        public static BuildInfo CreateOrUpdateBuildInfo()
        {
            var repositoryRoot = FindRepositoryRoot(AppContext.BaseDirectory);

            var buildInfo = new BuildInfo
            {
                GitCommit = RunGit(repositoryRoot, "rev-parse --short HEAD"),
                GitBranch = RunGit(repositoryRoot, "rev-parse --abbrev-ref HEAD"),
                RepositoryRoot = repositoryRoot
            };

            var path = Path.Combine(repositoryRoot, "buildinfo.json");

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            File.WriteAllText(path, JsonSerializer.Serialize(buildInfo, options));

            return buildInfo;
        }

        private static string FindRepositoryRoot(string startDirectory)
        {
            var directory = new DirectoryInfo(startDirectory);

            while (directory != null)
            {
                if (Directory.Exists(Path.Combine(directory.FullName, ".git")))
                    return directory.FullName;

                if (Directory.Exists(Path.Combine(directory.FullName, "AstronoData")))
                    return directory.FullName;

                directory = directory.Parent;
            }

            throw new DirectoryNotFoundException(
                $"Could not determine repository root from '{startDirectory}'.");
        }

        private static string RunGit(string workingDirectory, string arguments)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = arguments,
                    WorkingDirectory = workingDirectory,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = Process.Start(startInfo);

                if (process == null)
                    return Unknown;

                var output = process.StandardOutput.ReadToEnd().Trim();
                process.WaitForExit();

                if (process.ExitCode != 0 || string.IsNullOrWhiteSpace(output))
                    return Unknown;

                return output;
            }
            catch
            {
                return Unknown;
            }
        }
    }
}