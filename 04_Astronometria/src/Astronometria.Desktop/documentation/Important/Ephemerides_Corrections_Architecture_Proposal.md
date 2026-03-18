# Astronometria -- Ephemerides Corrections Architecture Proposal

## Purpose

This document defines a modular corrections architecture for the
Astronometria ephemerides pipeline. The goal is to introduce physically
separated, testable correction layers that can be validated individually
against JPL Horizons reference data.

This architecture is forward-looking and does not modify the current
pipeline implementation. It enables clean extensibility and
deterministic regression testing for future physical corrections.

------------------------------------------------------------------------

# 1. Design Goals

-   Preserve determinism
-   Keep the analytical VSOP87A core untouched
-   Allow incremental activation of physical corrections
-   Make every correction layer independently testable
-   Enable 1:1 validation against JPL Horizons configuration modes

------------------------------------------------------------------------

# 2. Baseline (Current Implementation)

Current pipeline state:

Geocentric, geometric, mean equator/equinox J2000

Pipeline:

VSOP87A (heliocentric, TT) → Earth subtraction → Geocentric ecliptic
J2000 → Rotation by mean obliquity ε₀ → Equatorial J2000 → RA/Dec

No light-time, aberration, precession, or nutation.

This corresponds to:

CorrectionLevel.GeometricJ2000

------------------------------------------------------------------------

# 3. Proposed Correction Levels

The following correction hierarchy is proposed.

Each level builds on the previous one.

------------------------------------------------------------------------

## Level 0 --- Geometric J2000

-   Geocentric
-   Geometric
-   Mean equator/equinox J2000
-   No light-time correction
-   No aberration
-   No precession
-   No nutation

Horizons configuration: VECT_CORR='NONE' REF_PLANE='FRAME'

------------------------------------------------------------------------

## Level 1 --- Light-Time Corrected

Adds:

-   Iterative light-time correction
-   Emission time adjustment

Still: - No aberration - No precession - No nutation

Horizons configuration: VECT_CORR='LT'

------------------------------------------------------------------------

## Level 2 --- Light-Time + Aberration

Adds:

-   Stellar aberration correction
-   Observer velocity influence

Still: - J2000 reference frame

Horizons configuration: VECT_CORR='LT+S'

------------------------------------------------------------------------

## Level 3 --- Mean of Date

Adds:

-   Precession transformation
-   J2000 → mean equator/equinox of date

Still: - No nutation

Horizons configuration: REF_PLANE='DATE' (without nutation)

------------------------------------------------------------------------

## Level 4 --- True of Date

Adds:

-   Nutation transformation
-   Mean of date → true of date

Horizons configuration: True-of-date output

------------------------------------------------------------------------

## Level 5 --- Apparent Coordinates

Full physical model:

-   Light-time
-   Aberration
-   Precession
-   Nutation
-   Optional relativistic corrections

Corresponds to classical apparent coordinates used in star charts.

------------------------------------------------------------------------

# 4. Recommended Software Structure

Introduce an enum:

enum CorrectionLevel { GeometricJ2000, LightTime, LightTimeAberration,
MeanOfDate, TrueOfDate, Apparent }

Pipeline architecture:

StateVector (geometric J2000) ↓ LightTimeCorrector ↓ AberrationCorrector
↓ PrecessionTransform ↓ NutationTransform ↓ ApparentCoordinateBuilder

Each stage must be:

-   Deterministic
-   Stateless
-   Independently testable

------------------------------------------------------------------------

# 5. Testing Strategy

For each correction level:

-   Generate Horizons batch file with matching configuration
-   Parse vector output
-   Store reference data in JSON
-   Validate pipeline output against reference

This enables:

-   Isolated validation of each physical effect
-   Detection of regression errors per correction layer
-   Controlled numerical tolerance per level

------------------------------------------------------------------------

# 6. Advantages of This Architecture

-   Scientific transparency
-   Modular extensibility
-   Clean regression testing
-   Horizons-based external validation
-   Clear separation between geometric and apparent states

------------------------------------------------------------------------

# 7. Conclusion

This corrections architecture provides a structured and scalable
foundation for extending Astronometria from purely geometric J2000
coordinates to fully corrected apparent positions.

The current implementation corresponds to Level 0. Future
implementations can activate higher levels without refactoring the
analytical VSOP87A core.

End of document.
