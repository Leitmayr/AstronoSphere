# Architecture Freeze V2 -- Time Domain Separation

## Executive Summary

Version V2 introduces a strict, compile-time enforced separation of time
domains within Astronometria.\
The astronomical core now operates exclusively on **Terrestrial Time
(TT)** via the new `TTInstant` type.\
Legacy mixed time representations (`AstroTime`) have been completely
removed.\
The dependency direction is strictly enforced as:

User → Conversion → Astro

This guarantees physical correctness, architectural clarity, and
compile-time protection against domain misuse.

------------------------------------------------------------------------

# Time Domain Separation -- V2 (Architecture Freeze)

## Motivation

In Version V1, UTC (User-Domain) and TT (Astro-Domain) were mixed inside
the class `AstroTime`.\
This resulted in a semantically unclear time representation:

-   UTC could unintentionally enter the Astro-Domain\
-   TT could be misused in the User-Domain\
-   Compiler-level domain separation was impossible

The goal of V2 was a **hard, type-based separation of time domains**,
such that incorrect usage is detected at compile time.

------------------------------------------------------------------------

## Result

### 1. Introduction of a Dedicated Astro Time Type

    AstroSim.Time.Astro
    └── TTInstant

`TTInstant`:

-   Represents Terrestrial Time (TT) exclusively\
-   Contains only astronomy-relevant functionality:
    -   `JulianDayTT`
    -   `JulianCenturiesSinceJ2000()`
    -   `JulianMillenniaSinceJ2000()`
-   Is immutable\
-   Is domain-specific\
-   Contains no UTC logic

------------------------------------------------------------------------

### 2. Ephemerides Core Fully TT-Based

The following components now accept **only `TTInstant`**:

-   `IVsopProvider`
-   `VsopProvider`
-   `PlanetPositionService`

The Astro-Domain operates exclusively in TT.

UTC is technically impossible to use in this layer.

------------------------------------------------------------------------

### 3. Removal of Legacy Time Logic

The class `AstroTime` has been completely removed.

No mixed representation of:

-   JD_UT\
-   JD_TT\
-   DeltaT logic

remains inside the core.

Time conversion is now exclusively the responsibility of the
Conversion-Domain.

------------------------------------------------------------------------

### 4. Reference Matrix After V2

    AstroSim.Time.User        → (none)
    AstroSim.Time.Conversion  → User
    AstroSim.Time.Astro       → (none)

    AstroSim.Core             → (no Time.* references)
    AstroSim.Ephemerides      → Astro
    AstroSim.Ephemerides.Test → Astro

Critical architectural rule:

    User → Conversion → Astro

Never the opposite direction.

------------------------------------------------------------------------

## Technical Validation

-   198/198 regression tests green\
-   No remaining usages of `AstroTime`\
-   Compiler enforces domain separation

------------------------------------------------------------------------

## Architectural Gain

-   Physically correct time representation\
-   Compile-time protection against domain mixing\
-   Clean separation of:
    -   User Time (UTC)\
    -   Conversion Logic (ΔT)\
    -   Astronomical Computation Time (TT)

The time architecture now matches the conceptual domain model.
