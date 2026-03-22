using System;
using System.IO;
using System.Threading.Tasks;
using EphemerisRegression.Batching;

namespace EphemerisRegression.Export
{
    public sealed class MeshDataL0RawExportRunner
    {
        private readonly string _baseDirectory;

        public MeshDataL0RawExportRunner()
        {
            var projectRoot = Directory.GetCurrentDirectory();

            _baseDirectory = Path.Combine(
                projectRoot,
                "Horizons",
                "Mesh",
                "Helio",
                "L0",
                "Raw");

            Directory.CreateDirectory(_baseDirectory);
        }

        public async Task RunAsync(
            MeshBatchDefinition batch,
            int chunkIndex,
            string rawContent)
        {
            if (rawContent == null)
                throw new ArgumentNullException(nameof(rawContent));

            string fileName =
                $"{batch.PlanetName}_TS-D_L0_{batch.EpochType}_Chunk_{chunkIndex:00}.csv";

            string fullPath = Path.Combine(_baseDirectory, fileName);

            await File.WriteAllTextAsync(fullPath, rawContent);
        }
    }
}