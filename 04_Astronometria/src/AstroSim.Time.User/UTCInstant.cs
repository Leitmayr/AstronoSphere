using System;

namespace AstroSim.Time.User
{
    public readonly struct UTCInstant
    {
        public int Year { get; }
        public int Month { get; }
        public int Day { get; }
        public int Hour { get; }
        public int Minute { get; }
        public int Second { get; }

        public UTCInstant(int year, int month, int day,
                          int hour, int minute, int second)
        {
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;
        }

        public override string ToString()
            => $"{Year:D4}-{Month:D2}-{Day:D2} {Hour:D2}:{Minute:D2}:{Second:D2} UTC";
    }
}