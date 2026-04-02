// ============================================================
// FILE: 03_TruthFactory/src/EphemerisFactory/Core/HorizonsRequestBuilder.cs
// STATUS: GEÄNDERT (RC5 – Frame Mapping Fix)
// ============================================================

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
            var frame = core.GetProperty("Frame");

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

            string step = time.GetProperty("StepDays").GetString()!;

            // =====================================================
            // RC5 FIX: Frame Mapping
            // =====================================================

            string frameType = frame.GetProperty("Type").GetString()!;

            string refPlane = frameType switch
            {
                "GeoEcliptic" => "ECLIPTIC",
                "HelioEcliptic" => "ECLIPTIC",
                "GeoEquatorial" => "FRAME",
                _ => throw new Exception($"Unsupported frame type: {frameType}")
            };

            // =====================================================
            // M1: Measurement (KISS)
            // =====================================================

            string level = "L0";
            string type = "VEC";

            string ephemType = HorizonsMapping.GetEphemType(level, type);
            string? vectCorr = HorizonsMapping.GetVectorCorrection(level);

            // =====================================================
            // REQUEST BUILD
            // =====================================================

            return new HorizonsApiRequest
            {
                Command = command,
                Center = center,
                StartTime = $"JD{F(start)}",
                StopTime = $"JD{F(stop)}",
                StepSize = step,
                RefPlane = refPlane, // 🔥 FIX
                RefSystem = "ICRF",
                OutputUnits = "AU-D",
                EphemType = ephemType,
                CsvFormat = "YES",
                VectorCorrection = vectCorr
            };
        }

        private static string F(double v) =>
            v.ToString("0.########", CultureInfo.InvariantCulture);
    }
}