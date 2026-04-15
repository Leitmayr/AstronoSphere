# Horizons Level Mapping -- Freeze M1

Version: 1.0\
Date: 2026-03-03 21:21 UTC\
MappingVersion: 1\
Project: Astronometria / EphemerisRegression

------------------------------------------------------------------------

# 1. Purpose

This document freezes the normative mapping between:

-   Astronometria Correction Levels (L0--L5)
-   Execution Mode (HELIO / GEO / OBSERVER)
-   Horizons API parameters

Any modification of this mapping requires:

-   MappingVersion increment
-   Baseline reset
-   Mesh revalidation (1600--2500)

------------------------------------------------------------------------

# 2. Global Invariants

The following parameters apply to all VECTORS requests unless explicitly
overridden:

-   MAKE_EPHEM=YES
-   CSV_FORMAT=YES
-   OBJ_DATA=NO
-   OUT_UNITS=AU-D
-   REF_SYSTEM=ICRF
-   Time format: ISO 8601 (YYYY-MM-DDTHH:MM)
-   Time scale: TDB (implicit in Horizons VECTORS)

Canonicalization rules follow Fundamental Definitions.

------------------------------------------------------------------------

# 3. HELIO Mode (CENTER=@10)

  Level   EPHEM_TYPE   VECT_CORR   REF_PLANE
  ------- ------------ ----------- -----------
  L0      VECTORS      NONE        ECLIPTIC
  L1      VECTORS      LT          ECLIPTIC
  L2      VECTORS      LT+S        ECLIPTIC
  L3      VECTORS      LT+S        ECLIPTIC
  L4      --           --          --
  L5      --           --          --

Notes:

-   L3 cannot be isolated in Horizons VECTORS mode.
-   L4/L5 are not meaningful in heliocentric context.

------------------------------------------------------------------------

# 4. GEO Mode (CENTER=500@399)

  Level   EPHEM_TYPE   VECT_CORR   REF_PLANE
  ------- ------------ ----------- -----------
  L0      VECTORS      NONE        ECLIPTIC
  L1      VECTORS      LT          ECLIPTIC
  L2      VECTORS      LT+S        ECLIPTIC
  L3      VECTORS      LT+S        ECLIPTIC
  L4      OBSERVER     LT+S        --
  L5      OBSERVER     LT+S        --

------------------------------------------------------------------------

# 5. OBSERVER Mode (Topocentric)

Normative observer definition (M1):

CENTER=coord@399\
COORD_TYPE=GEODETIC\
SITE_COORD=0,0,0

Equator, sea level, Greenwich meridian.

## L4

-   EPHEM_TYPE=OBSERVER
-   VECT_CORR=LT+S
-   ATMOSPHERE=NONE
-   QUANTITIES=1,3

## L5

-   EPHEM_TYPE=OBSERVER
-   VECT_CORR=LT+S
-   ATMOSPHERE=STANDARD
-   QUANTITIES=1,3

------------------------------------------------------------------------

# 6. Canonical Example (L0 HELIO)

CENTER=@10\
COMMAND=499\
CSV_FORMAT=YES\
EPHEM_TYPE=VECTORS\
MAKE_EPHEM=YES\
OBJ_DATA=NO\
OUT_UNITS=AU-D\
REF_PLANE=ECLIPTIC\
REF_SYSTEM=ICRF\
START_TIME=...\
STEP_SIZE=1h\
STOP_TIME=...\
VECT_CORR=NONE

------------------------------------------------------------------------

# 7. Architectural Notes

1.  L1 in Astronometria uses explicit time iteration. Horizons performs
    internal light-time correction.
2.  L3 (Relativistic Deflection) cannot be isolated in Horizons VECTORS
    mode.
3.  L4/L5 require OBSERVER mode.
4.  MappingVersion must be stored in JSON metadata.

------------------------------------------------------------------------

# 8. Freeze Status

MappingVersion = 1

This mapping is frozen for M1 development.
