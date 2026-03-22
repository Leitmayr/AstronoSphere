// ============================================================
// FILE: AstronoData/ScenarioCandidates/ScenarioCandidate.cs
// STATUS: UPDATE
// ============================================================

using System.Text.Json.Serialization;

namespace AstronoData.ScenarioCandidates
{
    /// <summary>
    /// PURPOSE:
    /// Raw scenario candidate as provided by scenario generators (Meeus etc.)
    ///
    /// CONTEXT:
    /// - Input to ScenarioHeaderGenerator (SHG)
    /// - Contains Event (metadata) + Core (physical experiment)
    ///
    /// CONSTRAINTS:
    /// - No logic
    /// - Must match JSON exactly
    /// </summary>
    public sealed class ScenarioCandidate
    {
        [JsonPropertyName("Event")]
        public EventDefinition Event { get; set; }

        [JsonPropertyName("Core")]
        public CoreDefinition Core { get; set; }
    }

    public sealed class EventDefinition
    {
        public string Category { get; set; }
        public double ApproximateJD { get; set; }
        public string Source { get; set; }
        public string Comment { get; set; }
    }

    public sealed class CoreDefinition
    {
        public TimeDefinition Time { get; set; }
        public ObserverDefinition Observer { get; set; }
        public string[] Targets { get; set; }
        public FrameDefinition Frame { get; set; }
        public CorrectionsDefinition Corrections { get; set; }
    }

    public sealed class TimeDefinition
    {
        public double StartJD { get; set; }
        public double StopJD { get; set; }
        public double StepDays { get; set; }
        public string TimeScale { get; set; }
    }

    public sealed class ObserverDefinition
    {
        public string Type { get; set; }
        public string Body { get; set; }
        public LocationDefinition Location { get; set; }
    }

    public sealed class LocationDefinition
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
        public double? Elevation { get; set; }
        public string SiteName { get; set; }
    }

    public sealed class FrameDefinition
    {
        public string Type { get; set; }
        public string Epoch { get; set; }
    }

    public sealed class CorrectionsDefinition
    {
        public bool LightTime { get; set; }
        public bool Aberration { get; set; }
        public bool Precession { get; set; }
        public bool Nutation { get; set; }
    }
}