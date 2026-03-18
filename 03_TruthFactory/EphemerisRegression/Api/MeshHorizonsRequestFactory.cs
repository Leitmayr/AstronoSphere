using EphemerisRegression.Batching;
using EphemerisRegression.Config;
using EphemerisRegression.Infrastructure;

namespace EphemerisRegression.Api
{
    public sealed class MeshHorizonsRequestFactory
    {
        private readonly HorizonsLevelBaseConfig _config;

        public MeshHorizonsRequestFactory(HorizonsLevelBaseConfig config)
        {
            _config = config;
        }

        public (HorizonsApiRequest Request, string Canonical, string Hash)
            Create(MeshBatchDefinition batch)
        {
            var request = new HorizonsApiRequest
            {
                Command = batch.PlanetCode,
                StartTime = batch.StartUtc.ToIsoString(),
                StopTime = batch.StopUtc.ToIsoString(),
                StepSize = $"{batch.StepDays} d",

                Center = _config.Center,
                RefPlane = _config.RefPlane,
                RefSystem = _config.RefSystem,
                VectorCorrection = _config.VectorCorrection,
                OutputUnits = _config.OutputUnits
            };

            var parameters = request.ToParameterDictionary();

            var canonical = CanonicalRequestBuilder.Build(parameters);
            var hash = HashCalculator.ComputeSha256(canonical);

            return (request, canonical, hash);
        }
    }
}