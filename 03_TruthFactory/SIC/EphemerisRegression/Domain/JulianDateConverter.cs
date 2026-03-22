using EphemerisRegression.Mesh;
using System;

namespace EphemerisRegression.Domain
{
    public static class JulianDateConverter
    {
        public static double ToJulianDay(MeshUtc utc)
        {
            int y = utc.Year;
            int m = utc.Month;
            int d = utc.Day;

            if (m <= 2)
            {
                y -= 1;
                m += 12;
            }

            int A = y / 100;
            int B = 2 - A + A / 4;

            double jd =
                Math.Floor(365.25 * (y + 4716)) +
                Math.Floor(30.6001 * (m + 1)) +
                d + B - 1524.5;

            double dayFraction =
                (utc.Hour / 24.0) +
                (utc.Minute / 1440.0) +
                (utc.Second / 86400.0);

            return jd + dayFraction;
        }

        public static MeshUtc FromJulianDay(double jd)
        {
            double J = jd + 0.5;
            double Z = Math.Floor(J);
            double F = J - Z;

            double A = Z;
            double alpha = Math.Floor((A - 1867216.25) / 36524.25);
            A = A + 1 + alpha - Math.Floor(alpha / 4);

            double B = A + 1524;
            double C = Math.Floor((B - 122.1) / 365.25);
            double D = Math.Floor(365.25 * C);
            double E = Math.Floor((B - D) / 30.6001);

            double day = B - D - Math.Floor(30.6001 * E) + F;

            int month = (E < 14) ? (int)(E - 1) : (int)(E - 13);
            int year = (month > 2) ? (int)(C - 4716) : (int)(C - 4715);

            int intDay = (int)Math.Floor(day);

            double frac = day - intDay;

            int hour = (int)(frac * 24);
            int minute = (int)((frac * 24 - hour) * 60);
            int second = (int)((((frac * 24 - hour) * 60) - minute) * 60);

            return new MeshUtc(year, month, intDay, hour, minute, second);
        }
    }
}