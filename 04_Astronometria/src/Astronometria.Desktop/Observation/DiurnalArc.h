/**
 * \file   DiurnalArc.h
 * \author Marcus Hiemer
 * \date: 2015-06-21
 * \brief Diurnal arc is the motion of an object in the sky
 *
 * The diurnal arc describes the motion of a celestial object in the sky as seen from a particular observation point . This class
 * calculates and stores the rise, transit and set of a celestial object. It neglects the varying influence of the refraction depending on
 * temperature and pressure. It calculates with fix value below the horizon considering a mean effect of refraction.
 *
 * Change Log:
 * 2015-06-21: initial revision
 * - calculates transit, rising and setting times
 * - calculates whether object is circumpolar or not
 * - declares cout operator to print results
 */

#ifndef DIURNALARC_H_
#define DIURNALARC_H_


#include "../czeit.h"
#include "../cinterp.h"
#include "SiderialTime.h"
#include "ObsSituation.h"
#include "ObservationPoint.h"
#include "math.h"	// for sin(.), ...

struct diurnalArc
{
	szeit  time;
	unsigned int state;
};


enum diurnalArcState {INVALID, VALID};
enum RiseOrSet {RISING, SETTING};

class DiurnalArc: public conversions {
	/**
	 * \class  <Klassenname>	
	 * \brief  This class
	 * calculates and stores the rise, transit and set of a celestial object.
	 *
	 * The diurnal arc describes the motion of a celestial object in the sky as seen from a particular observation point . This class
	 * calculates and stores the rise, transit and set of a celestial object. It neglects the varying influence of the refraction depending on
	 * temperature and pressure. It calculates with fix value below the horizon considering a mean effect of refraction.
	 */
private:
	diurnalArc  _rising;
	diurnalArc  _transit;
	diurnalArc  _setting;
	unsigned int _isCircumpolar;



	double approxHourAngle( const GA_coord & arg_GAOfCelestialBody, const observationPoint * arg_PointOfObservation );

	double calcTransit( const double arg_H0, GA_coord arg_GA[3], const double & arg_obsLongitude, czeit * arg_timeOfObservation );
	double calcRisingSetting(
			const double &arg_transit,
			const double arg_H0, GA_coord arg_GA[3],
			const observationPoint * arg_PointOfObservation,
			czeit * arg_timeOfObservation,
			const unsigned int &arg_RiseOrSet);

public:

	/**
	 * \brief Constructor
	 * @param arg_GAOfCelestialBody[3] GA-coordinates of Day-1, Day and Day+1
	 * @param arg_PointOfObservation observation point where diurnal arc is seen from
	 * @param arg_timeOfObservation Time for which the diurnal arc shall be calculated
	 */
	DiurnalArc( GA_coord  arg_GAOfCelestialBody[3], const observationPoint * arg_PointOfObservation, czeit * arg_timeOfObservation );

	virtual ~DiurnalArc();


	// Operatoren
	friend ostream& operator<<( ostream & os, const DiurnalArc & DArc );
	// -------------------------------------------------------------------------------------------------------

	inline diurnalArc  get_rising( void ) const;
	inline diurnalArc  get_transit( void ) const;
	inline diurnalArc  get_setting( void ) const;
	inline unsigned int getCircumpolarState( void ) const;
};



inline diurnalArc  DiurnalArc::get_rising(void) const
{
	return( _rising );
}

inline diurnalArc  DiurnalArc::get_transit(void) const
{
	return( _transit );
}

inline diurnalArc  DiurnalArc::get_setting(void) const
{
	return( _setting);
};

inline unsigned int DiurnalArc::getCircumpolarState( void ) const
{
	return ( _isCircumpolar );
};

#endif /* DIURNALARC_H_ */
