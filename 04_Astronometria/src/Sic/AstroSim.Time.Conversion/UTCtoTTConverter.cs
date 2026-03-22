using System;
using AstroSim.Time.User;

namespace AstroSim.Time.Conversion
{
    public static class UTCtoTTConverter
    {
        private const double JD_J2000 = 2451545.0;
        private const double SecondsPerDay = 86400.0;
        private const double DaysPerJulianCentury = 36525.0;

        public static double ConvertToJulianDayTT(UTCInstant utc)
        {
            double hourDecimal =
                utc.Hour +
                utc.Minute / 60.0 +
                utc.Second / 3600.0;

            double jdUT = GregorianToJulianDate(
                utc.Year,
                utc.Month,
                utc.Day,
                hourDecimal);

            double deltaT = DeltaT(jdUT, utc.Year);

            return jdUT + deltaT / SecondsPerDay;
        }

        private static double GregorianToJulianDate(
            int year, int month, int day, double hour)
        {
            if (month <= 2)
            {
                year -= 1;
                month += 12;
            }

            int A = (int)Math.Floor(year / 100.0);
            int B = 2 - A + (int)Math.Floor(A / 4.0);

            return Math.Floor(365.25 * (year + 4716)) +
                   Math.Floor(30.6001 * (month + 1)) +
                   day + hour / 24.0 + B - 1524.5;
        }

        private static double DeltaT(double jd, int year)
        {
            double t = (jd - JD_J2000) / DaysPerJulianCentury;
            double y = 2000.0 + t * 100.0;

            if (year < 948)
                return 2177 + 497 * t + 44.1 * t * t;

            if (year < 1600)
                return 102 + 102 * t + 25.3 * t * t;

            if (year <= 2005)
            {
                double u = y - 2000.0;
                return 63.86 + u * (
                           0.3345 + u * (
                          -0.060374 + u * (
                           0.0017275 + u * (
                           0.000651814 + u * 0.00002373599))));
            }

            if (year <= 2050)
            {
                double u = y - 2000.0;
                return 62.92 + 0.32217 * u + 0.005589 * u * u;
            }

            double v = (year - 1820) / 100.0;
            return -20 + 32 * v * v;
        }
    }
}