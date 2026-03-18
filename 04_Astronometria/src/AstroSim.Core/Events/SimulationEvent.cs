

using AstroSim.Core.Time;

namespace AstroSim.Core.Events;

public sealed class SimulationEvent
{
    public string Type { get; }
    public AstroTimeUT Time { get; }
    public string[] Bodies { get; }
    public string? Details { get; }

    public SimulationEvent(string type, AstroTimeUT time, string[] bodies, string? details = null)
    {
        Type = type;
        Time = time;
        Bodies = bodies;
        Details = details;
    }
}
