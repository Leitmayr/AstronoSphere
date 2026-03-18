using System;
using System.Security.Cryptography;
using System.Text;

namespace EphemerisRegression.Infrastructure
{
    public static class HashCalculator
    {
        public static string ComputeSha256(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            using var sha = SHA256.Create();

            var bytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = sha.ComputeHash(bytes);

            return ToHex(hashBytes);
        }

        private static string ToHex(byte[] bytes)
        {
            var sb = new StringBuilder(bytes.Length * 2);

            foreach (var b in bytes)
                sb.Append(b.ToString("x2"));

            return sb.ToString().ToUpperInvariant();
        }
    }
}
