# TS-B -- Node Crossing Regression Specification

*(CorrectionLevel.GeometricJ2000)*

------------------------------------------------------------------------

# 1. Purpose

Validation of the ephemeris pipeline at Level 0 using **ecliptic node
crossings** for both heliocentric and geocentric configurations.

------------------------------------------------------------------------

# 2. Horizons Time Format (Critical)

Horizons API requires strict ISO-8601 formatting:

``` csharp
return new HorizonsApiRequest
{
    Command = commandCode,
    StartTime = start.ToString("yyyy-MM-ddTHH:mm"),
    StopTime = stop.ToString("yyyy-MM-ddTHH:mm"),
    StepSize = stepSize,
    Center = "@10",
    RefPlane = "ECLIPTIC",
    RefSystem = "ICRF",
    VectorCorrection = null,
    OutputUnits = "AU-D"
};
```

Important: - `VECT_CORR` is **not passed** at Level 0 - STOP_TIME must
be strictly greater than START_TIME - Format must be `yyyy-MM-ddTHH:mm`

------------------------------------------------------------------------

# 3. Event Definition

Ascending Node: Z_before \< 0 and Z_after \> 0

Descending Node: Z_before \> 0 and Z_after \< 0

Interpolation:

JD_exact = JD_before + \|Z_before\| / (\|Z_before\| + \|Z_after\|) \*
ΔJD

------------------------------------------------------------------------

# 4. Horizons Configuration

Heliocentric: CENTER=@10 REF_PLANE=ECLIPTIC REF_SYSTEM=ICRF

Geocentric: CENTER=@399 REF_PLANE=ECLIPTIC REF_SYSTEM=ICRF

------------------------------------------------------------------------

# 5. NUnit Acceptance Tolerances (Planet-Dependent)

Angular limits based on VSOP87A published residuals. Converted using:

ΔAU = distance_AU × (arcsec × 4.848136811e-6)

Derived maximum absolute tolerance per coordinate:

-   Mercury: 1.890773e-06 AU
-   Venus: 3.490659e-06 AU
-   Earth: 4.848137e-06 AU
-   Mars: 7.369168e-06 AU
-   Jupiter: 5.042062e-05 AU
-   Saturn: 9.289030e-05 AU
-   Uranus: 4.654211e-04 AU
-   Neptune: 7.272205e-04 AU

These values define the maximum allowed \|ΔX\|, \|ΔY\|, \|ΔZ\| in NUnit
asserts.

Example:

Assert.That(deltaX, Is.LessThan(tolerance\[planet\]));

------------------------------------------------------------------------

# 6. Scientific Rationale

VSOP87A residual limits:

Inner planets: \~1″\
Jupiter/Saturn: \~2″\
Uranus/Neptune: up to 5″

Outer planets therefore require significantly larger linear tolerances.

------------------------------------------------------------------------

# 7. Completion Criteria

TS-B is complete when:

-   HelioAscendingNode
-   HelioDescendingNode
-   GeoAscendingNode
-   GeoDescendingNode

are generated for all planets, validated against Horizons, and NUnit
regression passes using planet-dependent tolerances.

------------------------------------------------------------------------

End of TS-B Specification
