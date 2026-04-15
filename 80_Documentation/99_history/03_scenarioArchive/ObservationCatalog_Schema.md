# ObservationCatalog Schema

## Astronometria Reference Data

Version: 1.0\
Date: 2026-03-12

------------------------------------------------------------------------

# 1. Ziel

Das ObservationCatalog definiert astronomische Szenarien, die zur
Validierung von Astronometria genutzt werden.

Jede Observation beschreibt:

-   astronomische Situation
-   ungefähren Zeitpunkt
-   Zeitfenster
-   Samplingstrategie

Die tatsächlichen Daten werden später über Ground Truth Systeme erzeugt.

------------------------------------------------------------------------

# 2. Minimales Schema

``` yaml
id: string

domain: string
event: string

primary_body: string
observer_body: string

approx_jd: float

window_before_days: int
window_after_days: int

sampling_step: string
```

------------------------------------------------------------------------

# 3. Erweiterte Felder

``` yaml
title: string
description: string

purpose:
  - string

tags:
  - string

source:
  generator: string
  algorithm: string
```

------------------------------------------------------------------------

# 4. Beispiel

``` yaml
id: planet_opposition_mars_2020

title: Mars opposition October 2020

domain: planet
event: opposition

primary_body: Mars
observer_body: Earth

approx_jd: 2459135.2

window_before_days: 5
window_after_days: 5

sampling_step: 1h

purpose:
  - retrograde_motion_validation
  - geocentric_pipeline_test

tags:
  - planetary_event
  - regression

source:
  generator: meeus_cpp
  algorithm: meeus_ch36
```

------------------------------------------------------------------------

# 5. Domain Kategorien

Empfohlene Domains:

planet\
moon\
observer\
frame\
time\
event\
edgecase

------------------------------------------------------------------------

# 6. Eventtypen

opposition\
conjunction\
inferior_conjunction\
elongation\
perihelion\
aphelion\
nodecrossing\
stationary\
zenith\
horizon\
phase\
eclipse

------------------------------------------------------------------------

# 7. ID-Konvention

Schema:

domain_event_body_epoch

Beispiel:

planet_opposition_mars_2020

Regeln:

-   IDs müssen eindeutig sein
-   IDs dürfen nicht verändert werden
-   Erweiterungen erfolgen über neue IDs

------------------------------------------------------------------------

# 8. Generierungsworkflow

Meeus Solver\
→ approx_jd erzeugen\
→ ObservationCatalog Eintrag erzeugen\
→ GTRequest generieren\
→ Ground Truth Daten abrufen\
→ Referenzdatensatz erzeugen

------------------------------------------------------------------------

# 9. Ziel

Das ObservationCatalog bildet langfristig:

-   einen astronomischen Szenariokatalog
-   eine Testfallbibliothek
-   eine reproduzierbare Validierungsbasis für Astronometria.
