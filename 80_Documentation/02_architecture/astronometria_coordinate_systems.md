# Astronometria – Global Coordinate System Definitions

---

## 1. Purpose

This document defines the global reference frames used throughout Astronometria.

These definitions are fundamental and apply consistently across:

- VSOP planetary positions
- Satellite systems (e.g. Jupiter moons)
- Ring geometry (e.g. Saturn)
- Event calculations
- Future correction models

All modules must comply with these definitions.

---

## 2. Ecliptic Reference Frame (J2000)

### Definition

The primary inertial reference frame for planetary state vectors.

### Origin

- Sun (heliocentric computations)
- Earth (after heliocentric subtraction → geocentric)

### Axes

- **X-axis**: Direction of the dynamical vernal equinox (J2000)
- **Z-axis**: North pole of the ecliptic (J2000)
- **Y-axis**: Completes a right-handed coordinate system

Mathematically:

Y = Z × X

### Properties

- Based on dynamical ecliptic and equinox J2000
- Used by VSOP87A
- Cartesian coordinates
- Position unit: AU
- Velocity unit: AU per day (Julian day)

---

## 3. Equatorial Reference Frame (J2000)

### Definition

Obtained by rotating the ecliptic J2000 frame around the X-axis by the mean obliquity of the ecliptic (ε₀, J2000).

### Axes

- **X-axis**: Direction of the vernal equinox (unchanged)
- **Z-axis**: North celestial pole (J2000)
- **Y-axis**: Completes right-handed system

### Rotation

Rotation angle:

ε₀ = mean obliquity of the ecliptic at J2000

Transformation:

X' = X  
Y' = Y cos ε₀ − Z sin ε₀  
Z' = Y sin ε₀ + Z cos ε₀

---

## 4. Units

### Position

- Astronomical Units (AU)

### Velocity

- AU per Julian day

A Julian day is defined as exactly 86400 SI seconds.

This is not a sidereal day.

---

## 5. Internal Frame Policy

All internal state vectors are expressed in:

- Cartesian coordinates
- J2000 reference frame
- TT-based time scale

No “of date” frame transformations are applied in the core state representation.

Precession, nutation, aberration, and apparent-place corrections are handled as separate transformation layers.

---

End of Coordinate System Definition

