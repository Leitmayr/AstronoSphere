using System.Collections.Generic;

namespace Astronometria.Ephemerides.VSOP.Model
{
    /// <summary>
    /// Represents one T^n series.
    /// Contains multiple VSOP terms.
    /// </summary>
    public sealed class VsopSeries
    {
        public List<VsopTerm> Terms { get; } = new();
    }
}