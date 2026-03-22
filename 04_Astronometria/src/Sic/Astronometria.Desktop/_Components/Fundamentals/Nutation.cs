using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Nutation : conversions
{

	protected double _deltaPsi;           /**>nutation in longitude in arc seconds (seconds of a degree) */
	protected double _deltaEpsilon;       /**>nutation in obliquity in arc seconds (seconds of a degree) */


	/**
	 * \brief calculates the nutation based on Meeus, Astronomical Algorithms, 2nd edition, chapter 22, pages 144ff.
	 * The method sets the attributes _deltaPsi and _deltaEpsilon. Both resultls are in arc seconds (seconds of a degree)
	 * \@param arg_milleniaSince2000 Millenia since J2000.0 (1.1.2000)
	 */


	// ========================= GET & SET FUNCTIONS ========================================
	public double getNutationInLongitude
	{
		get { return _deltaPsi; }
	}

	public double getNutationInObliquity
	{
		get { return _deltaEpsilon; }
	}

	public void setNutation( czeit arg_When)
	{
		//TODO Wie geht das in C#?		
		
		//czeit time = new czeit(arg_When);
		double locJD = arg_When.JD;
		double loc_time = czeit.millenniaSinceJ2000(locJD);
		calcNutation(loc_time);
	}

// ========================= CONSTRUCTORS ========================================
	public Nutation( czeit arg_When )	
	{
		//TODO Wie geht das in C#?		
		//czeit time = new czeit(arg_When);
		double locJD = arg_When.JD;
		double loc_time = czeit.centuriesSinceJ2000(locJD);
		calcNutation(loc_time);
	}
	
	~Nutation() { }



// ========================= METHODS ========================================
	public void calcNutation( double arg_milleniaSince2000 )
	{
		// calculations of this method taken from Meeus, Astronomical Algorithms, 2nd edition, chapter 22, pages 144ff.

		double D = 0;
		double M = 0;
		double M_ = 0;
		double F = 0;
		double Omega = 0;

		double argument = 0;

		double T = arg_milleniaSince2000;

		// Mean elongation of the moon from the sun
		// D = 297.850336 + 445267.111480*T - 0.0019142*T*T + T*T*T/189474;
		D = 297.850336 + T * (445267.111480 + T * (-0.0019142 + T / 189474));
		D = ohne_ueberlauf_degrees(D);

		// Mean anomaly of the Sun (Earth)
		M = 357.52772 + T * (35999.050340 - T * (0.0001603 + T / 300000));
		M = ohne_ueberlauf_degrees(M);

		// Mean anomaly of the Moon
		M_ = 134.96298 + T * (477198.867398 + T * (0.0086972 + T / 56250));
		M_ = ohne_ueberlauf_degrees(M_);

		// Moon's argument of latitude
		F = 93.27191 + T * (483202.017538 + T * (-0.0036825 + T / 327270));
		F = ohne_ueberlauf_degrees(F);

		// Longitude of the ascending node of the Moon's mean orbit on the ecliptic measured from the mean equinox of the date
		Omega = 125.04452 + T * (-1934.136261 + T * (0.0020708 + T / 450000));
		Omega = ohne_ueberlauf_degrees(Omega);


		for ( int i = 0; i < 63; i++)
		{
			argument = nutationConst.C_nutationArgument[i, 0] * D
						  + nutationConst.C_nutationArgument[i, 1] * M
						  + nutationConst.C_nutationArgument[i, 2] * M_
						  + nutationConst.C_nutationArgument[i, 3] * F
						  + nutationConst.C_nutationArgument[i, 4] * Omega;

			argument = ohne_ueberlauf_degrees(argument);

			_deltaPsi += (nutationConst.sinCoefficient[i, 0] + nutationConst.sinCoefficient[i, 1] * T) / 10000 * Math.Sin( Math.PI / 180 * argument);
			_deltaEpsilon += (nutationConst.cosCoefficient[i, 0] + nutationConst.cosCoefficient[i, 1] * T) / 10000 * Math.Cos( Math.PI / 180 * argument);

		}
	}

}

