# Genauigkeit von VSOP87A -- Analyse mit BibTeX-Referenzen

## 1) Offizielles Genauigkeitsversprechen

Die Theorie VSOP87 wurde veröffentlicht in:

Bretagnon & Francou (1988), *Astronomy & Astrophysics*, 202, 309--315.

VSOP87 wurde an die JPL-Ephemeride DE200 angepasst.\
Die publizierten maximalen Residuen liegen typischerweise bei:

-   Innere Planeten: \~1″\
-   Jupiter/Saturn: \~1--2″\
-   Uranus/Neptun: bis \~5″

Einheit der Fehlerangabe: **Bogensekunden**.

------------------------------------------------------------------------

## 2) Referenzephemeride

VSOP87 wurde gefittet gegen:

JPL Development Ephemeris DE200

Moderne Vergleiche (z. B. JPL Horizons) basieren typischerweise auf:

-   DE430 (Folkner et al., 2014)
-   DE440 (Park et al., 2021)

Diese Ephemeriden enthalten signifikante Verbesserungen gegenüber DE200.

------------------------------------------------------------------------

## 3) Umrechnung Winkel → Distanz (Neptun)

1″ = 4.848136811e-6 rad

Für Neptun (\~30 AU):

5″ ≈ 30 AU × 5 × 4.848e-6\
≈ 7.27e-4 AU\
≈ \~108 000 km

Eine gemessene Abweichung von 3.5e-4 AU (\~54 000 km) liegt somit im
erwarteten Bereich.

------------------------------------------------------------------------

## 4) Schlussfolgerung

Die publizierte maximale Winkelabweichung für Neptun beträgt etwa 5″.\
Dies entspricht linear etwa 100 000 km.

Eine beobachtete Abweichung von \~54 000 km ist daher vollständig
konsistent mit dem dokumentierten Genauigkeitsrahmen von VSOP87A.

------------------------------------------------------------------------

# BibTeX-Referenzen

``` bibtex
@article{bretagnon1988vsop87,
  author    = {Bretagnon, Pierre and Francou, Gérard},
  title     = {Planetary theories in rectangular and spherical variables – VSOP87 solutions},
  journal   = {Astronomy and Astrophysics},
  volume    = {202},
  pages     = {309--315},
  year      = {1988}
}

@techreport{folkner2014de430,
  author       = {Folkner, William M. and Williams, James G. and Boggs, Dale H. and Park, Ryan S. and Kuchynka, Petr},
  title        = {The Planetary and Lunar Ephemerides DE430 and DE431},
  institution  = {Jet Propulsion Laboratory},
  number       = {IPN Progress Report 42-196},
  year         = {2014}
}

@article{park2021de440,
  author    = {Park, Ryan S. and Folkner, William M. and Williams, James G. and Boggs, Dale H.},
  title     = {The JPL Planetary and Lunar Ephemerides DE440 and DE441},
  journal   = {The Astronomical Journal},
  volume    = {161},
  number    = {3},
  pages     = {105},
  year      = {2021},
  doi       = {10.3847/1538-3881/abd414}
}
```

------------------------------------------------------------------------

Erstellt am: 2026-02-16 07:38 UTC
