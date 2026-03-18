using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 * \struct 	sspher
 * \brief 	Struktur fuer sphaerische Koordinaten, wird z.B. fuer den Return der VSOP-Theorie verwwendet
 */
public struct sspher
{
	public double L;   /**< sphaerische Koordinate, Argument der Laenge: L, Einheit Grad */
	public double B;   /**< sphaerische Koordinate, Argument der Breite: B, Einheit Grad */
	public double R;   /**< sphaerische Koordinate, Radius: R, Einheit Astronomische Einheit (AU) */
};
// ------------------------------------------------------------------------------------------------------

/**
 * \struct 	skart
 * \brief 	Struktur fuer kartesische Koordinaten
 */
public struct skart
{
	public double x;       /**< kartesische Koordinate: x, Einheit i.d.R. AE */
	public double y;       /**< kartesische Koordinate: y, Einheit i.d.R. AE */
	public double z;       /**< kartesische Koordinate: z, Einheit i.d.R. AE */
};
// ------------------------------------------------------------------------------------------------------

/**
 * \struct 	GA_coord
 * \brief 	repraesentiert ein Koordinatenelement mit den sphaerischen Anteilen Abstand r, den Winkeln Laenge in Stunde, Minute, Sekunde und Breite in
 * Grad, Bogenminute, Bogensekunde sowie den kartesischen Koordinaten x, y, z
 */
public struct GA_coord
{
	// --------------------------------------------------
	// SPHAERISCHE KOORDINATEN
	/** Argument der Laenge  */
	public double hms;     /**< Kommadarstellung */
	public int lh;         /**< Stunde */
	public int lm;         /**< Minute */
	public double ls;      /**< Sekunde */

	public double hms_grad; /**< Argument der Laenge in Grad: 15*hms */

	// Argument der Breite
	public double gms;     /**< Kommadarstellung */
	public int bg;         /**< Grad */
	public int bbm;        /**< Bogenminute */
	public double bbs;     /**< Bogensekunde */

	public double r;       /**< Abstand */

	// --------------------------------------------------
	// KARTHESISCHE KOORDINATEN
	public double x;       /**< kartesische Koordinate: x, Einheit i.d.R. AE */
	public double y;       /**< kartesische Koordinate: y, Einheit i.d.R. AE */
	public double z;       /**< kartesische Koordinate: z, Einheit i.d.R. AE */
	// --------------------------------------------------

};
// ------------------------------------------------------------------------------------------------------

public struct GE_coord
{
	// --------------------------------------------------
	// SPHAERISCHE KOORDINATEN
	/** Argument der Laenge  */
	public double hms;     /**< Kommadarstellung */
	public int lh;         /**< Stunde */
	public int lm;         /**< Minute */
	public double ls;      /**< Sekunde */

	public double hms_grad; /**< Argument der Laenge in Grad: 15*hms */

	// Argument der Breite
	public double gms;     /**< Kommadarstellung */
	public int bg;         /**< Grad */
	public int bbm;        /**< Bogenminute */
	public double bbs;     /**< Bogensekunde */

	public double r;       /**< Abstand */

	// --------------------------------------------------
	// KARTHESISCHE KOORDINATEN
	public double x;       /**< kartesische Koordinate: x, Einheit i.d.R. AE */
	public double y;       /**< kartesische Koordinate: y, Einheit i.d.R. AE */
	public double z;       /**< kartesische Koordinate: z, Einheit i.d.R. AE */
	// --------------------------------------------------
};
// ------------------------------------------------------------------------------------------------------

public struct HE_coord
{
	// --------------------------------------------------
	// SPHAERISCHE KOORDINATEN
	/** Argument der Laenge  */
	public double hms;     /**< Kommadarstellung */
	public int lh;         /**< Stunde */
	public int lm;         /**< Minute */
	public double ls;      /**< Sekunde */

	public double hms_grad; /**< Argument der Laenge in Grad: 15*hms */

	// Argument der Breite
	public double gms;     /**< Kommadarstellung */
	public int bg;         /**< Grad */
	public int bbm;        /**< Bogenminute */
	public double bbs;     /**< Bogensekunde */

	public double r;       /**< Abstand */

	// --------------------------------------------------
	// KARTHESISCHE KOORDINATEN
	public double x;       /**< kartesische Koordinate: x, Einheit i.d.R. AE */
	public double y;       /**< kartesische Koordinate: y, Einheit i.d.R. AE */
	public double z;       /**< kartesische Koordinate: z, Einheit i.d.R. AE */
	// --------------------------------------------------

};
// ------------------------------------------------------------------------------------------------------

/**
 *\struct szeit
 *\brief enthaelt die Datenelemente fuer die Uhrzeit: Stunde, Minute, Sekunde je als Ganzzahlen, Uhrzeit in Kommadarstellung
 */
/**
 *\struct sdatum
 *\brief enthaelt die Datenelemente fuer das Datum: Tag, Monat, Jahr als Ganzzahlen
 */
public struct sdatum
{
	public int day;            /**< Tag (Ganzzahl) */
	public int month;          /**< Monat (Ganzzahl) */
	public int year;           /**< Jahr (Ganzzahl) */
	public double yearComma;  /**< jahr in float */
};
public struct szeit
{
	public int hour;                /**< hour (int)*/
	public int minute;              /**< minute (int) */
	public int second;              /**< second (int) */
	public double timeComma;       /**< time_comma (double)*/
};
// ------------------------------------------------------------------------------------------------------


public class conversions
{
	public conversions() { }


	public double deg2hour(double arg_deg)
	{
		// limit arg_deg to [0...360
		double ret_hours = arg_deg / astroConst.C__deg_per_hour;

		return (ret_hours);
	}
	public double hour2deg(double arg_hourdeg)
	{
		//limit arg_hourdeg to [0..24]
		double ret_deg = arg_hourdeg * astroConst.C__deg_per_hour;

		return (ret_deg);
	}

	public double hms2double(int arg_std, int arg_hmin, int arg_hsec)
	{

		double ret_hourInDouble = 0.0;

		double loc_minSecInSec = 0.0;
		double loc_secFractionOfHour = 0.0; // describes the portion of an hour of a number of seconds


		// fängt den Fall Sonderfall -00:XX:YY ab:
		// in diesem Fall kann mit grad nicht das VZ übergeben werden, weil arg_std = 0 ist.
		// Daher wird arg_min als Unterscheider verwendet

		if (arg_hmin >= 0)
			loc_minSecInSec = arg_hmin * astroConst.C__sec_per_min + arg_hsec; //convert min:ss -> ss
		else
			loc_minSecInSec = arg_hmin * astroConst.C__sec_per_min - arg_hsec; //convert min:ss -> ss

		loc_secFractionOfHour = (double)(loc_minSecInSec / astroConst.C__sec_per_hour);

		if (arg_std >= 0)
			ret_hourInDouble = (double)(arg_std + loc_secFractionOfHour);
		else
			ret_hourInDouble = (double)(arg_std - loc_secFractionOfHour);

		return (ret_hourInDouble);
	}
	public static int[] double2hour(double arg_hdoub)
	{
		double loc_decimalPlace;

		if (arg_hdoub < 0.0)
			// loc_decimalPlace = arg_hdoub - static_cast<double>(ceil(arg_hdoub));
			loc_decimalPlace = arg_hdoub - (double)(Math.Ceiling(arg_hdoub));
		else
			// loc_decimalPlace = arg_hdoub - static_cast<double>(floor(arg_hdoub));
			loc_decimalPlace = arg_hdoub - (double)(Math.Floor(arg_hdoub));

		if (loc_decimalPlace < 0.0)
			loc_decimalPlace = -loc_decimalPlace;
		
		int ret_hms0 = 0;

		if (arg_hdoub < 0.0)
			// ret_hms[0] = static_cast<int>(arg_hdoub + loc_decimalPlace);
			ret_hms0 = (int)(arg_hdoub + loc_decimalPlace);
		else
			// ret_hms[0] = static_cast<int>(arg_hdoub - loc_decimalPlace);
			ret_hms0 = (int)(arg_hdoub - loc_decimalPlace);

		//ret_hms[1] = static_cast<int>((loc_decimalPlace * 3600) / 60);
		int ret_hms1 = (int)((loc_decimalPlace * 3600) / 60);

		//ret_hms[2] = (static_cast<int>(loc_decimalPlace * 3600)) % 60;
		int ret_hms2 = (int)(loc_decimalPlace * 3600) % 60;

		int[] retArray = { ret_hms0, ret_hms1, ret_hms2 };

		return retArray;
	}


	/**
		* \brief converts floatin point time to szeit
		* @param arg_zeitDouble time in floating point
		* @return time in structure szeit
		*/
	public static szeit double2szeit(double arg_zeitDouble)
	{
		szeit ret_szeit;

		// limit to [0...24]
		double loc_zeitDouble = ohne_ueberlauf_hour(arg_zeitDouble);

		int[] std_min_sek = { 0, 0, 0 };
		std_min_sek = double2hour(loc_zeitDouble);

		ret_szeit.timeComma = loc_zeitDouble;
		ret_szeit.hour = std_min_sek[0];
		ret_szeit.minute = std_min_sek[1];
		ret_szeit.second = std_min_sek[2];

		return ( ret_szeit );
	}



	public static void double2hour(double arg_hdoub, int [] ret_hms)
	{
		double loc_decimalPlace;

		if (arg_hdoub< 0.0)
			loc_decimalPlace = arg_hdoub - (double)(Math.Ceiling(arg_hdoub));
		else
			loc_decimalPlace = arg_hdoub - (double)(Math.Floor(arg_hdoub));

		if (loc_decimalPlace< 0.0)
			loc_decimalPlace = - loc_decimalPlace;

		if (arg_hdoub< 0.0)
			ret_hms[0] = (int)(arg_hdoub + loc_decimalPlace);
		else
			ret_hms[0] = (int)(arg_hdoub - loc_decimalPlace);

		ret_hms[1] = (int)((loc_decimalPlace*3600)/ 60);
		ret_hms[2] = ((int)(loc_decimalPlace*3600))% 60;
	}
/**
	* \brief method removing overflow in hours ( >+24 hours and <0 hours)
	* @param grad argument in hours which can range in- or outside the range [0...+24]
	* @return hours value limited to the range [0...+24]
	*/
public static double ohne_ueberlauf_hour(double arg_deg)
	{
		/**
		* \brief method removing overflow in hours ( >+24 hours and <0 hours)
		*/

		double ret = arg_deg;
		if (ret > 0.0)
		{
			while (ret > 24.0) ret -= 24.0;
		}
		else
		{
			while (ret < 0.0) ret += 24.0;
		}
		return (ret);

	}
	// -------------------------------------------------------------------------------------------------------------------

	/**
		* \brief method fitting degrees value in range [-180...180]
		* @param arg_deg argument in degrees which can range in- or outside the range [-180...+180]
		* @return ret_deg value limited to the range [-180...+180]
		*/
	public static double minusPlus180(double arg_deg)
	{
		double ret = arg_deg;

		if (ret > 180.0)
		{
			while (ret > 180.0) ret -= 180.0;
		}
		else
		{
			while (ret < -180.0) ret += 180.0;
		}
		return (ret);
	}

	/**
		* \brief method fitting hour value in range [-12...12]
		* @param arg_hour argument in hours which can range in- or outside the range [-12...+12]
		* @return ret_hour value limited to the range [-12...+12]
		*/
	public static double minusPlus12(double arg_hour)
	{
		double ret = arg_hour;

		if (ret > 12.0)
		{
			while (ret > 12.0) ret -= 12.0;
		}
		else
		{
			while (ret < -12.0) ret += 12.0;
		}
		return (ret);
	}


	/**
		* \brief method removing overflow in degrees ( >+360° and <0°)
		* @param grad argument in degrees which can range in- or outside the range [0...+360]
		* @return degree value limited to the range [0...+360]
		*/
	public static double ohne_ueberlauf_degrees(double arg_grad)
	{
		/**
		 *\brief method removing overflow in degrees ( >+360° and <0°)
		*/
		double ret = arg_grad;
		if (ret < 0.0)
		{
			while (ret < 0.0) ret += 360.0;
		}
		else
		{
			while (ret > 360.0) ret -= 360.0;
		}

		return (ret);
	}
	// -------------------------------------------------------------------------------------------------------------------


	/**
		* \brief method removing overflow in declinations ( >+90° and <-90°)
		* @param grad argument in degrees which can range in- or outside the range [-90...+90]
		* @return degree value limited to the range [-90...+90]
		*/
	public static double ohne_ueberlauf_declination(double arg_grad)
	{
		/**
		 * \brief method removing overflow in declinations ( >+90° and <-90°)
		 */
		double ret = arg_grad;
		if (ret < -90.0)
		{
			while (ret < -90.0) ret += 90.0;
		}
		else
		{
			while (ret > 90.0) ret -= 90.0;
		}

		return (ret);
	}
	// -------------------------------------------------------------------------------------------------------------------

	/**
		* \brief method saturating a double argument to a min and max range
		* @param arg_value value to be saturated
		* @param arg_min lower saturation border
		* @param arg_min upper saturation border
		* @return saturated arg_value to [arg_min...arg_max]
		*/
	public static double saturate(double arg_value, double arg_min, double arg_max)
	{

		double ret = 0.0;

		//limit to [arg_min...arg_max]
		if (ret < -1)
        {
            ret = -1;
        }
        else if (ret > 1)
        {
            ret = 1;
        }

        return (ret);

    }


	/**
		* \brief Konvertiert das in der Form °, ', '' uebergebene Argument in Breite nach Gleitkomma
		* @param grad Argument der Breite, Wert fuer Grad
		* @param min Argument der Breite, Wert fuer Bogenminute
		* @param sec Argument der Breite, Wert fuer Bogensekunde
		* @return Argument der Breite in Gleitkomma
		*/
	public double breite2double( int grad, int min, double sec )
	{

		double loc_hilfe = 0.0;
		double ret_breite = 0.0;

		// fängt den Fall Sonderfall -00:XX:YY ab:
		// in diesem Fall kann mit grad nicht das VZ übergeben werden, weil grad = 0 ist.
		// Daher wird min als Unterscheider verwendet
		if (min >= 0)
			loc_hilfe = min * astroConst.C__sec_per_min + sec;
		else
			loc_hilfe = min * astroConst.C__sec_per_min - sec;
		//loc_hilfe = static_cast<double>(loc_hilfe / C__sec_per_hour);
		loc_hilfe = (double)(loc_hilfe / astroConst.C__sec_per_hour);

		// hier wird der Standard-Fall größer oder kleiner 0 abgefangen, wenn nämlich
		// der Zahlenwert von grad bereits das VZ enthalten kann
		if (grad >= 0)
			// ret_breite = static_cast<double>(grad + loc_hilfe);
			ret_breite = (double) (grad + loc_hilfe);
		else
			//ret_breite = static_cast<double>(grad - loc_hilfe);
			ret_breite = (double) (grad - loc_hilfe);

		return (ret_breite);
	}
	// --------------------------------------------------------------------------------------------------



	/**
		* \brief setzt den ganzzahligen Gradanteil eines als Gleitkomma uebergebenen Arguments in Breite
		* @param ddoub Argument der Breite in Gleitkomma
		* @return ganzzahliger Gradanteil
		*/
	public static double double2grad(double ddoub)
	{
		double nachkomma;
		if (ddoub < 0)
			nachkomma = ddoub - (double)(Math.Ceiling(ddoub));
		else
			nachkomma = ddoub - (double)(Math.Floor(ddoub));

		if (nachkomma < 0)
			nachkomma = -nachkomma;

		if (ddoub < 0)
			return (ddoub + nachkomma);
		else
			return (ddoub - nachkomma);

	}
	// --------------------------------------------------------------------------------------------------


	/**
		* \brief setzt den ganzzahligen Bogenminutenanteil eines als Gleitkomma uebergebenen Arguments in Breite
		* @param ddoub Argument der Breite in Gleitkomma
		* @return ganzzahliger Bogenminutenanteil
		*/
	public static double double2bogenmin(double ddoub)
	{
		int y;

		double nachkomma;
		if (ddoub < 0)
			nachkomma = ddoub - (double)(Math.Ceiling(ddoub));
		else
			nachkomma = ddoub - (double)(Math.Floor(ddoub));

		if (nachkomma < 0)
			nachkomma = -nachkomma;

		// y = static_cast<int>(nachkomma * 3600 / 60);
		y = (int)(nachkomma * 3600 / 60);

		return (y);
	}

	// --------------------------------------------------------------------------------------------------


	/**
		* \brief setzt den ganzzahligen Bogensekundennanteil eines als Gleitkomma uebergebenen Arguments in Breite
		* @param ddoub Argument der Breite in Gleitkomma
		* @return ganzzahliger Bogensekundenanteil
		*/
	public static double double2bogensek(double ddoub)
	{
		int y;

		double nachkomma;
		if (ddoub < 0)
			nachkomma = ddoub - (double)( Math.Ceiling(ddoub));
		else
			nachkomma = ddoub - (double)( Math.Floor(ddoub));

		if (nachkomma < 0)
			nachkomma = -nachkomma;

		y = ((int)(nachkomma * 3600)) % 60;
		return (y);
	}
	// --------------------------------------------------------------------------------------------------


	/**
		* \brief setzt den ganzzahligen Stundenanteil eines als Gleitkomma uebergebenen Arguments in Laenge
		* @param hdoub Argument der Laenge in Gleitkomma
		* @return ganzzahliger Stundenanteil
		*/
	public static int double2stunde(double hdoub)
	{
		double nachkomma;

		if (hdoub < 0)
			nachkomma = hdoub - (double)( Math.Ceiling(hdoub));
		else
			nachkomma = hdoub - (double)( Math.Floor(hdoub));

		if (nachkomma < 0)
			nachkomma = -nachkomma;

		if (hdoub < 0)
			return ((int)(hdoub + nachkomma));
		else
			return ((int)(hdoub - nachkomma));
	}

	// --------------------------------------------------------------------------------------------------


	/**
		* \brief setzt den ganzzahligen Minutenanteil eines als Gleitkomma uebergebenen Arguments in Laenge
		* @param hdoub Argument der Laenge in Gleitkomma
		* @return ganzzahliger Minutenanteil
		*/
	public static int double2minute(double hdoub)
	{
		int x;

		double nachkomma;

		if (hdoub < 0)
			nachkomma = hdoub - (double)( Math.Ceiling(hdoub));
		else
			nachkomma = hdoub - (double)(Math.Floor(hdoub));

		if (nachkomma < 0)
			nachkomma = -nachkomma;

		//x = static_cast<int>((nachkomma * 3600) / 60);
		x = (int) ((nachkomma * 3600) / 60);
		return (x);
	}
	// --------------------------------------------------------------------------------------------------


	/**
		* \brief setzt den ganzzahligen Sekundenanteil eines als Gleitkomma uebergebenen Arguments in Laenge
		* @param hdoub Argument der Laenge in Gleitkomma
		* @return ganzzahliger Sekundenanteil
		*/
	public static double double2sekunde(double hdoub)
	{
		double x;

		double nachkomma;

		if (hdoub < 0)
			nachkomma = hdoub - (double)( Math.Ceiling(hdoub));
		else
			nachkomma = hdoub - (double)( Math.Floor(hdoub));

		if (nachkomma < 0)
			nachkomma = -nachkomma;

		//x = (static_cast<int>(nachkomma * 3600)) % 60;
		x = ((nachkomma * 3600)) % 60;
		return (x);
	}
	// --------------------------------------------------------------------------------------------------
	// -----------------

	/**
		* \brief rechnet aus den gegebenen sphaerischen Koordinaten die kartesischen Koordinaten
		* @param arg_spher Laenge, Breite und Radius der sphaerischen Koordinaten: L, B, R
		* @return Kartesische Koordinaten; x, y, z
		*/
	public static skart spher2kart( sspher arg_spher )
	{
		skart ret_skart;
		double loc_R = arg_spher.R;
		double loc_B = arg_spher.B;
		double loc_L = arg_spher.L;

		ret_skart.x = loc_R * Math.Cos(Math.PI / 180 * loc_B) * Math.Cos(Math.PI / 180 * loc_L);
		ret_skart.y = loc_R * Math.Cos(Math.PI / 180 * loc_B) * Math.Sin(Math.PI / 180 * loc_L);
		ret_skart.z = loc_R * Math.Sin(Math.PI / 180 * loc_B);

		return (ret_skart);
	}

	public static HE_coord spherical2HE(sspher arg_spherical4Update)
	{
		HE_coord ret_HE;
		// return of apparent position is supposed to update the complete planet object
		ret_HE.hms_grad = arg_spherical4Update.L;
		ret_HE.gms = arg_spherical4Update.B;
		ret_HE.r = arg_spherical4Update.R;

		ret_HE.hms = ret_HE.hms_grad / 15;
		ret_HE.lh = double2stunde(ret_HE.hms);
		ret_HE.lm = double2minute(ret_HE.hms);
		ret_HE.ls = double2sekunde(ret_HE.hms);

		ret_HE.bg = (int) double2grad(ret_HE.gms);
		ret_HE.bbm = (int) double2bogenmin(ret_HE.gms);
		ret_HE.bbs = (int) double2bogensek(ret_HE.gms);

		skart loc_kart = spher2kart(arg_spherical4Update);
		ret_HE.x = loc_kart.x;
		ret_HE.y = loc_kart.y;
		ret_HE.z = loc_kart.z;

		return (ret_HE);
	}
	
	public static GE_coord spherical2GE(sspher arg_spherical4Update)
	{
		GE_coord ret_GE;
		// return of apparent position is supposed to update the complete planet object
		ret_GE.hms_grad = arg_spherical4Update.L;
		ret_GE.gms = arg_spherical4Update.B;
		ret_GE.r = arg_spherical4Update.R;

		ret_GE.hms = ret_GE.hms_grad / 15;
		ret_GE.lh = double2stunde(ret_GE.hms);
		ret_GE.lm = double2minute(ret_GE.hms);
		ret_GE.ls = double2sekunde(ret_GE.hms);

		ret_GE.bg = (int) double2grad(ret_GE.gms);
		ret_GE.bbm = (int) double2bogenmin(ret_GE.gms);
		ret_GE.bbs = (int) double2bogensek(ret_GE.gms);

		skart loc_kart = spher2kart(arg_spherical4Update);
		ret_GE.x = loc_kart.x;
		ret_GE.y = loc_kart.y;
		ret_GE.z = loc_kart.z;

		return (ret_GE);
	}

	public GA_coord double2GAcoord(double arg_ra_komma, double arg_dek_komma, double arg_distance)
	{
		GA_coord ret_scoord;
		ret_scoord.hms_grad = arg_ra_komma;
		ret_scoord.hms = arg_ra_komma / 15;
		ret_scoord.lh = (int) double2stunde(arg_ra_komma / 15);
		ret_scoord.lm = (int) double2minute(arg_ra_komma / 15);
		ret_scoord.ls = double2sekunde(arg_ra_komma / 15);

		ret_scoord.gms = arg_dek_komma;
		ret_scoord.bg = (int) double2grad(arg_dek_komma);
		ret_scoord.bbm = (int) double2bogenmin(arg_dek_komma);
		ret_scoord.bbs =  double2bogensek(arg_dek_komma);

	

		// Oliver Montenbruck, Grunlagen der Ephemeridenrechnung, 6. Auflage, S. 2
		// x = r*cos(beta)*cos(lambda)
		// y = r*cos(beta)*sin(lambda)
		// z = r*sin(beta)
		// beta: argument of latitude
		// lambda: argument of longitude
		ret_scoord.r = arg_distance;
		ret_scoord.x = arg_distance * Math.Cos(Math.PI / 180 * arg_dek_komma) * Math.Cos(Math.PI / 180 * arg_ra_komma);
		ret_scoord.y = arg_distance * Math.Cos(Math.PI / 180 * arg_dek_komma) * Math.Sin(Math.PI / 180 * arg_ra_komma);
		ret_scoord.z = arg_distance * Math.Sin(Math.PI / 180 * arg_dek_komma);

		return (ret_scoord);
	}

	public GE_coord double2GEcoord(double arg_ra_komma, double arg_dek_komma, double arg_distance)
	{
		GE_coord ret_scoord;
		ret_scoord.hms_grad = arg_ra_komma;
		ret_scoord.hms = arg_ra_komma/15;
		ret_scoord.lh = (int) double2stunde(arg_ra_komma/15);
		ret_scoord.lm = (int) double2minute(arg_ra_komma/15);
		ret_scoord.ls = double2sekunde(arg_ra_komma/15);

		ret_scoord.gms = arg_dek_komma;
		ret_scoord.bg = (int) double2grad(arg_dek_komma);
		ret_scoord.bbm = (int) double2bogenmin(arg_dek_komma);
		ret_scoord.bbs = double2bogensek(arg_dek_komma);

		// Oliver Montenbruck, Grunlagen der Ephemeridenrechnung, 6. Auflage, S. 2
		// x = r*cos(beta)*cos(lambda)
		// y = r*cos(beta)*sin(lambda)
		// z = r*sin(beta)
		// beta: argument of latitude
		// lambda: argument of longitude
		ret_scoord.r = arg_distance;
		ret_scoord.x = arg_distance * Math.Cos(Math.PI / 180 * arg_dek_komma) * Math.Cos(Math.PI / 180 * arg_ra_komma);
		ret_scoord.y = arg_distance * Math.Cos(Math.PI / 180 * arg_dek_komma) * Math.Sin(Math.PI / 180 * arg_ra_komma);
		ret_scoord.z = arg_distance * Math.Sin(Math.PI / 180 * arg_dek_komma);

		return ret_scoord;
	}


}

