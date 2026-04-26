# AS-ARCH: AstronoDiag (M2.1 Minimal Design)

## Context

Within **M2.1 (Mesh Expansion, L0)** the system intentionally produces **Experiments outside GroundTruth provider ranges**.

This is by design:

- Meshes are generated **without clipping** (determinism requirement)
- GroundTruth providers (e.g. Horizons) have **limited validity ranges per planet**
- Therefore:
  - some Experiments **cannot produce datasets**
  - some requests **will fail**
  - some datasets **must be skipped**

This is NOT an error in the system.

It is **expected system behavior**.

---

## Problem

Without a structured mechanism:

- pipeline will crash OR
- silently skip datasets OR
- produce inconsistent outputs

All three violate core principles:

- determinism
- traceability
- scientific integrity

> "no silent changes"

---

## Goal of AstronoDiag (M2.1)

Introduce a **minimal, generic, system-wide diagnostic mechanism** to:

- classify what happens
- record it deterministically
- avoid crashes
- avoid silent behavior

NOT a full system.

---

## Architectural Role

AstronoDiag = passive diagnostic layer

It:

- observes
- classifies
- records

It does NOT:

- modify pipeline behavior
- perform retries
- enforce policies
- control execution

---

## System Position

AstronoLab       → creates Seeds  
AstronoCert      → creates Experiments  
AstronoTruth     → creates GroundTruth  
Astronometria    → computes simulations  

AstronoDiag      → evaluates what happened  

Key principle:

System behavior ≠ System evaluation

---

## Core Concept: ASC.FMI

Inspired by SAE J1939 (SPN.FMI)

ASC.FMI = AstronoSphere Component . Failure Mode Identifier

### ASC (Component)

010 AstronoLab  
020 AstronoCert  
030 AstronoTruth  
040 Astronometria  
100 AstronoData  
110 AstronoIO  

---

### FMI (Failure Mode Identifier)

001 InvalidInput  
002 MissingField  
003 InvalidMaturity  
004 DataRangeViolation  
005 ProviderRangeViolation  
006 RequestFailed  
007 ParseFailed  
008 DeterminismViolation  
009 HashMismatch  
010 UnsupportedOperation  

---

## Initial Codes (M2.1)

030.003  AstronoTruth.InvalidMaturity  
030.005  AstronoTruth.ProviderRangeViolation  
030.006  AstronoTruth.RequestFailed  
030.007  AstronoTruth.ParseFailed  

020.003  AstronoCert.InvalidMaturity  

100.009  AstronoData.HashMismatch   (reserved)

---

## Severity (minimal)

Info  
Warning  
Error  

### Recommended mapping

ProviderRangeViolation → Warning  
InvalidMaturity        → Warning  
RequestFailed          → Error  
ParseFailed            → Error  
HashMismatch           → Error  

---

## Minimal Data Structure

### DiagnosticRecord

Represents one observed event.

Example:

{
  "Code": "030.005",
  "Symbol": "AstronoTruth.ProviderRangeViolation",
  "Severity": "Warning",
  "Message": "Experiment time range is outside provider range.",
  "SourceSystem": "AstronoTruth",
  "InputObjectType": "Experiment",
  "InputObjectId": "HELIO-J2000-TDB-...",
  "Details": {
    "Target": "Saturn",
    "StartJD": 1721059.5,
    "StopJD": 2086300.5,
    "ProviderMinJD": 2360233.5,
    "ProviderMaxJD": 2542859.5
  },
  "CreatedAtUtc": "..."
}

---

## Behavior in AstronoTruth (M2.1)

For each Experiment:

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

Important:

- NO crash  
- NO silent skip  
- ALWAYS deterministic output  

---

## Output Strategy (M2.1)

Keep simple:

03_GroundTruth/.../RunFailures/

- one DiagnosticRecord per skipped/failed dataset  
- deterministic file naming  
- no aggregation yet  

---

## Design Principles

- Codes are stable (never change)  
- Messages are flexible  
- No specialization (generic codes only)  
- No hidden logic  
- No implicit correction  

---

## Separation of Concerns

### CategoryMapper

- maps domain categories  
- part of data model  
- used for IDs / filenames  

### AstronoDiag

- describes system behavior  
- used for diagnostics / validation  

Key distinction:

CategoryMapper      → what the experiment is  
DiagnosticCatalog   → what happened during processing  

---

## Module Placement

Decision:

10_AstronoDiag

Contains:

- DiagnosticCode (ASC.FMI)  
- DiagnosticSeverity  
- DiagnosticRecord  
- DiagnosticCatalog (code definitions)  

NOT in:

AstronoData.Contracts

Reason:

- Diagnostics may later include logic  
- Contracts must stay minimal and stable  
- Ownership belongs to diagnostics layer  

---

## Future Extensions (NOT M2.1)

AstronoDiag will later expand to:

- Aggregation (per run)  
- Run statistics  
- Validation checks (Run == LastRun)  
- Hash verification  
- Completeness checks  
- RunCertificate  
- Failure policies  
- Lifecycle handling  

---

## Key Insight

AstronoDiag enables:

deterministic failure handling  
instead of implicit behavior  

This is essential for:

- Mesh-based validation  
- scientific traceability  
- reproducibility  

---

## M2.1 Scope Summary (STRICT)

DO:  
- define ASC.FMI  
- implement DiagnosticRecord  
- write records in AstronoTruth  

DO NOT:  
- implement lifecycle  
- implement retry logic  
- implement aggregation  
- implement certification  

---

## Final Principle

The system must never fail silently.  
Every non-generated dataset must be explainable.
