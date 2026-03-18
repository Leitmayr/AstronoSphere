# Astronometria -- Helio Test Suite Rationale (Phase 1)

## Purpose

This document explains the rationale behind the selection and structure
of the heliocentric test suites (TS-A, TS-B, TS-C1) used for regression
validation of the analytical VSOP87A-based ephemeris pipeline.

The goal is to document why these tests were selected and what numerical
and physical properties they validate.

------------------------------------------------------------------------

# 1. Overall Strategy

The heliocentric validation focuses exclusively on geometric J2000
coordinates (CorrectionLevel.GeometricJ2000).

The objective is to validate:

-   Correct implementation of VSOP87A series
-   Correct coordinate conversion to Cartesian ecliptic J2000
-   Numerical stability across multiple orbital cycles
-   Sign consistency and quadrant correctness
-   Proper computation of heliocentric distance

All event epochs are initially determined with coarse accuracy (±3
days). Fine-resolution timing is later extracted directly from JPL
Horizons reference data.

------------------------------------------------------------------------

# 2. TS-A -- Helio Ecliptic Quadrant Transitions

## Concept

Longitude-based quadrant transitions:

λ = 0°, 90°, 180°, 270°

Since:

x = r cos λ\
y = r sin λ

These transitions enforce deterministic sign changes in x and y.

## What TS-A Validates

-   Correct atan2 behavior
-   Correct quadrant assignment
-   Correct Cartesian projection from spherical coordinates
-   Proper angle normalization
-   Stability over multiple orbits

This test suite detects:

-   Sign inversion bugs
-   Incorrect angle wrapping
-   180° offset errors
-   Axis mixups (x/y swapped)

Two orbital cycles are used per planet to avoid single-phase bias.

------------------------------------------------------------------------

# 3. TS-B -- Helio Node Crossings

## Concept

Node crossings occur when:

z = 0

These correspond to orbital plane crossings of the ecliptic.

## What TS-B Validates

-   Correct inclination handling
-   Proper Z-component construction
-   Sign consistency of ecliptic latitude
-   Long-term stability of orbital plane geometry

This test suite detects:

-   Sign errors in inclination terms
-   Missing Z-series contributions
-   Coordinate rotation mistakes

Two full node cycles are used per planet.

------------------------------------------------------------------------

# 4. TS-C1 -- Helio Radius Extrema

## Concept

Perihelion and aphelion correspond to extrema of:

r = sqrt(x² + y² + z²)

These represent true geometric extrema of the orbital ellipse.

## What TS-C1 Validates

-   Correct radial distance computation
-   Stability of eccentricity terms
-   Proper summation of VSOP radial series
-   Detection of drift in long-period terms

Mars and Jupiter are especially important:

-   Mars: high eccentricity → sensitive to radial errors
-   Jupiter: long period → validates long-term stability

Two full orbital cycles are used per planet.

------------------------------------------------------------------------

# 5. Why Only Helio in Phase 1

The heliocentric tests isolate the analytical core (VSOP87A) without
interference from:

-   Light-time correction
-   Aberration
-   Precession
-   Nutation

This guarantees that any deviation detected at this stage originates
strictly from the analytical ephemeris core.

Geocentric and corrected tests are introduced only after the
heliocentric baseline is fully validated.

------------------------------------------------------------------------

# 6. Event Time Determination Philosophy

Instead of implementing a complex analytical event finder, the following
approach is used:

1.  Compute coarse start epochs (±3 days)
2.  Use JPL Horizons to densely sample the time window
3.  Extract precise event epochs from reference data

This ensures:

-   Alignment with DE ephemerides
-   No overengineering
-   Reproducibility
-   Scientific consistency

------------------------------------------------------------------------

# 7. Expected Numerical Sensitivity

  Test Suite   Sensitivity Type
  ------------ ---------------------------------
  TS-A         Quadrant & sign stability
  TS-B         Inclination & Z-geometry
  TS-C1        Radial & eccentricity stability

Combined, these suites provide broad structural validation of the
heliocentric analytical pipeline across geometry, angle handling, and
radial behavior.

------------------------------------------------------------------------

# 8. Conclusion

The heliocentric test suites were selected to provide:

-   Maximum structural coverage
-   Minimal algorithmic complexity
-   High diagnostic power
-   Compatibility with Horizons-based validation

They form the geometric baseline for all subsequent correction-level
tests.

------------------------------------------------------------------------

End of Helio Test Suite Rationale (Phase 1)
