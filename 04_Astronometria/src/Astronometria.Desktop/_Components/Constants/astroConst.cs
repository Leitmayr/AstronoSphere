using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


static class astroConst
{
	// Misc constants
	public const double pi = 3.1415926535897932384626433832795;
	public const double C__lightSpeed_KmPerSecond = 299792.458;
	public const double C__meanObliquityJ2000 = 23.43929111;
	public const double C__ConstantOfAberration = 20.49552; // arc seconds

	public const int C__MIN_PLANET_IDX_INNEN = 1;
	public const int C__MAX_PLANET_IDX_INNEN = 2;
	public const int C__MIN_PLANET_IDX_AUSSEN = 4;
	public const int C__MAX_PLANET_IDX_AUSSEN = 8;


	//Aufgangshöhen verschiedener Gestirne unter Berücksichtigung der Refraktion
	public const double C__H0_STARS = -0.5667;
	public const double C__H0_SUN = -0.8333;
	public const double C__H0_MOON = 0.125; // TODO check this!

	public const double C__SUN_ASTRO_DAEM = -18;
	public const double C__SUN_NAUT_DAEM = -12;
	public const double C__SUN_BUERG_DAEM = -6;

	// Astronomische Distanzen
	public const double AU2KM = 149597870.700; // 1 Astronomische Einheit = 149.6 Mio km
	public const double PAR2LY = 3.262; // 1 parsec = 3.262 LJ
	public const int LJ2AU = 63240;     // 1 LJ hat 63240 Astronomische Einheiten
	public const double PAR2AU = PAR2LY * LJ2AU; // 1 parsec hat ca. 206265 AU

	// Zeitpunkte
	public const double C__JD2000 = 2451545.0;     // J2000.0 is 2000 January 1 at 12h TD corresponding to JDE2451545.0 (Meeus [2], p. 133)
	public const double C__JD2000_0UT = 2451544.5; // this corresponds to 2000 January 1 at 0h TD corresponding = Julian Date at beginning of day
	public const double C__B1950 = 2433282.423;    // Besseljahr 1950.0, Vorgänger des Referenzäquinoktiums J2000.0
	public const double C_J1875 = 2405889.5;       // 1875.0 -> needed for the boundaries of Constellations

	// Zeitdauern
	public const double tage_pro_jahrhundert = 36525;
	public const double tage_pro_jahrtausend = 365250;
	public const int C__daysPerCentury = 36525;
	public const int C__daysPerMillenium = 365250;
	public const int C__sec_per_min = 60;
	public const int C__sec_per_hour = 3600;
	public const int C__sec_per_day = 86400;
	public const int C__deg_per_hour = 15;
	public const int C__hours_per_day = 24;
	public const double tage_pro_jahr = 365.2425;
	public const int C__tage_normaljahr = 365;
	public const int C__tage_schaltjahr = 366;
	public const double C_SiderialDegrees = 360.985647;


	// Planetenradien
	public static readonly double[] C_PLANETENRADIEN = { 2439.7, 6051.8, 6378.137, 3396.1, 71492, 60268, 25559, 24764, 1195 };
	public static readonly double[] C_JUPMONDRADIEN = { 1821.6, 1560.8, 2631.2, 2410.3 }; // Io (I), Europa (II), Ganymed (III), Callisto (IV)
	public const double C_JupiterFlattening = 0.9351256; // das entspricht der Jupiterabplattung von 133708km/142984km;		

	// Definition (B) taken from MEEUS (2nd ed.), p. 390 - Mercury, Venus, Mars, Jupiter, Saturn, Uranus, Neptune
	public static readonly double[] C_PLANETENRADIEN_AU = { 3.36,  8.34,  0, 4.68, 98.44,   82.73,  35.02,  33.5};

	 public const double C_RAD_MERKUR = 2439.7;
	 public const double C_RAD_VENUS = 6051.8;
	 public const double C_RAD_ERDE = 6378.14;
	 public const double C_RAD_MARS = 3396.1;
	 public const double C_RAD_JUPITER = 71492;
	 public const double C_RAD_SATURN = 60268;
	 public const double C_RAD_URANUS = 25559;
	 public const double C_RAD_NEPTUN = 24764;
	 public const double C_RAD_PLUTO = 1195;

	 public const double C_RAD_SONNE = 696000;


	public static readonly double[,] C_PHYSISCHE_EPHEMERIDEN_M0=
{
		{-0.42, 3.8, -2.73, 2},
		{-4.4, 0.09, 2.39, -0.65},
		{0,0,0,0}, // Erde - wird nicht benötigt
		{-1.52, 1.6, 0, 0},
		{-9.4, 0.5, 0, 0},
		{-8.88, 0, 0, 0}, // hier bleibt die Ringöffnung unberücksichtigt
		{-7.19, 0, 0, 0},
		{-6.87, 0, 0, 0},
		{-1.0, 0, 0, 0},
		};

// Mondkonstanten

// Mondradius
	 public const double C_RAD_MOND = 1737.4;
	 public const double C_distEarthMoon = 385000.56;// moon distance (Meeus p. 342)

	//Terme der mittleren Länge
	 public const double L0_JD2000 = 218.31665;     // mittlere Länge des Mondes am 1.1.2000, 0 UT
	 public const double L0_1 = 481267.88134;       // Reihenentwicklung der mittleren Länge, lineares Glied
	 public const double L0_2 = -0.001327;          // Reihenentwicklung der mittleren Länge, quadratisches Glied

	//Terme der mittleren Anomalie des Mondes
	 public const double l_M_JD2000 = 134.96341;    // mittlere Anomale des Mondes am 1.1.2000, 0 UT
	 public const double l_M_1 = 477198.86763;      // Reihenentwicklung der mittleren Anomalie des Mondes, lineares Glied
	 public const double l_M_2 = 0.008997;          // Reihenentwicklung der mittleren Anomalie des Mondes, quadratisches Glied

	//Terme der mittleren Anomalie der Sonne
	 public const double l_S_JD2000 = 357.52911;    // mittlere Anomale der Sonne am 1.1.2000, 0 UT
	 public const double l_S_1 = 35999.05029;       // Reihenentwicklung der mittleren Anomalie der Sonne, lineares Glied
	 public const double l_S_2 = 0.000154;          // Reihenentwicklung der mittleren Anomalie der Sonne, quadratisches Glied

	//mittlerer Abstand des Mondes vom aufsteigenden Knoten
	 public const double F_JD2000 = 93.27210;       //mittlerer Abstand des Mondes vom aufsteigenden Knoten am 1.1.2000, 0 UT
	 public const double F_1 = 483202.01753;            //Reihenentwicklung  mittlerer Abstand des Mondes vom aufsteigenden Knoten, lineares Glied
	 public const double F_2 = -0.003403;           //Reihenentwicklung mittlerer Abstand des Mondes vom aufsteigenden Knoten, quadratisches Glied

	//mittlerer Abstand Mond-Sonne
	 public const double D_JD2000 = 297.85020;      //mittlerer Abstand Mond-Sonne am 1.1.2000, 0 UT
	 public const double D_1 = 445267.11152;        //Reihenentwiclkung mittlerer Abstand Mond-Sonne, lineares Glied
	 public const double D_2 = -0.001630;           //Reihenentwiclkung mittlerer Abstand Mond-Sonne, quadratisches Glied


	// Sonnenkonstanten
	//Venusanteil
	 public const double G2_0 = 49.943;             //mittlere Anomalie der Venus, Konstantglied
	 public const double G2_1 = 58517.493;          //mittlere Anomalie der Venus, lineares Glied
													
	//Marsanteil
	 public const double G4_0 = 19.557;             //mittlere Anomalie des Mars, Konstantglied
	 public const double G4_1 = 19139.977;          //mittlere Anomalie des Mars, lineares Glied
													
	//Jupiteranteil
	 public const double G5_0 = 19.863;             //mittlere Anomalie des Jupiters, Konstantglied
	 public const double G5_1 = 3034.583;           //mittlere Anomalie des Jupiters, lineares Glied
	 public const double G5_0s = 173.58;            //mittlere Anomalie des Jupiters, Konstantglied im sin-Term
	 public const double G5_1s = 39.80;             //mittlere Anomalie des Jupiters, lineares Glied im sin-Term
	 public const double G5s = 1300 / 3600;         //mittlere Anomalie des Jupiters, Faktor vor sin-Termin in Grad
													
	//Saturnanteil
	 public const double G6_0 = 317.394;            //mittlere Anomalie des Saturns, Konstantglied
	 public const double G6_1 = 1221.794;           //mittlere Anomalie des Saturns, lineares Glied
											
	//Winkelbeziehung Sonne-Mond
	 public const double DD_0 = 297.852;            //mittlerer Winkelabstand des Mondes von der Sonne (mittlere Mondlänge - mittlere Sonnenlänge) - Konstantglied
	 public const double DD_1 = 445267.114;			//mittlerer Winkelabstand des Mondes von der Sonne (mittlere Mondlänge - mittlere Sonnenlänge) - lineares Glied
											
	//mittlere Anomalie des Mondes
	 public const double A_0 = 134.954;             //mittlere Anomalie des Mondes - Kontantglied
	 public const double A_1 = 477198.849;          //mittlere Anomalie des Mondes - lineares Glied
											
	//mittleres Argument der Breite des Mondes (Abstand vom Mondknoten)
	 public const double U_0 = 93.276;              //mittleres Argument der Breite des Mondes (Abstand vom Mondknoten) - Kontantglied
	 public const double U_1 = 483202.025;          //mittleres Argument der Breite des Mondes (Abstand vom Mondknoten) - lineares Glied
											
	//Differenz zwischen wahrer und mittlerer Sonnenlänge nach der Mittelpunktsgleichung
	 public const double Delta_L_10 = 6892.817 / 3600;  //Differenz zwischen wahrer und mittlerer Sonnenlänge - Konstantes Glied im sin(G)-Term
	 public const double Delta_L_11 = 17.240 / 3600;    //Differenz zwischen wahrer und mittlerer Sonnenlänge - lineares Glied im sin(G)-Term in Grad
	 public const double Delta_L_20 = 71.977 / 3600;    //Differenz zwischen wahrer und mittlerer Sonnenlänge - Konstantes Glied im sin(2G)-Term
	 public const double Delta_L_21 = 0.361 / 3600;     //Differenz zwischen wahrer und mittlerer Sonnenlänge - lineares Glied im sin(2G)-Term in Grad
	 public const double Delta_L_30 = 1.054 / 3600;     //Differenz zwischen wahrer und mittlerer Sonnenlänge - Konstantes Glied im sin(3G)-Term


	// Jupiter Ephemeris
	public const double JupiterRotationSys1 = 877.9;	
	public const double JupiterRotationSys2 = 870.27;   // relevant for GRS

	public const double daysForOneRevolutionInSys2 = 1 / (870.27 / 360);

	public const double GRSLongitude = 50;					// value from 1.1.24. source https://www.projectpluto.com/grs_lon.txt
	public const double ReferenceJDOfGSRLong = 2460310.5;   // value from 1.1.24. source https://www.projectpluto.com/grs_lon.txt
	public const double GRSAvgMovingRatePerDay = 0.038353;		// empirical value determined from JUPOS (14° per year): 14/365= 0.038353°/day.
	 public static readonly double[] C_GRS = { 24000/2, 13000/2 }; // {Width, Height} in km
	public static readonly double C_GRSLATITUDE = 22.1;         // Latitude of the GRS in the Jupiter System II; unit is deg



}

