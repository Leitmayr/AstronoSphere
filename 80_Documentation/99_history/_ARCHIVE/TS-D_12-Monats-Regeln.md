# TS-D Mesh Generation -- 12-Monats-Regeln

## Regel 1 --- Drei feste Epochen

Die Epochen sind global definiert und gelten für alle Planeten:

-   **Epoch1 (Core):** 1600--2400 \| Step = 30 Tage\
-   **Epoch2 (Extended):** 0--4000 \| Step = 180 Tage\
-   **Epoch3 (Extreme):** -4000--8000 \| Step = 720 Tage

Diese Definition ändert sich nicht.

------------------------------------------------------------------------

## Regel 2 --- Planetenspezifische Verfügbarkeit

Jeder Planet besitzt einen festen gültigen Horizons-Bereich:

PlanetRange = \[PlanetMinJD, PlanetMaxJD\]

Dieser Bereich stammt ausschließlich aus der Horizons-API.

------------------------------------------------------------------------

## Regel 3 --- Schnittmenge bestimmt den effektiven Bereich

Für jede Kombination aus Planet und Epoche gilt:

EffectiveStart = max(EpochStart, PlanetMin)\
EffectiveEnd = min(EpochEnd, PlanetMax)

Existiert keine Schnittmenge → keine Daten.

Keine Sonderfälle.

------------------------------------------------------------------------

## Regel 4 --- Jede Epoche erzeugt ihr eigenes Gitter

Für jede Epoche wird unabhängig ein vollständiges Zeitgitter erzeugt.

Überlappungen zwischen Epochen sind erlaubt und gewollt.

Keine Subtraktion anderer Epochen.\
Keine Bereichsbereinigung.

------------------------------------------------------------------------

## Regel 5 --- Iteration beginnt exakt am EpochStart

Zeitpunkte werden erzeugt nach:

t = EffectiveStart + n \* Step

mit

t ≤ EffectiveEnd

Es wird **kein zusätzlicher Endpunkt erzwungen**.

Restintervalle werden ignoriert.

------------------------------------------------------------------------

## Regel 6 --- Keine Synchronisation zwischen Epochen

Die Gitter sind unabhängig:

-   30d Gitter\
-   180d Gitter\
-   720d Gitter

Sie sind nicht verschachtelt.\
Sie sind nicht harmonisiert.\
Sie sind nicht aufeinander abgestimmt.

------------------------------------------------------------------------

## Regel 7 --- Maximale Datennutzung

Für jeden Planeten werden alle von Horizons verfügbaren Daten genutzt.

Es erfolgt keine künstliche Einschränkung auf Core-Bereiche.

------------------------------------------------------------------------

## Regel 8 --- Determinismus vor Optimierung

-   Keine dynamische Step-Anpassung\
-   Keine planetenspezifische Sonderlogik\
-   Keine heuristische Verdichtung\
-   Keine Nachkorrekturen

Wenn dieselben Regeln in 12 Monaten erneut ausgeführt werden,\
muss exakt derselbe Datensatz entstehen.

------------------------------------------------------------------------

## Zusammenfassung

TS-D ist ein überlagerndes Multi-Resolution-Zeitgitter, das
deterministisch\
die Schnittmenge aus festen Epochen und planetenspezifischer\
Horizons-Verfügbarkeit erzeugt.
