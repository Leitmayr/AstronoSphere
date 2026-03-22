// ============================================================
// FILE: CoreHashGenerator.cs
// STATUS: UPDATE
// ============================================================

using AstronoData.ScenarioCandidates;
using ScenarioHeaderGenerator.ScenarioCandidates;
using System.Security.Cryptography;
using System.Text;

namespace ScenarioHeaderGenerator
{
    public static class CoreHashGenerator
    {
        public static string Generate(CoreDefinition core)
        {
            var canonical = CoreCanonicalizer.ToCanonicalJson(core);

            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(canonical);
            var hash = sha.ComputeHash(bytes);

            return BitConverter.ToString(hash).Replace("-", "").Substring(0, 8);
        }
    }
}