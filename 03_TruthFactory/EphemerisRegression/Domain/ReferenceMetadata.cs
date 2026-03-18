using System;
using System.Collections.Generic;

namespace EphemerisRegression.Domain
{
    public sealed class ReferenceMetadata
    {
        public string? CanonicalRequest { get; set; }
        public string? RequestHash { get; set; }

        public string? EpochHash { get; set; }

        public List<RequestInfo>? Requests { get; set; }

        public string HorizonsUrl { get; set; } = "";
        public string EngineVersion { get; set; } = "";
        public string CorrectionLevel { get; set; } = "";
        public string Mode { get; set; } = "";
        public string EphemType { get; set; } = "";

        public DateTime GeneratedAtUtc { get; set; }
    }

    public sealed class RequestInfo
    {
        public string CanonicalRequest { get; set; } = "";
        public string RequestHash { get; set; } = "";
        public string HorizonsUrl { get; set; } = "";
    }
}