# DatasetHeader Specification – M1 Freeze

## 1. Purpose
Defines the canonical DatasetHeader structure for AstronoSphere M1.

---

## 2. Core Principle

Dataset = Scenario + Measurement + Provenance

---

## 3. Structure

```json
{
  "ScenarioID": "...",
  "DatasetID": "...",
  "Measurement": {
    "Level": "L0",
    "Instrument": "VECTORS",
    "Geometry": "Helio",
    "Frame": "Ecliptic"
  },
  "Provenance": {
    "Provider": "Horizons",
    "RequestHash": "...",
    "Source": "NASA/JPL Horizons"
  }
}
```

---

## 4. Rules

- ScenarioID is immutable
- DatasetID = ScenarioID + minimal Measurement identity
- MeasurementKey must be deterministic
- RequestHash must represent full external request

---

## 5. MeasurementKey

Example:

    L0-VECTORS-HELIO-ECLIPTIC

Used for:
- SOLL/IST comparison
- dataset planning
- reproducibility

---

## 6. DatasetID (KISS)

Recommended:

    <ScenarioID>--L0-VEC

---

## 7. Responsibilities

Scenario:
- defines physics

Measurement:
- defines observation

Factory:
- executes request

DatasetHeader:
- stores truth

---

## 8. Final Statement

This structure guarantees:
- determinism
- reproducibility
- extensibility
