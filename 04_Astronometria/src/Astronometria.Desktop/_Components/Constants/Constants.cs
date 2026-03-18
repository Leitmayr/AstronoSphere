using System;

static class Const
{

	public const double C__JD2000_0UT = 2451544.5;      // this corresponds to 2000 January 1 at 0h TD corresponding = Julian Date at beginning of day
	public const double C__JD2000 = 2451545.0;     // J2000.0 is 2000 January 1 at 12h TD corresponding to JDE2451545.0 (Meeus [2], p. 133)
	public static readonly string[] C__WT = { "Montag", "Dienstag", "Mittwoch", "Donnerstag", "Freitag", "Samstag", "Sonntag" };
	public const int C__YearZeroOfGregorianCalendar = 1582;
	public const int C__numberOfDaysLeapYear = 366;
	public const int C__numberOfDaysStandardYear = 365;
	public const int C__daysPerCentury = 36525;
	public const int C__daysPerMillenium = 365250;

	public static readonly int[] C__days_per_month_njahr = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
	public static readonly int[] C__days_per_month_sjahr = { 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

	// Dynamical Time constants
	public const int C__DynTime_LUT_start = 1620;
	public const double C__DynTime_1600 = 140.6;
	public const double C__DynTime_1620 = 124;
	public const int C_Max_DynLUT_ind = 186;

	public static readonly double[] DTLUT  =
		{
		 124,     15,   106,   98,   91,   85,    79,   74,   70,   65,
		  62,     58,    55,   53,   50,   48,    46,   44,   42,   40,
		  37,     35,    33,   31,   28,   26,    24,   22,   20,   18,
		  16,     14,    13,   12,   11,   10,     9,    9,    9,    9,
		   9,      9,     9,    9,   10,   10,    10,   10,   10,   11,
		  11,     11,    11,   11,   11,   11,    11,   12,   12,   12,
		  12,     12,    13,   13,   13,   13,    14,   14,   14,   15,
		  15,     15,    15,   16,   16,   16,    16,   16,   17,   17,
		  17,     17,    17,   17,   17,   17,    16,   16,   15,   14,
		  13,   13.1,  12.7, 12.5, 12.5, 12.5,  12.5, 12.5, 12.5, 12.3,
		  12,   11.4,  10.6,  9.6,  8.6,  7.5,   6.6,  6.0,  5.7,  5.6,
		  5.0,   5.9,   6.2,  6.5,  6.8,  7.1,   7.3,  7.5,  7.7,  7.8,
		  7.0,   7.5,   6.4,  5.4,  2.9,  1.6,  -1.0, -2.7, -3.6, -4.7,
		 -5.4,  -5.2,  -5.5, -5.6, -5.8, -5.9,  -6.2, -6.4, -6.1, -4.7,
		   -2,   0.0,   2.6,  5.4,  7.7, 10.5,  13.4,   16, 18.2, 20.2,
		   21,  22.4,  23.5, 23.9, 24.3, 24.3,  23.9, 23.9, 23.7, 24.0,
		   24,  25.3,  26.2, 27.3, 28.2, 29.1,  30.0, 30.7, 31.4, 32.2,
		   33,  34.0,  35.0, 36.5, 38.3, 40.2,  42.2, 44.5, 46.5, 48.5,
		   50,  52.2,  53.8, 54.9, 55.8, 56.9,  58.3
		};


	// ============================================	
	//					Seasons
	// ============================================
	//


	// -------------------- MITTLERE TERME --------------------

	public static readonly double[,] C_MEAN0 =
	{
		{1721139.29189, 365.13740, 0.06134, 0.00111, 0.00071},
		{1721233.25401, 365241.72562, -0.05323, 0.00907, 0.00025},
		{1721325.70455, 365242.49558, -0.11677, -0.00297, 0.00074},
		{1721414.39987, 365242.88527, -0.00769, -0.00933, -0.00006},

	};

	public static readonly double[,] C_MEAN2000 =
	{
		{2451623.80984, 365242.37404, 0.05169, -0.00411, -0.00057},
		{2451716.56767, 365241.62603, 0.00325, 0.00888, -0.00030},
		{2451810.21715, 365242.01767, -0.11575, 0.00337, 0.00078},
		{2451900.05952, 365242.74049, -0.06223, -0.00823, 0.00032},

	};


	// -------------------- PERIODISCHE TERME --------------------

	// für die Berechnung von S = Summe(A*cos(B+C*T)) gemäß Tabelle 26.C in Meeus, Astronomical Algorithms, S. 167
	public static readonly int[] C_A = { 485, 203, 199, 182, 156, 136, 77, 74, 70, 58, 52, 50, 45, 44, 29, 18, 17, 16, 14, 12, 12, 12, 9, 8 };

	public static readonly double[] C_B = {324.96, 337.23, 342.08, 27.85, 73.14, 171.52, 222.54, 296.72, 243.58, 119.81, 297.17, 21.02,
							247.54, 325.15, 60.93, 155.12, 288.79, 198.04, 199.76, 95.39, 287.11, 320.81, 227.73, 15.45};

	public static readonly double[] C_C = 
	{
		1934.136, 32964.467, 20.186, 445267.112, 45036.886, 22518.443, 65928.934, 
		3034.906, 9037.513, 33718.147, 150.678, 2281.226,
		29929.562, 31555.956, 4443.417, 67555.328, 4562.452, 
		62894.029, 31436.921, 14577.848, 31931.756, 34777.259, 
		1222.114, 16859.074
	};




}

