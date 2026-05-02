using System;
using System.Collections.Generic;

namespace AstronoDiag
{
    public sealed class DiagnosticRecord
    {
        public string Code { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public DiagnosticSeverity Severity { get; set; }

        public string Message { get; set; } = string.Empty;

        public string SourceSystem { get; set; } = "AstronoTruth";
        public string SubSourceSystem { get; set; } = "Horizons";

        public string InputObjectType { get; set; } = "Experiment";
        public string InputObjectId { get; set; } = string.Empty;
        public string CatalogNumber { get; set; } = string.Empty;

        public Dictionary<string, object> Details { get; set; } = new();

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}