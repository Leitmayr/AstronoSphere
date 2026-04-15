# AstronoSphere – Meta Interaction Model

## Post M1.9 Architectural Definition

---

# 1. Purpose

This document defines the **interaction model and operational modes** of AstronoSphere after M1.9.

It consolidates:

* system roles
* user interaction patterns
* state model
* use case abstraction

Goal:

> Establish a stable, minimal, and scalable conceptual model for all future extensions.

---

# 2. Core System Principle

AstronoSphere is **not a pipeline**.

It is:

> A deterministic, reproducible exploration system for astronomical state space.

---

# 3. Fundamental Separation of Concerns

| Component          | Responsibility                                |
| ------------------ | --------------------------------------------- |
| AstronoCert        | Experiment identity (CoreHash, ID)            |
| AstronoMeasurement | Measurement definition                        |
| AstronoTruth       | GroundTruth generation                        |
| Astronometria      | Simulation + heuristic exploration            |
| Astronolysis       | Planning, orchestration, statistical analysis |

---

# 4. State Model

## 4.1 Layered State Definition

AstronoSphere operates on three distinct state layers:

### 1. Scientific State (Experiment Core)

* physically meaningful
* deterministic
* hash-relevant

```
Core:
  Time
  Observer
  ObservedObject
  Frame
  Corrections
```

---

### 2. Observation State

```
ObservationState:
  Core
  Measurement
```

Represents a **fully defined physical observation**.

---

### 3. Session State (Astronometria)

```
SessionState:
  ObservationState
  View
  UIContext
```

Examples:

* projection
* zoom
* camera position
* UI selections

---

## 4.2 Critical Rule

> SeedCore ⊂ ObservationState ⊂ SessionState

---

## 4.3 Determinism Constraint

* Only **Core** participates in hashing
* SessionState MUST NOT influence CoreHash

---

# 5. State Flow

## 5.1 Interaction Flow

```
User Action → State Update → Rendering
```

---

## 5.2 Reproduction Flow

```
Experiment → State Reconstruction → Rendering
```

---

## 5.3 Design Rule

> Rendering is always derived from State
> Never the other way around

---

# 6. Multi-State Model

## 6.1 Structure

```
ObservationSession:
  ActiveStateId
  States[]
```

Each State:

```
State:
  StateId
  ObservationState
  View
  Label
```

---

## 6.2 Constraints

* Each State must be fully valid
* No delta-based states
* No implicit inheritance

---

## 6.3 Purpose

Multi-State enables:

* comparison
* visualization of effects
* didactic representation

---

# 7. System Modes (Core Abstraction)

All system interaction reduces to **three fundamental modes**.

---

## 7.1 Validation Mode

### Purpose

* ensure correctness
* guarantee determinism
* achieve full coverage

---

### Driver

Astronolysis

---

### Process

```
FOR each Experiment
  FOR each Measurement

    IF GroundTruth missing → AstronoTruth
    IF Simulation missing → Astronometria

    → Compare results
```

---

### Characteristics

* deterministic
* exhaustive
* batch-oriented

---

---

## 7.2 Sensitivity Mode

### Purpose

* understand cause-effect relationships
* analyze parameter influence

---

### Driver

Astronometria (expert usage)

---

### Process

```
Baseline State → Variant States → Comparison
```

---

### Example Variations

* LightTime ON/OFF
* Aberration ON/OFF
* Frame change
* Observer change

---

### Characteristics

* targeted
* interactive
* analytical

---

---

## 7.3 Didactic Mode

### Purpose

* explain effects visually
* enable intuitive understanding

---

### Driver

Astronometria + Multi-State

---

### Process

```
Predefined State Variations
→ simultaneous rendering
→ visual comparison
```

---

### Characteristics

* visual
* guided
* educational

---

---

# 8. Use Case Mapping

All identified use cases map to the three system modes:

| Use Case              | Mode                     |
| --------------------- | ------------------------ |
| Validation / Coverage | Validation               |
| Regression Testing    | Validation               |
| Truth vs Simulation   | Validation               |
| Reproducibility       | Validation               |
| Debug / Root Cause    | Sensitivity              |
| Manual Discovery      | Sensitivity              |
| Coverage Expansion    | Validation + Sensitivity |
| Automated Discovery   | Validation (extended)    |
| Didactics / Teaching  | Didactic                 |
| Presentation          | Didactic                 |
| Scenario Exploration  | Sensitivity + Validation |

---

## Key Result

> 12 Use Cases collapse into 3 fundamental system modes

---

# 9. Discovery Model

AstronoSphere operates as a closed loop:

```
Astronolysis → detects anomaly
→ Astronometria → interprets
→ new Seeds → new Experiments
→ back to Astronolysis
```

---

## Two Complementary Exploration Types

### 1. Statistical Exploration

* Astronolysis-driven
* broad, exhaustive

---

### 2. Heuristic Exploration

* Astronometria-driven
* targeted, experience-based

---

---

# 10. Astronometria Role (Clarified)

Astronometria operates in two modes:

---

## 10.1 Validation Mode

* driven by Astronolysis
* deterministic execution

---

## 10.2 Productive / Heuristic Mode

* standalone usage
* exploratory interaction
* seed generation

---

## Critical Constraint

> Heuristics must never directly modify the pipeline

---

---

# 11. Seed Generation from State

Astronometria can produce:

```
ObservationState → SeedCore
```

---

## Properties

* deterministic
* reproducible
* user-correctable before commit

---

---

# 12. Hashing Strategy

## 12.1 CoreHash

* based on Experiment Core
* stable
* scientific identity

---

## 12.2 Optional SessionHash

* based on SessionState
* NOT part of pipeline
* used for UI/session restore

---

---

# 13. Key Design Principles

---

## Separation of Concerns

* Experiment ≠ Measurement ≠ View

---

## Determinism First

* reproducibility is mandatory

---

## Explicit State

* no implicit behavior
* no UI-derived logic

---

## Orthogonality

* dimensions must remain independent

---

## KISS

* minimal abstractions
* no premature complexity

---

---

# 14. Final Statement

AstronoSphere combines:

* deterministic engineering (trust)
* statistical exploration (coverage)
* heuristic reasoning (understanding)

---

> Enabling controlled discovery in a high-dimensional astronomical state space.

---

# END
