# TS-D Valid Ranges for Heliozentrische Horizons VECTORS (@10)

Diese Tabelle dokumentiert die tatsächlich von der JPL Horizons API
(format=text, EPHEM_TYPE=VECTORS, CENTER=@10) bereitgestellten gültigen
Zeitbereiche für heliozentrische Planeten-State-Vektoren.

Die Grenzen wurden experimentell durch API-Abfragen bestimmt (Stand:
2026-02-24).


## Ermittelte Zeitbereiche mit  Horizons API

Dokumentation der min. und max. Daten *alle Planeten Ranges*:

Range Mercury:
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=199&EPHEM_TYPE=VECTORS&START_TIME=JD0.5&STOP_TIME=JD2305507.5&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> Beginn bei JD0
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=199&EPHEM_TYPE=VECTORS&START_TIME=JD9000000&STOP_TIME=JD10000D&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> No ephemeris for target "Mercury" after A.D. 9999-DEC-30 12:00:00.0000 TDB

Range Venus:
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=299&EPHEM_TYPE=VECTORS&START_TIME=JD0.5&STOP_TIME=JD2305507.5&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> Beginn bei JD0
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=299&EPHEM_TYPE=VECTORS&START_TIME=JD9000000&STOP_TIME=JD10000D&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> No ephemeris for target "Venus" after A.D. 9999-DEC-30 12:00:00.0000 TDB

Range Earth:
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=399&EPHEM_TYPE=VECTORS&START_TIME=JD0.5&STOP_TIME=JD2305507.5&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> Beginn bei JD0

https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=399&EPHEM_TYPE=VECTORS&START_TIME=JD9000000&STOP_TIME=JD10000D&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> No ephemeris for target "Earth" after A.D. 9999-DEC-30 12:00:00.0000 TDB

Range Mars:
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=499&EPHEM_TYPE=VECTORS&START_TIME=JD2305447.5&STOP_TIME=JD2305507.5&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> No ephemeris for target "Mars" prior to A.D. 1600-JAN-02 00:00:00.0000 TDB
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=499&EPHEM_TYPE=VECTORS&START_TIME=JD9000000&STOP_TIME=JD10000D&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> No ephemeris for target "Mars" after A.D. 2600-JAN-01 00:00:00.0000 TDB

Range Jupiter:
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=599&EPHEM_TYPE=VECTORS&START_TIME=JD2305447.5&STOP_TIME=JD2305507.5&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> No ephemeris for target "Jupiter" prior to A.D. 1600-JAN-11 00:00:00.0000 TDB
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=599&EPHEM_TYPE=VECTORS&START_TIME=JD9000000&STOP_TIME=JD10000D&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> No ephemeris for target "Jupiter" after A.D. 2200-JAN-09 00:00:00.0000 TDB

Range Saturn:
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=699&EPHEM_TYPE=VECTORS&START_TIME=JD2305447.5&STOP_TIME=JD2305507.5&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> No ephemeris for target "Saturn" prior to A.D. 1749-DEC-31 00:00:00.0000 TDB
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=699&EPHEM_TYPE=VECTORS&START_TIME=JD9000000&STOP_TIME=JD10000D&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> No ephemeris for target "Saturn" after A.D. 2250-JAN-05 00:00:00.0000 TDB

Range Uranus:
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=799&EPHEM_TYPE=VECTORS&START_TIME=JD2305447.5&STOP_TIME=JD2305507.5&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> No ephemeris for target "Uranus" prior to A.D. 1600-JAN-05 00:00:00.0000 TDB
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=799&EPHEM_TYPE=VECTORS&START_TIME=JD9000000&STOP_TIME=JD10000D&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> No ephemeris for target "Uranus" after A.D. 2399-DEC-16 00:00:00.0000 TDB

Range Neptune:
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=899&EPHEM_TYPE=VECTORS&START_TIME=JD2305447.5&STOP_TIME=JD2305507.5&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> No ephemeris for target "Neptune" prior to A.D. 1600-JAN-05 00:00:00.0000 TDB
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=899&EPHEM_TYPE=VECTORS&START_TIME=JD9000000&STOP_TIME=JD10000D&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> No ephemeris for target "Neptune" after A.D. 2400-JAN-01 00:00:00.0000 TDB


## Gültige Zeitbereiche

  Planet    Min Date (TDB)   Min JD      Max Date (TDB)   Max JD
  --------- ---------------- ----------- ---------------- -----------
  Mercury   -4713-11-25            0.5   9999-12-30       5373482.5
  Venus     -4713-11-25            0.5   9999-12-30       5373482.5
  Earth     -4713-11-25            0.5   9999-12-30       5373482.5
  Mars       1600-01-02      2305448.5   2600-01-01       2670690.5
  Jupiter    1600-01-11      2305457.5   2200-01-09       2524601.5
  Saturn     1749-12-31      2360233.5   2250-01-05       2542859.5
  Uranus     1600-01-05      2305451.5   2399-12-16       2597625.5
  Neptune    1600-01-05      2305451.5   2400-01-01       2597641.5

## Interpretation

-   Mercury, Venus, Earth: gültig von JD≈0 bis Jahr 9999
-   Mars: 1600--2600
-   Jupiter: 1600--2200
-   Saturn: 1749--2250
-   Uranus: 1600--2399
-   Neptune: 1600--2400

## Konsequenz für TS-D

-   Core (1600--2400): alle Planeten, aber Clipping auf
    planetenspezifische Range notwendig
-   Extended (0--4000): nur innere Planeten vollständig gültig
-   Extreme (-4000--8000): nur Mercury, Venus, Earth sinnvoll nutzbar

Diese Grenzen sind physikalische Modellgrenzen der zugrunde liegenden
DE-Ephemeriden, nicht Implementierungsbeschränkungen von
EphemerisRegression.
