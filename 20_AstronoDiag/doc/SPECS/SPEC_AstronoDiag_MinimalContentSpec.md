# AstronoDiag – Minimal Diagnostic Specification (M2.1)

---

## 1. Purpose / Context

AstronoSphere M2.1 introduces large-scale deterministic mesh expansion across extended time domains.

By design:

* Experiments are generated **without clipping**
* GroundTruth providers (e.g. Horizons) have **limited validity ranges**
* Therefore:

  * some Experiments cannot produce datasets
  * some requests will fail
  * some datasets must be skipped

This is **expected system behavior**, not an error.

Without a diagnostic layer, this leads to:

* pipeline crashes
* silent skips
* non-traceable inconsistencies

All of these violate:

* determinism
* scientific traceability
* reproducibility

> Principle: **No silent behavior. Every outcome must be explainable.**

---

## 2. Scope M2.1

AstronoDiag is introduced as a **minimal passive diagnostic layer**.

Responsibilities:

* classify events
* record events deterministically
* prevent crashes
* prevent silent skips

In M2.1:

* only **AstronoTruth integration**
* only **selected diagnostic codes**
* no cross-module usage yet

Note:

030.006 and 030.007 are part of the M2.1 diagnostic catalog,
but their deterministic validation is deferred.
M2.1 implementation focuses on deterministic pre-request diagnostics:
030.003 InvalidMaturity and 030.005 ProviderRangeViolation.

---

## 3. Non-Goals

Explicitly **out of scope**:

* no aggregation (per run summaries)
* no lifecycle management
* no retry logic
* no policy enforcement
* no pipeline control
* no data correction
* no validation logic (Run == LastRun etc.)

AstronoDiag is:

> **observer only, never actor**

---

## 4. ASC.FMI Code System

Diagnostic codes follow a fixed structure:

```
ASC.FMI
```

### ASC (Component)

| Code | Component     |
| ---- | ------------- |
| 010  | AstronoLab    |
| 020  | AstronoCert   |
| 030  | AstronoTruth  |
| 040  | Astronometria |
| 100  | AstronoData   |
| 110  | AstronoIO     |

---

### FMI (Failure Mode Identifier)

| Code | Meaning                |
| ---- | ---------------------- |
| 001  | InvalidInput           |
| 002  | MissingField           |
| 003  | InvalidMaturity        |
| 004  | DataRangeViolation     |
| 005  | ProviderRangeViolation |
| 006  | RequestFailed          |
| 007  | ParseFailed            |
| 008  | DeterminismViolation   |
| 009  | HashMismatch           |
| 010  | UnsupportedOperation   |

---

### Active Codes (M2.1)

| Code    | Symbol                                 |
| ------- | -------------------------------------- |
| 030.003 | AstronoTruth.InvalidMaturity           |
| 030.005 | AstronoTruth.ProviderRangeViolation    |
| 030.006 | AstronoTruth.RequestFailed             |
| 030.007 | AstronoTruth.ParseFailed               |
| 020.003 | AstronoCert.InvalidMaturity (reserved) |
| 100.009 | AstronoData.HashMismatch (reserved)    |



---

## 5. DiagnosticRecord Schema

A DiagnosticRecord represents **one observed event**.

```json
{
  "Code": "030.005",
  "Symbol": "AstronoTruth.ProviderRangeViolation",
  "Severity": "Warning",
  "Message": "Experiment time range is outside provider range.",
  "SourceSystem": "AstronoTruth",
  "SubSourceSystem": "Horizons",
  "InputObjectType": "Experiment",
  "InputObjectId": "<ExperimentID>",
  "CatalogNumber": "AS-000223",
  "Details": {
    "Target": "Saturn",
    "StartJD": 1721059.5,
    "StopJD": 2086300.5,
    "ProviderMinJD": 2360233.5,
    "ProviderMaxJD": 2542859.5
  },
  "CreatedAtUtc": "2026-04-26T12:00:00Z"
}
```

### Rules

* exactly one record per event
* no aggregation
* deterministic content
* no null fields

---

## 6. Severity Rules

Minimal severity model:

| Severity | Meaning            |
| -------- | ------------------ |
| Info     | informational only |
| Warning  | expected deviation |
| Error    | execution failure  |

### Mapping (M2.1)

| Code    | Severity |
| ------- | -------- |
| 030.003 | Warning  |
| 030.005 | Warning  |
| 030.006 | Error    |
| 030.007 | Error    |
| 100.009 | Error    |

---

## 7. AstronoTruth Behavior

For each Experiment, exactly one failure can be thrown and recorded, if present:

```
IF Maturity != Released
    → skip dataset
    → write DiagnosticRecord (030.003)

ELSE IF outside ProviderRange
    → skip dataset
    → write DiagnosticRecord (030.005)

ELSE IF request fails
    → write DiagnosticRecord (030.006)

ELSE IF parse fails
    → write DiagnosticRecord (030.007)

ELSE
    → generate dataset normally
```

### Key Rules

* NO crash
* NO silent skip
* deterministic decision path

ProviderRange must follow:

> t ∈ SimulationMesh AND t ∈ ProviderRange(planet) 

---

## 8. Output Location + File Naming

Location:

```
03_GroundTruth/DiagMessages/Run/
03_GroundTruth/DiagMessages/LastRun/
```

M2.1 behavior: Run/LastRun Rules
* before Run: 
    * copy DiagMessages/Run → DiagMessages/LastRun
    * delete all Files in DiagMessages/Run
* during Run:
    * new DiagnosticRecords  → DiagMessages/Run

General Rules:
* pure copy/move mechanics in Run/LastRun
* one file per DiagnosticRecord
* no aggregation
* no overwriting

Files with identical name are overwritten, that means "Duplicate Experiments collapse to one GroundTruth dataset" (Known Behavior and accepted for the M2.1)

### 8.1 File Naming (proposed)

```
DiagMsg__<CatalogNumber>__<Human>__<ASC.FMI>.json
```

Example:

```
DiagMsg__AS-000223__PLANET-SATURN-MXT1__030.005.json
```

### 8.2 Output Scope (Clarification)

M2.1 output is strictly:

- atomic DiagnosticRecords
- no interpretation
- no aggregation

All higher-level analysis is deferred to:
- future diagnostic layers
- Astronolysis

### Rationale

* allows direct traceability via CatalogNumber
* discriminates PipelineData from SystemData (DiagMsg)
* file names are most important for human users. This approach provides reasonable compromise between accuracy and human readibility
* alphabetic ordering well interpretable for humans

---

## 9. Determinism Rules

AstronoDiag MUST follow strict determinism:

* identical input → identical DiagnosticRecord
* stable code definitions (ASC.FMI never change)
* no randomness
* no timestamps influencing logic (only metadata)

### Critical Rules

* message text is NOT part of determinism
* code + details ARE part of determinism
* record creation must be unconditional

> "no silent changes" principle 

---

## 10. Future Extensions (NOT M2.1)

Planned evolution:

* aggregation (per run statistics)
* diagnostic summaries
* validation checks (Run == LastRun)
* completeness checks
* hash verification
* failure policies
* lifecycle states
* RunCertificate

---

## Final Principle

AstronoDiag establishes:

> deterministic failure visibility
> instead of implicit behavior

This is mandatory for:

* mesh-based validation
* scientific reproducibility
* system trust

---
