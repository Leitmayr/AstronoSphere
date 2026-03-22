/**
 * \file czeit.cpp
 * \author Marcus Hiemer
 * \date 2015-01-22
 *
 * 2011-11-16: implementiert die Klasse czeit
 * 2015-01-22: refactored
*/

#include "czeit.h"

using namespace std;



void czeit::notifyObserver(void)
{
	std::list<observer*>::const_iterator iterator;
	for (iterator = obs_list.begin(); iterator != obs_list.end(); ++iterator) {
	    (*iterator)->update(this);
	}

};

void czeit::registerObserver(observer *Observer)
{
	obs_list.push_back(Observer);
};

void czeit::removeObserver(observer *Observer)
{
	obs_list.remove(Observer);
};

// ================= Konstruktoren ================================

czeit::czeit():observable(), conversions()
{
	now();

	//check_input();

	uhrzeit.uhr_comma = hms2double(	uhrzeit.stunde,	uhrzeit.minute, uhrzeit.sekunde);
	JD = GD2JD(datum.tag, datum.monat, datum.jahr, uhrzeit.uhr_comma);
	JD0 = calc_JD0(JD);

	Delta_T = calc_DT(JD);

	JDE = JD + Delta_T/C__sec_per_day;

	TD_dat.jahr = datum.jahr;
	schaltjahr = is_schaltjahr(datum.jahr);

	TD_dat.monat = datum.monat;
	TD_dat.tag = datum.tag;


	TD_zeit.uhr_comma = uhrzeit.uhr_comma+Delta_T/3600;
	if (TD_zeit.uhr_comma > 24)
	{
		TD_zeit.uhr_comma -= 24;
		TD_dat.tag = datum.tag+1;
	}
	else if (TD_zeit.uhr_comma < 0)
	{
		TD_zeit.uhr_comma += 24;
		TD_dat.tag = datum.tag-1;
	}
	TD_dat.jahr_comma = calc_jahr_comma(TD_dat.jahr, TD_dat.monat, TD_dat.tag);
	int std_min_sek[3];
	double2hour(TD_zeit.uhr_comma, std_min_sek);
	TD_zeit.stunde = std_min_sek[0];
	TD_zeit.minute = std_min_sek[1];
	TD_zeit.sekunde = std_min_sek[2];


}


// Unit Tests
czeit::czeit(const double &JDatum):observable(), conversions()
{
	double arg_JDatum = JDatum;
	JD2all(arg_JDatum);

}


// Unit Tests
czeit::czeit(const int &tag, const int &monat, const int &jahr, const int &hour, const int &min, const int &sek):observable(), conversions()
{


	uhrzeit.stunde = hour;
	uhrzeit.minute = min;
	uhrzeit.sekunde = sek;

	uhrzeit.uhr_comma = hms2double(hour, min, sek);

	datum.tag = tag;
	datum.monat = monat;
	datum.jahr = jahr;
	datum.jahr_comma = calc_jahr_comma(datum.jahr, datum.monat, datum.tag);

	schaltjahr = is_schaltjahr(datum.jahr);

	//check_input();

	JD = GD2JD(datum.tag, datum.monat, datum.jahr, uhrzeit.uhr_comma);
	JD0 = calc_JD0(JD);

	Delta_T = calc_DT(JD);

	JDE = JD + Delta_T/C__sec_per_day;

	TD_dat.jahr = datum.jahr;


	TD_dat.monat = datum.monat;
	TD_dat.tag = datum.tag;



	TD_zeit.uhr_comma = uhrzeit.uhr_comma+Delta_T/3600;
	if (TD_zeit.uhr_comma > 24)
	{
		TD_zeit.uhr_comma -= 24;
		TD_dat.tag = datum.tag+1;
	}
	else if (TD_zeit.uhr_comma < 0)
	{
		TD_zeit.uhr_comma += 24;
		TD_dat.tag = datum.tag-1;
	}
	TD_dat.jahr_comma = calc_jahr_comma(TD_dat.jahr, TD_dat.monat, TD_dat.tag);
	int std_min_sek[3];
	double2hour(TD_zeit.uhr_comma, std_min_sek);
	TD_zeit.stunde = std_min_sek[0];
	TD_zeit.minute = std_min_sek[1];
	TD_zeit.sekunde = std_min_sek[2];
	//cout << TD_zeit.uhr_comma << "=" << TD_zeit.stunde << ":" << TD_zeit.minute << ":" << TD_zeit.sekunde;



}


czeit::czeit(const szeit &TD_time, const sdatum &TD_date):observable(), conversions()
{
	TD_dat.tag = TD_date.tag;
	TD_dat.monat = TD_date.monat;
	TD_dat.jahr = TD_date.jahr;
	schaltjahr = is_schaltjahr(TD_dat.jahr);

	TD_dat.jahr_comma = calc_jahr_comma(TD_dat.jahr, TD_dat.monat, TD_dat.tag);

	TD_zeit.stunde = TD_time.stunde;
	TD_zeit.minute = TD_time.minute;
	TD_zeit.sekunde = TD_time.sekunde;

	TD_zeit.uhr_comma = hms2double(TD_zeit.stunde, TD_zeit.minute, TD_zeit.sekunde);

	datum.jahr = TD_dat.jahr;
	datum.monat = TD_dat.monat;
	datum.tag = TD_dat.tag;
	datum.jahr_comma = calc_jahr_comma(datum.jahr, datum.monat, datum.tag);


	//check_input();

	// dummy: hier ist die UT Uhrzeit noch nicht bekannt, dennoch muss uhr_comma
	// gesetzt werden, damit das Julianische Datum approximiert werden kann, mit dem Delta_T errechnet wird
	uhrzeit.uhr_comma = TD_zeit.uhr_comma;
	JD = GD2JD(datum.tag, datum.monat, datum.jahr, uhrzeit.uhr_comma);

	Delta_T = calc_DT(JD);

	JDE = JD + Delta_T/C__sec_per_day;

	uhrzeit.uhr_comma = TD_zeit.uhr_comma - Delta_T/3600;
	if (uhrzeit.uhr_comma > 24)
	{
		uhrzeit.uhr_comma -= 24;
		datum.tag = datum.tag+1;
	}
	else if (uhrzeit.uhr_comma < 0)
	{
		uhrzeit.uhr_comma += 24;
		datum.tag = datum.tag-1;
	}
	int std_min_sek[3];
	double2hour(uhrzeit.uhr_comma, std_min_sek);
	uhrzeit.stunde = std_min_sek[0];
	uhrzeit.minute = std_min_sek[1];
	uhrzeit.sekunde = std_min_sek[2];

	JD = GD2JD(datum.tag, datum.monat, datum.jahr, uhrzeit.uhr_comma);
	JD0 = calc_JD0(JD);

	JDE = JD + Delta_T/C__sec_per_day;

}


// ================= Destruktor ================================

czeit::~czeit(void) {}


// ================= private methods ================================


// refactored
void czeit::now(void)
{
	time_t now = time(NULL);
	tm *z = localtime(&now);

	uhrzeit.stunde = z-> tm_hour;
	uhrzeit.minute = z-> tm_min;
	uhrzeit.sekunde = z-> tm_sec;

	datum.tag = z->tm_mday;
	datum.monat = z->tm_mon + 1;
	datum.jahr = z->tm_year + 1900;
	datum.jahr_comma = calc_jahr_comma(datum.jahr, datum.monat, datum.tag);

	schaltjahr = is_schaltjahr(datum.jahr);

}


void czeit::check_input(void)
// TODO: aufrufe sind auskommentiert - zu häufig unplausible Vertöße erkannt
{
	if ( uhrzeit.stunde > 23 )
	{
		cerr << endl << "hour out of range";
	};
	if ( uhrzeit.minute > 59 )
	{
		cerr << endl << "minute out of range";
	}
	if ( uhrzeit.sekunde > 59 )
	{
		cerr << endl << "second out of range";
	}

	if ( datum.monat  > 12 )
	{
		cerr << endl << "month out of range";
	}

	//Check if day too large. Determine max
	int loc_maxDayValue = C__days_per_month_njahr[ datum.monat -1 ];
	// check for leap year
	if ( datum.monat  == 2 )
	{
		if ( schaltjahr )
		{
			loc_maxDayValue = 29;
		}
	}

	if ( datum.tag  > loc_maxDayValue )
	{
		cerr << endl << "day out of range";
	}
}


//// refactored
//void czeit::double2hour(const double &arg_hdoub, int ret_hms[3])
//{
//	double loc_decimalPlace;
//
//	if (arg_hdoub < 0.0)
//		loc_decimalPlace = arg_hdoub - static_cast<double>(ceil(arg_hdoub));
//	else
//		loc_decimalPlace = arg_hdoub - static_cast<double>(floor(arg_hdoub));
//
//	if (loc_decimalPlace < 0.0)
//		loc_decimalPlace = - loc_decimalPlace;
//
//	if (arg_hdoub < 0.0)
//		ret_hms[0] = static_cast<int>(arg_hdoub + loc_decimalPlace);
//	else
//		ret_hms[0] = static_cast<int>(arg_hdoub - loc_decimalPlace);
//
//	ret_hms[1] = static_cast<int>((loc_decimalPlace*3600)/ 60);
//	ret_hms[2] = (static_cast<int>(loc_decimalPlace*3600))% 60;
//}
//
//// refactored
//double czeit::hms2double(const int &arg_std, const int &arg_hmin, const int &arg_hsec)
//{
//
//	double loc_minSecInSec;
//	double loc_secFractionOfHour; // describes the portion of an hour of a number of seconds
//
//	loc_minSecInSec = arg_hmin*C__sec_per_min+arg_hsec; //convert min:ss -> ss
//	loc_secFractionOfHour = static_cast<double> (loc_minSecInSec/C__sec_per_hour);
//
//	if (arg_std >= 0)
//		return(static_cast<double> (arg_std+loc_secFractionOfHour));
//	else
//		return(static_cast<double> (arg_std-loc_secFractionOfHour));
//}

// Unit tests available
// refactored
unsigned int czeit::day_of_the_year(const int& arg_year, const int &arg_month, const int &arg_day)
{

	unsigned int loc_daysOfFullMonths = 0;
	unsigned int ret_dayOfTheYear = 0;
	if (is_schaltjahr(arg_year)) // leap year
	{
		for (int i = 0; i < arg_month-1; i++)
			loc_daysOfFullMonths += C__days_per_month_sjahr[i];
	}
	else // normal year
	{
		for (int i = 0; i < arg_month-1; i++)
			loc_daysOfFullMonths += C__days_per_month_njahr[i];
	}

	// add the days of full months to the day of the month -> day of the year
	ret_dayOfTheYear = loc_daysOfFullMonths+arg_day;

	return(ret_dayOfTheYear);
}

// Unit tests available
double czeit::calc_jahr_comma(const int &arg_year, const int &arg_month, const int &arg_day)
{
	double loc_dayOfThatYear = static_cast<double>(day_of_the_year(arg_year, arg_month, arg_day));
	double loc_numberDaysThatYear = static_cast<double>(get_no_days_that_year(arg_year));
	return(arg_year + loc_dayOfThatYear/loc_numberDaysThatYear);

}

// Unit tests available
// refactored
sdatum czeit::JD2GD(const double &arg_JD)
{
	// =============================================================================================
	//
	// Algorithm taken from MONTENBRUCK - GRUNDLAGEN DER EPHEMERIDENRECHNUNG, 6. edition, p.43
	//
	// =============================================================================================
	double a, b, c, d, e, f;
	a = floor(arg_JD+0.5);

	if (a < 2299161)
		c = a+1524;
	else
	{
		b = floor((a-1867216.25)/36524.25);
		c = a+b-floor(b/4)+1525;
	}

	d = floor((c-122.1)/365.25);
	e = floor(365.25*d);
	f = floor((c-e)/30.6001);

	int loc_day= static_cast<int>(c - e - static_cast<int>(30.6001*f) + (arg_JD + 0.5 - a));
	int loc_month = static_cast<int>(f - 1 - 12*static_cast<int>(f/14));
	int loc_year =  static_cast<int>(d - 4715 - static_cast<int>((7 + loc_month)/10));

	sdatum ret_GD;
	ret_GD.tag = loc_day;
	ret_GD.monat = loc_month;
	ret_GD.jahr = loc_year;
	ret_GD.jahr_comma = calc_jahr_comma(loc_year, loc_month, loc_day);
	return(ret_GD);
}

// Unit tests available
// refactored
double czeit::GD2JD(const int &arg_day, const int &arg_month, const int &arg_year, const double &arg_time_double)
{

	double ret_JD = 0;


	// Hilfsgroessen
	double y;
	int m;
	int B;

	if (arg_month <= 2)
	{
		y = arg_year - 1;
		m = arg_month + 12;
	}
	else
	{
		y = arg_year;
		m = arg_month;
	}

	int A = static_cast<int>(floor(static_cast<double>(y/100)));


	// Algorithm taken from MEEUS - ASTRONOMICAL ALGORITHMS, 2nd edition, p. 63
	//
	// this section checks whether the search date is before or after establishing the Gregorian calender
	// and it exits in the undefined time betweeen Oct 4 and Oct 15, 1582
	//
	if (arg_year<1582) 	// handling of the years 1581 or earlier, i.e. before establishing the Gregorian Calendar on Oct 4, 1582

	{
		B = 0;
	}
	else if (arg_year>1582)  // handling of the years 1583 or later, i.e. after establishing the Gregorian Calendar on Oct 4, 1582

	{
		B = 2 - A + static_cast<int>(A/4);
	}
	else // handling of the year 1582 itself
	{
		if (arg_month < 10) // handling of the months before October 1582, when the Gregorian Calendar was introduced
		{
			B = 0;
		}
		else if (arg_month > 10) // handling of the months after October 1582, when the Gregorian Calendar was introduced
		{
			B = 2 - A + static_cast<int>(A/4);
		}
		else // handling of October 1582, when the Gregorian Calendar was introduced
		{
			if (arg_day <= 4) // before October 4 the old formula needs to be used
			{
				B = 0;
			}
			else if (arg_day >= 15) // after October 15 the new formula needs to be used
			{
				B = 2 - A + static_cast<int>(A/4);
			}
			else // handling of the days in the  unspecified time between Thursday, October 4 and Friday, October 15, 1582
			{
				return(-1); // exit code: data not specified!
			}
		}
	}


	// Berechnung des Julianischen Datums
	ret_JD =static_cast<int>(365.25*(y+4716)) + static_cast<int>(30.6001*(m+1)) + (arg_day + arg_time_double/24) + B - 1524.5;


	// TODO: ExitCode bei Werten vor dem 1.1.-4712, 12 Uhr
	if (ret_JD < 0)
		ret_JD = 0;


	return(ret_JD);
}

// Unit tests available
// refactored
double czeit::calc_JD0(const double &arg_JDdouble)
{
	double ret_JD0 = 0;
	double JD_0_UT = floor(arg_JDdouble);



	double delta_JD = arg_JDdouble - JD_0_UT;


	if (delta_JD >= 0.5)
	{
		//uhrzeit.uhr_comma = 24*(delta_JD+0.5);
		ret_JD0 = JD_0_UT+0.5;
		return(static_cast<double>(ret_JD0));
	}
	else
	{
	//uhrzeit.uhr_comma = 24*(delta_JD-0.5);
		ret_JD0 = JD_0_UT-0.5;

		// limit JD0 to values >= 0.0
		if(ret_JD0 < 0)
		{
			ret_JD0 = 0;
		}
		return(static_cast<double>(ret_JD0));
	}

}


// refactored
unsigned int czeit::calc_DeltaT_LUT_ind(const int &arg_year)
{
	double ret_LUTValue = floor(arg_year - C__DynTime_LUT_start)/2;
	return(static_cast<int>(ret_LUTValue));
}


// refactored
void czeit::JD2all(const double &arg_JDatum)
{
	JD = arg_JDatum;
	datum = JD2GD(JD);
	datum.jahr_comma = calc_jahr_comma(datum.jahr, datum.monat, datum.tag);
	JD0 = calc_JD0(JD);

	if (JD < 0.5) // "special case": handle the first Julian Day
	{
		uhrzeit.uhr_comma = 12 + 24*JD; // JD 0.0 is already 12 hour
	}
	else // "normal case"
	{
		uhrzeit.uhr_comma = 24*(JD - JD0);
	}
	int std_min_sek[3];
	double2hour(uhrzeit.uhr_comma, std_min_sek);
	uhrzeit.stunde = std_min_sek[0];
	uhrzeit.minute = std_min_sek[1];
	uhrzeit.sekunde = std_min_sek[2];


	Delta_T = calc_DT(JD);
	TD_zeit.uhr_comma = uhrzeit.uhr_comma+Delta_T/3600;

	TD_dat.jahr = datum.jahr;
	TD_dat.monat = datum.monat;
	TD_dat.tag = datum.tag;
	TD_dat.jahr_comma = calc_jahr_comma(TD_dat.jahr, TD_dat.monat, TD_dat.tag);


	if (TD_zeit.uhr_comma > 24)
	{
		TD_zeit.uhr_comma -= 24;
		TD_dat.tag = datum.tag+1;
	}
	else if (TD_zeit.uhr_comma < 0)
	{
		TD_zeit.uhr_comma += 24;
		TD_dat.tag = datum.tag-1;
	}

	double2hour(TD_zeit.uhr_comma, std_min_sek);
	TD_zeit.stunde = std_min_sek[0];
	TD_zeit.minute = std_min_sek[1];
	TD_zeit.sekunde = std_min_sek[2];

}


// ================= public methods ================================


// refactored
void czeit::set_JD(const double &arg_JDatum)
{
	double loc_JDatum = arg_JDatum;
	JD2all(loc_JDatum);
	notifyObserver();
}


//refactored
bool czeit::is_schaltjahr(const int &arg_year)
{

	bool ret_isLeapYear= 0;
	bool jahresbed = !(arg_year % 4);
	bool not_jahrhundert_std = arg_year % 100;
	bool jahrhundert_ausnahme = arg_year % 400;

	//cout << endl << "jahr: " << jahr << ", jahresbed: " << jahresbed << " | not_jahrhundert_std: " << not_jahrhundert_std << " | jahrhundert_ausnahme: " << jahrhundert_ausnahme;


	// ----------------------------------------------------------------------------------------------------
	// the definition of a leap year (=Schaltjahr) are taken from
	// MEEUS - ASTRONOMICAL ALGORITHMS, 2nd edition, p. 62
	//
	if (arg_year > 1582) // Rule applies for Gregorian Calendar (after the year 1582)
	{
		if ((jahresbed && not_jahrhundert_std) || !jahrhundert_ausnahme)
		{
			ret_isLeapYear = 1;
		}
	}
	else  // Rule applies for Julian Calendar (before the year 1582)
	{
		if (jahresbed == 1)
		{
			ret_isLeapYear = 1;
		}
	}
	// ----------------------------------------------------------------------------------------------------

	return(ret_isLeapYear);

}


//refactored
unsigned int czeit::get_no_days_that_year(const int &arg_year)
{
	if (is_schaltjahr(arg_year))
	{
		return(C__tage_schaltjahr); //366 days
	}
	else
		return(C__tage_normaljahr); //365 days
}




//refactored
double czeit::centuries_since_J2000(const double & arg_JD)
{
	return((arg_JD - C__JD2000)/tage_pro_jahrhundert);
}


//refactored
double czeit::millennia_since_J2000(const double & arg_JD)
{
	return((arg_JD - C__JD2000)/tage_pro_jahrtausend);
}


//refactored
double czeit::days_since_J2000(const double & arg_JD)
{
	return((arg_JD - C__JD2000));
}


// Unit tests available
//refactored
double czeit::calc_DT(const double &arg_JulDat) {

	// locals
	unsigned int loc_index;
	double ret_DynTime_Delta_T;
	double loc_centuriesSinceJ2000 = (JD - C__JD2000)/36525; //Jahrhunderte (cent.) seit dem Jahr 2000, neg. wenn vor J2000.0
	double loc_yearsSinceJ2000 = loc_centuriesSinceJ2000*100;


	if(datum.jahr < 948)
	{
		ret_DynTime_Delta_T = 2715.5 + 573.36*loc_centuriesSinceJ2000 + 46.5*loc_centuriesSinceJ2000*loc_centuriesSinceJ2000; // Meeus [1] S.73
	}
	else if ((948 <= datum.jahr) and (datum.jahr < 1600))
	{
		ret_DynTime_Delta_T = 50.6 + 67.5*loc_centuriesSinceJ2000 + 22.5*loc_centuriesSinceJ2000*loc_centuriesSinceJ2000; // Meeus [1] S.73
	}
	else if ((1600 <= datum.jahr) and (datum.jahr < 1620))
	{
		// lineare Interpolation zwischen dem letzten gueltigen Approximationswert der Formel auf S. 73  fuer den Zeitraum 948 ... 1600
		// und dem aus Meeus [1] entnommenen ersten Tabellenwert (S.72) fuer 1620
		//cout << datum.jahr;
		double year = datum.jahr;
		ret_DynTime_Delta_T = C__DynTime_1600 - (year - 1600)/(1620-1600)*(C__DynTime_1600 - C__DynTime_1620);
	}
	else if ((1620 <= datum.jahr) and (datum.jahr <= 1992))
	{
		// index determination
		loc_index = calc_DeltaT_LUT_ind(datum.jahr);

		// LUT Evaluation
		if (loc_index >= 0 and loc_index <= C_Max_DynLUT_ind)
			ret_DynTime_Delta_T = DTLUT[loc_index];
		else if (loc_index < 0)
			ret_DynTime_Delta_T = DTLUT[0];
		else
			ret_DynTime_Delta_T = DTLUT[C_Max_DynLUT_ind];

	}
	else if ((1992 < datum.jahr ) and (datum.jahr <= 2005))
	{
	//	Between years 1986 and 2005
	//  taken from http://eclipse.gsfc.nasa.gov/SEcat5/deltatpoly.html am 2012-06.26
		ret_DynTime_Delta_T = 63.86 + 0.3345 * loc_yearsSinceJ2000
				- 0.060374 * loc_yearsSinceJ2000*loc_yearsSinceJ2000
				+ 0.0017275 * loc_yearsSinceJ2000*loc_yearsSinceJ2000*loc_yearsSinceJ2000
				+ 0.000651814 * loc_yearsSinceJ2000*loc_yearsSinceJ2000*loc_yearsSinceJ2000*loc_yearsSinceJ2000
				+ 0.00002373599 * loc_yearsSinceJ2000*loc_yearsSinceJ2000*loc_yearsSinceJ2000*loc_yearsSinceJ2000*loc_yearsSinceJ2000;

		//		where: t = y - 2000
	}
	else if ((2005 < datum.jahr ) and (datum.jahr <= 2050))
	{
	//
	//	Between years 2005 and 2050, calculate:
	//  taken from http://eclipse.gsfc.nasa.gov/SEcat5/deltatpoly.html am 2012-06.26
		ret_DynTime_Delta_T = 62.92 + 0.32217 * loc_yearsSinceJ2000 + 0.005589 * loc_yearsSinceJ2000* loc_yearsSinceJ2000;
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
		double t = (datum.jahr - 1820)/100;
		ret_DynTime_Delta_T = -20 + 32 * t*t;
	}

	return(ret_DynTime_Delta_T );
}



// ================= Operatoren ================================


ostream& operator<<(ostream & os, const czeit & zeitpunkt)
{
	cout << endl;
	cout.precision(10);
	cout << "JD:      " << zeitpunkt.JD0;

	cout << endl << "Datum:   ";
	// führende Null beim Tag
	if (zeitpunkt.datum.tag < 10)
		cout << "0";
	cout << zeitpunkt.datum.tag << ".";

	// führende Null beim Monat
	if (zeitpunkt.datum.monat < 10)
		cout << "0";
	cout << zeitpunkt.datum.monat << "."
	<< zeitpunkt.datum.jahr;

	cout << endl << "Uhrzeit: ";
	//führende Null bei der Stunde
	if (zeitpunkt.uhrzeit.stunde < 10)
		cout << "0";
	cout << zeitpunkt.uhrzeit.stunde << "h";

	//führende Null bei der Minute
	if (zeitpunkt.uhrzeit.minute < 10)
		cout << "0";
	cout << zeitpunkt.uhrzeit.minute << "m";

	//führende Null bei der Sekunde
	if (zeitpunkt.uhrzeit.sekunde < 10)
		cout << "0";
	cout << zeitpunkt.uhrzeit.sekunde << "s";

	// OPTIONALE AUSGABE: Uhrzeit in Kommadarstellung
	//cout << " = ";
	//cout.precision(5);
	//cout << zeitpunkt.uhrzeit.uhr_comma << " Stunden" << endl;
	return os;
}
