# Astronometria -- Architecture Freeze V2

Generated: 2026-02-20 22:47 UTC

------------------------------------------------------------------------

## 1. Motivation

This freeze documents the structural separation of time domains as a
fundamental architectural decision.

Time handling is now considered a frozen core design.

------------------------------------------------------------------------

## 2. Project Structure

New projects introduced:

-   AstroSim.Time.User
-   AstroSim.Time.Astro
-   AstroSim.Time.Conversion

------------------------------------------------------------------------

## 3. Dependency Rules

  Project       May Reference
  ------------- --------------------------------
  User          None
  Astro         None
  Conversion    User + Astro
  Ephemerides   Astro
  Projection    User
  Simulation    Astro
  Reporting     User + Conversion
  Tests         Astro (+ Conversion if needed)

------------------------------------------------------------------------

## 4. Compiler-Enforced Isolation

Time scale separation is enforced through:

-   Separate assemblies
-   Explicit time types
-   No shared base time class
-   No implicit conversions

------------------------------------------------------------------------

## 5. Structural Stability Level

This freeze establishes:

-   Deterministic time handling
-   Compile-time time scale safety
-   Clear boundary responsibility for ΔT

No further structural refactoring of time is expected before TDB
introduction.

------------------------------------------------------------------------

End of document.
