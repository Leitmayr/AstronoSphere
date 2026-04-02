// ============================================================
// FILE: CoreCanonicalizer.cs
// STATUS: UPDATED (RC1 – G17 precision sync)
// ============================================================

using AstronoData.ScenarioCandidates;
using ScenarioHeaderGenerator.ScenarioCandidates;
using System.Globalization;
using System.Text;

namespace ScenarioHeaderGenerator
{
    public static class CoreCanonicalizer
    {
        private static string F(double v) =>
            v.ToString("G17", CultureInfo.InvariantCulture);

        public static string ToCanonicalJson(CoreDefinition core)
        {
            var sb = new StringBuilder();

            sb.Append("{");

            // Time
            sb.Append("\"Time\":{");
            sb.Append($"\"StartJD\":{F(core.Time.StartJD)},");
            sb.Append($"\"StopJD\":{F(core.Time.StopJD)},");
            sb.Append($"\"StepDays\":\"{core.Time.StepDays}\",");
            sb.Append($"\"TimeScale\":\"{core.Time.TimeScale}\"");
            sb.Append("},");

            // Observer
            sb.Append("\"Observer\":{");
            sb.Append($"\"Type\":\"{core.Observer.Type}\",");
            sb.Append($"\"Body\":\"{core.Observer.Body}\",");

            sb.Append("\"Location\":{");
            sb.Append($"\"Lat\":{F(core.Observer.Location.Lat)},");
            sb.Append($"\"Lon\":{F(core.Observer.Location.Lon)},");
            sb.Append($"\"Elevation\":{F(core.Observer.Location.Elevation ?? 0.0)},");
            sb.Append($"\"SiteName\":\"{core.Observer.Location.SiteName}\"");
            sb.Append("}");

            sb.Append("},");

            // Targets
            sb.Append("\"Targets\":[");
            for (int i = 0; i < core.Targets.Length; i++)
            {
                if (i > 0) sb.Append(",");
                sb.Append($"\"{core.Targets[i]}\"");
            }
            sb.Append("],");

            // Frame
            sb.Append("\"Frame\":{");
            sb.Append($"\"Type\":\"{core.Frame.Type}\",");
            sb.Append($"\"Epoch\":\"{core.Frame.Epoch}\"");
            sb.Append("},");

            // Corrections
            sb.Append("\"Corrections\":{");
            sb.Append($"\"LightTime\":{core.Corrections.LightTime.ToString().ToLower()},");
            sb.Append($"\"Aberration\":{core.Corrections.Aberration.ToString().ToLower()},");
            sb.Append($"\"Precession\":{core.Corrections.Precession.ToString().ToLower()},");
            sb.Append($"\"Nutation\":{core.Corrections.Nutation.ToString().ToLower()}");
            sb.Append("}");

            sb.Append("}");

            return sb.ToString();
        }
    }
}