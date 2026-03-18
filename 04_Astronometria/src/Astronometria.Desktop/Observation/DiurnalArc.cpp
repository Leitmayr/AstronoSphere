/**
 * \file   DiurnalArc.cpp
 * \author Marcus Hiemer
 * \date:  11.06.2015
 * \brief 
 *
 * Description:
 *
 * Change Log:
 * 2015-06-21: initial revision
 * - calculates transit, rising and setting times
 * - calculates whether object is circumpolar or not
 * - implements cout operator to print results
 */

#include "DiurnalArc.h"

DiurnalArc::DiurnalArc( GA_coord arg_GAOfCelestialBody[3], const observationPoint * arg_PointOfObservation, czeit * arg_timeOfObservation )
{

	double loc_m0 = 0;
	double loc_m1 = 0;
	double loc_m2 = 0;

	double H0 = approxHourAngle( arg_GAOfCelestialBody[1], arg_PointOfObservation );

	loc_m0 = calcTransit( 		H0,
								arg_GAOfCelestialBody,
								arg_PointOfObservation->getGeogrLong(),
								arg_timeOfObservation );

	loc_m1 = calcRisingSetting( loc_m0,
								H0,
								arg_GAOfCelestialBody,
								arg_PointOfObservation,
								arg_timeOfObservation,
								RISING );

	loc_m2 = calcRisingSetting( loc_m0,
								H0,
								arg_GAOfCelestialBody,
								arg_PointOfObservation,
								arg_timeOfObservation,
								SETTING );


	if ( _isCircumpolar > 0.5 )
	{
		// do nothing
	}
	else
	{
		int hour_min_sec [3] = { 0, 0, 0 };
		conversions::double2hour( loc_m0, hour_min_sec );
		_transit.time.uhr_comma = loc_m0;
		_transit.time.stunde = hour_min_sec[0];
		_transit.time.minute = hour_min_sec[1];
		_transit.time.sekunde = hour_min_sec[2];
		_transit.state = 1;

		conversions::double2hour( loc_m1, hour_min_sec );
		_rising.time.uhr_comma = loc_m1;
		_rising.time.stunde = hour_min_sec[0];
		_rising.time.minute = hour_min_sec[1];
		_rising.time.sekunde = hour_min_sec[2];
		_rising.state = 1;

		conversions::double2hour( loc_m2, hour_min_sec );
		_setting.time.uhr_comma = loc_m2;
		_setting.time.stunde = hour_min_sec[0];
		_setting.time.minute = hour_min_sec[1];
		_setting.time.sekunde = hour_min_sec[2];
		_setting.state = 1;
	}

//	cout << *this;

}

DiurnalArc::~DiurnalArc() {

}

double DiurnalArc::approxHourAngle( const GA_coord & arg_GAOfCelestialBody, const observationPoint * arg_PointOfObservation )
{
	double ret_H0 = 0;

	double loc_geographicalLat = arg_PointOfObservation->getGeogrLat(); 	// determine geogr. latitude from argument
	double loc_dekOfObservationDay = arg_GAOfCelestialBody.gms;				// determine deklination from argument

	// ToDo: C__HO_STARS ersetzen durch den wahren Refraktionswert des Gestirns!
	double loc_circumpolar =
			(sin( pi/180*C__H0_STARS ) - sin( pi/180*loc_geographicalLat )*sin( pi/180*loc_dekOfObservationDay )) /
			(cos( pi/180*loc_geographicalLat )*cos( pi/180*loc_dekOfObservationDay ));

	// loc_SiderialTime anlegen
	// extract geographical longitude from argument

	if ( loc_circumpolar <= 1 )
	{
		ret_H0 = 180/pi*acos( loc_circumpolar );
		_isCircumpolar = 0;
	}
	else 	// if above term is larger than 1 the celestial body is circumpolar, hence rise and set do not make sense

	{
		ret_H0 = 0;
		_isCircumpolar = 1;

		// set INVALID state and default values
		_rising.state = INVALID;
		czeit * loc_rising  = new czeit(0.0);
		_rising.time = loc_rising->get_UT_hms();

		_setting.state = INVALID;
		czeit * loc_setting = new czeit(0.0);
		_setting.time = loc_setting->get_UT_hms();

		_transit.state = INVALID;
		czeit * loc_transit  = new czeit(0.0);
		_transit.time = loc_transit->get_UT_hms();

	}

	return( ret_H0 );
}

double DiurnalArc::calcTransit( const double arg_H0, GA_coord arg_GA[3], const double & arg_obsLongitude, czeit * arg_timeOfObservation )
{

	// exit quantities for do while loop
	double eps = 0.0001;
	unsigned int loop_cnt = 0;
	const unsigned int MAX_LOOP_CNT = 5;

	// local variables
	double m0 = 0;
	double m_alt = 0;
	double m_new = 0;
	double Delta_m = 0;
	double ret_m0 = 0;
	double n;
	double loc_alpha[3];

	double loc_time_base[3];
	double alpha_interp = 0.0;
	double loc_SidTime0 = 0.0;
	double loc_HourAngle = arg_H0;

	// why "+i-1": get JDs of previous day (-1 for i=0), current day (0 for i=1) and next day (1 for i=2)
	for (int i = 0; i < 3; i++)
	{
		loc_time_base[i] = arg_timeOfObservation->get_JD()+i-1;
		loc_alpha[i] = arg_GA[i].hms_grad;
	}


	// StepA: calc_m0
	SiderialTime * loc_SidTime = new SiderialTime( arg_timeOfObservation );
	loc_SidTime0 =loc_SidTime->get_meanSidTime0();

	m0 = ( loc_alpha[1] + arg_obsLongitude - C__deg_per_hour*loc_SidTime->get_meanSidTime0() )/360;

	//handle values of m0 out of [0...1]
	while ( m0 < 0)
		m0 = m0 + 1;
	while ( m0 > 1 )
		m0 = m0 - 1;


	m_alt = m0;	// save m0 in m_alt for loop

	do // run this refinement loop at least once but break it if delta sufficiently small or max_number_of_loop counts exceeded
	{
	// StepB: refine m0 -> Delta_m - see Meeus, Astronomical Algorithms, 2nd ed., p. 103
	// Step B.1:
	// calc n = m0 + DeltaT/86400
	n = m_alt + arg_timeOfObservation->get_Delta_T()/C__sec_per_day;

	// Step B.2: interpolate the new position at the time instant represented by m0

	// ---------------------------------------------------
	// Todo: Debug Abweichung in Interpolationsfunktion
	// ---------------------------------------------------
	alpha_interp = cinterp::interp_y3_n( loc_time_base, loc_alpha, 3, n ); //unit: degrees

	// Meeus, Astronomical Algorithms, 2nd ed., p. 103
	// Step B.3: determine siderial time in Greenwhich in degrees
	loc_SidTime0 = C__deg_per_hour*(loc_SidTime->get_meanSidTime0()) + C_SiderialDegrees*m_alt; //unit: degrees
	loc_SidTime0 = conversions::ohne_ueberlauf_degrees( loc_SidTime0 );

	// Step B.4: determine the new hour angle
	loc_HourAngle = loc_SidTime0 - arg_obsLongitude - alpha_interp; // unit: degrees

	// Step B.5: calculate deviation to the last iteration
	Delta_m = -loc_HourAngle/360;

	// Step B.6: determine the new point of time of transit
	m_new = m_alt + Delta_m;

	// save new value for next calculation step
	m_alt = m_new;

	// increment loop counter
	loop_cnt++; 		// limit max. number of loop runs
	} while( (( Delta_m > eps ) or ( Delta_m < -eps )) and ( loop_cnt < MAX_LOOP_CNT ) );

	ret_m0 = C__hours_per_day*m_new;

	return( ret_m0 );
}


double DiurnalArc::calcRisingSetting(
		const double &arg_transit,
		const double arg_H0, GA_coord arg_GA[3],
		const observationPoint * arg_PointOfObservation,
		czeit * arg_timeOfObservation,
		const unsigned int &arg_RiseOrSet )

{
	double eps = 0.0001;
	unsigned int loop_cnt = 0;
	const unsigned int MAX_LOOP_CNT = 5;

	// local variables
	double m = 0;
	double m_alt = 0;
	double m_new = 0;
	double Delta_m = 0;
	double ret_m = 0;
	double n;
	double loc_alpha[3];
	double loc_delta[3];
	double loc_time_base[3];
	double delta_interp = 0.0;
	double alpha_interp = 0.0;
	double loc_SidTime0 = 0.0;
	double loc_HourAngle = arg_H0;
	double loc_altitude = 0.0;

	// why "+i-1": get JDs of previous day (-1 for i=0), current day (0 for i=1) and next day (1 for i=2)
	for (int i = 0; i < 3; i++)
	{
		loc_time_base[i] = arg_timeOfObservation->get_JD()+i-1;
		loc_alpha[i] = arg_GA[i].hms_grad;
		loc_delta[i] = arg_GA[i].gms;
	}

	if ( arg_RiseOrSet == RISING )			// 0: rising
		m = arg_transit/24 - arg_H0/360;
	else if ( arg_RiseOrSet == SETTING ) 	// 1: setting
		m = arg_transit/24 + arg_H0/360;
	else									// sonst: unklar
	{
		ret_m = 0;
		return ( ret_m );					// exit function without any additional calculation
	}

	// StepA: calc_m0
	SiderialTime * loc_SidTime = new SiderialTime( arg_timeOfObservation );


	//handle values of m0 out of [0...1]
	while ( m < 0 )
		m = m + 1;
	while ( m > 1 )
		m = m - 1;


	m_alt = m;	// save m in m_alt for loop


	do // run this refinement loop at least once but break it if delta sufficiently small or max_number_of_loop counts exceeded
	{
	// StepB: refine m0 -> Delta_m - see Meeus, Astronomical Algorithms, 2nd ed., p. 103
	// Step B.1:
	// calc n = m0 + DeltaT/86400
	n = m_alt + arg_timeOfObservation->get_Delta_T()/C__sec_per_day;

	// Step B.2: interpolate the new position at the time instant represented by m
	// ---------------------------------------------------
	// Todo: Debug Abweichung in Interpolationsfunktion
	// ---------------------------------------------------
	alpha_interp = cinterp::interp_y3_n( loc_time_base, loc_alpha, 3, n ); //unit: degrees
	delta_interp = cinterp::interp_y3_n( loc_time_base, loc_delta, 3, n ); //unit: degrees

	// Meeus, Astronomical Algorithms, 2nd ed., p. 103
	// Step B.3: determine siderial time in Greenwhich in degrees
	loc_SidTime0 = C__deg_per_hour*(loc_SidTime->get_meanSidTime0()) + C_SiderialDegrees*m_alt; //unit: degrees
	loc_SidTime0 = conversions::ohne_ueberlauf_degrees( loc_SidTime0 );

	// Step B.4: determine the new hour angle
	loc_HourAngle = loc_SidTime0 - arg_PointOfObservation->getGeogrLong()  - alpha_interp; // unit: degrees

	// Step B.5: determine the altitude h according to Meeus, Astronomical Algorithms, 2nd ed., p. 93, Eqn. (13.6)
	loc_altitude = 	180/pi*asin
					(
					sin( pi/180*arg_PointOfObservation->getGeogrLat() )*sin( pi/180*delta_interp ) +
					cos( pi/180*arg_PointOfObservation->getGeogrLat() )*cos( pi/180*delta_interp )*cos( pi/180*loc_HourAngle )
					);
	// limit to [-90...+90]
	loc_altitude = ohne_ueberlauf_declination( loc_altitude );


	// Step B.6: calculate deviation to the last iteration
	// ToDo: C__H0_STARS!!!
	Delta_m = 	( loc_altitude - C__H0_STARS )/
				( 360*cos( pi/180*delta_interp )*cos(pi/180*arg_PointOfObservation->getGeogrLat())*sin( pi/180*loc_HourAngle ));

	// Step B.7: determine the new point of time of transit
	m_new = m_alt + Delta_m;

	// save new value for next calculation step
	m_alt = m_new;

	// increment loop counter
	loop_cnt++; 		// limit max. number of loop runs
	} while( (( Delta_m > eps ) or ( Delta_m < -eps )) and ( loop_cnt < MAX_LOOP_CNT ) );


	ret_m = C__hours_per_day*m_new;

	return( ret_m );
}


ostream& operator<<( ostream & os, const DiurnalArc & DArc )
{
	cout << endl << endl << endl;
	cout << "------------------------------------------------------------------------------";
	cout << endl;
	cout << "Rising: " << DArc._rising.time.stunde << ":" << DArc._rising.time.minute << ":" << DArc._rising.time.sekunde;
	cout << " | Transit: " << DArc._transit.time.stunde << ":" << DArc._transit.time.minute << ":" << DArc._transit.time.sekunde;
	cout << " | Setting: " << DArc._setting.time.stunde << ":" << DArc._setting.time.minute << ":" << DArc._setting.time.sekunde;
	cout << endl << "------------------------------------------------------------------------------" << endl << endl << endl;

	return os;
}
