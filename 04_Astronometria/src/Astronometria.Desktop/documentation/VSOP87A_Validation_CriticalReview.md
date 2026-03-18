# VSOP87A Validation -- Critical Review of TS-A Results

Created: 2026-02-16 09:55 UTC

## 1. Context

Within Test Suite A (TS-A), heliocentric ecliptic quadrant events (L0,
L6, L12, L18) were validated against JPL Horizons (DE440/DE441).

Maximum observed deviation for Neptune: ≈ 3.6e-4 AU (\~54,000 km)

This document evaluates whether this deviation is consistent with the
published accuracy limits of VSOP87A.

------------------------------------------------------------------------

## 2. Published Accuracy of VSOP87A

Bretagnon & Francou (1988) report typical maximum angular residuals:

-   Inner planets: \~1 arcsecond
-   Jupiter/Saturn: \~1--2 arcseconds
-   Uranus/Neptune: up to \~5 arcseconds

VSOP87 was fitted against DE200. Modern Horizons comparisons use
DE430/DE440/DE441.

------------------------------------------------------------------------

## 3. Angular to Linear Conversion (Neptune)

1 arcsecond = 4.848136811e-6 rad

For Neptune (\~30 AU):

5″ ≈ 30 AU × 5 × 4.848e-6\
≈ 7.27e-4 AU\
≈ 108,000 km

Observed deviation:

3.6e-4 AU ≈ 54,000 km

This corresponds to \~2.5 arcseconds --- well within the documented
residual range.

------------------------------------------------------------------------

## 4. Time Scale Consideration (TT vs TDB)

TT--TDB difference: \~1--2 milliseconds

Neptune orbital velocity ≈ 5 km/s

Resulting positional effect: \~10 meters

Conclusion: Time scale differences cannot explain 54,000 km deviations.

------------------------------------------------------------------------

## 5. Reference Frame Consideration

Possible minor differences between: - Dynamical J2000 (VSOP basis) -
ICRF-based Horizons output

Magnitude: few arcseconds Comparable to VSOP residual level.

Frame effects are secondary to model residuals.

------------------------------------------------------------------------

## 6. Consistency Checks

Observed characteristics:

-   Error magnitude scales with orbital radius
-   Inner planets show much smaller deviations
-   Residuals are smooth and systematic
-   No axis permutation or transformation artifacts detected

This behavior is consistent with theoretical model limits.

------------------------------------------------------------------------

## 7. Conclusion

The observed deviations in TS-A are:

-   Physically plausible
-   Consistent with published VSOP87A limits
-   Not caused by time scale mismatch
-   Not indicative of implementation error

TS-A validation is therefore considered successful within the documented
accuracy bounds of VSOP87A.
