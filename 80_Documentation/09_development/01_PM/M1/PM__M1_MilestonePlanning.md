# AstronoSphere – Milestone M1 Roadmap

## Purpose

This document defines the **execution roadmap for Milestone M1** of the AstronoSphere ecosystem.

M1 represents the first **architectural proof of concept** of the complete AstronoSphere validation loop.

The goal is not feature completeness but a **working vertical slice** connecting:

Scenario definition → Ground truth generation → Engine computation → Validation.

---

# 1. Architectural Context

AstronoSphere consists of three independent systems:

AstronoSphere
│
├ Astronometria
│ astronomical computation engine
│
├ AstronoData
│ scientific reference data repository
│
└ TruthFactories
ground truth dataset generators


Responsibilities:

| Component | Responsibility |
|-----------|---------------|
| Astronometria | Astronomical computation |
| AstronoData | Scientific data backbone |
| TruthFactories | External reference data generation |

The system operates as a **scientific validation loop**.

ObservationCatalog
↓
TruthFactory Run
↓
ScientificReference Dataset
↓
Astronometria Computation
↓
Residual Analysis
↓
Edge Case Detection
↓
ObservationCatalog



Milestone M1 must demonstrate that this loop works end-to-end.

---

# 2. Definition of Milestone M1

M1 is achieved when the following pipeline exists and operates reliably:

Scenario
↓
EphemerisFactory
↓
Reference Dataset
↓
Astronometria
↓
Residual Analysis


The system must be able to:

1. Define astronomical scenarios
2. Generate reference data from Horizons
3. Compute the same scenario in Astronometria
4. Compare results
5. Produce residual statistics

This constitutes the **first scientific validation cycle**.

---

# 3. Optimized Execution Sequence

The following execution order minimizes rework and context switching.

## Step 1 – Scenario Finalization

Define a stable **baseline scenario set**.

Typical scenarios:

Mars_Opposition_2025
Jupiter_Opposition_2022
Mercury_Perihelion_Mesh
InnerPlanets_1Y_Mesh
OuterPlanets_10Y_Mesh



Scenarios are stored in the ObservationCatalog.

Each scenario must contain:

- ScenarioName
- ScenarioID
- CoreHash
- Description
- Core definition

---

## Step 2 – EphemerisFactory Refactor

Refactor the existing **EphemerisRegression tool** into a proper TruthFactory.

Changes:
Event generator removed
ScenarioLoader introduced
Scenario → Horizons request mapping

Factory Architecture:
ScenarioLoader
GroundTruthRequestFactory
HorizonsClient
Exporter

Output artifacts:
CSV (raw Horizons output)
JSON (canonical reference object)

---

## Step 3 – Astronometria Scenario Adapter

Astronometria must be able to compute scenarios directly.

Mapping:
Scenario
→ Frame
→ Targets
→ Time mesh
→ Correction level


The engine itself remains unchanged.

---

## Step 4 – Reference Dataset Generation

Run EphemerisFactory for all baseline scenarios.

Output location:
AstronoData/ReferenceData


This produces the **first official reference dataset**.

---

## Step 5 – Validation Run

Astronometria computes the same scenarios.

Residuals are calculated:
ΔX
ΔY
ΔZ
ΔR
optional: ΔRA / ΔDEC


Output:
Residual statistics
Validation result


---

# 4. Scenario Design

Scenarios describe **astronomical experiments**.

A scenario must be completely deterministic.

Example structure:

```json
{
  "ScenarioName": "Mars_Opposition_2025",

  "CoreHash": "71B2D0F3",

  "ScenarioID": "GEO-TDB-2460000-2460010-1D",

  "Description": "Mars opposition around January 2025",

  "Core": {

    "Time": {
      "StartJD": 2460000,
      "StopJD": 2460010,
      "StepDays": 1,
      "TimeScale": "TDB"
    },

    "Observer": {
      "Type": "Geocentric"
    },

    "Targets": ["Mars"],

    "Frame": "GeoEquatorial",

    "Corrections": {
      "LightTime": false
    }

  }

}



Scenario Identity

Each scenario has three identity layers.

Field	Purpose
ScenarioName	Human readable title
ScenarioID	Deterministic scenario identifier
CoreHash	Short hash derived from the core definition

CoreHash generation:

SHA256(canonical(Core))
→ first 8 hex characters

Example:

Mars_Opposition_2025__71B2D0F3.json

Only the CoreHash appears in filenames.

6. Scenario Immutability Rule

The scenario core is immutable.

TruthFactories must never modify the scenario definition.

Rule:

Factories may read the scenario
Factories may not modify the scenario

The scenario represents the physical experiment.

Factories represent the measurement system.

7. Factory Metadata

Factories extend the dataset with their own metadata.

Structure:

{
  "FactoryMetadata": {

    "FactoryName": "EphemerisFactory",

    "FactoryHash": "A94C22E1",

    "Source": "JPL_Horizons",

    "ReferenceModel": "DE440",

    "GeneratedAtUtc": "2026-03-14T10:32:00Z"

  }
}

FactoryHash is computed from:

canonical(Factory parameters)

Example parameters:

Source
ReferenceEphemeris
StepMode
API configuration
8. Benefits of Dual Hash Design

Two hashes provide clear traceability.

Case	Interpretation
CoreHash same, FactoryHash different	same physical scenario, different reference generator
CoreHash different, FactoryHash same	new scenario using same factory
Both different	completely new experiment
9. AstronoData Repository Layout
AstronoData
│
├ ObservationCatalog
│
├ ReferenceData
│
├ FactoryRuns
│
└ AccuracyReports

ObservationCatalog contains scenario definitions.

ReferenceData contains validated datasets.

FactoryRuns contain temporary generator outputs.

10. Recommended Chat Structure for M1

To maintain performance and clarity, the development work should be split across four chats.

AST_META

Architecture discussions and milestone planning.

Topics:

system architecture

roadmap

scenario design

validation strategy

AST_DEV

Astronometria engine development.

Topics:

ephemeris models

coordinate frames

correction pipeline

time system

scenario adapter

FACTORY_DEV

TruthFactory development.

Topics:

EphemerisFactory

Horizons API

scenario loading

dataset export

request hashing

VALIDATION

Scientific validation and testing.

Topics:

reference datasets

residual analysis

mesh validation

tolerance studies

statistical evaluation

11. Expected Result of Milestone M1

After completion of M1 the following capability exists:

Scenario
↓
EphemerisFactory
↓
Reference Dataset
↓
Astronometria
↓
Residual Analysis
↓
Validation Report

This proves that AstronoSphere works as an integrated scientific validation architecture.

End of Document



