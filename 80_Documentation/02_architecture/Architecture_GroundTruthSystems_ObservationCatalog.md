# Astronometria Architecture

## Ground Truth Systems (GTS) & Observation Catalog

Author: Marcus Hiemer / Astronometria\
Date: 2026-03-12

------------------------------------------------------------------------

# 1. Ziel dieser Architektur

Astronometria benötigt hochpräzise Referenzdaten, um:

-   numerische Modelle zu validieren
-   Regression Tests auszuführen
-   wissenschaftliche Reproduzierbarkeit sicherzustellen

Diese Referenzdaten stammen aus **Ground Truth Systems (GTS)** wie:

-   NASA JPL Horizons
-   IERS Earth Orientation
-   SPICE Ephemeriden
-   weitere astronomische Datenquellen

Wichtig: Astronometria selbst soll **nicht direkt mit diesen Systemen
kommunizieren**.\
Stattdessen werden Referenzdaten über separate Tools erzeugt und
anschließend eingefroren.

------------------------------------------------------------------------

# 2. Grundprinzip

Die Architektur trennt drei Ebenen:

ObservationCatalog → GTRequests → ReferenceData

Pipeline:

Meeus Solver\
→ ObservationCatalog\
→ GTRequests\
→ GT Tools\
→ RawGTData\
→ Validated Reference Data\
→ Astronometria

Damit werden:

-   deterministische Tests
-   reproduzierbare Ergebnisse
-   Offlinefähigkeit

erreicht.

------------------------------------------------------------------------

# 3. Ground Truth Systems

Ground Truth Systeme liefern hochpräzise astronomische Daten.

Beispiele:

  System     Domäne
  ---------- --------------------------
  Horizons   Planetare Ephemeriden
  SPICE      Ephemeris Rohdaten
  IERS       Erdrotation
  USNO       astronomische Ereignisse
  Gaia       Sternkataloge

Für Astronometria ist derzeit besonders wichtig:

**JPL Horizons**

------------------------------------------------------------------------

# 4. Ground Truth Tools

Der Zugriff auf GTS erfolgt über separate Tools.

Beispiel:

EphemerisRegression (ER)

Eigenschaften:

-   CLI Interface
-   reproduzierbare Datenproduktion
-   Speicherung im Referenzdatenrepo

Beispiel CLI:

er run ts-a L0\
er run ts-b L0

------------------------------------------------------------------------

# 5. AstroReferenceData Repository

Alle Referenzdaten werden zentral gespeichert.

Empfohlene Struktur:

AstroReferenceData - ObservationCatalog - GTRequests - RawGTData -
ReferenceData - Metadata

Dieses Repository ist die **Single Source of Truth**.

------------------------------------------------------------------------

# 6. Observation Catalog

Der ObservationCatalog enthält wissenschaftliche Szenarien.

Beispiele:

-   Mars opposition
-   Venus inferior conjunction
-   Mercury perihelion
-   Node crossing
-   Zenith crossing
-   Horizon crossing

Diese Szenarien bilden einen **astronomischen Validierungskatalog**.

------------------------------------------------------------------------

# 7. Klassische Testsituationen

Typische Ephemeris‑Stresssituationen:

1.  Opposition\
2.  Conjunction\
3.  Inferior Conjunction\
4.  Greatest Elongation\
5.  Perihelion\
6.  Aphelion\
7.  Node Crossing\
8.  Stationary Point\
9.  Zenith Crossing\
10. Horizon Crossing\
11. Maximum Phase Angle\
12. Eclipse Geometry

------------------------------------------------------------------------

# 8. Meeus-basierte Ereignisschätzung

Ein vorhandenes C++ Tool implementiert Meeus‑Formeln.

Dieses Tool liefert **grobe Ereigniszeiten**.

Beispiel:

Mars opposition ≈ JD 2459135.2

Daraus wird ein Zeitfenster erzeugt:

start = JD − window\
stop = JD + window

Beispiel:

start = JD − 5 days\
stop = JD + 5 days\
step = 1h

Die präzisen Daten werden anschließend von Horizons erzeugt.

------------------------------------------------------------------------

# 9. Beispiel Observation Definition

``` yaml
id: planet_opposition_mars_2020

domain: planet
event: opposition

primary_body: Mars
observer_body: Earth

approx_jd: 2459135.2

window_before_days: 5
window_after_days: 5

sampling_step: 1h

purpose:
  - retrograde_motion
  - geocentric_geometry
  - velocity_validation

source:
  generator: meeus_cpp
  algorithm: meeus_ch36
```

------------------------------------------------------------------------

# 10. GT Request Definition

``` yaml
id: horizons_planet_opposition_mars_2020

gts: horizons

command: 499
center: 500

start_jd: 2459130
stop_jd: 2459140

step: 1h

ephem_type: vectors
frame: ecliptic
aberrations: none
```

------------------------------------------------------------------------

# 11. Dataset Metadata

``` yaml
dataset_id: horizons_planet_opposition_mars_2020_L0

source_gts: horizons
gts_version: DE440

generation_time: 2026-03-12
generator_tool: EphemerisRegression

observation_id: planet_opposition_mars_2020

sampling:
  start_jd: 2459130
  stop_jd: 2459140
  step_hours: 1

data_type:
  state_vectors

hash: 8f2a1c...
```

------------------------------------------------------------------------

# 12. Observation ID Strategie

Empfohlenes Schema:

domain_event_body_epoch

Beispiele:

planet_opposition_mars_2020\
planet_conjunction_venus_2012\
orbit_perihelion_mercury_2000\
geometry_nodecrossing_moon_2025

IDs sollten **niemals geändert werden**, um Reproduzierbarkeit zu
sichern.

------------------------------------------------------------------------

# 13. Gesamtpipeline

Meeus Solver\
→ ObservationCatalog\
→ GTRequests\
→ GT Tools\
→ RawGTData\
→ Validation / Freeze\
→ AstroReferenceData\
→ Astronometria

------------------------------------------------------------------------

# 14. Vorteile

Diese Architektur ermöglicht:

-   wissenschaftliche Reproduzierbarkeit
-   deterministische Regression Tests
-   Offlinefähigkeit
-   austauschbare Ground Truth Systeme
-   dokumentiertes astronomisches Wissen

Der ObservationCatalog wird langfristig zu einem zentralen Bestandteil
der Astronometria‑Validierungsstrategie.
