using AstroSim.Core.Coordinates;
using AstroSim.Projection.Viewport;
using NUnit.Framework;

namespace AstroSim.Projection.Tests.Viewport
{
    [TestFixture]
    public class CenteredMapToPixelMapperTests
    {
        [Test]
        public void MapCenter_ShouldMapToScreenCenter()
        {
            var vp = new MapViewport
            {
                WidthPx = 800,
                HeightPx = 600,
                Scale = 1.0,
                PanXPx = 0,
                PanYPx = 0
            };

            var mapper = new CenteredMapToPixelMapper();

            // MapPoint01 center
            var p = new MapPoint01(0.5, 0.5);
            var px = mapper.Map01ToPixel(p, vp);

            Assert.That(px.X, Is.EqualTo(400).Within(1e-12));
            Assert.That(px.Y, Is.EqualTo(300).Within(1e-12));
        }

        [Test]
        public void MapTopRight_ShouldMapToTopRight_WhenNoPanNoScale()
        {
            var vp = new MapViewport
            {
                WidthPx = 800,
                HeightPx = 600,
                Scale = 1.0,
                PanXPx = 0,
                PanYPx = 0
            };

            var mapper = new CenteredMapToPixelMapper();

            // Top-right in Map01: X=1, Y=1
            // X -> 800, Y -> 0 (wegen invertierter Y-Achse)
            var p = new MapPoint01(1.0, 1.0);
            var px = mapper.Map01ToPixel(p, vp);

            Assert.That(px.X, Is.EqualTo(800).Within(1e-12));
            Assert.That(px.Y, Is.EqualTo(0).Within(1e-12));
        }

        [Test]
        public void PanAndScale_ShouldAffectPixelCoordinates()
        {
            var vp = new MapViewport
            {
                WidthPx = 800,
                HeightPx = 600,
                Scale = 2.0,
                PanXPx = 10,
                PanYPx = -20
            };

            var mapper = new CenteredMapToPixelMapper();

            // Ein Punkt leicht rechts/oben vom Zentrum
            var p = new MapPoint01(0.6, 0.6);

            // Erwartung (kurz hergeleitet):
            // cx=400, cy=300
            // mx=(0.6-0.5)*800=80
            // my=(0.6-0.5)*600=60
            // x=400 + 80*2 + 10 = 570
            // y=300 - 60*2 - 20 = 160
            var px = mapper.Map01ToPixel(p, vp);

            Assert.That(px.X, Is.EqualTo(570).Within(1e-12));
            Assert.That(px.Y, Is.EqualTo(160).Within(1e-12));
        }
    }
}