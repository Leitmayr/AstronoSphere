//AstroSim.Projection.Tests\Viewport\CenteredRadiusToPixelMapperTests.cs
using AstroSim.Projection.Viewport;
using NUnit.Framework;

namespace AstroSim.Projection.Tests.Viewport
{
    [TestFixture]
    public class CenteredRadiusToPixelMapperTests
    {
        [Test]
        public void CenteredToPixel_ZeroOffset_ReturnsCenter()
        {
            var mapper = new CenteredRadiusToPixelMapper();

            var p = new CenteredRadiusPoint(0.0, 0.0);
            var px = mapper.CenteredToPixel(p, centerXPx: 100.0, centerYPx: 200.0, radiusPx: 50.0);

            Assert.That(px.X, Is.EqualTo(100.0).Within(1e-12));
            Assert.That(px.Y, Is.EqualTo(200.0).Within(1e-12));
        }

        [Test]
        public void CenteredToPixel_PositiveUnitMultipliers_AppliesRadiusAndCenter()
        {
            var mapper = new CenteredRadiusToPixelMapper();

            var p = new CenteredRadiusPoint(1.0, 1.0);
            var px = mapper.CenteredToPixel(p, centerXPx: 0.0, centerYPx: 0.0, radiusPx: 10.0);

            Assert.That(px.X, Is.EqualTo(10.0).Within(1e-12));
            Assert.That(px.Y, Is.EqualTo(10.0).Within(1e-12));
        }

        [Test]
        public void CenteredToPixel_NegativeAndFractionalCoordinates_AreHandledCorrectly()
        {
            var mapper = new CenteredRadiusToPixelMapper();

            var p = new CenteredRadiusPoint(-1.0, 0.5);
            var px = mapper.CenteredToPixel(p, centerXPx: 20.0, centerYPx: 30.0, radiusPx: 5.0);

            Assert.That(px.X, Is.EqualTo(15.0).Within(1e-12));    // 20 + 5 * -1
            Assert.That(px.Y, Is.EqualTo(32.5).Within(1e-12));   // 30 + 5 * 0.5
        }

        [Test]
        public void CenteredToPixel_ZeroRadius_ReturnsCenterRegardlessOfPoint()
        {
            var mapper = new CenteredRadiusToPixelMapper();

            var p = new CenteredRadiusPoint(123.456, -987.654);
            var px = mapper.CenteredToPixel(p, centerXPx: 7.0, centerYPx: 9.0, radiusPx: 0.0);

            Assert.That(px.X, Is.EqualTo(7.0).Within(1e-12));
            Assert.That(px.Y, Is.EqualTo(9.0).Within(1e-12));
        }
    }
}