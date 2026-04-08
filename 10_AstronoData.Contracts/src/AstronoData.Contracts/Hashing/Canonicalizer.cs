// ============================================================
// FILE: 10_AstronoData.Contracts/src/Hashing/Canonicalizer.cs
// STATUS: FINAL (STRICT M1.9 + EMPTY OBJECT GUARD)
// ============================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace AstronoData.Contracts.Hashing
{
    public static class Canonicalizer
    {
        public static string Build(object input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var entries = new List<string>();

            Traverse(input, "", entries);

            // CRITICAL: prevent empty canonicalization
            if (entries.Count == 0)
                throw new InvalidOperationException(
                    "Canonicalization produced no entries. Invalid input object.");

            var sorted = entries
                .OrderBy(e => e, StringComparer.Ordinal)
                .ToList();

            var canonical = string.Join("\n", sorted);

            // DEBUG (MANDATORY)
            Console.WriteLine("=== CANONICAL STRING ===");
            Console.WriteLine(canonical);
            Console.WriteLine("========================");

            return canonical;
        }

        private static void Traverse(object obj, string prefix, List<string> entries)
        {
            if (obj == null)
                throw new InvalidOperationException($"Null value detected at '{prefix}'");

            var type = obj.GetType();

            // Leaf
            if (IsLeaf(type))
            {
                entries.Add($"{prefix}={NormalizeValue(obj)}");
                return;
            }

            // IEnumerable (order must be preserved!)
            if (obj is IEnumerable enumerable && !(obj is string))
            {
                int index = 0;

                foreach (var item in enumerable)
                {
                    var newPrefix = $"{prefix}[{index}]";
                    Traverse(item, newPrefix, entries);
                    index++;
                }

                return;
            }

            // Object → ONLY public properties
            var props = type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead)
                .OrderBy(p => p.Name, StringComparer.Ordinal)
                .ToList();

            foreach (var prop in props)
            {
                var value = prop.GetValue(obj);

                var newPrefix = string.IsNullOrEmpty(prefix)
                    ? prop.Name
                    : $"{prefix}.{prop.Name}";

                Traverse(value, newPrefix, entries);
            }
        }

        private static bool IsLeaf(Type type)
        {
            return type.IsPrimitive
                   || type == typeof(string)
                   || type == typeof(decimal)
                   || type == typeof(double)
                   || type == typeof(float)
                   || type == typeof(bool);
        }

        private static string NormalizeValue(object value)
        {
            switch (value)
            {
                case double d:
                    return NormalizeNumber(d);

                case float f:
                    return NormalizeNumber(f);

                case decimal m:
                    return NormalizeNumber((double)m);

                case bool b:
                    return b ? "true" : "false";

                case string s:
                    return s;

                default:
                    throw new InvalidOperationException(
                        $"Unsupported type in canonicalization: {value.GetType().Name}");
            }
        }

        private static string NormalizeNumber(double value)
        {
            double factor = Math.Pow(10, 9);
            double truncated = Math.Truncate(value * factor) / factor;

            return truncated.ToString("0.000000000", CultureInfo.InvariantCulture);
        }
    }
}