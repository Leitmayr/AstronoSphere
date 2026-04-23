# AstronoSphere -- Levels & Milestone Plan

## Part 1 -- Levels Definition

### Level 1 -- Scientific Closure

Goal: A fully validated, reproducible scientific core.

Includes: - L0--L2 physics (Apparent Place complete) - Horizons +
Miriade integration (Truth sources) - Deterministic pipeline (Run ==
LastRun) - Full reproducibility (state → Horizons request derivable) -
Astronolysis feedback loop (basic) - Edge cases integrated into catalog
(EDGE category) - Trust cases (≥3 documented validation proofs)

Excludes: - ALTAZ / Topocentric - GUI / Visualization - INDI / hardware
integration

------------------------------------------------------------------------

### Level 2 -- Observation Bridge

Goal: Connect simulation to real observation.

Includes: - Topocentric transformation (ALTAZ) - Sidereal time
handling - Meeus validation vs Miriade - Observer modeling (Earth
rotation, location)

------------------------------------------------------------------------

### Level 3 -- Experiential System

Goal: User-facing, trustworthy astronomical system.

Includes: - GUI (AstronoLab / Astronometria visualization) - Interactive
exploration - API access - Educational / visualization layer

------------------------------------------------------------------------

## Part 2 -- M2.x Plan (Achieving Level 1)


### M2.1 -- Mesh Expansion (L0)

**Objective: the only dimension: Experiment expansion**

-   Implement structured time mesh
-   No clipping to Horizons range
-   Validate stability across mesh
-   Maintain deterministic pipeline

GIT Feature Branch: feature/M2.1-mesh-expansion-l0

### M2.2 -- Engine Integration (L0)

**Objective: the only dimension: Integrate Engine into AstronoSphere pipeline**

-   reactivate/setup structured Testsuites (experiment based) in Astronometria
-   Run L0 Tests and have them all green within the specified accuracy tolerance

GIT Feature Branch: feature/M2.2-engine-integration-l0

### M2.3 -- Simu results in Pipeline 

**Objective: the only dimension: simulation results of Astronometria are being written the to containers in AstronoData**

-   Constraint: all this happens in the **production mode** of Astronometria
-   Baseline is generated for first validated run. Scientific review carried out before.
-   read data from 02_Experiments/Released and 03_GroundTruth/Baseline
-   Simulation file structure according to GroundTruth (i.e. including a DatasetHeader)
-   write data to Run/LastRun/Baseline

### M2.4 -- System State (Reproducibility)

**Objective: the only dimension: Impelement state machine principle in Astronometria**

-   Ensure full state reconstruction: State must reconstruct the full canonical request (bit-identical)
-   Derive Horizons request from system state
-   Validate deterministic replay.


### M2.5 -- Light-Time (L1)

**Objective: the only dimension: expand physics ot Level 1**

-   Constraint: all this happens in the Test Environment
-   read data from 02_Experiments/Released and 03_GroundTruth/Baseline
-   Implement iterative light-time correction
-   Validate with EDGE cases (Neptune, Mercury)
-   validate results against Horizons 03_GroundTruth/Baseline. Consider accuracy toleance. Stay in Testing framework.

### M2.6 -- Aberration (L2)

**Objective: the only dimension: expand physics ot Level 2**

-   Constraint: all this happens in the Test Environment
-   read data from 02_Experiments/Released and 03_GroundTruth/Baseline
-   Implement aberration model
-   validate results againt Horizons 03_GroundTruth/Baseline. Consider accuracy toleance. Stay in Testing framework.


### M2.7 -- Integrate Miriade as second Ground Truth

**Objective: the only dimension: Successful Miriade Truth Factory**

-   M2.7.1: understand Miriade API
-   M2.7.2: Request Mapping (Horizons → Miriade)
-   M2.7.3: Normalization (Output → AstronoData)
-   Run == LastRun: deterministic Miriade API calls

### M2.8 -- Integrate Astronolysis 

**Objective: the only dimension: Astronolysis to conduct delta**
**Non-Objective: generting seeds automatically**

-   Constraint: all this happens in the **production mode** of Astronometria
-   read data from 02_Experiments/Released, 02_GroundTruth/Baseline and 03_Simulation/Baseline
-   conduct simple dummy analysis, e.g. VSOP accuracy on mesh: NO interpretation, NO model feedback, ONLY numeric delta


### M2.9 -- Astronolysis of Horizons vs. Miriade

**Objective: the only dimension: compare Miriade vs. Horizons for Holy12 Experiments**

-   run Holy12 on Horizons with DE440
-   run Holy12 on Miriade with DE330
-   compare delta between DE440 and DE330 on the Holy12 scenarios


### M2.10 -- Trust Validation

**Objective: the only dimension: No development, just determination of Trust Scenarios to strengthen trust in the pipeline**

-   Define and validate 3+ trust cases:
    -   Model convergence (Meeus → VSOP → Horizons)
    -   Deterministic reproducibility
    -   comparison of Miriade vs. Horizons

------------------------------------------------------------------------

## Part 3 -- M3+ Mapping

### M3 -- Reproducibility & System Integrity

-   Full provenance tracking
-   Truth quality classification
-   Stable experiment catalog

### M4 -- Validation Expansion

-   Astronolysis (automated seed generation)
-   Truth vs Truth comparison
-   Event-based validation (Opposition, etc.)

### M5 -- Observation Layer

-   ALTAZ / Topocentric
-   Sidereal time validation (Meeus vs Miriade)
-   Observer-based outputs

### M6 -- Product Layer

-   GUI
-   API
-   Visualization

------------------------------------------------------------------------

## Part 4 -- Timeline (M2.x)

Estimated duration: 4--8 weeks

-   M2.1 Mesh: 1--2 weeks
-   M2.2 Light-Time: 1--2 weeks
-   M2.3 Aberration: 1--2 weeks
-   M2.4 System State: 1 week
-   M2.5 Trust Cases: 1 week

Execution rules: - One dimension at a time - Validation-first approach -
No scope expansion (Stealth Mode)

------------------------------------------------------------------------

## Alignment with Stealth Manifest

-   No GUI / INDI / API work
-   Strict dimensional separation
-   Full validation before progression
-   No parallel development paths

------------------------------------------------------------------------

# End of Document
