using EphemerisRegression.Config;

namespace EphemerisRegression.Config.Helio
{
    public class HorizonsHelioLevel2LightTimeAberrationConfig
        : HorizonsHelioLevel1LightTimeConfig
    {
        public override string? VectorCorrection => "LT+S";
    }
}
