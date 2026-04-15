# ScenarioHeaderGenerator Mapping Specification (CAND → DRAFT)
Version 1.1

## 1. Purpose
This document defines the deterministic transformation performed by the ScenarioHeaderGenerator (SHG).

Input:
- ScenarioCandidate (CAND)

Output:
- DraftScenario (DRAFT)

SHG is a pure technical component:
- no scientific interpretation
- no heuristics
- no side effects
- fully deterministic

## 2. Mapping Overview
| Draft Field | Source |
|------------|--------|
| Core | Candidate.Core (1:1) |
| ScenarioID | derived from Core |
| CoreHash | SHA256(canonical(Core)) |
| CatalogNumber | global incremental |
| Status | fixed (created/private) |

## 3. Core Handling
Core is copied without modification.

## 4. ScenarioID

### Origin Mapping
- Heliocentric → HELIO
- Geocentric → GEO
- Topocentric → TOPO
- null → NOOBS

### With Time
<Origin>-<TimeScale>-<StartJD>-<StopJD>-<StepDays>D

### Without Time
<Origin>-NOTIME[-<Target>]

## 5. CoreHash
SHA256(canonical(Core)) → first 8 hex chars

Canonicalization:
- JSON
- sorted keys
- no nulls
- UTF-8
- no whitespace

## 6. CatalogNumber
AS-000001 (incremental)

## 7. Status
{
  "maturity": "created",
  "visibility": "private"
}

## 8. Time Rules

StepDays defines temporal resolution (Δt).

Valid:

Interval:
StartJD < StopJD
StepDays > 0

Single point:
StartJD == StopJD
StepDays > 0 (no operational effect)

Invalid:
StepDays <= 0
StartJD > StopJD

## 9. Determinism
Same input → same output.

## 10. Forbidden
SHG must not:
- interpret data
- modify core
- add metadata

## 11. Summary
SHG assigns identity, not meaning.
