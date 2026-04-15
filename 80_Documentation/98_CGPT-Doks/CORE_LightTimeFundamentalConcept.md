# Light-Time Fundamental Concept (L1)

Version: 1.0  
Date: 2026-03-03  
Project: Astronometria / Ephemeris Pipeline

---

## 1. Purpose

This document defines the **Light‑Time correction concept** (Astronometria correction level **L1**) in a way that is:

- **Physically justified**
- **Architecturally explicit**
- **Deterministic and testable**
- Compatible with Astronometria’s strict **time-domain separation** (UTC vs TT)
- Compatible with long-range **mesh validation** (1600–2500)

Light‑Time is treated as a **time iteration problem**, not as a “vector tweak”.

Note: TT and TD are different terms expressing the same: Dynamic Terrestrial Time. TT is used as the internal time scale (TDB support future extension).

---

## 2. Definitions and Notation

### 2.1 Time scales

- **t_obs (TT)**: observation time (the time at which the observer receives the light), represented by `TtInstant`.
- **t_emit (TT)**: emission time (the time at which the light left the target body), represented by `TtInstant`.

> Astronometria’s Ephemeris (Astro Domain) operates in **TT** only.
> No UTC appears inside the ephemeris pipeline.

### 2.2 Geometric position function

Let:

- **r(t)** be the target body’s **geometric position vector** at time **t**, produced by the **Geometry pipeline** (L0) in the requested frame.

The geometry pipeline is the strictly ordered transformation chain:

- `VSOP → Origin → Plane → Epoch`

No physical “appearance” effects are part of r(t) at L0.

### 2.3 Light speed

- **c**: speed of light, expressed consistently with Astronometria internal units:
  - distance unit: **AU**
  - time unit: **day**
  - therefore: **c_AU_per_day**

---

## 3. Physical Background

When observing a planet, the observed direction/position at **t_obs** is not the planet’s geometric state at that same instant.

Instead, the light was emitted earlier, at:

\[
t_{emit} = t_{obs} - \tau
\]

where the light travel time is:

\[
\tau = \frac{\|r(t_{emit})\|}{c}
\]

Crucially, \(r\) depends on \(t_{emit}\), therefore the equation is **implicit**:

\[
t_{emit} = t_{obs} - \frac{\|r(t_{emit})\|}{c}
\]

This is a **fixed-point equation** in the unknown **t_emit**.

---

## 4. Architectural Decision

### 4.1 Why Light‑Time is NOT a normal “correction stage”

Many physical corrections (aberration, relativistic deflection, refraction) can be modeled as **spatial transforms** applied to a vector:

- input: position vector r
- output: corrected position vector r'

Light‑Time is different:

- It does **not** primarily transform space.
- It changes the **time of evaluation** of the geometric ephemeris.

Therefore, Light‑Time is modeled as a **time iteration step executed before** the spatial correction pipeline.



### 4.2 Resulting pipeline structure

For correction level >= L1:

1. Compute a geometric position at observation time (optional diagnostic).
2. Solve for **t_emit** via Light‑Time iteration.
3. Recompute the geometric position at **t_emit**.
4. Only then apply subsequent spatial corrections (L2+).

This preserves the orthogonal architecture:

- **Geometry pipeline**: frame/coordinate transforms
- **Light‑Time solver**: time back-propagation (emission time)
- **Spatial correction pipeline**: appearance effects (aberration, relativity, …)

---

## 5. L1 Mathematical Specification

### 5.1 Problem statement

Given:

- observation time \(t_{obs}\)
- geometric position function \(r(t)\)
- light speed \(c\)

Find \(t_{emit}\) such that:

\[
t_{emit} = t_{obs} - \frac{\|r(t_{emit})\|}{c}
\]

### 5.2 Fixed-point iteration

Initialize:

\[
t_0 = t_{obs}
\]

Iterate:

1. Compute \(r(t_i)\) using the **geometry pipeline only**.
2. Compute travel time:
   \[
   \tau_i = \frac{\|r(t_i)\|}{c}
   \]
3. Update:
   \[
   t_{i+1} = t_{obs} - \tau_i
   \]

Stop when:

\[
\left| t_{i+1} - t_i \right| < \varepsilon_t
\]

or when a safety limit is reached.

---

## 6. Convergence Parameters (Normative)

### 6.1 Time epsilon

Normative value:

\[
\varepsilon_t = 1\times 10^{-12} \text{ days}
\]

This corresponds to approximately:

- \(1\times 10^{-12} \times 86400 \approx 8.64\times 10^{-8}\) seconds
- **0.086 microseconds**

Rationale:

- Orders of magnitude below any VSOP87A model limitation
- Deterministic and stable across 1600–2500 mesh validation

### 6.2 Maximum iterations

Normative value:

- `maxIterations = 10`

Rationale:

- Safety limit for unexpected conditions
- Typical convergence expected within 2–3 iterations for planetary distances

If the limit is reached without convergence, the system must:

- return the latest estimate, and
- optionally flag the result (telemetry/logging) for debugging.



---

## 7. Unit Consistency

Astronometria uses:

- position: **AU**
- time: **day**
- velocity: **AU/day**

Therefore the speed of light constant used by the solver must be:

- **c_AU_per_day**

No implicit conversion must occur inside the solver.

---

## 8. Scope Boundaries

### 8.1 What L1 includes

- Light‑Time correction for the target vector relative to the chosen origin (e.g., Sun-centered or Earth-centered), as provided by `GeometricPosition(t)`.

### 8.2 What L1 explicitly excludes

- Aberration (L2)
- Relativistic deflection (later)
- Topocentric effects (later)
- Atmospheric refraction (later)
- Any changes to time scales (TT ↔ TDB) inside the ephemeris pipeline

---

## 9. Integration Contract

### 9.1 Required callable

The solver requires a pure function:

- `GeometricPosition(planet, frame, t: TtInstant) -> Vector3`

This function must be:

- deterministic
- side-effect free
- independent of correction levels

### 9.2 Pipeline contract (pseudocode)

For a given request `(planet, frame, correctionLevel, t_obs)`:

```
r_obs = GeometricPosition(planet, frame, t_obs)

if correctionLevel < L1:
    return r_obs

t_emit = SolveLightTime(t_obs, (t) => GeometricPosition(planet, frame, t))
r_emit = GeometricPosition(planet, frame, t_emit)

if correctionLevel == L1:
    return r_emit

return ApplySpatialCorrections(r_emit, planet, frame, correctionLevel, t_obs, t_emit)
```

Notes:

- Spatial corrections may require both `t_obs` and `t_emit` depending on their physical definition.
- L1 returns the geometric vector at emission time.

---

## 10. Velocity Implications

Astronometria’s velocity provider differentiates the **full position provider** output:

\[
v(t) \approx \frac{r(t+\Delta) - r(t-\Delta)}{2\Delta}
\]

Because Light‑Time is inside the position computation path for L1+:

- velocity automatically includes Light‑Time behavior,
- without duplicating any correction logic in the velocity provider.

This is a major architectural advantage.

---

## 11. Testing and Validation Strategy (L1)

### 11.1 Deterministic behavior

The solver must be deterministic:

- same inputs -> same number of iterations (typically) -> same output
- no randomization
- no dependence on wall-clock time

### 11.2 Regression tests

Recommended tests:

1. **Convergence sanity**
   - Ensure convergence within a small iteration count for representative planets.
2. **Round-trip monotonicity**
   - Verify that \(t_{emit} \le t_{obs}\) always.
3. **Mesh drift check**
   - Run the 1600–2500 mesh (coarse initially) and verify no systematic drift patterns emerge relative to reference data.
4. **Edge ranges**
   - Validate at the boundaries 1600 and 2500.

### 11.3 Reference generation implications (EphemerisRegression)

For L1 reference datasets:

- the generator must request Horizons data corresponding to the same physical interpretation:
  - either “apparent” vectors (with light-time) or geometric vectors at emission time, depending on Horizons configuration.
- the chosen Horizons parameters must be canonicalized and stored with request hash for reproducibility.

### 11.4 Edge Case Scenarios

#### 11.4.1 maxDistance

- in heliocentric coordinates: Neptune in Aphel has the highest distance from the sun and will be an edge case for the LightTime influence. 
- in geocentric coordinates: Neptune, while in "conjunction" with the sun is on the opposite side of the sun with the biggest spacial distance to the earth.

#### 11.4.2 maxIterations

- Mercury in perihel has the highest velocity and will be an edge case for maxIterations. This edge case should be considered in the validation strategy of L1 implementation.
- Mercury in superior conjunction (biggest distance from earth) and close to Perihel might be an even a bigger edge case.

#### 11.3.3 Numerical Stability

- RAWrap (23h59m -> 00h01m longitude) should be an edge case considered in the validation strategy of L1 implementation: while the light travels from Neptune to the Sun or the Earth, Neptune crosses 00h00m longitude. That means, the algorithm must calculated back over the WrapPoint at 00h00m. 
- A similar effect arises at the node points: during the light time, a change in algebraic sign from - -> + or + -> - occurs. The algo must handle this

---

## 12. Rationale Summary

- Light‑Time solves an implicit equation in **time**.
- Treating it as a spatial correction hides critical structure.
- Modeling it as an explicit time solver:
  - improves correctness,
  - simplifies debugging,
  - enables future higher-accuracy models (e.g., DE series),
  - keeps the correction pipeline orthogonal and testable.

---

End of document.
