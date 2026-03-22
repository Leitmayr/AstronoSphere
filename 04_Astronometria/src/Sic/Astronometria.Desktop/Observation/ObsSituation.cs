using System;
/*
 * \file   ObsSituation.cs
 * \author Marcus Hiemer
 * \date:  2026-01-12
 * \brief implements the class ObsSituation
 *
 * Description:
 * implements the class ObsSituation. Equations taken from Chapter 13 in Meeus, Astronomical Algorithms, 2nd ed.
 *
 *
 * Change Log:
 * 2026-01-12: transferred from C++ to C#
 *
 * 2015-06-03: implements member functions calcHourAngle(.), calcHorizontalAltitude(.) and calcHorizontalAzimuth(.)
 * 2015-06-05:
 * - changed constructor: GA_coord no argument any more. Replaced by ra and dek in float instead
 * - changed interface to calcHourAngle: arg_GALong now in hours and float instead of degrees and float
 * - calcHourAngle(.): bugfixes of issues found by unit tests
 * 		+ changed use of apparent siderial time now instead of mean siderial time
 * 		+ added geogr. longitude instead of subtracting it (western coordinates given negatively)
 * 		+ ohne_ueberlauf_hour(.) taken for overflow handling instead of minusPlus12(.)
 *	- calcAltitude(.): bugfixes of issues found by unit tests
 *		+ added asin(.) in formula for altitude
 *	- calcAzimuth(.): : bugfixes of issues found by unit tests
 *		+ corrected second term in atan2(.) formular
 *		+ minusplus180(.) geändert in ohne_ueberlauf_degrees(.)
 * 2015-06-16:
 * - changed algebraic sign of the hour angle as per definition in Meeus: western geographical longitudes: +, eastern longitudes: -
 * - added member functions updateObject(.) and setObsSituation(.) to be able to update existing object
 * - constructor now calls updateObject(.), setObsSituation(.) likewise
 */

class ObsSituation :  conversions 
{
	/**
		 * \class  ObsSituation
		 * \brief  calculates a celestial observing situation like the position of a planet as seen from a particular geographical
		 * 		   point at a given instant of time
		 *
		 * Description:
		 * this class calculates the position of celestial object as seen from a geographical point on the earth. For this, the
		 * time instant of observation, the geographic location as well as the celestial object's geocentric equatorial coordinates
		 * have to be handed over to the class object. The class then calculates the siderial time at the geographical point of observation, hence the hour angle and finally the
		 * azimuth and altitude (=height) of the celestial object in local horizontal coordinates.
		 *
		 */


		protected double _hourAngle;	/**< hourAngle: angle in hours float [0..24] - positive west of the south meridian, negative east of south meridian*/
		protected double _azimuth; 	/**< azimuth: angle in degrees [0..360] measured westward from the south: south: 0°, west: 90°, north: 180°, east: 270° */
		protected double _altitude; 	/**< altitude: angle in degrees[-90..90] - positive above the horizon, negative below, 0° on the horizontal line */



		//protected double calcHourAngle( const czeit arg_timeInstant, const double arg_GALong, const double arg_geogrLong );
		//// ------------------------------------------------------------------------------------------------------------------



		/**
			 * \brief returns local hour angle
			 * @return _hourAngle: angle in hours float - positive west of the south meridian, negative east of south meridian
		*/
		public double getHourAngle
		{
			get { return (_hourAngle); }
		}

		/**
			 * \brief returns altitude of a celestial object on a geographical observation point for a given time instant
			 * @return _altitude: angle in degrees - positive above the horizon, negative below, 0° on the horizontal line
		*/
		public double getAltitude
		{
			get { return (_altitude); }
		}

			/**
				 * \brief returns azimuth of a celestial object on a geographical observation point for a given time instant
				 * @return _azimuth: azimuth: angle in degrees measured westward from the south: south: 0°, west: 90°, north: 180°, east: 270°
			*/
		public double getAzimuth
		{
			get { return (_azimuth); }
		}



	///**
	// * \brief Constructor receveing time and coordinates of celestial object and georgraphical observation point.
	// * Calculating hour angle, altitude in horizontal coordinates, azimuth in horizontal coordinates and assigning them to the class attributes
	// * @param arg_GA_ra right ascension of celestial object in hours and float
	// * @param arg_GA_dek declination of celestial object in degrees and float
	// * @param arg_geogrLong geographical longitude of observation point on earth in degrees and float
	// * @param arg_geogrLat geographical latitude of observation point on earth in degrees and float
	// */
	public ObsSituation(	 czeit arg_timeInstant,
								 double arg_GA_ra,
								 double arg_GA_dek,
								 double arg_geogrLong,
								 double arg_geogrLat)

	{
		updateObject( arg_timeInstant, arg_GA_ra, arg_GA_dek, arg_geogrLong, arg_geogrLat );
	}


	///**
	// * \brief set Function to update ObsSituation Object
	// * @param arg_zeit new argument to calculate observation situation with
	// * @param arg_GA_ra right ascension of celestial object in hours and float
	// * @param arg_GA_dek declination of celestial object in degrees and float
	// * @param arg_geogrLong geographical longitude of observation point on earth in degrees and float
	// */
	public void setObsSituation(  czeit arg_timeInstant,
										 double arg_GA_ra,
										 double arg_GA_dek,
										 double arg_geogrLong,
										 double arg_geogrLat )
	{
		updateObject( arg_timeInstant, arg_GA_ra, arg_GA_dek, arg_geogrLong, arg_geogrLat );

	}


	// ==================================================================================================================


	/**
	 * \brief updates all attributes based on new time argument
	 * param arg_zeit new argument to calculate siderial time with
	 * @param arg_GA_ra right ascension of celestial object in hours and float
	 * @param arg_GA_dek declination of celestial object in degrees and float
	 * @param arg_geogrLong geographical longitude of observation point on earth in degrees and float
	 * @param arg_geogrLat geographical latitude of observation point on earth in degrees and float
	 */
	public void updateObject( 	 czeit arg_timeInstant,
										 double arg_GA_ra,
										 double arg_GA_dek,
										 double arg_geogrLong,
										 double arg_geogrLat )
	{
		_hourAngle = calcHourAngle( arg_timeInstant, arg_GA_ra, arg_geogrLong );
		_altitude  = calcHorizontalAltitude( _hourAngle, arg_GA_dek, arg_geogrLat );
		_azimuth   = calcHorizontalAzimuth( _hourAngle, arg_GA_dek, arg_geogrLat );
	}


	/**
	 * \brief calculates the local hour angle at the geographical position where the celestial observation takes place
	 * @param  arg_timeInstant time object at which the celestial observation takes place
	 * @param  arg_GALong right ascension (GA longitude) of the celestial object in hours and float
	 * @param  arg_geogrLong geographical longitude of the observation point in degrees: -: west of Greenwich, + east of Greenwich
	 * @return local hour angle of the celestial object in hours and float limited to [0...+24]
	 */
	protected static double calcHourAngle(  czeit arg_timeInstant,  double arg_GALong,  double arg_geogrLong )
	{
		double ret_hourAngle = 0.0;

		SiderialTime sidTime = new SiderialTime( arg_timeInstant );
		double loc_appSidTimeAtGreenwich = sidTime.get_appSidTime; // return in hours and float

		// formula taken from Meeus, Astronomical Algorithms, 2nd ed., p. 92
		// H = SiderialTimeAtGreenwhich - right ascension - geographical Longitude
		ret_hourAngle = loc_appSidTimeAtGreenwich - arg_GALong - arg_geogrLong/astroConst.C__deg_per_hour;

		// limit to [-0.0...+24.0]
		ret_hourAngle = ohne_ueberlauf_hour( ret_hourAngle );

		return (ret_hourAngle);
	}
	// --------------------------------------------------------------------------------------------------------------------



	/**
	 	 * \brief calculates the altitude of an object in horizontal coordinates: + above horizon, - below horizon
	 	 * @param arg_hourAngle local hour angle in hours and float
	 	 * @param arg_GALat declination (GA latitude) of celestial object in degrees
	 	 * @param  arg_geogrLat geographical latitude of the observation point in degrees: +: north of equator, -: south of equator
		 * @return altitude of celestial object over horizon (neg. below horizon) in deg limited to [-90°...90°]
	  */
	protected static double calcHorizontalAltitude(  double arg_hourAngle,  double arg_GALat,  double arg_geogrLat )
	{
		double ret_altitude = 0.0;

		// convert the float hours to float deg for later use in sin(.)-function
		double loc_hourAngleDeg = astroConst.C__deg_per_hour * arg_hourAngle;

		// Meeus, Astronomical Algorithms, 2nd ed., p. 93, eqn. (13.6)
		ret_altitude = 	180/Math.PI*Math.Asin(
					Math.Sin( Math.PI/180*arg_geogrLat ) * Math.Sin( Math.PI/180*arg_GALat )
					+   Math.Cos( Math.PI/180*arg_geogrLat ) * Math.Cos( Math.PI/180*arg_GALat ) * Math.Cos( Math.PI/180*loc_hourAngleDeg )
					);

		// limit to [-90...+90]
		ret_altitude = ohne_ueberlauf_declination( ret_altitude );

		return ( ret_altitude );
	}
	// --------------------------------------------------------------------------------------------------------------------



	/**
		 * \brief calculates the azimuth of an object in horizontal coordinates: + westward of south, - eastward of south
		 * @param arg_hourAngle local hour angle in hours and float
		 * @param arg_GALat declination (GA latitude) of celestial object in degrees
		 * @param  arg_geogrLat geographical latitude of the observation point in degrees: +: north of equator, -: south of equator
		 * @return azimuth of a celestial object with respect to the south meridian in deg (neg. to east) limited to [0°...360°]
	*/
	protected static double calcHorizontalAzimuth( double arg_hourAngle,  double arg_GALat,  double arg_geogrLat )
	{
		double ret_azimuth = 0.0;

		// convert the float hours to float deg for later use in Math.Sin(.)-function
		double loc_hourAngleDeg = astroConst.C__deg_per_hour * arg_hourAngle;

		// Meeus, Astronomical Algorithms, 2nd ed., p. 93, eqn. (13.5)
		ret_azimuth = 180/Math.PI*Math.Atan2( Math.Sin( Math.PI/180*loc_hourAngleDeg ),
									Math.Cos( Math.PI/180*loc_hourAngleDeg )*Math.Sin( Math.PI/180*arg_geogrLat )-
									Math.Tan( Math.PI/180*arg_GALat )*Math.Cos( Math.PI/180*arg_geogrLat )
								   );

		ret_azimuth = ohne_ueberlauf_degrees( ret_azimuth );

		return ( ret_azimuth );

	}
	// --------------------------------------------------------------------------------------------------------------------


}