# TS-D Helio L0 -- Initial Statistical Evaluation

Generated: 2026-02-26 20:42 UTC

------------------------------------------------------------------------

## 1. Overview

This document summarizes the first statistical evaluation of TS-D
(Heliocentric, L0) across three mesh zones:

-   **Core** (1600--2400)
-   **Extended** (0--4000)
-   **Extreme** (-4000--8000)

Metrics evaluated per planet:

-   Mean radial deviation (Mean_dr)
-   Root Mean Square deviation (RMS_dr)
-   Maximum deviation (Max_dr)
-   95th percentile (P95_dr)

All values are in AU.

------------------------------------------------------------------------

# 2. Core Zone (1600--2400)

### Observations

-   Errors increase with heliocentric distance.
-   Outer planets dominate absolute deviation.
-   Neptune and Uranus are the limiting bodies.
-   RMS ≈ 1.1--1.3 × Mean → well-behaved distribution.
-   No numerical instability observed.

### Interpretation

Core zone deviations are consistent with known VSOP87A model residuals.
Tolerances scaled by factor 1.38 are physically justified.

------------------------------------------------------------------------

# 3. Extended Zone (0--4000)

### Observations

-   Outer planets remain nearly identical to Core statistics.
-   Jupiter, Saturn, Uranus, Neptune show almost no epoch degradation.
-   Inner planets show moderate increase in mean and RMS deviation.

### Interpretation

VSOP87A remains highly stable outside the classical 1600--2400 range.
Outer planet error appears model-limited rather than epoch-limited.

Extended tolerances require only minor scaling (\~1.5).

------------------------------------------------------------------------

# 4. Extreme Zone (-4000--8000)

### Observations

-   Outer planets remain remarkably stable.
-   Neptune and Uranus show nearly identical statistics to Core.
-   Inner planets (Mercury, Venus, Earth) show clear deviation growth.
-   RMS significantly larger than Mean for inner planets → structured
    drift component.
-   No catastrophic divergence observed.

### Interpretation

Long-term degradation affects inner solar system more strongly. Outer
solar system error remains model-dominated and epoch-stable.

Extreme scaling (\~11×) is driven primarily by Mercury/Venus/Earth, not
by outer planets.

------------------------------------------------------------------------

# 5. Cross-Epoch Comparative Summary

  Planet Group    Core → Extended   Extended → Extreme
  --------------- ----------------- --------------------
  Outer Planets   Nearly constant   Nearly constant
  Inner Planets   Moderate growth   Strong growth

Key finding:

-   VSOP87A degradation is not uniform.
-   Outer planets show structural stability across epochs.
-   Inner planets exhibit time-dependent drift.

This strongly indicates model-term sensitivity rather than numerical
instability.

------------------------------------------------------------------------

# 6. Scientific Conclusions

1.  The VSOP implementation is numerically stable.
2.  No evidence of trigonometric or time-scale instability.
3.  Outer planet deviations are systematic and bounded.
4.  Inner planet deviations increase with epoch distance from J2000.
5.  The model behaves physically consistent across all three meshes.

------------------------------------------------------------------------

# 7. Recommended Next Steps

1.  Plot dr vs JD for Earth (Extreme) to visualize drift structure.
2.  Compare Core vs Extended graphically per planet.
3.  Add Geo pipeline evaluation (after correcting provider usage).
4.  Perform velocity deviation analysis once velocity model is active.
5.  Evaluate DE440/DE450 comparison using same statistical framework.

------------------------------------------------------------------------

End of report.
