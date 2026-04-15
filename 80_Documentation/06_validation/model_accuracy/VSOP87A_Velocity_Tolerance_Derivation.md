# Derivation of Physically Consistent Velocity Tolerances

**Astronometria -- VSOP87A Model Consistency**

Generated: 2026-02-19 21:26 UTC

------------------------------------------------------------------------

## 1. Background

VSOP87A provides planetary positions with published angular residuals
relative to DE200:

-   Inner planets: \~1 arcsecond
-   Jupiter/Saturn: \~1--2 arcseconds
-   Uranus/Neptune: up to \~5 arcseconds

These are *angular* deviations.

However, regression tests compare:

-   Position differences in **AU**
-   Velocity differences in **AU/day**

Therefore, angular errors must be converted into linear tolerances.

------------------------------------------------------------------------

## 2. Angular Conversion

1 arcsecond:

1″ = 4.848136811 × 10⁻⁶ radians

For small angles:

Δx ≈ r · Δθ\
Δv ≈ v · Δθ

Thus:

Position error scales with orbital radius\
Velocity error scales with orbital speed

------------------------------------------------------------------------

## 3. Typical Orbital Speeds

  Planet    Velocity (km/s)   Velocity (AU/day)
  --------- ----------------- -------------------
  Mercury   47.4              0.027
  Venus     35.0              0.020
  Earth     29.8              0.017
  Mars      24.1              0.014
  Jupiter   13.1              0.0076
  Saturn    9.7               0.0056
  Uranus    6.8               0.0039
  Neptune   5.4               0.0031

------------------------------------------------------------------------

## 4. Expected Velocity Errors

### Inner planets (\~1″)

Δv ≈ v · 4.8e−6

Example Earth:

0.017 × 4.8e−6 ≈ 8e−8 AU/day

Mercury:

0.027 × 4.8e−6 ≈ 1.3e−7 AU/day

Magnitude: \~1e−7 AU/day

------------------------------------------------------------------------

### Jupiter / Saturn (\~2″)

Jupiter:

0.0076 × 2 × 4.8e−6 ≈ 7e−8

Saturn:

0.0056 × 2 × 4.8e−6 ≈ 5e−8

Magnitude: \~1e−7 AU/day

------------------------------------------------------------------------

### Uranus / Neptune (\~5″)

Neptune:

0.0031 × 5 × 4.8e−6 ≈ 7.5e−8

Magnitude: \~1e−7 AU/day

------------------------------------------------------------------------

## 5. Key Observation

Unlike position tolerances (which increase with heliocentric radius),

velocity tolerances remain approximately constant across planets,

because outer planets move more slowly.

------------------------------------------------------------------------

## 6. Recommended Tolerances

### Heliocentric

    Mercury: 2e-7 AU/day
    Venus:   1e-7 AU/day
    Earth:   1e-7 AU/day
    Mars:    1e-7 AU/day
    Jupiter: 1e-7 AU/day
    Saturn:  1e-7 AU/day
    Uranus:  1e-7 AU/day
    Neptune: 1e-7 AU/day

### Geocentric

Geocentric velocities:

v_geo = v_planet − v_earth

Error propagation increases uncertainty.

Recommended safety factor: ×2

    Mercury: 3e-7 AU/day
    Venus:   3e-7 AU/day
    Earth:   3e-7 AU/day
    Mars:    2e-7 AU/day
    Jupiter: 2e-7 AU/day
    Saturn:  2e-7 AU/day
    Uranus:  2e-7 AU/day
    Neptune: 2e-7 AU/day

------------------------------------------------------------------------

## 7. Scientific Interpretation

These tolerances:

-   Reflect the published angular accuracy of VSOP87A
-   Avoid artificial numerical precision
-   Maintain physical consistency
-   Align regression thresholds with model limitations

VSOP87A is the dominant error source in Astronometria.

The tolerances therefore reflect *model accuracy*, not numerical
identity.

------------------------------------------------------------------------

End of document.
