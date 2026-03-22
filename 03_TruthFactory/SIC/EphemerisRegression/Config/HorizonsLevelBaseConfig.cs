namespace EphemerisRegression.Config
{
    public abstract class HorizonsLevelBaseConfig
    {
        public abstract string Center { get; }

        public virtual string RefPlane => "ECLIPTIC";
        public virtual string RefSystem => "ICRF";
        public virtual string? VectorCorrection => null;

        public virtual string TimeType => "TDB";
        public virtual string OutputUnits => "AU-D";
        public virtual string StepSize => "1h";
        public virtual string TableType => "V";

        // NEU
        public virtual string CsvFormat => "NO";
        // Default = klassisches Format
    }
}