# Astronometria – Ephemerides Pipeline Context

**Purpose of this document:**  
Provide a precise technical description of the internal ephemeris pipeline so that external tooling (e.g. JPL Horizons batch generator + parser) can generate regression reference datasets compatible with Astronometria.

This document is intended to be used in parallel development chats where automated reference data generation is implemented.

---

# 1. High-Level Pipeline Overview

The analytical ephemeris pipeline in Astronometria is strictly layered and deterministic.

```
VSOP87A (heliocentric, cartesian, J2000, TT)
    ↓
Heliocentric State Vector (Planet)
    ↓
Heliocentric State Vector (Earth)
    ↓
Geocentric subtraction (Planet − Earth)
    ↓
Geocentric Ecliptic Cartesian (J2000)
    ↓
Rotation by mean obliquity ε₀
    ↓
Geocentric Equatorial Cartesian (J2000)
    ↓
RA/Dec derivation (atan2, asin)
```

No numerical integration is used.
No precession, nutation, aberration, or light-time correction is applied in v1.

The pipeline is purely analytical and stateless.

---

# 2. Time Model

## 2.1 Internal Time Scale

Internal ephemeris time: **TT (Terrestrial Time)**

Time input options:
- Calendar UTC → converted to JD_UT → converted to JD_TT using ΔT
- Direct JD_TT constructor

Julian epoch reference:

```
J2000.0 = JD 2451545.0 (TT)
```

VSOP time parameter:

```
T = (JD_TT − 2451545.0) / 365250.0
```

Unit: Julian millennia

---

## 2.2 ΔT Model

ΔT (TT − UT) implemented using Meeus polynomial approximation.

For regression against Horizons:
- Horizons output must be requested in **TDB**
- TT vs TDB difference (~milliseconds) is negligible at AU tolerance 1e-7

---

# 3. Reference Frames

## 3.1 Ecliptic Reference Frame (Primary)

- Origin: Sun (heliocentric), Earth (after subtraction)
- Frame: Dynamical ecliptic and equinox J2000
- Cartesian coordinates (X, Y, Z)

Axes:
- X-axis → Direction of dynamical vernal equinox (J2000)
- Z-axis → North ecliptic pole (J2000)
- Y-axis → Completes right-handed system

Units:
- Position: AU
- Velocity: AU/day

---

## 3.2 Geocentric Ecliptic

Computed as:

```
Geo_ecl = Helio_planet − Helio_earth
```

No light-time correction.
Pure geometric state.

---

## 3.3 Equatorial Reference Frame (J2000)

Rotation applied:

```
X_eq = X_ecl
Y_eq = Y_ecl cos ε₀ − Z_ecl sin ε₀
Z_eq = Y_ecl sin ε₀ + Z_ecl cos ε₀
```

Where:
- ε₀ = mean obliquity of the ecliptic (J2000)

Resulting frame:
- Mean equator and equinox of J2000
- ICRF-aligned (validated against JPL Horizons)

---

# 4. RA/Dec Derivation

From equatorial Cartesian vector:

```
r = √(X² + Y² + Z²)

RA  = atan2(Y, X)
if RA < 0 → RA += 2π

Dec = asin(Z / r)
```

Range normalization:
- RA ∈ [0, 2π)
- Dec ∈ [−π/2, +π/2]

No apparent corrections applied.

---

# 5. Horizons Configuration Requirements (for Regression Data)

To generate compatible reference data via JPL Horizons batch mode, the following configuration MUST be used:

- Table type: Vector table
- Observer: @399 (Earth center)
- Frame: ICRF
- Reference plane: x-y axes of reference frame (equatorial-aligned, inertial)
- Vector correction: geometric states
- Time scale: TDB
- Output units: AU-D

Important:
DO NOT use:
- "body mean equator and node of date"
- apparent states
- light-time corrected vectors

Otherwise a systematic rotation or offset will occur.

---

# 6. Numerical Expectations

## Internal Algebraic Checks

Heliocentric validation against IMCCE vsop87.chk:
- Tolerance: 1e-10 AU

Geocentric subtraction:
- Tolerance: 1e-10 AU

## Horizons Cross-Validation

Typical deviation vs JPL DE ephemerides:

```
~ 1e-7 AU (tens of meters)
```

This is expected due to:
- Analytical VSOP87A vs numerical DE integration
- TT vs TDB differences

---

# 7. Current Limitations (v1)

Not yet included:

- Precession of equinox
- Nutation
- Aberration
- Light-time correction
- Relativistic effects
- Topocentric correction

All states are:

> Geocentric, geometric, mean equator/equinox J2000

---

# 8. Regression Strategy Goal

The goal of automated Horizons batch generation is:

- Increase regression depth
- Provide multi-epoch validation across 1600–2500
- Detect numerical regressions when:
  - Replacing VSOP implementation
  - Introducing alternative ephemerides
  - Refactoring coordinate transforms

Regression tests should validate:

- Heliocentric Cartesian
- Geocentric Cartesian (ecliptic)
- Geocentric Cartesian (equatorial)
- RA/Dec

---

# 9. Determinism

The entire pipeline is:

- Stateless
- Purely analytical
- Deterministic for given JD_TT

This makes it suitable for large-scale batch validation against external datasets.

---

End of Ephemerides Pipeline Context Document

