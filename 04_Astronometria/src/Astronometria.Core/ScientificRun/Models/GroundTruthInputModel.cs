using System.Collections.Generic;

namespace Astronometria.Core.ScientificRun.Models
{
    public sealed class GroundTruthInputModel
    {
        public ExperimentReferenceModel ExperimentRef { get; set; } = new();

        public GroundTruthDatasetHeaderModel DatasetHeader { get; set; } = new();

        public List<GroundTruthSampleModel> Data { get; set; } = new();

        public string SourceFile { get; set; } = string.Empty;
    }

    public sealed class ExperimentReferenceModel
    {
        public string ExperimentID { get; set; } = string.Empty;

        public string CoreHash { get; set; } = string.Empty;

        public string CatalogNumber { get; set; } = string.Empty;
    }

    public sealed class GroundTruthDatasetHeaderModel
    {
        public GroundTruthMeasurementModel Measurement { get; set; } = new();

        public string DatasetID { get; set; } = string.Empty;

        public GroundTruthTruthMetadataModel TruthMetadata { get; set; } = new();

        public GroundTruthFactoryMetadataModel FactoryMetadata { get; set; } = new();
    }

    public sealed class GroundTruthMeasurementModel
    {
        public string Level { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;
    }

    public sealed class GroundTruthTruthMetadataModel
    {
        public string CanonicalRequest { get; set; } = string.Empty;

        public string RequestHash { get; set; } = string.Empty;

        public string EpochHash { get; set; } = string.Empty;

        public List<GroundTruthRequestModel> Requests { get; set; } = new();

        public string TruthProviderUrl { get; set; } = string.Empty;
    }

    public sealed class GroundTruthRequestModel
    {
        public string CanonicalRequest { get; set; } = string.Empty;

        public string RequestHash { get; set; } = string.Empty;

        public string HorizonsUrl { get; set; } = string.Empty;
    }

    public sealed class GroundTruthFactoryMetadataModel
    {
        public string FactoryName { get; set; } = string.Empty;

        public string FactoryVersion { get; set; } = string.Empty;

        public string Source { get; set; } = string.Empty;

        public string ReferenceEphemeris { get; set; } = string.Empty;

        public string Mode { get; set; } = string.Empty;

        public string EphemType { get; set; } = string.Empty;

        public string CorrectionLevel { get; set; } = string.Empty;

        public string TimeScale { get; set; } = string.Empty;
    }

    public sealed class GroundTruthSampleModel
    {
        public double JD { get; set; }

        public GroundTruthVectorModel Position { get; set; } = new();

        public GroundTruthVectorModel Velocity { get; set; } = new();
    }

    public sealed class GroundTruthVectorModel
    {
        public double X { get; set; }

        public double Y { get; set; }

        public double Z { get; set; }
    }
}