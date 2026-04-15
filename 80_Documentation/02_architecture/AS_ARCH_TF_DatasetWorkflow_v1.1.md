# AS_ARCH_DatasetWorkflow_v1.1

## 1. Purpose

This document defines the dataset generation workflow for TruthFactories in AstronoSphere (M1 scope).

Goal:
- eliminate manual pipeline control
- ensure deterministic dataset generation
- keep Scenario free of measurement logic

---

## 2. Core Principle

Scenario defines the physical experiment.

Measurement is NOT part of the Scenario.

Factory reconciles desired vs existing datasets.

---

## 3. Default Measurement Behavior (M1)

If no MeasurementDefinition exists, the Factory applies a default configuration.

DefaultLevels = ["L0"]

Notes:
- Defined in code (not in data)
- Applies to all scenarios
- Represents minimal geometric baseline

---

## 4. Factory Behavior

For each Scenario:

existing = DatasetRepository.GetDatasets(scenarioId)

levels = DefaultLevels

missing = levels - existing

for level in missing:
    generateDataset(scenarioId, level)

---

## 5. Dataset Semantics

Dataset = Scenario + Measurement

Measurement information is stored exclusively in the DatasetHeader.

Scenario Core remains unchanged.

DatasetHeader represents the actual measurement state (IST).

---

## 6. Design Rules

- DefaultLevels are defined in code, not in data
- Scenario must not contain measurement logic
- Factory must not define measurement strategy
- MeasurementDefinition is optional (introduced in M2)
- DatasetHeader is the single source of truth for measurement state

---

## 7. Evolution Path

M1:
    DefaultLevels = ["L0"]

M1.5:
    DefaultLevels can be extended (e.g. ["L0","L1"]) without schema changes

M2:
    if MeasurementDefinition exists:
        use MeasurementDefinition
    else:
        fallback to DefaultLevels

---

## 8. Architectural Benefits

- fully data-driven pipeline
- no CLI configuration required
- idempotent execution
- minimal maintainer effort
- forward compatible with MeasurementDefinition

---

## 9. Key Principle

The Factory reconciles:

SOLL = DefaultLevels or MeasurementDefinition
IST  = existing datasets

→ generate only missing datasets

---

End of document.
