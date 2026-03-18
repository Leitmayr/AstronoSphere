using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
 * \class 	vsop
 * \brief 	setzt die VSOP Theorie um und berechnet abhaengig vom uebergebenen Planetenindex die sphaerischen Koordinaten in HE
 * gemaess der VSOP87-Theorie.
 * 			Basisklassen: ccoord
 *
 * Objekte der Klasse vsop sind Planeten, fuer die die heliozentrisch ekliptikale Koordinaten mittels der "Variations Seculaire des Orbites
 * Planetaires" (VSOP) ermittelt werden. Dies ist der Einsatzgrund fuer die Klasse vsop. Der auszuwaehlende Planet wird mittels des
 * Planetenindex {1: Merkur, 2: Venus, 3: Erde, 4: Mars,  * 5. Jupiter, 6: Saturn, 7: Uranus, 8: Neptun} vom Client selektiert.
 * Zum Einsatz kommen dabei die Terme der VSOP87 Theorie, die die heliozentrisch ekliptikalen Koordinaten (HE) in sphaerischer Form und in
 * Grad zurueckliefert. Dieser Return wird in Stunde/Minute/Sekunde und Grad/Bogenminute/Bogensekunde umgewandelt. Ausserdem werden die
 * kartesischen Koordinaten x, y, z ermittelt. Die resultierende Struktur scoord HE kann als Input fuer die Erzeugung von Objekten der Klasse
 * planeten verwendet werden. Die Klasse vsop leitet aus der Basisklasse ccoord ab, weil zahlreiche Koordinatenumwandlungsmethoden daraus
 * verwendet werden.
*/
public class vsop : ccoord
{


	protected int _vsop_id;
	protected HE_coord _HEvsopDyn; /**< Heliozentrisch ekliptikale Koordinaten, siehe struct scoord fuer Details*/
	protected HE_coord _HEvsopFK5; /**< Heliozentrisch ekliptikale Koordinaten, siehe struct scoord fuer Details*/

	public int getVsopID
	{
		get { return _vsop_id; }
	}

	public double get_vsopLaengeGradDyn
	{
		get { return _HEvsopDyn.hms_grad; }
	}

	public double get_vsopBreiteGradDyn
	{
		get { return _HEvsopDyn.gms; }
	}

	public HE_coord get_HEVsopDyn
	{
		get { return _HEvsopDyn; }
	}

	public HE_coord get_HEVsopFK5
	{
		get { return (_HEvsopFK5); }
	}


	// -------------------------------------------------------------------------------------------------------------------------------------------------
	// Constructor
	// -------------------------------------------------------------------------------------------------------------------------------------------------
	public vsop(int arg_planet_idx, czeit arg_TimeOfVSOPEqn)
	{

		// initialize arg_jt_since2000 with JD: call with UT
		double arg_jt_since2000 = czeit.millenniaSinceJ2000(arg_TimeOfVSOPEqn.JD);

		if (defines.C__vsopTimeSelectionSwitch == 1)
		{
			arg_jt_since2000 = czeit.millenniaSinceJ2000(arg_TimeOfVSOPEqn.JDE); // JDE Aufruf mit dyn. Zeit
		}

		_vsop_id = arg_planet_idx;
		czeit _s = arg_TimeOfVSOPEqn;

		// TODO: Implementierung Observer
		//			_s.registerObserver(this);

		sspher loc_LBRFromVsopDyn;
		sspher loc_LBRFromVsopFK5;


		if (is_planet_idx_ok(arg_planet_idx) == 1)
		{
			loc_LBRFromVsopDyn = calc_vsop(arg_planet_idx, arg_jt_since2000);
			_HEvsopDyn = spherical2HE(loc_LBRFromVsopDyn);

			if (defines.C__vsopTimeSelectionSwitch == 1)
			{
				loc_LBRFromVsopFK5 = dynEqn2FK5(loc_LBRFromVsopDyn, _s.JDE);  // JDE Aufruf mit dyn. Zeit
			}
			else
			{
				loc_LBRFromVsopFK5 = dynEqn2FK5(loc_LBRFromVsopDyn, _s.JD);  // JD Aufruf mit UT
			}
			
			spherical2HE(loc_LBRFromVsopFK5);
			_HEvsopFK5 = spherical2HE(loc_LBRFromVsopFK5);
		}
	}
	// -------------------------------------------------------------------------------------------------------------------------------------------------

	/**
	 * \brief prueft, ob der Planetenindex plausibel ist fuer einen Planeten
	 * @param arg_planet_idx Planeten Index {1: Merkur, 2: Venus, 3: Erde, 4: Mars, 5. Jupiter, 6: Saturn, 7: Uranus, 8: Neptun}
	 * @return 0: planeten idx nicht ok, Ersatzwert dann = 3 (Erde), 1: planeten_idx ok
	 */
	protected int is_planet_idx_ok(int arg_planet_idx)
	{
		int ret_arg = 1;
		if ((arg_planet_idx < astroConst.C__MIN_PLANET_IDX_INNEN) || (arg_planet_idx > astroConst.C__MAX_PLANET_IDX_AUSSEN))
		{
			ret_arg = 0;
		}
		return (ret_arg);
	}

	// -------------------------------------------------------------------------------------------------------------------------------------------------

	/**
	 * \brief calculates the heliocentric longitude L from the Li-Terms (i=0...5)
	 * @param Li, i={0, 1, ...5}: L-Terms from vsop theorie
	 * @param arg_millenia_since2000: millenia since J2000.0
	 * @return L: heliocentric longitude in degrees limited to [0...360]
	 */
	protected double process_LTerms(double arg_L0, double arg_L1, double arg_L2, double arg_L3,
			double arg_L4, double arg_L5, double arg_millenia_since2000)
	{
		double L = arg_L0
				+ arg_millenia_since2000 * (arg_L1 +
						arg_millenia_since2000 * (arg_L2 +
								arg_millenia_since2000 * (arg_L3 +
										arg_millenia_since2000 * (arg_L4 +
												arg_millenia_since2000 * arg_L5))));


		// convert to degrees and limit to [0...360]
		double L_grad = L * 180 / Math.PI;
		L_grad = ohne_ueberlauf_degrees(L_grad);

		return (L_grad);

	}
	// -------------------------------------------------------------------------------------------------------------------------------------------------


	/**
	 * \brief calculates the heliocentric latitude B from the Li-Terms (i=0...5)
	 * @param Bi, i={0, 1, ...5}: B-Terms from vsop theorie
	 * @param arg_millenia_since2000: millenia since J2000.0
	 * @return B: heliocentric latitude in degrees limited to [-90...90]
	 */
	protected double process_BTerms(double arg_B0, double arg_B1, double arg_B2, double arg_B3,
			double arg_B4, double arg_B5, double arg_millenia_since2000)
	{
		double B = arg_B0
			+ arg_millenia_since2000 * (arg_B1 +
					arg_millenia_since2000 * (arg_B2 +
							arg_millenia_since2000 * (arg_B3 +
									arg_millenia_since2000 * (arg_B4 +
											arg_millenia_since2000 * arg_B5))));


		//convert to degrees and limit to [-90...+90]
		double B_grad = B * 180 / Math.PI;
		B_grad = ohne_ueberlauf_declination(B_grad);

		return (B_grad);
	}
	// -------------------------------------------------------------------------------------------------------------------------------------------------


	/**
	 * \brief calculates the sun distance R from the Ri-Terms (i=0...5)
	 * @param Ri, i={0, 1, ...5}: R-Terms from vsop theorie
	 * @param arg_millenia_since2000: millenia since J2000.0
	 * @return R: distance from the sun in astronomical units
	 */
	protected double process_RTerms(double arg_R0, double arg_R1, double arg_R2, double arg_R3,
			double arg_R4, double arg_R5, double arg_millenia_since2000)
	{

		double R = arg_R0
			+ arg_millenia_since2000 * (arg_R1 +
				+arg_millenia_since2000 * (arg_R2 +
						+arg_millenia_since2000 * (arg_R3 +
								+arg_millenia_since2000 * (arg_R4 +
										+arg_millenia_since2000 * (arg_R5)))));


		return (R);
	}
	// -------------------------------------------------------------------------------------------------------------------------------------------------



	public sspher calc_vsop(int arg_planet_idx, double arg_jt_since2000)
	{
		double loc_jt_since2000 = arg_jt_since2000;
		sspher LBR;
			LBR.B = 0.0;
			LBR.L = 0.0;
			LBR.R = 0.0;
		// gemaess Planetenindex {1: Merkur, 2: Venus, 3: Erde, 4: Mars, 5. Jupiter, 6: Saturn, 7: Uranus, 8: Neptun}
		// wird in die entsprechende vsop-Methode für den ausgesuchten Planeten verzweigt
		switch (arg_planet_idx)
			{
			case 1: LBR = vsop_merkur(loc_jt_since2000); break;
			case 2: LBR = vsop_venus(loc_jt_since2000); break;
			case 3: LBR = vsop_erde(loc_jt_since2000); break;
			case 4: LBR = vsop_mars(loc_jt_since2000); break;
			case 5: LBR = vsop_jupiter(loc_jt_since2000); break;
			case 6: LBR = vsop_saturn(loc_jt_since2000); break;
			case 7: LBR = vsop_uranus(loc_jt_since2000); break;
			case 8: LBR = vsop_neptun(loc_jt_since2000); break;
			}

		return (LBR);
	}
	// -------------------------------------------------------------------------------------------------------------------------------------------------

	//public HE_coord spher2HE( sspher arg_LBR)









	// -------------------------------------------------------------------------------------------------------------------------------------------------
	// * \brief The calculations of the following methods for Mercury through Neptune  are taken from Meeus, "Astronomical Algorithms", 2nd edition, Chapter 32, page 218.
	// * The employed constants are taken from Bretagnons VSOP theory and were downloaded from the internet. Considering scaling factors,
	// * these data were identical to the periodic terms given in Meeus, Appendix III, pages 413ff. It is important to state that from the variety
	// * of downloaded files those were selected which provide the constants in heliocentric coordinates given for the employed equinox.
	// * Hence, the returned data from this method are the spherical coordinates L, B and R for Mercury based on VSOP
	// * @param arg_jt_since2000 Millenia since J2000.0 (JD2451545.0)
	// * @return spherical coordinates L, B, R for Mercury. L in deg in [0...360], B in deg in [-90...+90], R in astronomical units
	public sspher vsop_merkur(double arg_jt_since2000)
	{
		sspher ret_LBR;


		// ------------------------------------------------------------------------------------------------------------------
		// determination of the heliocentric longitude
		double L0 = 0;
		double L1 = 0;
		double L2 = 0;
		double L3 = 0;
		double L4 = 0;
		double L5 = 0;

		// C_Mer_NumberOfLTerms = C_Mer_NumberOfBTerms = C_Mer_NumberOfRTerms
		int[] loc_arrMerkurSizeOfLTerms = { 0, 0, 0, 0, 0, 0 };
		int[] loc_arrMerkurSizeOfBTerms = { 0, 0, 0, 0, 0, 0 };
		int[] loc_arrMerkurSizeOfRTerms = { 0, 0, 0, 0, 0, 0 };

		// TODO: replace this by a call of the defintion in class Settings
		int loc_vsopHighAccuracyAcvivationSwitch = defines.C__vsopHighAccuracyAcvivationSwitch;

		for (int i = 0; i < vsopMercuryConst.C_Mer_NumberOfLTerms; i++)
		{
			if (loc_vsopHighAccuracyAcvivationSwitch == 1) // high accuracy
			{
				loc_arrMerkurSizeOfLTerms[i] = vsopMercuryConst.C_Arr_Mer_SizeOfLTerms[i];
				loc_arrMerkurSizeOfBTerms[i] = vsopMercuryConst.C_Arr_Mer_SizeOfBTerms[i];
				loc_arrMerkurSizeOfRTerms[i] = vsopMercuryConst.C_Arr_Mer_SizeOfRTerms[i];
			}
			else //low accuracy
			{
				loc_arrMerkurSizeOfLTerms[i] = vsopMercuryConst.C_Arr_Mer_SizeOfLTermsTrunc[i];
				loc_arrMerkurSizeOfBTerms[i] = vsopMercuryConst.C_Arr_Mer_SizeOfBTermsTrunc[i];
				loc_arrMerkurSizeOfRTerms[i] = vsopMercuryConst.C_Arr_Mer_SizeOfRTermsTrunc[i];
			}

		}


		//L0
		for (int j = 0; j < loc_arrMerkurSizeOfLTerms[0]; j++)
		{
			L0 += vsopMercuryConst.C_Arr2_Mer_L0[j,0] * Math.Cos(vsopMercuryConst.C_Arr2_Mer_L0[j,1] + vsopMercuryConst.C_Arr2_Mer_L0[j,2] * arg_jt_since2000);
		}

		//L1
		for (int j = 0; j < loc_arrMerkurSizeOfLTerms[1]; j++)
		{
			L1 += vsopMercuryConst.C_Arr2_Mer_L1[j,0] * Math.Cos(vsopMercuryConst.C_Arr2_Mer_L1[j,1] + vsopMercuryConst.C_Arr2_Mer_L1[j,2] * arg_jt_since2000);
		}

		//L2
		for (int j = 0; j < loc_arrMerkurSizeOfLTerms[2]; j++)
		{
			L2 += vsopMercuryConst.C_Arr2_Mer_L2[j,0] * Math.Cos(vsopMercuryConst.C_Arr2_Mer_L2[j,1] + vsopMercuryConst.C_Arr2_Mer_L2[j,2] * arg_jt_since2000);
		}


		//L3
		for (int j = 0; j < loc_arrMerkurSizeOfLTerms[3]; j++)
		{
			L3 += vsopMercuryConst.C_Arr2_Mer_L3[j,0] * Math.Cos(vsopMercuryConst.C_Arr2_Mer_L3[j,1] + vsopMercuryConst.C_Arr2_Mer_L3[j,2] * arg_jt_since2000);
		}

		//L4
		for (int j = 0; j < loc_arrMerkurSizeOfLTerms[4]; j++)
		{
			L4 += vsopMercuryConst.C_Arr2_Mer_L4[j,0] * Math.Cos(vsopMercuryConst.C_Arr2_Mer_L4[j,1] + vsopMercuryConst.C_Arr2_Mer_L4[j,2] * arg_jt_since2000);
		}

		//L5
		for (int j = 0; j < loc_arrMerkurSizeOfLTerms[5]; j++)
		{
			L5 += vsopMercuryConst.C_Arr2_Mer_L5[j,0] * Math.Cos(vsopMercuryConst.C_Arr2_Mer_L5[j,1] + vsopMercuryConst.C_Arr2_Mer_L5[j,2] * arg_jt_since2000);
		}


		// calculate the sum of all L-terms, convert them to degrees and limit them to [0...360]
		ret_LBR.L = process_LTerms(L0, L1, L2, L3, L4, L5, arg_jt_since2000);



		// ------------------------------------------------------------------------------------------------------------------
		// determination of the heliocentric latitude
		double B0 = 0;
		double B1 = 0;
		double B2 = 0;
		double B3 = 0;
		double B4 = 0;
		double B5 = 0;

		//B0
		for (int j = 0; j < loc_arrMerkurSizeOfBTerms[0]; j++)
		{
			B0 += vsopMercuryConst.C_Arr2_Mer_B0[j,0] * Math.Cos(vsopMercuryConst.C_Arr2_Mer_B0[j,1] + vsopMercuryConst.C_Arr2_Mer_B0[j,2] * arg_jt_since2000);
		}

		//B1
		for (int j = 0; j < loc_arrMerkurSizeOfBTerms[1]; j++)
		{
			B1 += vsopMercuryConst.C_Arr2_Mer_B1[j,0] * Math.Cos(vsopMercuryConst.C_Arr2_Mer_B1[j,1] + vsopMercuryConst.C_Arr2_Mer_B1[j,2] * arg_jt_since2000);
		}


		//B2
		for (int j = 0; j < loc_arrMerkurSizeOfBTerms[2]; j++)
		{
			B2 += vsopMercuryConst.C_Arr2_Mer_B2[j,0] * Math.Cos(vsopMercuryConst.C_Arr2_Mer_B2[j,1] + vsopMercuryConst.C_Arr2_Mer_B2[j,2] * arg_jt_since2000);
		}

		//B3
		for (int j = 0; j < loc_arrMerkurSizeOfBTerms[3]; j++)
		{
			B3 += vsopMercuryConst.C_Arr2_Mer_B3[j,0] * Math.Cos(vsopMercuryConst.C_Arr2_Mer_B3[j,1] + vsopMercuryConst.C_Arr2_Mer_B3[j,2] * arg_jt_since2000);
		}

		//B4
		for (int j = 0; j < loc_arrMerkurSizeOfBTerms[4]; j++)
		{
			B4 += vsopMercuryConst.C_Arr2_Mer_B4[j,0] * Math.Cos(vsopMercuryConst.C_Arr2_Mer_B4[j,1] + vsopMercuryConst.C_Arr2_Mer_B4[j,2] * arg_jt_since2000);
		}

		//B5
		for (int j = 0; j < loc_arrMerkurSizeOfBTerms[5]; j++)
		{
			B5 += vsopMercuryConst.C_Arr2_Mer_B5[j,0] * Math.Cos(vsopMercuryConst.C_Arr2_Mer_B5[j,1] + vsopMercuryConst.C_Arr2_Mer_B5[j,2] * arg_jt_since2000);
		}


		// calculate the sum of all B-terms, convert them to degrees and limit them to [-90...90]
		ret_LBR.B = process_BTerms(B0, B1, B2, B3, B4, B5, arg_jt_since2000);


		// ------------------------------------------------------------------------------------------------------------------
		// determination of the sun distance
		double R0 = 0;
		double R1 = 0;
		double R2 = 0;
		double R3 = 0;
		double R4 = 0;
		double R5 = 0;

		//R0
		for (int j = 0; j < loc_arrMerkurSizeOfRTerms[0]; j++)
		{
			R0 += vsopMercuryConst.C_Arr2_Mer_R0[j,0] * Math.Cos(vsopMercuryConst.C_Arr2_Mer_R0[j,1] + vsopMercuryConst.C_Arr2_Mer_R0[j,2] * arg_jt_since2000);
		}

		//R1
		for (int j = 0; j < loc_arrMerkurSizeOfRTerms[1]; j++)
		{
			R1 += vsopMercuryConst.C_Arr2_Mer_R1[j,0] * Math.Cos(vsopMercuryConst.C_Arr2_Mer_R1[j,1] + vsopMercuryConst.C_Arr2_Mer_R1[j,2] * arg_jt_since2000);
		}


		//R2
		for (int j = 0; j < loc_arrMerkurSizeOfRTerms[2]; j++)
		{
			R2 += vsopMercuryConst.C_Arr2_Mer_R2[j,0] * Math.Cos(vsopMercuryConst.C_Arr2_Mer_R2[j,1] + vsopMercuryConst.C_Arr2_Mer_R2[j,2] * arg_jt_since2000);
		}

		//R3
		for (int j = 0; j < loc_arrMerkurSizeOfRTerms[3]; j++)
		{
			R3 += vsopMercuryConst.C_Arr2_Mer_R3[j,0] * Math.Cos(vsopMercuryConst.C_Arr2_Mer_R3[j,1] + vsopMercuryConst.C_Arr2_Mer_R3[j,2] * arg_jt_since2000);
		}

		//R4
		for (int j = 0; j < loc_arrMerkurSizeOfRTerms[4]; j++)
		{
			R4 += vsopMercuryConst.C_Arr2_Mer_R4[j,0] * Math.Cos(vsopMercuryConst.C_Arr2_Mer_R4[j,1] + vsopMercuryConst.C_Arr2_Mer_R4[j,2] * arg_jt_since2000);
		}

		//R5
		for (int j = 0; j < loc_arrMerkurSizeOfRTerms[5]; j++)
		{
			R5 += vsopMercuryConst.C_Arr2_Mer_R5[j,0] * Math.Cos(vsopMercuryConst.C_Arr2_Mer_R5[j,1] + vsopMercuryConst.C_Arr2_Mer_R5[j,2] * arg_jt_since2000);
		}

		// calculate the sum of all R-Terms
		ret_LBR.R = process_RTerms(R0, R1, R2, R3, R4, R5, arg_jt_since2000);



		return ret_LBR;
	}
	public sspher vsop_venus(double arg_jt_since2000)
	{
		sspher ret_LBR;


		// ------------------------------------------------------------------------------------------------------------------
		// determination of the heliocentric longitude
		double L0 = 0;
		double L1 = 0;
		double L2 = 0;
		double L3 = 0;
		double L4 = 0;
		double L5 = 0;

		// C_Ven_NumberOfLTerms = C_Ven_NumberOfBTerms = C_Ven_NumberOfRTerms
		int[] loc_arrVenusSizeOfLTerms = { 0, 0, 0, 0, 0, 0 };
		int[] loc_arrVenusSizeOfBTerms = { 0, 0, 0, 0, 0, 0 };
		int[] loc_arrVenusSizeOfRTerms = { 0, 0, 0, 0, 0, 0 };

		// TODO: replace this by a call of the defintion in class Settings
		int loc_vsopHighAccuracyAcvivationSwitch = defines.C__vsopHighAccuracyAcvivationSwitch;

		for (int i = 0; i < vsopVenusConst.C_Ven_NumberOfLTerms; i++)
		{
			if (loc_vsopHighAccuracyAcvivationSwitch == 1) // high accuracy
			{
				loc_arrVenusSizeOfLTerms[i] = vsopVenusConst.C_Arr_Ven_SizeOfLTerms[i];
				loc_arrVenusSizeOfBTerms[i] = vsopVenusConst.C_Arr_Ven_SizeOfBTerms[i];
				loc_arrVenusSizeOfRTerms[i] = vsopVenusConst.C_Arr_Ven_SizeOfRTerms[i];
			}
			else //low accuracy
			{
				loc_arrVenusSizeOfLTerms[i] = vsopVenusConst.C_Arr_Ven_SizeOfLTermsTrunc[i];
				loc_arrVenusSizeOfBTerms[i] = vsopVenusConst.C_Arr_Ven_SizeOfBTermsTrunc[i];
				loc_arrVenusSizeOfRTerms[i] = vsopVenusConst.C_Arr_Ven_SizeOfRTermsTrunc[i];
			}

		}


		//L0
		for (int j = 0; j < loc_arrVenusSizeOfLTerms[0]; j++)
		{
			L0 += vsopVenusConst.C_Arr2_Ven_L0[j,0] * Math.Cos(vsopVenusConst.C_Arr2_Ven_L0[j,1] + vsopVenusConst.C_Arr2_Ven_L0[j,2] * arg_jt_since2000);
		}

		//L1
		for (int j = 0; j < loc_arrVenusSizeOfLTerms[1]; j++)
		{
			L1 += vsopVenusConst.C_Arr2_Ven_L1[j,0] * Math.Cos(vsopVenusConst.C_Arr2_Ven_L1[j,1] + vsopVenusConst.C_Arr2_Ven_L1[j,2] * arg_jt_since2000);
		}

		//L2
		for (int j = 0; j < loc_arrVenusSizeOfLTerms[2]; j++)
		{
			L2 += vsopVenusConst.C_Arr2_Ven_L2[j,0] * Math.Cos(vsopVenusConst.C_Arr2_Ven_L2[j,1] + vsopVenusConst.C_Arr2_Ven_L2[j,2] * arg_jt_since2000);
		}


		//L3
		for (int j = 0; j < loc_arrVenusSizeOfLTerms[3]; j++)
		{
			L3 += vsopVenusConst.C_Arr2_Ven_L3[j,0] * Math.Cos(vsopVenusConst.C_Arr2_Ven_L3[j,1] + vsopVenusConst.C_Arr2_Ven_L3[j,2] * arg_jt_since2000);
		}

		//L4
		for (int j = 0; j < loc_arrVenusSizeOfLTerms[4]; j++)
		{
			L4 += vsopVenusConst.C_Arr2_Ven_L4[j,0] * Math.Cos(vsopVenusConst.C_Arr2_Ven_L4[j,1] + vsopVenusConst.C_Arr2_Ven_L4[j,2] * arg_jt_since2000);
		}

		//L5
		for (int j = 0; j < loc_arrVenusSizeOfLTerms[5]; j++)
		{
			L5 += vsopVenusConst.C_Arr2_Ven_L5[j,0] * Math.Cos(vsopVenusConst.C_Arr2_Ven_L5[j,1] + vsopVenusConst.C_Arr2_Ven_L5[j,2] * arg_jt_since2000);
		}


		// calculate the sum of all L-terms, convert them to degrees and limit them to [0...360]
		ret_LBR.L = process_LTerms(L0, L1, L2, L3, L4, L5, arg_jt_since2000);



		// ------------------------------------------------------------------------------------------------------------------
		// determination of the heliocentric latitude
		double B0 = 0;
		double B1 = 0;
		double B2 = 0;
		double B3 = 0;
		double B4 = 0;
		double B5 = 0;

		//B0
		for (int j = 0; j < loc_arrVenusSizeOfBTerms[0]; j++)
		{
			B0 += vsopVenusConst.C_Arr2_Ven_B0[j,0] * Math.Cos(vsopVenusConst.C_Arr2_Ven_B0[j,1] + vsopVenusConst.C_Arr2_Ven_B0[j,2] * arg_jt_since2000);
		}

		//B1
		for (int j = 0; j < loc_arrVenusSizeOfBTerms[1]; j++)
		{
			B1 += vsopVenusConst.C_Arr2_Ven_B1[j,0] * Math.Cos(vsopVenusConst.C_Arr2_Ven_B1[j,1] + vsopVenusConst.C_Arr2_Ven_B1[j,2] * arg_jt_since2000);
		}


		//B2
		for (int j = 0; j < loc_arrVenusSizeOfBTerms[2]; j++)
		{
			B2 += vsopVenusConst.C_Arr2_Ven_B2[j,0] * Math.Cos(vsopVenusConst.C_Arr2_Ven_B2[j,1] + vsopVenusConst.C_Arr2_Ven_B2[j,2] * arg_jt_since2000);
		}

		//B3
		for (int j = 0; j < loc_arrVenusSizeOfBTerms[3]; j++)
		{
			B3 += vsopVenusConst.C_Arr2_Ven_B3[j,0] * Math.Cos(vsopVenusConst.C_Arr2_Ven_B3[j,1] + vsopVenusConst.C_Arr2_Ven_B3[j,2] * arg_jt_since2000);
		}

		//B4
		for (int j = 0; j < loc_arrVenusSizeOfBTerms[4]; j++)
		{
			B4 += vsopVenusConst.C_Arr2_Ven_B4[j,0] * Math.Cos(vsopVenusConst.C_Arr2_Ven_B4[j,1] + vsopVenusConst.C_Arr2_Ven_B4[j,2] * arg_jt_since2000);
		}

		//B5
		for (int j = 0; j < loc_arrVenusSizeOfBTerms[5]; j++)
		{
			B5 += vsopVenusConst.C_Arr2_Ven_B5[j,0] * Math.Cos(vsopVenusConst.C_Arr2_Ven_B5[j,1] + vsopVenusConst.C_Arr2_Ven_B5[j,2] * arg_jt_since2000);
		}


		// calculate the sum of all B-terms, convert them to degrees and limit them to [-90...90]
		ret_LBR.B = process_BTerms(B0, B1, B2, B3, B4, B5, arg_jt_since2000);


		// ------------------------------------------------------------------------------------------------------------------
		// determination of the sun distance
		double R0 = 0;
		double R1 = 0;
		double R2 = 0;
		double R3 = 0;
		double R4 = 0;
		double R5 = 0;

		//R0
		for (int j = 0; j < loc_arrVenusSizeOfRTerms[0]; j++)
		{
			R0 += vsopVenusConst.C_Arr2_Ven_R0[j,0] * Math.Cos(vsopVenusConst.C_Arr2_Ven_R0[j,1] + vsopVenusConst.C_Arr2_Ven_R0[j,2] * arg_jt_since2000);
		}

		//R1
		for (int j = 0; j < loc_arrVenusSizeOfRTerms[1]; j++)
		{
			R1 += vsopVenusConst.C_Arr2_Ven_R1[j,0] * Math.Cos(vsopVenusConst.C_Arr2_Ven_R1[j,1] + vsopVenusConst.C_Arr2_Ven_R1[j,2] * arg_jt_since2000);
		}


		//R2
		for (int j = 0; j < loc_arrVenusSizeOfRTerms[2]; j++)
		{
			R2 += vsopVenusConst.C_Arr2_Ven_R2[j,0] * Math.Cos(vsopVenusConst.C_Arr2_Ven_R2[j,1] + vsopVenusConst.C_Arr2_Ven_R2[j,2] * arg_jt_since2000);
		}

		//R3
		for (int j = 0; j < loc_arrVenusSizeOfRTerms[3]; j++)
		{
			R3 += vsopVenusConst.C_Arr2_Ven_R3[j,0] * Math.Cos(vsopVenusConst.C_Arr2_Ven_R3[j,1] + vsopVenusConst.C_Arr2_Ven_R3[j,2] * arg_jt_since2000);
		}

		//R4
		for (int j = 0; j < loc_arrVenusSizeOfRTerms[4]; j++)
		{
			R4 += vsopVenusConst.C_Arr2_Ven_R4[j,0] * Math.Cos(vsopVenusConst.C_Arr2_Ven_R4[j,1] + vsopVenusConst.C_Arr2_Ven_R4[j,2] * arg_jt_since2000);
		}

		//R5
		for (int j = 0; j < loc_arrVenusSizeOfRTerms[5]; j++)
		{
			R5 += vsopVenusConst.C_Arr2_Ven_R5[j,0] * Math.Cos(vsopVenusConst.C_Arr2_Ven_R5[j,1] + vsopVenusConst.C_Arr2_Ven_R5[j,2] * arg_jt_since2000);
		}

		// calculate the sum of all R-Terms
		ret_LBR.R = process_RTerms(R0, R1, R2, R3, R4, R5, arg_jt_since2000);



		return ret_LBR;


	}
	public sspher vsop_erde(double arg_jt_since2000)
	{
		sspher ret_LBR;

		// ------------------------------------------------------------------------------------------------------------------
		// determination of the heliocentric longitude
		double L0 = 0;
		double L1 = 0;
		double L2 = 0;
		double L3 = 0;
		double L4 = 0;
		double L5 = 0;


		// C_Ear_NumberOfLTerms = C_Ear_NumberOfBTerms = C_Ear_NumberOfRTerms
		int[] loc_arrEarthSizeOfLTerms = { 0, 0, 0, 0, 0, 0 };
		int[] loc_arrEarthSizeOfBTerms = { 0, 0, 0, 0, 0, 0 };
		int[] loc_arrEarthSizeOfRTerms = { 0, 0, 0, 0, 0, 0 };

		// TODO: replace this by a call of the defintion in class Settings
		int loc_vsopHighAccuracyAcvivationSwitch = defines.C__vsopHighAccuracyAcvivationSwitch;

		for (int i = 0; i < vsopEarthConst.C_Ear_NumberOfLTerms; i++)
		{
			if (loc_vsopHighAccuracyAcvivationSwitch == 1) // high accuracy
			{
				loc_arrEarthSizeOfLTerms[i] = vsopEarthConst.C_Arr_Ear_SizeOfLTerms[i];
				loc_arrEarthSizeOfBTerms[i] = vsopEarthConst.C_Arr_Ear_SizeOfBTerms[i];
				loc_arrEarthSizeOfRTerms[i] = vsopEarthConst.C_Arr_Ear_SizeOfRTerms[i];
			}
			else //low accuracy
			{
				loc_arrEarthSizeOfLTerms[i] = vsopEarthConst.C_Arr_Ear_SizeOfLTermsTrunc[i];
				loc_arrEarthSizeOfBTerms[i] = vsopEarthConst.C_Arr_Ear_SizeOfBTermsTrunc[i];
				loc_arrEarthSizeOfRTerms[i] = vsopEarthConst.C_Arr_Ear_SizeOfRTermsTrunc[i];
			}

		}


		//L0
		for (int j = 0; j < loc_arrEarthSizeOfLTerms[0]; j++)
		{
			L0 += vsopEarthConst.C_Arr2_Ear_L0[j, 0] * Math.Cos(vsopEarthConst.C_Arr2_Ear_L0[j,1] + vsopEarthConst.C_Arr2_Ear_L0[j,2] * arg_jt_since2000);
		}

		//L1
		for (int j = 0; j < loc_arrEarthSizeOfLTerms[1]; j++)
		{
			L1 += vsopEarthConst.C_Arr2_Ear_L1[j, 0] * Math.Cos(vsopEarthConst.C_Arr2_Ear_L1[j,1] + vsopEarthConst.C_Arr2_Ear_L1[j,2] * arg_jt_since2000);
		}


		//L2
		for (int j = 0; j < loc_arrEarthSizeOfLTerms[2]; j++)
		{
			L2 += vsopEarthConst.C_Arr2_Ear_L2[j, 0] * Math.Cos(vsopEarthConst.C_Arr2_Ear_L2[j,1] + vsopEarthConst.C_Arr2_Ear_L2[j,2] * arg_jt_since2000);
		}


		//L3
		for (int j = 0; j < loc_arrEarthSizeOfLTerms[3]; j++)
		{
			L3 += vsopEarthConst.C_Arr2_Ear_L3[j, 0] * Math.Cos(vsopEarthConst.C_Arr2_Ear_L3[j,1] + vsopEarthConst.C_Arr2_Ear_L3[j,2] * arg_jt_since2000);
		}

		//L4
		for (int j = 0; j < loc_arrEarthSizeOfLTerms[4]; j++)
		{
			L4 += vsopEarthConst.C_Arr2_Ear_L4[j,0] * Math.Cos(vsopEarthConst.C_Arr2_Ear_L4[j,1] + vsopEarthConst.C_Arr2_Ear_L4[j,2] * arg_jt_since2000);
		}

		//L5
		for (int j = 0; j < loc_arrEarthSizeOfLTerms[5]; j++)
		{
			L5 += vsopEarthConst.C_Arr2_Ear_L5[j,0] * Math.Cos(vsopEarthConst.C_Arr2_Ear_L5[j,1] + vsopEarthConst.C_Arr2_Ear_L5[j,2] * arg_jt_since2000);
		}


		// calculate the sum of all L-terms, convert them to degrees and limit them to [0...360]
		ret_LBR.L = process_LTerms(L0, L1, L2, L3, L4, L5, arg_jt_since2000);



		// ------------------------------------------------------------------------------------------------------------------
		// determination of the heliocentric latitude
		double B0 = 0;
		double B1 = 0;
		double B2 = 0;
		double B3 = 0;
		double B4 = 0;
		double B5 = 0;


		//B0
		for (int j = 0; j < loc_arrEarthSizeOfBTerms[0]; j++)
		{
			B0 += vsopEarthConst.C_Arr2_Ear_B0[j,0] * Math.Cos(vsopEarthConst.C_Arr2_Ear_B0[j,1] + vsopEarthConst.C_Arr2_Ear_B0[j,2] * arg_jt_since2000);
		}

		//B1
		for (int j = 0; j < loc_arrEarthSizeOfBTerms[1]; j++)
		{
			B1 += vsopEarthConst.C_Arr2_Ear_B1[j,0] * Math.Cos(vsopEarthConst.C_Arr2_Ear_B1[j,1] + vsopEarthConst.C_Arr2_Ear_B1[j,2] * arg_jt_since2000);
		}


		//B2
		for (int j = 0; j < loc_arrEarthSizeOfBTerms[2]; j++)
		{
			B2 += vsopEarthConst.C_Arr2_Ear_B2[j,0] * Math.Cos(vsopEarthConst.C_Arr2_Ear_B2[j,1] + vsopEarthConst.C_Arr2_Ear_B2[j,2] * arg_jt_since2000);
		}

		//B3
		for (int j = 0; j < loc_arrEarthSizeOfBTerms[3]; j++)
		{
			B3 += vsopEarthConst.C_Arr2_Ear_B3[j,0] * Math.Cos(vsopEarthConst.C_Arr2_Ear_B3[j,1] + vsopEarthConst.C_Arr2_Ear_B3[j,2] * arg_jt_since2000);
		}

		//B4
		for (int j = 0; j < loc_arrEarthSizeOfBTerms[4]; j++)
		{
			B4 += vsopEarthConst.C_Arr2_Ear_B4[j,0] * Math.Cos(vsopEarthConst.C_Arr2_Ear_B4[j,1] + vsopEarthConst.C_Arr2_Ear_B4[j,2] * arg_jt_since2000);
		}


		// calculate the sum of all B-terms, convert them to degrees and limit them to [-90...90]
		ret_LBR.B = process_BTerms(B0, B1, B2, B3, B4, B5, arg_jt_since2000);


		// ------------------------------------------------------------------------------------------------------------------
		// determination of the sun distance
		double R0 = 0;
		double R1 = 0;
		double R2 = 0;
		double R3 = 0;
		double R4 = 0;
		double R5 = 0;

		//R0
		for (int j = 0; j < loc_arrEarthSizeOfRTerms[0]; j++)
		{
			R0 += vsopEarthConst.C_Arr2_Ear_R0[j,0] * Math.Cos(vsopEarthConst.C_Arr2_Ear_R0[j,1] + vsopEarthConst.C_Arr2_Ear_R0[j,2] * arg_jt_since2000);
		}

		//R1
		for (int j = 0; j < loc_arrEarthSizeOfRTerms[1]; j++)
		{
			R1 += vsopEarthConst.C_Arr2_Ear_R1[j,0] * Math.Cos(vsopEarthConst.C_Arr2_Ear_R1[j,1] + vsopEarthConst.C_Arr2_Ear_R1[j,2] * arg_jt_since2000);
		}


		//R2
		for (int j = 0; j < loc_arrEarthSizeOfRTerms[2]; j++)
		{
			R2 += vsopEarthConst.C_Arr2_Ear_R2[j,0] * Math.Cos(vsopEarthConst.C_Arr2_Ear_R2[j,1] + vsopEarthConst.C_Arr2_Ear_R2[j,2] * arg_jt_since2000);
		}

		//R3
		for (int j = 0; j < loc_arrEarthSizeOfRTerms[3]; j++)
		{
			R3 += vsopEarthConst.C_Arr2_Ear_R3[j,0] * Math.Cos(vsopEarthConst.C_Arr2_Ear_R3[j,1] + vsopEarthConst.C_Arr2_Ear_R3[j,2] * arg_jt_since2000);
		}

		//R4
		for (int j = 0; j < loc_arrEarthSizeOfRTerms[4]; j++)
		{
			R4 += vsopEarthConst.C_Arr2_Ear_R4[j,0] * Math.Cos(vsopEarthConst.C_Arr2_Ear_R4[j,1] + vsopEarthConst.C_Arr2_Ear_R4[j,2] * arg_jt_since2000);
		}

		//R5
		for (int j = 0; j < loc_arrEarthSizeOfRTerms[5]; j++)
		{
			R5 += vsopEarthConst.C_Arr2_Ear_R5[j,0] * Math.Cos(vsopEarthConst.C_Arr2_Ear_R5[j,1] + vsopEarthConst.C_Arr2_Ear_R5[j,2] * arg_jt_since2000);
		}

		// calculate the sum of all R-Terms
		ret_LBR.R = process_RTerms(R0, R1, R2, R3, R4, R5, arg_jt_since2000);



		return ret_LBR;



	}
	public sspher vsop_mars( double arg_jt_since2000)
    {
		sspher ret_LBR;

		// ------------------------------------------------------------------------------------------------------------------
		// determination of the heliocentric longitude
		double L0 = 0;
		double L1 = 0;
		double L2 = 0;
		double L3 = 0;
		double L4 = 0;
		double L5 = 0;

		// C_Mar_NumberOfLTerms = C_Mar_NumberOfBTerms = C_Mar_NumberOfRTerms
		int[] loc_arrMarsSizeOfLTerms = { 0, 0, 0, 0, 0, 0 };
		int[] loc_arrMarsSizeOfBTerms = { 0, 0, 0, 0, 0, 0 };
		int[] loc_arrMarsSizeOfRTerms = { 0, 0, 0, 0, 0, 0 };

		// TODO: replace this by a call of the defintion in class Settings
		int loc_vsopHighAccuracyAcvivationSwitch = defines.C__vsopHighAccuracyAcvivationSwitch;

		for (int i = 0; i < vsopMarsConst.C_Mar_NumberOfLTerms; i++)
		{
			if (loc_vsopHighAccuracyAcvivationSwitch == 1) // high accuracy
			{
				loc_arrMarsSizeOfLTerms[i] = vsopMarsConst.C_Arr_Mar_SizeOfLTerms[i];
				loc_arrMarsSizeOfBTerms[i] = vsopMarsConst.C_Arr_Mar_SizeOfBTerms[i];
				loc_arrMarsSizeOfRTerms[i] = vsopMarsConst.C_Arr_Mar_SizeOfRTerms[i];
			}
			else //low accuracy
			{
				loc_arrMarsSizeOfLTerms[i] = vsopMarsConst.C_Arr_Mar_SizeOfLTermsTrunc[i];
				loc_arrMarsSizeOfBTerms[i] = vsopMarsConst.C_Arr_Mar_SizeOfBTermsTrunc[i];
				loc_arrMarsSizeOfRTerms[i] = vsopMarsConst.C_Arr_Mar_SizeOfRTermsTrunc[i];
			}

		}


		//L0
		for (int j = 0; j < loc_arrMarsSizeOfLTerms[0]; j++)
		{
			L0 += vsopMarsConst.C_Arr2_Mar_L0[j,0] * Math.Cos(vsopMarsConst.C_Arr2_Mar_L0[j,1] + vsopMarsConst.C_Arr2_Mar_L0[j,2] * arg_jt_since2000);
		}

		//L1
		for (int j = 0; j < loc_arrMarsSizeOfLTerms[1]; j++)
		{
			L1 += vsopMarsConst.C_Arr2_Mar_L1[j,0] * Math.Cos(vsopMarsConst.C_Arr2_Mar_L1[j,1] + vsopMarsConst.C_Arr2_Mar_L1[j,2] * arg_jt_since2000);
		}


		//L2
		for (int j = 0; j < loc_arrMarsSizeOfLTerms[2]; j++)
		{
			L2 += vsopMarsConst.C_Arr2_Mar_L2[j,0] * Math.Cos(vsopMarsConst.C_Arr2_Mar_L2[j,1] + vsopMarsConst.C_Arr2_Mar_L2[j,2] * arg_jt_since2000);
		}


		//L3
		for (int j = 0; j < loc_arrMarsSizeOfLTerms[3]; j++)
		{
			L3 += vsopMarsConst.C_Arr2_Mar_L3[j,0] * Math.Cos(vsopMarsConst.C_Arr2_Mar_L3[j,1] + vsopMarsConst.C_Arr2_Mar_L3[j,2] * arg_jt_since2000);
		}

		//L4
		for (int j = 0; j < loc_arrMarsSizeOfLTerms[4]; j++)
		{
			L4 += vsopMarsConst.C_Arr2_Mar_L4[j,0] * Math.Cos(vsopMarsConst.C_Arr2_Mar_L4[j,1] + vsopMarsConst.C_Arr2_Mar_L4[j,2] * arg_jt_since2000);
		}

		//L5
		for (int j = 0; j < loc_arrMarsSizeOfLTerms[5]; j++)
		{
			L5 += vsopMarsConst.C_Arr2_Mar_L5[j,0] * Math.Cos(vsopMarsConst.C_Arr2_Mar_L5[j,1] + vsopMarsConst.C_Arr2_Mar_L5[j,2] * arg_jt_since2000);
		}


		// calculate the sum of all L-terms, convert them to degrees and limit them to [0...360]
		ret_LBR.L = process_LTerms(L0, L1, L2, L3, L4, L5, arg_jt_since2000);



		// ------------------------------------------------------------------------------------------------------------------
		// determination of the heliocentric latitude
		double B0 = 0;
		double B1 = 0;
		double B2 = 0;
		double B3 = 0;
		double B4 = 0;
		double B5 = 0;

		//B0
		for (int j = 0; j < loc_arrMarsSizeOfBTerms[0]; j++)
		{
			B0 += vsopMarsConst.C_Arr2_Mar_B0[j,0] * Math.Cos(vsopMarsConst.C_Arr2_Mar_B0[j,1] + vsopMarsConst.C_Arr2_Mar_B0[j,2] * arg_jt_since2000);
		}

		//B1
		for (int j = 0; j < loc_arrMarsSizeOfBTerms[1]; j++)
		{
			B1 += vsopMarsConst.C_Arr2_Mar_B1[j,0] * Math.Cos(vsopMarsConst.C_Arr2_Mar_B1[j,1] + vsopMarsConst.C_Arr2_Mar_B1[j,2] * arg_jt_since2000);
		}


		//B2
		for (int j = 0; j < loc_arrMarsSizeOfBTerms[2]; j++)
		{
			B2 += vsopMarsConst.C_Arr2_Mar_B2[j,0] * Math.Cos(vsopMarsConst.C_Arr2_Mar_B2[j,1] + vsopMarsConst.C_Arr2_Mar_B2[j,2] * arg_jt_since2000);
		}

		//B3
		for (int j = 0; j < loc_arrMarsSizeOfBTerms[3]; j++)
		{
			B3 += vsopMarsConst.C_Arr2_Mar_B3[j,0] * Math.Cos(vsopMarsConst.C_Arr2_Mar_B3[j,1] + vsopMarsConst.C_Arr2_Mar_B3[j,2] * arg_jt_since2000);
		}

		//B4
		for (int j = 0; j < loc_arrMarsSizeOfBTerms[4]; j++)
		{
			B4 += vsopMarsConst.C_Arr2_Mar_B4[j,0] * Math.Cos(vsopMarsConst.C_Arr2_Mar_B4[j,1] + vsopMarsConst.C_Arr2_Mar_B4[j,2] * arg_jt_since2000);
		}

		//B5
		for (int j = 0; j < loc_arrMarsSizeOfBTerms[5]; j++)
		{
			B5 += vsopMarsConst.C_Arr2_Mar_B5[j,0] * Math.Cos(vsopMarsConst.C_Arr2_Mar_B5[j,1] + vsopMarsConst.C_Arr2_Mar_B5[j,2] * arg_jt_since2000);
		}


		// calculate the sum of all B-terms, convert them to degrees and limit them to [-90...90]
		ret_LBR.B = process_BTerms(B0, B1, B2, B3, B4, B5, arg_jt_since2000);


		// ------------------------------------------------------------------------------------------------------------------
		// determination of the sun distance
		double R0 = 0;
		double R1 = 0;
		double R2 = 0;
		double R3 = 0;
		double R4 = 0;
		double R5 = 0;

		//R0
		for (int j = 0; j < loc_arrMarsSizeOfRTerms[0]; j++)
		{
			R0 += vsopMarsConst.C_Arr2_Mar_R0[j,0] * Math.Cos(vsopMarsConst.C_Arr2_Mar_R0[j,1] + vsopMarsConst.C_Arr2_Mar_R0[j,2] * arg_jt_since2000);
		}

		//R1
		for (int j = 0; j < loc_arrMarsSizeOfRTerms[1]; j++)
		{
			R1 += vsopMarsConst.C_Arr2_Mar_R1[j,0] * Math.Cos(vsopMarsConst.C_Arr2_Mar_R1[j,1] + vsopMarsConst.C_Arr2_Mar_R1[j,2] * arg_jt_since2000);
		}


		//R2
		for (int j = 0; j < loc_arrMarsSizeOfRTerms[2]; j++)
		{
			R2 += vsopMarsConst.C_Arr2_Mar_R2[j,0] * Math.Cos(vsopMarsConst.C_Arr2_Mar_R2[j,1] + vsopMarsConst.C_Arr2_Mar_R2[j,2] * arg_jt_since2000);
		}

		//R3
		for (int j = 0; j < loc_arrMarsSizeOfRTerms[3]; j++)
		{
			R3 += vsopMarsConst.C_Arr2_Mar_R3[j,0] * Math.Cos(vsopMarsConst.C_Arr2_Mar_R3[j,1] + vsopMarsConst.C_Arr2_Mar_R3[j,2] * arg_jt_since2000);
		}

		//R4
		for (int j = 0; j < loc_arrMarsSizeOfRTerms[4]; j++)
		{
			R4 += vsopMarsConst.C_Arr2_Mar_R4[j,0] * Math.Cos(vsopMarsConst.C_Arr2_Mar_R4[j,1] + vsopMarsConst.C_Arr2_Mar_R4[j,2] * arg_jt_since2000);
		}

		//R5
		for (int j = 0; j < loc_arrMarsSizeOfRTerms[5]; j++)
		{
			R5 += vsopMarsConst.C_Arr2_Mar_R5[j,0] * Math.Cos(vsopMarsConst.C_Arr2_Mar_R5[j,1] + vsopMarsConst.C_Arr2_Mar_R5[j,2] * arg_jt_since2000);
		}

		// calculate the sum of all R-Terms
		ret_LBR.R = process_RTerms(R0, R1, R2, R3, R4, R5, arg_jt_since2000);



		return ret_LBR;

	}
	public sspher vsop_jupiter(double arg_jt_since2000)
	{

		sspher ret_LBR;

		// ------------------------------------------------------------------------------------------------------------------
		// determination of the heliocentric longitude
		double L0 = 0;
		double L1 = 0;
		double L2 = 0;
		double L3 = 0;
		double L4 = 0;
		double L5 = 0;

		// C_Jup_NumberOfLTerms = C_Jup_NumberOfBTerms = C_Jup_NumberOfRTerms
		int[] loc_arrJupiterSizeOfLTerms = { 0, 0, 0, 0, 0, 0 };
		int[] loc_arrJupiterSizeOfBTerms = { 0, 0, 0, 0, 0, 0 };
		int[] loc_arrJupiterSizeOfRTerms = { 0, 0, 0, 0, 0, 0 };

		// TODO: replace this by a call of the defintion in class Settings
		int loc_vsopHighAccuracyAcvivationSwitch = defines.C__vsopHighAccuracyAcvivationSwitch;

		for (int i = 0; i < vsopJupiterConst.C_Jup_NumberOfLTerms; i++)
		{
			if (loc_vsopHighAccuracyAcvivationSwitch == 1) // high accuracy
			{
				loc_arrJupiterSizeOfLTerms[i] = vsopJupiterConst.C_Arr_Jup_SizeOfLTerms[i];
				loc_arrJupiterSizeOfBTerms[i] = vsopJupiterConst.C_Arr_Jup_SizeOfBTerms[i];
				loc_arrJupiterSizeOfRTerms[i] = vsopJupiterConst.C_Arr_Jup_SizeOfRTerms[i];
			}
			else //low accuracy
			{
				loc_arrJupiterSizeOfLTerms[i] = vsopJupiterConst.C_Arr_Jup_SizeOfLTermsTrunc[i];
				loc_arrJupiterSizeOfBTerms[i] = vsopJupiterConst.C_Arr_Jup_SizeOfBTermsTrunc[i];
				loc_arrJupiterSizeOfRTerms[i] = vsopJupiterConst.C_Arr_Jup_SizeOfRTermsTrunc[i];
			}

		}


		//L0
		for (int j = 0; j < loc_arrJupiterSizeOfLTerms[0]; j++)
		{
			L0 += vsopJupiterConst.C_Arr2_Jup_L0[j,0] * Math.Cos(vsopJupiterConst.C_Arr2_Jup_L0[j,1] + vsopJupiterConst.C_Arr2_Jup_L0[j,2] * arg_jt_since2000);
		}

		//L1
		for (int j = 0; j < loc_arrJupiterSizeOfLTerms[1]; j++)
		{
			L1 += vsopJupiterConst.C_Arr2_Jup_L1[j,0] * Math.Cos(vsopJupiterConst.C_Arr2_Jup_L1[j,1] + vsopJupiterConst.C_Arr2_Jup_L1[j,2] * arg_jt_since2000);
		}


		//L2
		for (int j = 0; j < loc_arrJupiterSizeOfLTerms[2]; j++)
		{
			L2 += vsopJupiterConst.C_Arr2_Jup_L2[j,0] * Math.Cos(vsopJupiterConst.C_Arr2_Jup_L2[j,1] + vsopJupiterConst.C_Arr2_Jup_L2[j,2] * arg_jt_since2000);
		}


		//L3
		for (int j = 0; j < loc_arrJupiterSizeOfLTerms[3]; j++)
		{
			L3 += vsopJupiterConst.C_Arr2_Jup_L3[j,0] * Math.Cos(vsopJupiterConst.C_Arr2_Jup_L3[j,1] + vsopJupiterConst.C_Arr2_Jup_L3[j,2] * arg_jt_since2000);
		}

		//L4
		for (int j = 0; j < loc_arrJupiterSizeOfLTerms[4]; j++)
		{
			L4 += vsopJupiterConst.C_Arr2_Jup_L4[j,0] * Math.Cos(vsopJupiterConst.C_Arr2_Jup_L4[j,1] + vsopJupiterConst.C_Arr2_Jup_L4[j,2] * arg_jt_since2000);
		}

		//L5
		for (int j = 0; j < loc_arrJupiterSizeOfLTerms[5]; j++)
		{
			L5 += vsopJupiterConst.C_Arr2_Jup_L5[j,0] * Math.Cos(vsopJupiterConst.C_Arr2_Jup_L5[j,1] + vsopJupiterConst.C_Arr2_Jup_L5[j,2] * arg_jt_since2000);
		}


		// calculate the sum of all L-terms, convert them to degrees and limit them to [0...360]
		ret_LBR.L = process_LTerms(L0, L1, L2, L3, L4, L5, arg_jt_since2000);



		// ------------------------------------------------------------------------------------------------------------------
		// determination of the heliocentric latitude
		double B0 = 0;
		double B1 = 0;
		double B2 = 0;
		double B3 = 0;
		double B4 = 0;
		double B5 = 0;

		//B0
		for (int j = 0; j < loc_arrJupiterSizeOfBTerms[0]; j++)
		{
			B0 += vsopJupiterConst.C_Arr2_Jup_B0[j,0] * Math.Cos(vsopJupiterConst.C_Arr2_Jup_B0[j,1] + vsopJupiterConst.C_Arr2_Jup_B0[j,2] * arg_jt_since2000);
		}

		//B1
		for (int j = 0; j < loc_arrJupiterSizeOfBTerms[1]; j++)
		{
			B1 += vsopJupiterConst.C_Arr2_Jup_B1[j,0] * Math.Cos(vsopJupiterConst.C_Arr2_Jup_B1[j,1] + vsopJupiterConst.C_Arr2_Jup_B1[j,2] * arg_jt_since2000);
		}


		//B2
		for (int j = 0; j < loc_arrJupiterSizeOfBTerms[2]; j++)
		{
			B2 += vsopJupiterConst.C_Arr2_Jup_B2[j,0] * Math.Cos(vsopJupiterConst.C_Arr2_Jup_B2[j,1] + vsopJupiterConst.C_Arr2_Jup_B2[j,2] * arg_jt_since2000);
		}

		//B3
		for (int j = 0; j < loc_arrJupiterSizeOfBTerms[3]; j++)
		{
			B3 += vsopJupiterConst.C_Arr2_Jup_B3[j,0] * Math.Cos(vsopJupiterConst.C_Arr2_Jup_B3[j,1] + vsopJupiterConst.C_Arr2_Jup_B3[j,2] * arg_jt_since2000);
		}

		//B4
		for (int j = 0; j < loc_arrJupiterSizeOfBTerms[4]; j++)
		{
			B4 += vsopJupiterConst.C_Arr2_Jup_B4[j,0] * Math.Cos(vsopJupiterConst.C_Arr2_Jup_B4[j,1] + vsopJupiterConst.C_Arr2_Jup_B4[j,2] * arg_jt_since2000);
		}

		//B5
		for (int j = 0; j < loc_arrJupiterSizeOfBTerms[5]; j++)
		{
			B5 += vsopJupiterConst.C_Arr2_Jup_B5[j,0] * Math.Cos(vsopJupiterConst.C_Arr2_Jup_B5[j,1] + vsopJupiterConst.C_Arr2_Jup_B5[j,2] * arg_jt_since2000);
		}


		// calculate the sum of all B-terms, convert them to degrees and limit them to [-90...90]
		ret_LBR.B = process_BTerms(B0, B1, B2, B3, B4, B5, arg_jt_since2000);


		// ------------------------------------------------------------------------------------------------------------------
		// determination of the sun distance
		double R0 = 0;
		double R1 = 0;
		double R2 = 0;
		double R3 = 0;
		double R4 = 0;
		double R5 = 0;

		//R0
		for (int j = 0; j < loc_arrJupiterSizeOfRTerms[0]; j++)
		{
			R0 += vsopJupiterConst.C_Arr2_Jup_R0[j,0] * Math.Cos(vsopJupiterConst.C_Arr2_Jup_R0[j,1] + vsopJupiterConst.C_Arr2_Jup_R0[j,2] * arg_jt_since2000);
		}

		//R1
		for (int j = 0; j < loc_arrJupiterSizeOfRTerms[1]; j++)
		{
			R1 += vsopJupiterConst.C_Arr2_Jup_R1[j,0] * Math.Cos(vsopJupiterConst.C_Arr2_Jup_R1[j,1] + vsopJupiterConst.C_Arr2_Jup_R1[j,2] * arg_jt_since2000);
		}


		//R2
		for (int j = 0; j < loc_arrJupiterSizeOfRTerms[2]; j++)
		{
			R2 += vsopJupiterConst.C_Arr2_Jup_R2[j,0] * Math.Cos(vsopJupiterConst.C_Arr2_Jup_R2[j,1] + vsopJupiterConst.C_Arr2_Jup_R2[j,2] * arg_jt_since2000);
		}

		//R3
		for (int j = 0; j < loc_arrJupiterSizeOfRTerms[3]; j++)
		{
			R3 += vsopJupiterConst.C_Arr2_Jup_R3[j,0] * Math.Cos(vsopJupiterConst.C_Arr2_Jup_R3[j,1] + vsopJupiterConst.C_Arr2_Jup_R3[j,2] * arg_jt_since2000);
		}

		//R4
		for (int j = 0; j < loc_arrJupiterSizeOfRTerms[4]; j++)
		{
			R4 += vsopJupiterConst.C_Arr2_Jup_R4[j,0] * Math.Cos(vsopJupiterConst.C_Arr2_Jup_R4[j,1] + vsopJupiterConst.C_Arr2_Jup_R4[j,2] * arg_jt_since2000);
		}

		//R5
		for (int j = 0; j < loc_arrJupiterSizeOfRTerms[5]; j++)
		{
			R5 += vsopJupiterConst.C_Arr2_Jup_R5[j,0] * Math.Cos(vsopJupiterConst.C_Arr2_Jup_R5[j,1] + vsopJupiterConst.C_Arr2_Jup_R5[j,2] * arg_jt_since2000);
		}

		// calculate the sum of all R-Terms
		ret_LBR.R = process_RTerms(R0, R1, R2, R3, R4, R5, arg_jt_since2000);



		return ret_LBR;


	}
	public sspher vsop_saturn(double arg_jt_since2000)
	{
		sspher ret_LBR;


		// ------------------------------------------------------------------------------------------------------------------
		// determination of the heliocentric longitude
		double L0 = 0;
		double L1 = 0;
		double L2 = 0;
		double L3 = 0;
		double L4 = 0;
		double L5 = 0;

		// C_Sat_NumberOfLTerms = C_Sat_NumberOfBTerms = C_Sat_NumberOfRTerms
		int[] loc_arrSaturnSizeOfLTerms = { 0, 0, 0, 0, 0, 0 };
		int[] loc_arrSaturnSizeOfBTerms = { 0, 0, 0, 0, 0, 0 };
		int[] loc_arrSaturnSizeOfRTerms = { 0, 0, 0, 0, 0, 0 };
		// TODO: replace this by a call of the defintion in class Settings
		int loc_vsopHighAccuracyAcvivationSwitch = defines.C__vsopHighAccuracyAcvivationSwitch;

		for (int i = 0; i < vsopSaturnConst.C_Sat_NumberOfLTerms; i++)
		{
			if (loc_vsopHighAccuracyAcvivationSwitch == 1) // high accuracy
			{
				loc_arrSaturnSizeOfLTerms[i] = vsopSaturnConst.C_Arr_Sat_SizeOfLTerms[i];
				loc_arrSaturnSizeOfBTerms[i] = vsopSaturnConst.C_Arr_Sat_SizeOfBTerms[i];
				loc_arrSaturnSizeOfRTerms[i] = vsopSaturnConst.C_Arr_Sat_SizeOfRTerms[i];
			}
			else //low accuracy
			{
				loc_arrSaturnSizeOfLTerms[i] = vsopSaturnConst.C_Arr_Sat_SizeOfLTermsTrunc[i];
				loc_arrSaturnSizeOfBTerms[i] = vsopSaturnConst.C_Arr_Sat_SizeOfBTermsTrunc[i];
				loc_arrSaturnSizeOfRTerms[i] = vsopSaturnConst.C_Arr_Sat_SizeOfRTermsTrunc[i];
			}

		}


		//L0
		for (int j = 0; j < loc_arrSaturnSizeOfLTerms[0]; j++)
		{
			L0 += vsopSaturnConst.C_Arr2_Sat_L0[j,0] * Math.Cos(vsopSaturnConst.C_Arr2_Sat_L0[j,1] + vsopSaturnConst.C_Arr2_Sat_L0[j,2] * arg_jt_since2000);
		}

		//L1
		for (int j = 0; j < loc_arrSaturnSizeOfLTerms[1]; j++)
		{
			L1 += vsopSaturnConst.C_Arr2_Sat_L1[j,0] * Math.Cos(vsopSaturnConst.C_Arr2_Sat_L1[j,1] + vsopSaturnConst.C_Arr2_Sat_L1[j,2] * arg_jt_since2000);
		}


		//L2
		for (int j = 0; j < loc_arrSaturnSizeOfLTerms[2]; j++)
		{
			L2 += vsopSaturnConst.C_Arr2_Sat_L2[j,0] * Math.Cos(vsopSaturnConst.C_Arr2_Sat_L2[j,1] + vsopSaturnConst.C_Arr2_Sat_L2[j,2] * arg_jt_since2000);
		}


		//L3
		for (int j = 0; j < loc_arrSaturnSizeOfLTerms[3]; j++)
		{
			L3 += vsopSaturnConst.C_Arr2_Sat_L3[j,0] * Math.Cos(vsopSaturnConst.C_Arr2_Sat_L3[j,1] + vsopSaturnConst.C_Arr2_Sat_L3[j,2] * arg_jt_since2000);
		}

		//L4
		for (int j = 0; j < loc_arrSaturnSizeOfLTerms[4]; j++)
		{
			L4 += vsopSaturnConst.C_Arr2_Sat_L4[j,0] * Math.Cos(vsopSaturnConst.C_Arr2_Sat_L4[j,1] + vsopSaturnConst.C_Arr2_Sat_L4[j,2] * arg_jt_since2000);
		}

		//L5
		for (int j = 0; j < loc_arrSaturnSizeOfLTerms[5]; j++)
		{
			L5 += vsopSaturnConst.C_Arr2_Sat_L5[j,0] * Math.Cos(vsopSaturnConst.C_Arr2_Sat_L5[j,1] + vsopSaturnConst.C_Arr2_Sat_L5[j,2] * arg_jt_since2000);
		}


		// calculate the sum of all L-terms, convert them to degrees and limit them to [0...360]
		ret_LBR.L = process_LTerms(L0, L1, L2, L3, L4, L5, arg_jt_since2000);



		// ------------------------------------------------------------------------------------------------------------------
		// determination of the heliocentric latitude
		double B0 = 0;
		double B1 = 0;
		double B2 = 0;
		double B3 = 0;
		double B4 = 0;
		double B5 = 0;

		//B0
		for (int j = 0; j < loc_arrSaturnSizeOfBTerms[0]; j++)
		{
			B0 += vsopSaturnConst.C_Arr2_Sat_B0[j,0] * Math.Cos(vsopSaturnConst.C_Arr2_Sat_B0[j,1] + vsopSaturnConst.C_Arr2_Sat_B0[j,2] * arg_jt_since2000);
		}

		//B1
		for (int j = 0; j < loc_arrSaturnSizeOfBTerms[1]; j++)
		{
			B1 += vsopSaturnConst.C_Arr2_Sat_B1[j,0] * Math.Cos(vsopSaturnConst.C_Arr2_Sat_B1[j,1] + vsopSaturnConst.C_Arr2_Sat_B1[j,2] * arg_jt_since2000);
		}


		//B2
		for (int j = 0; j < loc_arrSaturnSizeOfBTerms[2]; j++)
		{
			B2 += vsopSaturnConst.C_Arr2_Sat_B2[j,0] * Math.Cos(vsopSaturnConst.C_Arr2_Sat_B2[j,1] + vsopSaturnConst.C_Arr2_Sat_B2[j,2] * arg_jt_since2000);
		}

		//B3
		for (int j = 0; j < loc_arrSaturnSizeOfBTerms[3]; j++)
		{
			B3 += vsopSaturnConst.C_Arr2_Sat_B3[j,0] * Math.Cos(vsopSaturnConst.C_Arr2_Sat_B3[j,1] + vsopSaturnConst.C_Arr2_Sat_B3[j,2] * arg_jt_since2000);
		}

		//B4
		for (int j = 0; j < loc_arrSaturnSizeOfBTerms[4]; j++)
		{
			B4 += vsopSaturnConst.C_Arr2_Sat_B4[j,0] * Math.Cos(vsopSaturnConst.C_Arr2_Sat_B4[j,1] + vsopSaturnConst.C_Arr2_Sat_B4[j,2] * arg_jt_since2000);
		}

		//B5
		for (int j = 0; j < loc_arrSaturnSizeOfBTerms[5]; j++)
		{
			B5 += vsopSaturnConst.C_Arr2_Sat_B5[j,0] * Math.Cos(vsopSaturnConst.C_Arr2_Sat_B5[j,1] + vsopSaturnConst.C_Arr2_Sat_B5[j,2] * arg_jt_since2000);
		}


		// calculate the sum of all B-terms, convert them to degrees and limit them to [-90...90]
		ret_LBR.B = process_BTerms(B0, B1, B2, B3, B4, B5, arg_jt_since2000);


		// ------------------------------------------------------------------------------------------------------------------
		// determination of the sun distance
		double R0 = 0;
		double R1 = 0;
		double R2 = 0;
		double R3 = 0;
		double R4 = 0;
		double R5 = 0;

		//R0
		for (int j = 0; j < loc_arrSaturnSizeOfRTerms[0]; j++)
		{
			R0 += vsopSaturnConst.C_Arr2_Sat_R0[j,0] * Math.Cos(vsopSaturnConst.C_Arr2_Sat_R0[j,1] + vsopSaturnConst.C_Arr2_Sat_R0[j,2] * arg_jt_since2000);
		}

		//R1
		for (int j = 0; j < loc_arrSaturnSizeOfRTerms[1]; j++)
		{
			R1 += vsopSaturnConst.C_Arr2_Sat_R1[j,0] * Math.Cos(vsopSaturnConst.C_Arr2_Sat_R1[j,1] + vsopSaturnConst.C_Arr2_Sat_R1[j,2] * arg_jt_since2000);
		}


		//R2
		for (int j = 0; j < loc_arrSaturnSizeOfRTerms[2]; j++)
		{
			R2 += vsopSaturnConst.C_Arr2_Sat_R2[j,0] * Math.Cos(vsopSaturnConst.C_Arr2_Sat_R2[j,1] + vsopSaturnConst.C_Arr2_Sat_R2[j,2] * arg_jt_since2000);
		}

		//R3
		for (int j = 0; j < loc_arrSaturnSizeOfRTerms[3]; j++)
		{
			R3 += vsopSaturnConst.C_Arr2_Sat_R3[j,0] * Math.Cos(vsopSaturnConst.C_Arr2_Sat_R3[j,1] + vsopSaturnConst.C_Arr2_Sat_R3[j,2] * arg_jt_since2000);
		}

		//R4
		for (int j = 0; j < loc_arrSaturnSizeOfRTerms[4]; j++)
		{
			R4 += vsopSaturnConst.C_Arr2_Sat_R4[j,0] * Math.Cos(vsopSaturnConst.C_Arr2_Sat_R4[j,1] + vsopSaturnConst.C_Arr2_Sat_R4[j,2] * arg_jt_since2000);
		}

		//R5
		for (int j = 0; j < loc_arrSaturnSizeOfRTerms[5]; j++)
		{
			R5 += vsopSaturnConst.C_Arr2_Sat_R5[j,0] * Math.Cos(vsopSaturnConst.C_Arr2_Sat_R5[j,1] + vsopSaturnConst.C_Arr2_Sat_R5[j,2] * arg_jt_since2000);
		}

		// calculate the sum of all R-Terms
		ret_LBR.R = process_RTerms(R0, R1, R2, R3, R4, R5, arg_jt_since2000);



		return ret_LBR;


	}
	public sspher vsop_uranus(double arg_jt_since2000)
	{

		sspher ret_LBR;

		// ------------------------------------------------------------------------------------------------------------------
		// determination of the heliocentric longitude
		double L0 = 0;
		double L1 = 0;
		double L2 = 0;
		double L3 = 0;
		double L4 = 0;
		double L5 = 0;

		// C_Ura_NumberOfLTerms = C_Ura_NumberOfBTerms = C_Ura_NumberOfRTerms
		int[] loc_arrUranusSizeOfLTerms = { 0, 0, 0, 0, 0, 0 };
		int[] loc_arrUranusSizeOfBTerms = { 0, 0, 0, 0, 0, 0 };
		int[] loc_arrUranusSizeOfRTerms = { 0, 0, 0, 0, 0, 0 };

		// TODO: replace this by a call of the defintion in class Settings
		int loc_vsopHighAccuracyAcvivationSwitch = defines.C__vsopHighAccuracyAcvivationSwitch;

		for (int i = 0; i < vsopUranusConst.C_Ura_NumberOfLTerms; i++)
		{
			if (loc_vsopHighAccuracyAcvivationSwitch == 1) // high accuracy
			{
				loc_arrUranusSizeOfLTerms[i] = vsopUranusConst.C_Arr_Ura_SizeOfLTerms[i];
				loc_arrUranusSizeOfBTerms[i] = vsopUranusConst.C_Arr_Ura_SizeOfBTerms[i];
				loc_arrUranusSizeOfRTerms[i] = vsopUranusConst.C_Arr_Ura_SizeOfRTerms[i];
			}
			else //low accuracy
			{
				loc_arrUranusSizeOfLTerms[i] = vsopUranusConst.C_Arr_Ura_SizeOfLTermsTrunc[i];
				loc_arrUranusSizeOfBTerms[i] = vsopUranusConst.C_Arr_Ura_SizeOfBTermsTrunc[i];
				loc_arrUranusSizeOfRTerms[i] = vsopUranusConst.C_Arr_Ura_SizeOfRTermsTrunc[i];
			}

		}


		//L0
		for (int j = 0; j < loc_arrUranusSizeOfLTerms[0]; j++)
		{
			L0 += vsopUranusConst.C_Arr2_Ura_L0[j,0] * Math.Cos(vsopUranusConst.C_Arr2_Ura_L0[j,1] + vsopUranusConst.C_Arr2_Ura_L0[j,2] * arg_jt_since2000);
		}

		//L1
		for (int j = 0; j < loc_arrUranusSizeOfLTerms[1]; j++)
		{
			L1 += vsopUranusConst.C_Arr2_Ura_L1[j,0] * Math.Cos(vsopUranusConst.C_Arr2_Ura_L1[j,1] + vsopUranusConst.C_Arr2_Ura_L1[j,2] * arg_jt_since2000);
		}


		//L2
		for (int j = 0; j < loc_arrUranusSizeOfLTerms[2]; j++)
		{
			L2 += vsopUranusConst.C_Arr2_Ura_L2[j,0] * Math.Cos(vsopUranusConst.C_Arr2_Ura_L2[j,1] + vsopUranusConst.C_Arr2_Ura_L2[j,2] * arg_jt_since2000);
		}


		//L3
		for (int j = 0; j < loc_arrUranusSizeOfLTerms[3]; j++)
		{
			L3 += vsopUranusConst.C_Arr2_Ura_L3[j,0] * Math.Cos(vsopUranusConst.C_Arr2_Ura_L3[j,1] + vsopUranusConst.C_Arr2_Ura_L3[j,2] * arg_jt_since2000);
		}

		//L4
		for (int j = 0; j < loc_arrUranusSizeOfLTerms[4]; j++)
		{
			L4 += vsopUranusConst.C_Arr2_Ura_L4[j,0] * Math.Cos(vsopUranusConst.C_Arr2_Ura_L4[j,1] + vsopUranusConst.C_Arr2_Ura_L4[j,2] * arg_jt_since2000);
		}

		//L5
		for (int j = 0; j < loc_arrUranusSizeOfLTerms[5]; j++)
		{
			L5 += vsopUranusConst.C_Arr2_Ura_L5[j,0] * Math.Cos(vsopUranusConst.C_Arr2_Ura_L5[j,1] + vsopUranusConst.C_Arr2_Ura_L5[j,2] * arg_jt_since2000);
		}


		// calculate the sum of all L-terms, convert them to degrees and limit them to [0...360]
		ret_LBR.L = process_LTerms(L0, L1, L2, L3, L4, L5, arg_jt_since2000);



		// ------------------------------------------------------------------------------------------------------------------
		// determination of the heliocentric latitude
		double B0 = 0;
		double B1 = 0;
		double B2 = 0;
		double B3 = 0;
		double B4 = 0;
		double B5 = 0;

		//B0
		for (int j = 0; j < loc_arrUranusSizeOfBTerms[0]; j++)
		{
			B0 += vsopUranusConst.C_Arr2_Ura_B0[j,0] * Math.Cos(vsopUranusConst.C_Arr2_Ura_B0[j,1] + vsopUranusConst.C_Arr2_Ura_B0[j,2] * arg_jt_since2000);
		}

		//B1
		for (int j = 0; j < loc_arrUranusSizeOfBTerms[1]; j++)
		{
			B1 += vsopUranusConst.C_Arr2_Ura_B1[j,0] * Math.Cos(vsopUranusConst.C_Arr2_Ura_B1[j,1] + vsopUranusConst.C_Arr2_Ura_B1[j,2] * arg_jt_since2000);
		}


		//B2
		for (int j = 0; j < loc_arrUranusSizeOfBTerms[2]; j++)
		{
			B2 += vsopUranusConst.C_Arr2_Ura_B2[j,0] * Math.Cos(vsopUranusConst.C_Arr2_Ura_B2[j,1] + vsopUranusConst.C_Arr2_Ura_B2[j,2] * arg_jt_since2000);
		}

		//B3
		for (int j = 0; j < loc_arrUranusSizeOfBTerms[3]; j++)
		{
			B3 += vsopUranusConst.C_Arr2_Ura_B3[j,0] * Math.Cos(vsopUranusConst.C_Arr2_Ura_B3[j,1] + vsopUranusConst.C_Arr2_Ura_B3[j,2] * arg_jt_since2000);
		}

		//B4
		for (int j = 0; j < loc_arrUranusSizeOfBTerms[4]; j++)
		{
			B4 += vsopUranusConst.C_Arr2_Ura_B4[j,0] * Math.Cos(vsopUranusConst.C_Arr2_Ura_B4[j,1] + vsopUranusConst.C_Arr2_Ura_B4[j,2] * arg_jt_since2000);
		}

		//B5


		// calculate the sum of all B-terms, convert them to degrees and limit them to [-90...90]
		ret_LBR.B = process_BTerms(B0, B1, B2, B3, B4, B5, arg_jt_since2000);


		// ------------------------------------------------------------------------------------------------------------------
		// determination of the sun distance
		double R0 = 0;
		double R1 = 0;
		double R2 = 0;
		double R3 = 0;
		double R4 = 0;
		double R5 = 0;

		//R0
		for (int j = 0; j < loc_arrUranusSizeOfRTerms[0]; j++)
		{
			R0 += vsopUranusConst.C_Arr2_Ura_R0[j,0] * Math.Cos(vsopUranusConst.C_Arr2_Ura_R0[j,1] + vsopUranusConst.C_Arr2_Ura_R0[j,2] * arg_jt_since2000);
		}

		//R1
		for (int j = 0; j < loc_arrUranusSizeOfRTerms[1]; j++)
		{
			R1 += vsopUranusConst.C_Arr2_Ura_R1[j,0] * Math.Cos(vsopUranusConst.C_Arr2_Ura_R1[j,1] + vsopUranusConst.C_Arr2_Ura_R1[j,2] * arg_jt_since2000);
		}


		//R2
		for (int j = 0; j < loc_arrUranusSizeOfRTerms[2]; j++)
		{
			R2 += vsopUranusConst.C_Arr2_Ura_R2[j,0] * Math.Cos(vsopUranusConst.C_Arr2_Ura_R2[j,1] + vsopUranusConst.C_Arr2_Ura_R2[j,2] * arg_jt_since2000);
		}

		//R3
		for (int j = 0; j < loc_arrUranusSizeOfRTerms[3]; j++)
		{
			R3 += vsopUranusConst.C_Arr2_Ura_R3[j,0] * Math.Cos(vsopUranusConst.C_Arr2_Ura_R3[j,1] + vsopUranusConst.C_Arr2_Ura_R3[j,2] * arg_jt_since2000);
		}

		//R4
		for (int j = 0; j < loc_arrUranusSizeOfRTerms[4]; j++)
		{
			R4 += vsopUranusConst.C_Arr2_Ura_R4[j,0] * Math.Cos(vsopUranusConst.C_Arr2_Ura_R4[j,1] + vsopUranusConst.C_Arr2_Ura_R4[j,2] * arg_jt_since2000);
		}

		// calculate the sum of all R-Terms
		ret_LBR.R = process_RTerms(R0, R1, R2, R3, R4, R5, arg_jt_since2000);



		return ret_LBR;


	}
	public sspher vsop_neptun(double arg_jt_since2000)
	{
		sspher ret_LBR;

		// ------------------------------------------------------------------------------------------------------------------
		// determination of the heliocentric longitude
		double L0 = 0;
		double L1 = 0;
		double L2 = 0;
		double L3 = 0;
		double L4 = 0;
		double L5 = 0;

		// C_Nep_NumberOfLTerms = C_Nep_NumberOfBTerms = C_Nep_NumberOfRTerms
		int[] loc_arrNeptunSizeOfLTerms = { 0, 0, 0, 0, 0, 0 };
		int[] loc_arrNeptunSizeOfBTerms = { 0, 0, 0, 0, 0, 0 };
		int[] loc_arrNeptunSizeOfRTerms = { 0, 0, 0, 0, 0, 0 };

		// TODO: replace this by a call of the defintion in class Settings
		int loc_vsopHighAccuracyAcvivationSwitch = defines.C__vsopHighAccuracyAcvivationSwitch;

		for (int i = 0; i < vsopNeptuneConst.C_Nep_NumberOfLTerms; i++)
		{
			if (loc_vsopHighAccuracyAcvivationSwitch == 1) // high accuracy
			{
				loc_arrNeptunSizeOfLTerms[i] = vsopNeptuneConst.C_Arr_Nep_SizeOfLTerms[i];
				loc_arrNeptunSizeOfBTerms[i] = vsopNeptuneConst.C_Arr_Nep_SizeOfBTerms[i];
				loc_arrNeptunSizeOfRTerms[i] = vsopNeptuneConst.C_Arr_Nep_SizeOfRTerms[i];
			}
			else //low accuracy
			{
				loc_arrNeptunSizeOfLTerms[i] = vsopNeptuneConst.C_Arr_Nep_SizeOfLTermsTrunc[i];
				loc_arrNeptunSizeOfBTerms[i] = vsopNeptuneConst.C_Arr_Nep_SizeOfBTermsTrunc[i];
				loc_arrNeptunSizeOfRTerms[i] = vsopNeptuneConst.C_Arr_Nep_SizeOfRTermsTrunc[i];
			}

		}

		//L0
		for (int j = 0; j < loc_arrNeptunSizeOfLTerms[0]; j++)
		{
			L0 += vsopNeptuneConst.C_Arr2_Nep_L0[j,0] * Math.Cos(vsopNeptuneConst.C_Arr2_Nep_L0[j,1] + vsopNeptuneConst.C_Arr2_Nep_L0[j,2] * arg_jt_since2000);
		}

		//L1
		for (int j = 0; j < loc_arrNeptunSizeOfLTerms[1]; j++)
		{
			L1 += vsopNeptuneConst.C_Arr2_Nep_L1[j,0] * Math.Cos(vsopNeptuneConst.C_Arr2_Nep_L1[j,1] + vsopNeptuneConst.C_Arr2_Nep_L1[j,2] * arg_jt_since2000);
		}


		//L2
		for (int j = 0; j < loc_arrNeptunSizeOfLTerms[2]; j++)
		{
			L2 += vsopNeptuneConst.C_Arr2_Nep_L2[j,0] * Math.Cos(vsopNeptuneConst.C_Arr2_Nep_L2[j,1] + vsopNeptuneConst.C_Arr2_Nep_L2[j,2] * arg_jt_since2000);
		}


		//L3
		for (int j = 0; j < loc_arrNeptunSizeOfLTerms[3]; j++)
		{
			L3 += vsopNeptuneConst.C_Arr2_Nep_L3[j,0] * Math.Cos(vsopNeptuneConst.C_Arr2_Nep_L3[j,1] + vsopNeptuneConst.C_Arr2_Nep_L3[j,2] * arg_jt_since2000);
		}

		//L4
		for (int j = 0; j < loc_arrNeptunSizeOfLTerms[4]; j++)
		{
			L4 += vsopNeptuneConst.C_Arr2_Nep_L4[j,0] * Math.Cos(vsopNeptuneConst.C_Arr2_Nep_L4[j,1] + vsopNeptuneConst.C_Arr2_Nep_L4[j,2] * arg_jt_since2000);
		}

		//L5
		for (int j = 0; j < loc_arrNeptunSizeOfLTerms[5]; j++)
		{
			L5 += vsopNeptuneConst.C_Arr2_Nep_L5[j,0] * Math.Cos(vsopNeptuneConst.C_Arr2_Nep_L5[j,1] + vsopNeptuneConst.C_Arr2_Nep_L5[j,2] * arg_jt_since2000);
		}


		// calculate the sum of all L-terms, convert them to degrees and limit them to [0...360]
		ret_LBR.L = process_LTerms(L0, L1, L2, L3, L4, L5, arg_jt_since2000);



		// ------------------------------------------------------------------------------------------------------------------
		// determination of the heliocentric latitude
		double B0 = 0;
		double B1 = 0;
		double B2 = 0;
		double B3 = 0;
		double B4 = 0;
		double B5 = 0;

		//B0
		for (int j = 0; j < loc_arrNeptunSizeOfBTerms[0]; j++)
		{
			B0 += vsopNeptuneConst.C_Arr2_Nep_B0[j,0] * Math.Cos(vsopNeptuneConst.C_Arr2_Nep_B0[j,1] + vsopNeptuneConst.C_Arr2_Nep_B0[j,2] * arg_jt_since2000);
		}

		//B1
		for (int j = 0; j < loc_arrNeptunSizeOfBTerms[1]; j++)
		{
			B1 += vsopNeptuneConst.C_Arr2_Nep_B1[j,0] * Math.Cos(vsopNeptuneConst.C_Arr2_Nep_B1[j,1] + vsopNeptuneConst.C_Arr2_Nep_B1[j,2] * arg_jt_since2000);
		}


		//B2
		for (int j = 0; j < loc_arrNeptunSizeOfBTerms[2]; j++)
		{
			B2 += vsopNeptuneConst.C_Arr2_Nep_B2[j,0] * Math.Cos(vsopNeptuneConst.C_Arr2_Nep_B2[j,1] + vsopNeptuneConst.C_Arr2_Nep_B2[j,2] * arg_jt_since2000);
		}

		//B3
		for (int j = 0; j < loc_arrNeptunSizeOfBTerms[3]; j++)
		{
			B3 += vsopNeptuneConst.C_Arr2_Nep_B3[j,0] * Math.Cos(vsopNeptuneConst.C_Arr2_Nep_B3[j,1] + vsopNeptuneConst.C_Arr2_Nep_B3[j,2] * arg_jt_since2000);
		}

		//B4
		for (int j = 0; j < loc_arrNeptunSizeOfBTerms[4]; j++)
		{
			B4 += vsopNeptuneConst.C_Arr2_Nep_B4[j,0] * Math.Cos(vsopNeptuneConst.C_Arr2_Nep_B4[j,1] + vsopNeptuneConst.C_Arr2_Nep_B4[j,2] * arg_jt_since2000);
		}

		//B5
		for (int j = 0; j < loc_arrNeptunSizeOfBTerms[5]; j++)
		{
			B5 += vsopNeptuneConst.C_Arr2_Nep_B5[j,0] * Math.Cos(vsopNeptuneConst.C_Arr2_Nep_B5[j,1] + vsopNeptuneConst.C_Arr2_Nep_B5[j,2] * arg_jt_since2000);
		}


		// calculate the sum of all B-terms, convert them to degrees and limit them to [-90...90]
		ret_LBR.B = process_BTerms(B0, B1, B2, B3, B4, B5, arg_jt_since2000);


		// ------------------------------------------------------------------------------------------------------------------
		// determination of the sun distance
		double R0 = 0;
		double R1 = 0;
		double R2 = 0;
		double R3 = 0;
		double R4 = 0;
		double R5 = 0;
		//R0
		for (int j = 0; j < loc_arrNeptunSizeOfRTerms[0]; j++)
		{
			R0 += vsopNeptuneConst.C_Arr2_Nep_R0[j,0] * Math.Cos(vsopNeptuneConst.C_Arr2_Nep_R0[j,1] + vsopNeptuneConst.C_Arr2_Nep_R0[j,2] * arg_jt_since2000);
		}

		//R1
		for (int j = 0; j < loc_arrNeptunSizeOfRTerms[1]; j++)
		{
			R1 += vsopNeptuneConst.C_Arr2_Nep_R1[j,0] * Math.Cos(vsopNeptuneConst.C_Arr2_Nep_R1[j,1] + vsopNeptuneConst.C_Arr2_Nep_R1[j,2] * arg_jt_since2000);
		}


		//R2
		for (int j = 0; j < loc_arrNeptunSizeOfRTerms[2]; j++)
		{
			R2 += vsopNeptuneConst.C_Arr2_Nep_R2[j,0] * Math.Cos(vsopNeptuneConst.C_Arr2_Nep_R2[j,1] + vsopNeptuneConst.C_Arr2_Nep_R2[j,2] * arg_jt_since2000);
		}

		//R3
		for (int j = 0; j < loc_arrNeptunSizeOfRTerms[3]; j++)
		{
			R3 += vsopNeptuneConst.C_Arr2_Nep_R3[j,0] * Math.Cos(vsopNeptuneConst.C_Arr2_Nep_R3[j,1] + vsopNeptuneConst.C_Arr2_Nep_R3[j,2] * arg_jt_since2000);
		}

		//R4
		for (int j = 0; j < loc_arrNeptunSizeOfRTerms[4]; j++)
		{
			R4 += vsopNeptuneConst.C_Arr2_Nep_R4[j,0] * Math.Cos(vsopNeptuneConst.C_Arr2_Nep_R4[j,1] + vsopNeptuneConst.C_Arr2_Nep_R4[j,2] * arg_jt_since2000);
		}

		// calculate the sum of all R-Terms
		ret_LBR.R = process_RTerms(R0, R1, R2, R3, R4, R5, arg_jt_since2000);



		return ret_LBR;

	}
	// -------------------------------------------------------------------------------------------------------------------------------------------------




}

