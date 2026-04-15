# Measurement Model – Horizons Mapping (M1 Freeze)

## 1. Overview

This document defines the measurement model used in AstronoSphere M1.

Core principle:

- Scenario = physical reality
- Measurement = observation of that reality

Measurement is defined as:

    Measurement = Level + Instrument

Extended model:

    Truth = f(Level, Instrument, Observer, Frame)

---

## 2. Dimensions

### Level (Physics Depth)

- L0 → geometric (no correction)
- L1 → light-time (LT)
- L2 → light-time + aberration (LT+S)

Mapped to Horizons:

- L0 → VECT_CORR = NONE / omitted
- L1 → VECT_CORR = LT
- L2 → VECT_CORR = LT+S

---

### Instrument (Output Representation)

- VECTORS → XYZ + Velocity
- RADEC   → Right Ascension / Declination
- ALTAZ   → (future)

Mapped to Horizons:

- VECTORS → EPHEM_TYPE = VECTORS
- RADEC   → EPHEM_TYPE = OBSERVER

Measurement may include provider-specific parameters such as:

- QUANTITIES (for observer outputs)
- output filters

These belong to the measurement configuration but are provider-dependent.

---

### Geometry (from Scenario)

- Observer → CENTER
- Frame    → REF_PLANE + REF_SYSTEM

Geometry is defined by the Scenario (Observer).

Measurement must not redefine geometry, but it must be aware of it when constructing requests.

---

## 3. Conceptual Model

```
                    INSTRUMENT DIMENSION
        --------------------------------------------
         VECTORS        | RADEC         | ALTAZ
        --------------------------------------------
L0  →   (1)            | (2)           | (3)
L1  →   (4)            | (5)           | (6)
L2  →   (7)            | (8)           | (9)
```

Each cell represents a **measurement point**.

---

## 4. Interpretation

Each measurement point corresponds to:

- one Horizons request
- one dataset
- one unique configuration

Example:

    Ephemeris + Horizons + L1 + VECTORS

---

## 5. Separation of Concerns

### Engine

- computes physics
- produces internal state (XYZ)
- transforms to RA/DEC internally

### TruthFactory

- performs measurement
- calls Horizons
- produces datasets

---

## 6. Key Insight

The system samples reality across three axes:

- Level (physics depth)
- Instrument (representation)
- Geometry (observer/frame)

This forms a structured measurement space.

---

## 7. Status (M1)

Implemented:

- Levels: L0, L1, L2
- Instruments: VECTORS, RADEC

Not implemented:

- ALTAZ
- L3–L5

---

## 8. DatasetHeader Structure

DatasetHeader.Measurement:

- Level
- Instrument
- Geometry (resolved)
- Frame
- Provider
- RequestHash

## 8. Measurement Identity

Each MeasurementDefinition must provide a canonical MeasurementKey.

Example:

    L1-VECTORS-GEO-ECLIPTIC

This key is used for:

- dataset identification (logical)
- SOLL/IST comparison in factories
- deterministic dataset planning


## 9. Conclusion

This model defines the foundation of the AstronoSphere validation system:

- deterministic
- extensible
- physically meaningful
