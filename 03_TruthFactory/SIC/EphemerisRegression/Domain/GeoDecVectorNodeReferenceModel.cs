// ============================================================
// FILE: /Domain/GeoDecVectorNodeReferenceModel.cs
// STATUS: NEU
// ============================================================

using EphemerisRegression.Event;

namespace EphemerisRegression.Domain
{
    public sealed class GeoDecVectorNodeReferenceModel
    {
        public string Planet { get; init; } = "";
        public string TestSuite { get; init; } = "";
        public string EventName { get; init; } = "";
        public string CorrectionLevel { get; init; } = "L0";

        public object Metadata { get; init; } = default!;

        public NodeEvent Node { get; init; } = default!;
    }
}