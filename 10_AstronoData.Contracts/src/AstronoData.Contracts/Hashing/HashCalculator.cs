// ============================================================
// FILE: 10_AstronoData.Contracts/src/Hashing/HashCalculator.cs
// STATUS: FINAL
// ============================================================

using System.Security.Cryptography;
using System.Text;

namespace AstronoData.Contracts.Hashing
{
    public static class HashCalculator
    {
        public static string Compute(string canonical)
        {
            using var sha = SHA256.Create();

            var bytes = Encoding.UTF8.GetBytes(canonical);
            var hash = sha.ComputeHash(bytes);

            var sb = new StringBuilder();

            foreach (var b in hash)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}