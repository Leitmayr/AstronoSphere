/**
 * \file   observationPoint.cpp
 * \author Marcus Hiemer
 * \date:  2026-01-12
 * \brief
 *
 * Description:
 *
 * Change Log:
 * 2026-01-12: transferred from C++ to C#
 *
 * 2015-01-22: created
 * 2015-05-14:
 * - renamed calcSiderealTime(.) by calcLocalSiderealTime(.)
 * - additional use of overflow protection methods inherited from conversions: ohne_ueberlauf_hour, ohne_ueberlauf_degrees
 * - local overflow protection code replaced by inherited overflow-methods ohne_ueberlauf_... from class conversions
 * - calcMeanSiderealGreenwichTime0(.) now calculated based on Meeus and not on Montenbruck
 * - implemented calcAppSiderealGreenwichTime(.)
 * - some additional comments added, code better structured, some refactoring done
 * 2015-05-26:
 * - BUGFIX: ohne_ueberlauf_degrees(.) replaced by ohne_ueberlauf_declination(.) in constructors
 * - BUGFIX: ohne_ueberlauf_hours(.) replaced by minusPlus180(.) in constructors
 * - BUGFIX: ohne_ueberlauf_hour(.) introduced in methods calcLocalMeanSiderealTime(.) and calcLocalAppSiderealTime(.) for overflow handling of result
 * 2015-06-16:
 * - changed formulae to calculate ret_SidTime in calcLocalMeanSiderealTime(.) and calcLocalAppSiderealTime(.): western longitudes now positive,
 * eastern negative
 */



/**
 * \class 	observationPoint
 * \brief 	observationPoint describes a geographical observation point on the earth. The class inherits form conversions.
 * 			An observationPoint object contains name and geographical coordinates of that location in degrees and float.
 * 			It calculates the local mean and apparent siderial time and offers opportunities, to calculate the diurnal path of
 * 			a celestial object with respect to that location on the earth.
 */
class observationPoint : conversions
{
private:
	protected string _nameOfObservationPoint; 					/**< name of the observation point */
	protected double _localMeanSiderealTime;		/**< local mean siderial time (mittlere Sternzeit) at the observation point */
	protected double _localAppSiderealTime;		/**< local apparent siderial time (wahre Sternzeit) at the observation point */
	protected double _localJulDat;				/**< local Julian Day of the observation point */
	protected double _geogrLong;					/**< geogr. long. of observation point in deg and float with respect to Greenwich: + towards west, - towards east */
	protected double _geogrLat;					/**< geogr. lat. of observation point in deg and float with respect to equator: + towards north, - towards south */
	protected int _timeZone;						/**< time zone of the observation point: - towards west, + towards east*/
	// =========================================================================


	/**
	 * \brief calculates the local mean sidereal time from the Julian Day at 0UT and the UT of a time instant
	 * @param 	arg_zeit: zeit object
	 * @return 	double. return is the local mean sidereal time at arg_zeit.
	 */
	protected double calcLocalMeanSiderealTime( const czeit *arg_zeit );
	//-------------------------------------------------------------------------------------------------------------------------------


	/**
	 * \brief calculates the local mean sidereal time from the Julian Day at 0UT and the UT of a time instant
	 * @param 	arg_zeit: zeit object
	 * @return 	double. return is the local apparent sidereal time at arg_zeit.
	 */
	protected double calcLocalAppSiderealTime( const czeit *arg_zeit );
	//-------------------------------------------------------------------------------------------------------------------------------


	/**
	 * private Memberfunktion, die als Argumente das JD des Tagesbeginns und die Zeit im Fliesskommaformat uebergeben bekommt
	 * @param 	arg_zeit: zeit object
	 * @return 	double. Rueckgabewert ist die lokale Zeit am Beobachtungszeitpunkt zum Zeitpunkt zeit_comma (ohne Beruecksichtigung der Sommerzeit)
	 */
	protected double calcLocalJulDat( const czeit *arg_zeit );
	//-------------------------------------------------------------------------------------------------------------------------------




	//Konstruktoren


	/**KONSTRUKTOR1
	 * Standardkonstruktor, waehlt den Standardbeobachtungsort und die aktuelle zeit ueber den Standardkonstruktor von czeit
	 * */
	public observationPoint();
	//-------------------------------------------------------------------------------------------------------------------------------



	/**KONSTRUKTOR2
	 * Standardkonstruktor,  waehlt den Standardbeobachtungsort und die als JD uebergebene Zeit
	 * @param *arg_zeit: Zeiger auf Zeitobjekt (Datum und Uhrzeit)
	 * @param zz: Zeitzone des Beobachtungsortes (positiv nach Osten)
	 */
	public observationPoint( const czeit *arg_zeit, const int &zz );
	//-------------------------------------------------------------------------------------------------------------------------------

	/**KONSTRUKTOR3
	 * Waehlt uebergebenen Ort mit Koordinaten und als Zeit-Objekt übergebenen Zeitpunkt
	 * @param ortsname: Name des Beobachtungsorts
	 * @param rdoub: geografische Laenge des Beobachtungsorts in Grad (Format Gleitkomma), bzgl. Greenwich: positiv nach Osten, negativ nach Westen
	 * @param degdoub: geografische Breite des Beobachtungsorts in Grad (Format Gleitkomma), positiv auf Nordhalbkugel
	 * @param *arg_zeit: Zeiger auf Zeitobjekt (Datum und Uhrzeit)
	 * @param zz: Zeitzone des Beobachtungsortes (positiv nach Osten)
	 */
	public observationPoint( const char * ortsname, const double &rdoub, const double &degdoub,
			const czeit *arg_zeit, const int &zz );
	//-------------------------------------------------------------------------------------------------------------------------------


	// =============== get-Methods =========================
	
		/**
	 * \brief Liefert die Zeitzone des Beobachtungsorts zurueck
	 * @return Zeitzone des Beobachtungsorts
	 */
	public int get_zz( void ) const
	{
		get { return(_timeZone) };
	}


	/**
	 * inline getter(), der die mittlere Sternzeit des beobachtungsort-Objekts zurueckgibt
	 * */
	public double MeanSidTime( void ) const
	{
		get { return _localMeanSiderealTime };
	}


	//-------------------------------------------------------------------------------------------------------------------------------


	/**
	 * inline getter(), der die wahre Sternzeit des beobachtungsort-Objekts zurueckgibt
	 * */
	public double AppSidTime( void ) const
	{
		get { return _localAppSiderealTime };
	}
	
	/**
	 * getter(), der die geografische Laenge des Beobachtungsortes zurueckgibt
	 * */
	public double GeogrLong( void ) const
	{
		get { return _geogrLong };
	}

	/**
	 *  getter(), der die geografische Breite des Beobachtungsortes zurueckgibt
	 * */
	public double GeogrLat( void ) const
	{
		get { return _geogrLat };
	}


	public double LocalJD(void) const
	{
		get { return(_localJulDat) };
	}




	// =================    Konstruktoren   ================================================================


	// Constructor 1
	public observationPoint()
	{
		_nameOfObservationPoint = "Kehlen";
		_geogrLong = minusPlus180( CONST__DEFAULT_LAENGE);
		_geogrLat = ohne_ueberlauf_declination( CONST__DEFAULT_BREITE );
		_timeZone = 1;

		// Standard constructor for czeit selecting the current time by means of now(.)
		czeit * loc_initTime = new czeit();
		_localMeanSiderealTime = calcLocalMeanSiderealTime( loc_initTime );
		_localAppSiderealTime = calcLocalAppSiderealTime( loc_initTime );
		_localJulDat = calcLocalJulDat( loc_initTime );
	}
	// -------------------------------------------------------------------------------------------------------


	// --------------------------------------------------------------------------------------------------------------------------
	// Constructor 2
	public observationPoint(const czeit arg_zeit, const int zz)
	{
		_nameOfObservationPoint = "Kehlen";
		_geogrLong =  minusPlus180( CONST__DEFAULT_LAENGE );
		_geogrLat = ohne_ueberlauf_declination( CONST__DEFAULT_BREITE );
		_timeZone = zz;
		_localMeanSiderealTime = calcLocalMeanSiderealTime( arg_zeit );
		_localAppSiderealTime = calcLocalAppSiderealTime( arg_zeit );
		_localJulDat = calcLocalJulDat( arg_zeit );
	}
	// -------------------------------------------------------------------------------------------------------


	// --------------------------------------------------------------------------------------------------------------------------
	// Constructor 3
	public observationPoint(	const string ortsname, const double rdoub, const double degdoub,
			const czeit arg_zeit, const int zz )
	{
		_nameOfObservationPoint = "ortsname";
		_geogrLong = minusPlus180( rdoub );
		_geogrLat = ohne_ueberlauf_declination( degdoub );
		_timeZone = zz;
		_localMeanSiderealTime = calcLocalMeanSiderealTime( arg_zeit );
		_localAppSiderealTime = calcLocalAppSiderealTime( arg_zeit );
		_localJulDat = calcLocalJulDat( arg_zeit );
	}
	// -------------------------------------------------------------------------------------------------------




	// ================= private methods ================================

	// --------------------------------------------------------------------------------------------------------------------------
	protected static double calcLocalMeanSiderealTime( const czeit arg_zeit )
	{
		double ret_SidTime = 0.0;

		// calc. sidereal time at Greenwhich for the given time instant at the local observation point

		SiderialTime MeanSiderialTime = new SiderialTime( arg_zeit );

		double loc_MeanSidGreenTime = MeanSiderialTime->get_meanSidTime();

		// add geogr. length (in hours) to the mean siderial time in Greenwich
		ret_SidTime = loc_MeanSidGreenTime - _geogrLong/astroConst.C__deg_per_hour;

		ret_SidTime = ohne_ueberlauf_hour( ret_SidTime );
		return( ret_SidTime );

	}
	// --------------------------------------------------------------------------------------------------------------------------


	// --------------------------------------------------------------------------------------------------------------------------
	protected static double observationPoint::calcLocalAppSiderealTime( const czeit *arg_zeit )
	{
		double ret_SidTime = 0.0;

		// calc. sidereal time at Greenwhich for the given time instant at the local observation point

		SiderialTime  AppSiderialTime = new SiderialTime( arg_zeit );

		double loc_AppSidGreenTime = AppSiderialTime->get_appSidTime();

		// add geogr. length (in hours) to the mean siderial time in Greenwich
		ret_SidTime = loc_AppSidGreenTime - _geogrLong/astroConst.C__deg_per_hour;

		ret_SidTime = ohne_ueberlauf_hour( ret_SidTime );

		return( ret_SidTime );

	}
	// --------------------------------------------------------------------------------------------------------------------------


	// --------------------------------------------------------------------------------------------------------------------------
	protected static double observationPoint::calcLocalJulDat( const czeit *arg_zeit )
	{

		double ret_locJulDat = 0.0;
		double loc_JD0 = arg_zeit->get_JD0();
		double loc_UT = arg_zeit->get_zeit();

		// JD0: Beginning of day
		// + Universal time + geographical length in degrees divided by 15 --> this is the local time
		// divide it by 24 --> this is the portion of the day
		ret_locJulDat = loc_JD0 + ( loc_UT + _geogrLong/astroConst.C__deg_per_hour ) / astroConst.C__hours_per_day;

		return( ret_locJulDat );
	}
	// --------------------------------------------------------------------------------------------------------------------------




}
