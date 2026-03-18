using System;
using Astronometria;
/**
 * \file   DiurnalArc.cs
 * \author Marcus Hiemer
 * \date:  12.01.2026
 * \brief 
 *
 * Description:
 *
 * Change Log:
 * 2026-01-12: transferred from C++ to C#
 * 2015-06-21: initial revision
 * - calculates transit, rising and setting times
 * - calculates whether object is circumpolar or not
 * - implements cout operator to print results
 */



namespace Astronometria
{
	public struct diurnalArc
	{
		public szeit time;
		public int state;
	};
	public enum diurnalArcState { INVALID, VALID };
	public enum RiseOrSet { RISING, SETTING };
}

public class DiurnalArc: conversions
{ 
	/**
	 * \class  DiurnalArc	
	 * \brief  This class calculates and stores the rise, transit and set of a celestial object.
	 *
	 * The diurnal arc describes the motion of a celestial object in the sky as seen from a particular observation point . This class
	 * calculates and stores the rise, transit and set of a celestial object. It neglects the varying influence of the refraction depending on
	 * temperature and pressure. It calculates with fix value below the horizon considering a mean effect of refraction.
	 */

	public diurnalArc  _rising;
	public diurnalArc  _transit;
	public diurnalArc  _setting;
	public int _isCircumpolar;



	//protected double approxHourAngle(  GA_coord  arg_GAOfCelestialBody,  observationPoint  arg_PointOfObservation );

	//protected double calcTransit(  double arg_H0, GA_coord arg_GA[3],  double  arg_obsLongitude, czeit  arg_timeOfObservation );
	//protected double calcRisingSetting(
	//		double arg_transit,
	//		double arg_H0, GA_coord arg_GA[],
	//		observationPoint arg_PointOfObservation,
	//		czeit  arg_timeOfObservation,
	//		int arg_RiseOrSet);

	/**
	 * \brief Constructor
	 * @param arg_GAOfCelestialBody[3] GA-coordinates of Day-1, Day and Day+1
	 * @param arg_PointOfObservation observation point where diurnal arc is seen from
	// * @param arg_timeOfObservation Time for which the diurnal arc shall be calculated
	// */
	//public DiurnalArc( GA_coord  arg_GAOfCelestialBody[], const observationPoint arg_PointOfObservation, czeit arg_timeOfObservation );



	// -------------------------------------------------------------------------------------------------------



	public diurnalArc  get_rising 
	{
		get { return (_rising); }
	}

	public diurnalArc  get_transit
	{
		get { return (_transit); }
	}

	public diurnalArc get_setting 
	{
		get { return (_setting); }
	}

	public int getCircumpolarState
	{
		get { return (_isCircumpolar); }
	}

	public DiurnalArc( GA_coord [] arg_GAOfCelestialBody,  observationPoint arg_PointOfObservation, czeit arg_timeOfObservation )
	{

		double loc_m0 = 0;
		double loc_m1 = 0;
		double loc_m2 = 0;

		double H0 = approxHourAngle( arg_GAOfCelestialBody[1], arg_PointOfObservation );

		int arg_rising = (int)RiseOrSet.RISING;
		int arg_setting = (int)RiseOrSet.SETTING;

		loc_m0 = calcTransit( 		H0,
									arg_GAOfCelestialBody,
									arg_PointOfObservation.getGeogrLong,
									arg_timeOfObservation );

		loc_m1 = calcRisingSetting( loc_m0,
									H0,
									arg_GAOfCelestialBody,
									arg_PointOfObservation,
									arg_timeOfObservation,
									arg_rising);

		loc_m2 = calcRisingSetting( loc_m0,
									H0,
									arg_GAOfCelestialBody,
									arg_PointOfObservation,
									arg_timeOfObservation,
									arg_setting);


		if ( _isCircumpolar > 0.5 )
		{
			// do nothing
		}
		else
		{
			int [] hour_min_sec = new int[3] ;
			conversions.double2hour( loc_m0, hour_min_sec );
			_transit.time.timeComma = loc_m0;
			_transit.time.hour = hour_min_sec[0];
			_transit.time.minute = hour_min_sec[1];
			_transit.time.second = hour_min_sec[2];
			_transit.state = 1;

			conversions.double2hour( loc_m1, hour_min_sec );
			_rising.time.timeComma = loc_m1;
			_rising.time.hour = hour_min_sec[0];
			_rising.time.minute = hour_min_sec[1];
			_rising.time.second = hour_min_sec[2];
			_rising.state = 1;

			conversions.double2hour( loc_m2, hour_min_sec );
			_setting.time.timeComma = loc_m2;
			_setting.time.hour = hour_min_sec[0];
			_setting.time.minute = hour_min_sec[1];
			_setting.time.second = hour_min_sec[2];
			_setting.state = 1;
		}

	//	cout << *this;

	}



	protected double approxHourAngle( GA_coord arg_GAOfCelestialBody,  observationPoint arg_PointOfObservation )
	{
		double ret_H0 = 0;

		double loc_geographicalLat = arg_PointOfObservation.getGeogrLat; 	// determine geogr. latitude from argument
		double loc_dekOfObservationDay = arg_GAOfCelestialBody.gms;				// determine deklination from argument

		// ToDo: C__HO_STARS ersetzen durch den wahren Refraktionswert des Gestirns!
		double loc_circumpolar =
				(Math.Sin( Math.PI/180*astroConst.C__H0_STARS ) - Math.Sin( Math.PI/180*loc_geographicalLat )*Math.Sin( Math.PI/180*loc_dekOfObservationDay )) /
				(Math.Cos( Math.PI/180*loc_geographicalLat )*Math.Cos( Math.PI/180*loc_dekOfObservationDay ));

		// loc_SiderialTime anlegen
		// extract geographical longitude from argument

		if ( loc_circumpolar <= 1 )
		{
			ret_H0 = 180/Math.PI*Math.Acos( loc_circumpolar );
			_isCircumpolar = 0;
		}
		else 	// if above term is larger than 1 the celestial body is circumpolar, hence rise and set do not make sense

		{
			ret_H0 = 0;
			_isCircumpolar = 1;

			// set INVALID state and default values
			_rising.state = (int)diurnalArcState.INVALID;
			czeit loc_rising  = new czeit(0.0);
			_rising.time = loc_rising.UT;

			_setting.state = (int)diurnalArcState.INVALID;
			czeit loc_setting = new czeit(0.0);
			_setting.time = loc_setting.UT;

			_transit.state = (int)diurnalArcState.INVALID;
			czeit loc_transit  = new czeit(0.0);
			_transit.time = loc_transit.UT;

		}

		return( ret_H0 );
	}

	static double calcTransit( double arg_H0, GA_coord [] arg_GA,  double arg_obsLongitude, czeit arg_timeOfObservation )
	{

		// exit quantities for do while loop
		double eps = 0.0001;
		int loop_cnt = 0;
		const int MAX_LOOP_CNT = 5;

		// local variables
		double m0 = 0;
		double m_alt = 0;
		double m_new = 0;
		double Delta_m = 0;
		double ret_m0 = 0;
		double n;
		double [] loc_alpha = new double[3];

		double [] loc_time_base = new double [3];
		double alpha_interp = 0.0;
		double loc_SidTime0 = 0.0;
		double loc_HourAngle = arg_H0;

		// why "+i-1": get JDs of previous day (-1 for i=0), current day (0 for i=1) and next day (1 for i=2)
		for (int i = 0; i < 3; i++)
		{
			loc_time_base[i] = arg_timeOfObservation.JD+i-1;
			loc_alpha[i] = arg_GA[i].hms_grad;
		}


		// StepA: calc_m0
		SiderialTime loc_SidTime = new SiderialTime( arg_timeOfObservation );
		loc_SidTime0 = loc_SidTime.get_meanSidTime0;

		m0 = ( loc_alpha[1] + arg_obsLongitude - astroConst.C__deg_per_hour*loc_SidTime.get_meanSidTime0 )/360;

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
		n = m_alt + arg_timeOfObservation.DeltaT/astroConst.C__sec_per_day;

		// Step B.2: interpolate the new position at the time instant represented by m0

		// ---------------------------------------------------
		// Todo: Debug Abweichung in Interpolationsfunktion
		// ---------------------------------------------------
		alpha_interp = cinterp.interp_y3_n( loc_time_base, loc_alpha, 3, n ); //unit: degrees

		// Meeus, Astronomical Algorithms, 2nd ed., p. 103
		// Step B.3: determine siderial time in Greenwhich in degrees
		loc_SidTime0 = astroConst.C__deg_per_hour*loc_SidTime.get_meanSidTime0 + astroConst.C_SiderialDegrees*m_alt; //unit: degrees
		loc_SidTime0 = conversions.ohne_ueberlauf_degrees( loc_SidTime0 );

		// Step B.4: determine the new hour angle
		loc_HourAngle = loc_SidTime0 - arg_obsLongitude - alpha_interp; // unit: degrees

		// Step B.5: calculate deviation to the last iteration
		Delta_m = -loc_HourAngle/360.0;

		// Step B.6: determine the new point of time of transit
		m_new = m_alt + Delta_m;

		// save new value for next calculation step
		m_alt = m_new;

		// increment loop counter
		loop_cnt++; 		// limit max. number of loop runs
		} while( (( Delta_m > eps ) || ( Delta_m < -eps )) && ( loop_cnt < MAX_LOOP_CNT ) );

		ret_m0 = astroConst.C__hours_per_day*m_new;

		return( ret_m0 );
	}


	protected double calcRisingSetting(
			double arg_transit,
			double arg_H0, GA_coord [] arg_GA,
			observationPoint arg_PointOfObservation,
			czeit arg_timeOfObservation,
			int arg_RiseOrSet )

	{
		double eps = 0.0001;
		int loop_cnt = 0;
		const int MAX_LOOP_CNT = 5;

		// local variables
		double m = 0;
		double m_alt = 0;
		double m_new = 0;
		double Delta_m = 0;
		double ret_m = 0;
		double n;
		double [] loc_alpha = new double [3];
		double [] loc_delta = new double [3];
		double [] loc_time_base = new double [3];
		double delta_interp = 0.0;
		double alpha_interp = 0.0;
		double loc_SidTime0 = 0.0;
		double loc_HourAngle = arg_H0;
		double loc_altitude = 0.0;

		// why "+i-1": get JDs of previous day (-1 for i=0), current day (0 for i=1) and next day (1 for i=2)
		for (int i = 0; i < 3; i++)
		{
			loc_time_base[i] = arg_timeOfObservation.JD+i-1;
			loc_alpha[i] = arg_GA[i].hms_grad;
			loc_delta[i] = arg_GA[i].gms;
		}

		int loc_rising = (int)RiseOrSet.RISING;
		int loc_setting = (int)RiseOrSet.SETTING;

		if ( arg_RiseOrSet == loc_rising)			// 0: rising
			m = arg_transit/24.0 - arg_H0/360.0;
		else if ( arg_RiseOrSet == loc_setting) 	// 1: setting
			m = arg_transit/24.0 + arg_H0/360.0;
		else									// sonst: unklar
		{
			ret_m = 0;
			return ( ret_m );					// exit function without any additional calculation
		}

		// StepA: calc_m0
		SiderialTime loc_SidTime = new SiderialTime( arg_timeOfObservation );


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
		n = m_alt + arg_timeOfObservation.DeltaT/astroConst.C__sec_per_day;

		// Step B.2: interpolate the new position at the time instant represented by m
		// ---------------------------------------------------
		// Todo: Debug Abweichung in Interpolationsfunktion
		// ---------------------------------------------------
		alpha_interp = cinterp.interp_y3_n( loc_time_base, loc_alpha, 3, n ); //unit: degrees
		delta_interp = cinterp.interp_y3_n( loc_time_base, loc_delta, 3, n ); //unit: degrees

		// Meeus, Astronomical Algorithms, 2nd ed., p. 103
		// Step B.3: determine siderial time in Greenwhich in degrees
		loc_SidTime0 = astroConst.C__deg_per_hour*(loc_SidTime.get_meanSidTime0) + astroConst.C_SiderialDegrees*m_alt; //unit: degrees
		loc_SidTime0 = conversions.ohne_ueberlauf_degrees( loc_SidTime0 );

		// Step B.4: determine the new hour angle
		loc_HourAngle = loc_SidTime0 - arg_PointOfObservation.getGeogrLong  - alpha_interp; // unit: degrees

		// Step B.5: determine the altitude h according to Meeus, Astronomical Algorithms, 2nd ed., p. 93, Eqn. (13.6)
		loc_altitude = 	180/Math.PI*Math.Asin
						(
						Math.Sin( Math.PI/180*arg_PointOfObservation.getGeogrLat )*Math.Sin( Math.PI/180*delta_interp ) +
						Math.Cos( Math.PI/180*arg_PointOfObservation.getGeogrLat )*Math.Cos( Math.PI/180*delta_interp )*Math.Cos( Math.PI/180*loc_HourAngle )
						);
		// limit to [-90...+90]
		loc_altitude = ohne_ueberlauf_declination( loc_altitude );


		// Step B.6: calculate deviation to the last iteration
		// ToDo: C__H0_STARS!!!
		Delta_m = 	( loc_altitude - astroConst.C__H0_STARS )/
					( 360*Math.Cos( Math.PI/180*delta_interp )*Math.Cos(Math.PI/180*arg_PointOfObservation.getGeogrLat)*Math.Sin( Math.PI/180*loc_HourAngle ));

		// Step B.7: determine the new point of time of transit
		m_new = m_alt + Delta_m;

		// save new value for next calculation step
		m_alt = m_new;

		// increment loop counter
		loop_cnt++; 		// limit max. number of loop runs
		} while( (( Delta_m > eps ) || ( Delta_m < -eps )) && ( loop_cnt < MAX_LOOP_CNT ) );


		ret_m = astroConst.C__hours_per_day*m_new;

		return( ret_m );
	}




};

