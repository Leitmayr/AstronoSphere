namespace EphemerisRegression.Mesh
{
    public readonly struct MeshUtc
    {
        public int Year { get; }
        public int Month { get; }
        public int Day { get; }
        public int Hour { get; }
        public int Minute { get; }
        public int Second { get; }

        public MeshUtc(
            int year,
            int month,
            int day,
            int hour = 0,
            int minute = 0,
            int second = 0)
        {
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;
        }

        public string ToIsoString()
        {
            return $"{Year:D4}-{Month:D2}-{Day:D2}T{Hour:D2}:{Minute:D2}:{Second:D2}";
        }

        public override string ToString() => ToIsoString();
    }
}