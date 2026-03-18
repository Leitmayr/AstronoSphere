using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;



public class Star
{
    #region attributes
    public Point _posGA { get; set; }           // Geocentric equatorial coordinates of the star: RA in deg [0, 360], Dec in deg [-90...90]
    public Point _posGnomPolar { get; set; }    // Polaris in center, polar coordinates (r, gamma). r normalized [0..1], gamma in deg [0...360]
    public Point _posGnomCart{ get; set; }      // Polaris in center, cartesian coordinates (xGnomCart, yGnomCart). x and y normalized [0...1]

    // Horizontal coordinates of a star 
    public Point _posHorPolar { get; set; }     // Zenith in center, polar coordinates
    public Point _posHorCart { get; set; }      // Zenith in center, cartesian coordinates
    

    public double _mag { get; set; }
    public string _specType { get; set; }
    public Color _starColor { get; set; }

    #endregion 

    #region Constructor
    public Star(double arg_RA, double arg_Dec, double arg_mag, string arg_specType, observationPoint myObsPoint, czeit arg_time)
    {

        #region star related data from data base        
        _posGA = new Point (arg_RA, arg_Dec);
        _mag = arg_mag;
        _specType = arg_specType;
        #endregion


        #region map related data of the star
        czeit loc_time = arg_time;
        
        // setup geografic coordinates
        double loc_geoLong = myObsPoint.getGeogrLong;
        double loc_geoLat = myObsPoint.getGeogrLat;
        Point geoCoord = new Point(loc_geoLong, loc_geoLat);
                
        // determine gnomonic polar coordinates (r, gamma)
        double loc_radius = calcGnomPolarRadius(loc_geoLat, _posGA.Y);
        double loc_gamma = calcGamma(loc_time, _posGA, geoCoord);
        _posGnomPolar = new Point(loc_radius, loc_gamma);

        // determine gnomonic cartesian coordinates (xGnomCart, yGnomCart)
        double xCentered = _posGnomPolar.X * Math.Sin(Math.PI / 180 * _posGnomPolar.Y);
        double yCentered = _posGnomPolar.X * Math.Cos(Math.PI / 180 * _posGnomPolar.Y);

        // Map01: Center = (0.5, 0.5)
        double mapX = 0.5 + xCentered / 2.0;
        double mapY = 0.5 - yCentered / 2.0;

        // ChatGPT:
        // Optional: wenn du *nur* sichtbare Punkte zeichnen willst, kannst du später im Rendering clippen.
        // Ich würde hier NICHT clampen, damit off-map Sterne erkennbar "außerhalb" liegen.
        _posGnomCart = new Point(mapX, mapY);
        #endregion
    }
    #endregion

    double calcGnomPolarRadius(double arg_geoLat, double arg_Dec)
    {
        double ret_GnomPolarRadius = (90 - arg_Dec) / (180 - arg_geoLat);
        return (ret_GnomPolarRadius);
    }

    double calcGamma(czeit arg_time, Point arg_GA, Point arg_GeoCoord )
    {
        double loc_RA = arg_GA.X;
        double loc_Dec = arg_GA.Y;

        double loc_geoLong = arg_GeoCoord.X;
        double loc_geoLat = arg_GeoCoord.Y;

        ObsSituation mySituation = new ObsSituation(arg_time, loc_RA / 15, loc_Dec, loc_geoLong, loc_geoLat);
        double loc_hourAngle = mySituation.getHourAngle;

        double ret_Gamma = 15 * loc_hourAngle;

        return (ret_Gamma);
    }


   
}