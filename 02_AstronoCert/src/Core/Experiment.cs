using System.Text.Json.Serialization;

namespace AstronoCert.Core
{
    public sealed class Experiment
    {
        public string SchemaVersion { get; set; } = "1.0";

        public string ExperimentID { get; set; }
        public string CatalogNumber { get; set; }
        public string CoreHash { get; set; }

        public CoreDefinition Core { get; set; }

        public object Event { get; set; }
        public object Metadata { get; set; }
        public object Notes { get; set; }

        [JsonIgnore]
        public object ScenarioCitation { get; set; }

        [JsonIgnore]
        public object DatasetHeader { get; set; }
    }
}