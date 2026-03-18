using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class defines
{


	public const int C__vsopHighAccuracyAcvivationSwitch = 1; // 0: less high, 1: high
	public const int C__vsopTimeSelectionSwitch = 0; // 0: time in UT, 1: time in TD

	public static readonly string [] C__PLANET_NAMES = { "Merkur", "Venus", "Erde", "Mars", "Jupiter", "Saturn", "Uranus", "Neptun", "Pluto" };

	public const int C_PLANET_IDX_MERCURY = 1;
	public const int C_PLANET_IDX_VENUS = 2;
	public const int C_PLANET_IDX_EARTH = 3;
	public const int C_PLANET_IDX_MARS = 4;
	public const int C_PLANET_IDX_JUPITER = 5;
	public const int C_PLANET_IDX_SATURN = 6;
	public const int C_PLANET_IDX_URANUS = 7;
	public const int C_PLANET_IDX_NEPTUNE = 8;

	public const int C__BE_NUMBER_OF_EPOCHS = 11;
	public const int C__BE_NUMBER_OF_PLANETS = 9;
	public const int C__NUMBER_OF_MESSIER = 110;

	public const int C__DynTime_LUT_start = 1620;
	public const double C__DynTime_1600 = 140.6;
	public const double C__DynTime_1620 = 124;
	public const int C_Max_DynLUT_ind = 186;

	// DTLUT: Lookup Tabelle der Abweichung Dynamische Zeit-Weltzeit
	public static readonly double[] DTLUT =
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



}

