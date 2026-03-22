using System;

namespace AstroSim.Core.Site
{

    public sealed class ObservationSite
    {
        public double LongitudeDeg { get; }   // +E
        public double LatitudeDeg { get; }
        public double HeightMeters { get; }
        public TimeZoneInfo? TimeZone { get; } // nur für Ausgabe, niemals für Physik

        public ObservationSite(double longitudeDeg, double latitudeDeg, double heightMeters = 0, TimeZoneInfo? timeZone = null)
        {
            LongitudeDeg = longitudeDeg;
            LatitudeDeg = latitudeDeg;
            HeightMeters = heightMeters;
            TimeZone = timeZone;
        }
    }
}
