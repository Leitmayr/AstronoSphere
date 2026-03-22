using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 * \struct sBE
 * \brief enthaelt die Elemente Inklination, Laenge des aufsteigenden Knotens, und Argument des Perihels, jeweils in deg und in Kommadarstellung.
 */
struct sBE
{
	/**
	 * Bahnelemente wurden gewaehlt gemaess dem Astronomical Almanac
	 */
	double inclination;                 /**< Inklination in deg */
	double lengthAscNode;               /**< Laenge des aufsteigenden Knotens in deg */
	double argPerihel;                  /**< Argument Perihel in deg */
};

/**
 * \class 	cpraezession
 * \brief 	Abstrakte Klasse. Transformiert Koordinaten (GA, GE, BE) von einem in das andere Aequinoktium
 * 			Basisklassen: -
 *
 * Transformiert Koordinaten (GA, GE, BE) von einem in das andere Aequinoktium. Rueckgabewert ist fuer GA und GE eine Struktur vom Typ
 * scoord,  die Rektaszension und Deklination in Kommaform enthaelt bzw. fuer BE ist es die Struktur sBE mit den Elementen Inklination,
 * Laenge des aufsteigenden Knotens, und Argument des Perihels, jeweils in deg und in Kommadarstellung.
*/

public abstract class cpraezession
{
	/**
 * @param 	arg_startEclLong: Start equinox, ecliptical longitude in degrees as float number
 * @param 	arg_startEclLat: Start equinox, ecliptical latitude in degrees as float number
 * @param 	arg_T_start: Start equinox
 * @param 	arg_T_end: Target equinox
 * @return 	ret_resTargetEquinox[2]: [0] is ecliptical longitude [1] is latitude in ecliptical coordinates, in deg and as a float number
 */
	public static double[] calc_ekl_praez(double arg_startEclLong, double arg_startEclLat, double arg_T_start, double arg_T_end)
	{

		double loc_eclipticLatitude_komma = 0.0;
		double loc_eclipticLongitude_komma = 0.0;
		double A, B, C;



		// ===============================================================
		// Preparations: calculate the time quantities
		double T0 = czeit.millenniaSinceJ2000(arg_T_start);
		double T = (arg_T_end - arg_T_start) / astroConst.tage_pro_jahrhundert;



		// ===============================================================
		// Some more intermediate calculations: required support quantities
		// formulae taken from MEEUS - Astronomical Algorithms, 2nd edition, eqn. (21.5), p. 136
		// Meeus calls this Math.PI
		double angleEclipticVernalEquinox = (174.876384 + 3289.4789 / astroConst.C__sec_per_hour * T0 + 0.60622 / astroConst.C__sec_per_hour * T0 * T0)
											- (869.8089 / astroConst.C__sec_per_hour + 0.50491 / astroConst.C__sec_per_hour * T0) * T
											+ 0.03536 / astroConst.C__sec_per_hour * T * T;
		// Meeus calls this eta
		double eclipticChange = (
											(47.0029 - 0.06603 * T0 + 0.000598 * T0 * T0) * T
											+ (-0.03302 + 0.000598 * T0 * T0) * T * T
											+ 0.000060 * T0 * T0 * T0
											)
											/ astroConst.C__sec_per_hour;
		//Meus calls this p
		double commonPrecessionInLength = (
											(5029.0966 + 2.22226 * T0 - 0.000042 * T0 * T0) * T
											+ (1.11113 - 0.000042 * T0) * T * T
											- 0.000006 * T * T * T
											)
											/ astroConst.C__sec_per_hour;



		// ===============================================================
		// even more intermediate calculations: some additional support quantities
		// A = Math.Cos(eta)*Math.Cos(beta0)*Math.Sin(Math.PI-lambda0) - Math.Sin(eta)*Math.Sin(beta0)
		A = Math.Cos(Math.PI / 180 * eclipticChange) * Math.Cos(Math.PI / 180 * arg_startEclLat) * Math.Sin(Math.PI / 180 * (angleEclipticVernalEquinox - arg_startEclLong))
				- Math.Sin(Math.PI / 180 * eclipticChange) * Math.Sin(Math.PI / 180 * arg_startEclLat);

		// B = Math.Cos(beta0)*Math.Cos(Math.PI-lambda0)
		B = Math.Cos(Math.PI / 180 * arg_startEclLat) * Math.Cos(Math.PI / 180 * (angleEclipticVernalEquinox - arg_startEclLong));

		// C = Math.Cos(eta)*Math.Sin(beta0) + Math.Sin(eta)*Math.Cos(beta0)*Math.Sin(Math.PI-lambda0)
		C = Math.Cos(Math.PI / 180 * eclipticChange) * Math.Sin(Math.PI / 180 * arg_startEclLat)
			+ Math.Sin(Math.PI / 180 * eclipticChange) * Math.Cos(Math.PI / 180 * arg_startEclLat) * Math.Sin(Math.PI / 180 * (angleEclipticVernalEquinox - arg_startEclLong));

		// now start reduction
		// ============================================================================================
		//Rigorous formulae, Meeus (21.7), p. 137
		// ---------------------------------------------------------------------------
		//1. Ecliptical longitude
		// Step 1a: calculate arcus tangens
		loc_eclipticLongitude_komma = 180 / Math.PI * Math.Atan2(A, B);
		// Step 1b: correct argument
		loc_eclipticLongitude_komma = commonPrecessionInLength + angleEclipticVernalEquinox - loc_eclipticLongitude_komma;

		// handle overflow to [0...360]
		loc_eclipticLongitude_komma = conversions.ohne_ueberlauf_degrees(loc_eclipticLongitude_komma);


		//2. Ecliptical latitude
		// Step 2a yields directly the result
		loc_eclipticLatitude_komma = 180 / Math.PI * Math.Asin(C);

		// handle overflow to [-90...+90]
		loc_eclipticLatitude_komma = conversions.ohne_ueberlauf_declination(loc_eclipticLatitude_komma);

		// ---------------------------------------------------------------------------

		
		double[] retArray = { loc_eclipticLongitude_komma, loc_eclipticLatitude_komma };

		// prepare return values
		return retArray;

	}
	// ----------------------------------------------------------------------------------------------------------------------------

	/**
	 * @param 	arg_startRA: right ascension at start equinox in float and deg
	 * @param   arg_startDek: declination at start equinox in float and deg
	 * @param 	arg_T_start: Julian date of the start equinox
	 * @param 	arg_T_end: Julian date of the target equinox
	 * @return 	contains GA_coord type with coordinates of the target equinox
	 */
	public static double[] calc_aeq_praez(double arg_startRA, double arg_startDek, double arg_T_start, double arg_T_end)
	{
		double loc_ra_komma;
		double loc_dek_komma;

		// ===============================================================
		// Preparations: calculate the time quantities
		double T0 = czeit.centuriesSinceJ2000(arg_T_start);
		double T = (arg_T_end - arg_T_start) / astroConst.tage_pro_jahrhundert;


		// ===============================================================
		// Some more intermediate calculations: required support quantities
		// formulae taken from MEEUS - Astronomical Algorithms, 2nd edition, eqn. (21.2), p. 134
		double zeta = (
						(2306.2181 + 1.39656 * T0 + 0.000139 * T0 * T0) * T
						+ (0.30188 + 0.000344 * T0) * T * T
						+ 0.017998 * T * T * T
						)
						/ astroConst.C__sec_per_hour;

		double z = (
						(2306.2181 + 1.39656 * T0 + 0.000139 * T0 * T0) * T
						+ (1.09468 + 0.000066 * T0) * T * T
						+ 0.018203 * T * T * T
						)
						/ astroConst.C__sec_per_hour;


		double theta = (
						(2004.3109 - 0.85390 * T0 - 0.000217 * T0 * T0) * T
						- (0.42665 + 0.000217 * T0) * T * T
						- 0.041833 * T * T * T
						)
						/ astroConst.C__sec_per_hour;



		// ===============================================================
		// even more intermediate calculations: some additional support quantities
		double A, B, C;

		// formulae Meeus, (21.3), p. 134
		// A = Math.Cos(delta0)*Math.Math.Sin(alpha0+zeta)
		A = Math.Cos(Math.PI / 180 * arg_startDek) * Math.Sin(Math.PI / 180 * (arg_startRA + zeta));

		// B = Math.Cos(theta)*Math.Cos(delta0)*Math.Cos(alpha0 + zeta) - Math.Sin(theta)*Math.Sin(delta0)
		B = Math.Cos(Math.PI / 180 * theta) * Math.Cos(Math.PI / 180 * arg_startDek) * Math.Cos(Math.PI / 180 * (arg_startRA + zeta))
				- Math.Sin(Math.PI / 180 * theta) * Math.Sin(Math.PI / 180 * arg_startDek);

		// C = Math.Sin(theta)*Math.Cos(delta0)*Math.Cos(alpha0+zeta) + Math.Cos(theta)*Math.Sin(delta0)
		C = Math.Sin(Math.PI / 180 * theta) * Math.Cos(Math.PI / 180 * arg_startDek) * Math.Cos(Math.PI / 180 * (arg_startRA + zeta))
				+ Math.Cos(Math.PI / 180 * theta) * Math.Sin(Math.PI / 180 * arg_startDek);


		// now start reduction
		// ============================================================================================
		//Rigorous formulae, Meeus (21.4), p. 134
		// ---------------------------------------------------------------------------
		//1. Right Ascension
		// Step 1a: calculate arcus tangens
		loc_ra_komma = 180 / Math.PI * Math.Atan2(A, B); // Attention because of ambiguity of atan -> might be cause of a failure!
											   // Step 1b: correct argument
		loc_ra_komma = loc_ra_komma + z;

		// handle overflow to [0...360]
		loc_ra_komma = conversions.ohne_ueberlauf_degrees(loc_ra_komma);
		// ---------------------------------------------------------------------------

		// ---------------------------------------------------------------------------
		//2. Declination
		// Step 2a yields directly the result
		loc_dek_komma = 180 / Math.PI * Math.Asin(C);

		// handle overflow to [-90...+90]
		loc_dek_komma = conversions.ohne_ueberlauf_declination(loc_dek_komma);
		// ---------------------------------------------------------------------------

		// ============================================================================================


		// ============================================================================================
		// ALTERNATIVELY
		// Formulae taken from Montenbruck,Grundlagen der Ephemeridenrechnung, 6. ed, p. 24
		// ---------------------------------------------------------------------------
		//	loc_dek_komma = 180/pi*asin(sin(pi/180*theta)*cos(pi/180*arg_start_equinox.gms)*cos(pi/180*(arg_start_equinox.hms_grad+zeta))
		//								+ cos(pi/180*theta)*sin(pi/180*arg_start_equinox.gms));
		//
		//	loc_ra_komma = ret_end_equinox.hms_grad = z + 180/pi*asin(cos(pi/180*arg_start_equinox.gms)
		//								*sin(pi/180*(arg_start_equinox.hms_grad+zeta))/cos(pi/180*ret_end_equinox.gms));
		// ============================================================================================



		double[] retArray = { loc_ra_komma, loc_dek_komma };

		// prepare return values
		return retArray;
	}
	// ----------------------------------------------------------------------------------------------------------------------------



}
