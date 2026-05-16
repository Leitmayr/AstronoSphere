using System.Collections.Generic;

namespace Astronometria.Core.ScientificRun.Hashing
{
    public sealed class DataHashModel
    {
        public List<DataHashSample> Samples { get; init; } = new();
    }

    public sealed class DataHashSample
    {
        public double JD { get; init; }

        public double X { get; init; }

        public double Y { get; init; }

        public double Z { get; init; }
    }
}