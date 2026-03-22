using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Astronometria.Core.Coordinates;
namespace Astronometria.Core.Domain;



public sealed class StarLine
{
    public string ConstellationIAU3 { get; }
    public EquatorialCoord Start { get; }
    public EquatorialCoord End { get; }

    public StarLine(string constellationIAU3, EquatorialCoord start, EquatorialCoord end)
    {
        ConstellationIAU3 = constellationIAU3;
        Start = start;
        End = end;
    }
}