namespace EphemerisRegression.Event
{
    public sealed class HelioEvent
    {
        public string Planet { get; init; } = string.Empty;

        public int CommandCode { get; init; }

        public string TestSuite { get; init; } = string.Empty;

        public string EventName { get; init; } = string.Empty;

        public double JulianDate { get; init; }

        public int WindowDays { get; init; }

        public string StepSize { get; init; } = "1h";
    }
}
