namespace AstronoDiag
{
    public static class DiagnosticCatalog
    {
        public static readonly DiagnosticCodeDefinition AstronoTruthInvalidMaturity =
            new("030.003", "AstronoTruth.InvalidMaturity", DiagnosticSeverity.Warning);

        public static readonly DiagnosticCodeDefinition AstronoTruthProviderRangeViolation =
            new("030.005", "AstronoTruth.ProviderRangeViolation", DiagnosticSeverity.Warning);

        public static readonly DiagnosticCodeDefinition AstronoTruthRequestFailed =
            new("030.006", "AstronoTruth.RequestFailed", DiagnosticSeverity.Error);

        public static readonly DiagnosticCodeDefinition AstronoTruthParseFailed =
            new("030.007", "AstronoTruth.ParseFailed", DiagnosticSeverity.Error);
    }
}