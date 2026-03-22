/**
 * \file   ObsSituation.h
 * \author Marcus Hiemer
 * \date:  16.06.2015
 * \brief  class definition of the class ObsSituation describing an observing situation
 *
 * Description:
 * calculates a celestial observing situation like the position of a planet as seen from a particular geographical
 * point at a given instant of time
 *
 * Change Log:
 * + 2015-06-03: initial revision with
 * 	- attributes _hourAngle, _azimuth, _altitude
 *	- member functions calcHourAngle(.), calcHorizontalAltitude(.) and calcHorizontalAzimuth(.);
 * + 2015-06-05:
 * - changed constructor: GA_coord no argument any more. Replaced by ra and dek in float instead
 * - changed interface to calcHourAngle: arg_GALong now in hours and float instead of degrees and float
 * 2015-06-16:
 * - added member functions updateObject(.) and setObsSituation(.) to be able to update existing object
 *
 */

#ifndef OBSSITUATION_H_
#define OBSSITUATION_H_


#include "..\conversions\conversions.h"
#include "..\czeit.h"
#include "..\CONST\astro_const.h"
#include "..\Nutation\obliquity.h"
#include "SiderialTime.h"
#include <math.h> 		// for sin(.), cos(.), ... in cpp


class ObsSituation : public conversions {
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

	private:
		double _hourAngle;	/**< hourAngle: angle in hours float [0..24] - positive west of the south meridian, negative east of south meridian*/
		double _azimuth; 	/**< azimuth: angle in degrees [0..360] measured westward from the south: south: 0°, west: 90°, north: 180°, east: 270° */
		double _altitude; 	/**< altitude: angle in degrees[-90..90] - positive above the horizon, negative below, 0° on the horizontal line */


		/**
			 * \brief calculates the local hour angle at the geographical position where the celestial observation takes place
			 * @param  arg_timeInstant time object at which the celestial observation takes place
			 * @param  arg_GALong right ascension (GA longitude) of the celestial object in hours and float
			 * @param  arg_geogrLong geographical longitude of the observation point in degrees: -: west of Greenwich, + east of Greenwich
			 * @return local hour angle of the celestial object in hours and float limited to [0...+24]
		*/
		double calcHourAngle( const czeit * arg_timeInstant, const double &arg_GALong, const double &arg_geogrLong );
		// ------------------------------------------------------------------------------------------------------------------


		/**
		 	 * \brief calculates the altitude of an object in horizontal coordinates: + above horizon, - below horizon
		 	 * @param arg_hourAngle local hour angle in hours and float
		 	 * @param arg_GALat declination (GA latitude) of celestial object in degrees
		 	 * @param  arg_geogrLat geographical latitude of the observation point in degrees: +: north of equator, -: south of equator
			 * @return altitude of celestial object over horizon (neg. below horizon) in deg limited to [-90°...90°]
		 */
		double calcHorizontalAltitude( const double &arg_hourAngle, const double &arg_GALat, const double &arg_geogrLat );
		// ------------------------------------------------------------------------------------------------------------------


		/**
			 * \brief calculates the azimuth of an object in horizontal coordinates: + westward of south, - eastward of south
			 * @param arg_hourAngle local hour angle in hours and float
			 * @param arg_GALat declination (GA latitude) of celestial object in degrees
			 * @param  arg_geogrLat geographical latitude of the observation point in degrees: +: north of equator, -: south of equator
			 * @return azimuth of a celestial object with respect to the south meridian in deg (neg. to east) limited to [0°...360°]
		*/
		double calcHorizontalAzimuth( const double &arg_hourAngle, const double &arg_GALat, const double &arg_geogrLat );
		// ------------------------------------------------------------------------------------------------------------------

		/**
		 * \brief updates all attributes based on new time argument
		 * param arg_zeit new argument to calculate siderial time with
		 * @param arg_GA_ra right ascension of celestial object in hours and float
		 * @param arg_GA_dek declination of celestial object in degrees and float
		 * @param arg_geogrLong geographical longitude of observation point on earth in degrees and float
		 * @param arg_geogrLat geographical latitude of observation point on earth in degrees and float
		 */
		void updateObject( 	const czeit *arg_timeInstant,
							const double & arg_GA_ra,
							const double & arg_GA_dek,
							const double & arg_geogrLong,
							const double & arg_geogrLat );

	public:
		/**
		 * \brief Constructor receveing time and coordinates of celestial object and georgraphical observation point.
		 * Calculating hour angle, altitude in horizontal coordinates, azimuth in horizontal coordinates and assigning them to the class attributes
		 * @param arg_GA_ra right ascension of celestial object in hours and float
		 * @param arg_GA_dek declination of celestial object in degrees and float
		 * @param arg_geogrLong geographical longitude of observation point on earth in degrees and float
		 * @param arg_geogrLat geographical latitude of observation point on earth in degrees and float
		 */
		ObsSituation( 	const czeit * arg_timeInstant,
						const double & arg_GA_ra,
						const double & arg_GA_dek,
						const double & arg_geogrLong,
						const double & arg_geogrLat );

		virtual ~ObsSituation();


		/**
		 * \brief set Function to update ObsSituation Object
		 * @param arg_zeit new argument to calculate observation situation with
		 * @param arg_GA_ra right ascension of celestial object in hours and float
		 * @param arg_GA_dek declination of celestial object in degrees and float
		 * @param arg_geogrLong geographical longitude of observation point on earth in degrees and float
		 */
		void setObsSituation( const czeit * arg_timeInstant,
											const double & arg_GA_ra,
											const double & arg_GA_dek,
											const double & arg_geogrLong,
											const double & arg_geogrLat );


		/**
			 * \brief returns local hour angle
			 * @return _hourAngle: angle in hours float - positive west of the south meridian, negative east of south meridian
		*/
		inline double getHourAngle( void ) const;
		// ------------------------------------------------------------------------------------------------------------------


		/**
			 * \brief returns altitude of a celestial object on a geographical observation point for a given time instant
			 * @return _altitude: angle in degrees - positive above the horizon, negative below, 0° on the horizontal line
		*/
		inline double getAltitude( void ) const;
		// ------------------------------------------------------------------------------------------------------------------


		/**
			 * \brief returns azimuth of a celestial object on a geographical observation point for a given time instant
			 * @return _azimuth: azimuth: angle in degrees measured westward from the south: south: 0°, west: 90°, north: 180°, east: 270°
		*/
		inline double getAzimuth( void ) const;
		// ------------------------------------------------------------------------------------------------------------------

};

inline double ObsSituation::getHourAngle( void ) const
{
	return ( _hourAngle );
}

inline double ObsSituation::getAltitude( void ) const
{
	return ( _altitude );
}

inline double ObsSituation::getAzimuth( void ) const
{
	return ( _azimuth );
}
#endif /* OBSSITUATION_H_ */
