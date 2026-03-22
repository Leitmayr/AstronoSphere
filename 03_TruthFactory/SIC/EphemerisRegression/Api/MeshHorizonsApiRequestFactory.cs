using System.Globalization;
using EphemerisRegression.Config;

namespace EphemerisRegression.Api
{
    /// <summary>
    /// Mesh-specific Horizons request factory.
    /// Uses JD-only START/STOP to avoid calendar edge cases (year 0, BC, etc.).
    /// </summary>
    public sealed class MeshHorizonsApiRequestFactory
    {
        private readonly HorizonsLevelBaseConfig _config;

        public MeshHorizonsApiRequestFactory(HorizonsLevelBaseConfig config)
        {
            _config = config;
        }

        public HorizonsApiRequest Create(
            int commandCode,
            double startJulianDay,
            double stopJulianDay,
            double stepDays)
        {
            return new HorizonsApiRequest
            {
                Command = commandCode,

                // JD-only format (no whitespace!)
                StartTime = "JD" + startJulianDay.ToString("0.0#############", CultureInfo.InvariantCulture),
                StopTime = "JD" + stopJulianDay.ToString("0.0#############", CultureInfo.InvariantCulture),

                StepSize = stepDays.ToString("0", CultureInfo.InvariantCulture) + "D",

                Center = _config.Center,
                RefPlane = _config.RefPlane,
                RefSystem = _config.RefSystem,
                VectorCorrection = _config.VectorCorrection,
                OutputUnits = _config.OutputUnits
            };
        }
    }
}