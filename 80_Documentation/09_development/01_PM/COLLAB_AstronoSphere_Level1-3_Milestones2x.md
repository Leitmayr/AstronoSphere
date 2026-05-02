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

Goals of Milestone 2: complete the pipeline by Integrating Astronometria and Astronolysis and enhance physics to Level 1

### M2.1 -- Mesh Expansion (L0)

**Objective: the only dimension: Experiment expansion**

-   Implement structured time mesh
-   No clipping to Horizons range
-   Validate stability across mesh
-   Maintain deterministic pipeline


### M2.2 -- Preparation of Astronometria Testing (L0)

**Objective: the only dimension: Integrate Engine into AstronoSphere pipeline**

-   reactivate/setup structured Testsuites (experiment based) in Astronometria
-   Run L0 Tests and have them all green within the specified accuracy tolerance


### M2.3 -- Light-Time (L1)

**Objective: the only dimension: expand physics ot Level 1**

-   Constraint: all this happens in the Test Environment
-   read data from 02_Experiments/Released and 03_Astronometria/Baseline
-   Implement iterative light-time correction
-   Validate with EDGE cases (Neptune, Mercury)
-   validate results againt Horizons Ground Truth. Consider accuracy toleance. Stay in Testing framework.

### M2.4 -- Aberration (L2)

**Objective: the only dimension: expand physics ot Level 2**

-   Constraint: all this happens in the Test Environment
-   read data from 02_Experiments/Released and 03_GroundTruth/Baseline
-   Implement aberration model
-   validate results againt Horizons Ground Truth. Consider accuracy toleance. Stay in Testing framework.

### M2.5 -- Simu results in Pipeline 

**Objective: the only dimension: simulation results of Astronometria are being written the to containers in AstronoData**

-   Constraint: all this happens in the **production mode** of Astronometria
-   read data from 02_Experiments/Released and 03_GroundTruth/Baseline
-   write data to Run/LastRun/Baseline

### M2.6 -- Integrate Astronolysis 

**Objective: the only dimension: Astronolysis to conduct first analysis**
**Non-Objective: generting seeds automatically**

-   Constraint: all this happens in the **production mode** of Astronometria
-   read data from 02_Experiments/Released, 02_GroundTruth/Baseline and 03_Astronometria/Baseline
-   conduct simple dummy analysis, e.g. VSOP accuracy on mesh


### M2.7 -- System State (Reproducibility)

**Objective: the only dimension: Impelement state machine principle in Astronometria**

-   Ensure full state reconstruction
-   Derive Horizons request from system state
-   Validate deterministic replay.

### M2.8 -- Integrate Miriade as second Ground Truth

**Objective: the only dimension: Successful Miriade Truth Factory**

-   understand Miriade API
-   read experiment from 02_Experiment/Released
-   translate to Miriad API and save data in 03_GroundTruth/Ephemeris/Miriade/Run
-   Run == LastRun: deterministic Miriade API calls

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
