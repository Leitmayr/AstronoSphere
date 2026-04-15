# Astronometria -- Fundamental Definitions V2

Generated: 2026-02-20 22:47 UTC

------------------------------------------------------------------------

## 1. Time System (Updated)

### 1.1 Input Time

External input time is UTC and represented exclusively by:

    UtcInstant

------------------------------------------------------------------------

### 1.2 Internal Time Scale

All ephemeris calculations use TT exclusively and are represented by:

    TtInstant

UTC must never appear in the Astro Domain.

------------------------------------------------------------------------

### 1.3 ΔT Responsibility

ΔT (TT − UT) is computed exclusively in the Conversion Domain.

The Astro Domain does not compute or store ΔT.

------------------------------------------------------------------------

### 1.4 Valid Time Range

Supported range remains:

1600--2500

Outside this range, accuracy is not guaranteed.

------------------------------------------------------------------------

### 1.5 Time Model Stability

Any change of:

-   ΔT model
-   TT ↔ TDB conversion
-   Reference time scale

requires:

-   Mesh validation
-   Regression validation
-   Documentation update
-   Version increment

------------------------------------------------------------------------

## 2. Reference Frames

Unchanged from V1.

------------------------------------------------------------------------

## 3. Coordinate Conventions

Unchanged from V1.

------------------------------------------------------------------------

## 4. External Reference Model

Unchanged from V1.

------------------------------------------------------------------------

End of document.
