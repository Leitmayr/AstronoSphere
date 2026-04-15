# TS-C (GeoDec Nodes) -- Test Development Documentation

**Generated:** 2026-03-02T19:27:56.747403 UTC

------------------------------------------------------------------------

## 1. Purpose of TS-C

TS-C validates geocentric declination node crossings using:

-   **Vector mode (VECTORS)** in equatorial frame (ICRF)
-   **Observer mode (OBSERVER)** delivering RA/DEC directly from
    Horizons

Philosophy: EphemerisRegression performs minimal internal calculations.
Horizons performs the physical transformations. Astronometria and
Horizons remain computationally independent.

------------------------------------------------------------------------

## 2. TS-C Architecture

Pipeline:

1.  Event generation (`GeoDecNodeEventRunner`)
2.  Raw export (`GeoDecNodeL0RawExportRunner`)
    -   Vector (ICRF, TT)
    -   Observer (RA/DEC, TT)
3.  JSON export (`GeoDecNodeL0ExportRunner`)
    -   Vector node detection via Z sign change
    -   Observer node detection via DEC sign change

------------------------------------------------------------------------

## 3. Vector Configuration (TS-C)

Frame: Equatorial inertial (ICRF) Center: 500@399 (Earth-centered)
TimeType: TT RefPlane: FRAME RefSystem: ICRF EphemType: VECTORS

Expected behavior: Z component changes sign when crossing the celestial
equator.

------------------------------------------------------------------------

## 4. Observer Configuration (TS-C)

EphemType: OBSERVER Center: 500@399 TimeType: TT (Observer allows UT or
TT only) QUANTITIES=1 (RA/DEC) No RA_FORMAT parameter (causes API 400
error)

Expected behavior: DEC changes sign at the same JD as vector Z.

------------------------------------------------------------------------

## 5. Validation Strategy

For each event:

-   Confirm Z sign change (Vector file)
-   Confirm DEC sign change (Observer file)
-   Confirm identical JD
-   Confirm Reference frame = ICRF in header

------------------------------------------------------------------------

## 6. Test Integration in Astronometria

Astronometria should:

-   Load VectorEq JSON
-   Load Observer JSON
-   Validate sign change behavior
-   Compare against VSOP-based computation

------------------------------------------------------------------------

## 7. Known Constraints

-   Observer TIME_TYPE must be UT or TT
-   RA_FORMAT parameter causes API 400 error
-   Step size must not contain spaces (use 1h, not 1 h)

------------------------------------------------------------------------

TS-C is now architecturally independent, deterministic, and aligned with
Horizons reference output.
