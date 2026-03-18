/**
 * \file cinterp.h
 * \author Marcus Hiemer
 * \date 2012-06-16
 * \brief class definition for the class cinterp
 *
 * cinterp is an abstract class defining various interpolation tasks.
 *
 * Change Log:
 * - 2015-01-22: initial revision
 * - 2012-04-06: update
 * - 2015-06-16: changed return value of interp_y3_n(.) from long double to double
*/



#ifndef CINTERP_H_
#define CINTERP_H_

#include <cstddef>
#include <iostream>

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
private:
	/**
	 *
	 * @param x_achse Vektor mit x-Achsen Stuetzstellen fuer die Interpolation
	 * @param y_achse Vektor mit y-Achsen Stuetzstellen fuer die Interpolation
	 * @param dim_arr Dimension des uebergebenen Stuetzstellen Arrays
	 * @param xinterp x-Stuetzstelle, fuer den der Wert y interpoliert werden soll
	 * @param int_breite Intervallbreite, wichtig fuer den Faktor n
	 * @return structure s_intfact mit den Differenzwerten a, b, c sowie dem ermittelten Interpolationsfaktor n
	 */
	static s_intfact3 calc_diff(const double *x_achse, const double *y_achse, const size_t &dim_arr, const double &xinterp, const double &int_breite);
	// ----------------------------------------------------------------------------------------------------------------------------



	/**
	*
	 * \brief Handelt Ueberlaeufe in den uebergebenen zu interpolierenden Vektoren, z.B. bei [357 359 1 2 4]
	* @param y_achse Vektor mit den zu interpolierenden y-Werten. Kein const, da Werte veraendert werden, sollte es zu einem Ueberlauf
	* kommen
	* @param MAX_VAL maximal moeglicher Wert in y, z.B. bei Rekaszensionen 24h, bei Gradwerten 360, etc.
	* @param dim Dimension des uebergebenen y-Vektors
	*/
	static void handle_ueberlauf(double y_achse[], double MAX_VAL, size_t dim);
	// ----------------------------------------------------------------------------------------------------------------------------



public:
	/**
	 * \brief Ermittelt den interpolierten y-Wert fuer eine gewuenschte Stuetzstelle
	 * @param x_achse Vektor mit x-Achsen Stuetzstellen fuer die Interpolation
	 * @param y_achse Vektor mit y-Achsen Stuetzstellen fuer die Interpolation
	 * @param dim_arr Dimension des uebergebenen Stuetzstellen Arrays
	 * @param xinterp x-Stuetzstelle, fuer den der Wert y interpoliert werden soll
	 * @param int_breite Intervallbreite, wichtig fuer den Faktor n
	 * @return structure swpaar mit dem interpolierten Wertepaar x*, y*
	 */
	static swpaar interp_y3(const double x_achse[], const double y_achse[], const size_t &dim_arr, const double &xinterp, const double &int_breite);
	// ----------------------------------------------------------------------------------------------------------------------------



	/**
	 * \brief Ermittelt den x-Wert fuer einen Nulldurchgang
	 * @param x_achse Vektor mit x-Achsen Stuetzstellen fuer die Interpolation
	 * @param y_achse Vektor mit y-Achsen Stuetzstellen fuer die Interpolation
	 * @param dim_arr Dimension des uebergebenen Stuetzstellen Arrays
	 * @param int_breite Intervallbreite, wichtig fuer den Faktor n
	 * @return structure swpaar mit dem interpolierten Wertepaar x*, 0
	 */
	static swpaar interp_null3(const double x_achse[], const double y_achse[], const size_t &dim_arr, const double &int_breite);
	// ----------------------------------------------------------------------------------------------------------------------------



	/**
	 * * \brief Ermittelt fuer einen Extremwert die Werte x*, y*
	 * @param x_achse Vektor mit x-Achsen Stuetzstellen fuer die Interpolation
	 * @param y_achse Vektor mit y-Achsen Stuetzstellen fuer die Interpolation
	 * @param dim_arr Dimension des uebergebenen Stuetzstellen Arrays
	 * @param int_breite Intervallbreite, wichtig fuer den Faktor n
	 * @return structure swpaar mit dem interpolierten Wertepaar x*, y*, wobei y* ein Extremwert ist
	 */
	static swpaar interp_ext3(const double x_achse[], const double y_achse[], const size_t &dim_arr, const double &int_breite);
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
	static double interp_y3_n(const double x_achse[], const double y_achse[], const size_t &dim_arr, const long double &n);
	// ----------------------------------------------------------------------------------------------------------------------------
};


#endif /* CINTERP_H_ */
