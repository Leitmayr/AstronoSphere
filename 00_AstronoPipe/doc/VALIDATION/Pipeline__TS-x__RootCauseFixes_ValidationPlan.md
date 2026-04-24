# AstronoSphere – M1.7 Validation Plan

## Ziel

Sicherstellen, dass alle RC-Fixes:

* korrekt implementiert sind
* keine Regression erzeugen
* wissenschaftlich konsistent bleiben

---

# Validierungsstrategie

Zwei Ebenen:

## Ebene 1 – Gegen aktuellen Stand ("Heute")

Ziel:

* isolierte Wirkung der Fixes prüfen

---

## Ebene 2 – Gegen alte Referenzdaten

Ziel:

* fachliche Plausibilität
* keine ungewollten Abweichungen

---

# Test-Sets

## RC5 – Frame Mapping

* 2–3 TS-C Szenarien

Checks:

* REF_PLANE korrekt
* Zustand plausibel verändert

---

## RC6 – Validity Domain

* 2 gültige Szenarien
* 2 ungültige Szenarien

Checks:

* gültig → normaler Run
* ungültig → skip + logging

---

## RC1 – Scenario Precision

* 2 TS-A
* 2 TS-B

Checks:

* StartJD/StopJD ≥ 9 Stellen
* ScenarioID korrekt trunciert

---

## RC4 – TimeScale

* 3 gemischte Szenarien

Checks:

* überall TDB
* keine TD mehr vorhanden

---

## RC3 – JSON Precision

* 2 RD-Files

Checks:

* CSV vs JSON Konsistenz
* keine unnötige Rundung

---

## RC7 – URL Validierung

* 2 Datensätze

Checks:

* URL direkt im Browser ausführbar
* identisch mit API-Call

---

# Ebene 2 – Vergleich mit alten Daten

## Stichprobe

* 2 TS-A
* 2 TS-B
* 2 TS-C
* 2 TS-D

---

## Bewertungslogik

| Ergebnis                    | Bedeutung |
| --------------------------- | --------- |
| identisch                   | OK        |
| unterschiedlich + erklärbar | OK        |
| unerwartet                  | Analyse   |

---

# Full Regression

Nach allen Fixes:

1. Holy12 laufen lassen
2. TS-A bis TS-D komplett
3. Run vs LastRun vergleichen

Erwartung:
→ binär identisch

---

# Zusatztests

## Fehlerfälle

* API Fehler simulieren
* Timeout
* leere Antworten

---

## Hash Validierung

* RequestHash stabil?
* nur bei echten Änderungen unterschiedlich?

---

# Abbruchkriterien

Fix gilt als korrekt wenn:

* alle Ebene-1 Tests bestanden
* keine unerklärten Abweichungen
* Determinismus erhalten bleibt

---

# Abschluss

Nach erfolgreicher Validierung:

1. Commit
2. Baseline definieren
3. M1 abgeschlossen

→ Übergang zu M2 möglich
