ER_Zielarchitektur_GUI+CLI.md
# EphemerisRegression Target Architecture (GUI + CLI)

## 1. Purpose of this document

This document defines the **target architecture** of the EphemerisRegression (ER) tool after the introduction of the GUI.

The architecture must satisfy the following requirements:

1. CLI and GUI must use **exactly the same execution path**.
2. Existing **TestSuites (TS-A … TS-E)** must remain fully supported.
3. The system must allow **generic dataset generation** via GUI parameters.
4. The architecture must remain **minimal and maintainable**.
5. The system must produce **deterministic datasets**.

This document serves as the **reference specification** for further implementation work.

---

# 2. Architectural Principles

The architecture is guided by the following principles.

### 2.1 Single Execution Path

All dataset generation must go through a single core service.


GUI
CLI
↓
ExecutionService
↓
Dataset generation pipeline


Neither the GUI nor the CLI may directly call dataset runners.

---

### 2.2 Scenario-driven execution

All runs are defined by a single object:


RegressionScenario


The scenario defines the full configuration of a dataset generation run.

---

### 2.3 Minimalism

EphemerisRegression is a **tool**, not a framework.

The architecture intentionally avoids:

- complex interface hierarchies
- dependency injection frameworks
- plugin systems

The system should remain understandable in a single sitting.

---

# 3. High-Level Architecture


User
│
├── CLI
│
└── GUI
│
▼
RegressionScenario
│
▼
ExecutionService
│
▼
DatasetRunner
│
▼
Horizons API
│
▼
RAW CSV + JSON output


---

# 4. Core Concepts

## 4.1 RegressionScenario

The **RegressionScenario** defines a complete dataset generation request.

Typical fields:


DatasetPreset (optional)

Target

Origin
Plane
Epoch

CorrectionLevel

EpochStrategy

Start
Stop
Step

OutputDirectory


The scenario must contain enough information to produce a deterministic dataset.

---

## 4.2 Scenario Presets

Existing TestSuites are implemented as **Scenario Presets**.

Examples:


TS-A
TS-B
TS-C
TS-D
TS-E


These presets simply populate a `RegressionScenario`.

Example:


TS-A preset
EpochStrategy = QuadrantEvents
Target = Earth
Origin = HELIO
Plane = ECLIPTIC
Epoch = J2000
CorrectionLevel = L0


---

## 4.3 Epoch Strategies

Epoch generation is separated from dataset execution.

Available strategies:


QuadrantEvents
NodeEvents
DistanceExtrema
Mesh
Interval


These strategies generate a list of epochs.


EpochStrategy
↓
List<Epoch>


---

# 5. Execution Pipeline

The dataset generation pipeline consists of the following stages.


RegressionScenario
│
▼
EpochStrategy
│
▼
EpochGenerator
│
▼
HorizonsRequestBuilder
│
▼
HorizonsApiClient
│
▼
RawCsvWriter
│
▼
JsonWriter


Each stage has a clearly defined responsibility.

---

# 6. ExecutionService

The ExecutionService acts as the **single entry point** for all dataset generation.

Responsibilities:

- Accept a `RegressionScenario`
- Invoke the appropriate epoch strategy
- Build Horizons requests
- Execute dataset generation
- Coordinate export

Example interface:


RunAsync(RegressionScenario scenario)


Both CLI and GUI call this service.

---

# 7. CLI Integration

The CLI remains available for scripted dataset generation.

Example:


er run TS-A --level L1


The CLI performs:


CLI arguments
↓
RegressionScenario
↓
ExecutionService.RunAsync()


---

# 8. GUI Integration

The GUI provides a visual editor for `RegressionScenario`.

Users can configure:


Target
Origin
Plane
Epoch
CorrectionLevel

EpochStrategy

Start / Stop / Step


After configuration the GUI calls:


ExecutionService.RunAsync(scenario)


---

# 9. Dataset Output

Each run produces:


RAW CSV
JSON dataset


The CSV is a direct copy of the Horizons output.

The JSON contains parsed data and metadata.

Metadata includes:


CanonicalRequest
RequestHash
GenerationTime
Scenario information


---

# 10. Determinism

The architecture guarantees deterministic outputs.

Principle:


same scenario
↓
same Horizons request
↓
same dataset


The `RegressionScenario` is therefore the root of determinism.

---

# 11. Project Structure

Suggested project layout:


EphemerisRegression.Core
Execution
ExecutionService

Domain
RegressionScenario

EpochGeneration
EpochStrategy
EpochGeneratorFactory

Horizons
HorizonsRequestBuilder
HorizonsApiClient

Export
RawCsvWriter
JsonWriter

EphemerisRegression.Console
Program.cs

EphemerisRegression.GUI
GUI implementation


---

# 12. Refactoring Strategy

Refactoring must proceed in small safe steps.

Recommended steps:

### Commit 1

Introduce `ExecutionService`.

Program.cs delegates execution.

No functional change.

---

### Commit 2

Introduce `RegressionScenario`.

CLI builds a scenario and passes it to the ExecutionService.

---

### Commit 3

GUI creates scenarios and calls the ExecutionService.

---

# 13. Avoiding Overengineering

The architecture deliberately avoids:

- large class hierarchies
- complex dependency injection
- unnecessary abstraction layers

The entire core should remain **small and transparent**.

---

# 14. Long-term Goal

EphemerisRegression should become a tool that allows developers to generate deterministic reference datasets without needing to understand the Horizons API.

Typical usage:


Define scenario
Run scenario
Use dataset


This architecture supports that goal.