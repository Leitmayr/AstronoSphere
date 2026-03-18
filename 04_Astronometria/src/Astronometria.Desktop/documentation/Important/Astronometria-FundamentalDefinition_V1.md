# Astronometria – Fundamental Definitions

This document defines the non-negotiable core conventions of Astronometria.

All definitions in this file are normative.

Any change to this document affects numerical identity,
scientific interpretation, or reproducibility.

Such changes require explicit versioning and impact analysis.

---

# 1. Time System

## 1.1 Input Time

- External input time is UTC.
- No direct UT1 modeling is performed.
- UTC ≈ UT is accepted within target precision (~1 arcsecond).

## 1.2 Internal Time Scale

- UTC is converted to TT (Terrestrial Time).
- ΔT is computed using the Meeus model.
- Ephemeris calculations are performed in Julian Ephemeris Date (JDE, TT).

## 1.3 Valid Time Range

- Supported range: 1600–2500.
- Outside this range, accuracy is not guaranteed.

## 1.4 Time Model Stability

A change of ΔT model or time scale (e.g., TT → TDB)
requires:

- Quantified delta analysis
- Mesh comparison
- Explicit documentation

---

# 2. Reference Frames

## 2.1 Inertial Frame

- Base inertial system: J2000.0.
- Effectively aligned with ICRS within ~1 arcsecond.
- No explicit FK5 modeling.

## 2.2 Reference Plane

- Ecliptic of J2000.0.

## 2.3 Coordinate Handedness

- Right-handed coordinate system.
- X-axis toward vernal equinox.
- Z-axis toward north ecliptic pole.

---

# 3. Coordinate Conventions

## 3.1 Internal Units

- Angles internally stored in radians.
- Distances in astronomical units (AU).
- Velocities in AU/day.

## 3.2 Heliocentric Cartesian

VSOP output (L,B,R) is transformed to:

Heliocentric Cartesian coordinates:

X = R * cos(B) * cos(L)  
Y = R * cos(B) * sin(L)  
Z = R * sin(B)

## 3.3 Geocentric Cartesian

Geocentric coordinates are computed as:

Geo = Planet_HE − Earth_HE

Earth subtraction must occur inside the ephemeris pipeline.

It is never performed inside regression or test logic.

---

# 4. External Reference Model

## 4.1 Reference Authority

## 4.1 Reference Authority

JPL Horizons is treated as external scientific ground truth.

Official API documentation:
https://ssd.jpl.nasa.gov/api/horizons.api

Main Horizons system page:
https://ssd.jpl.nasa.gov/horizons/

Validation is performed against the DE series used internally by Horizons
(e.g., DE440).

## 4.2 Ephemeris Series

Validation is performed against the DE series used by Horizons
(e.g., DE440).

VSOP87A was originally fitted to DE200.

Residuals are interpreted accordingly.

---

# 5. Canonical Horizons Request Specification

The request hash is computed over a canonical parameter list.

This specification guarantees reproducibility and
prevents ambiguity caused by URL formatting differences.

## 5.1 Included Parameters

Only the following parameters are included in canonicalization:

COMMAND
CENTER
MAKE_EPHEM
EPHEM_TYPE
REF_PLANE
REF_SYSTEM
VECT_CORR
OUT_UNITS
CSV_FORMAT
OBJ_DATA
START_TIME
STOP_TIME
STEP_SIZE

## 5.2 Normalization Rules

- Parameter names are uppercase.
- Parameter values are uppercase where applicable.
- Time format must be ISO 8601 (YYYY-MM-DDTHH:MM).
- No whitespace.
- No URL encoding variations.
- STEP_SIZE must use explicit unit notation (e.g., 1h).

## 5.3 Ordering Rule

Parameters are sorted alphabetically by parameter name.

## 5.4 Canonical String Format

Canonical request string is constructed as:

PARAMETER=VALUE

(one per line, newline-separated)

Example:

CENTER=500@399
COMMAND=499
CSV_FORMAT=YES
EPHEM_TYPE=VECTORS
MAKE_EPHEM=YES
OUT_UNITS=AU-D
REF_PLANE=ECLIPTIC
REF_SYSTEM=ICRF
START_TIME=2026-06-03T22:00
STEP_SIZE=1h
STOP_TIME=2026-06-05T02:00
VECT_CORR=NONE

## 5.5 Hash Definition

Hash algorithm: SHA256  
Hash is computed over UTF-8 encoded canonical string.  
Canonicalization version must be stored in JSON metadata.

Example JSON:

{
  "Request": {
    "CanonicalizationVersion": "v1",
    "HashAlgorithm": "SHA256",
    "Hash": "..."
  }
}
