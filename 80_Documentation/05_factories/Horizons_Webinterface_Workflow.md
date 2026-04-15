# Astronometria -- Nutzung des JPL Horizons Web-Interfaces (Helio Level 0)

## Zweck

Dieses Dokument beschreibt, wie Helio-Events für die
Ephemeris-Regression über das JPL Horizons Web-Interface abgefragt und
gespeichert werden.

Die Abfragen erfolgen über die offizielle Horizons REST-API:

https://ssd.jpl.nasa.gov/api/horizons.api

------------------------------------------------------------------------

# 1. Ziel der Abfragen

Für jedes HelioEvent sollen folgende Daten erzeugt werden:

-   Zeitfenster: JD_center ± 1.5 Tage
-   Schrittweite: 1 Stunde
-   Referenzzentrum: Sonne (@10)
-   Referenzebene: Ekliptik
-   Vektor-Korrekturen: NONE
-   Ausgabe: Position + Geschwindigkeit (X,Y,Z,VX,VY,VZ)
-   Einheiten: AU-D
-   Format: CSV

------------------------------------------------------------------------

# 2. Benötigte Parameter

Folgende Parameter müssen im API-Request gesetzt werden:

  Parameter    Wert
  ------------ ----------------------------------
  COMMAND      Planetencode (z.B. 499 für Mars)
  CENTER       @10
  MAKE_EPHEM   YES
  EPHEM_TYPE   VECTORS
  START_TIME   Kalenderzeit (UTC)
  STOP_TIME    Kalenderzeit (UTC)
  STEP_SIZE    1h
  REF_PLANE    ECLIPTIC
  VECT_CORR    NONE
  OUT_UNITS    AU-D
  CSV_FORMAT   YES
  OBJ_DATA     NO

------------------------------------------------------------------------

# 3. Zeitkonvertierung

Horizons API erwartet Kalenderzeiten.

Daher muss das Julianische Datum (JD_TT) in eine UTC-Zeit im Format:

YYYY-MM-DD HH:MM

konvertiert werden.

Beispiel:

JD 2460830.58 → 2026-06-04T13:55

Wichtig ist hierbei das Datumsformat nach ISO 8601: kein Leerzeichen 
zwischen Datum und Uhrzeit!

------------------------------------------------------------------------

# 4. Beispiel-API-Aufruf

Beispiel für Mars (Helio Level 0):

https://ssd.jpl.nasa.gov/api/horizons.api? COMMAND=499 &CENTER=@10
&MAKE_EPHEM=YES &EPHEM_TYPE=VECTORS &START_TIME=2026-06-03 22:00
&STOP_TIME=2026-06-05 02:00 &STEP_SIZE=1h &REF_PLANE=ECLIPTIC
&VECT_CORR=NONE &OUT_UNITS=AU-D &CSV_FORMAT=YES &OBJ_DATA=NO

Dieser Aufruf liefert eine CSV-Tabelle mit:

-   JD
-   X, Y, Z
-   VX, VY, VZ

------------------------------------------------------------------------

# 5. Speicherung

Die Rohdaten werden gespeichert unter:

/EphemerisRegression/Horizons/Helio/RawResults/

Dateinamensschema:

`<Planet>`{=html}*`<Testsuite>`{=html}*`<Event>`{=html}\_Level0.csv

Beispiel:

Mars_TS-A_L180_Level0.csv

------------------------------------------------------------------------

# 6. Weiterverarbeitung

Nach Speicherung:

1.  CSV-Datei parsen
2.  Event-Feinzeit bestimmen
3.  Referenz-StateVector erzeugen
4.  JSON-Datei für NUnit-Tests schreiben

------------------------------------------------------------------------

Ende des Dokuments.
