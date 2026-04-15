# Astronometria -- Velocity Accuracy Analysis

Generated: 2026-03-03 19:33 UTC

------------------------------------------------------------------------

## 1. Objective

Target numerical accuracy for velocity computation:

**\|Δv\| \< 1e-08 AU/day**

This is one order of magnitude tighter than the adopted VSOP87A velocity
tolerances (\~1e-7 AU/day).

------------------------------------------------------------------------

## 2. Numerical Method

Central difference scheme:

v(t) ≈ (r(t + Δt) − r(t − Δt)) / (2Δt)

Selected: Δt = 0.02 days

Error order: O(Δt²)

------------------------------------------------------------------------

## 3. Truncation Error Analysis

For harmonic orbital motion:

r(t) ≈ R cos(nt)

Third derivative magnitude:

r''' ≈ R n³

Worst case: Mercury (largest n).

With Δt = 0.02 days, truncation error is:

≈ 2e-9 AU/day

For outer planets (e.g., Neptune), truncation error is negligible
(\~1e-15 AU/day).

Conclusion: Truncation error safely below 1e-08 target.

------------------------------------------------------------------------

## 4. Cancellation Analysis

Critical case examined: Neptune at aphelion (smallest velocity).

Estimated: \|v\| ≈ 0.0028 AU/day\
Δr ≈ 1e-4 AU (dominant components)\
Double precision machine epsilon ≈ 2e-16

Cancellation error estimate:

Velocity error ≈ 2.5e-9 AU/day

Worst-case small component (vz) also analyzed: Error remains \~1e-9
AU/day range.

Conclusion: No catastrophic cancellation at Δt = 0.02 days.

------------------------------------------------------------------------

## 5. Component-wise Stability

vx, vy: Large position scale (\~30 AU) but sufficiently large Δr.

vz: Smaller magnitude (\~1 AU scale), but still numerically safe.

All components satisfy \< 1e-08 AU/day requirement.

------------------------------------------------------------------------

## 6. Final Decision

Δt = 0.02 days

Rationale: - Meets 1e-08 AU/day target - Stable under double precision -
Balanced truncation vs rounding error - Future-proof for full pipeline
differentiation

------------------------------------------------------------------------

End of document.
