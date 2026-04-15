# AstronoSphere – Project Cheat Sheet

## Project Overview

AstronoSphere is a scientific software ecosystem for reproducible astronomical computation and validation.

The system consists of three main components:

- Astronometria – astronomical computation engine
- AstronoData – scientific reference data repository
- TruthFactories – generators of ground truth datasets

The architecture is based on **scenario-driven validation**.

---

# Core Concept

Every scientific computation is defined by a **Scenario**.

Scenarios are stored in the **ObservationCatalog** and uniquely identified by a **ScenarioID**.

A scenario describes:

- time range (Julian Dates)
- observer definition
- targets
- reference frame
- correction configuration

Factories generate reference datasets for scenarios.

Astronometria computes results for the same scenarios.

Results are compared and analyzed.

---

# Validation Loop

ObservationCatalog
→ TruthFactory Run
→ ScientificReference Dataset (AstronoData)
→ Astronometria Computation
→ Validation / Residual Analysis
→ Edge Case Detection
→ new Scenario Candidates
→ ObservationCatalog

This creates a **self-extending validation system**.

---

# Repository Structure

AstronoSphere
documentation and architecture

Astronometria
astronomical computation engine

AstronoData
scientific reference data repository

TruthFactories
reference data generators

---

# ScenarioID Concept

ScenarioIDs consist of two parts:

CORE
physical scenario definition

FACTORY
reference data generation parameters

Example:

HELIO-TDB-2451545-2451555-1D--EPH-HORIZONS-DE440

---

# TruthFactory Pattern

Every factory follows the same architecture:

1 ScenarioLoader
2 GroundTruthRequestFactory
3 GroundTruth API
4 GT Exporter

Output is written to AstronoData.

---

# Engineering Principles

Generic KISS Coding

- simple modules
- explicit responsibilities
- transparent data flow
- long-term maintainability

Efficiency Package

- no trial and error
- short answers
- whole files when coding
- milestone summaries

---

# Documentation Structure

docs

00_core
fundamental system definitions

01_vision
project vision

02_architecture
system architecture

03_scenarios
ObservationCatalog

04_data
AstronoData structure

05_factories
TruthFactory architecture

06_validation
validation strategy

07_reports
scientific analysis

08_principles
engineering principles

09_development
project planning

10_diagrams
architecture diagrams

99_history
archived documents

---

# Mission

AstronoSphere aims to create a **scientifically reproducible astronomical computation platform**.

The goal is not only to compute celestial mechanics but to validate them systematically and transparently.