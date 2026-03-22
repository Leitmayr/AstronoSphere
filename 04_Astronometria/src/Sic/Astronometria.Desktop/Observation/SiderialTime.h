/**
 * \file   SiderialTime.h
 * \author Marcus Hiemer
 * \date:  2015-06-16
 * \brief 
 *
 * Description:
 *
 * Change Log:
 * - 2015-05-19:
 * 		- calcuation methods now private
 * 		- arguments of all calculation methods is now a czeit Argument
 * 		- improved doxygen comments
 * - 2015-06-16:
 * 		- added member functions updateObject(.) and setSidTime(.) to be able to update existing object
 *
 */

#ifndef SIDERIALTIME_H_
#define SIDERIALTIME_H_

#include "../czeit.h"
#include <math.h>
#include "../CONST/astro_const.h"

#include "../Nutation/nutation.h"
#include "../Nutation/obliquity.h"

class SiderialTime: public conversions {
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
private:
	double _meanSidTime;	/**< mean siderial time in hours and float: */
	double _meanSidTime0;	/**< mean siderial time in hours at 0 UT and float: */
	double _appSidTime;		/**< apparent siderial time in hours and float: */


	/**
	 * \brief Liefert die mittlere Greenwich Zeit des Tagesanfangs
	 * @param 	arg_zeit: const reference on a czeit object.
	 * @return Zeitzone des Beobachtungsorts
	 */
	double calcMeanSiderealGreenwichTime0( const czeit *arg_zeit );
	//-------------------------------------------------------------------------------------------------------------------------------


	/**
	 * public Memberfunktion, die als Argumente das JD des Tagesbeginns und die Zeit in UT uebergeben bekommt
	 * @param 	arg_zeit: const reference on a czeit object.
	 * @see 	calc_sternzeit(.)
	 * @return 	double. Rueckgabewert ist die mittlere Greenwich Zeit zum gewuenschten Zeitpunkt. Wir benoetigt zur Berechnung der Sternzeit.
	 */
	double calcMeanSiderealGreenwichTime( const czeit *arg_zeit );
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
	double calcAppSiderealGreenwichTime( const czeit *arg_zeit );
	//-------------------------------------------------------------------------------------------------------------------------------


	/**
	 * \brief updates all attributes based on new time argument
	 * param arg_zeit new argument to calculate siderial time with
	 */
	void updateObject( const czeit *arg_zeit );



public:
	/**
	 * \brief constructor creating for the given time instant and for the Greenwich meridian the
	 * 		- mean siderial time
	 * 		- mean siderial time at 0UT
	 * 		- apparent siderial time
	 */
	SiderialTime( const czeit *arg_zeit );

	virtual ~SiderialTime();


	/**
	 * \brief set Function to update SiderialTime Object
	 * @param arg_zeit new argument to calculate siderial time with
	 */
	void setSidTime( const czeit *arg_zeit );

	/**
	 * \brief returns the mean siderial time at Greenwich
	 */
	inline double get_meanSidTime( void ) const;

	/**
	 * \brief returns the mean siderial time at Greenwich at 0 UT
	 */
	inline double get_meanSidTime0( void ) const;

	/**
	 * \brief returns the apparent siderial time at Greenwich
	 */
	inline double get_appSidTime( void ) const;
};


inline double SiderialTime::get_meanSidTime( void ) const
{
	return( _meanSidTime );
}

inline double SiderialTime::get_meanSidTime0( void ) const
{
	return( _meanSidTime0 );
}

inline double SiderialTime::get_appSidTime( void ) const
{
	return( _appSidTime );
}


#endif /* SIDERIALTIME_H_ */
