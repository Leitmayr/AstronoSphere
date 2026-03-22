/**
 * \file cinterp.cpp
 * \author Marcus Hiemer
 * \date 2015-06-16
 * \brief implementiert die Klasse czeit
 *
 * Change Log:
 * - 2015-01-22: initial revision
 * - 2015-06-16: changed return value of interp_y3_n(.) from long double to double
*/

using namespace std;




/**
 * \struct swpaar
 * \brief = " Struktur Wertepaar" - Wertepaar mit zwei double Variablen, die einen interpolierten 2D-Punkt darstellen
 */
struct swpaar
{
	double xinterp;  	/**< enthalten die interpolierten x-Achsenwerte */
	double yinterp; 	/**< enthalten die interpolierten y-Achsenwerte */
};

/**
 * \struct s_intfact3
 * \brief Interpolationsfaktoren einer auf drei Stuetzstellen basierenden Interpolation
 */
struct s_intfact3
{
	double n; /**< Interpolationsfaktor, i.d.R. gilt -1 < n < 1:
						-1: interpolierter Wert ist y[0],
						 0: interpolierter Wert ist y[1],
						+1: interpolierter Wert ist y[2]
						andere Werte fuer n bedeuten entsprechend dazwischen liegende Interpolationswerte */
	double a; /**< Differenzwert: a = y[1] - y[0] */
	double b; /**< Differenzwert: b = y[2] - y[1] */
	double c; /**< Differenzwert: c = a - b = y[2] + y[0] - 2*y[1] */
};

/**
 * \class cinterp
 * \brief enthaelt einige Interpolationsfunktionen gemaess Meeus Kapitel 3
 */
class cinterp
{
		/**
		 *
		 * @param x_achse Vektor mit x-Achsen Stuetzstellen fuer die Interpolation
		 * @param y_achse Vektor mit y-Achsen Stuetzstellen fuer die Interpolation
		 * @param dim_arr Dimension des uebergebenen Stuetzstellen Arrays
		 * @param xinterp x-Stuetzstelle, fuer den der Wert y interpoliert werden soll
		 * @param int_breite Intervallbreite, wichtig fuer den Faktor n
		 * @return structure s_intfact mit den Differenzwerten a, b, c sowie dem ermittelten Interpolationsfaktor n
		 */
		protected static s_intfact3 calc_diff(const double *x_achse, const double *y_achse, const size_t &dim_arr, const double &xinterp, const double &int_breite);
		// ----------------------------------------------------------------------------------------------------------------------------



		/**
		*
		 * \brief Handelt Ueberlaeufe in den uebergebenen zu interpolierenden Vektoren, z.B. bei [357 359 1 2 4]
		* @param y_achse Vektor mit den zu interpolierenden y-Werten. Kein const, da Werte veraendert werden, sollte es zu einem Ueberlauf
		* kommen
		* @param MAX_VAL maximal moeglicher Wert in y, z.B. bei Rekaszensionen 24h, bei Gradwerten 360, etc.
		* @param dim Dimension des uebergebenen y-Vektors
		*/
		protected static void handle_ueberlauf(double y_achse[], double MAX_VAL, size_t dim);
		// ----------------------------------------------------------------------------------------------------------------------------



		/**
		 * \brief Ermittelt den interpolierten y-Wert fuer eine gewuenschte Stuetzstelle
		 * @param x_achse Vektor mit x-Achsen Stuetzstellen fuer die Interpolation
		 * @param y_achse Vektor mit y-Achsen Stuetzstellen fuer die Interpolation
		 * @param dim_arr Dimension des uebergebenen Stuetzstellen Arrays
		 * @param xinterp x-Stuetzstelle, fuer den der Wert y interpoliert werden soll
		 * @param int_breite Intervallbreite, wichtig fuer den Faktor n
		 * @return structure swpaar mit dem interpolierten Wertepaar x*, y*
		 */
		public static swpaar interp_y3(const double x_achse[], const double y_achse[], const size_t &dim_arr, const double &xinterp, const double &int_breite);
		// ----------------------------------------------------------------------------------------------------------------------------



		/**
		 * \brief Ermittelt den x-Wert fuer einen Nulldurchgang
		 * @param x_achse Vektor mit x-Achsen Stuetzstellen fuer die Interpolation
		 * @param y_achse Vektor mit y-Achsen Stuetzstellen fuer die Interpolation
		 * @param dim_arr Dimension des uebergebenen Stuetzstellen Arrays
		 * @param int_breite Intervallbreite, wichtig fuer den Faktor n
		 * @return structure swpaar mit dem interpolierten Wertepaar x*, 0
		 */
		public static swpaar interp_null3(const double x_achse[], const double y_achse[], const size_t &dim_arr, const double &int_breite);
		// ----------------------------------------------------------------------------------------------------------------------------



		/**
		 * * \brief Ermittelt fuer einen Extremwert die Werte x*, y*
		 * @param x_achse Vektor mit x-Achsen Stuetzstellen fuer die Interpolation
		 * @param y_achse Vektor mit y-Achsen Stuetzstellen fuer die Interpolation
		 * @param dim_arr Dimension des uebergebenen Stuetzstellen Arrays
		 * @param int_breite Intervallbreite, wichtig fuer den Faktor n
		 * @return structure swpaar mit dem interpolierten Wertepaar x*, y*, wobei y* ein Extremwert ist
		 */
		public static swpaar interp_ext3(const double x_achse[], const double y_achse[], const size_t &dim_arr, const double &int_breite);
		// ----------------------------------------------------------------------------------------------------------------------------



		/**
		 * \brief interpoliert den Wert der y-Achse, wenn der Faktor n uebergeben wird
		 * @param x_achse Achse mit i.d.R. Zeit-Werten
		 * @param y_achse Achse mit Werten, fur die interpoliert werden soll
		 * @param dim_arr Dimension des y-Achsenvektors
		 * @param n Anteil eines Intervalls zwischen y2 und y3 (n>0) bzw. y1 und y2 (n<0), andere Repraesentation des x-Achsenabschnitts, fuer
		 * den y* berechnet werden soll
		 * @return interpolierter y-Wert
		 */
		public static double interp_y3_n(const double x_achse[], const double y_achse[], const size_t &dim_arr, const long double &n);
		// ----------------------------------------------------------------------------------------------------------------------------


	public void handle_ueberlauf(double y_achse[], double MAX_VAL, size_t dim)
	{
		double diff_y[dim-1]; // Differenzarray zur Ermittlung der Sprungstelle
		for (size_t i = 1; i < dim; i++)
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

	public double interp_y3_n(const double x_achse[], const double y_achse[], const size_t &dim_arr, const long double &n)
	{

		double xlok[dim_arr];
		double ylok[dim_arr];

		for (size_t i = 0; i < dim_arr; i++)
		{
				xlok[i] = x_achse[i];
				ylok[i] = y_achse[i];
		}
		handle_ueberlauf(ylok, 360, 3);

		s_intfact3 int_help;
		int_help = cinterp::calc_diff( xlok,  ylok, dim_arr, 0, 1);
		int_help.n = n;
		//cout << endl << "Geh ich hier ueberhaupt rein???";

		//Meeus S. 25, Gln. (3.3)
		double ret_help = y_achse[1]+n/2*(int_help.a+int_help.b+n*int_help.c);
		if (ret_help > 360)
			return(ret_help - 360);
		else
			return(ret_help);
	}


	public swpaar interp_y3(const double x_achse[], const double y_achse[], const size_t &dim_arr, const double &xinterp, const double &int_breite)
	{
		swpaar int_paar;

		if (xinterp > x_achse[dim_arr-1])
		{
			int_paar.xinterp = x_achse[dim_arr-1];
			int_paar.yinterp = y_achse[dim_arr-1];
			cout << endl << "+ + + Exc1: Interpolationsstützstelle außerhalb Zeitintervall (zu gross). Wähle rechten Intervallrand. + + +";
			return(int_paar);
		}
		else if (xinterp < x_achse[0])
		{
			int_paar.xinterp = x_achse[0];
			int_paar.yinterp = y_achse[0];
			cout << endl << "+ + + Exc2: Interpolationsstützstelle außerhalb Zeitintervall (zu klein). Wähle linken Intervallrand. + + +";
			return(int_paar);
		}
		else
		{
			s_intfact3 int_help;
			int_help = cinterp::calc_diff( x_achse,  y_achse, dim_arr, xinterp, int_breite);

			int_paar.xinterp= xinterp;
			// Meeus S.25,  Gln. (3.3)
			int_paar.yinterp = y_achse[1] + int_help.n/2*(int_help.a + int_help.b + int_help.n * int_help.c);
			return(int_paar);
		}
	}


	public swpaar interp_ext3(const double x_achse[], const double y_achse[], const size_t &dim_arr, const double &int_breite)
	{
		swpaar int_paar;
		s_intfact3 int_help;
		int_help = cinterp::calc_diff( x_achse,  y_achse, dim_arr, 0, 0);

		// Meeus S.25, Gln. (3.4)
		int_paar.yinterp = y_achse[1]- (int_help.a+int_help.b)*(int_help.a+int_help.b)/(8*int_help.c);

		// Meeus S.25, Gln. (3.5)
		double n_m = -(int_help.a + int_help.b)/(2*int_help.c);

		int_paar.xinterp = x_achse[1] + n_m*int_breite;
		return(int_paar);
	}

	public swpaar interp_null3(const double x_achse[], const double y_achse[], const size_t &dim_arr, const double &int_breite)
	{

		double yaxis[dim_arr];
		for (size_t i = 0; i< dim_arr; i++)
		{
			yaxis[i] = 3600*y_achse[i];
			//cout << endl << "yaxis[" << i << "] =" << yaxis[i];
		}

		swpaar int_paar;
		int_paar.yinterp = 0;

		s_intfact3 int_help;
		int_help = cinterp::calc_diff( x_achse,  yaxis, dim_arr, 0, 0);

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

	public s_intfact3 calc_diff(const double *x_achse, const double *y_achse, const size_t &dim_arr, const double &xinterp, const double &int_breite)
	{
		s_intfact3 diffs;
		//cout << endl << "calc_diff: Springe ich hier rein?";
		if (dim_arr < 3)
		{
			diffs.n = 0;
			diffs.a = 0;
			diffs.b = 0;
			diffs.c = 1;
			cout << endl << "Zu wenige Stützstellen, keine Interpolation möglich.";
			return(diffs);
		}
		else
		{
			double diff_y[dim_arr-1];
			double ddiff_y[dim_arr-2];

			// Berechne Mittenindex
			int mitten_idx = static_cast<int> (dim_arr-1-(dim_arr-1)/2);
			//cout << endl << "Mitten-Index: " << mitten_idx;


			for (size_t i = 0; i < dim_arr-1; i++)
			{
				diff_y[i] = y_achse[i+1] - y_achse[i];
			}

			for (size_t i = 0; i < dim_arr-2; i++)
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

