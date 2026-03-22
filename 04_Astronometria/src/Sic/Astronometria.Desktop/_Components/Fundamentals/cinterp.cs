using System;
/**
 * \file cinterp.cs
 * \author Marcus Hiemer
 * \date 2026-01-12
 * \brief implementiert die Klasse czeit
 *
 * Change Log:
 * - 2026-01-12: transferred from C++ to C#
 *
 * - 2015-01-22: initial revision
 * - 2015-06-16: changed return value of interp_y3_n(.) from long double to double
*/




/**
 * \struct swpaar
 * \brief = " Struktur Wertepaar" - Wertepaar mit zwei double Variablen, die einen interpolierten 2D-Punkt darstellen
 */
public struct swpaar
{
	public double xinterp;      /**< enthalten die interpolierten x-Achsenwerte */
	public double yinterp; 	/**< enthalten die interpolierten y-Achsenwerte */
};

/**
 * \struct s_intfact3
 * \brief Interpolationsfaktoren einer auf drei Stuetzstellen basierenden Interpolation
 */
public struct s_intfact3
{
	public double n; /**< Interpolationsfaktor, i.d.R. gilt -1 < n < 1:
						-1: interpolierter Wert ist y[0],
						 0: interpolierter Wert ist y[1],
						+1: interpolierter Wert ist y[2]
						andere Werte fuer n bedeuten entsprechend dazwischen liegende Interpolationswerte */
	public double a; /**< Differenzwert: a = y[1] - y[0] */
	public double b; /**< Differenzwert: b = y[2] - y[1] */
	public double c; /**< Differenzwert: c = a - b = y[2] + y[0] - 2*y[1] */
};

/**
 * \class cinterp
 * \brief enthaelt einige Interpolationsfunktionen gemaess Meeus Kapitel 3
 */
class cinterp 
{

	public static void handle_ueberlauf(double [] y_achse, double MAX_VAL, int dim)
	{
		double[] diff_y = new double [dim-1]; // Differenzarray zur Ermittlung der Sprungstelle
		for (int i = 1; i < dim; i++)
		{
			// Pruefung auf Ueberlauf: wenn Vorgaengerwert deutlich groesser oder deutlich kleiner war als aktueller Wert, dann Ueberlauf
			// Differenzbildung: bei Ueberlauf ist die Betragsdifferenz der y_werte besonders gross
			// wenn Betragsdifferenz groesser ist als die Haelfte des maximal moeglichen Werts wird dies als Ueberlauf gewertet
			// dann wird noch unterschieden, ob der aktuelle Wert (Schleifenindex i) groesser ist als der letzte. Falls ja
			// wird der letzte (i-1) um den MAX_VAL vergroessert (Fall rechtlaeufige Bewegung). Falls der aktuelle (i) groesser
			// ist als der letzte, koennte der Ueberlauf z.B. durch eine ruecklaeufige Bewegung hervorgerufen worden sein. Dann
			// wird der letzte Wert erhoeht
			//
			// Ziel: im Array stehen dann stets Werte die keinen Sprung aufweisen, die also in der Naehe des MAX_VAL liegen.
			diff_y[i-1] = y_achse[i] - y_achse[i-1];
			diff_y[i-1] = diff_y[i-1] < 0 ? -diff_y[i-1] : diff_y[i-1];
			if (diff_y[i-1] > MAX_VAL/2)
			{
				if (y_achse[i] > y_achse[i-1]) // Fall ruecklaeufige Bewegung, z.B. y = [1 359 2] -> [361 359 362]
					y_achse[i-1] += MAX_VAL;
				else 							// Fall rechtlaeufige Bewegung., z.B. y = [359 1 2] -> [359 361 362]
					y_achse[i] += MAX_VAL;
			}
		}
	}

	public static double interp_y3_n( double [] x_achse,  double [] y_achse,  int dim_arr,  double n)
	{

		double [] xlok = new double[dim_arr];
		double [] ylok = new double[dim_arr];

		for (int i = 0; i < dim_arr; i++)
		{
				xlok[i] = x_achse[i];
				ylok[i] = y_achse[i];
		}
		handle_ueberlauf(ylok, 360, 3);

		s_intfact3 int_help;
		int_help = cinterp.calc_diff( xlok,  ylok, dim_arr, 0, 1);
		int_help.n = n;
		//cout << endl << "Geh ich hier ueberhaupt rein???";

		//Meeus S. 25, Gln. (3.3)
		double ret_help = y_achse[1]+n/2*(int_help.a+int_help.b+n*int_help.c);
		if (ret_help > 360)
			return(ret_help - 360);
		else
			return(ret_help);
	}


	public static swpaar interp_y3( double [] x_achse,  double [] y_achse,  int dim_arr,  double xinterp,  double int_breite)
	{
		swpaar int_paar;

		if (xinterp > x_achse[dim_arr-1])
		{
			int_paar.xinterp = x_achse[dim_arr-1];
			int_paar.yinterp = y_achse[dim_arr-1];
			return(int_paar);
		}
		else if (xinterp < x_achse[0])
		{
			int_paar.xinterp = x_achse[0];
			int_paar.yinterp = y_achse[0];
			return(int_paar);
		}
		else
		{
			s_intfact3 int_help;
			int_help = cinterp.calc_diff( x_achse,  y_achse, dim_arr, xinterp, int_breite);

			int_paar.xinterp= xinterp;
			// Meeus S.25,  Gln. (3.3)
			int_paar.yinterp = y_achse[1] + int_help.n/2*(int_help.a + int_help.b + int_help.n * int_help.c);
			return(int_paar);
		}
	}


	public static swpaar interp_ext3( double[] x_achse,  double[] y_achse,  int dim_arr,  double int_breite)
	{
		swpaar int_paar;
		s_intfact3 int_help;
		int_help = cinterp.calc_diff( x_achse,  y_achse, dim_arr, 0, 0);

		// Meeus S.25, Gln. (3.4)
		int_paar.yinterp = y_achse[1]- (int_help.a+int_help.b)*(int_help.a+int_help.b)/(8*int_help.c);

		// Meeus S.25, Gln. (3.5)
		double n_m = -(int_help.a + int_help.b)/(2*int_help.c);

		int_paar.xinterp = x_achse[1] + n_m*int_breite;
		return(int_paar);
	}

	public static swpaar interp_null3( double [] x_achse,  double [] y_achse,  int dim_arr, double int_breite)
	{

		double [] yaxis = new double[dim_arr];
		for (int i = 0; i< dim_arr; i++)
		{
			yaxis[i] = 3600*y_achse[i];
			//cout << endl << "yaxis[" << i << "] =" << yaxis[i];
		}

		swpaar int_paar;
		int_paar.yinterp = 0.0;

		s_intfact3 int_help;
		int_help = cinterp.calc_diff( x_achse,  yaxis, dim_arr, 0, 0);

		double n0 = 0;
		double n_alt = 0;
		double delta_n;
		double eps = 0.00001;
		int schleifenzaehler = 0;
		do {
			schleifenzaehler++;
			//cout << endl << "Durchlauf #" << schleifenzaehler;

			// Meeus S.26, Gln. (3.6)
			n0 = -2*yaxis[1]/(int_help.a + int_help.b + int_help.c*n_alt);
			delta_n = n0 - n_alt;
			if (delta_n < 0) delta_n = -delta_n;
			//cout << ", n0: " << n0 << ", |delta_n| = "<< delta_n <<endl ;
			n_alt = n0;
		} while (delta_n > eps && schleifenzaehler < 10);

		int_paar.xinterp = x_achse[1]+n0*int_breite;
		return(int_paar);
	}

	public static s_intfact3 calc_diff( double [] x_achse,  double [] y_achse,  int dim_arr,  double xinterp, double int_breite)
	{
		s_intfact3 diffs;
		//cout << endl << "calc_diff: Springe ich hier rein?";
		if (dim_arr < 3)
		{
			diffs.n = 0;
			diffs.a = 0;
			diffs.b = 0;
			diffs.c = 1;
			return(diffs);
		}
		else
		{
			double [] diff_y = new double[dim_arr-1];
			double [] ddiff_y = new double [dim_arr-2];

			// Berechne Mittenindex
			int mitten_idx = (int) (dim_arr-1-(dim_arr-1)/2);
			//cout << endl << "Mitten-Index: " << mitten_idx;


			for (int i = 0; i < dim_arr-1; i++)
			{
				diff_y[i] =  y_achse[i+1] - y_achse[i];
			}

			for (int i = 0; i < dim_arr-2; i++)
			{
				ddiff_y[i] = diff_y[i+1] - diff_y[i];
			}
			diffs.a = diff_y[0];
			diffs.b = diff_y[1];
			diffs.c = ddiff_y[0];
			// interpolation factor (Meeus)
			if (int_breite == 0)
				diffs.n = 1;
			else
			{
				diffs.n = (xinterp - x_achse[mitten_idx])/int_breite;
			}


			return(diffs);


		}

	}

}

