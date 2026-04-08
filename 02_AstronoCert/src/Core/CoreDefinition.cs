using System.Text.Json.Serialization;

namespace AstronoCert.Core
{
    public sealed class CoreDefinition
    {
        [JsonPropertyName("Time")]
        public TimeDefinition Time { get; set; }

        [JsonPropertyName("Observer")]
        public ObserverDefinition Observer { get; set; }

        [JsonPropertyName("ObservedObject")]
        public ObservedObjectDefinition ObservedObject { get; set; }

        [JsonPropertyName("Frame")]
        public FrameDefinition Frame { get; set; }
    }

    public sealed class TimeDefinition
    {
        public double StartJD { get; set; }
        public double StopJD { get; set; }
        public string Step { get; set; }
        public string TimeScale { get; set; }
    }

    public sealed class ObserverDefinition
    {
        public string Type { get; set; }
        public string Body { get; set; }
    }

    public sealed class ObservedObjectDefinition
    {
        public string BodyClass { get; set; }
        public string[] Targets { get; set; }
    }

    public sealed class FrameDefinition
    {
        public string Type { get; set; }
        public string Epoch { get; set; }
    }
}