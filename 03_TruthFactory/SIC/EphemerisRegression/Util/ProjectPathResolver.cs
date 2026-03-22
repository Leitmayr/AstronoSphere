using System;
using System.IO;

namespace EphemerisRegression.Util
{
    public static class ProjectPathResolver
    {
        public static string GetSolutionRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);

            // Suche nach Projektordner "EphemerisRegression"
            while (dir != null && dir.Name != "EphemerisRegression")
            {
                dir = dir.Parent;
            }

            if (dir == null)
                throw new InvalidOperationException("Project root not found.");

            // Eine Ebene höher = Solution Root
            return dir.Parent!.FullName;
        }
    }
}
