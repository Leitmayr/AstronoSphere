# Getting Started -- Astronometria TS-C Test Development

**Generated:** 2026-03-02T19:32:31.721674 UTC

------------------------------------------------------------------------

## Purpose of the New Chat

This chat focuses exclusively on developing Astronometria test cases
based on TS-C reference data.

EphemerisRegression has already generated:

-   GeoDec Node events
-   Raw Vector files (ICRF, equatorial, TT)
-   Raw Observer files (RA/DEC, TT)
-   JSON reference files for Vector and Observer

The task now is to validate Astronometria against these references.

------------------------------------------------------------------------

## What Is Already Guaranteed

From EphemerisRegression:

-   Vector coordinates are Earth-centered (500@399)
-   Reference frame is ICRF (equatorial)
-   TIME_TYPE = TT
-   Z sign change corresponds to celestial equator crossing
-   DEC sign change occurs at identical JD
-   Request canonical + SHA256 hash stored in JSON

TS-C raw + JSON generation is considered stable.

------------------------------------------------------------------------

## Scope of the New Chat

In Astronometria we will:

1.  Load TS-C JSON reference files
2.  Compute corresponding values via VSOP / pipeline
3.  Compare:
    -   Z_eq vs reference Z
    -   Declination vs reference DEC
4.  Define tolerances
5.  Implement NUnit test cases

No Horizons calls are made in Astronometria.

------------------------------------------------------------------------

## Required Source Files in New Chat

To proceed efficiently, provide:

-   Current Astronometria test project structure
-   Time system implementation (AstroTime, TT handling)
-   Current VSOP interface for geocentric position
-   Existing TS-B test example (for pattern reuse)

------------------------------------------------------------------------

## Testing Philosophy

-   Horizons = external physical reference
-   Astronometria = independent computational pipeline
-   No coordinate transformations reused from EphemerisRegression
-   Deterministic tests only
-   No heuristic tolerances without justification

------------------------------------------------------------------------

## First Target

Implement one full TS-C test for Neptune:

-   Ascending node
-   Descending node
-   Validate sign change
-   Validate JD proximity
-   Validate numeric tolerance

------------------------------------------------------------------------

This chat should remain strictly focused on Astronometria test logic. No
modifications to EphemerisRegression unless explicitly required.
