## Rauschanalyse
- Quantisierungsrauschen
- Ableitungen v, a
- DE440 Terme identifizieren 
- Vergleich des Rauschverhaltens Miriade und Horizons
- Ideale Abtastzeitpunkte durch verschachtelte Zeitreihe 

##  Garbage Collector/ErrorHandler
- die einzelnen GUIs erzeugen Datenelemente und prüfen auf Probleme
- Probleme z.B. Out Of Bounds, too many Data, inconsistent Data, …
- Kategorisierung: ERROR, WARNUNG, INFO abh. Von Schwere
- Zentraler Sammler/Logger für alle erzeugten oder nicht erzeugten Files zum späteren Debugging
- zentrale Schnittstelle über AstronoData.IO?

## LifecycleDefinition
- Umgang mit deprecated Files definieren
- Dokumentieren, dass AstronoCert und AstronoTruth nur "Released" files verarbeiten (Metadata.Status.Maturity = "Released"). Gehört ins DataModel-Dokument

## Astronolysis – Edge Case Seed Derivation
- Delta logs (Mesh Validation)

Output:
EdgeCaseSeedsDefinition:
    - detect overshoots (ratio > 1.0)
    - detect plateau regions (ratio ~ 0.95–1.0, sustained)
- cluster by experiment

Extract representative JD windowsValidation:
- reproduce known MVH1 findings:  AS-000279  AS-000334  AS-000338  AS-000280 (plateau)
    
Success Criterion:
- derived seeds match manually identified cases

Wichtig
Du hast damit einen perfekten zukünftigen Test:
Astronolysis muss das finden,was wir heute manuell gefunden haben
Das ist Gold wert.

Fazit
Fokus bleibt auf M2.2 ✔Erkenntnis geht nicht verloren ✔Zukünftige Validierung vorbereitet ✔
Genau so arbeitet man sauber durch die Milestones.

## Astonolysis - BackToBack Testing
- Same Delta Algo As today (Test Framework)
- Same statistics as today (Script)

## Astronolysis:
"Inner Planets Pre-0 Epoch Accuracy Analysis" -> starke Abweichungen vorhanden, Before Christ Analyse!
- drehe Anzahl Parameter in VSOP runter und analysiere, ob sich der Zeitraum verändert, zu dem das Modell schlechter wird

## AstronoTruth
- die geoäquatorialen Experimente 59-72 im Catalog müssen noch einmal mit korrektem Horizons Lauf erstellt werden

## Naming
- die GroundTruth-Daten könnten auch ein Prefix mit dem Experiment gebrauchen. Könnte man am Ende dann konsistent durchziehen, und die Pipeline von Anfang bis Ende rennen lassen. Run == LastRun wäre dann der BackToBack Test