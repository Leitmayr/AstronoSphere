# CM-05 Baseline Strategy

## 1. Purpose

This document defines the baseline strategy for the AstronoSphere repository.

The goal is to:

- ensure scientific reproducibility
- define stable reference states
- enable reliable comparison between system versions

---

## 2. Core Principle

> A baseline represents a fully validated, reproducible, and scientifically consistent state of AstronoSphere.

---

## 3. Definition of a Baseline

A baseline is defined by:

- a specific Git commit (main branch)
- a consistent set of reference data
- successful validation against all HolyScenarios

---

## 4. Baseline Criteria

A baseline is created only if all conditions are met:

### 4.1 Scientific Validation (Mandatory)

> All HolyScenarios must pass within defined tolerances.

---

### 4.2 System Integrity

- all projects build successfully
- pipeline runs without errors
- no broken dependencies

---

### 4.3 Data Consistency

- ReferenceData is complete
- EngineData matches ReferenceData within tolerances

---

## 5. Baseline Creation

Baselines are created:

- after merging a feature into `main`
- only when validation is successful

---

### 5.1 Git Tagging

Each baseline is marked with a Git tag:

```
baseline/vX.Y
```

Example:

```
baseline/v1.0
baseline/v1.1
```

---

## 6. Reference Data Handling

ReferenceData is part of the baseline.

Rules:

- ReferenceData must be versioned
- changes to ReferenceData require re-validation
- ReferenceData must not be modified retroactively

---

## 7. Run / LastRun Concept

For validation runs:

```
Run      → newly generated data
LastRun  → previous baseline data
```

Comparison determines:

- correctness
- regression
- release readiness

---

## 8. Release Decision

A new baseline is accepted if:

- Run == LastRun (within tolerance)
- or justified scientific improvement exists

---

## 9. Traceability

Each baseline must allow reconstruction of:

- code state (Git commit)
- data state (ReferenceData)
- validation result (HolyScenarios)

---

## 10. Design Benefits

This strategy ensures:

- reproducible scientific results
- controlled evolution of the system
- reliable regression detection

---

## 11. Summary

The baseline strategy:

- defines scientifically validated system states
- uses HolyScenarios as validation gate
- integrates code and data into a single reproducible unit

---

End of document.
