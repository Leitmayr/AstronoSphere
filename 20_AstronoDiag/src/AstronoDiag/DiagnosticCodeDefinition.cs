namespace AstronoDiag
{
    public sealed class DiagnosticCodeDefinition
    {
        public string Code { get; }
        public string Symbol { get; }
        public DiagnosticSeverity Severity { get; }

        public DiagnosticCodeDefinition(
            string code,
            string symbol,
            DiagnosticSeverity severity)
        {
            Code = code;
            Symbol = symbol;
            Severity = severity;
        }
    }
}