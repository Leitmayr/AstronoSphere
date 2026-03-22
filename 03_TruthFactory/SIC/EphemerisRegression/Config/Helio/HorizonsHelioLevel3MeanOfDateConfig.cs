using EphemerisRegression.Config;

namespace EphemerisRegression.Config.Helio
{
    public class HorizonsHelioLevel3MeanOfDateConfig
        : HorizonsHelioLevel2LightTimeAberrationConfig
    {
        public override string RefPlane => "DATE";
    }
}
