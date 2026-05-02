# RC5 – Frame Mapping Fix

## Status
Completed and validated

## Problem
Horizons requests used a hardcoded:
REF_PLANE = ECLIPTIC

This ignored the Scenario Frame definition and caused incorrect physical interpretation,
especially for GeoEquatorial scenarios (TS-C).

## Root Cause
Missing mapping from:
Core.Frame.Type → Horizons REF_PLANE

## Fix
Implemented mapping in HorizonsRequestBuilder:

- GeoEcliptic   → ECLIPTIC
- HelioEcliptic → ECLIPTIC
- GeoEquatorial → FRAME

## Validation

### Test Cases
- AS-000063
- AS-000066
- AS-000072

### Results
- REF_PLANE correctly switched to FRAME
- CanonicalRequest matches expected structure
- State vectors unchanged (within rounding precision)

## Conclusion
Bug fully resolved.
No regression observed.
Deterministic behavior preserved.

## Timestamp
2026-04-02T15:15:17.958400
