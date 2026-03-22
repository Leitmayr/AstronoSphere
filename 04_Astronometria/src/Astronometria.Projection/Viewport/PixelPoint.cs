using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astronometria.Projection.Viewport
{
    public readonly struct PixelPoint
    {
        public double X { get; }
        public double Y { get; }

        public PixelPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString() => $"({X}, {Y})";
    }
}
