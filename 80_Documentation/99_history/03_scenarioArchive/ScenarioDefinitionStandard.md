# Scenario Definition Standard

Scenarios define astronomical situations that can be reproduced
by factories and the Astronometria engine.

---

## Core Principle

All time definitions must use Julian Dates.

This avoids issues with historical calendar changes
(Julian vs Gregorian calendar).

---

## Scenario Fields

Required fields:

- ScenarioID
- Target
- ReferenceFrame
- CorrectionLevel
- StartJD
- StopJD
- StepJD
- TimeScale

---

## ScenarioID

ScenarioID uniquely identifies a scenario.

All datasets derived from the scenario share the same ScenarioID.

This enables full reproducibility of results.

---

## Time Definition

All scenarios must define time using:

Julian Day numbers.

Example:
StartJD = 2451545.0
StopJD = 2451546.0
StepJD = 0.125