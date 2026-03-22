using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroSim.Core.Time
{
    public readonly struct AstroTimeUT
    {
        public double JulianDay { get; }

        public AstroTimeUT(double julianDay)
        {
            JulianDay = julianDay;
        }

        public override string ToString() => $"JD(UT)={JulianDay}";
    }
}
