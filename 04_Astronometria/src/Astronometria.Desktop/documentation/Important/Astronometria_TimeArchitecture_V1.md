# Astronometria -- Time Architecture Definition V1

Generated: 2026-02-20 22:47 UTC

------------------------------------------------------------------------

## 1. Purpose

This document defines the strict architectural separation of time
domains within Astronometria.

Time is a foundational system component.\
All astronomical correctness, regression reproducibility, and long‑term
maintainability depend on a clean time model.

------------------------------------------------------------------------

## 2. Domain Separation

Astronometria defines two independent time domains:

### 2.1 User Domain

-   Time scale: UTC
-   Gregorian calendar handling
-   Time zones
-   Daylight Saving Time (DST)
-   User input / output representation

Core type:

    UtcInstant

The User Domain must not access TT or TDB.

------------------------------------------------------------------------

### 2.2 Astro Domain

-   Time scale: TT (Terrestrial Time)
-   Internal ephemeris calculations
-   VSOP time parameters
-   J2000 reference calculations

Core type:

    TtInstant

The Astro Domain must not access UTC.

------------------------------------------------------------------------

### 2.3 Conversion Domain (Boundary Layer)

Responsible for:

-   UTC ↔ TT conversion
-   ΔT computation
-   Gregorian ↔ Julian Date conversion

Core component:

    ITimeScaleConverter
    MeeusTimeScaleConverter
    DeltaTCalculator

The Conversion Domain is the only domain allowed to reference both User
and Astro time types.

------------------------------------------------------------------------

## 3. Core Time Types

### 3.1 UtcInstant

Represents a Julian Date in UTC.

Immutable struct.

### 3.2 TtInstant

Represents a Julian Ephemeris Date in TT.

Provides astronomical helpers:

-   JulianCenturiesSinceJ2000()
-   JulianMillenniaSinceJ2000()

------------------------------------------------------------------------

## 4. J2000 Constants

The following constants belong to the Astro Domain:

-   JD_J2000 = 2451545.0
-   DaysPerJulianCentury = 36525.0
-   DaysPerJulianMillennium = 365250.0

These constants are part of astronomical mathematics, not user time
handling.

------------------------------------------------------------------------

## 5. Forbidden Dependencies

The following are strictly prohibited:

-   Astro Domain referencing UTC
-   User Domain referencing TT
-   Implicit time scale conversion
-   Mixed time storage inside a single class

------------------------------------------------------------------------

## 6. Migration Rule

The legacy class `AstroTime` is deprecated.

All ephemeris calculations must use:

    TtInstant

All user interactions must use:

    UtcInstant

------------------------------------------------------------------------

## 7. Future Extension

Future support for TDB or TDC will introduce:

    TdbInstant

without modifying the existing Astro Domain API.

------------------------------------------------------------------------

End of document.
