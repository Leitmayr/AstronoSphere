# CM-03 Branching Strategy

## 1. Purpose

This document defines the branching strategy for the AstronoSphere repository.

The goal is to:

- maintain a consistent and reproducible system state
- support focused development
- ensure scientific validity of all changes

---

## 2. Core Principle

> The main branch always represents a fully consistent and scientifically valid state of the entire system.

This includes:

- all pipeline stages
- all data interactions
- all validation criteria

---

## 3. Branch Types

### 3.1 Main Branch

```
main
```

Rules:

- always stable
- always buildable
- always passes validation
- represents the current baseline of AstronoSphere

---

### 3.2 Feature Branches

```
feature/<stage>-<short-description>
```

Examples:

```
feature/03-lighttime-correction
feature/04-velocity-precision
feature/01-mars-opposition-scenarios
```

---

## 4. Stage-Oriented Development

Each feature branch has exactly one **primary stage**.

> Every feature is anchored to a single stage of the pipeline.

### 4.1 Rules

- a feature must declare a primary stage
- the branch name must include the stage number
- development is focused on this stage

---

### 4.2 Cross-Stage Changes

Changes in other stages are allowed under strict conditions:

- must be required by the primary stage
- must be minimal
- must not introduce unrelated modifications

> Cross-stage changes must be justified by the primary stage.

---

## 5. Merge Criteria

A feature branch may only be merged into `main` if all criteria are fulfilled.

---

### 5.1 Scientific Validation (Mandatory)

> All HolyScenarios must pass successfully within defined tolerances.

This is the primary merge gate.

Notes:

- HolyScenarios are defined in the ObservationCatalog
- tolerances are part of the validation specification
- this rule ensures scientific correctness of the system

---

### 5.2 System Consistency

- all projects build successfully
- the pipeline executes without errors
- no broken dependencies between stages

---

### 5.3 Scope Integrity

- changes are consistent with the declared primary stage
- no unrelated refactoring is included

---

## 6. Merge Policy

- merges are performed only after validation
- no direct commits to `main`
- all changes go through feature branches

---

## 7. Design Rationale

This strategy ensures:

- strong alignment with the pipeline architecture
- controlled evolution of the system
- reproducible scientific results

The stage-based branching model reflects the structure of AstronoSphere.

---

## 8. Summary

The branching strategy:

- enforces a stable main branch
- structures development around pipeline stages
- uses HolyScenarios as the scientific validation gate

This guarantees that AstronoSphere remains consistent, reproducible, and scientifically valid at all times.

---

End of document.
