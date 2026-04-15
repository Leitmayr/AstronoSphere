# CM-06 Data Versioning Strategy

## 1. Purpose

This document defines how data within AstronoSphere is versioned.

The goal is to:

- ensure reproducibility of scientific results
- maintain consistency between code and data
- enable reliable regression analysis

---

## 2. Core Principle

> Data is versioned together with code and is part of the system state.

---

## 3. Versioned Data Domains

The following data domains are versioned:

- ObservationCatalog
- ReferenceData
- (optionally) EngineData for analysis

Primary focus:

> ReferenceData is the authoritative baseline data.

---

## 4. Versioning Mechanism

### 4.1 Git-Based Versioning

- all data resides inside the monorepo
- data is versioned via Git commits
- baselines are identified via Git tags

---

### 4.2 Baseline Binding

Each baseline implicitly defines:

- code version (commit hash)
- data version (repository state)

> No separate data versioning system is used.

---

## 5. ReferenceData Rules

### 5.1 Immutability

> Released ReferenceData must never be modified.

If changes are required:

- create new data
- generate new baseline

---

### 5.2 Structure

```
ReferenceData/
|
+- Runs/
|   +- Run
|   +- LastRun
|
+- Released/
    +- <baseline-id>
```

---

## 6. Run / LastRun Workflow

- Run = newly generated data
- LastRun = previous baseline snapshot

Comparison:

- validates correctness
- detects regressions

---

## 7. Promotion to Released

Data is moved from Run to Released if:

- HolyScenarios pass
- results are consistent or improved

---

## 8. Data Consistency Rule

> Code and data must always match within a baseline.

Forbidden:

- using old data with new code without validation
- mixing data from different baselines

---

## 9. External Data Sources

If external sources are used (e.g. Horizons):

- results must be stored locally
- external dependency must not affect reproducibility

---

## 10. Traceability

Each dataset must be traceable to:

- baseline tag
- generation method
- source parameters

---

## 11. Design Benefits

This strategy ensures:

- full reproducibility
- deterministic pipeline execution
- independence from external systems

---

## 12. Summary

The data versioning strategy:

- uses Git as single source of truth
- treats data as part of the system state
- enforces immutability of released data
- guarantees reproducibility

---

End of document.
