namespace AstronoData.Contracts.Domain
{
    /// <summary>
    /// Defines all mesh types used in AstronoSphere.
    ///
    /// IMPORTANT:
    /// - This is pure domain classification.
    /// - No inference from filenames or paths.
    /// - Used for both simulation and validation semantics.
    ///
    /// M2.2:
    /// - MVH1/MVH2/MVH3 are used for Horizons validation (Mesh_Testing)
    /// - MCRE/MXT1/MXT2 are simulation-only (no strict GT matching required)
    /// </summary>
    public enum MeshType
    {
        MCRE,
        MXT1,
        MXT2,
        MVH1,
        MVH2,
        MVH3
    }
}