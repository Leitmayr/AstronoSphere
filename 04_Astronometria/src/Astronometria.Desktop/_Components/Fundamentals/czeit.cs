using System;



/**
 *\struct sdatum
 *\brief enthaelt die Datenelemente fuer das Datum: Tag, Monat, Jahr als Ganzzahlen
 */
//public struct sdatum
//{
//public int day;            /**< Tag (Ganzzahl) */
//public int month;          /**< Monat (Ganzzahl) */
//public int year;           /**< Jahr (Ganzzahl) */
//public double yearComma;  /**< jahr in float */
//};
//public struct szeit
//{
//public int hour;                /**< hour (int)*/
//public int minute;              /**< minute (int) */
//public int second;              /**< second (int) */
//public double timeComma;       /**< time_comma (double)*/
//};




/**
 * \class 	czeit
 * \brief 	czeit = class "zeit"
 * 			kapselt die Eigenschaften der Zeit (Datum, Uhrzeit, JD)
 * 			Basisklassen: -
 *
 * Klasse fuer Zeitobjekte. Beim Erstellen der Objekte werden automatisch die Elemente, Datum, Uhrzeit und vor allem Julianisches
 * Datum erzeugt. Methoden:
 * - konvertiert Gregorianisches in Julianisches Datum und umgekehrt.
 * - legt Objekte des gegenwaertigen Zeitpunkts an (Standardkonstruktor)
 * - berechnet das JD des Tagesanfangs (JD0)
 * - stellt als static Methode die Tage seit JD2000.0 bereit
*/
public class czeit
{
	/**
	 *\struct szeit
	 *\brief enthaelt die Datenelemente fuer die Uhrzeit: Stunde, Minute, Sekunde je als Ganzzahlen, Uhrzeit in Kommadarstellung
	 */

	protected double _jd;           // Julian Date of the time instant
	protected double _jd0;          // Julian Date at the beginning of the day, 0 UT
	double _jde;                    // Julian Date considering TD */
	protected sdatum _date;         // Date in UT
	protected sdatum _dateTD;       // Date in Dynamical Time (TD)
	protected szeit _UT;            // time instant in Universal Time (UT)
	protected szeit _TD;            // time instant in Dynamical Time (TD)
	private double _deltaT;         // time difference between UT and TD	
	bool _leapYear;                 // is it a leap year or not?


	public double JD
	{
		get { return _jd; }
	}

	public double JD0
	{
		get { return _jd0; }
	}
	public double JDE
	{
		get { return _jde; }
	}

	public sdatum Date
	{
		get { return _date; }
	}

	public sdatum DateTD
	{
		get { return _dateTD; }
	}

	public szeit UT
	{
		get { return _UT; }
	}
	public szeit TD
	{
		get { return _TD; }
	}
	public double DeltaT
	{
		get { return _deltaT; }
	}
	public bool LeapYear
	{
		get { return _leapYear; }
	}


    #region Constructor
    // ==============================================================================
    // Constructors
    // input of date and time in UT
    public czeit(int argDay, int argMonth, int argYear, int argHour, int argMinute, int argSecond)
	{
		double locHourDouble = hms2double( argHour,  argMinute,  argSecond);


		_jd = GD2JD(argDay, argMonth, argYear, locHourDouble);
		_jd0 = calcJD0(_jd);

		this._UT.hour = argHour;
		this._UT.minute = argMinute;
		this._UT.second = argSecond;

		double locMinutePortion = (double)argMinute / 60;
		double locSecondPortion = (double) argSecond / 3600;
		
		this._UT.timeComma = argHour + locMinutePortion + locSecondPortion;

		this._date.day = argDay;
		this._date.month = argMonth;
		this._date.year = argYear;
		this._date.yearComma = calcYearComma(argYear, argMonth, argDay);

		this._leapYear = isLeapYear(argYear);
		calcTD(_date.year, _date.month, _date.day );

		_jde = GD2JD( _date.day, _date.month, _date.year, _TD.timeComma);
	}
	public czeit(int argDay, int argMonth, int argYear)
	{
		this._UT.hour = 0;
		this._UT.minute = 0;
		this._UT.second = 0;
		this._UT.timeComma = 0.0;

		this._date.day = argDay;
		this._date.month = argMonth;
		this._date.year = argYear;
		this._date.yearComma = calcYearComma(argYear, argMonth, argDay);

		_jd = GD2JD(argDay, argMonth, argYear, 0.0);
		_jd0 = calcJD0(_jd);

		this._leapYear = isLeapYear(argYear);
		calcTD( _date.year, _date.month, _date.day );
		_jde = GD2JD(_date.day, _date.month, _date.year, _TD.timeComma);
	}

	// input of date and time in UT
	public czeit(szeit argTimeStruct, sdatum argDateStruct)
	{
		this._UT.hour = argTimeStruct.hour;
		this._UT.minute = argTimeStruct.minute;
		this._UT.second = argTimeStruct.second;
		this._UT.timeComma = argTimeStruct.timeComma;

		this._date.year = argDateStruct.year;
		this._date.month = argDateStruct.month;
		this._date.day = argDateStruct.day;
		this._date.yearComma = argDateStruct.yearComma;

		this._leapYear = isLeapYear(this._date.year);


		this._jd = GD2JD(argDateStruct.day, argDateStruct.month, argDateStruct.year, argTimeStruct.timeComma);
		this._jd0 = calcJD0(this._jd);

		calcTD( _date.year, _date.month, _date.day );
		_jde = GD2JD(_date.day, _date.month, _date.year, _TD.timeComma);
	}

	// Standard constructor setting czeit object to 1.1.2000
	public czeit()
	{
		this._jd = 2451544.5; // 1.1.2000
		this._date = JD2GD(this._jd);
		_jd0 = calcJD0(this._jd);

		// this updates the UT time based on the deviation of JD and JD0
		this._UT = conversions.double2szeit(24 * (_jd - _jd0));

		this._leapYear = isLeapYear(this._date.year);

		calcTD(_date.year, _date.month, _date.day);
	}


	// input of the Julian Date in UT
	public czeit(double argJDinUT)
	{
		

		if ( argJDinUT > 0 )
		{
			this._jd = argJDinUT;
		}
		else
		{
			this._jd = 0.0;
		}

		this._date = JD2GD(this._jd);
		_jd0 = calcJD0(this._jd);

		// this updates the UT time based on the deviation of JD and JD0
		this._UT = conversions.double2szeit(24 * (_jd - _jd0));

		this._leapYear = isLeapYear(this._date.year);

		calcTD( _date.year, _date.month, _date.day );
	}
    #endregion
    // ==============================================================================


    // ==============================================================================
    // Methods

    // ------------------------------------------------------------------------------
    // Gregorian Date to Julian Date
    protected static double GD2JD(int argDay, int argMonth, int argYear, double arg_time_double)
	{

		double ret_JD = 0;

		// Hilfsgroessen
		double y;
		int m;
		int B;

		if (argMonth <= 2)
		{
			y = argYear - 1;
			m = argMonth + 12;
		}
		else
		{
			y = argYear;
			m = argMonth;
		}

		int A = (int)Math.Floor(y / 100);


		// Algorithm taken from MEEUS - ASTRONOMICAL ALGORITHMS, 2nd edition, p. 63
		//
		// this section checks whether the search date is before or after establishing the Gregorian calender
		// and it exits in the undefined time betweeen Oct 4 and Oct 15, 1582
		//
		if (argYear < Const.C__YearZeroOfGregorianCalendar)    // handling of the years 1581 or earlier, i.e. before establishing the Gregorian Calendar on Oct 4, 1582

		{
			B = 0;
		}
		else if (argYear > Const.C__YearZeroOfGregorianCalendar)  // handling of the years 1583 or later, i.e. after establishing the Gregorian Calendar on Oct 4, 1582

		{
			B = 2 - A + (int)(A / 4);
		}
		else // handling of the year 1582 itself
		{
			if (argMonth < 10) // handling of the months before October 1582, when the Gregorian Calendar was introduced
			{
				B = 0;
			}
			else if (argMonth > 10) // handling of the months after October 1582, when the Gregorian Calendar was introduced
			{
				B = 2 - A + (int)(A / 4);
			}
			else // handling of October 1582, when the Gregorian Calendar was introduced
			{
				if (argDay <= 4) // before October 4 the old formula needs to be used
				{
					B = 0;
				}
				else if (argDay >= 15) // after October 15 the new formula needs to be used
				{
					B = 2 - A + (int)(A / 4);
				}
				else // handling of the days in the  unspecified time between Thursday, October 4 and Friday, October 15, 1582
				{
					return (-1); // exit code: data not specified!
				}
			}
		}

		// Berechnung des Julianischen Datums
		ret_JD = (int)(365.25 * (y + 4716)) + (int)(30.6001 * (m + 1)) + (argDay + arg_time_double / 24) + B - 1524.5;


		// TODO: ExitCode bei Werten vor dem 1.1.-4712, 12 Uhr
		if (ret_JD < 0)
			ret_JD = 0;


		return (ret_JD);
	}


	// ------------------------------------------------------------------------------
	// Julian Date to Gregorian Date
	protected static sdatum JD2GD(double argJD)
	{
		// =============================================================================================
		//
		// Algorithm taken from MONTENBRUCK - GRUNDLAGEN DER EPHEMERIDENRECHNUNG, 6. edition, p.43
		//
		// =============================================================================================
		double a, b, c, d, e, f;
		int locDay, locMonth, locYear;

		a = Math.Floor(argJD + 0.5);

		if (a < 2299161)
			c = a + 1524;
		else
		{
			b = Math.Floor((a - 1867216.25) / 36524.25);
			c = a + b - Math.Floor(b / 4) + 1525;
		}

		d = Math.Floor((c - 122.1) / 365.25);
		e = Math.Floor(365.25 * d);
		f = Math.Floor((c - e) / 30.6001);

		locDay = (int)(c - e - (int)(30.6001 * f) + (argJD + 0.5 - a));
		locMonth = (int)(f - 1 - 12 * (int)(f / 14));
		locYear = (int)(d - 4715 - (int)((7 + locMonth) / 10));

		sdatum ret_GD;
		ret_GD.day = locDay;
		ret_GD.month = locMonth;
		ret_GD.year = locYear;
		ret_GD.yearComma = calcYearComma(locYear, locMonth, locDay);

		return (ret_GD);
	}

	protected static double calcJD0(double arg_JDdouble)
	{
		double ret_JD0 = 0;
		double JD_0_UT = Math.Floor(arg_JDdouble);



		double delta_JD = arg_JDdouble - JD_0_UT;


		if (delta_JD >= 0.5)
		{
			//uhrzeit.uhr_comma = 24*(delta_JD+0.5);
			ret_JD0 = JD_0_UT + 0.5;
			return ((double)ret_JD0);
		}
		else
		{
			//uhrzeit.uhr_comma = 24*(delta_JD-0.5);
			ret_JD0 = JD_0_UT - 0.5;

			// limit JD0 to values >= 0.0
			if (ret_JD0 < 0)
			{
				ret_JD0 = 0;
			}
			return ((double)ret_JD0);
		}

	}
	// ------------------------------------------------------------------------------
	// determine if a leap years
	public static bool isLeapYear(int argYear)
	{

		// initialization of the return value: by standard the return value is "not leap year"
		bool ret_isLeapYear = false;

		// initialization of the bool variables for the exceptions
		// ---------------------------------------------
		bool jahresbed = false;
		bool not_jahrhundert_std = false;
		bool jahrhundert_ausnahme = false;

		// treat exceptions now
		// ---------------------------------------------

		// a leap year is a year which can be divided
		int loc_Help = 0;
		loc_Help = argYear % 4;
		if (loc_Help == 0)
			jahresbed = true;
		else
			jahresbed = false;

		// exception1: every 100 years a year is not a leap year even if it can be divided by 4
		loc_Help = argYear % 100;
		if (loc_Help == 0)
			not_jahrhundert_std = false;
		else
			not_jahrhundert_std = true;

		// exception2: every 400 years a year is a leap year although it can be divided by 100
		loc_Help = argYear % 400;
		if (loc_Help == 0)
			jahrhundert_ausnahme = true;
		else
			jahrhundert_ausnahme = false;

		// ----------------------------------------------------------------------------------------------------
		// the definition of a leap year (=Schaltjahr) are taken from
		// MEEUS - ASTRONOMICAL ALGORITHMS, 2nd edition, p. 62
		//
		if (argYear > Const.C__YearZeroOfGregorianCalendar) // Rule applies for Gregorian Calendar (after the year 1582)
		{
			if ((jahresbed && not_jahrhundert_std) || jahrhundert_ausnahme)
			{
				ret_isLeapYear = true;
			}
		}
		else  // Rule applies for Julian Calendar (before the year 1582)
		{
			if (jahresbed == true)
			{
				ret_isLeapYear = true;
			}
		}
		// ----------------------------------------------------------------------------------------------------

		return (ret_isLeapYear);

	}


	public static double centuriesSinceJ2000 ( double arg_JD )
	{
		double loc_DynamicTime = arg_JD;
/*		czeit help = new czeit (arg_JD);
		double loc_timeComma = help.TD.timeComma;

		double loc_DynamicTime = arg_JD;
		loc_DynamicTime = calcJD0(arg_JD);

		// Julian Date in TD
		loc_DynamicTime = loc_DynamicTime + loc_timeComma;*/

		return ( (loc_DynamicTime - astroConst.C__JD2000) / astroConst.C__daysPerCentury ); //centuries snce J2000.0, neg. before J2000.0
	}

	public static double millenniaSinceJ2000(  double arg_JD )
{

		double loc_DynamicTime = arg_JD; 
		/*		czeit help = new czeit(arg_JD);
				double loc_timeComma = help.TD.timeComma;

				double loc_DynamicTime = arg_JD;
				loc_DynamicTime = calcJD0(arg_JD);
		*/
		// Julian Date in TD
		// loc_DynamicTime = loc_DynamicTime + loc_timeComma;

		return ( (loc_DynamicTime - astroConst.C__JD2000)/ astroConst.C__daysPerMillenium); //millienia since J2000.0, neg. before J2000.0
	}



	protected void JD2all(double arg_JDatum)
	{
		_jd = arg_JDatum;
		_date = JD2GD(JD);
		_date.yearComma = calcYearComma(_date.year, _date.month, _date.day);
		_jd0 = calc_JD0(JD);

		if (JD < 0.5) // "special case": handle the first Julian Day
		{
			_UT.timeComma = 12 + 24 * JD; // JD 0.0 is already 12 hour
		}
		else // "normal case"
		{
			_UT.timeComma = 24 * (JD - JD0);
		}

		int[] std_min_sek = {  0, 0, 0 };
		std_min_sek = conversions.double2hour(_UT.timeComma);
		_UT.hour = std_min_sek[0];
		_UT.minute = std_min_sek[1];
		_UT.second = std_min_sek[2];

		// New calculation of TD in dedicated method calcTD(.);
		calcTD(_date.year, _date.month, _date.day);
		_jde = GD2JD(_date.day, _date.month, _date.year, _TD.timeComma);

/*		// Old implementation of TD
 *		
 *		_deltaT = calcDT(_jd, _dateTD.year);
		_TD.timeComma = _UT.timeComma + _deltaT / 3600;


 *		_dateTD.year = _date.year;
		_dateTD.month = _date.month;
		_dateTD.day = _date.day;
		_dateTD.yearComma = calcYearComma(_dateTD.year, _dateTD.month, _dateTD.day);


		if (_TD.timeComma > 24)
		{
			_TD.timeComma -= 24;
			_dateTD.day = _date.day + 1;
		}
		else if (_TD.timeComma < 0)
		{
			_TD.timeComma += 24;
			_dateTD.day = _date.day - 1;
		}
		else 
		{ 
			// do nothing
		}

		std_min_sek = conversions.double2hour(_TD.timeComma);
		_TD.hour = std_min_sek[0];
		_TD.minute = std_min_sek[1];
		_TD.second = std_min_sek[2];*/

	}

	protected static int getNumberDaysThatYear(int argYear)
	{
		if (isLeapYear(argYear))
		{
			return (Const.C__numberOfDaysLeapYear); //366 days
		}
		else
			return (Const.C__numberOfDaysStandardYear); //365 days
	}

	protected static double calcYearComma(int argYear, int argMonth, int argDay)
	{
		double locDayOfThatYear = (double)(dayOfTheYear(argYear, argMonth, argDay));
		double locNumberDaysThatYear = (double)(getNumberDaysThatYear(argYear));
		return (argYear + locDayOfThatYear / locNumberDaysThatYear);
	}

	protected static int dayOfTheYear(int argYear, int argMonth, int argDay)
	{

		int locDaysOfFullMonths = 0;
		int ret_dayOfTheYear = 0;
		if (isLeapYear(argYear)) // leap year
		{
			for (int i = 0; i < argMonth - 1; i++)
				locDaysOfFullMonths += Const.C__days_per_month_sjahr[i];
		}
		else // normal year
		{
			for (int i = 0; i < argMonth - 1; i++)
				locDaysOfFullMonths += Const.C__days_per_month_njahr[i];
		}

		// add the days of full months to the day of the month -> day of the year
		ret_dayOfTheYear = locDaysOfFullMonths + argDay;

		return (ret_dayOfTheYear);
	}

	protected double calcDT(double arg_JulDat, int argYearInUT)
	{
		// locals
		int loc_index;
		double ret_DynTime_Delta_T;
		double loc_centuriesSinceJ2000 = (arg_JulDat - Const.C__JD2000) / Const.C__daysPerCentury; //Jahrhunderte (cent.) seit dem Jahr 2000, neg. wenn vor J2000.0
		double loc_yearsSinceJ2000 = loc_centuriesSinceJ2000 * 100;


		if (argYearInUT < 948)
		{
			ret_DynTime_Delta_T = 2177 + 497 * loc_centuriesSinceJ2000 + 44.1 * loc_centuriesSinceJ2000 * loc_centuriesSinceJ2000; // Meeus [2] S.78
	//		ret_DynTime_Delta_T = 2715.5 + 573.36 * loc_centuriesSinceJ2000 + 46.5 * loc_centuriesSinceJ2000 * loc_centuriesSinceJ2000; // Meeus [1] S.73
		}
		else if ((948 <= argYearInUT) && (argYearInUT < 1600))
		{
			ret_DynTime_Delta_T = 102 + 102 * loc_centuriesSinceJ2000 + 25.3 * loc_centuriesSinceJ2000 * loc_centuriesSinceJ2000; // Meeus [1] S.73
//			ret_DynTime_Delta_T = 50.6 + 67.5 * loc_centuriesSinceJ2000 + 22.5 * loc_centuriesSinceJ2000 * loc_centuriesSinceJ2000; // Meeus [1] S.73
		}
		else if ((1600 <= argYearInUT) && (argYearInUT < 1620))
		{
			// lineare Interpolation zwischen dem letzten gueltigen Approximationswert der Formel auf S. 73  fuer den Zeitraum 948 ... 1600
			// und dem aus Meeus [1] entnommenen ersten Tabellenwert (S.72) fuer 1620
			//cout << datum.jahr;
			double year = argYearInUT;
			ret_DynTime_Delta_T = Const.C__DynTime_1600 - (year - 1600) / (1620 - 1600) * (Const.C__DynTime_1600 - Const.C__DynTime_1620);
		}
		else if ((1620 <= argYearInUT) && (argYearInUT <= 1992))
		{
			// index determination
			loc_index = calc_DeltaT_LUT_ind(argYearInUT);

			// LUT Evaluation
			if ((loc_index >= 0) && (loc_index < Const.C_Max_DynLUT_ind))
				ret_DynTime_Delta_T = 0.5*( Const.DTLUT[loc_index] + Const.DTLUT[loc_index+1] ); // interpolate
			else if (loc_index < 0)
				ret_DynTime_Delta_T = Const.DTLUT[0];
			else
				ret_DynTime_Delta_T = Const.DTLUT[Const.C_Max_DynLUT_ind];

		}
		else if ((1992 < argYearInUT) && (argYearInUT <= 2005))
		{
			//	Between years 1986 and 2005
			//  taken from http://eclipse.gsfc.nasa.gov/SEcat5/deltatpoly.html am 2012-06.26
			ret_DynTime_Delta_T = 63.86 + 0.3345 * loc_yearsSinceJ2000
					- 0.060374 * loc_yearsSinceJ2000 * loc_yearsSinceJ2000
					+ 0.0017275 * loc_yearsSinceJ2000 * loc_yearsSinceJ2000 * loc_yearsSinceJ2000
					+ 0.000651814 * loc_yearsSinceJ2000 * loc_yearsSinceJ2000 * loc_yearsSinceJ2000 * loc_yearsSinceJ2000
					+ 0.00002373599 * loc_yearsSinceJ2000 * loc_yearsSinceJ2000 * loc_yearsSinceJ2000 * loc_yearsSinceJ2000 * loc_yearsSinceJ2000;

			//		where: t = y - 2000
		}
		else if ((2005 < argYearInUT) && (argYearInUT <= 2050))
		{
			//
			//	Between years 2005 and 2050, calculate:
			//  taken from http://eclipse.gsfc.nasa.gov/SEcat5/deltatpoly.html am 2012-06.26
			ret_DynTime_Delta_T = 62.92 + 0.32217 * loc_yearsSinceJ2000 + 0.005589 * loc_yearsSinceJ2000 * loc_yearsSinceJ2000;
			//		where: t = y - 2000
			//
			//	This expression is derived from estimated values of Delta_T in the years 2010 and 2050.
			//  The value for 2010 (66.9 seconds) is based on a linearly extrapolation from 2005 using 0.39 seconds/year
			//  (average from 1995 to 2005). The value for 2050 (93 seconds) is linearly extrapolated from 2010 using 0.66
			//  seconds/year (average rate from 1901 to 2000).
		}
		else  // globale Approximationsformel mittels Polynom
		{
			// Outside the period of observations (500 BCE to 2005 CE), the value of Delta_T can be extrapolated from measured values
			// using the long-term mean parabolic trend: Delta_T = -20 + 32 * t^2 seconds

			//  taken from http://eclipse.gsfc.nasa.gov/SEcat5/deltatpoly.html am 2012-06.26
			double t = (argYearInUT - 1820) / 100;
			ret_DynTime_Delta_T = -20 + 32 * t * t;
		}

		return (ret_DynTime_Delta_T);
	}

	int calc_DeltaT_LUT_ind(int arg_year)
	{
		double locDiff = arg_year - Const.C__DynTime_LUT_start;
		double ret_LUTValue = (int)Math.Floor(locDiff) / 2;
		return ((int)(ret_LUTValue));
	}

	public void calcTD( int argYearInUT, int argMonthInUT, int argDayInUT )
	{
		// calc Delta T, return is in seconds
		_deltaT = calcDT(_jd, argYearInUT);

		_dateTD.year = argYearInUT;
		_dateTD.month = argMonthInUT;
		_dateTD.day = argDayInUT;
		_dateTD.yearComma = calcYearComma(_dateTD.year, _dateTD.month, _dateTD.day);

		// Delta T to hours: divide by 3600
		_TD.timeComma = _UT.timeComma + _deltaT / 3600;




		// this code corrects the date in case there is a day/month/year shift caused by the deviation between UT and TD
		if (_TD.timeComma < 0) // this indicates that a shift back to the previous day has occurred
		{
			if (_date.day == 1) // if the shift back occurs on the first day of the month it causes a month or both a month and a year shift
			{
				if (_dateTD.month > 1) // this treats the case that it is not a year shift 
				{
					_dateTD.month--;
					if (_dateTD.month == 2) // after shift back it is a february
					{
						if (_leapYear)      // the february is in a leap year
						{
							_dateTD.day = 29;   // 29 days in leap year
						}
						else
						{
							_dateTD.day = 28;   // 28 days in ordinary year
						}
					}
					else
					{
						_dateTD.day = Const.C__days_per_month_njahr[_dateTD.month - 1]; // #last day of month
					}
				}
				else
				{
					// shift back causes year shift back and month shift back in December to 31.12.(Y-1)
					_dateTD.month = 12;
					_dateTD.year--;
					_dateTD.day = 31;

				}
			} // end of month of month/year back shift
			else // treat the fact that is an ordinary back shift during the month
			{
				_dateTD.day--;  // reduce the day by one
			}
		_TD.timeComma += 24; // correct the time now, since it is negative
		}
		else if (_TD.timeComma > 24) // this indicates that a shift forward to the next day has occurred
		{
			// this and the next if are required to determine the last day of the month
			int loc_lastDayOfMonth = Const.C__days_per_month_njahr[_dateTD.month - 1]; // pick value from month-length-table

			if ( (_date.month == 2) && (_leapYear ) ) // if it is a february and a leap year, increment value from table
			{
				loc_lastDayOfMonth++;
			}

			// check now for the last day of the month
			if (loc_lastDayOfMonth == _dateTD.day)  // if it is the last day of the month
			{

				if (_dateTD.month != 12)            // if it is not the last day in december
				{
					_dateTD.day = 1;                // the day is shifted forward to the first day of the next month
					_dateTD.month++;                // since it is the next month the month must be incremented
				}
				else                                // if it is the last day in december the shift forward causes a year shift, too
				{
					_dateTD.day = 1;                // the next day is New Year's Day: 1.1.(Y+1)
					_dateTD.month = 1;
					_dateTD.year++;                 //  increment the year as well
				}
			}
			else									// it is not the last day, just increment the day
			{
				_dateTD.day++;						// increment only the day, no other actions
			}
		_TD.timeComma -= 24;						// correct the time now, since it was > 24
		}

		szeit loc_timeTD = new szeit();

		loc_timeTD = conversions.double2szeit(_TD.timeComma);

		_TD.hour = loc_timeTD.hour;
		_TD.minute = loc_timeTD.minute;	
		_TD.second = loc_timeTD.second;

	}

	// Achtung: nahe an double2szeit aus conversions aber umfänglicher -> vermutlich bessere Implementierung, ggf. double2szeit in conversions damit ersetzen
	szeit double2zeit(double arg_hdoub)
	{
		szeit ret_sDate;
		double loc_decimalPlace;
		int locHour;

		if (arg_hdoub < 0.0)
		{
			loc_decimalPlace = arg_hdoub - (int)(Math.Ceiling(arg_hdoub));
		}
		else
		{
			loc_decimalPlace = arg_hdoub - (int)(Math.Floor(arg_hdoub));
		}

		if (loc_decimalPlace < 0.0)
		{
			loc_decimalPlace = -loc_decimalPlace;
		}

		if (arg_hdoub < 0.0)
		{
			locHour = (int)(arg_hdoub + loc_decimalPlace);
		}
		else
		{
			locHour = (int)(arg_hdoub - loc_decimalPlace);
		}

		ret_sDate.timeComma = arg_hdoub;
		ret_sDate.hour = locHour;

		ret_sDate.minute = (int)((loc_decimalPlace * 3600) / 60);

		double locMinutePortion = (double) ret_sDate.minute / 60;
 
		ret_sDate.second = (int)((loc_decimalPlace - locMinutePortion) * 3600);



		return ( ret_sDate);
	}

	double hms2double(int arg_std, int arg_hmin, int arg_hsec)
	{

	double ret_hourInDouble = 0.0;

	double loc_minSecInSec = 0.0;
	double loc_secFractionOfHour = 0.0; // describes the portion of an hour of a number of seconds


	// fängt den Fall Sonderfall -00:XX:YY ab:
	// in diesem Fall kann mit grad nicht das VZ übergeben werden, weil arg_std = 0 ist.
	// Daher wird arg_min als Unterscheider verwendet

	if (arg_hmin >= 0 )
		loc_minSecInSec = arg_hmin* 60 + arg_hsec; //convert min:ss -> ss
	else
		loc_minSecInSec = arg_hmin* 60 - arg_hsec; //convert min:ss -> ss

	loc_secFractionOfHour = (double)(loc_minSecInSec/3600 );

	if (arg_std >= 0)
		ret_hourInDouble = (double)((double)arg_std+loc_secFractionOfHour );
	else
		ret_hourInDouble = (double)((double)arg_std -loc_secFractionOfHour );

	return (ret_hourInDouble );
}
	// refactored
	public void set_JulDat(double arg_JDatum)
	{
		double loc_JDatum = arg_JDatum;
		JD2all(loc_JDatum);
	
		// Todo: Observer	
		//notifyObserver();
	}


	public double calc_JD0(double arg_JDdouble)
	{
		double ret_JD0 = 0;
		double JD_0_UT = Math.Floor(arg_JDdouble);



		double delta_JD = arg_JDdouble - JD_0_UT;


		if (delta_JD >= 0.5)
		{
			//uhrzeit.uhr_comma = 24*(delta_JD+0.5);
			ret_JD0 = JD_0_UT+0.5;
			return((double)(ret_JD0));
		}
		else
		{
			//uhrzeit.uhr_comma = 24*(delta_JD-0.5);
			ret_JD0 = JD_0_UT - 0.5;

			// limit JD0 to values >= 0.0
			if (ret_JD0 < 0)
			{
				ret_JD0 = 0;
			}
			return ((double)(ret_JD0));
		}

	}


}
