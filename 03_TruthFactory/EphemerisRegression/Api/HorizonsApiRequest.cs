// ============================================================
// FILE: /Api/HorizonsApiRequest.cs
// STATUS: FIXED (VECTOR / OBSERVER clean separation)
// ============================================================

using System.Collections.Generic;

namespace EphemerisRegression.Api
{
    public sealed class HorizonsApiRequest
    {
        public int Command { get; init; }

        public string StartTime { get; init; } = "";
        public string StopTime { get; init; } = "";
        public string StepSize { get; init; } = "1h";

        public string Center { get; init; } = "@10";

        public string RefPlane { get; init; } = "ECLIPTIC";
        public string RefSystem { get; init; } = "ICRF";

        public string? VectorCorrection { get; init; }

        public string OutputUnits { get; init; } = "AU-D";

        public string CsvFormat { get; init; } = "NO";

        // VECTORS (default) or OBSERVER
        public string EphemType { get; init; } = "VECTORS";

        // Additional parameters for OBSERVER mode:
        // TIME_TYPE, QUANTITIES, ANG_FORMAT, CAL_FORMAT, MAKE_EPHEM, etc.
        public IDictionary<string, string>? AdditionalParameters { get; init; }

        public IDictionary<string, string> ToParameterDictionary()
        {
            var dict = new Dictionary<string, string>
            {
                ["COMMAND"] = Command.ToString(),
                ["CENTER"] = Center,
                ["START_TIME"] = StartTime,
                ["STOP_TIME"] = StopTime,
                ["STEP_SIZE"] = StepSize,
                ["EPHEM_TYPE"] = EphemType,
                ["CSV_FORMAT"] = CsvFormat,
                ["OBJ_DATA"] = "NO"
            };

            // =====================================================
            // VECTORS MODE
            // =====================================================
            if (EphemType == "VECTORS")
            {
                dict["REF_PLANE"] = RefPlane;
                dict["REF_SYSTEM"] = RefSystem;

                if (!string.IsNullOrWhiteSpace(OutputUnits))
                    dict["OUT_UNITS"] = OutputUnits;

                if (!string.IsNullOrWhiteSpace(VectorCorrection))
                    dict["VECT_CORR"] = VectorCorrection!;
            }

            // =====================================================
            // OBSERVER MODE
            // =====================================================
            // IMPORTANT:
            // Do NOT send REF_PLANE / REF_SYSTEM / OUT_UNITS here.
            // Horizons rejects them for OBSERVER.
            if (EphemType == "OBSERVER" && AdditionalParameters != null)
            {
                foreach (var kvp in AdditionalParameters)
                    dict[kvp.Key] = kvp.Value;
            }

            return dict;
        }
    }
}