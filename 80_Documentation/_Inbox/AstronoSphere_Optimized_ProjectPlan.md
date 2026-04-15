# AstronoSphere -- Optimized Project Plan (M1.9 → M2)

## Overview

This plan refines the original timeline by separating: - Structural
changes - Process improvements - Stabilization - Physics implementation

Goal: Controlled evolution with deterministic validation (B2B at every
step)

------------------------------------------------------------------------

# PART I -- PIPELINE & PROCESS MATURITY

## Week 1 (Completed)

### Goal: Full Pipeline Stabilization

-   144 scenarios executed
-   Test plan (Excel)
-   TS-A ... TS-D validation
-   Error clustering and fixes

### Result

-   Deterministic pipeline
-   M1.8 baseline established

------------------------------------------------------------------------

## Week 2--3

### Focus: Structural Pipeline Refactoring

-   DatasetHeader generation → TruthFactory
-   SHG reduced to:
    -   ScenarioID
    -   CoreHash
    -   CatalogNumber
-   File naming cleanup

### Goal

Clean architectural ownership: - Scenario ≠ Measurement

------------------------------------------------------------------------

## Week 3--4

### Focus: Scenario Process & Authoring

-   Introduce AstronoLab (Scenario Authoring Tool)
    -   Load existing scenarios (144 import!)
    -   Edit metadata
    -   Save as Candidate
-   Scenario deprecation (invalid Horizons domains)

### Goal

Eliminate manual data handling

------------------------------------------------------------------------

## Week 4--5 (NEW -- Critical)

### Focus: Stabilization Phase

-   Full 144 scenario run
-   B2B against M1.8 baseline

### Goal

Ensure: - No regression - Full determinism after refactor

------------------------------------------------------------------------

## Week 5--7

### Focus: Engine Integration

-   Connect Astronometria to ReferenceData
-   Validate test cases

### Goal

Green test suite

------------------------------------------------------------------------

## Week 7--8

### Focus: Analysis Tool Integration

-   VSOP evaluation in new architecture
-   Provenance chain validation

### Goal

Scientific validation layer active

------------------------------------------------------------------------

## Week 8--9

### Focus: Meeus Integration

-   Port Meeus Scenario Factory (C++ → C#)
-   Use as JD provider (NOT scenario generator!)

### Goal

Optional input source for AstronoLab

------------------------------------------------------------------------

# PART II -- PHYSICS PIPELINE (ENGINE)

## Week 10--13

### Focus: L1 / L2 / L2+ Implementation

-   Light-time
-   Aberration
-   Extended correction chain

### Validation

Against: - ReferenceData from Part I

------------------------------------------------------------------------

# DECISION POINT

## Option A (Recommended First)

-   Astronometria release
-   Visualization:
    -   Polar projection
    -   Horizontal projection
    -   Mercator projection

## Option B (Later)

-   Additional TruthFactories
-   Open-source expansion of AstronoSphere

------------------------------------------------------------------------

# KEY PRINCIPLES

-   Determinism first
-   One change at a time
-   B2B validation after each phase
-   No parallel formats
-   Clear ownership:
    -   AstronoLab → Authoring
    -   SHG → Identity
    -   TruthFactory → Measurement

------------------------------------------------------------------------

# SUMMARY

This plan transforms AstronoSphere into a:

> Deterministic, tool-driven scientific system

while minimizing risk through strict sequencing and validation.
