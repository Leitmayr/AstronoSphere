using EphemerisRegression.Mesh;

namespace EphemerisRegression.Batching
{
    public sealed class MeshBatchDefinition
    {
        public MeshEpochType EpochType { get; }
        public string PlanetName { get; }
        public int PlanetCode { get; }

        public MeshUtc StartUtc { get; }
        public MeshUtc StopUtc { get; }

        public double StepDays { get; }
        public int StepCount { get; }

        public MeshBatchDefinition(
            MeshEpochType epochType,
            string planetName,
            int planetCode,
            MeshUtc startUtc,
            MeshUtc stopUtc,
            double stepDays,
            int stepCount)
        {
            EpochType = epochType;
            PlanetName = planetName;
            PlanetCode = planetCode;
            StartUtc = startUtc;
            StopUtc = stopUtc;
            StepDays = stepDays;
            StepCount = stepCount;
        }

        public override string ToString()
        {
            return $"{PlanetName} | {EpochType} | {StartUtc} -> {StopUtc} | Steps: {StepCount}";
        }
    }
}