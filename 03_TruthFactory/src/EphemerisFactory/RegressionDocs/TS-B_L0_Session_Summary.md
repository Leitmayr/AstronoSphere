# TS-B Level 0 Session Summary

## 1. Scope of This Session

This session finalized **TS-B -- Level 0 (Geometric, no corrections)**
for:

-   Geocentric Nodes
-   Heliocentric Nodes

All test data was generated exclusively via NASA JPL Horizons API.

Goal: Create stable RAW CSV files and derived JSON test sources for
later regression testing in Astronometria.Ephemerides.Test.

------------------------------------------------------------------------

# 2. Conceptual Definition -- TS-B

TS-B focuses on Ecliptic Node Crossings:

-   Ascending Node (Z: negative → positive)
-   Descending Node (Z: positive → negative)

We do not test the node-finding algorithm here.

We test: - Correct sign of Z before/after event - Correct state vectors
(X, Y, Z) - Correct velocities (VX, VY, VZ) - Stability around ±1 day

------------------------------------------------------------------------

# 3. Horizons Configuration (Level 0)

Common properties:

-   Output type: GEOMETRIC cartesian states
-   Reference frame: Ecliptic of J2000.0
-   Output units: AU-D
-   Step size: 1h
-   Window: ±1 day around precomputed JD
-   No vector corrections (VectorCorrection = null)

## Geocentric (L0_Geo)

Center = 500@399 (Earth center)

## Heliocentric (L0_Helio)

Center = @10 (Sun center)

------------------------------------------------------------------------

# 4. Project: EphemerisRegression

All TS-B L0 data generation was performed here.

## Folder Structure

EphemerisRegression/ └── Horizons/ ├── Geo/ │ └── TS-B/ │ ├── Raw/ │ │
├── `<Planet>`{=html}\_TS-B_L0_AscendingNode.csv │ │ └──
`<Planet>`{=html}\_TS-B_L0_DescendingNode.csv │ └── Json/ │ └──
`<Planet>`{=html}\_TS-B_L0_GeoNodes.json │ └── Helio/ └── TS-B/ ├── Raw/
│ ├── `<Planet>`{=html}\_TS-B_L0_AscendingNode.csv │ └──
`<Planet>`{=html}\_TS-B_L0_DescendingNode.csv └── Json/ └──
`<Planet>`{=html}\_TS-B_L0_HelioNodes.json

------------------------------------------------------------------------

# 5. Execution Flow

For both Geo and Helio:

Step 1 -- RAW Export Runner: - GeoNodeL0RawExportRunner -
HelioNodeL0RawExportRunner

Action: - Call Horizons API - Save full response to CSV

Step 2 -- JSON Export Runner: - GeoNodeL0ExportRunner -
HelioNodeL0ExportRunner

Action: - Parse CSV using HorizonsVectorParser - Extract closest state
vectors for: - JD - 1 day - JD - JD + 1 day - Serialize
NodeReferenceModel

------------------------------------------------------------------------

# 6. JSON Structure

Example structure:

{ "Planet": "Mars", "TestSuite": "TS-B", "CorrectionLevel": "L0_Geo" \|
"L0_Helio", "Ascending": { "JulianDate": ..., "Before": { StateVector },
"At": { StateVector }, "After": { StateVector } }, "Descending": { ... }
}

StateVector contains:

-   JulianDate
-   X, Y, Z
-   VX, VY, VZ

------------------------------------------------------------------------

# 7. Validation Observations

Manual validation confirmed:

-   Z sign change matches expected node direction
-   Velocity sign consistent with ascending/descending logic
-   JD values consistent with Horizons Web Interface
-   Geo and Helio behave consistently

------------------------------------------------------------------------

# 8. Architectural Decisions

1.  No internal astronomy logic for test data generation → Horizons is
    single source of truth.

2.  Strict separation:

    -   RAW generation
    -   JSON generation

3.  Exact JD values reused (no recomputation)

4.  Async API calls must always be awaited.

------------------------------------------------------------------------

# 9. Current Status

TS-B Level 0 is complete for:

-   Geo Nodes
-   Helio Nodes

Ready to copy JSON files into:

Astronometria.Ephemerides.Test

Next step: Create NUnit regression tests in the Pipeline project based
on the generated JSON.

------------------------------------------------------------------------

End of TS-B L0 Session Summary.
