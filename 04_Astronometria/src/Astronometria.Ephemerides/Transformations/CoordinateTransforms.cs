using System;
using Astronometria.Core.Geometry;

namespace Astronometria.Ephemerides.Transformations
{
    /// <summary>
    /// Coordinate transformation utilities.
    /// </summary>
    internal static class CoordinateTransform
    {
        /// <summary>
        /// Converts a vector from ecliptic J2000
        /// to equatorial J2000 reference frame.
        /// </summary>
        public static Vector3 EclipticToEquatorial(Vector3 ecl)
        {
            double eps = Obliquity.Epsilon0;

            double cos = Math.Cos(eps);
            double sin = Math.Sin(eps);

            return new Vector3(
                ecl.X,
                ecl.Y * cos - ecl.Z * sin,
                ecl.Y * sin + ecl.Z * cos
            );
        }
    }
}
