# Astronometria – Ephemerides Corrections Architecture (v1)

**Purpose**
Define a modular, testable, and Horizons-compatible correction architecture for extending the current geometric J2000 ephemeris pipeline to higher physical fidelity levels.

This document supersedes earlier drafts and reflects alignment with the validated V1 geometric pipeline.

---

# 1. Architectural Principles

1. Preserve determinism
2. Keep VSOP87A analytical core untouched
3. Separate physical corrections from reference-frame transformations
4. Allow incremental activation of correction layers
5. Enable 1:1 validation against JPL Horizons configuration modes
6. Maintain strict layer ordering

---

# 2. Baseline (Current Implementation)

Current state (validated):

Geocentric, geometric, mean equator/equinox J2000

Pipeline:

VSOP87A (heliocentric, TT)
→ Earth subtraction
→ Geocentric ecliptic J2000
→ Rotation by mean obliquity ε₀
→ Equatorial J2000
→ RA/Dec

No light-time, aberration, precession, or nutation.

This corresponds to:

CorrectionLevel.GeometricJ2000

---

# 3. Conceptual Separation

Two fundamentally different operation types exist:

## 3.1 Physical Corrections

Modify the physical direction of the incoming light.

* Light-time correction
* Stellar aberration
* Relativistic terms (future)

## 3.2 Reference Frame Transformations

Change only the coordinate frame, not the physical direction.

* Precession (J2000 → mean of date)
* Nutation (mean → true of date)

This separation must be preserved in implementation.

---

# 4. Correction Levels (Horizons-Aligned)

Each level builds strictly on the previous one.

---

## Level 0 – Geometric J2000

* Geocentric
* Geometric
* Mean equator/equinox J2000

Horizons:
VECT_CORR='NONE'
REF_PLANE='FRAME'

---

## Level 1 – Light-Time Corrected

Adds:

* Iterative light-time solution
* Emission time adjustment

Still J2000 frame.

Horizons:
VECT_CORR='LT'

---

## Level 2 – Light-Time + Aberration

Adds:

* Stellar aberration
* Observer velocity contribution

Still J2000 frame.

Horizons:
VECT_CORR='LT+S'

---

## Level 3 – Mean of Date

Adds:

* Precession transform
* J2000 → mean equator/equinox of date

Horizons:
REF_PLANE='DATE' (mean)

---

## Level 4 – True of Date

Adds:

* Nutation transform
* Mean → true equator/equinox of date

Horizons:
True-of-date configuration

---

## Level 5 – Apparent Coordinates

Full physical model:

* Light-time
* Aberration
* Precession
* Nutation
* Optional relativistic refinements

Corresponds to classical apparent sky positions.

---

# 5. Software Architecture

## 5.1 Core Interface

```
public interface IStateVectorCorrector
{
    StateVector Apply(StateVector state, AstroTime time);
}
```

All correction layers must:

* Be stateless
* Be deterministic
* Not depend on UI
* Be individually testable

---

## 5.2 Pipeline Executor

```
public sealed class EphemerisPipeline
{
    private readonly IReadOnlyList<IStateVectorCorrector> _steps;

    public EphemerisPipeline(IEnumerable<IStateVectorCorrector> steps)
    {
        _steps = steps.ToList();
    }

    public StateVector Execute(StateVector baseState, AstroTime time)
    {
        var state = baseState;
        foreach (var step in _steps)
            state = step.Apply(state, time);
        return state;
    }
}
```

---

## 5.3 Correction Configuration

```
public sealed class EphemerisCorrectionConfig
{
    public bool ApplyLightTime { get; init; }
    public bool ApplyAberration { get; init; }
    public bool ApplyPrecession { get; init; }
    public bool ApplyNutation { get; init; }
}
```

Factory builds pipeline from config.

---

# 6. Execution Order (Strict)

1. LightTimeCorrector
2. AberrationCorrector
3. PrecessionTransform
4. NutationTransform

Changing this order is not allowed.

---

# 7. Horizons Regression Mapping

For automated regression generation, Horizons must mirror the selected correction level.

Example mapping:

| Correction Level    | Horizons Configuration              |
| ------------------- | ----------------------------------- |
| GeometricJ2000      | VECT_CORR='NONE', REF_PLANE='FRAME' |
| LightTime           | VECT_CORR='LT'                      |
| LightTimeAberration | VECT_CORR='LT+S'                    |
| MeanOfDate          | REF_PLANE='DATE'                    |
| TrueOfDate          | True-of-date mode                   |

Batch-generated vector outputs should be parsed and stored as JSON for regression validation.

---

# 8. Determinism and Regression Strategy

Each correction level must:

* Produce identical output for identical JD_TT
* Be independently validated against Horizons
* Maintain numeric tolerances appropriate to physical model

Regression depth should include:

* Multiple epochs (1600–2500)
* Multiple planets
* Quadrant-sensitive configurations

---

# 9. Migration Strategy

Future replacement of VSOP87A with another ephemeris (e.g. DE-based provider):

* Replace base state provider
* Leave correction pipeline untouched
* Re-run regression suite

This ensures architectural stability.

---

# 10. Conclusion

The Corrections Architecture v1 provides a scalable and scientifically transparent extension path from geometric J2000 states to fully apparent coordinates.

Current implementation corresponds to Level 0.

Higher levels can be activated incrementally without modifying the validated analytical core.

---

End of Corrections Architecture v1
