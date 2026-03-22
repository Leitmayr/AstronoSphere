using EphemerisRegression.Config;

namespace EphemerisRegression.Config.Geo
{
    public class HorizonsGeoLevel2LightTimeAberrationConfig
        : HorizonsGeoLevel1LightTimeConfig
    {
        public override string? VectorCorrection => "LT+S";
    }
}
