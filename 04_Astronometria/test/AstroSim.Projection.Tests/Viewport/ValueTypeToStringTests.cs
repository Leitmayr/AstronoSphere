//csharp ..\AstroSim.Projection.Tests\Viewport\ValueTypeToStringTests.cs
using AstroSim.Core.Coordinates;
using AstroSim.Projection.Viewport;
using NUnit.Framework;

namespace AstroSim.Projection.Tests.Viewport
{
    [TestFixture]
    public class ValueTypeToStringTests
    {
        [Test]
        public void MapPoint01_ToString_FormatsCoordinates()
        {
            var mp = new MapPoint01(0.1, 0.25);
            var s = mp.ToString();

            Assert.That(s, Is.EqualTo("(0,1, 0,25)"));
        }

        [Test]
        public void PixelPoint_ToString_FormatsCoordinates()
        {
            var pp = new PixelPoint(400.0, 300.0);
            var s = pp.ToString();

            Assert.That(s, Is.EqualTo("(400, 300)"));
        }
    }
}