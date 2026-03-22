/**
 * \file czeit.h
 * \author Marcus Hiemer
 * \date 2011-11-16
 * \brief Klassendefinition fuer die Klasse czeit
 *
 * Klasse fuer Zeitobjekte. Beim Erstellen der Objekte werden automatisch die Elemente, Datum, Uhrzeit und vor allem Julianisches
 * Datum erzeugt.
 * 
 * Change log:
 * 2021-04-05:
 * - added bool schaltjahr
*/


#ifndef CZEIT_H_
#define CZEIT_H_

//#include <vector>
#include <list>
#include <iterator>
#include <stdlib.h>
#include <math.h>
#include <iostream>
#include <ctime>
#include "CONST/astro_const.h"
#include "CONST/defines.h"
#include "CONST/calender_const.h"

#include "conversions/conversions.h"
#include "observer.h"
#include "observable.h"

#include <iostream>
using namespace std;


class cbeo;
//class observer;

/**
 * \class 	czeit
 * \brief 	czeit = class "zeit"
 * 			kapselt die Eigenschaften der Zeit (Datum, Uhrzeit, JD)
 * 			Basisklassen: -
 *
 * Klasse fuer Zeitobjekte. Beim Erstellen der Objekte werden automatisch die Elemente, Datum, Uhrzeit und vor allem Julianisches
 * Datum erzeugt. Methoden:
 * - konvertiert Gregorianisches in Julianisches Datum und umgekehrt.
 * - legt Objekte des gegenwaertigen Zeitpunkts an (Standardkonstruktor)
 * - berechnet das JD des Tagesanfangs (JD0)
 * - stellt als static Methode die Tage seit JD2000.0 bereit
*/
class czeit : public observable, public conversions
{
protected:
	double JD;			/**< JD Julianisches Datum */
	double JD0;			/**< JD0 Julianisches Datum des Tages um 0:00h, also zu Tagesbeginn */
	double JDE; 		/**< JDE Julian Date considering TD */
	list<observer*> obs_list;
	bool schaltjahr;
	/**
	 * \brief Methode konvertiert ein Julianisches in ein Gregorianisches Datum. Keine Argumente, benutzt also die
	 * Attribute der Klasse und setzt auch die errechneten Attribute
	 * @return kein Return, schreibt direkt in die Attribute
	 */
	sdatum JD2GD(const double &arg_JD);
	// -------------------------------------------------------------------------------------------------------



	/**
	 * \brief Methode konvertiert ein Gregorianisches in ein Julianisches Datum.
	 * @param arg_day Tag
	 * @param arg_month Monat
	 * @param arg_year Jahr
	 * @param arg_time_double Zeit in Fließkommadarstellung
	 * @return Julianisches Datum
	 */
	double GD2JD(const int &arg_day, const int &arg_month, const int &arg_year, const double &arg_time_double);
	// -------------------------------------------------------------------------------------------------------

	/**
	 * \brief berechnet das JD des Tagesanfangs in Bezug auf ein übergebenes JD
	 * @param JDdouble Julianisches Datum allgemein, also zu einer beliebigen Tageszeit
	 * @return JF Julianisches Datum des Tagesanfangs um 0:00 h
	 */
	double calc_JD0(const double &arg_JDdouble);
	// -------------------------------------------------------------------------------------------------------


private:
	szeit uhrzeit;  	/**< uhrzeit enthaelt die Datenelemente fuer das Datum: Tag, Monat, Jahr als Ganzzahlen */
	sdatum datum;		/**< enthaelt die Datenelemente fuer das Datum: Tag, Monat, Jahr als Ganzzahlen */
	szeit TD_zeit; 		/**< uhrzeit in TD */
	sdatum TD_dat; 		/**< datum in TD */
	double Delta_T;		/**< [s] Abweichung zwischen Dynamischer Zeit (TT) und Weltzeit (UT): Delta_T = TT - UT */


	void JD2all(const double &arg_JDatum);

	/**
	 * \brief now() bestimmt Datum und Uhrzeit zum aktuellen Zeitpunkt gemaess Systemuhr (Achtung Sommerzeit!).
	 * Methode setzt berechnet *nicht* uhr_comma automatisch
	 */
	void now(void);
	// -------------------------------------------------------------------------------------------------------



	/**
	 * \brief Fuer ein bestimmtes jahr wird der Index der Delta_T LUT zurueckgegeben
	 * @param jahr Jahreszahl fuer die der Tabellenindex bestimmt werden soll
	 * @return Index der Delta_T LUT
	 */
	unsigned int calc_DeltaT_LUT_ind(const int &arg_year);
	//
	// -------------------------------------------------------------------------------------------------------


	void check_input(void);


public:
	virtual void notifyObserver();
	virtual void registerObserver(observer *Observer);
	virtual void removeObserver(observer *Observer);

	// spätere Schnittstelle für QT SLOT?
	void set_JD(const double &JDatum);



	// Konstruktoren

	/**
	 * Standardkonstruktor
	 * \brief verwendet die Methode now() zum Bestimmen des aktuellen Systemuhr-Zeitpunkts, berechnet damit automatisch das JD und JD0
	 * und setzt alle notwendigen Attribute
	 */
	czeit();
	// -------------------------------------------------------------------------------------------------------

	/**
		 * Konstruktor
		 * \brief berechnet JD0 aus dem uebergebenen JD, errechnet automatisch Datum und Uhzeit und setzt die entsprechenden Attribute
		 * \@param JDatum Julianisches Datum zur Objekterzeugung
		 */
	czeit(const double &JDatum);
	// -------------------------------------------------------------------------------------------------------


	/**
	 * Konstruktor
	 * \brief setzt die Attribute gemaess der Uebergabewerte und berechnet automatisch die Uhrzeit in Kommadarstelung sowie JD und JD0
	 * @param tag Tag des Datums
	 * @param monat Monat des Datums
	 * @param jahr Jahr des Datums
	 * @param hour Stunde gemaess Uebergabe
	 * @param min Minute gemaess Uebergabe
	 * @param sek Sekunde gemaess Uebergabe
	 */
	czeit(const int &tag, const int &monat, const int &jahr, const int &hour, const int &min, const int &sek);
	// -------------------------------------------------------------------------------------------------------


	/**
	 * Konstruktor
	 * \brief berechnet zunaechst mittels now() das aktuelle Datum und Uhrzeit. Ueberschreibt danach die Uhrzeit mit den Uebergabeparametern.
	 * D.h.: bekommt Stunde, Min, Sek uebergeben und setzt das Datum auf das aktuelle Datum gemaess Systemuhr
	 * @param hour  Stunde gemaess Uebergabe
	 * @param min  Minute gemaess Uebergabe
	 * @param sek Sekunde gemaess Uebergabe
	 */
	//czeit(const int &hour, const int &min, const int &sek);
	// -------------------------------------------------------------------------------------------------------

	/**
	 * \brief Die meisten Berechnungen liefern Daten und Uhrzeiten in der dynamischen Zeit zurueck. Dieser Konstruktor
	 * erstellt ein czeit Objekt mit Hilfe der TD-Daten und rechnet daraus die UT-Zeiten
	 * @param TD_zeit Zeit der dynamischen Zeit
	 * @param TD_datum Datum der dynamischen Zeit
	 */
	czeit(const szeit &TD_zeit, const sdatum &TD_datum);
	// -------------------------------------------------------------------------------------------------------

	//czeit(const double &zeit_comma);

	// Destruktor
	/**
	 * \brief virtueller Destkruktor ~ceit()
	 */
	virtual ~czeit(void);
	// -------------------------------------------------------------------------------------------------------


	// Operatoren
	/**
	 * \brief ueberladener Ausgabeoperator, gibt Datum und Uhrzeit aus
	 * @param os outstream mit der Ausgabe fuer Objekte der Klasse czeit
	 * @param zeitpunkt der ausgegeben werden soll
	 * @return der ostream zur Ausgabe eines czeit-Objekts
	 */
	friend ostream& operator<<(ostream & os, const czeit & zeitpunkt);
	// -------------------------------------------------------------------------------------------------------


	// public methods

	/**
	 * \brief berechnet anhand von tag, monat, jahr, um den wie vielten Tag im Jahr es sich handelt
	 * @param jahr Jahr
	 * @param monat Monat
	 * @param tag Tag
	 * @return Tag im Jahr jahr
	 */
	unsigned int day_of_the_year(const int &jahr, const int &monat, const int &tag);
	// -------------------------------------------------------------------------------------------------------


	/**
	 * \brief berechnet aus tag.monat.jahr eine Kommazahl, z.B. 30.6.2012 = 2012.5
	 * @param year Jahreszahl (int)
	 * @param month Monat (int)
	 * @param day Tag (int)
	 * @return Datum als Kommazahl bzgl. Jahr
	 */
	double calc_jahr_comma(const int &year, const int &month, const int &day);
	//
	// -------------------------------------------------------------------------------------------------------




	/**
	 * \brief berechnet fuer ein Jahr die Anzahl der Tage (Schaltjahr: 366, Gemeinjahr: 365)
	 * @param arg_year Jahreszahl (int)
	 * @return Anzahl der Tage des uebergebenen Jahres
	 */
	unsigned int get_no_days_that_year(const int &arg_year);
	//
	// -------------------------------------------------------------------------------------------------------


	/**
	 * \brief liefert zurueck, ob das Jahr jahr ein Schaltjahr war (return 1) oder ein Gemeinjahr (return 0)
	 * @param jahr
	 * @return Schaltjahr (1) oder Gemeinjahr (0)
	 */
	bool is_schaltjahr(const int &arg_year);
	//
	// -------------------------------------------------------------------------------------------------------


	/**
	 *
	 *\brief liefert fuer ein bestimmtes Julianisches Datum den Wert fuer Delta_T zurueck
	 * @param arg_JulDat Julianisches Datum, fuer das der Wert von Delta T bestimmt werden soll
	 * @return delta_T in sekunden
	 */
	double calc_DT(const double &arg_JulDat);
	// -------------------------------------------------------------------------------------------------------



	/**
	 * \brief statische Methode: haeufig werden in allen moeglichen Rechnungen die Tage seit J2000.0 benoetigt. Diese
	 * statische Methode liefert fuer ein angefragtes JD an, wie viele Tage seit J2000.0 vergangen sind
	 * @param arg_JD Julianisches Datum, fuer das berechnet werden soll, wie viele Tage es seit J2000.0 waren
	 * @return Anzahl der Tage seit J2000.0
	 */
	static double days_since_J2000(const double & arg_JD);
	// -------------------------------------------------------------------------------------------------------



	/**
	 * \brief statische Methode: haeufig werden in allen moeglichen Rechnungen die Jahrhunderte seit J2000.0 benoetigt. Diese
	 * statische Methode liefert fuer ein angefragtes JD an, wie viele Jahrhunderte seit J2000.0 vergangen sin
	 * @param arg_JD Julianisches Datum, fuer das berechnet werden soll, wie viele Jahrhunderte es seit J2000.0 waren
	 * @return Anzahl der Jahrhunderte seit J2000.0, kleiner Null wenn das Jahr vor 2000.0 war
	 */
	static double centuries_since_J2000(const double & arg_JD);
	// -------------------------------------------------------------------------------------------------------



	/**
	 * \brief statische Methode: in den Rechnungen der VSOP werden die Jahrtausende seit J2000.0 benoetigt. Diese
	 * statische Methode liefert fuer ein angefragtes JD an, wie viele Jahrtausende seit J2000.0 vergangen sin
	 * @param arg_JD Julianisches Datum, fuer das berechnet werden soll, wie viele Jahrtausende es seit J2000.0 waren
	 * @return Anzahl der Jahrtausende seit J2000.0, kleiner Null wenn das Jahr vor 2000.0 war
	 */
	static double millennia_since_J2000(const double & arg_JD);
	// -------------------------------------------------------------------------------------------------------



	/**
	 * \brief Konvertiert eine in Stunden:Minuten:Sekunden angegebene zeit in seine Gleitkommadarstellung
	 * @param std Uhrzeit (Stundenwert)
	 * @param hmin Uhrzeit (Minutenwert)
	 * @param hsec Uhrzeit (Stundenwert)
	 * @see double2hour()
	 * @return Kommawert der uebergebenen Uhrzeit
	 */
	//double hms2double(const int &std, const int &hmin, const int &hsec);
	// -------------------------------------------------------------------------------------------------------


	/**
	 * \brief konstanter getter der die Uhrzeit in Gleitkommadarstellung liefert
	 * @return Uhrzeit in Gleitkommadarstellung
	 */
	double get_zeit(void) const;
	// -------------------------------------------------------------------------------------------------------


	/**
	 * \brief konstanter getter der die Uhrzeit in Stunde, Minute, Sekunde liefert (Datentyp szeit)
	 * @return Uhrzeit in UT in Stunde, Minute, Sekunde
	 */
	szeit get_UT_hms(void) const;
	// -------------------------------------------------------------------------------------------------------



	/**
	 * \brief konvertiert eine in Gleitkommadarstellung uebergebene Uhrzeit in
	 * @param hdoub Uebergabewert der Uhrzeit in Gleitkommadarstellung
	 * @param hms int-Arraymit Stunde: hms[0], Minute: hms[1], Sekunde: hms[2]
	 * @see hms2double()
	 * @return int-Array mit Stunde: hms[0], Minute: hms[1], Sekunde: hms[2]
	 */
	//void double2hour(const double &hdoub, int hms[3]);
	// -------------------------------------------------------------------------------------------------------




	/**
	 * \brief konstanter getter der das JD des Tagesanfangs zurueckgibt (JD0)
	 * @return JD des Tagesanfangs in Gleitkomma
	 */
	double get_JD0(void) const;
	// -------------------------------------------------------------------------------------------------------



	/**
	 * \brief konstanter getter der das JD zurueckgibt (JD)
	 * @return JD des Objekts
	 */
	double get_JD(void) const;
	// -------------------------------------------------------------------------------------------------------



	/**
	 * \brief konstanter getter der den Tag des Datums des Objekts zurueckgibt
	 * @return Tag des Datums des Objekts
	 */
	int get_tag(void) const;
	// -------------------------------------------------------------------------------------------------------



	/**
	 * \brief konstanter getter der den Monat des Datums des Objekts zurueckgibt
	 * @return Monat des Datums des Objekts
	 */
	int get_monat(void) const;
	// -------------------------------------------------------------------------------------------------------



	/**
	 * \brief konstanter getter der das Jahr des Datums des Objekts zurueckgibt
	 * @return Jahr des Datums des Objekts
	 */
	int get_jahr(void) const;
	// -------------------------------------------------------------------------------------------------------



	/**
	 * \brief konstanter getter der die Stunde der Uhrzeit des Objekts zurueckgibt
	 * @return Stunde der Uhrzeit des Objekts
	 */
	int get_stunde(void) const;
	// -------------------------------------------------------------------------------------------------------




	/**
	 * \brief konstanter getter der die Minute der Uhrzeit des Objekts zurueckgibt
	 * @return Stunde der Minute des Objekts
	 */
	int get_min(void) const;
	// -------------------------------------------------------------------------------------------------------



	/**
	 * \brief konstanter getter der die Sekunde der Uhrzeit des Objekts zurueckgibt
	 * @return Sekunde der Uhrzeit des Objekts
	 */
	int get_sek(void) const;
	// -------------------------------------------------------------------------------------------------------



	/**
	 * \brief liefert interpolierten Schaetzwert fuer Delta_T zurueck. Wert entnommen aus Tabellen
	 * @return interpolierten Schaetzwert fuer Delta_T zurueck
	 */
	double get_Delta_T(void) const;
	// -------------------------------------------------------------------------------------------------------



	/**
	 * \brief liefert die Dynamische Zeit als Struktur zurueck
	 * @return Dynamische Zeit als Struktur
	 */
	szeit get_TD_zeit(void) const;
	// -------------------------------------------------------------------------------------------------------



	/**
	 * \brief liefert das Datum der Dynamischen Zeit als Struktur zurueck
	 * @return Datum der Dynamischen Zeit als Struktur
	 */
	sdatum get_TD_datum(void) const;
	// -------------------------------------------------------------------------------------------------------



	/**
	 * \brief liefert das Datum der Zeit als Struktur zurueck
	 * @return Datum der Zeit als Struktur
	 */
	sdatum get_datum(void) const;
	// -------------------------------------------------------------------------------------------------------


	/**
	 * \brief returns Julian Date considering Delta_T
	 * @return JDE as a float
	 */
	inline double get_JDE(void) const;

};

// =============== inline-Definitionen =========================

inline szeit czeit::get_UT_hms(void) const
{
	return(uhrzeit);
}

inline sdatum czeit::get_datum(void) const
{
	return(datum);
}

inline szeit czeit::get_TD_zeit(void) const
{
	return(TD_zeit);
}

inline sdatum czeit::get_TD_datum(void) const
{
	return(TD_dat);
}


inline int czeit::get_stunde(void) const
{
	return(uhrzeit.stunde);
}
// -------------------------------------------------------------------------------------------------------



inline int czeit::get_min(void) const
{
	return(uhrzeit.minute);
}
// -------------------------------------------------------------------------------------------------------



inline int czeit::get_sek(void) const
{
	return(uhrzeit.sekunde);
}
// -------------------------------------------------------------------------------------------------------



inline double czeit::get_Delta_T(void) const
{
	return(Delta_T);
}


inline double czeit::get_zeit(void) const
{
	return(uhrzeit.uhr_comma);
}

inline int czeit::get_tag(void) const
{
	return(datum.tag);
}

inline int czeit::get_monat(void) const
{
	return(datum.monat);
}

inline int czeit::get_jahr(void) const
{
	return(datum.jahr);
}

inline double czeit::get_JD0(void) const
{
	return(JD0);
}


inline double czeit::get_JD(void) const
{
	return(JD);
}


inline double czeit::get_JDE(void) const
{
	return(JDE);
}


#endif /* CZEIT_H_ */
