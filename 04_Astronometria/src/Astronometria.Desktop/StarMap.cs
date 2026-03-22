using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Diagnostics;
using Astronometria.Projection.Viewport;
using Astronometria.Core.Coordinates;
using System.Windows; // für Point
using Astronometria.Core.Site;
using Astronometria.Core.Time;
using Astronometria.Projection.Projections;
using Astronometria.Adapters;

namespace Astronometria
{ 
    public class StarMapControl : FrameworkElement
    {

        #region nachUmbau
        // neu nach Umbau
        public MapViewport Viewport { get; } = new MapViewport();
        public IMapToPixelMapper MapToPixel { get; set; } = new CenteredMapToPixelMapper();
        public ICenteredRadiusToPixelMapper CenteredMapper { get; set; } = new CenteredRadiusToPixelMapper();
        public IEquatorialMapProjection Projection { get; set; } = new PolarEquatorialProjection(new ObsSituationHourAngleCalculator());

        private ObservationSite? _site;
        private AstroTimeUT? _timeUT;

        //private MapPoint01 ProjectEquatorialToMap01(EquatorialCoord eq)
        //{
        //    // 1) Radius (gnomonic/polar) wie in deiner Star-Klasse
        //    double r = (90.0 - eq.Decdeg) / (180.0 - myObsPoint.getGeogrLat);

        //    // 2) Gamma über Stundenwinkel
        //    ObsSituation sit = new ObsSituation(myTime, eq.RAdeg / 15.0, eq.Decdeg, myObsPoint.getGeogrLong, myObsPoint.getGeogrLat);
        //    double gammaDeg = 15.0 * sit.getHourAngle;

        //    // 3) centered cartesian
        //    double xCentered = r * Math.Sin(Math.PI / 180.0 * gammaDeg);
        //    double yCentered = r * Math.Cos(Math.PI / 180.0 * gammaDeg);

        //    // 4) Map01 (mit deinem bestätigten Spiegel)
        //    double mapX = 0.5 + xCentered / 2.0;
        //    double mapY = 0.5 - yCentered / 2.0;

        //    return new MapPoint01(mapX, mapY);
        //}

        private Point ToWpfPoint(double mapX, double mapY)
        {
            var px = MapToPixel.Map01ToPixel(new MapPoint01(mapX, mapY), Viewport);
            return new Point(px.X, px.Y);
        }

        protected override void OnRenderSizeChanged(System.Windows.SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            Viewport.WidthPx = ActualWidth;
            Viewport.HeightPx = ActualHeight;

            InvalidateVisual();
        }


        #endregion nachUmbau





        // alt aus vor Umbau

        public List<Star> Stars { get; private set; } = new List<Star>();
        public Dictionary<string, List<EquatorialLine>> ConstellationLines { get; set; }
        public observationPoint myObsPoint { get; set; }
        public czeit myTime { get; set; }
        
        // WICHTIG: für XAML
        public StarMapControl()
        {
            
        }

        // Optional
        public StarMapControl(List<Star> stars, observationPoint arg_myObs, czeit arg_timeOfObs) 
        {
            SetStars(stars);
 
        }


        public void SetStars(List<Star> stars)
        {
            Stars = stars ?? new List<Star>();

            // System.Diagnostics.Debug.WriteLine($"SetStars: {Stars.Count} Sterne");

            InvalidateVisual();
        }

        public void InitMap(observationPoint obs, czeit time, List<Star> stars)
        {
            myObsPoint = obs ?? throw new ArgumentNullException(nameof(obs));
            myTime = time ?? throw new ArgumentNullException(nameof(time));

            _site = new ObservationSite(myObsPoint.getGeogrLong, myObsPoint.getGeogrLat);
            _timeUT = new AstroTimeUT(myTime.JD); // <-- hier passend zu deiner API

            SetStars(stars);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            return finalSize;
        }

        protected override void OnRender(DrawingContext dc)
        {


            //Debug.WriteLine(    $"OnRender: Stars.Count = {Stars?.Count}");
            base.OnRender(dc);



            #region mapDisc
            // -------------------------------------------------------------------------------------
            double width = ActualWidth;
             double height = ActualHeight;

            if (width <= 0 || height <= 0)
                return;

            Point center = new Point(width / 2, height / 2);
            double radius = Math.Min(width, height) / 2;



            // Viewport soll genau die Kreisscheibe abdecken (2r x 2r)
            Viewport.WidthPx = 2 * radius;
            Viewport.HeightPx = 2 * radius;

            // in Control zentrieren: Pan verschiebt den Viewport in die echte Control-Koordinate
            Viewport.PanXPx = center.X - radius;
            Viewport.PanYPx = center.Y - radius;

            // Zoom zunächst neutral
            Viewport.Scale = 1.0;

            // 🔵 Hintergrund-Kreis
            dc.DrawEllipse(
                Brushes.DarkBlue,
                null,
                center,
                radius,
                radius
            );

            dc.PushClip(new EllipseGeometry(center, radius, radius));

            if (myObsPoint == null || myTime == null)
                return;
            // -------------------------------------------------------------------------------------
            #endregion mapDisc





            #region meridian
            // -------------------------------------------------------------------------------------
            Point loc_north = new Point(center.X, center.Y - radius);
            Point loc_south = new Point(center.X, center.Y + radius);

            Pen meridianPen = new Pen(Brushes.Black, 0.75);
            meridianPen.DashStyle = new DashStyle(new double[] { 16, 8 }, 0);
            dc.DrawLine(
                meridianPen,
                loc_north,
                loc_south);
            // -------------------------------------------------------------------------------------
            #endregion meridian


            #region equator
            // -------------------------------------------------------------------------------------
            double loc_obsPointLong = myObsPoint.getGeogrLong;
            double loc_obsPointLat = myObsPoint.getGeogrLat;

            double loc_radiusOfEquator = (90) / (180 - loc_obsPointLat)* radius;

            Pen equatorPen = new Pen(Brushes.Black, 0.75);
            equatorPen.DashStyle = new DashStyle(new double[] { 16, 8 }, 0);
            dc.DrawEllipse
            (
                    null,
                    equatorPen,
                    center,
                    loc_radiusOfEquator,
                    loc_radiusOfEquator
            );
            // -------------------------------------------------------------------------------------
            #endregion

            #region circumpolar
            // -------------------------------------------------------------------------------------
            double loc_circumpolarRadiusOfEquator = (loc_obsPointLat ) / (180 - loc_obsPointLat)* radius;
            Pen circumpolarPen = new Pen(Brushes.Black, 0.5);
            circumpolarPen.DashStyle = new DashStyle(new double[] { 8, 4 }, 0);
            dc.DrawEllipse
            (
                    null,
                    circumpolarPen,
                    center,
                    loc_circumpolarRadiusOfEquator,
                    loc_circumpolarRadiusOfEquator
            );
            #endregion circumpolar
            // -------------------------------------------------------------------------------------


            #region poleEcliptic
            // -------------------------------------------------------------------------------------
            double loc_RAofPole = 18;
            double loc_DecOfPole = 66.55;       //  magic number

            // Pole of the ecliptic
            ObsSituation myObsSit = new ObsSituation(myTime, loc_RAofPole, 66.55, loc_obsPointLong, loc_obsPointLat);
            double loc_gammaOfPole = 15 * myObsSit.getHourAngle;
            double loc_radiusOfPole = (90 - loc_DecOfPole) / (180 - loc_obsPointLat);

            // determine gnomonic cartesian coordinates (xGnomCart, yGnomCart)
            double xOfPole = loc_radiusOfPole * Math.Sin(Math.PI / 180 * loc_gammaOfPole);
            double yOfPole = loc_radiusOfPole * Math.Cos(Math.PI / 180 * loc_gammaOfPole);

            double loc_centerOfPoleX = center.X + radius * xOfPole;
            double loc_centerOfPoleY = center.Y + radius * yOfPole;

            Point loc_centerOfPole = new Point(loc_centerOfPoleX, loc_centerOfPoleY);
            double loc_poleRadiusToDraw = radius * 90 / (180 - loc_obsPointLat);

            //draw pole of the ecliptic

            Pen eclipticPen = new Pen(Brushes.Red, 0.75);
            eclipticPen.DashStyle = new DashStyle(new double[] { 16, 8 }, 0);
            dc.DrawEllipse
            (
                    null,
                    eclipticPen,
                    loc_centerOfPole,
                    loc_poleRadiusToDraw,
                    loc_poleRadiusToDraw
            );
            // -------------------------------------------------------------------------------------
            #endregion poleEcliptic


            #region constellations
            // -------------------------------------------------------------------------------------
            // ⭐ Constellations zeichnen
            if (ConstellationLines == null)
                return;

            Pen pen = new Pen(Brushes.LightGray, 0.25);

            foreach (var kv in ConstellationLines)
            {
                foreach (var line in kv.Value)
                {
                    var m1 = Projection.Project(line.Start, _site, _timeUT.Value);
                    var m2 = Projection.Project(line.End, _site, _timeUT.Value);

                    var p1 = ToWpfPoint(m1.X, m1.Y);
                    var p2 = ToWpfPoint(m2.X, m2.Y);

                    dc.DrawLine(pen, p1, p2);
                }
            }
            // -------------------------------------------------------------------------------------
            #endregion constellations



            #region stars
            // -------------------------------------------------------------------------------------
            // ⭐ Sterne zeichnen
            var starBrush = new SolidColorBrush(
                Color.FromArgb(180, 255, 255, 255) // leicht transparent
            );


            foreach (var star in Stars)
            {
                // Optional: nur sichtbare Punkte zeichnen
                if (star._posGnomCart.X < 0 || star._posGnomCart.X > 1 ||
                    star._posGnomCart.Y < 0 || star._posGnomCart.Y > 1)
                    continue;

                // Map01 -> PixelPoint -> WPF Point
                var px = MapToPixel.Map01ToPixel(
                    new Astronometria.Core.Coordinates.MapPoint01(star._posGnomCart.X, star._posGnomCart.Y),
                    Viewport);

                Point tempPoint = new Point(px.X, px.Y);
                //Debug.WriteLine($"Star: X={star._posGnomCart.X:F3}, Y={star._posGnomCart.Y:F3}");

                Color starColor = ColorFromSpectralType(star._specType);
                

                double r = RadiusFromMagnitude(star._mag);
                double alpha = 1; // AlphaFromMagnitude(star._mag);

                byte a = (byte)(alpha*255);

                Color finalColor = Color.FromArgb(a, starColor.R, starColor.G, starColor.B);

                var brush = new SolidColorBrush(finalColor);

                // Color aufrufen

                dc.DrawEllipse(brush, null, tempPoint, r, r);
            }
            #endregion stars
            // -------------------------------------------------------------------------------------


            dc.Pop();
        }

        protected double AlphaFromMagnitude(double m)
        {
            double alpha = 1.0 - (m + 1.46) / 7.96;
            return Math.Max(0.25, Math.Min(1.0, alpha));
        }

        protected double RadiusFromMagnitude(double m)
        {
            double r = 1.0 + 4.0 * Math.Pow(10, -0.2 * (m + 1.46));
            r = Math.Max(1.1, Math.Min(8.0, r));
            return r;
        }

        protected Color ColorFromSpectralType(string arg_SpectralType)
        {

            if (string.IsNullOrEmpty(arg_SpectralType))
                return Colors.White;

            char letter = char.ToUpper(arg_SpectralType[0]);
            int number = 5; // Default Mittelwert
            if (arg_SpectralType.Length > 1 && char.IsDigit(arg_SpectralType[1]))
                number = arg_SpectralType[1] - '0';

            // Basisfarben O->M
            Color colorStart, colorEnd;
            switch (letter)
            {
                case 'O': colorStart = Color.FromRgb(155, 176, 255); colorEnd = Color.FromRgb(155, 176, 255); break;
                case 'B': colorStart = Color.FromRgb(170, 191, 255); colorEnd = Color.FromRgb(155, 176, 255); break;
                case 'A': colorStart = Color.FromRgb(202, 215, 255); colorEnd = Color.FromRgb(170, 191, 255); break;
                case 'F': colorStart = Color.FromRgb(248, 247, 255); colorEnd = Color.FromRgb(202, 215, 255); break;
                case 'G': colorStart = Color.FromRgb(255, 244, 234); colorEnd = Color.FromRgb(248, 247, 255); break;
                case 'K': colorStart = Color.FromRgb(255, 210, 161); colorEnd = Color.FromRgb(255, 244, 234); break;
                case 'M': colorStart = Color.FromRgb(255, 204, 111); colorEnd = Color.FromRgb(255, 210, 161); break;
                default: return Colors.White;
            }

            // lineare Interpolation nach Zahl (0–9)
            double t = number / 9.0;
            byte r = (byte)(colorStart.R * (1 - t) + colorEnd.R * t);
            byte g = (byte)(colorStart.G * (1 - t) + colorEnd.G * t);
            byte b = (byte)(colorStart.B * (1 - t) + colorEnd.B * t);

            return Color.FromRgb(r, g, b);
        }
    }
}