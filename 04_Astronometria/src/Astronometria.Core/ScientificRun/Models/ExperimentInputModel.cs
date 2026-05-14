using System.Collections.Generic;

namespace Astronometria.Core.ScientificRun.Models
{
    public sealed class ExperimentInputModel
    {
        public string SchemaVersion { get; set; } = string.Empty;

        public string ExperimentID { get; set; } = string.Empty;

        public string CatalogNumber { get; set; } = string.Empty;

        public string CoreHash { get; set; } = string.Empty;

        public ExperimentCoreModel Core { get; set; } = new();

        public ExperimentEventModel Event { get; set; } = new();

        public ExperimentMetadataModel Metadata { get; set; } = new();

        public string Notes { get; set; } = string.Empty;

        public string SourceFile { get; set; } = string.Empty;
    }

    public sealed class ExperimentCoreModel
    {
        public ExperimentTimeModel Time { get; set; } = new();

        public ExperimentObserverModel Observer { get; set; } = new();

        public ExperimentObservedObjectModel ObservedObject { get; set; } = new();

        public ExperimentFrameModel Frame { get; set; } = new();
    }

    public sealed class ExperimentTimeModel
    {
        public double StartJD { get; set; }

        public double StopJD { get; set; }

        public string Step { get; set; } = string.Empty;

        public string TimeScale { get; set; } = string.Empty;
    }

    public sealed class ExperimentObserverModel
    {
        public string Type { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;
    }

    public sealed class ExperimentObservedObjectModel
    {
        public string BodyClass { get; set; } = string.Empty;

        public List<string> Targets { get; set; } = new();
    }

    public sealed class ExperimentFrameModel
    {
        public string Type { get; set; } = string.Empty;

        public string Epoch { get; set; } = string.Empty;
    }

    public sealed class ExperimentEventModel
    {
        public string Category { get; set; } = string.Empty;

        public string Qualifier { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }

    public sealed class ExperimentMetadataModel
    {
        public string Author { get; set; } = string.Empty;

        public int Priority { get; set; }

        public ExperimentStatusModel Status { get; set; } = new();
    }

    public sealed class ExperimentStatusModel
    {
        public string Maturity { get; set; } = string.Empty;

        public string Visibility { get; set; } = string.Empty;
    }
}