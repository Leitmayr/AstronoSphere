namespace Astronometria.Core.ScientificRun.Build
{
    public sealed class BuildInfo
    {
        public string GitCommit { get; init; } = string.Empty;

        public string GitBranch { get; init; } = string.Empty;

        public string RepositoryRoot { get; init; } = string.Empty;
    }
}
