# AstronoMeasurement Concept

## 1. Rationale

AstronoSphere separates responsibilities into distinct system layers:

- Astronometria (computation)
- AstronoData (data storage)
- AstronoTruth (external reference generation). Inside AstronoTruth, various different TruthFactories may reside (e.g. EphemerisFactory)

This separation ensures scientific traceability and reproducibility.

However, a critical conceptual gap exists between these layers:

- An **Experiment** defines *what* physical experiment is performed.
- A **TruthFactory** defines *how* external data is retrieved.
- The **Engine** defines *how* physics is computed.

What is missing is a formal definition of:

> **How an experiment is measured.**

This gap becomes visible when multiple measurement variants are required
for the same experiment, for example:

- L0 (geometric)
- L1 (light-time corrected)
- L2 (aberration)
- ...

A single experiment can therefore produce multiple datasets depending on
the measurement interpretation.

Without a dedicated abstraction, this leads to:

- duplicated logic in factories and engine
- inconsistent interpretation of correction levels
- non-comparable datasets
- hidden configuration inside code

---

## 1.1 Introduction of Measurement Semantics

AstronoMeasurement introduces a new conceptual layer:

> **Instrument = Measurement Definition**

An instrument represents a well-defined and reusable measurement setup.

Example:

- L0 → purely geometric state vectors
- L1 → light-time corrected vectors

This concept provides:

- a stable naming scheme
- a shared semantic contract across all systems
- deterministic mapping to both:
  - engine correction pipeline
  - GroundTruth API configuration

---

## 1.2 Orthogonality Principle

AstronoMeasurement preserves the orthogonality of the architecture:

| Concept        | Responsibility              |
|----------------|-----------------------------|
| Experiment     | Physical observation        |
| Instrument     | Measurement definition      |
| GroundTruth    | Data acquisition            |
| Engine         | Physical computation        |

This ensures:

- experiment remain stable and minimal
- measurement variants do not pollute the ObservationCatalog
- factories remain generic and reusable

---

## 1.3 System-Wide Consistency

The introduction of instruments enables:

### 1. Consistent Interpretation

The same instrument identifier (e.g. "L1") has identical meaning in:

- Astronometria
- TruthFactories
- validation pipeline

### 2. Comparable Datasets

Datasets become directly comparable because they share:

- identical experiment
- identical measurement definition

### 3. Deterministic Reproducibility

Measurement configuration becomes:

- explicit
- versionable
- reproducible

---

## 1.4 Avoiding Architectural Anti-Patterns

Without AstronoMeasurement, typical anti-patterns emerge:

- embedding correction logic inside factories
- using implicit flags instead of explicit definitions
- duplicating configuration across components

AstronoMeasurement prevents these by:

- centralizing measurement semantics
- enforcing explicit configuration
- decoupling physics from measurement

---

## 1.5 Long-Term Vision

AstronoMeasurement enables future extensions:

- user-selectable instruments
- expert-defined measurement configurations
- GUI-based instrument selection
- standardized comparison across different reference sources

It establishes a foundation for:

> **scientifically consistent and reproducible measurement workflows**

---

## 1.6 Summary

AstronoMeasurement introduces a missing abstraction:

- Experiment defines the physics
- Instrument defines the measurement
- Dataset represents the result

This separation is essential for:

- scalability
- clarity
- scientific correctness

It transforms AstronoSphere from a computation system into a:

> **fully structured measurement and validation framework**
