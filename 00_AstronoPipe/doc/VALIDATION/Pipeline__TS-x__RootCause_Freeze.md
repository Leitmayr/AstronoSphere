# AstronoSphere – M1.7 RC Freeze

## Status

M1 Pipeline wurde vollständig analysiert (TS-A bis TS-D).

Ergebnis:

* Pipeline ist deterministisch
* Architektur ist stabil
* 7 Root Causes identifiziert

Dieses Dokument friert die RCs und definiert die Fix-Strategie.

---

# RC Übersicht

## RC1 – Scenario Precision (JD)

StartJD/StopJD werden im SHG zu stark gerundet.

**Fix:**

* Speicherung mit hoher Präzision (>= 9 Dezimalstellen)

**Zusatzregel (NEU):**

* ScenarioID:
  → JD wird auf **3 Nachkommastellen trunciert**
* Gleiches gilt für Referenzdateinamen (M1 pragmatisch)

---

## RC2 – Step Pattern (000/333/667 etc.)

**Status:**
Kein Fehler

**Ursache:**
1H = 1/24 D → periodisches Muster

**Maßnahme:**
Dokumentation, kein Fix

---

## RC3 – JSON Precision Loss

**Problem:**
CSV enthält höhere Präzision als JSON

**Fix:**

* JSON Serialization anpassen
* keine unnötige Rundung

---

## RC4 – TimeScale Inkonsistenz (TD vs TDB)

**Problem:**
Uneinheitliche Verwendung im ScenarioHeader

**Fix:**

* Standard: **TDB**
* konsistent in:

  * Scenario Core
  * ScenarioID
  * Factory Requests

---

## RC5 – Frame Mapping Fehler

**Problem:**
GeoEquatorial wird falsch gemappt

**Fix:**

* GeoEquatorial → REF_PLANE=FRAME
* GeoEcliptic → REF_PLANE=ECLIPTIC

**Priorität:**
CRITICAL

---

## RC6 – Horizons Validity Domain fehlt

**Problem:**
Requests außerhalb gültiger Zeitbereiche

**Fix (M1):**

* Factory erkennt invalid ranges
* skip + logging

**Ziel (M2):**

* Constraint bereits im ScenarioCreationTool
* nur gültige Szenarien im OC

---

## RC7 – TruthProviderUrl falsch

**Problem:**
Gespeicherter URL entspricht nicht realem Request

**Fix:**

* Speicherung des tatsächlichen API-URLs

---

# Designentscheidungen (NEU)

## ScenarioID Regel

* volle Präzision im Core
* reduzierte Präzision in ID

```text
Core:        2459845.640277778
ScenarioID:  2459845.640
```

---

## TimeScale Standard

* Default: **TDB**
* KISS → keine Mehrdeutigkeit

---

## Zukunft: ScenarioCreationTool

* GUI-basierte Erstellung
* integriert:

  * Validity Domains
  * Header-Unterstützung
* Ziel:
  → keine invaliden Szenarien im OC

---

# Fix-Reihenfolge

1. RC5 – Frame Mapping
2. RC6 – Validity Domain
3. RC1 – Scenario Precision
4. RC4 – TimeScale
5. RC3 – JSON Precision
6. RC7 – TruthProviderUrl
7. RC2 – Dokumentation

---

# Fazit

Systemstatus:

* Architektur stabil
* Determinismus nachgewiesen
* Fixes klar isoliert

→ bereit für M1.7 Fix-Phase
