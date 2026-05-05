# AstronoSphere M2 Execution Checklist

## Phase 0 -- M1 Abschluss (Gate)

-   144 Experimente vollständig generiert
-   Run == LastRun (binär identisch)
-   Daten korrekt in 03_GroundTruth
-   DatasetHeader nur aus AstronoTruth
-   Full Pipeline Script läuft automatisiert

## Phase 1 -- Validation Backbone

-   Holy12 definieren
-   GroundTruth plausibilisieren
-   AST gegen Holy12 laufen lassen
-   Ergebnisse prüfen
-   Pipeline-Test mit AST

## Phase 2 -- Regression Stabilisierung

-   AST gegen 144 Experimente
-   Delta prüfen
-   Toleranzen erfüllt
-   deterministische Runs

## Phase 3 -- Mesh & Analyse

-   Extended Mesh
-   Extreme Mesh
-   GroundTruth erzeugen
-   AST Simulation
-   Astronolysis (Drift, Muster, Outlier)

## Phase 4 -- L1

-   MeasurementConfig L1
-   GroundTruth erzeugen
-   AST erweitern
-   Simulation + Vergleich
-   Analyse

## Phase 5 -- L2

-   MeasurementConfig L2
-   GroundTruth erzeugen
-   AST erweitern
-   Simulation + Vergleich
-   Analyse

## M2 Done

-   L0 validiert
-   Mesh analysiert
-   L1 + L2 implementiert und validiert
-   deterministische Pipeline
