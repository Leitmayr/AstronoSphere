using EphemerisRegression.Config;

namespace EphemerisRegression.Config.Geo
{
    public class HorizonsGeoLevel1LightTimeConfig
        : HorizonsGeoLevel0Config
    {
        public override string? VectorCorrection => "LT";
    }
}
