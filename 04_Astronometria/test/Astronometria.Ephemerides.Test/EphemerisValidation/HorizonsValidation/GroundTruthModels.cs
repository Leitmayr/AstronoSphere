using System.Collections.Generic;

namespace AstroSim.Ephemerides.Test.EphemerisValidation.HorizonsValidation.Common
{
    /// <summary>
    /// PURPOSE:
    /// Minimal JSON model for AstronoSphere GroundTruth ephemeris files.
    ///
    /// CONTEXT:
    /// M2.2 Astronometria Testing reads Horizons Baseline files and compares
    /// Astronometria VSOP L0 position results against GroundTruth.
    ///
    /// CONSTRAINTS:
    /// - KISS only.
    /// - Position comparison only in M2.2.
    /// - No production pipeline output.
    /// - No interpretation of GroundTruth metadata.
    /// </summary>
    public sealed class GroundTruthFile
    {
        public ExperimentRef ExperimentRef { get; set; } = new();

        public DatasetHeader DatasetHeader { get; set; } = new();

        public List<GroundTruthSample> Data { get; set; } = new();
    }

    public sealed class ExperimentRef
    {
        public string ExperimentID { get; set; } = string.Empty;

        public string CoreHash { get; set; } = string.Empty;

        public string CatalogNumber { get; set; } = string.Empty;
    }

    public sealed class DatasetHeader
    {
        public Measurement Measurement { get; set; } = new();

        public string DatasetID { get; set; } = string.Empty;
    }

    public sealed class Measurement
    {
        public string Level { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;
    }

    public sealed class GroundTruthSample
    {
        public double JD { get; set; }

        public GroundTruthVector Position { get; set; } = new();

        public GroundTruthVector Velocity { get; set; } = new();
    }

    public sealed class GroundTruthVector
    {
        public double X { get; set; }

        public double Y { get; set; }

        public double Z { get; set; }
    }
}