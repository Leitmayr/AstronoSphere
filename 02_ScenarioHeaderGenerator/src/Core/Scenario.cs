// ============================================================
// FILE: Scenario.cs
// ============================================================

using ScenarioHeaderGenerator.ScenarioCandidates;

namespace ScenarioHeaderGenerator.Core
{
    public sealed class Scenario
    {
        public string SchemaVersion { get; set; } = "1.0";

        public string ScenarioID { get; set; }
        public string CatalogNumber { get; set; }
        public string CoreHash { get; set; }

        public CoreDefinition Core { get; set; }
    }
}