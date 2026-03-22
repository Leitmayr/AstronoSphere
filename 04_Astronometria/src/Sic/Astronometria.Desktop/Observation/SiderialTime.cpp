/**
* \file   SiderialTime.cpp
* \author Marcus Hiemer
* \date:  2026-01-12
* \brief
*
* Description:
*
* Change Log:
* - 2026-01-12: transferred from C++ to C#
* - 2015-06-16:
* 		- added member functions updateObject(.) and setSidTime(.) to be able to update existing object
* 		- constructor now calls updateObject(.), setSidTime(.) likewise
* - 2015-05-19:
* 		- arguments of all calculation methods is now a czeit Argument
*/

public class SiderialTime: conversions {
		/**
		 * \class  SiderialTime
		 * \brief  contains calculations of the siderial time
		 * 		   Base classes: conversions
		 *
		 * Description:
		 * this class calculated the mean and apparent siderial time in Greenwich. The apparent siderial time thereby considers deviations
		 * of the real vernal equinox from its mean representation which is taken for the calculation of the mean siderial time.
		 * The siderial time is the right ascension of the meridian with respect to the (true or mean) equinox.
		 * It is important to mention that all employed Julian Dates and the employed time instants are with respect to the Greenwich
		 * meridian. If the siderial time of an observation point shall be calculated, the mean/apparent siderial
		 * times at Greenwich have to be calculated and transformed to the geographical length of the observation point afterwards
		 * The calculations of this class are taken from Chapter 12 in Meeus, Astronomical Algorithms, pages 87ff.
		 */
		protected double _meanSidTime;	/**< mean siderial time in hours and float: */
		protected double _meanSidTime0;	/**< mean siderial time in hours at 0 UT and float: */
		protected double _appSidTime;		/**< apparent siderial time in hours and float: */


		/**
		 * \brief Liefert die mittlere Greenwich Zeit des Tagesanfangs
		 * @param 	arg_zeit: const reference on a czeit object.
		 * @return Zeitzone des Beobachtungsorts
		 */
		protected double calcMeanSiderealGreenwichTime0( const czeit arg_zeit );
		//-------------------------------------------------------------------------------------------------------------------------------


		/**
		 * public Memberfunktion, die als Argumente das JD des Tagesbeginns und die Zeit in UT uebergeben bekommt
		 * @param 	arg_zeit: const reference on a czeit object.
		 * @see 	calc_sternzeit(.)
		 * @return 	double. Rueckgabewert ist die mittlere Greenwich Zeit zum gewuenschten Zeitpunkt. Wir benoetigt zur Berechnung der Sternzeit.
		 */
		protected double calcMeanSiderealGreenwichTime( const czeit arg_zeit );
		//-------------------------------------------------------------------------------------------------------------------------------



		/**
		 * \brief the calculation of the *apparent* sidereal time at Greenwich is taken from Meeus, Astronomical Algorithms, 2nd edition, p.88
		 * Since the sidereal time is the hour angle of the vernal equinox at the Greenwhich meridian we must distinguish between the mean
		 * and the apparent sidereal time. Due to the obliquity of the ecliptic and the effect of nutation the apparent (=real) vernal equinox
		 * deviates from the mean vernal equinox and hence does the sidereal time.
		 * This method calculates the apparent sidereal time and it does consider nutation and obliquity of the ecliptic for this.
		 * @param 	arg_zeit: const reference on a czeit object.
		 * @return apparent sidereal time at Greenwich
		 */
		protected double calcAppSiderealGreenwichTime( const czeit arg_zeit );
		//-------------------------------------------------------------------------------------------------------------------------------


		/**
		 * \brief updates all attributes based on new time argument
		 * param arg_zeit new argument to calculate siderial time with
		 */
		protected void updateObject( const czeit arg_zeit );




		/**
		 * \brief constructor creating for the given time instant and for the Greenwich meridian the
		 * 		- mean siderial time
		 * 		- mean siderial time at 0UT
		 * 		- apparent siderial time
		 */
		public SiderialTime( const czeit arg_zeit );

		
		/**
		 * \brief set Function to update SiderialTime Object
		 * @param arg_zeit new argument to calculate siderial time with
		 */
		public void setSidTime( const czeit arg_zeit );


		/**
		 * \brief returns the mean siderial time at Greenwich at 0 UT
		 */
		public double get_meanSidTime0( void ) const
		{
			get { return( _meanSidTime0 ) };
		}

		/**
		 * \brief returns the apparent siderial time at Greenwich
		 */
		public double get_appSidTime( void ) const
		{
			get { return( _appSidTime ) };
		}

		SiderialTime( const czeit *arg_zeit )
		{
			updateObject( arg_zeit );
		}




	// ========================================= private methods =========================================

	protected void setSidTime( const czeit arg_zeit )
	{
		updateObject( arg_zeit );
	}



	protected void updateObject( const czeit arg_zeit )
	{
		_meanSidTime0 = calcMeanSiderealGreenwichTime0( arg_zeit );
		_meanSidTime = calcMeanSiderealGreenwichTime( arg_zeit );
		_appSidTime = calcAppSiderealGreenwichTime( arg_zeit );
	}

	// --------------------------------------------------------------------------------------------------------------------------
	protected double calcMeanSiderealGreenwichTime0( const czeit arg_zeit )
	{
		double loc_JD0 = arg_zeit->get_JD0();
		double T = ( loc_JD0 - astroConst.C__JD2000 )/tage_pro_jahrhundert;

		// Meeus S. 83, Gln. (11.3) in Stunden Gleitkomma
		// long double Theta_0 = (100.46061837 + 36000.770053608*T + 0.000387933*T*T - T*T*T/38710000); // in Grad!

		// Meeus S. 83, Gln. (11.3) in Stunden Gleitkomma
		// this is the optimized formula with less multiplications
		double Theta_0 = ( 100.46061837 + T* (36000.77005368 + T* ( 0.000387933 - T / 38710000 ))); // in Grad!

		// convert degrees to hours
		Theta_0 = deg2hour( Theta_0 );

		// limit to range [0...24]
		Theta_0 = ohne_ueberlauf_hour( Theta_0 );

		return ( Theta_0 );
	}
	// --------------------------------------------------------------------------------------------------------------------------


	// --------------------------------------------------------------------------------------------------------------------------
	double calcMeanSiderealGreenwichTime( const czeit arg_zeit )
	{
		// Taken from Montenbruck, Grundlagen der Epheremidenrechnung, p. 47
		// dies ist eine empirische Berechnungsformel
		// double temp = static_cast<double> ( 6.664520 + 0.0657098244*( arg_JD0 - 2451544.5 ) + 1.00273790935*arg_UniversalTimedouble );

		double loc_UT = ohne_ueberlauf_hour( arg_zeit->get_zeit() );

		// Meeus 2nd ed., p.87 bottom
		double ret_MeanSidGreenwichTime = 1.00273790935*loc_UT  + calcMeanSiderealGreenwichTime0( arg_zeit );

		// limit to [0...24]
		ret_MeanSidGreenwichTime  = ohne_ueberlauf_hour( ret_MeanSidGreenwichTime );

		return( ret_MeanSidGreenwichTime );
	}
	// --------------------------------------------------------------------------------------------------------------------------


	// --------------------------------------------------------------------------------------------------------------------------
	double ´calcAppSiderealGreenwichTime( const czeit arg_zeit )
	{


		double ret_AppSidGreenTime = 0.0;

		// -----------------------------------------------------------------------------------
		double loc_MeanSidTime = calcMeanSiderealGreenwichTime( arg_zeit );

		// create Nutation and Obliquity objects to get nutation in longitude and true obliquity of the ecliptic
		Nutation   loc_nutEffect   = new Nutation  ( arg_zeit );
		Obliquity  loc_obliqEffect = new Obliquity ( arg_zeit );

		// get nutation in longitude
		double loc_DeltaPsi = loc_nutEffect->getNutationInLongitude();

		// get true obliquity of the ecliptic
		double loc_epsilon  = loc_obliqEffect->getTrueObliquity();

		// -----------------------------------------------------------------------------------
		// note1: first loc_DeltaPsi has to be transformed to hours, since its original unit is arc seconds: deg2hour(.)
		// note2: the mean sid time is given in hours. Therefore, both DeltaPsi and loc_epsilon have to be transformed to hours: /C__sec_per_hour
		// note3: see also Example 13b, p. 95 in Meeus, Astronomical Algorithms, 2nd ed.
		ret_AppSidGreenTime = loc_MeanSidTime + ( deg2hour( loc_DeltaPsi ) * Math.cos( Math.PI/180*loc_epsilon ) ) / astroConst.C__sec_per_hour ;

		ret_AppSidGreenTime = ohne_ueberlauf_hour( ret_AppSidGreenTime );


		return ( ret_AppSidGreenTime );
	}
	// --------------------------------------------------------------------------------------------------------------------------
}