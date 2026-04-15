# TS-D Helio L0 -- Detailed Statistical Evaluation

Generated: 2026-02-26 20:44 UTC

------------------------------------------------------------------------

## 1. Scope

This document summarizes the quantitative evaluation of TS-D
(Heliocentric, L0) across three mesh zones:

-   Core (1600--2400)
-   Extended (0--4000)
-   Extreme (-4000--8000)

Metrics per planet (AU):

-   Mean radial deviation (Mean_dr)
-   RMS deviation (RMS_dr)
-   Maximum deviation (Max_dr)
-   95th percentile (P95_dr)

------------------------------------------------------------------------

# 2. Core Zone (1600--2400)

## Numerical Results

Earth\
Mean: 5.69E-07\
RMS: 6.30E-07\
Max: 1.28E-06\
P95: 1.14E-06

Jupiter\
Mean: 6.44E-06\
RMS: 7.64E-06\
Max: 1.84E-05\
P95: 1.46E-05

Neptune\
Mean: 4.09E-04\
RMS: 4.76E-04\
Max: 9.54E-04\
P95: 8.12E-04

Uranus\
Mean: 1.44E-04\
RMS: 1.79E-04\
Max: 5.20E-04\
P95: 3.29E-04

## Interpretation

-   Errors increase strongly with heliocentric radius.
-   Outer planets (Uranus, Neptune) define the tolerance ceiling.
-   RMS ≈ 1.15× Mean → stable distribution without spikes.
-   Core tolerances required scaling factor ≈ 1.38 (driven by Neptune
    Max).

------------------------------------------------------------------------

# 3. Extended Zone (0--4000)

## Numerical Results

Earth\
Mean: 1.36E-06\
RMS: 1.59E-06\
Max: 3.90E-06

Jupiter\
Mean: 6.44E-06\
RMS: 7.63E-06\
Max: 1.81E-05

Neptune\
Mean: 4.10E-04\
RMS: 4.77E-04\
Max: 9.53E-04

Uranus\
Mean: 1.44E-04\
RMS: 1.79E-04\
Max: 5.19E-04

## Interpretation

-   Outer planets remain nearly identical to Core statistics.
-   Neptune Mean difference (Core → Extended): +0.00000016 AU
    (negligible).
-   Jupiter virtually unchanged.
-   Inner planets show moderate increase (Earth Mean \~2.4× Core).
-   Extended tolerances required only minor scaling (\~1.5×).

Conclusion: VSOP87A degradation outside 1600--2400 is minimal for outer
planets.

------------------------------------------------------------------------

# 4. Extreme Zone (-4000--8000)

## Numerical Results

Earth\
Mean: 5.69E-06\
RMS: 8.89E-06\
Max: 3.66E-05\
P95: 1.99E-05

Mercury\
Mean: 6.83E-06\
RMS: 9.38E-06\
Max: 3.24E-05

Neptune\
Mean: 4.10E-04\
RMS: 4.77E-04\
Max: 9.52E-04

Uranus\
Mean: 1.44E-04\
RMS: 1.80E-04\
Max: 5.19E-04

## Interpretation

-   Outer planets remain essentially constant across all epochs.
-   Neptune Mean difference (Core → Extreme): \~0.00000065 AU
    (insignificant).
-   Inner planets exhibit clear growth with epoch distance.
-   Earth Mean increases by factor ≈ 10 relative to Core.
-   RMS significantly larger than Mean for inner planets → structured
    drift component.
-   Extreme tolerance scaling (\~11×) is driven by Mercury/Venus/Earth.

No catastrophic divergence observed.

------------------------------------------------------------------------

# 5. Cross-Epoch Comparative Summary

Outer planets (Jupiter--Neptune):

-   Stable mean and RMS across all meshes.
-   Error dominated by model limitations, not epoch distance.
-   No long-term instability detected.

Inner planets (Mercury--Earth):

-   Progressive growth from Core → Extended → Extreme.
-   Drift component becomes visible in RMS increase.
-   Behavior consistent with long-term perturbation sensitivity.

------------------------------------------------------------------------

# 6. Global Conclusion

1.  Implementation is numerically stable.
2.  No indication of time-scale instability.
3.  Outer solar system error is epoch-invariant.
4.  Inner solar system exhibits time-dependent degradation.
5.  VSOP87A remains robust even in extreme epoch testing.
6.  Statistical scaling factors (1.38 \| 1.5 \| 11.0) are physically
    justified.

------------------------------------------------------------------------

# 7. Recommended Next Steps

1.  Plot dr vs JD for Earth (Extreme) to visualize drift structure.
2.  Compare Core vs Extended numerically per planet in tabular form.
3.  Perform Geo pipeline correction and repeat statistical evaluation.
4.  Extend analysis to velocity once implemented.
5.  Benchmark against DE440/DE450 using identical framework.

------------------------------------------------------------------------

End of detailed report.
