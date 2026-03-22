using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ccoord : conversions
	{

	/**
		* \class ccoord
		* \brief Klassendefinition fuer die Klasse ccoord
		*  	- kapselt die Eigenschaften eines Orts (sphaerische und kartesische Koordinaten, dreidimensional)
		*
		* Klasse fuer die Repraesentation von Koordinaten. Enthaelt die Struktur scoord (sphaerische Koordinaten in Breite, Laenge, Abstand sowie
		* die kartesischen Koordinaten)
	*/

	// ========================= ATTRIBUTES ========================================
	public GA_coord _GAcoord;
	public GE_coord _GEcoord;
	public HE_coord _HEcoord;
	public HE_coord _HERefCoord;


	// ========================= GET FUNCTIONS ========================================

	public HE_coord get_HE_coord 
	{
		get { return _HEcoord; }
	}

	public GE_coord get_GE_coord 
	{
		get { return _GEcoord; }
	}

	public GA_coord get_GA_coord
	{
		get { return _GAcoord; }
	}

	public double get_GAL
	{
		get { return _GAcoord.hms; }
	}

	public double get_GAB
	{
		get { return _GAcoord.gms; }
	}

	// ========================= CONSTRUCTORS ========================================
	public ccoord(HE_coord arg_HE, HE_coord arg_HERef, double arg_JDStart)
	{
		_HEcoord = arg_HE;
		_HERefCoord = arg_HERef;

		// hier muss  zwischenzeitlich noch der Effekt der Lichtlaufzeit berücksichtigt werden

		_GEcoord = HE2GE(_HEcoord, _HERefCoord);
		_GAcoord = GE2GA(_GEcoord, arg_JDStart);
	}

	public ccoord() { }


	// ========================= METHODS ========================================

	public static sspher dynEqn2FK5( sspher arg_dynLBR, double arg_JDVsopData )
	{
		sspher ret_LBR;
		double loc_DeltaL, loc_DeltaB, loc_LDash, T;

		// TODO wie erfolgt der Aufruf in C#?

		czeit time = new czeit(arg_JDVsopData);
		double locJD = time.JD;
		T = czeit.centuriesSinceJ2000(locJD);

		loc_LDash = arg_dynLBR.L - 1.397*T - 0.00031*T* T;

		// Meeus - Astronomical Algorithms, 2nd edition, p. 219, Eqn. 32.3
		loc_DeltaL = ( -0.09033 + 0.03916*( Math.Cos( Math.PI/180*loc_LDash ) + Math.Sin( Math.PI/180*loc_LDash ) )* Math.Tan( Math.PI/180*arg_dynLBR.B ) ) / astroConst.C__sec_per_hour;
		loc_DeltaB =  0.03916*( Math.Cos( Math.PI/180* loc_LDash) - Math.Sin( Math.PI/180* loc_LDash) ) / astroConst.C__sec_per_hour;

		ret_LBR.L = arg_dynLBR.L + loc_DeltaL;
		ret_LBR.B = arg_dynLBR.B + loc_DeltaB;
		ret_LBR.R = arg_dynLBR.R;

		return (ret_LBR );
	}


	/**
	 * \brief Transformation der heliozentrisch-ekliptikalen (HE) Koordinaten in ein geozentrisch-ekliptikales (GE) Koordinatensystem: HE2GE
	 * gemaess der VSOP87 Theorie
	 * @parameter arg_HE HE Koordinaten des gesuchten Planeten. Ausgang fuer die Transformation.
	 * @parameter arg_HE_Ref HE-Koordinaten des Referenzsystems.
	 * @return Geozentrisch ekliptikale Koordinaten (GE), oder genauer: die zentrischen Koordinaten fuer das Referenzsystem, dessen HE-Koordinaten
	 * in den Parametern arg_HERefX, arg_HERefY, arg_HERefZ uebergeben wurden
	 */
	public GE_coord HE2GE( HE_coord arg_HE, HE_coord arg_HE_Ref )
		{

			GE_coord GE_ret;
			GE_ret.x = arg_HE.x - arg_HE_Ref.x;
			GE_ret.y = arg_HE.y - arg_HE_Ref.y;
			GE_ret.z = arg_HE.z - arg_HE_Ref.z;

			GE_ret.r = Math.Sqrt( GE_ret.x * GE_ret.x + GE_ret.y * GE_ret.y + GE_ret.z* GE_ret.z );


			GE_ret.gms = 180/Math.PI* Math.Atan2(
													GE_ret.z, ( Math.Sqrt( GE_ret.x * GE_ret.x + GE_ret.y * GE_ret.y)));
			GE_ret.hms_grad = 180/Math.PI* Math.Atan2( GE_ret.y, GE_ret.x );


			GE_ret.gms = ohne_ueberlauf_declination(GE_ret.gms);
			GE_ret.hms_grad = ohne_ueberlauf_degrees(GE_ret.hms_grad);

			GE_ret.hms = GE_ret.hms_grad/15;

			//Umwandlung nach h, min, sek
			GE_ret.lh = double2stunde(GE_ret.hms);
			GE_ret.lm = double2minute(GE_ret.hms);
			GE_ret.ls = double2sekunde(GE_ret.hms);

			// Umwandlung in °,'," bzw. h, min, s
			//GE_ret.bg = static_cast<int>(double2grad(GE_ret.gms));
			GE_ret.bg = (int)(double2grad( GE_ret.gms ));
			//GE_ret.bbm = static_cast<int>(double2bogenmin(GE_ret.gms));
			GE_ret.bbm = (int)(double2bogenmin( GE_ret.gms ));
			GE_ret.bbs = double2grad( GE_ret.gms );

			return(GE_ret);

	}
	// -------------------------------------------------------------------------------------------------------------------------------------------------


	/**
	 * Transformation geozentrisch-ekliptikaler (GE) Koordinaten in geozentrisch-aequatoriale (GA) Koordinaten: GE2GA ueber kart. Koord.
	 * @param GE geozentrisch ekliptikale Koordinaten, die nach GA transformiert werden sollen
	 * @param zielepoche
	 * @return scoord GA
	 */
	public static GA_coord GE2GA( GE_coord GE, double zielepoche)
		{
			GA_coord GA_ret;

			// determine obliquity
			czeit loc_timeOfObliquity = new czeit(zielepoche);
			Obliquity loc_Obliquity = new Obliquity(loc_timeOfObliquity);

			double epsilon = loc_Obliquity.getTrueObliquity;

			// Umwandlung in kartesische Koordinaten
			GA_ret.x = GE.x;
			GA_ret.y = GE.y* Math.Cos(Math.PI / 180 * epsilon) - GE.z* Math.Sin(Math.PI / 180 * epsilon);
			GA_ret.z = GE.y* Math.Sin(Math.PI / 180 * epsilon) + GE.z* Math.Cos(Math.PI / 180 * epsilon);

			// Umwandlung in sphärische Koordinaten
			GA_ret.r = Math.Sqrt(GA_ret.x* GA_ret.x + GA_ret.y* GA_ret.y + GA_ret.z* GA_ret.z);


			// Using Meeus - Astronomical Algorithms, 2nd edition, Eqn. (13.3), p.93
			GA_ret.hms_grad = 180 / Math.PI* Math.Atan2(
														(Math.Sin(Math.PI / 180 * GE.hms_grad) * Math.Cos(Math.PI / 180 * epsilon) 
														- Math.Tan(Math.PI / 180 * GE.gms) * Math.Sin(Math.PI / 180 * epsilon)), Math.Cos(Math.PI / 180 * GE.hms_grad)
														);
			GA_ret.hms_grad = ohne_ueberlauf_degrees(GA_ret.hms_grad);
			GA_ret.hms = GA_ret.hms_grad / 15;

			//Umwandlung nach h, min, sek
			GA_ret.lh = double2stunde(GA_ret.hms);
			GA_ret.lm = double2minute(GA_ret.hms);
			GA_ret.ls = double2sekunde(GA_ret.hms);


			// Using Meeus - Astronomical Algorithms, 2nd edition, Eqn. (13.3), p.93
			GA_ret.gms = 180 / Math.PI* Math.Asin(
													Math.Sin(Math.PI / 180 * GE.gms) * Math.Cos(Math.PI / 180 * epsilon) 
													+ Math.Cos(Math.PI / 180 * GE.gms) * Math.Sin(Math.PI / 180 * epsilon) * Math.Sin(Math.PI / 180 * GE.hms_grad)
													);
			GA_ret.gms = ohne_ueberlauf_declination(GA_ret.gms);

			// Umwandlung in °,'," bzw. h, min, s
			GA_ret.bg = (int) (double2grad(GA_ret.gms));

			//GA_ret.bbm = static_cast<int>(double2bogenmin(GA_ret.gms));
			GA_ret.bbm = (int) (double2bogenmin(GA_ret.gms));
			GA_ret.bbs = double2bogensek(GA_ret.gms);


			return (GA_ret);

			}


	// -------------------------------------------------------------------------------------------------------------------



}

