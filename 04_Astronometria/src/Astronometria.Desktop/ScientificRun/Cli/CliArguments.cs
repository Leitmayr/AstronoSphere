namespace Astronometria.ScientificRun.Cli
{
    public sealed class CliArguments
    {
        public bool ShowHelp { get; init; }

        public string? CatalogNumber { get; init; }

        public bool IsSingleExperimentRun => !string.IsNullOrWhiteSpace(CatalogNumber);


        public bool RunAll { get; init; }
    }
}