// ============================================================
// FILE: 10_AstronoData.Contracts/src/Hashing/HashService.cs
// STATUS: FINAL
// ============================================================

namespace AstronoData.Contracts.Hashing
{
    public interface IHashService
    {
        string BuildCanonical(object input);
        string ComputeHash(string canonical);
    }

    public class HashService : IHashService
    {
        public string BuildCanonical(object input)
        {
            return Canonicalizer.Build(input);
        }

        public string ComputeHash(string canonical)
        {
            return HashCalculator.Compute(canonical);
        }
    }
}