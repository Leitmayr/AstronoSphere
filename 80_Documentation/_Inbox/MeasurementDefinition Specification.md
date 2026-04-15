# AstronoSphere – MeasurementDefinition Specification (M1.9+)

## 1. Purpose

MeasurementDefinition defines **how an Experiment is observed**.

It is a first-class, versionable, reusable entity.

It guarantees:

* explicit measurement semantics
* reuse across multiple Experiments
* deterministic dataset generation
* decoupling from Experiment creation

---

## 2. Core Principle

MeasurementDefinition is NOT execution logic.

It is a **declarative description** of a measurement.

Execution is performed by:

* AstronoTruth (GroundTruth)
* Astronometria (Simulation)

---

## 3. Conceptual Model

MeasurementDefinition
→ defines measurement intent

CanonicalRequest
→ defines executable request

Relationship:

CanonicalRequest = f(Experiment.Core + MeasurementDefinition + Defaults)

---

## 4. Data Structure (FINAL)

```json
{
  "MeasurementDefinition": {
    "MeasurementId": "MD-L0-VEC-001",
    "Version": "1.0.0",

    "Level": "L0",
    "Type": "VEC",

    "Parameters": {
      "RefPlane": "ECLIPTIC",
      "RefSystem": "ICRF",
      "OutputUnits": "AU-D"
    },

    "Description": "Heliocentric geometric state vectors (no corrections)",

    "CreatedAtUtc": "2026-04-10T00:00:00Z"
  }
}
```

---

## 5. Field Definitions

### 5.1 MeasurementId

* unique identifier
* immutable
* used for referencing

Format:

MD-<Level>-<Type>-<RunningNumber>

Example:

MD-L0-VEC-001

---

### 5.2 Version

* semantic versioning (MAJOR.MINOR.PATCH)
* required for reproducibility
* MUST change if behavior changes

---

### 5.3 Level

Defines correction level:

* L0 → geometric
* L1 → light-time
* L2 → aberration
* ...

---

### 5.4 Type

Defines measurement type:

* VEC → state vectors
* RADEC → spherical coordinates
* ALTAZ → topocentric (future)

---

### 5.5 Parameters (CRITICAL)

Defines additional measurement configuration.

RULES:

* MUST be fully explicit
* MUST be deterministic
* MUST be canonicalizable
* MUST be included in CanonicalRequest
* MUST affect RequestHash

Examples:

* RefPlane
* RefSystem
* OutputUnits
* ObserverMode (future)
* Quantities (for OBSERVER)

---

### 5.6 Description

* human-readable
* informational only
* MUST NOT affect hashing

---

### 5.7 CreatedAtUtc

* informational only
* MUST NOT affect hashing

---

## 6. Storage Model

Location:

AstronoData/
04_MeasurementDefinitions/
Created/
Released/
Processed/

Rules:

* same lifecycle pattern as Seeds
* only Released definitions are allowed in pipeline
* AstronoMeasurement is the ONLY writer

---

## 7. Integration into Pipeline

### Phase 1

Seeds → Experiments (AstronoCert)

STOP

---

### Phase 2

MeasurementDefinitions created/selected

(AstronoMeasurement)

---

### Phase 3

Execution:

FOR each Experiment
FOR each MeasurementDefinition

```
CanonicalRequest =
  merge(
    Experiment.Core,
    MeasurementDefinition,
    Defaults
  )

→ AstronoTruth
→ Astronometria
```

---

## 8. Canonicalization Rules

MeasurementDefinition MUST be transformed into CanonicalRequest:

Example mapping:

| MeasurementDefinition | CanonicalRequest     |
| --------------------- | -------------------- |
| Level = L0            | VECTOR_CORR = NONE   |
| Type = VEC            | EPHEM_TYPE = VECTORS |
| RefPlane              | REF_PLANE            |
| RefSystem             | REF_SYSTEM           |

RULES:

* mapping MUST be deterministic
* mapping MUST be centralized
* mapping MUST NOT depend on runtime context

---

## 9. Hashing Rules

RequestHash is computed from:

CanonicalRequest ONLY

BUT:

MeasurementDefinition MUST fully influence CanonicalRequest

Therefore:

MeasurementDefinition → indirectly part of hash

---

## 10. Determinism Rules

* identical MeasurementDefinition → identical CanonicalRequest
* identical CanonicalRequest → identical RequestHash
* identical inputs → identical dataset

---

## 11. Forbidden Behaviors

* implicit defaults in MeasurementDefinition
* partial definitions
* runtime modification of MeasurementDefinition
* environment-dependent behavior
* missing parameters
* hidden mappings

---

## 12. Design Principles

### Separation of Concerns

Experiment → WHAT exists
MeasurementDefinition → HOW it is observed

---

### Determinism First

No randomness
No implicit logic
No hidden defaults

---

### KISS

Simple structures
Explicit fields
No over-engineering

---

## 13. Future Extensions

* GUI-based editor (AstronoMeasurement)
* Parameter presets
* Expert-defined measurement templates
* multi-instrument comparisons

---

## 14. Final Principle

MeasurementDefinition is the **contract between intent and execution**.

It must be:

* explicit
* deterministic
* reproducible
* versionable

---

END OF DOCUMENT
