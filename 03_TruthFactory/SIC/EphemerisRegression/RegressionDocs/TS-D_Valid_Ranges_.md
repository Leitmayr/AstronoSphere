# TS-D Valid Ranges for Heliozentrische Horizons VECTORS (@10)

Diese Tabelle dokumentiert die tatsächlich von der JPL Horizons API
(format=text, EPHEM_TYPE=VECTORS, CENTER=@10) bereitgestellten gültigen
Zeitbereiche für heliozentrische Planeten-State-Vektoren.

Die Grenzen wurden experimentell durch API-Abfragen bestimmt (Stand:
2026-02-24).

## Gültige Zeitbereiche

  Planet    Min Date (TDB)   Min JD      Max Date (TDB)   Max JD
  --------- ---------------- ----------- ---------------- -----------
  Mercury   0000-01-01       1721059.5   9999-12-30       5373482.5
  Venus     0000-01-01       1721059.5   9999-12-30       5373482.5
  Earth     0000-01-01       1721059.5   9999-12-30       5373482.5
  Mars      1600-01-02       2305448.5   2600-01-01       2670690.5
  Jupiter   1600-01-11       2305457.5   2200-01-09       2524601.5
  Saturn    1749-12-31       2360233.5   2250-01-05       2542859.5
  Uranus    1600-01-05       2305451.5   2399-12-16       2597625.5
  Neptune   1600-01-05       2305451.5   2400-01-01       2597641.5

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
