using System;
using System.IO;
using NUnit.Framework;

namespace Astronometria.Ephemerides.Test
{
    internal static class SolutionPathResolver
    {
        public static string GetSolutionRoot()
        {
            var dir = new DirectoryInfo(TestContext.CurrentContext.TestDirectory);

            while (dir != null)
            {
                var sln = Path.Combine(dir.FullName, "Astronometria.sln");
                if (File.Exists(sln))
                    return dir.FullName;

                dir = dir.Parent;
            }

            throw new InvalidOperationException("Solution root not found.");
        }
    }
}