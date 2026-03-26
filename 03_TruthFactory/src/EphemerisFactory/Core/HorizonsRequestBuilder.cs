using EphemerisRegression.Api;
using System;
using System.Globalization;
using System.Text.Json;

namespace EphemerisFactory.Core
{
    public static class HorizonsRequestBuilder
    {
        public static HorizonsApiRequest Build(JsonElement root)
        {
            var core = root.GetProperty("Core");

            var observer = core.GetProperty("Observer");
            var time = core.GetProperty("Time");
            var targets = core.GetProperty("Targets");

            var targetName = targets[0].GetString()!;
            int command = PlanetMapper.ToCommand(targetName);

            string observerType = observer.GetProperty("Type").GetString()!;

            string center = observerType switch
            {
                "Heliocentric" => "@10",
                "Geocentric" => "500@399",
                _ => throw new Exception($"Unsupported observer type: {observerType}")
            };

            double start = time.GetProperty("StartJD").GetDouble();
            double stop = time.GetProperty("StopJD").GetDouble();

            // 🔥 NEU: Step ist STRING (z.B. "1H")
            string step = time.GetProperty("StepDays").GetString()!;

            return new HorizonsApiRequest
            {
                Command = command,
                Center = center,
                StartTime = $"JD{F(start)}",
                StopTime = $"JD{F(stop)}",

                // 🔥 WICHTIG: kein "D" anhängen mehr!
                StepSize = step,

                RefPlane = "ECLIPTIC",
                RefSystem = "ICRF",
                OutputUnits = "AU-D",
                EphemType = "VECTORS",
                CsvFormat = "YES"
            };
        }

        private static string F(double v) =>
            v.ToString("0.########", CultureInfo.InvariantCulture);
    }
}