using System;
using Astronometria.Ephemerides.VSOP.Model;

namespace Astronometria.Ephemerides.VSOP.Calculation
{
    /// <summary>
    /// Performs VSOP coordinate computation.
    /// </summary>
    public static class VsopCalculator
    {
        /// <summary>
        /// Computes all three coordinates for a given planet and time T.
        /// T must be Julian millennia since J2000 (TT).
        /// </summary>
        public static double[] Compute(VsopPlanet planet, double T)
        {
            double[] result = new double[3];

            for (int i = 0; i < 3; i++)
                result[i] = ComputeCoordinate(planet.Coordinates[i], T);

            return result;
        }

        private static double ComputeCoordinate(VsopCoordinate coordinate, double T)
        {
            double value = 0.0;
            double Tn = 1.0;

            for (int n = 0; n < coordinate.Series.Length; n++)
            {
                double sum = 0.0;

                foreach (var term in coordinate.Series[n].Terms)
                {
                    sum += term.A * Math.Cos(term.B + term.C * T);
                }

                value += sum * Tn;
                Tn *= T;
            }

            return value;
        }
    }
}
