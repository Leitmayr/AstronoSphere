# AstronoSphere — M1.9 Recap

## Status

**Mission accomplished: M1.9 Milestone successfully reached**

---

# 1. Zielbild von M1.9

Ziel war der Aufbau einer vollständig deterministischen Pipeline:

```text
Seeds → Experiments → GroundTruth
```

mit folgenden Eigenschaften:

* reproduzierbare Ergebnisse
* stabile Hashes
* nachvollziehbare Datenherkunft (Provenance)
* physikalisch konsistente Ephemeriden

---

# 2. Finaler Architekturzustand

## Pipeline

```text
01_Seeds/Incoming
    ↓ AstronoLab
01_Seeds/Prepared
    ↓ AstronoCert
02_Experiments/Released
    ↓ AstronoTruth
03_GroundTruth/Run
```

Parallel:

```text
Run ↔ LastRun (Determinismus-Check)
```

---

## Rollen der Komponenten

### AstronoLab

* Seed-Erzeugung
* keine Veränderung der Core-Daten
* keine Messlogik

### AstronoCert

* Erzeugung von Experiments
* CoreHash (deterministisch)
* ExperimentID
* KEIN DatasetHeader

### AstronoTruth

* GroundTruth-Erzeugung (Horizons)
* DatasetHeader
* RequestHash
* physikalische Daten

---

# 3. Technische Durchbrüche

## 3.1 Deterministisches Hashing

```text
Canonical(Core) → SHA256 → CoreHash (truncated)
```

Eigenschaften:

* vollständig reproduzierbar
* unabhängig von JSON-Formatierung
* stabil über Pipeline

---

## 3.2 Parameter-Hash (RequestHash)

Problem:

* CanonicalRequest ≠ serialisierter String
* \n vs echte Zeilen → unterschiedliche Hashes

Lösung:

```text
HashInput = PARAM1|PARAM2|PARAM3|...
```

Beispiel:

```text
CENTER=@10|COMMAND=299|...|STOP_TIME=JD...
```

Ergebnis:

* 1:1 reproduzierbar (Web + Code)
* unabhängig von Formatierung
* robust gegen Escape-Sequenzen

---

## 3.3 Precision Handling (kritischer Durchbruch)

### Problem

```text
string → double → string
```

führt zu:

```text
JD drift (~1e-9)
```

---

### Ursache

* IEEE754 double Repräsentation
* nicht exakt darstellbare Dezimalzahlen

---

### Lösung

```text
JD als String durch Pipeline führen
```

und:

```text
StateVector gehört zum Horizons-JD
```

nicht zum ursprünglichen Input-JD

---

### Ergebnis

* keine künstliche Präzision
* physikalisch konsistente Daten
* vollständige Nachvollziehbarkeit

---

# 4. Validierungsstrategie

## 4.1 Golden Samples

Verwendet:

```text
DS3, DS15, DS23, DS46, DS57, DS72, DS104, DS145
```

Ziele:

* Precision prüfen
* Hash prüfen
* URL prüfen
* Datenkonsistenz prüfen

---

## 4.2 Run == LastRun

* binärer Vergleich der JSON-Dateien
* deterministischer Pipeline-Beweis

Ergebnis:

```text
Run == LastRun → TRUE
```

---

## 4.3 Externe Validierung

* Horizons URL getestet → identische Daten
* SHA256 Web-Tool → Hash identisch

---

# 5. Zentrale Fehler & Learnings

## 5.1 Double Parsing Problem

```text
string → double → string → DRIFT
```

→ gelöst durch String-Pipeline

---

## 5.2 JSON Serialization Drift

* unterschiedliche Formatierung → andere Werte

→ gelöst durch kontrollierte Ausgabe

---

## 5.3 Hash Inkonsistenz

```text
Canonical ≠ Header ≠ WebHash
```

→ gelöst durch Parameter-Hash

---

## 5.4 Trial & Error vs STRICT Mode

Erkenntnis:

```text
Nur reproduzierbare Hypothesen sind valide
```

---

# 6. Ergebniszustand M1.9

## Technisch

```text
✔ deterministische Pipeline
✔ stabile Hashes
✔ reproduzierbare API Calls
✔ konsistente Daten
```

---

## Wissenschaftlich

```text
✔ vollständige Provenance
✔ physikalische Konsistenz
✔ nachvollziehbare Unsicherheit
```

---

## Operativ

```text
✔ automatisierbarer Pipeline-Run
✔ Validierung über Run == LastRun
✔ stabile Datenbasis
```

---

# 7. Offene Punkte (M2)

* BeyondCompare CLI Integration
* weitere Datenprovider (z. B. Miriade)
* höhere Levels (L1, L2)
* Integration Astronometria
* Analyse (Astronolysis)

---

# 8. Wichtigste Learnings (Top 5)

1. Strings sind Wahrheit, nicht doubles
2. determinism > elegance
3. Hashes müssen reproduzierbar sein, nicht „schön“
4. Pipeline zuerst, Features danach
5. STRICT MODE funktioniert

---

# 9. Schlussstatement

M1.9 erreicht das Ziel:

```text
Ein deterministisches, reproduzierbares und wissenschaftlich belastbares Ephemeris-System
```

Der entscheidende Punkt:

```text
Jeder Datenpunkt ist bis zur Horizons-Anfrage zurückverfolgbar
```

---

# Status

**M1.9 COMPLETE**
