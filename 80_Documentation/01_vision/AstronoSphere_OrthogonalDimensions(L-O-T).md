# AstronoSphere – Orthogonal Dimensions (L–O–T)

## 1. Purpose

This document defines the **three orthogonal dimensions** of astronomical computation in AstronoSphere:

* **L (Ephemeris / Location of bodies)**
* **O (Observer / Observation geometry)**
* **T (Time / Time systems and transformations)**

The goal is to establish a **clean, testable, and extensible architecture** that separates concerns and enables deterministic validation.

---

## 2. Core Principle

> Astronomical computation is the combination of three independent dimensions:
>
> **Where is the object? (L)**
> **From where is it observed? (O)**
> **At what time? (T)**

These dimensions must be:

* **orthogonal**
* **independently testable**
* **combined only at measurement level**

---

## 3. Dimension Definitions

### 3.1 L – Ephemeris Dimension

Represents the **physical state of celestial bodies**.

Includes:

* planetary positions (VSOP, DE, …)
* coordinate transformations (frame pipeline)
* correction pipeline (L0, L1, L2, …)

Output:

* geometric or apparent **state vectors**

---

### 3.2 O – Observer Dimension

Represents the **observation point and geometry**.

Includes:

* observer type:

  * heliocentric
  * geocentric
  * topocentric
* location:

  * latitude
  * longitude
  * elevation
* Earth model:

  * spherical → ellipsoid → geoid (future)

Output:

* transformation from global coordinates to **observer-relative coordinates**

---

### 3.3 T – Time Dimension

Represents the **time system and conversions**.

Includes:

* time scales:

  * UTC
  * TT
  * TDB
  * UT1 (future)
* ΔT models
* sidereal time

Output:

* consistent **time representation in Astro Domain**

---

## 4. Architectural Role of T

T is **not a pipeline**, but a **service layer**.

Responsibilities:

* time conversion (UTC ↔ TT ↔ TDB)
* Julian date handling
* sidereal time computation

Constraint:

> No mixing of time domains inside L or O.

---

## 5. Orthogonality Rules

### 5.1 Separation

* L must not depend on O
* O must not depend on L
* T must not be embedded in either

---

### 5.2 Combination Point

The three dimensions are combined only at:

> **Measurement**

This is formalized via **AstronoMeasurement**.

---

## 6. Measurement as Integration Layer

A measurement defines:

* Level (L0…Lx)
* Instrument (VEC, RADEC, …)

Measurement combines:

```
Result = f(L, O, T)
```

This ensures:

* consistent interpretation
* comparable datasets
* deterministic reproducibility

---

## 7. Pipeline Mapping

### 7.1 L-Pipeline

* VSOP / DE input
* Frame pipeline
* Correction pipeline

---

### 7.2 O-Pipeline (future)

* observer definition
* coordinate projection
* topocentric transformation

---

### 7.3 T-Service

* conversion layer
* ΔT provider
* sidereal time provider

---

### 7.4 Multiple Pipelines

The Ephemeris Pipeline is one pipeline running in AstronoSphere.
The Observer Pipeline will become a second pipeline. It will reqire an own "Builder" like AstronoLab is a Builder for Ephemeris Experiments.

---

## 8. Orchestrator Role

An **Orchestrator** coordinates the dimensions:

Responsibilities:

1. Request time from T
2. Request state from L
3. Apply observer transformation (O)
4. execute measurement

This is the only place where all dimensions interact.

---

## 9. Validation Strategy

Each dimension can be validated independently:

### L Validation

* vs Horizons (VECTORS)
* mesh validation

### O Validation

* observer transformations
* topocentric consistency

### T Validation

* ΔT tables
* sidereal time reference

---

## 10. Design Advantages

* strict separation of concerns
* high testability
* deterministic behavior
* extensibility (e.g. DE440, new observer models)
* compatibility with external ground truth systems

---

## 11. Long-Term Vision

The L–O–T model enables:

* high-precision event computation
* observer-dependent phenomena (eclipses, transits, ISS passes)
* cross-model comparison with full traceability

---

## 12. Summary

AstronoSphere decomposes astronomical computation into:

* **L** → physical reality
* **O** → observation geometry
* **T** → temporal reference

These are combined only through **measurement**, ensuring:

> **clarity, reproducibility, and scientific trust**

