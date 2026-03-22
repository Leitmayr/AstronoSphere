using System;

namespace Astronometria.Time.Astro
{
    /// <summary>
    /// Represents a Terrestrial Time (TT) instant.
    /// Core time type for astronomical calculations.
    /// Immutable.
    /// </summary>
    public readonly struct TTInstant
    {
        private const double JD_J2000 = 2451545.0;
        private const double DaysPerJulianCentury = 36525.0;
        private const double DaysPerJulianMillennium = 365250.0;

        /// <summary>
        /// Julian Day (TT)
        /// </summary>
        public double JulianDayTT { get; }

        public TTInstant(double julianDayTT)
        {
            JulianDayTT = julianDayTT;
        }

        public double JulianCenturiesSinceJ2000()
        {
            return (JulianDayTT - JD_J2000) / DaysPerJulianCentury;
        }

        public double JulianMillenniaSinceJ2000()
        {
            return (JulianDayTT - JD_J2000) / DaysPerJulianMillennium;
        }

        public override string ToString()
            => $"JD(TT)={JulianDayTT}";
    }
}