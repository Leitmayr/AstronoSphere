//csharp ..\Astronometria.Projection.Tests\Viewport\ValueTypeToStringTests.cs
using Astronometria.Core.Coordinates;
using Astronometria.Projection.Viewport;
using NUnit.Framework;

namespace Astronometria.Projection.Tests.Viewport
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