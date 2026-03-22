using AstroSim.Core.Coordinates;

namespace Astronometria
{
    public sealed class EquatorialLine
    {
        public EquatorialCoord Start { get; }
        public EquatorialCoord End { get; }

        public EquatorialLine(EquatorialCoord start, EquatorialCoord end)
        {
            Start = start;
            End = end;
        }
    }
}
