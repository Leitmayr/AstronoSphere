# Ephemerides Validation Report – V1

**Project:** Astronometria  
**Layer:** Ephemerides  
**Status:** VALIDATED (v1)  
**Date:** 2026-02-14

---

## 1. Scope

Validation of the complete analytical ephemeris pipeline:

```
VSOP87A (heliocentric, J2000, TT)
→ Earth subtraction (geocentric, ecliptic)
→ Obliquity rotation (equatorial, J2000)
→ RA/Dec derivation
```

Velocity support is implemented and internally verified but not yet required in production rendering.

---

## 2. Reference Frame Definition

### Ecliptic Reference Frame (J2000)

- Origin: Sun (heliocentric) / Earth (after subtraction)
- X-axis: Direction of the dynamical vernal equinox (J2000)
- Z-axis: North pole of the ecliptic (J2000)
- Y-axis: Completes a right-handed system

### Equatorial Reference Frame (J2000)

- Obtained via rotation by mean obliquity ε₀
- Mean equator of J2000
- Validated against ICRF (JPL Horizons)

### Time Model

- Internal ephemeris time: TT (Julian Date TT)
- ΔT: Meeus polynomial approximation
- External validation performed using TDB (JPL Horizons)

---

## 3. Validation Layers

### 3.1 VSOP87A – Heliocentric Validation

Reference source: IMCCE `vsop87.chk`

- 10 reference epochs per planet
- Tolerance: `1e-10 AU`
- All tests green

**Result:**  
Heliocentric state vectors numerically verified.

---

### 3.2 Geocentric (Ecliptic) Validation

Algebraic validation:

```
Geo = HelioPlanet − HelioEarth
```

Test planets:
- Mars
- Venus
- Jupiter

Test epochs: 1800–2000 range

Tolerance:
`1e-10 AU`

**Result:**  
Earth subtraction and geocentric transformation confirmed.

---

### 3.3 Geocentric Equatorial (J2000) Validation

Reference source: JPL Horizons

Horizons configuration:

- Vector table
- Observer: @399 (Earth)
- Frame: ICRF
- Reference plane: equatorial-aligned inertial
- Vector correction: geometric states
- Time scale: TDB
- Units: AU-D

Test planets:
- Mars
- Venus
- Jupiter

Test epochs:
- JD 2451545.0
- JD 2415020.0
- JD 2378495.0 (where applicable)

Tolerance:
`5e-7 AU`

Observed deviations:
Within expected VSOP vs DE ephemeris differences (tens of meters).

**Result:**  
Independent external validation successful.

---

### 3.4 Quadrant & Angle Robustness

Additional validation ensured:

- RA normalization to [0°, 360°)
- Correct quadrant handling using atan2
- Stable declination sign
- No NaN or infinity in edge cases

All tests green.

---

### 3.5 Time Model Validation

Validated:

- Calendar → JD_TT conversion
- Direct JD_TT constructor
- Julian millennia since J2000 consistency

Tolerance:
`1e-12`

**Result:**  
Time conversion deterministic and numerically stable.

---

## 4. Critical Debug Milestone

A temporary 90° discrepancy was observed during early Horizons comparison.

Root cause:
Incorrect Horizons reference plane setting  
("body mean equator and node of date")

Resolution:
Switched to equatorial-aligned inertial frame.

Conclusion:
The implementation was correct; the discrepancy originated from external configuration.

This prevented the introduction of an unnecessary coordinate rotation and preserved frame consistency.

---

## 5. Numerical Quality Assessment

Typical agreement with JPL DE ephemerides:

- Position differences: ~10⁻⁷ AU
- Equivalent to tens of meters

Fully consistent with analytical VSOP87A limitations.

---

## 6. Conclusion

The analytical ephemeris pipeline is:

- Numerically stable
- Frame-consistent
- Externally validated
- Scientifically robust

The Ephemerides layer is therefore declared:

> **Validated v1 – Scientifically Cross-Verified**

Ready for:

- Integration into rendering
- RA/Dec projection usage
- Simulation layer expansion

---

End of Ephemerides Validation Report – V1

