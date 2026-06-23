# AstronoSphere — M2.x Timeline (Final)

## Context

This timeline defines the execution plan for achieving **Level 1 — Scientific Closure**.

It strictly follows:

- Stealth Mode discipline (one dimension at a time)  
- Validation-first principle  
- Deterministic pipeline requirements (Run == LastRun)  
- KISS and reproducibility rules  

Reference: COLLAB_AstronoSphere_Level1-3_Milestones2x_V1.1.md

---

# Overall Duration

**Estimated total duration: 6 – 9 weeks**

- optimistic: ~40 days  
- realistic: ~45–55 days  
- pessimistic (Miriade complexity): up to ~65 days  

---

# Phase Breakdown

## M2.1 — Mesh Expansion (L0)

**Objective:** Experiment expansion only

**Duration:** 3–5 days

- structured time mesh
- no Horizons clipping
- deterministic behavior validation

---

## M2.2 — Engine Integration (L0)

**Objective:** Integrate Astronometria into pipeline (tests only)

**Duration:** 3–4 days

- re-enable structured test suites
- experiment-based execution
- all L0 tests green within tolerance

--> took shorter. Done in 1-2 days.

---

## M2.3 — Simulation Results in Pipeline

**Objective:** Persist simulation outputs (production mode)

**Duration:** 3–5 days

- first validated run → Baseline creation (manual scientific validation required)
- read: 02_Experiments/Released + 03_GroundTruth/Baseline
- write: 04_Simulations/{Run, LastRun, Baseline}
- structure aligned with GroundTruth (including DatasetHeader)

--> took longer. Rather 6-8 days. Extensive Spec and Validation was needed to keep the Milestone stable.

---

## M2.4.0 — System State (Reproducibility)

**Objective:** Simulation run with State Machine

**Duration:** 5–7 days

## M2.4.1 — System State

**Objective:** 
Persist internal PHYS.* node types
instead of legacy VSOP87.* node types

Validation:
- manual calculation of Hashes with Web-Tool for selected samples
- Beyond Compare with rule based exception

## M2.4.2 — System State

**Objective:** Clean Up of Frame Definition in json

Validation:
Beyond Compare with rule based exception

---

## M2.4.5 — Time Domain 

Contents:

Canonical Time Definition
AstronoSphere global astro time: TDB
Experiments: TDB
GroundTruth: TDB-aligned
SimulationModel.NativeTimeScale: TT/TDB/...
Engine must perform conversion prior to model evaluation
Astronometria implementation
Input remains TDB (from Experiment/GroundTruth)

Prior to VSOP87 evaluation:

TT = f(TDB)
VSOP87 proceeds natively in TT
This native time base is explicitly specified in SimulationModel.TimeScale
Validation
Results change slightly
No longer a strict M2.3 byte-for-byte identity
Plausibility check of the deltas
Subsequently, pragmatic promotion to a new baseline

This approach is sensible because it prevents L1 LightTime from being implemented on an undefined time base.

## M2.4.9 — AstronoDiatg Consolidation

**Objective**: 
Cleanup and standardization of AstronoSphere Diagnostic

**Content:**
- FMI-defintiions in 030 and 040 inconsistent -> need clean up
- Reviews and standardization of Severiy, Priority, Persistance and Run/LastRUn Rules
- clean up of local Diag specs
- overwriting policy in Chapter 8

**Target:** 
first version of a FREEZE Core Diagnostic Documentation

---
## M2.5 — Light-Time (L1)

**Objective:** Introduce physical correction (time-domain)

**Duration:** 6–10 days

- iterative Light-Time solver
- operates on models natively evaluated in TT
- validate against 03_GroundTruth/Baseline
- EDGE validation:
  - Neptune (distance)
  - Mercury (velocity)
- convergence + stability checks

---

## M2.6 — Aberration (L2)

**Objective:** Introduce spatial correction

**Duration:** 6–10 days

- aberration model implementation
- validation against Horizons GroundTruth
- strict separation from Light-Time (time vs space)

---

## M2.6.5 — TDB Branch in Physics StateTree

M2.6.5 = optional TimeDomain branch / TDB-native node layer

Horizons StateTree Image: 
States 1b, 2b, 3b, 5b, 6b, 7b, 9b, 10b, 11b 
for models, which are running in TDB natively or for dedicated VSOP-TDB-Analysis.

---

## M2.7.0 — AstronoTruth EQU with Horizons 

**Objective:** Add Equatorial Coordinates to enable processing of AS-000059 to AS-000072 Experiments -> Completion of the prepratations made in M2.4

### Sub-steps (strictly sequential):

1. Remove existing ECL/EQU Bug for AS-000059 to AS-000072
2. Generate AS-000059 to AS-000072
3. Adapt File names to AS-000XXX-...json in accordance with Astronometria and Experiment Naming

- deterministic request generation
- Run == LastRun validation of existing GroundTruth
- Comparison with manual Horizons Calls for EQU-Experiments

## M2.7.1 — Connect AstronoTruth Horizons with AstronoMeasurement

**Objective:** Define Horizons Call parameters with AstornoMeasurement

1. To Disuss once MS starts: include AstronoMeasurement for the definiton of the Ground Truth Request  (later also neede in Miriade)

- Run == LastRun validation against M2.7.0 results

## M2.7.5 — Miriade Integration (Ground Truth #2)

**Objective:** Second deterministic TruthFactory. Calls with AstronoMeasurement

**Duration:** 10–20 days

### Sub-steps (strictly sequential):

1. API understanding  
2. Request mapping (Horizons → Miriade)  
3. Output normalization → AstronoData format  

- deterministic request generation
- Run == LastRun validation

⚠️ Highest uncertainty in M2

---

## M2.8 — Astronolysis (Delta Only)

**Objective:** Numeric comparison only

**Duration:** 2–3 days

- read:
  - 02_Experiments/Released  
  - 03_GroundTruth/Baseline  
  - 04_Simulations/Baseline  
- compute deltas (e.g. VSOP vs Horizons)

STRICT LIMITS:

- NO interpretation  
- NO model feedback  
- ONLY numeric delta  

---

## M2.9 — Horizons vs Miriade Comparison

**Objective:** Cross-validation of truth sources

**Duration:** 2–3 days

- run Holy12 on:
  - Horizons (DE440)
  - Miriade (DE330)
- compute delta between ephemerides

---

## M2.10 — Trust Validation

**Objective:** Establish scientific trust (no development)

**Duration:** 2–4 days

Define and validate at least 3 trust cases:

- model convergence (Meeus → VSOP → Horizons)
- deterministic reproducibility
- Horizons vs Miriade comparison

---

# Execution Rules

## 1. Dimensional Discipline

At any time:

> EXACTLY ONE dimension may change

---

## 2. Validation First

Every step must define:

- hypothesis
- expected behavior
- validation method

---

## 3. Definition of Done

A phase is complete only if:

- Run == LastRun (binary identical)
- Golden samples validated
- no unexplained deviations
- full behavior understanding achieved

---

## 4. No Scope Expansion

Strictly forbidden:

- GUI / visualization
- API development
- new features outside validation scope

---

# Final Principle

> M2 is not feature development.

> M2 is the construction of a **physically correct, deterministic, and reproducible system**.
