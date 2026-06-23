using System;

namespace Astronometria.Core.ScientificRun.StateTree
{
    /// <summary>
    /// PURPOSE:
    /// Defines the internal canonical M2.4 physics StateTree node types.
    ///
    /// CONTEXT:
    /// M2.4.0 uses PHYS.* internally, while persisted ScientificRun JSON
    /// may still keep legacy VSOP87.* node names for baseline compatibility.
    ///
    /// CONSTRAINTS:
    /// This type must not contain target identity. Target identity belongs to NodeId.
    /// </summary>
    public sealed class PhysicsStateNodeType : IEquatable<PhysicsStateNodeType>
    {
        public static readonly PhysicsStateNodeType HelioEclJ2000VecL0 =
            new("PHYS.L0.HELIO.ECL.J2000.VEC");

        public static readonly PhysicsStateNodeType GeoEclJ2000VecL0 =
            new("PHYS.L0.GEO.ECL.J2000.VEC");

        public static readonly PhysicsStateNodeType GeoEquJ2000VecL0 =
            new("PHYS.L0.GEO.EQU.J2000.VEC");

        public string Value { get; }

        private PhysicsStateNodeType(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Physics node type must not be empty.", nameof(value));

            Value = value;
        }

        public static PhysicsStateNodeType FromValue(string value)
        {
            if (string.Equals(value, HelioEclJ2000VecL0.Value, StringComparison.Ordinal))
                return HelioEclJ2000VecL0;

            if (string.Equals(value, GeoEclJ2000VecL0.Value, StringComparison.Ordinal))
                return GeoEclJ2000VecL0;

            if (string.Equals(value, GeoEquJ2000VecL0.Value, StringComparison.Ordinal))
                return GeoEquJ2000VecL0;

            throw new NotSupportedException($"Unsupported M2.4 physics node type: '{value}'.");
        }

        public bool Equals(PhysicsStateNodeType? other)
        {
            if (other == null)
                return false;

            return string.Equals(Value, other.Value, StringComparison.Ordinal);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PhysicsStateNodeType);
        }

        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(Value);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}