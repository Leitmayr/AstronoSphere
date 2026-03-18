# TS-A Knowledge Transfer -- Project Context for Next Session

Created: 2026-02-16 09:55 UTC

This document summarizes the state of the project after completion of
TS-A, so that work can continue efficiently in a new session.

------------------------------------------------------------------------

# 1. What TS-A Covers

Test Suite A validates:

Heliocentric ecliptic quadrant crossings (L0, L6, L12, L18)

For each planet: - EventFinder determines precise JD via sign-change
detection + interpolation - Horizons provides high-resolution vector
data - Pipeline output is compared against Horizons vectors - Deviations
are statistically evaluated

Observed outer-planet deviations (e.g., Neptune \~54,000 km) are
consistent with VSOP87A published accuracy limits.

------------------------------------------------------------------------

# 2. Validation Outcome

✔ Implementation verified\
✔ Transformations verified\
✔ Model limitations understood\
✔ Time scale mismatch ruled out as dominant cause\
✔ Frame differences considered secondary

Pipeline validated within VSOP87A accuracy envelope.

------------------------------------------------------------------------

# 3. Solution Architecture Overview

## 3.1 EphemerisRegression Project

Purpose: - Generate Horizons reference data - Event finding - JSON
serialization of reference vectors

Key folders:

Api/\
EventFinding/\
Runner/\
Horizons/Helio/RawResults/\
Horizons/Geo/RawResults/

------------------------------------------------------------------------

## 3.2 AstroSim.Ephemerides.Test

Purpose: - NUnit regression tests - Comparison against stored JSON
reference data

Structure:

EphemerisValidation/\
TestData/\
Helio/TS-A/\
Geo/TS-A/

Regression tests load JSON from TestData (Copy to Output Directory
enabled).

------------------------------------------------------------------------

# 4. Collaboration Model (Efficiency Rules)

For future sessions:

1.  One clear goal per session
2.  Strict separation of:
    -   Physics problems
    -   Numerical problems
    -   Architecture problems
3.  Hypothesis-driven debugging
4.  Avoid parallel refactoring during validation steps
5.  Always document scientific conclusions

------------------------------------------------------------------------

# 5. Current State Before TS-B

Validated:

VSOP → HE → GE → GA pipeline (Level 0)

Next candidate steps:

-   TS-B: Node crossings (Z sign change)
-   TS-C: Distance extrema
-   CorrectionLevel expansion (future)

------------------------------------------------------------------------

# 6. Known Model Limitations

VSOP87A residual limits: - Inner planets \~1″ - Outer planets up to \~5″

Neptune example: 54,000 km deviation ≈ 2.5″ → within theoretical bounds.

------------------------------------------------------------------------

# 7. Ready State

System stable.\
Architecture stable.\
Test infrastructure stable.\
Scientific limitations understood.

Project ready to proceed with TS-B.
