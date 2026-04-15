# Derivation of Physically Consistent Position Tolerances

**Astronometria -- VSOP87A Model Consistency**

Generated: 2026-02-19 21:27 UTC

------------------------------------------------------------------------

## 1. Background

VSOP87A provides planetary positions with published angular residuals
relative to DE200:

-   Inner planets: \~1 arcsecond
-   Jupiter/Saturn: \~1--2 arcseconds
-   Uranus/Neptune: up to \~5 arcseconds

These are *angular* deviations.

Regression tests compare Cartesian state vectors in **AU**, therefore
angular residuals must be converted into linear tolerances.

------------------------------------------------------------------------

## 2. Angular Conversion

1 arcsecond:

1″ = 4.848136811 × 10⁻⁶ radians

For small angles:

Δr ≈ r · Δθ

Thus:

Linear position error scales with heliocentric distance.

------------------------------------------------------------------------

## 3. Typical Orbital Radii

  Planet    Mean Distance (AU)
  --------- --------------------
  Mercury   0.4
  Venus     0.7
  Earth     1.0
  Mars      1.5
  Jupiter   5.2
  Saturn    9.5
  Uranus    19
  Neptune   30

------------------------------------------------------------------------

## 4. Expected Position Errors

### Inner planets (\~1″)

Δr ≈ r · 4.8e−6

Mercury:

0.4 × 4.8e−6 ≈ 2e−6 AU

Earth:

1.0 × 4.8e−6 ≈ 5e−6 AU

Mars:

1.5 × 4.8e−6 ≈ 7e−6 AU

Magnitude: 5e−6 AU

------------------------------------------------------------------------

### Jupiter / Saturn (\~2″)

Jupiter:

5.2 × 2 × 4.8e−6 ≈ 5e−5 AU

Saturn:

9.5 × 2 × 4.8e−6 ≈ 9e−5 AU

Magnitude: 1e−4 AU

------------------------------------------------------------------------

### Uranus / Neptune (\~5″)

Neptune:

30 × 5 × 4.8e−6 ≈ 7e−4 AU

Magnitude: \~7e−4 AU

------------------------------------------------------------------------

## 5. Key Observation

Position tolerances increase with heliocentric radius.

Outer planets therefore require significantly larger linear tolerances
than inner planets.

------------------------------------------------------------------------

## 6. Recommended Tolerances

### Heliocentric

    Mercury: 5e-6 AU
    Venus:   5e-6 AU
    Earth:   5e-6 AU
    Mars:    1e-5 AU
    Jupiter: 5e-5 AU
    Saturn:  1e-4 AU
    Uranus:  5e-4 AU
    Neptune: 7e-4 AU

### Geocentric

Geocentric positions:

r_geo = r_planet − r_earth

Error propagation increases uncertainty.

Recommended safety factor: ×2

    Mercury: 1e-5 AU
    Venus:   1e-5 AU
    Earth:   1e-5 AU
    Mars:    2e-5 AU
    Jupiter: 1e-4 AU
    Saturn:  2e-4 AU
    Uranus:  1e-3 AU
    Neptune: 1e-3 AU

------------------------------------------------------------------------

## 7. Scientific Interpretation

These tolerances:

-   Reflect published VSOP87A angular accuracy
-   Avoid artificial numerical precision
-   Maintain physical consistency
-   Align regression thresholds with model limitations

VSOP87A is the dominant model error source.

Tolerances therefore reflect *model accuracy*, not numerical identity.

------------------------------------------------------------------------

End of document.
