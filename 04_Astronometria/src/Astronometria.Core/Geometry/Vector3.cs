using System;

namespace Astronometria.Core.Geometry
{
    /// <summary>
    /// Represents a 3D Cartesian vector.
    /// Used for astronomical state vectors.
    /// </summary>
    public readonly struct Vector3
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3 Zero => new Vector3(0.0, 0.0, 0.0);

        public double Length()
        {
            return System.Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(
                a.X + b.X,
                a.Y + b.Y,
                a.Z + b.Z);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(
                a.X - b.X,
                a.Y - b.Y,
                a.Z - b.Z);
        }

        public static Vector3 operator *(Vector3 v, double scalar)
        {
            return new Vector3(
                v.X * scalar,
                v.Y * scalar,
                v.Z * scalar);
        }

        public static Vector3 operator /(Vector3 v, double scalar)
        {
            return new Vector3(
                v.X / scalar,
                v.Y / scalar,
                v.Z / scalar);
        }
    }
}
