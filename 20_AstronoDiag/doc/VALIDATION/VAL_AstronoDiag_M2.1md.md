# VAL_AstronoDiag_M2.1

## Purpose

Validate deterministic and correct behavior of AstronoDiag in M2.1.

Focus:

* correct classification (ASC.FMI)
* correct skip behavior
* correct DiagnosticRecord content
* no crash
* no silent skip

---

## Scope

Validated:

* 030.003 InvalidMaturity
* 030.005 ProviderRangeViolation


Not validated:

* aggregation
* determinism across runs (future)
* cross-module diagnostics

---

## Test Strategy

Each test case defines:

* Input Experiment
* Expected Behavior
* Expected DiagnosticRecord
* Expected Output (file presence)

---

# Test Cases

---

## Case 1 — Invalid Maturity 030.003

### Input

Experiment with:

```json
"Metadata": {
  "Status": {
    "Maturity": "Deprecated"
  }
}
```

### Expected Behavior

* dataset NOT generated
* DiagnosticRecord created

### Expected Diagnostic

```
Code: 030.003
Severity: Warning
```

### Expected Details

```json
{
  "CatalogNumber": "AS-XXXXXX"
}
```

### Expected Output

```
DiagMsg__<CatalogNumber>__<Human>__030.003.json
```

---

## Case 2 — Provider Range Violation (Full Out of Range) 030.005

### Input

Experiment:

* Target: Saturn
* JD range fully before provider min

Example:

```
StartJD = 1721059.5
StopJD  = 2086300.5
```

Provider range Saturn:

```
MinJD = 2360233.5
MaxJD = 2542859.5
```

### Expected Behavior

* dataset NOT generated
* DiagnosticRecord created

### Expected Diagnostic

```
Code: 030.005
Severity: Warning
```

### Expected Details

```json
{
  "Target": "Saturn",
  "StartJD": 1721059.5,
  "StopJD": 2086300.5,
  "ProviderMinJD": 2360233.5,
  "ProviderMaxJD": 2542859.5
}
```

### Expected Output

```
DiagMsg__AS-000223__PLANET-SATURN-MXT1__030.005.json
```

---

## Case 3 — Provider Range Violation (Partial Overlap)

### Input

Experiment:

* overlaps partially with provider range

### Expected Behavior

⚠ Decision required:

Current M2.1 rule:

> ANY violation → skip dataset

### Expected Diagnostic

```
Code: 030.005
Severity: Warning
```

### Validation Goal

* confirm rule is consistently applied
* no partial dataset generation


### Expected Details

```json
{
  "Target": "Saturn",
  "StartJD": 2086511.5,
  "StopJD": 2451541.5,
  "ProviderMinJD": 2360233.5,
  "ProviderMaxJD": 2542859.5
}
```

### Expected output

```
DiagMsg__AS-000231__PLANET-SATURN-MXT1__030.005.json
```
---

## Case 4 — Request Failed

### M2.1 Definition
Status: Deferred / Not validated in M2.1
Reason: Requires injectable test doubles or controlled provider/parser failure simulation.


### Input

Simulate Horizons failure:

* invalid URL
* forced timeout
* network failure

### Expected Behavior

* dataset NOT generated
* DiagnosticRecord created

### Expected Diagnostic

```
Code: 030.006
Severity: Error
```

### Expected Details

```json
{
  "RequestUrl": "...",
  "Target": "Saturn"
}
```

### Expected Output

```
DiagMsg__...__030.006.json
```

---

## Case 5 — Parse Failed

### M2.1 Definition
Status: Deferred / Not validated in M2.1
Reason: Requires injectable test doubles or controlled provider/parser failure simulation.

### Input

Simulate:

* malformed Horizons response
* truncated CSV

### Expected Behavior

* dataset NOT generated
* DiagnosticRecord created

### Expected Diagnostic

```
Code: 030.007
Severity: Error
```

### Expected Details

```json
{
  "ParseStage": "CSV",
  "Target": "Saturn"
}
```

---

## Case 6 — Valid Dataset

### Input

Experiment fully inside provider range

### Expected Behavior

* dataset generated
* NO DiagnosticRecord

### Validation Goal

* ensure no false positives

---

## Case 7 — Determinism (Single Run)

### Input

Run identical experiment twice

### Expected Behavior

* identical DiagnosticRecord content
* identical filename
* identical JSON structure

### Validation Goal

* confirm deterministic generation

---

## Case 8 — Completeness

### Input

Batch run (e.g. 370 experiments)

### Expected Behavior

For each experiment:

* exactly ONE of:

  * dataset generated
  * DiagnosticRecord exists

### Validation Goal

* no missing outputs
* no double outputs
* no silent skips

---

# Deferred Validation Cases

The following diagnostic codes are defined in the M2.1 specification,
but are not actively validated in M2.1 because deterministic triggering
requires injectable test doubles or controlled provider/parser failure
simulation.

Deferred:
- 030.006 AstronoTruth.RequestFailed
- 030.007 AstronoTruth.ParseFailed

Validation is deferred until AstronoTruth exposes stable test seams
(e.g. injectable provider client / parser input).

---

# Validation Rules


## Rule 1 — No Crash

Pipeline must complete:

* regardless of failures
* regardless of provider issues

---

## Rule 2 — No Silent Skip

Every skipped dataset MUST produce:

* exactly one DiagnosticRecord

---

## Rule 3 — One Outcome per Experiment

Per experiment:

```
EITHER dataset
OR DiagnosticRecord
```

Never both.

---

## Rule 4 — Deterministic Output

Same input:

* same decision
* same code
* same file name
* same content (except timestamp)

---

## Rule 5 — Structural Integrity

Each DiagnosticRecord must:

* contain all required fields
* contain no null values
* be valid JSON

---

# Decisions

---

## D-1 Partial Provider Overlap

Options:

A) strict → any violation = skip (current spec)
B) allow clipped dataset (NOT recommended)


Decision: A) strict → any violation = skip


---

## D-2 Details Content Standardization

Define minimal required keys:

* Target (mandatory)
* StartJD / StopJD (mandatory)
* ProviderMinJD / ProviderMaxJD (for 030.005)

Decision: accepted

---

## D-3 RequestFailed Detail Level

Options:

* minimal (URL only)
* extended (HTTP code, timeout, etc.)

Decision: extended


---

# Final Principle

Validation must prove:

> The system never fails silently
> and every non-generated dataset is explainable.

---


Test Protocol (German):


1) DiagMessages:
- LastRun wird nicht mehr gelöscht, sondern aggregiert alle vorherigen Run Läufe. Check. 
- Files mit demselben Dateinamen werden überschrieben. Check.
- Experiment 223 und 239 erzeugen nach wie vor DiagMessages. Check.
- Für 223 und 239 werden keine GroundTruth Daten erzeugt. Check.
- Experiment 167 erzeugt eine DiagMessage und keine Datensatz, wenn Status.Maturity = Deprecated

2) EphemerisFactory:
- Experiment 167 erzeugt einen GroundTruth Datensatz, wenn Status.Maturity = Released. Check.
- LastRun wird nicht mehr gelöscht, sondern aggregiert alle vorherigen Run Läufe. Check. 

3) Determinismus
- Für alle Datensätze gilt: Re-Run bewirkt Run ==  LastRun. Check.

4) Misc Tests

4a) VAL Case 8 Completeness / Mini-Batch
Nicht alle 370 sofort, sondern z.B. 5–10 Experimente gemischt:
valid

4b) invalid call: 030.006 by data set 325
full out-of-range
partial overlap 
deprecated
Erwartung: pro Experiment genau eins: Dataset oder DiagMessage.
-> checked for Golden Samples.

4c) Folder-Existenz nach Clean State
DiagMessages/Run und DiagMessages/LastRun einmal leer/löschen und prüfen, ob die Factory sie selbst korrekt anlegt.
-> Folder werden angelegt. Run == LastRun bei Re-Run checked. Ok

4d) Filename-Kollision bewusst prüfen
Gleicher Diag-Case zweimal: LastRun überschreibt gleichnamiges File, kein Crash.
-> checked. ok.

4e) Kein False Positive bei valid Released
Hast Du mit 167 schon gemacht. Check.
-> checked. ok.

4f)Deprecated schlägt ProviderRange. 
Ein out-of-range Experiment temporär auf Deprecated: Erwartung 030.003, nicht 030.005. Das testet die IF/ELSE-Priorität.
-> checked with data set 220. Ok.

5) Back To Back Testing

Datensätze 3, 15, 23, 46, 57 (Golden Samples von früher): Daten wurden binär identisch erzeugt wie am 15.4. -> Implementierung hat alte Daten nicht verändert.
