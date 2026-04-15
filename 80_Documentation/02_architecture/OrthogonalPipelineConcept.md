# Astronometria -- Orthogonal Frame and Correction Pipeline Architecture

Generated: 2026-03-03 19:33 UTC

------------------------------------------------------------------------

## 1. Fundamental Principle

Two orthogonal pipelines:

1.  **Geometric Frame Pipeline**
2.  **Physical Correction Pipeline**

Fundamental internal frame: HelioEclipticJ2000 (geometric, inertial,
TT-based).

------------------------------------------------------------------------

## 2. Frame Pipeline (Geometric Transformations)

Stages:

1.  VSOP → HelioEclipticJ2000
2.  Origin Transform (Helio / Geo)
3.  Plane Transform (Ecliptic / Equatorial)
4.  Epoch Transform (J2000 / OfDate via Precession/Nutation)

Frame defines: - Origin - Axis orientation - Epoch reference

Frame does NOT define physical effects.

------------------------------------------------------------------------

## 3. Correction Pipeline (Physical Model Depth)

Stages:

1.  Light-Time
2.  Aberration
3.  Relativistic Deflection
4.  Topocentric
5.  Refraction

Corrections modify the physical apparent location of the object. They do
not change coordinate axes.

------------------------------------------------------------------------

## 4. Combined Pipeline

r = VSOP → Frame Pipeline → Correction Pipeline → return r

------------------------------------------------------------------------

## 5. Velocity Computation

Velocity is computed via numerical differentiation of the fully
processed position:

v(t) = d/dt \[ FullPipeline(r(t)) \]

This guarantees correctness for time-dependent transformations.

------------------------------------------------------------------------

## 6. Architectural Advantages

-   Clear separation of geometry and physics
-   Testability per stage
-   Horizons parameter mapping compatibility
-   Long-term maintainability
-   Deterministic behavior

------------------------------------------------------------------------

End of document.
