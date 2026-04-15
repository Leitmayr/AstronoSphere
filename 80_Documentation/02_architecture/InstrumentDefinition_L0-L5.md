# Instrument Definition L0–L5 (Freeze v1)

## 1. Purpose

This document defines the official measurement instruments of AstronoSphere.

An instrument specifies how a physical scenario is measured.
It provides a stable, system-wide semantic definition used by:

- Astronometria (computation)
- TruthFactories (reference data generation)
- Validation pipeline

This document establishes a **normative definition** of correction levels L0–L5.

---

## 2. Conceptual Model

A measurement is defined as:

Measurement = (Scenario, Instrument)

Where:

- Scenario defines the physical experiment
- Instrument defines how the measurement is performed

---

## 3. Correction Levels (L0–L5)

### L0 — Geometric

Definition:

- Pure geometric state vector
- No physical corrections applied

Includes:

- VSOP / base ephemeris
- frame transformations

Excludes:

- Light-Time
- Aberration
- Relativistic effects
- Observer effects
- Refraction

---

### L1 — Light-Time

Definition:

- L0 +
- Light-Time correction

Physical meaning:

- Position evaluated at emission time (t_emit)

Reference:

- Light-Time Fundamental Concept

---

### L2 — Aberration

Definition:

- L1 +
- Aberration of light

Physical meaning:

- Apparent shift due to observer motion

---

### L3 — Relativistic Deflection

Definition:

- L2 +
- Relativistic light deflection

Physical meaning:

- Gravitational bending of light (primarily by Sun)

---

### L4 — Topocentric

Definition:

- L3 +
- Observer location effects

Includes:

- Earth rotation
- observer position on Earth

---

### L5 — Refraction

Definition:

- L4 +
- Atmospheric refraction

Physical meaning:

- bending of light in Earth's atmosphere

---

## 4. Orthogonality Principle

Correction levels are strictly cumulative:

L0 → L1 → L2 → L3 → L4 → L5

Each level adds exactly one physical effect.

---

## 5. Measurement Domain

In addition to correction level, each measurement operates on a domain.

### Domains

- TARGET
  Measurement of celestial object state vectors

- OBSERVER
  Measurement of observer-related quantities

- TARGET+OBSERVER
  Combined measurement involving both

---

## 6. Separation of Concerns

- CorrectionLevel defines the physical depth of the model
- MeasurementDomain defines what is being measured

These dimensions are independent.

---

## 7. Architectural Implications

- Instruments are shared across all system components
- Factories map instruments to external API parameters
- Astronometria maps instruments to internal correction pipeline

No component may redefine instrument semantics.

---

## 8. Summary

AstronoMeasurement introduces:

- a stable definition of L0–L5
- a clear separation between physics and measurement
- a system-wide semantic contract

This is a foundational element for reproducible astronomical computation.

---

End of document.
