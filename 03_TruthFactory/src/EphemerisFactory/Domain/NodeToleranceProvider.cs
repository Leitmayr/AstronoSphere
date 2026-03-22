using System;
using System.Collections.Generic;

namespace EphemerisRegression.Domain
{
    public sealed class NodeTolerance
    {
        public double PositionDelta { get; init; }
        public double VelocityDelta { get; init; }
    }

    public static class NodeToleranceProvider
    {
        private static readonly Dictionary<string, NodeTolerance> _map =
            new()
            {
                { "Mercury",  new() { PositionDelta = 1e-7, VelocityDelta = 1e-6 } },
                { "Venus",    new() { PositionDelta = 1e-7, VelocityDelta = 1e-6 } },
                { "Mars",     new() { PositionDelta = 5e-7, VelocityDelta = 5e-6 } },
                { "Jupiter",  new() { PositionDelta = 1e-6, VelocityDelta = 1e-5 } },
                { "Saturn",   new() { PositionDelta = 1e-5, VelocityDelta = 1e-4 } },
                { "Uranus",   new() { PositionDelta = 5e-5, VelocityDelta = 5e-4 } },
                { "Neptune",  new() { PositionDelta = 5e-4, VelocityDelta = 5e-3 } }
            };

        public static NodeTolerance Get(string planet)
        {
            if (!_map.TryGetValue(planet, out var tol))
                throw new InvalidOperationException(
                    $"No tolerance defined for {planet}");

            return tol;
        }
    }
}
