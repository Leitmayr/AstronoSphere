// ============================================================
// FILE: /Domain/GeoDecObserverNodeReferenceModel.cs
// STATUS: NEU
// ============================================================

namespace EphemerisRegression.Domain
{
    public sealed class GeoDecObserverNodeReferenceModel
    {
        public string Planet { get; init; } = "";
        public string TestSuite { get; init; } = "";
        public string EventName { get; init; } = "";
        public string CorrectionLevel { get; init; } = "L0";

        public object Metadata { get; init; } = default!;

        public ObserverNodeEvent Node { get; init; } = default!;
    }

    public sealed class ObserverNodeEvent
    {
        public double JulianDate { get; init; }

        public ObserverState Before { get; init; } = default!;
        public ObserverState At { get; init; } = default!;
        public ObserverState After { get; init; } = default!;
    }

    public sealed class ObserverState
    {
        public double JulianDate { get; init; }
        public double Ra { get; init; }
        public double Dec { get; init; }
    }
}