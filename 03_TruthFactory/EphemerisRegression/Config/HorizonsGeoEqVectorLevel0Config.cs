namespace EphemerisRegression.Config
{
    public sealed class HorizonsGeoEqVectorLevel0Config
        : HorizonsLevelBaseConfig
    {
        public override string Center => "500@399";

        // Äquatorial statt ekliptisch
        public override string RefPlane => "FRAME";
        public override string RefSystem => "ICRF";

        public override string TimeType => "TT";

        public override string TableType => "V";

        public override string OutputUnits => "AU-D";

        public override string CsvFormat => "NO";

        public override string? VectorCorrection => null;
    }
}