// ============================================================
// FILE: 03_AstronoTruth/src/EphemerisFactory/Core/AstronoTruthDiagnosticGuard.cs
// STATUS: NEW (M2.1 AstronoDiag pre-request guard)
// ============================================================

using AstronoDiag;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace EphemerisFactory.Core
{
    public static class AstronoTruthDiagnosticGuard
    {
        public static DiagnosticRecord? EvaluatePreRequest(JsonElement root)
        {
            var maturity = root
                .GetProperty("Metadata")
                .GetProperty("Status")
                .GetProperty("Maturity")
                .GetString() ?? string.Empty;

            if (!string.Equals(maturity, "Released", StringComparison.OrdinalIgnoreCase))
                return BuildInvalidMaturity(root, maturity);

            var target = GetTarget(root);
            var startJD = GetStartJD(root);
            var stopJD = GetStopJD(root);

            var range = HorizonsProviderRangeCatalog.GetRange(target);

            if (startJD < range.ProviderMinJD || stopJD > range.ProviderMaxJD)
            {
                return BuildProviderRangeViolation(
                    root,
                    target,
                    startJD,
                    stopJD,
                    range.ProviderMinJD,
                    range.ProviderMaxJD);
            }

            return null;
        }

        public static DiagnosticRecord BuildRequestFailed(
            JsonElement root,
            string requestUrl,
            string reason,
            string? responseSnippet = null)
        {
            var def = DiagnosticCatalog.AstronoTruthRequestFailed;

            var details = new Dictionary<string, object>
            {
                { "Target", GetTarget(root) },
                { "RequestUrl", requestUrl },
                { "Reason", reason }
            };

            if (!string.IsNullOrWhiteSpace(responseSnippet))
                details["ResponseSnippet"] = responseSnippet;

            return BuildBaseRecord(
                root,
                def,
                "Horizons request failed.",
                details);
        }

        public static DiagnosticRecord BuildParseFailed(
            JsonElement root,
            string requestUrl,
            string parseStage,
            string reason)
        {
            var def = DiagnosticCatalog.AstronoTruthParseFailed;

            return BuildBaseRecord(
                root,
                def,
                "Horizons response could not be parsed.",
                new Dictionary<string, object>
                {
                    { "Target", GetTarget(root) },
                    { "RequestUrl", requestUrl },
                    { "ParseStage", parseStage },
                    { "Reason", reason }
                });
        }

        private static DiagnosticRecord BuildInvalidMaturity(JsonElement root, string maturity)
        {
            var def = DiagnosticCatalog.AstronoTruthInvalidMaturity;

            return BuildBaseRecord(
                root,
                def,
                "Experiment maturity is not Released.",
                new Dictionary<string, object>
                {
                    { "CatalogNumber", GetCatalogNumber(root) },
                    { "ActualMaturity", maturity }
                });
        }

        private static DiagnosticRecord BuildProviderRangeViolation(
            JsonElement root,
            string target,
            double startJD,
            double stopJD,
            double providerMinJD,
            double providerMaxJD)
        {
            var def = DiagnosticCatalog.AstronoTruthProviderRangeViolation;

            return BuildBaseRecord(
                root,
                def,
                "Experiment time range is outside provider range.",
                new Dictionary<string, object>
                {
                    { "Target", target },
                    { "StartJD", startJD },
                    { "StopJD", stopJD },
                    { "ProviderMinJD", providerMinJD },
                    { "ProviderMaxJD", providerMaxJD }
                });
        }

        private static DiagnosticRecord BuildBaseRecord(
            JsonElement root,
            DiagnosticCodeDefinition def,
            string message,
            Dictionary<string, object> details)
        {
            return new DiagnosticRecord
            {
                Code = def.Code,
                Symbol = def.Symbol,
                Severity = def.Severity,
                Message = message,
                SourceSystem = "AstronoTruth",
                SubSourceSystem = "Horizons",
                InputObjectType = "Experiment",
                InputObjectId = GetExperimentId(root),
                CatalogNumber = GetCatalogNumber(root),
                Details = details,
                CreatedAtUtc = DateTime.UtcNow
            };
        }

        private static string GetCatalogNumber(JsonElement root)
        {
            return root.GetProperty("CatalogNumber").GetString()!;
        }

        private static string GetExperimentId(JsonElement root)
        {
            return root.GetProperty("ExperimentID").GetString()!;
        }

        private static string GetTarget(JsonElement root)
        {
            return root
                .GetProperty("Core")
                .GetProperty("ObservedObject")
                .GetProperty("Targets")[0]
                .GetString()!;
        }

        private static double GetStartJD(JsonElement root)
        {
            return root
                .GetProperty("Core")
                .GetProperty("Time")
                .GetProperty("StartJD")
                .GetDouble();
        }

        private static double GetStopJD(JsonElement root)
        {
            return root
                .GetProperty("Core")
                .GetProperty("Time")
                .GetProperty("StopJD")
                .GetDouble();
        }
    }
}