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

---

## M2.3 — Simulation Results in Pipeline

**Objective:** Persist simulation outputs (production mode)

**Duration:** 3–5 days

- first validated run → Baseline creation (manual scientific validation required)
- read: 02_Experiments/Released + 03_GroundTruth/Baseline
- write: 04_Simulations/{Run, LastRun, Baseline}
- structure aligned with GroundTruth (including DatasetHeader)

---

## M2.4 — System State (Reproducibility)

**Objective:** Full deterministic state reconstruction

**Duration:** 5–7 days

- reconstruct complete canonical request (bit-identical)
- include:
  - Experiment Core
  - Instrument (L0/L1/L2)
  - TimeScale (TT)
  - Frame
  - Provider mapping
- validate deterministic replay

---

## M2.5 — Light-Time (L1)

**Objective:** Introduce physical correction (time-domain)

**Duration:** 6–10 days

- iterative Light-Time solver
- operates strictly in TT domain
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

## M2.7 — Miriade Integration (Ground Truth #2)

**Objective:** Second deterministic TruthFactory

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
