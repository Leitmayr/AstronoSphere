# Measurement & Instrument Definition – AstronoSphere (M1 Freeze Draft)

Version: 1.0  
Status: Architecture Clarification (M1)  
Scope: AstronoMeasurement / TruthFactory / Engine Interface  

---

## 1. Purpose

This document defines the **Measurement abstraction** within AstronoSphere.

It formalizes how:

- correction levels (L0–L5)
- output representations (Vectors, RA/DEC, etc.)

are combined into a single, consistent concept.

This document complements:

- Observer Concept Clarification
- Ephemerides Pipeline L0–L5 Freeze

---

## 2. Core Concept

Measurement defines:

> **How a scenario is observed and represented**

It consists of two orthogonal dimensions:

```
Measurement = Level + Instrument
```

---

## 3. Measurement Dimensions

### 3.1 Level (Physics Depth)

Level defines the **physical realism** of the computation.

| Level | Meaning |
|------|--------|
| L0 | Geometric |
| L1 | + Light-Time |
| L2 | + Aberration |
| L3 | + Relativity |
| L4 | + Topocentric effects |
| L5 | + Refraction |

Level corresponds directly to the **ephemerides pipeline**.

---

### 3.2 Instrument (Representation)

Instrument defines:

> **What is measured / returned**

It is independent of physics.

---

#### M1 Instrument Types

```
Instrument.Type:

- VECTORS
- RADEC
```

---

#### Future Extension (planned)

```
- ALTAZ
```

---

## 4. Separation Principle

Measurement must NOT be confused with:

| Concept | Responsibility |
|--------|----------------|
| Scenario | physical setup |
| Observer | origin |
| Frame | coordinate system |
| Measurement | output + physics depth |

---

## 5. Mapping to Engine

The engine computes:

```
State (XYZ)
 → Representation (RA/DEC, etc.)
```

Rules:

- Level affects the **state**
- Instrument affects the **representation**

---

## 6. Mapping to Horizons

### 6.1 Instrument Mapping

| Instrument | EPHEM_TYPE |
|-----------|-----------|
| VECTORS | VECTORS |
| RADEC | OBSERVER |

---

### 6.2 Level Mapping

| Level | VECT_CORR |
|------|-----------|
| L0 | NONE |
| L1 | LT |
| L2 | LT+S |
| L3+ | LT+S (approximation) |

---

## 7. DatasetHeader Representation

Measurement must be explicitly stored:

```
DatasetHeader:
    Level
    Instrument
```

This defines the **actual measurement performed (IST)**.

---

## 8. Factory Responsibility

Factories:

- read Scenario
- apply Measurement
- request data from provider

Factories must NOT:

- interpret physics
- modify Scenario

---

## 9. Engine Responsibility

Engine:

- computes according to Level
- transforms according to Instrument

Engine must NOT:

- depend on provider
- know measurement source

---

## 10. Design Rules

### Rule 1

Measurement is mandatory for every dataset.

---

### Rule 2

Measurement must not be stored in Scenario Core.

---

### Rule 3

Level and Instrument must remain orthogonal.

---

### Rule 4

Instrument must not change physics.

---

### Rule 5

Level must not define representation.

---

## 11. Architectural Outcome

With Measurement:

- Scenario remains minimal
- Factory becomes generic
- Engine becomes deterministic
- Providers become interchangeable

---

## 12. Summary

Measurement introduces the missing abstraction:

```
Scenario = experiment
Measurement = observation method
Dataset = result
```

This enables:

- clean architecture
- extensibility
- reproducibility

---

End of document.
