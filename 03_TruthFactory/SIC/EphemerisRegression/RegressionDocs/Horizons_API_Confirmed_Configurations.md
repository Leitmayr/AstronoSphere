# Horizons API -- Confirmed Working Configurations

**Generated:** 2026-03-02T19:27:56.747526 UTC

This document lists only configurations verified via successful API
calls.

------------------------------------------------------------------------

## 1. Common Parameters

-   format=text
-   MAKE_EPHEM=YES
-   OBJ_DATA=NO

------------------------------------------------------------------------

## 2. GEO Vector (Equatorial, TS-C)

EPHEM_TYPE=VECTORS\
CENTER=500@399\
REF_PLANE=FRAME\
REF_SYSTEM=ICRF\
TIME_TYPE=TT\
OUT_UNITS=AU-D\
STEP_SIZE=1h

Behavior: Returns Cartesian state vectors (X,Y,Z,VX,VY,VZ) in ICRF.

------------------------------------------------------------------------

## 3. GEO Observer (RA/DEC)

EPHEM_TYPE=OBSERVER\
CENTER=500@399\
TIME_TYPE=TT\
QUANTITIES=1\
ANG_FORMAT=DEG\
CAL_FORMAT=JD\
CSV_FORMAT=YES

Important: - RA_FORMAT must NOT be used (causes HTTP 400). - Only TT or
UT allowed for TIME_TYPE.

Behavior: Returns JDTT, RA (deg), DEC (deg).

------------------------------------------------------------------------

## 4. STEP_SIZE Rules

Valid examples: - 1h - 1d

Invalid: - 1 h (space not allowed)

------------------------------------------------------------------------

## 5. Time Systems

Vectors: TDB or TT allowed\
Observer: UT or TT only

TS-C uses TT consistently.

------------------------------------------------------------------------

## 6. Reference Frames

REF_PLANE=FRAME + REF_SYSTEM=ICRF\
→ Equatorial inertial coordinates\
→ Z corresponds to celestial equator crossing

------------------------------------------------------------------------

This document intentionally excludes unverified or legacy
configurations.
