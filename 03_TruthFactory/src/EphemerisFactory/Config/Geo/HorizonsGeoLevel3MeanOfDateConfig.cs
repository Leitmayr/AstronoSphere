using EphemerisRegression.Config;

namespace EphemerisRegression.Config.Geo
{
    public class HorizonsGeoLevel3MeanOfDateConfig
        : HorizonsGeoLevel2LightTimeAberrationConfig
    {
        public override string RefPlane => "DATE";
    }
}
