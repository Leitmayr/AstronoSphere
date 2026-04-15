# EphemerisRegression -- GUI-Based Reference Data Generator Concept

Generated: 2026-03-03 19:33 UTC

------------------------------------------------------------------------

## 1. Vision

EphemerisRegression evolves into a GUI-driven scientific reference
generator.

The GUI exposes the same two orthogonal axes as Astronometria:

-   Frame selection
-   CorrectionLevel selection

------------------------------------------------------------------------

## 2. GUI Controls

### Frame

-   Origin: Helio / Geo
-   Plane: Ecliptic / Equatorial
-   Epoch: J2000 / OfDate

Mapped to Horizons parameters: CENTER REF_PLANE REF_SYSTEM

------------------------------------------------------------------------

### Correction Level

Mapped to Horizons: VECT_CORR

Levels: Geometric (NONE) Light-Time (LT) Light-Time + Aberration (LT+S)
Apparent

------------------------------------------------------------------------

### Time Controls

-   Start JD
-   Stop JD
-   Step size

------------------------------------------------------------------------

## 3. Core Logic

GenerateReferenceDataset(Frame, CorrectionLevel, JDSet)

For each JD: Call Horizons with mapped parameters. Serialize CSV (raw)
and JSON (canonical).

------------------------------------------------------------------------

## 4. Strategic Benefit

-   Exact 1:1 mapping between Astronometria and Horizons
-   Rapid generation of reference datasets
-   Isolation of individual pipeline stages
-   Reproducible regression testing
-   Future GUI-driven validation workflow

------------------------------------------------------------------------

## 5. Long-Term Impact

The GUI becomes a deterministic scientific data factory, mirroring
Astronometria's architecture.

------------------------------------------------------------------------

End of document.
