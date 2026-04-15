# CM-02 Project Numbering

## 1. Purpose

This document defines the numbering convention used in the AstronoSphere repository.

The goal is to:

- create a deterministic and intuitive structure
- reflect the processing pipeline in the project layout
- reduce cognitive load
- ensure long-term stability

---

## 2. Core Principle

The numbering system represents **meaning, not ordering convenience**.

> Numbers encode the role of a component within the system.

---

## 3. Numbering Domains

### 3.1 Pipeline (00–09)

Represents the execution flow of AstronoSphere.

| Number | Component | Role |
|--------|----------|------|
| 00 | AstronoPipe | Orchestration |
| 01 | Scenario Factory | Candidate generation |
| 02 | Header Generator | Scenario finalization |
| 03 | TruthFactory | Reference data generation |
| 04 | Astronometria | Engine computation |
| 05 | AnalysisTool | Result analysis |

Rules:

- numbers reflect execution order
- pipeline is strictly directional

---

### 3.2 Data Mapping (01–05)

Each pipeline step maps directly to a data container.

| Code | Data |
|------|------|
| 01 | 01_CandidateData |
| 02 | 02_ObservationCatalog |
| 03 | 03_ReferenceData |
| 04 | 04_EngineData |
| 05 | 05_AnalysisData |

Principle:

> Each processing step produces exactly one primary data domain.

---

### 3.3 Data Access Layer (10–19)

| Number | Component |
|--------|----------|
| 10 | AstronoData.Contracts |
| 11 | AstronoData.IO |

Purpose:

- define external access to data
- isolate file system logic

---

### 3.4 Documentation (80–89)

| Number | Component |
|--------|----------|
| 80 | Documentation |

Documentation is explicitly separated from runtime components.

---

## 4. Stages vs Implementations

A numbered component represents a **stage**, not a single implementation.

> A stage may contain multiple implementations of the same role.

Example:

- 03_TruthFactory may include multiple concrete factories (e.g. Horizons, SPICE, Meeus)

Rules:

- implementations within a stage must not depend on each other
- implementations are interchangeable
- orchestration decides which implementation is used

---

## 5. Stability Rules

### 5.1 Immutability

> Once assigned, a number must never change.

This ensures:

- stable references
- consistent Git history
- long-term traceability

---

### 5.2 No Reordering

Components are never renumbered.

If a new component is introduced:

- assign a new number
- do not insert between existing numbers

Example:

```
06_NewComponent.pj
```

---

### 5.3 No Fractional or Derived Numbers

Forbidden patterns:

- 01.5_Component
- 02b_Component

---

## 6. Semantic Meaning of Numbers

Numbers encode **responsibility and position in the system**, not just sorting.

Example:

- "03" always refers to TruthFactory-related logic
- "04" always refers to engine computation

---

## 7. Separation of Concerns

The numbering enforces architectural boundaries:

- 00–05 → processing pipeline
- 10–19 → data access
- 80–89 → documentation

---

## 8. Design Benefits

The numbering system provides:

- immediate understanding of system flow
- direct mapping between code and data
- simplified debugging
- reduced onboarding effort

---

## 9. Summary

The numbering convention is a core structural element of AstronoSphere.

It:

- reflects the pipeline
- encodes system semantics
- ensures stability
- supports long-term maintainability

---

End of document.
