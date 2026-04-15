# 00_Architecture_Index.md

## Purpose
Central navigation document for all architectural concepts in AstronoSphere.

---

## Core System

- AstronoLab → generates scenario candidates 
- AstronoCert → generates experiments from candidates (certification)
- AstronoTruth → ground truth generation
- Astronometria → computation engine
- Astronolysis → validation entity for results generation
- AstronoData → data backbone


---

## Key Concepts

### Scenario System
Defines reproducible astronomical experiments.

See:
- Scenario Definition Specification (v1.4)

---

### Experiments (formerly "ObservationCatalog")
Central registry of released scenarios.

See:
- ObservationCatalog Governance Policy

---

### Orthogonal Pipeline
Separation of geometry and physical corrections.

See:
- Orthogonal Pipeline Concept

---

### Time Architecture
Strict separation of UTC / TD domains.

See:
- Time Architecture Definition

---

### Light-Time (L1)
Time iteration solver for emission time.

See:
- Light-Time Fundamental Concept

---

### AstronoTruth
External data acquisition layer.

See:
- Truth Factory Architecture
- Scientific Reference Policy

---

### Astonolysation
Scenario → Reference → Engine → Comparison → Analysis

See:
- Validation Strategy

---

## Usage

- Entry point for all architectural work
- Avoid duplicate concepts
- Always extend instead of creating parallel docs
