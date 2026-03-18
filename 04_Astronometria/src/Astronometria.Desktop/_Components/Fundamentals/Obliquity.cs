using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Obliquity
{
	/**
	 * \class  Obliquity
	 * \brief  describes the obliquity of the ecliptic
	 * 		   Base classes: none
	 *
	 * Description:
	 * accurate star coordinates are only possible by knowing the obliquity of the ecliptic. The class Obliquity calculates mean and true obliquity by means of
	 * equations taken from Jean Meeus, Astronomical Algorithms, 2nd edition, chapter 22, pages 147f. The employed equations are accurate over a wide time range.
	 * Care must be taken outside of that range. Results become meaningless there.
	 */


	// ========================= ATTRIBUTES ========================================
	protected double _meanObliquity;
	protected double _trueObliquity;

	// ========================= GET FUNCTIONS ========================================
	public double getMeanObliquity
	{
		get { return _meanObliquity; }
	}

	public double getTrueObliquity
	{
		get { return _trueObliquity; }
	}

	// ========================= CONSTRUCTORS ========================================
	public Obliquity( czeit arg_When )
	{
		_meanObliquity = calculateMeanObliquity(arg_When); 
		_trueObliquity = calculateTrueObliquity(_meanObliquity, arg_When);
	}

	

	// ========================= METHODS ========================================
	protected double calculateMeanObliquity( czeit arg_When )
	{
		// ------------------------------------------------------------------------------------------------
		double ret_meanObliquity = 0;
		double loc_meanObliquityDelta = 0;
		// ------------------------------------------------------------------------------------------------



		// ------------------------------------------------------------------------------------------------
		// centuries since J2000.0
		// TODO Wie geht das in C#?
		double locJD = arg_When.JD;
		double T = czeit.centuriesSinceJ2000(locJD);
		// time since J2000.0 normalized to ten thousand years
		double U = T / 100;
		// ------------------------------------------------------------------------------------------------



		// ------------------------------------------------------------------------------------------------
		// two methods are suggested (Meeus, 2nd edition, page 147):
		// in the time range [-8000 ... 12000] we can use Laskar's approximation, outside of this range the values are completely wrong
		// outside [-8000 ... 12000] we stick to the definition of the International Astronomical Union. The accuracy within [-8000....12000] is less
		// than Laskar's approximation, but the error outside is less, too
		// [-8000 ... 12000] == | U | < 1
		if ((U >= -1.0) && (U <= 1.0))
		{
			// Laskar's approximation of epsilon0: J. Laska, Astonomy and Astrophysics, Vol. 157, page 68 (1986)
			loc_meanObliquityDelta = // Meeus (22.3)
				U * (-4680.93 + U * (-1.55 + U * (1999.25 + U * (-51.38 + U * (-249.67 + U * (-39.05 + U * (7.12 + U * (27.87 + U * (5.79 + 2.45 * U)))))))));
		}
		else
		{
			// approximation suggested by the IAU
			loc_meanObliquityDelta = T * (-46.8150 + T * (-0.00059 + 0.001813 * T)); // Meeus (22.2)
		}
		// ------------------------------------------------------------------------------------------------



		// ------------------------------------------------------------------------------------------------
		// handle overflow
		if (loc_meanObliquityDelta > 0)
		{
			while (loc_meanObliquityDelta > astroConst.C__sec_per_hour)
				loc_meanObliquityDelta -= astroConst.C__sec_per_hour;
		}
		else
		{
			while (loc_meanObliquityDelta < -astroConst.C__sec_per_hour)
				loc_meanObliquityDelta += astroConst.C__sec_per_hour;
		}
		// ------------------------------------------------------------------------------------------------


		// mean obliquity is the one at 2000.0 corrected by the delta term (normalized to hours)
		ret_meanObliquity = astroConst.C__meanObliquityJ2000 + loc_meanObliquityDelta / astroConst.C__sec_per_hour;

		return (ret_meanObliquity);

	}

	protected double calculateTrueObliquity( double arg_meanObliquity, czeit arg_When )
	{
		double ret_trueObliquity = 0;

		Nutation nutationInObliquity = new Nutation(arg_When);

		ret_trueObliquity = arg_meanObliquity + nutationInObliquity.getNutationInObliquity/ astroConst.C__sec_per_hour;

		return (ret_trueObliquity);

	}





}




