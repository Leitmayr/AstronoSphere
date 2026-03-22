using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astronometria.Core.Time
{

    public readonly struct TimeRangeUT
    {
        public AstroTimeUT Start { get; }
        public AstroTimeUT End { get; }

        public TimeRangeUT(AstroTimeUT start, AstroTimeUT end)
        {
            Start = start;
            End = end;
        }

        public override string ToString() => $"{Start} .. {End}";
    }
}
