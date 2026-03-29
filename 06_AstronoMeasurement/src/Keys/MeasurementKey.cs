// ============================================================
// FILE: 06_AstronoMeasurement/src/AstronoMeasurement/Keys/MeasurementKey.cs
// STATUS: NEU
// ============================================================

using System;

namespace AstronoMeasurement.Keys
{
    /// <summary>
    /// Unique identifier for a measurement.
    /// 
    /// KISS (M1):
    /// - Level + Type (VEC)
    /// - Used later for SOLL/IST comparison
    /// </summary>
    public sealed class MeasurementKey : IEquatable<MeasurementKey>
    {
        public string Level { get; }
        public string Type { get; } // VEC

        public MeasurementKey(string level, string type)
        {
            Level = level;
            Type = type;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as MeasurementKey);
        }

        public bool Equals(MeasurementKey other)
        {
            if (other == null) return false;

            return Level == other.Level &&
                   Type == other.Type;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Level, Type);
        }

        public override string ToString()
        {
            return $"{Level}-{Type}";
        }
    }
}