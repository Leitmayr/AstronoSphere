/**
 * \file observationPoint.h
 * \author Marcus Hiemer
 * \date 2015-06-16
 * \brief class definition for the class observationPoint
 *
 * observationPoint describes a geographic observation point. It requires a time instant which is used to determine
 * sidereal time and local time.
 * The class inherits from conversions in order to use the overflow handling methods.
 *
 * Log:
 * 2012-04-03: initial implemenations
 * 2015-03-30: refactored
 * 2015-05-13: refactored getMeanGreenwichTime0(): overflow handling and conversion deg -> hour
 * 2015-05-14:
 * - inherits from class conversions now
 * - renamed MeanGreenwichTime by calcMeanSiderealGreenwichTime(.)
 * - introduced double calcAppSiderealGreenwichTime( double &arg_JD0, const double &arg_UniversalTimedouble );
 * - moved former inline method getMeanGreenwichTime0(.) to cpp and renamed it there to calcMeanSiderealGreenwichTime0(.)
 * - calcSiderealTime renamed by calcLocalSiderealTime(.)
 * - some additional comments added, code better structured, some refactoring done, improved doxygen comments
 * 2015-05-18:
 * - class siderealTime created
 * - method calcMeanSiderealGreenwichTime0(.) moved to siderialTime
 * - method calcMeanSiderealGreenwichTime(.) moved to siderialTime
 * - method calcAppSiderealGreenwichTime(.) moved to siderialTime
 * 2015-05-19:
 * - added attribute _localAppSiderealTime
 * - renamed attribute _siderialTime to _localMeanSiderealTime
 * - renamed method calcLocalSiderealTime(.) to calcLocalMeanSiderealTime(.)
 * - added calcLocalAppSiderealTime(.)
 * - changed arguments of the calculation methods to one czeit Argument
 * - improved doxygen comments
 * 2015-06-16:
 * - changed attribute comment definition for geopraphical longitude: west+, east-
 *
 */


#ifndef OBSERVATIONPOINT_H_
#define OBSERVATIONPOINT_H_

#include "../ccoord.h"
#include "../czeit.h"
#include "../conversions/conversions.h"

#include "SiderialTime.h"
#include <math.h>
#include "../CONST/astro_const.h"
#include "../CONST/beo_const.h"


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
	char _name[25]; 					/**< name of the observation point */
	double _localMeanSiderealTime;		/**< local mean siderial time (mittlere Sternzeit) at the observation point */
	double _localAppSiderealTime;		/**< local apparent siderial time (wahre Sternzeit) at the observation point */
	double _localJulDat;				/**< local Julian Day of the observation point */
	double _geogrLong;					/**< geogr. long. of observation point in deg and float with respect to Greenwich: + towards west, - towards east */
	double _geogrLat;					/**< geogr. lat. of observation point in deg and float with respect to equator: + towards north, - towards south */
	int _timeZone;						/**< time zone of the observation point: - towards west, + towards east*/
	// =========================================================================


	/**
	 * \brief calculates the local mean sidereal time from the Julian Day at 0UT and the UT of a time instant
	 * @param 	arg_zeit: zeit object
	 * @return 	double. return is the local mean sidereal time at arg_zeit.
	 */
	double calcLocalMeanSiderealTime( const czeit *arg_zeit );
	//-------------------------------------------------------------------------------------------------------------------------------


	/**
	 * \brief calculates the local mean sidereal time from the Julian Day at 0UT and the UT of a time instant
	 * @param 	arg_zeit: zeit object
	 * @return 	double. return is the local apparent sidereal time at arg_zeit.
	 */
	double calcLocalAppSiderealTime( const czeit *arg_zeit );
	//-------------------------------------------------------------------------------------------------------------------------------


	/**
	 * private Memberfunktion, die als Argumente das JD des Tagesbeginns und die Zeit im Fliesskommaformat uebergeben bekommt
	 * @param 	arg_zeit: zeit object
	 * @return 	double. Rueckgabewert ist die lokale Zeit am Beobachtungszeitpunkt zum Zeitpunkt zeit_comma (ohne Beruecksichtigung der Sommerzeit)
	 */
	double calcLocalJulDat( const czeit *arg_zeit );
	//-------------------------------------------------------------------------------------------------------------------------------



public:
	//Konstruktoren


	/**KONSTRUKTOR1
	 * Standardkonstruktor, waehlt den Standardbeobachtungsort und die aktuelle zeit ueber den Standardkonstruktor von czeit
	 * */
	observationPoint();
	//-------------------------------------------------------------------------------------------------------------------------------



	/**KONSTRUKTOR2
	 * Standardkonstruktor,  waehlt den Standardbeobachtungsort und die als JD uebergebene Zeit
	 * @param *arg_zeit: Zeiger auf Zeitobjekt (Datum und Uhrzeit)
	 * @param zz: Zeitzone des Beobachtungsortes (positiv nach Osten)
	 */
	observationPoint( const czeit *arg_zeit, const int &zz );
	//-------------------------------------------------------------------------------------------------------------------------------

	/**KONSTRUKTOR3
	 * Waehlt uebergebenen Ort mit Koordinaten und als Zeit-Objekt übergebenen Zeitpunkt
	 * @param ortsname: Name des Beobachtungsorts
	 * @param rdoub: geografische Laenge des Beobachtungsorts in Grad (Format Gleitkomma), bzgl. Greenwich: positiv nach Osten, negativ nach Westen
	 * @param degdoub: geografische Breite des Beobachtungsorts in Grad (Format Gleitkomma), positiv auf Nordhalbkugel
	 * @param *arg_zeit: Zeiger auf Zeitobjekt (Datum und Uhrzeit)
	 * @param zz: Zeitzone des Beobachtungsortes (positiv nach Osten)
	 */
	observationPoint( const char * ortsname, const double &rdoub, const double &degdoub,
			const czeit *arg_zeit, const int &zz );
	//-------------------------------------------------------------------------------------------------------------------------------


	//Destruktor
	~observationPoint(void);



	/**
	 * inline getter(), der die mittlere Sternzeit des beobachtungsort-Objekts zurueckgibt
	 * */
	double getMeanSiderealTime( void ) const;
	//-------------------------------------------------------------------------------------------------------------------------------


	/**
	 * inline getter(), der die wahre Sternzeit des beobachtungsort-Objekts zurueckgibt
	 * */
	double getAppSiderealTime( void ) const;
	//-------------------------------------------------------------------------------------------------------------------------------



	/**
	 * inline getter(), der die lokale Zeit des beobachtungsort-Objekts zurueckgibt (ohne Beruecksichtigung der Sommerzeit)
	 * */
	double getLocalJD( void ) const;
	//-------------------------------------------------------------------------------------------------------------------------------



	/**
	 * inline getter(), der die geografische Laenge des Beobachtungsortes zurueckgibt
	 * */
	double getGeogrLong( void ) const;
	//-------------------------------------------------------------------------------------------------------------------------------



	/**
	 * inline getter(), der die geografische Breite des Beobachtungsortes zurueckgibt
	 * */
	double getGeogrLat( void ) const;



	/**
	 * \brief Liefert die Zeitzone des Beobachtungsorts zurueck
	 * @return Zeitzone des Beobachtungsorts
	 */
	int get_zz( void ) const;



	/**
	 * ueberladener << Operator, der de geografischen Koordinaten sowie die Sternzeit zu einem bestimmten Zeitpunkt ausgibt
	 *
	 */
	// Operatoren
	friend ostream& operator<<(ostream & os, const observationPoint & pos);



};


// =============== inline-Definitionen =========================
inline int observationPoint::get_zz( void ) const
{
	return(_timeZone);
}

inline double observationPoint::getMeanSiderealTime( void ) const
{
	return _localMeanSiderealTime;
}

inline double observationPoint::getAppSiderealTime( void ) const
{
	return _localAppSiderealTime;
}

inline double observationPoint::getGeogrLong( void ) const
{
	return _geogrLong;
}

inline double observationPoint::getGeogrLat( void ) const
{
	return _geogrLat;
}


inline double observationPoint::getLocalJD(void) const
{
	return(_localJulDat);
}

#endif /* OBSERVATIONPOINT_H_ */
