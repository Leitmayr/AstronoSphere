# Astronometria -- TS-A Heliocentric Regression Breakthrough Documentation

This document summarizes the architectural, numerical, and procedural
milestones achieved during the implementation of TS-A (Heliocentric
Quadrant Event Tests, Level 0).

It documents:

1.  Testcase rationale
2.  Horizons API integration
3.  Initial inaccuracies and corrective measures
4.  EventFinder architecture
5.  Visual Studio solution structure
6.  Key design decisions
7.  Current regression status

------------------------------------------------------------------------

# 1. Testcase Definition -- Rationale (TS-A)

TS-A validates heliocentric ecliptic J2000 Cartesian state vectors by
exploiting geometric quadrant crossings of planetary motion.

For each planet we defined four deterministic events:

  Event   Condition
  ------- ------------------------
  L0      Y: negative → positive
  L6      X: positive → negative
  L12     Y: positive → negative
  L18     X: negative → positive

These correspond to heliocentric ecliptic longitudes:

-   L0 → 0°
-   L6 → 90°
-   L12 → 180°
-   L18 → 270°

These events provide:

-   Strong geometric sensitivity
-   Deterministic sign transitions
-   Full quadrant coverage
-   Excellent regression anchors

Only the **first occurrence per event type** is used for TS-A.

------------------------------------------------------------------------

# 2. Horizons API -- Final Working Configuration

The Horizons REST API was initially problematic due to parameter syntax
issues.

Final working configuration:

-   EPHEM_TYPE=VECTORS
-   VECT_CORR='NONE'
-   REF_PLANE='ECLIPTIC'
-   REF_SYSTEM='ICRF'
-   OUT_UNITS='AU-D'
-   CSV_FORMAT='YES'
-   OBJ_DATA='NO'
-   Time format: JD (TDB)

Critical fixes included:

-   Correct time format (JD_TDB)
-   Correct quoting of parameters
-   Correct URL encoding
-   Correct vector correction parameter handling

Once stabilized, API calls became deterministic and robust.

------------------------------------------------------------------------

# 3. Initial Problem: Start Values Too Inaccurate

Initial manually estimated event epochs were insufficiently precise.

Observed issues:

-   No sign transition in zoom window
-   Incorrect quadrant detected
-   Horizons regression window misaligned

Conclusion:

Manual estimation was not sufficiently reliable for deterministic
regression.

This led to the implementation of an automated event detection system.

------------------------------------------------------------------------

# 4. EventFinder Implementation

We implemented a deterministic two-stage event detection system.

## Stage 1 -- Coarse Scan

-   Step size: 1 day
-   Detect sign changes of X and Y
-   Identify candidate Julian Date

## Stage 2 -- Refinement

-   Window: ±3 days
-   Step size: 1 hour
-   Detect exact bracket
-   Linear interpolation to sub-hour precision

Interpolation formula:

t = t1 + (-v1 / (v2 - v1)) \* (t2 - t1)

This yielded sub-second level epoch precision.

All eight planets were processed successfully.

------------------------------------------------------------------------

# 5. Architectural Setup (Visual Studio Structure)

Final relevant structure:

EphemerisRegression/ │ ├── Api/ │ ├── HorizonsApiClient.cs │ ├──
HorizonsApiRequestFactory.cs │ ├── EventFinding/ │ └──
HelioQuadrantEventGenerator.cs │ ├── Event/ │ ├── HelioEvent.cs │ └──
HelioEvents.cs │ ├── Parsing/ │ └── HorizonsVectorParser.cs │ ├──
Runner/ │ ├── HelioEventGenerationRunner.cs │ └── HelioApiRunner.cs │
└── Program.cs

Program.cs supports two execution modes:

-   Event Detection
-   Horizons Regression

Switching is controlled via config flags.

------------------------------------------------------------------------

# 6. Major Design Decisions

-   Deterministic architecture
-   No orbital period tables required
-   No heuristic guessing
-   No manual epoch tuning
-   Strict geometric definitions
-   Linear interpolation sufficient at 1h resolution
-   No premature optimization

The full TS-A computation across all planets required \~130 seconds,
which is acceptable for a regression dataset generator.

------------------------------------------------------------------------

# 7. Final TS-A Output

We generated:

-   32 deterministic events (4 per planet)
-   32 Horizons CSV datasets
-   32 JSON reference files
-   Fully validated quadrant ordering
-   Orbital-period-consistent spacing

All planets were plausibilized against known orbital periods:

-   Mercury: \~88 days
-   Venus: \~225 days
-   Earth: \~365 days
-   Mars: \~687 days
-   Jupiter: \~11.86 years
-   Saturn: \~29.46 years
-   Uranus: \~84 years
-   Neptune: \~165 years

All results matched expected physical behavior.

------------------------------------------------------------------------

# 8. Status

TS-A (Heliocentric, Level 0, Geometric J2000) is complete.

We now possess:

-   Deterministic reference epochs
-   Verified Horizons reference vectors
-   Structured JSON regression data
-   Stable architecture

Next step:

Implementation of NUnit regression tests against the Ephemerides
pipeline.

------------------------------------------------------------------------

End of document.
