/*
 * \file   ObsSituation.cpp
 * \author Marcus Hiemer
 * \date:  2015-06-16
 * \brief implements the class ObsSituation
 *
 * Description:
 * implements the class ObsSituation. Equations taken from Chapter 13 in Meeus, Astronomical Algorithms, 2nd ed.
 *
 *
 * Change Log:
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

#include "ObsSituation.h"
#include <stdlib.h>

ObsSituation::ObsSituation(	const czeit * arg_timeInstant,
							const double & arg_GA_ra,
							const double & arg_GA_dek,
							const double & arg_geogrLong,
							const double & arg_geogrLat)

{
	updateObject( arg_timeInstant, arg_GA_ra, arg_GA_dek, arg_geogrLong, arg_geogrLat );
}


void ObsSituation::setObsSituation( const czeit * arg_timeInstant,
									const double & arg_GA_ra,
									const double & arg_GA_dek,
									const double & arg_geogrLong,
									const double & arg_geogrLat )
{
	updateObject( arg_timeInstant, arg_GA_ra, arg_GA_dek, arg_geogrLong, arg_geogrLat );

}

ObsSituation::~ObsSituation() {
	// TODO Auto-generated destructor stub
}
// ==================================================================================================================


void ObsSituation::updateObject( 	const czeit *arg_timeInstant,
									const double & arg_GA_ra,
									const double & arg_GA_dek,
									const double & arg_geogrLong,
									const double & arg_geogrLat )
{
	_hourAngle = calcHourAngle( arg_timeInstant, arg_GA_ra, arg_geogrLong );
	_altitude  = calcHorizontalAltitude( _hourAngle, arg_GA_dek, arg_geogrLat );
	_azimuth   = calcHorizontalAzimuth( _hourAngle, arg_GA_dek, arg_geogrLat );
}


double ObsSituation::calcHourAngle( const czeit * arg_timeInstant, const double &arg_GALong, const double &arg_geogrLong )
{
	double ret_hourAngle = 0.0;

	SiderialTime * sidTime = new SiderialTime( arg_timeInstant );
	double loc_appSidTimeAtGreenwich = sidTime->get_appSidTime(); // return in hours and float

	// formula taken from Meeus, Astronomical Algorithms, 2nd ed., p. 92
	// H = SiderialTimeAtGreenwhich - right ascension - geographical Longitude
	ret_hourAngle = loc_appSidTimeAtGreenwich - arg_GALong - arg_geogrLong/C__deg_per_hour;

	// limit to [-0.0...+24.0]
	ret_hourAngle = ohne_ueberlauf_hour( ret_hourAngle );

	return (ret_hourAngle);
}
// --------------------------------------------------------------------------------------------------------------------




double ObsSituation::calcHorizontalAltitude( const double &arg_hourAngle, const double &arg_GALat, const double &arg_geogrLat )
{
	double ret_altitude = 0.0;

	// convert the float hours to float deg for later use in sin(.)-function
	double loc_hourAngleDeg = C__deg_per_hour * arg_hourAngle;

	// Meeus, Astronomical Algorithms, 2nd ed., p. 93, eqn. (13.6)
	ret_altitude = 	180/pi*asin(
				sin( pi/180*arg_geogrLat ) * sin( pi/180*arg_GALat )
				+   cos( pi/180*arg_geogrLat ) * cos( pi/180*arg_GALat ) * cos( pi/180*loc_hourAngleDeg )
				);

	// limit to [-90...+90]
	ret_altitude = ohne_ueberlauf_declination( ret_altitude );

	return ( ret_altitude );
}
// --------------------------------------------------------------------------------------------------------------------




double ObsSituation::calcHorizontalAzimuth( const double &arg_hourAngle, const double &arg_GALat, const double &arg_geogrLat )
{
	double ret_azimuth = 0.0;

	// convert the float hours to float deg for later use in sin(.)-function
	double loc_hourAngleDeg = C__deg_per_hour * arg_hourAngle;

	// Meeus, Astronomical Algorithms, 2nd ed., p. 93, eqn. (13.5)
	ret_azimuth = 180/pi*atan2( sin( pi/180*loc_hourAngleDeg ),
							    cos( pi/180*loc_hourAngleDeg )*sin( pi/180*arg_geogrLat )-tan( pi/180*arg_GALat )*cos( pi/180*arg_geogrLat )
							   );

	ret_azimuth = ohne_ueberlauf_degrees( ret_azimuth );

	return ( ret_azimuth );

}
// --------------------------------------------------------------------------------------------------------------------


