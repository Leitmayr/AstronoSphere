using EphemerisRegression.Config;

namespace EphemerisRegression.Config.Helio
{
    public class HorizonsHelioLevel1LightTimeConfig
        : HorizonsHelioLevel0Config
    {
        public override string? VectorCorrection => "LT";
    }
}
