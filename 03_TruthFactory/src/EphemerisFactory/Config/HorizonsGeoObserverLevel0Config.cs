// ============================================================
// FILE: /Config/HorizonsGeoObserverLevel0Config.cs
// STATUS: NEU (TS-C)
// ============================================================

namespace EphemerisRegression.Config
{
    public class HorizonsGeoObserverLevel0Config
        : HorizonsLevelBaseConfig
    {
        public override string Center => "500@399";

        // Switch Horizons to OBSERVER tables
        public override string TableType => "O";

        // Observer mode does not use vector units
        public override string OutputUnits => "";

        public override string TimeType => "TT";
    }
}