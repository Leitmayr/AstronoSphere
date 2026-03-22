Ephemeris Regression – Session Summary (Helio & Geo Horizons Integration)
1. Goal of This Session

Establish a robust regression testing framework for:

VSOP-based heliocentric ecliptic coordinates (HE)

Derived geocentric ecliptic coordinates (GE)

Using NASA JPL Horizons as reference

Based on event-driven test epochs (quadrant crossings L0, L6, L12, L18)

2. Test Case Strategy – Rationale
2.1 Why Event-Based Tests?

Instead of random epochs, we defined physically meaningful test cases:

TS-A (Heliocentric Quadrant Crossings)

Heliocentric ecliptic longitude λ:

Event	Condition	Cartesian Meaning
L0	λ = 0h	y sign change (− → +)
L6	λ = 6h	x sign change (+ → −)
L12	λ = 12h	y sign change (+ → −)
L18	λ = 18h	x sign change (− → +)

These are ideal because:

They stress coordinate sign behavior

They detect rotation / orientation errors

They expose small systematic offsets

3. Horizons API – What We Learned
3.1 Correct Minimal Working Request

The API is extremely sensitive to:

Date format

VECT_CORR parameter

STOP_TIME > START_TIME

Correct CENTER

Correct working format:

format=text
COMMAND=199
EPHEM_TYPE=VECTORS
START_TIME=2025-02-27T00:00
STOP_TIME=2025-02-28T00:00
STEP_SIZE=1h
CENTER=@10
REF_PLANE=ECLIPTIC
REF_SYSTEM=ICRF
OUT_UNITS=AU-D


Important:

❌ VECT_CORR=NONE → breaks API

✅ leave it out entirely

Date must be: yyyy-MM-ddTHH:mm

4. Major Breakthrough: EventFinder

Initial start epochs were too inaccurate.

Therefore we implemented:

HelioQuadrantEventGenerator

Algorithm:

Coarse scan (1 day step)

Detect sign change in X or Y

Refine to 1 hour resolution

Linear interpolation:

JD_exact = JD_before + |y_before| / (|y_before| + |y_after|) * ΔJD


Result:

Mercury L0 = 2460725.490759237

Matches analytical expectation exactly

This gave us precise Horizons start epochs.

5. Architecture (Visual Studio Structure)
EphemerisRegression
│
├── Api
│   ├── HorizonsApiClient.cs
│   ├── HorizonsApiRequest.cs
│   └── HelioHorizonsRequestFactory.cs
│
├── EventFinding
│   └── HelioQuadrantEventGenerator.cs
│
├── Events
│   ├── HelioEvent.cs
│   └── HelioEvents.cs
│
├── Parser
│   └── HorizonsVectorParser.cs
│
├── Runner
│   ├── HelioApiRunner.cs
│   ├── GeoApiRunner.cs
│   └── HelioEventGenerationRunner.cs
│
└── Program.cs


Config-based mode switching:

Event detection

Horizons regression

6. Helio vs Geo Reference Strategy

Pipeline:

VSOP → HE → GE → GA


Testing strategy:

Level	Reference
HE	Horizons with CENTER=@10
GE	Horizons with CENTER=@399
GA	later

GE references are NOT derived from HE; Horizons computes them directly.

7. Statistics Analysis
7.1 Typical Errors (HE)

Most planets:

1e-7 AU → ~15 km

1e-6 AU → ~150 km

1e-5 AU → ~1500 km

But:

Neptune L18

Deviation:

0.000360 AU ≈ 54 000 km


This is NOT numerical noise.

This is a systematic model difference.

8. Why Errors Are Worst Near L0?

Near quadrant crossings:

One coordinate ≈ 0

Any constant offset dominates relative error

Small angular misalignment → large projected error

But:

The 54 000 km case is too large to be explained by projection alone.

Likely causes:

Time scale mismatch (TT vs TDB)

VSOP truncation

Different reference frame realization

ICRF vs dynamical frame differences

Light-time handling differences

Needs deeper analysis tomorrow.

9. Test Infrastructure Built

Implemented:

JSON reference storage

Automatic vector parsing

NUnit regression tests

Deviation statistics export

Per planet / per quadrant metrics:

MaxX/Y/Z

RMSX/Y/Z

Tests are now:

Executable

Data-driven

Structured

Extendable to velocities

10. Velocity Strategy

Even though pipeline does not compute velocity yet:

Test cases are already structured to include VX/VY/VZ

They can be disabled in Test Explorer

Architecture already supports them

Good future-proofing decision.

11. Current Status

✅ Horizons integration stable
✅ Event detection validated
✅ Helio and Geo regression running
✅ Statistics export implemented
⚠ Neptune-level discrepancies need investigation
⚠ Time-scale alignment (TT vs TDB) must be reviewed

12. What We Achieved Today

Full automated event-based regression generation

Stable Horizons API integration

Systematic test architecture

Quantitative deviation measurement

Identification of model-scale differences

This was a structural breakthrough session.