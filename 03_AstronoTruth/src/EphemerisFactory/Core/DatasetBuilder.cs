// ============================================================
// FILE: 03_TruthFactory/src/EphemerisFactory/Core/DatasetBuilder.cs
// STATUS: UPDATE (M1.9 Variant A - object model, deterministic writer)
// ============================================================

using EphemerisRegression.Domain;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace EphemerisFactory.Core
{
    public static class DatasetBuilder
    {
        public static string Build(
            string experimentJson,
            string canonical,
            string requestHash,
            string epochHash,
            string level,
            string url,
            string rawCsv)
        {
            using var doc = JsonDocument.Parse(experimentJson);
            var root = doc.RootElement;

            string experimentId = root.GetProperty("ExperimentID").GetString()!;
            string catalogNumber = root.GetProperty("CatalogNumber").GetString()!;
            string coreHash = root.GetProperty("CoreHash").GetString()!;

            var core = root.GetProperty("Core");
            var time = core.GetProperty("Time");
            var observer = core.GetProperty("Observer");

            string timeScale = time.GetProperty("TimeScale").GetString()!;
            string observerType = observer.GetProperty("Type").GetString()!;

            string mode = observerType switch
            {
                "Heliocentric" => "HELIO",
                "Geocentric" => "GEO",
                _ => throw new Exception($"Unsupported observer type: {observerType}")
            };

            string datasetId = $"{experimentId}__EPH-HORIZONS-VEC-{level}";

            var metadataProvider = new HorizonsMetadataProvider();

            var rootModel = new GroundTruthDatasetModel
            {
                ExperimentRef = new ExperimentRefModel
                {
                    ExperimentID = experimentId,
                    CoreHash = coreHash,
                    CatalogNumber = catalogNumber
                },
                DatasetHeader = new DatasetHeaderModel
                {
                    Measurement = new MeasurementModel
                    {
                        Level = level,
                        Type = "VEC"
                    },
                    DatasetID = datasetId,
                    TruthMetadata = new TruthMetadataModel
                    {
                        CanonicalRequest = canonical,
                        RequestHash = requestHash,
                        EpochHash = epochHash,
                        TruthProviderUrl = url,
                        GeneratedAtUtc = null,
                        Requests = new List<RequestTraceModel>
                        {
                            new RequestTraceModel
                            {
                                CanonicalRequest = canonical,
                                RequestHash = requestHash,
                                HorizonsUrl = url
                            }
                        }
                    },
                    FactoryMetadata = metadataProvider.CreateFactoryMetadata(mode, level, timeScale),
                    TruthCitation = metadataProvider.CreateTruthCitation(),
                    Provenance = metadataProvider.CreateProvenance(),
                    EngineCitation = metadataProvider.CreateEngineCitation(),
                    ValidationFingerprint = null
                },
                Data = ParseStateVectors(rawCsv)
            };

            return Serialize(rootModel);
        }

        // =====================================================
        // DATA PARSING
        // =====================================================

        private static List<StateVector> ParseStateVectors(string rawCsv)
        {
            var rows = HorizonsCsvParser.ParseRaw(rawCsv);
            var result = new List<StateVector>(rows.Count);

            foreach (var row in rows)
            {
                if (row.Values.Length < 7)
                    continue;

                result.Add(new StateVector(
                    row.Values[0],
                    row.Values[1],
                    row.Values[2],
                    row.Values[3],
                    row.Values[4],
                    row.Values[5],
                    row.Values[6],
                    row.JdRaw
                 ));
            }

            return result;
        }

        // =====================================================
        // SERIALIZATION
        // =====================================================

        private static string Serialize(GroundTruthDatasetModel model)
        {
            var buffer = new ArrayBufferWriter<byte>();

            var options = new JsonWriterOptions
            {
                Indented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            using (var writer = new Utf8JsonWriter(buffer, options))
            {
                writer.WriteStartObject();

                WriteExperimentRef(writer, model.ExperimentRef);
                WriteDatasetHeader(writer, model.DatasetHeader);
                WriteData(writer, model.Data);

                writer.WriteEndObject();
            }

            return Encoding.UTF8.GetString(buffer.WrittenSpan);
        }

        private static void WriteExperimentRef(Utf8JsonWriter writer, ExperimentRefModel model)
        {
            writer.WritePropertyName("ExperimentRef");
            writer.WriteStartObject();

            writer.WriteString("ExperimentID", model.ExperimentID);
            writer.WriteString("CoreHash", model.CoreHash);
            writer.WriteString("CatalogNumber", model.CatalogNumber);

            writer.WriteEndObject();
        }

        private static void WriteDatasetHeader(Utf8JsonWriter writer, DatasetHeaderModel model)
        {
            writer.WritePropertyName("DatasetHeader");
            writer.WriteStartObject();

            writer.WritePropertyName("Measurement");
            writer.WriteStartObject();
            writer.WriteString("Level", model.Measurement.Level);
            writer.WriteString("Type", model.Measurement.Type);
            writer.WriteEndObject();

            writer.WriteString("DatasetID", model.DatasetID);

            WriteTruthMetadata(writer, model.TruthMetadata);

            writer.WritePropertyName("FactoryMetadata");
            JsonSerializer.Serialize(writer, model.FactoryMetadata, CreateSerializerOptions());

            writer.WritePropertyName("TruthCitation");
            JsonSerializer.Serialize(writer, model.TruthCitation, CreateSerializerOptions());

            writer.WritePropertyName("Provenance");
            JsonSerializer.Serialize(writer, model.Provenance, CreateSerializerOptions());

            writer.WritePropertyName("EngineCitation");
            JsonSerializer.Serialize(writer, model.EngineCitation, CreateSerializerOptions());

            if (model.ValidationFingerprint == null)
            {
                writer.WriteNull("ValidationFingerprint");
            }
            else
            {
                writer.WriteString("ValidationFingerprint", model.ValidationFingerprint);
            }

            writer.WriteEndObject();
        }

        private static void WriteTruthMetadata(Utf8JsonWriter writer, TruthMetadataModel model)
        {
            writer.WritePropertyName("TruthMetadata");
            writer.WriteStartObject();

            writer.WriteString("CanonicalRequest", model.CanonicalRequest);
            writer.WriteString("RequestHash", model.RequestHash);
            writer.WriteString("EpochHash", model.EpochHash);

            writer.WritePropertyName("Requests");
            JsonSerializer.Serialize(writer, model.Requests, CreateSerializerOptions());

            writer.WriteString("TruthProviderUrl", model.TruthProviderUrl);

            if (model.GeneratedAtUtc == null)
            {
                writer.WriteNull("GeneratedAtUtc");
            }
            else
            {
                writer.WriteString("GeneratedAtUtc", model.GeneratedAtUtc);
            }

            writer.WriteEndObject();
        }

        private static void WriteData(Utf8JsonWriter writer, List<StateVector> data)
        {
            writer.WritePropertyName("Data");
            writer.WriteStartArray();

            foreach (var d in data)
            {
                writer.WriteStartObject();

                // 🔥 JD FIX
                writer.WritePropertyName("JD");
                writer.WriteRawValue(d.JulianDateRaw);

                writer.WritePropertyName("Position");
                writer.WriteRawValue(
                    $"{{\"X\":{Format(d.X)},\"Y\":{Format(d.Y)},\"Z\":{Format(d.Z)}}}"
                );

                writer.WritePropertyName("Velocity");
                writer.WriteRawValue(
                    $"{{\"X\":{Format(d.VX)},\"Y\":{Format(d.VY)},\"Z\":{Format(d.VZ)}}}"
                );

                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }

        private static string Format(double value)
        {
            return value.ToString("G17", CultureInfo.InvariantCulture);
        }
        private static void WriteRawNumber(Utf8JsonWriter writer, string propertyName, double value)
        {
            writer.WritePropertyName(propertyName);

            // WICHTIG:
            // Datenblock-Emission bleibt absichtlich auf der bisherigen,
            // bereits validierten Linie, damit Step1/Step2 stabil bleiben.
            // Keine Rundung, invariant, deterministisch.
            var raw = value.ToString("G17", CultureInfo.InvariantCulture);
            writer.WriteRawValue(raw);
        }

        private static JsonSerializerOptions CreateSerializerOptions()
        {
            return new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never
            };
        }
    }

    // =========================================================
    // MODELS
    // =========================================================

    public sealed class GroundTruthDatasetModel
    {
        public ExperimentRefModel ExperimentRef { get; set; } = new();
        public DatasetHeaderModel DatasetHeader { get; set; } = new();
        public List<StateVector> Data { get; set; } = new();
    }

    public sealed class ExperimentRefModel
    {
        public string ExperimentID { get; set; } = "";
        public string CoreHash { get; set; } = "";
        public string CatalogNumber { get; set; } = "";
    }

    public sealed class DatasetHeaderModel
    {
        public MeasurementModel Measurement { get; set; } = new();
        public string DatasetID { get; set; } = "";
        public TruthMetadataModel TruthMetadata { get; set; } = new();
        public FactoryMetadataModel FactoryMetadata { get; set; } = new();
        public TruthCitationModel TruthCitation { get; set; } = new();
        public ProvenanceModel Provenance { get; set; } = new();
        public EngineCitationModel EngineCitation { get; set; } = new();
        public string? ValidationFingerprint { get; set; }
    }

    public sealed class MeasurementModel
    {
        public string Level { get; set; } = "";
        public string Type { get; set; } = "";
    }

    public sealed class TruthMetadataModel
    {
        public string CanonicalRequest { get; set; } = "";
        public string RequestHash { get; set; } = "";
        public string EpochHash { get; set; } = "";
        public List<RequestTraceModel> Requests { get; set; } = new();
        public string TruthProviderUrl { get; set; } = "";
        public string? GeneratedAtUtc { get; set; }
    }

    public sealed class RequestTraceModel
    {
        public string CanonicalRequest { get; set; } = "";
        public string RequestHash { get; set; } = "";
        public string HorizonsUrl { get; set; } = "";
    }
}