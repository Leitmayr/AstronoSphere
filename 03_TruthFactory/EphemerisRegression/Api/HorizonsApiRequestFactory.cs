// ============================================================
// FILE: /Api/HorizonsApiRequestFactory.cs
// STATUS: ÄNDERUNG
// ============================================================

using System;
using System.Collections.Generic;
using System.Globalization;
using EphemerisRegression.Config;
using EphemerisRegression.Event;

namespace EphemerisRegression.Api
{
    public sealed class HorizonsApiRequestFactory
    {
        private readonly HorizonsLevelBaseConfig _config;

        public HorizonsApiRequestFactory(HorizonsLevelBaseConfig config)
        {
            _config = config;
        }

        public HorizonsApiRequest CreateCustom(
            int commandCode,
            DateTime start,
            DateTime stop,
            string stepSize)
        {
            return BuildRequest(commandCode, start, stop, stepSize);
        }

        public HorizonsApiRequest Create(HelioEvent helioEvent)
        {
            double startJd = helioEvent.JulianDate - helioEvent.WindowDays;
            double stopJd = helioEvent.JulianDate + helioEvent.WindowDays;

            var start = JulianToDateTime(startJd);
            var stop = JulianToDateTime(stopJd);

            return BuildRequest(helioEvent.CommandCode, start, stop, _config.StepSize);
        }

        // ============================================================
        // CENTRAL BUILD LOGIC
        // ============================================================

        private HorizonsApiRequest BuildRequest(
            int commandCode,
            DateTime start,
            DateTime stop,
            string stepSize)
        {
            bool observerMode = _config.TableType == "O";

            var additional = new Dictionary<string, string>
            {
                ["TIME_TYPE"] = _config.TimeType
            };

            if (observerMode)
            {
                additional["QUANTITIES"] = "1";
                additional["ANG_FORMAT"] = "DEG";
                additional["CAL_FORMAT"] = "JD";
            }

            return new HorizonsApiRequest
            {
                Command = commandCode,
                Center = _config.Center,
                StartTime = start.ToString("yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture),
                StopTime = stop.ToString("yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture),
                StepSize = stepSize,

                RefPlane = _config.RefPlane,
                RefSystem = _config.RefSystem,

                // For OBSERVER we keep OutputUnits property harmless;
                // actual omission happens in ToParameterDictionary().
                OutputUnits = _config.OutputUnits,

                CsvFormat = _config.CsvFormat,
                VectorCorrection = _config.VectorCorrection,

                EphemType = observerMode ? "OBSERVER" : "VECTORS",
                AdditionalParameters = additional
            };
        }

        private static DateTime JulianToDateTime(double jd)
        {
            double unixTime = (jd - 2440587.5) * 86400.0;
            return DateTimeOffset
                .FromUnixTimeSeconds((long)unixTime)
                .UtcDateTime;
        }
    }
}