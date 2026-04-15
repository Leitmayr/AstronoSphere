
# AstronoSphere Ephemeris Validation Framework Concept

Status: Architectural Note  
Purpose: Capture the **framework architecture idea** discussed during design sessions so it can guide later implementation.

This document describes the conceptual architecture of the **AstronoSphere Ephemeris Validation Framework**.

The goal of this architecture is to create a **reproducible scientific validation ecosystem** for ephemeris engines such as Astronometria and potential third‑party implementations.

This document is intentionally conceptual and does not define implementation details.

---

# 1 Core Idea

The AstronoSphere system separates two fundamentally different responsibilities:

1. **Experiment Definition**
2. **Experiment Execution and Evaluation**

These two subsystems are **orthogonal**.

```
Scenario Infrastructure
        ⟂
Validation Infrastructure
```

This separation enables stable scenario definitions while allowing multiple engines and datasets to evolve independently.

---

# 2 System Overview

The complete workflow can be summarized as:

```
Scenario Definition
        ↓
Ground Truth Generation
        ↓
Engine Execution
        ↓
Result Analysis
```

Each stage produces persistent data artifacts that can be reused by later stages.

```
Scenario → ReferenceData → EngineData → AnalysisData
```

---

# 3 Scenario Infrastructure

The Scenario Infrastructure defines **astronomical experiments**.

It does **not compute ephemerides**.

Its purpose is to produce deterministic experiment definitions.

## Components

```
MeeusScenarioFactory
        ↓
ScenarioCandidates
        ↓
ScenarioHeaderGenerator
        ↓
ObservationCatalog
```

### Responsibilities

| Component | Responsibility |
|----------|---------------|
MeeusScenarioFactory | generates astronomical event candidates |
ScenarioHeaderGenerator | constructs deterministic scenario headers |
ObservationCatalog | curated registry of accepted experiments |

A scenario represents a **reproducible astronomical experiment definition**.

Example:

```
Mars opposition near perihelion
Epoch: 2003‑08‑28
Observer: Earth
Target: Mars
```

The ObservationCatalog therefore acts as the **scientific experiment registry**.

---

# 4 Validation Infrastructure

The Validation Infrastructure executes experiments and evaluates results.

```
ObservationCatalog
        ↓
TruthFactory
        ↓
ReferenceData
        ↓
EphemerisEngine
        ↓
EngineData
        ↓
AnalysisTool
        ↓
AnalysisData
```

### Responsibilities

| Component | Responsibility |
|----------|---------------|
TruthFactory | generates ground truth reference datasets |
EphemerisEngine | computes ephemerides using a specific engine |
AnalysisTool | compares engine results against reference data |

This infrastructure produces the **scientific validation results**.

---

# 5 Data Layers

The system operates on four persistent data layers.

| Layer | Description |
|-----|-------------|
Scenario | experiment definition |
ReferenceData | ground truth data |
EngineData | results produced by an engine |
AnalysisData | statistical comparison results |

Each layer is independent and addressable.

---

# 6 Component API Concept

Each storage component exposes a simple retrieval interface.

General concept:

```
<Component>.Provide(identifier)
```

Examples:

```
ObservationCatalog.Provide(catalogNumber)
ReferenceDataRepository.Provide(datasetID)
EngineDataRepository.Provide(datasetID)
AnalysisDataRepository.Provide(datasetID)
```

The **runner components** request data via these APIs rather than accessing storage directly.

This provides:

- storage abstraction
- reproducibility
- component independence

---

# 7 Engine Interface Concept

To allow multiple ephemeris engines to participate in the validation ecosystem, engines should implement a common interface.

Conceptual interface:

```
interface IEphemerisEngine
{
    EngineData RunScenario(Scenario scenario);
}
```

This allows the framework to run the same experiment against different engines.

Example engines:

```
AstronometriaEngine
ThirdPartyEngine
ExperimentalEngine
```

This architecture enables **cross‑engine validation**.

---

# 8 Dataset Identity

Scenarios are identified by a **CatalogNumber**.

Datasets require additional identifiers.

Typical identifiers:

```
ScenarioID
TruthFactoryID
EngineID
ValidationFingerprint
```

The ValidationFingerprint uniquely identifies a validation run.

Example:

```
VF‑20260316‑8F3A12C‑AS0001
```

---

# 9 Provenance Chain

Every validation result should preserve its full provenance chain.

```
Scenario
    ↓
TruthFactory
    ↓
EphemerisEngine
    ↓
AnalysisTool
```

This guarantees full scientific traceability.

---

# 10 Plugin Architecture

Because the system communicates through APIs and interfaces, the framework naturally supports plugins.

Possible plugin types:

```
ScenarioFactories
TruthFactories
EphemerisEngines
AnalysisTools
```

This design enables external contributors to integrate additional engines or data sources.

---

# 11 Long‑Term Vision

The AstronoSphere architecture therefore represents more than a single application.

It forms a **scientific validation framework for ephemeris engines**.

Possible future uses:

- validation of new ephemeris implementations
- comparison between independent engines
- regression testing of astronomical algorithms
- reproducible publication of validation datasets

---

# 12 Summary

Key architectural principles:

- separation of experiment definition and execution
- deterministic scenario definitions
- persistent data layers
- component APIs for data access
- engine interface abstraction
- full provenance tracking

Together these elements form the conceptual **AstronoSphere Ephemeris Validation Framework**.
