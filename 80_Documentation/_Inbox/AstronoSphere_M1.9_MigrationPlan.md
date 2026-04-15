# AstronoSphere M1.9 Migration Plan

## Overview
Goal: Refactor pipeline (Seeds → Experiments → GroundTruth) with strict Back2Back validation.

Duration: ~10–14 days

---

## Sprint 0 – B2B Foundation (Days 1–2)

### Day 1
- Generate 144 dataset baseline (Candidates, Scenarios, GroundTruth)
- Create Git baseline

### Day 2
- Setup compare scripts:
  - compare-run-lastRun.ps1
  - compare-data.ps1 (data-only)

---

## Sprint 1 – Scenario + Seeds (Days 3–5)

### Day 3
- Refactor ScenarioHeader
- Validate CoreHash

### Day 4
- Introduce Seeds structure:
  - Incoming / Prepared / Processed

### Day 5
- ScenarioMerger
- Validate CoreHash equality

---

## Sprint 2 – AstronoLab + Cert + Experiments (Days 6–8)

### Day 6
- AstronoLab implementation

### Day 7
- AstronoCert implementation
- CatalogNumber + Naming

### Day 8
- Finalize Experiment naming

---

## Sprint 3 – AstronoTruth (Days 9–11)

### Day 9
- Switch input to Experiments

### Day 10
- DatasetHeader + naming

### Day 11
- B2B validation (compare-data)

---

## Sprint 4 – Pipeline Stabilization (Days 12–14)

### Day 12
- Update scripts

### Day 13
- Full 144 run

### Day 14
- Hardening

---

## Critical Checkpoints

- CP1: Baseline stable
- CP2: CoreHash identical after migration
- CP3: Experiments deterministic
- CP4: GroundTruth unchanged
- CP5: Full pipeline stable

---

## Working Mode

1. Implement
2. Test immediately
3. Compare data
4. Commit

---

## Key Rule

DATA MUST NEVER CHANGE
