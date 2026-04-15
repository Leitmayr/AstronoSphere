# AstronoSphere – Experiment vs Measurement Definition (M1.9 Freeze)

## 1. Purpose

This document defines the conceptual separation between:

- Experiment (Seed / Core)
- Measurement (L-level, Instrument)

This separation is fundamental for:
- determinism
- reproducibility
- clean architecture
- future extensibility

---

## 2. Core Principle

> An Experiment defines the physical scenario.
> A Measurement defines how the scenario is observed.

---

## 3. Experiment Definition

An Experiment describes a **pure geometric-physical configuration**.

It includes:

- Time (StartJD, StopJD, Step, TimeScale)
- Observer (Origin: Helio, Geo, later Topo)
- Target (BodyClass, Targets)
- Frame (Type, Epoch)

The Experiment answers:

> "Where is the object in space at time t?"

---

## 4. What is NOT part of the Experiment

The Experiment does NOT include:

- Light-time correction
- Aberration
- Relativistic effects
- Any observational effects

These are NOT properties of reality, but of observation.

---

## 5. Measurement Definition

Measurement defines how the Experiment is observed.

It includes:

- Level (L0, L1, L2, ...)
- Instrument (VEC, RADEC, ...)

Examples:

- L0 → geometric position
- L1 → light-time corrected position
- L2 → apparent position

---

## 6. Role of Corrections

Corrections belong exclusively to Measurement.

They modify the interpretation of the same Experiment:

- L0 → r(t_obs)
- L1 → r(t_emit)
- L2 → apparent position

Corrections do NOT change the Experiment.

---

## 7. Observer Clarification

The Observer is part of the Experiment.

Reason:

Changing the observer changes the physical scenario:

- Sun → Mars (heliocentric)
- Earth → Mars (geocentric)

These are different realities, not different measurements.

---

## 8. Mental Model

Experiment = Stage (scene)
Measurement = Camera
Corrections = Camera settings

Same scene, different images.

---

## 9. Architectural Rule

Observer ∈ Experiment  
Corrections ∈ Measurement

---

## 10. M1.9 Simplification

In M1.9:

- Only L0 is used
- Therefore all Corrections are implicitly OFF
- Corrections are not stored in Seeds

This keeps Seeds minimal and deterministic.

---

## 11. Benefits

This separation ensures:

- deterministic CoreHash
- comparability across datasets
- no conflicting definitions
- extensibility for future levels

---

## 12. Final Definition

An Experiment is a complete, observer-dependent physical scenario.

A Measurement is a transformation applied to that scenario to simulate observation.

---

End of document.
