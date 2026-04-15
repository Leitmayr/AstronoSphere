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

-   Implement structured time mesh
-   No clipping to Horizons range
-   Validate stability across mesh
-   Maintain deterministic pipeline

### M2.2 -- Light-Time (L1)

-   Implement iterative light-time correction
-   Validate with EDGE cases (Neptune, Mercury)
-   Verify convergence behavior

### M2.3 -- Aberration (L2)

-   Implement aberration model
-   Validate directional shifts
-   Compare with Horizons/Miriade

### M2.4 -- System State (Reproducibility)

-   Ensure full state reconstruction
-   Derive Horizons request from system state
-   Validate deterministic replay

### M2.5 -- Trust Validation

-   Define and validate 3+ trust cases:
    -   Model convergence (Meeus → VSOP → Horizons)
    -   Truth comparison (Horizons vs Miriade)
    -   Deterministic reproducibility

Note: each M2.x step MUST NOT indtroduce new dimensions!

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
