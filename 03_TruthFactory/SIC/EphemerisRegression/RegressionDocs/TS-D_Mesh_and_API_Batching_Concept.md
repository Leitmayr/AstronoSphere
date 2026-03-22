# TS-D – Mesh & Horizons API Batching Concept
## EphemerisRegression Design Document

---

# Executive Summary

This document defines the Mesh generation strategy and the Horizons API batching architecture for TS-D (Mesh Tests).

Goals:
- Statistically meaningful accuracy evaluation of VSOP vs DE-440
- Deterministic and reproducible time sampling
- API-efficient batch processing
- Controlled degradation analysis outside the specification interval
- Clean separation of concerns (Mesh ≠ API ≠ VSOP)

---

# 1. Conceptual Architecture

EphemerisRegression responsibilities:
1. Generate time mesh (UTC-based)
2. Batch request Horizons reference data
3. Compute VSOP results for same timestamps
4. Compute deviations
5. Export deviation data for Python statistical analysis

Mesh generation and API batching are strictly separated concerns.

---

# 2. Mesh Design

## Core Principle
- Deterministic
- Segment-based
- Planet-independent
- Produces UTC timestamps (Horizons-compatible)
- Reproducible via fixed definition

---

## Three-Zone Strategy

Originally, these fixed ranges were planned (Three Epochs Approach):

Epoche 1 (Core Zone): 1600–2400, Step 30d (inner) / 60d (outer)
Epoche 2 (Extended Zone): 0–4000, Step 180d
Epoche 3 (Extreme Zone): -4000–8000, Step 2y

However, since this data range is not available for all planets in a heliocentric ecliptical reference system, the range have to be defined according to the available ranges in JPL-Horizons.

## Determination of the available ranges in Horizons

The min max. ranges were determined by means of dedicated API-requests outside the specified range, to achieve an error return from the horizons which tells about the borders:

Range Mercury:
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=199&EPHEM_TYPE=VECTORS&START_TIME=JD0.5&STOP_TIME=JD2305507.5&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> Beginn bei JD0
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=199&EPHEM_TYPE=VECTORS&START_TIME=JD9000000&STOP_TIME=JD10000D&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> No ephemeris for target "Mercury" after A.D. 9999-DEC-30 12:00:00.0000 TDB

Range Venus:
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=299&EPHEM_TYPE=VECTORS&START_TIME=JD0.5&STOP_TIME=JD2305507.5&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> Beginn bei JD0
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=299&EPHEM_TYPE=VECTORS&START_TIME=JD9000000&STOP_TIME=JD10000D&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> No ephemeris for target "Venus" after A.D. 9999-DEC-30 12:00:00.0000 TDB

Range Earth:
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=399&EPHEM_TYPE=VECTORS&START_TIME=JD0.5&STOP_TIME=JD2305507.5&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> Beginn bei JD0

https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=399&EPHEM_TYPE=VECTORS&START_TIME=JD9000000&STOP_TIME=JD10000D&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> No ephemeris for target "Earth" after A.D. 9999-DEC-30 12:00:00.0000 TDB

Range Mars:
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=499&EPHEM_TYPE=VECTORS&START_TIME=JD2305447.5&STOP_TIME=JD2305507.5&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> No ephemeris for target "Mars" prior to A.D. 1600-JAN-02 00:00:00.0000 TDB
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=499&EPHEM_TYPE=VECTORS&START_TIME=JD9000000&STOP_TIME=JD10000D&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> No ephemeris for target "Mars" after A.D. 2600-JAN-01 00:00:00.0000 TDB

Range Jupiter:
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=599&EPHEM_TYPE=VECTORS&START_TIME=JD2305447.5&STOP_TIME=JD2305507.5&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> No ephemeris for target "Jupiter" prior to A.D. 1600-JAN-11 00:00:00.0000 TDB
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=599&EPHEM_TYPE=VECTORS&START_TIME=JD9000000&STOP_TIME=JD10000D&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> No ephemeris for target "Jupiter" after A.D. 2200-JAN-09 00:00:00.0000 TDB

Range Saturn:
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=699&EPHEM_TYPE=VECTORS&START_TIME=JD2305447.5&STOP_TIME=JD2305507.5&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> No ephemeris for target "Saturn" prior to A.D. 1749-DEC-31 00:00:00.0000 TDB
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=699&EPHEM_TYPE=VECTORS&START_TIME=JD9000000&STOP_TIME=JD10000D&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> No ephemeris for target "Saturn" after A.D. 2250-JAN-05 00:00:00.0000 TDB

Range Uranus:
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=799&EPHEM_TYPE=VECTORS&START_TIME=JD2305447.5&STOP_TIME=JD2305507.5&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> No ephemeris for target "Uranus" prior to A.D. 1600-JAN-05 00:00:00.0000 TDB
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=799&EPHEM_TYPE=VECTORS&START_TIME=JD9000000&STOP_TIME=JD10000D&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> No ephemeris for target "Uranus" after A.D. 2399-DEC-16 00:00:00.0000 TDB

Range Neptune:
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=899&EPHEM_TYPE=VECTORS&START_TIME=JD2305447.5&STOP_TIME=JD2305507.5&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> No ephemeris for target "Neptune" prior to A.D. 1600-JAN-05 00:00:00.0000 TDB
https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=899&EPHEM_TYPE=VECTORS&START_TIME=JD9000000&STOP_TIME=JD10000D&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
-> No ephemeris for target "Neptune" after A.D. 2400-JAN-01 00:00:00.0000 TDB

## Valid time ranges
  Planet    Min Date (TDB)   Min JD      Max Date (TDB)   Max JD
  --------- ---------------- ----------- ---------------- -----------
  Mercury   -4713-11-25            0.5   9999-12-30       5373482.5
  Venus     -4713-11-25            0.5   9999-12-30       5373482.5
  Earth     -4713-11-25            0.5   9999-12-30       5373482.5
  Mars       1600-01-02      2305448.5   2600-01-01       2670690.5
  Jupiter    1600-01-11      2305457.5   2200-01-09       2524601.5
  Saturn     1749-12-31      2360233.5   2250-01-05       2542859.5
  Uranus     1600-01-05      2305451.5   2399-12-16       2597625.5
  Neptune    1600-01-05      2305451.5   2400-01-01       2597641.5

## Interpretation

-   Mercury, Venus, Earth: valid from JD=0.5 to year 9999
-   Mars: 1600--2600
-   Jupiter: 1600--2200
-   Saturn: 1749--2250
-   Uranus: 1600--2399
-   Neptune: 1600--2400

## Consequence for TS-D

Tranlating this to the Three Epochs approach yields:
-   Core (1600--2400): all Planeten, but Clipping to planet specific range required
-   Extended (0--4000): only for Mercury, Venus, Earth completely valid
-   Extreme (-4000--8000): only for Mercury, Venus, Earth completely reasonable

These borders are physical model limitations of the underlying DE-ephemerides, not implementation limitations of the program EphemerisRegression.


---

# 3. Random Sampling Layer (Optional)

- Epoche 1: 3 random timestamps per year
- Epoche 2: 2 random timestamps per year
- Epoche 3: 1 random timestamp per 5 years
- Fixed random seed required
- limits see Section #2

---

# 4. Horizons API Batching Strategy

DO NOT request per timestamp.
Instead: Use START/STOP/STEP batch calls per segment.

Max ~2000–3000 steps per request.
If segment exceeds limit → split into subsegments.

Sequential execution recommended.
Optional 500ms delay between calls.

---

# 5. Caching Strategy

Hash key: planet + start + stop + step
If cached result exists → skip API request

---

# 6. Performance

Without batching: 100,000+ API calls (unacceptable)
With batching: ~50–150 API calls (manageable)

---

# Status

Prepared for TS-D Feature Branch.
JSON export and statistical evaluation intentionally excluded.