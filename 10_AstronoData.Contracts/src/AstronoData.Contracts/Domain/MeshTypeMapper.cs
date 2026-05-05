using System;

namespace AstronoData.Contracts.Domain
{
    public static class MeshTypeMapper
    {
        /// <summary>
        /// Maps Event.Description → MeshType
        /// STRICT:
        /// - no inference
        /// - exact mapping only
        /// </summary>
        public static MeshType Map(string description)
        {
            return description switch
            {
                "MCRE" => MeshType.MCRE,
                "MXT1" => MeshType.MXT1,
                "MXT2" => MeshType.MXT2,
                "MVH1" => MeshType.MVH1,
                "MVH2" => MeshType.MVH2,
                "MVH3" => MeshType.MVH3,

                _ => throw new InvalidOperationException(
                    $"Unknown MeshType description: {description}")
            };
        }

        /// <summary>
        /// True only for Mesh_Testing experiments (Horizons validation subset)
        /// </summary>
        public static bool IsHorizonsValidationMesh(MeshType meshType)
        {
            return meshType is MeshType.MVH1
                or MeshType.MVH2
                or MeshType.MVH3;
        }
    }
}