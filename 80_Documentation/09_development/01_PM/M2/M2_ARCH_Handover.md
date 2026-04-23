# AstronoSphere M2 -- ARCH Chat Handover

## Purpose

This document initializes the ARCH discussion for M2.

Goal: Define detailed architecture for implementing a measurement-driven
validation system based on the stable M1.9 pipeline.

------------------------------------------------------------------------

## Current System State (M1.9)

-   Full deterministic pipeline established: Seeds → Experiments →
    GroundTruth → Simulations → Results
-   Run / LastRun validation working
-   GroundTruth separated and provider-based
-   DatasetHeader generated exclusively by AstronoTruth
-   144 Experiments available

The system is stable and reproducible.

------------------------------------------------------------------------

## M2 Objective

Transform Astronometria into a fully validated, measurement-driven
simulation engine.

Key idea:

Measurement = Test Definition

------------------------------------------------------------------------

## Core M2 Architecture Topics

### 1. AstronoData.IO

Minimal abstraction layer for:

-   Experiment access
-   GroundTruth access
-   Simulation writing

Constraints: - No business logic - Deterministic behavior - Pure data
transport layer

------------------------------------------------------------------------

### 2. Measurement System (Hybrid)

Design:

-   Code defines scientific meaning
-   JSON defines selectable configurations

Location: 06_Measurements/

Responsibilities: - Map Measurement → Horizons request - Map Measurement
→ AST configuration

------------------------------------------------------------------------

### 3. Pipeline Integration

Pipeline remains PowerShell-based.

Responsibilities: - Trigger full pipeline runs - Support Measurement
selection - Maintain deterministic Run/LastRun validation

------------------------------------------------------------------------

### 4. Validation Backbone

Holy12 experiments:

-   fixed reference set
-   manually verified
-   used for debugging and validation

------------------------------------------------------------------------

### 5. Mesh Strategy

-   Extended Mesh (density)
-   Extreme Mesh (1600--2500)

Purpose: - detect drift - detect systematic errors - validate long-term
stability

------------------------------------------------------------------------

### 6. Astronolysis Role

Responsibilities: - statistical comparison - drift detection - outlier
detection - seed generation

Strict separation from AST

------------------------------------------------------------------------

### 7. Physics Extension Strategy

Iterative loop:

1.  Define Measurement (L1, L2)
2.  Generate GroundTruth
3.  Implement AST feature
4.  Simulate
5.  Compare
6.  Analyze

------------------------------------------------------------------------

## Non-Goals for M2

-   No GUI (M4)
-   No Meeus Factory (M3)
-   No new domains (focus on Ephemeris)

------------------------------------------------------------------------

## Key Principles

-   Determinism first
-   No hidden logic
-   Separation of concerns
-   GroundTruth is immutable baseline
-   Measurement drives development

------------------------------------------------------------------------

## Expected Outcome of ARCH Chat

-   Final structure of AstronoData.IO
-   MeasurementDefinition + Resolver design
-   Pipeline integration concept
-   Validation strategy refinement

------------------------------------------------------------------------

End of document.
