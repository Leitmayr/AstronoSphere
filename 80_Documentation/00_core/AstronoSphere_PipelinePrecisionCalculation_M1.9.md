# AstronoSphere M1.9 – Precision Analysis (Pipeline & Horizons)

## 1. Overview

This document summarizes the precision behavior across the full AstronoSphere M1.9 pipeline:

Seed → AstronoLab → AstronoCert → AstronoTruth → Horizons → GroundTruth JSON


The goal is to identify:
- where precision is preserved
- where deviations occur
- how large these deviations are physically

---

## 2. Precision Along the Pipeline

### 2.1 Seed Incoming

The Julian Date (JD) is defined in the seed as a decimal value.

Observation:
- No modification occurs at this stage.


StartJD(Seed Incoming) = reference value


---

### 2.2 AstronoLab → Seed Prepared

AstronoLab processes the seed but does not modify the JD.

StartJD(Seed Prepared) == StartJD(Seed Incoming)


✔ Precision preserved

---

### 2.3 AstronoCert → Experiment Released

AstronoCert also preserves the JD exactly.

StartJD(Experiment) == StartJD(Seed Prepared)


✔ Precision preserved

---

### 2.4 AstronoTruth → Request Builder

AstronoTruth reads the JD using raw JSON text (`GetRawText()`).

Example:

StartJD (raw): 2463504.088194444
START_TIME = JD2463504.088194444

StartJD(Request) == StartJD(Experiment)


✔ No precision loss before API call

---

## 3. Horizons API Behavior

Horizons accepts Julian Dates directly.

Observed behavior:
- In most cases: JD is returned unchanged
- In some cases: small deviation

Typical deviation:

ΔJD ≈ 1e-9 days

Examples:

Input: 2463504.088194444
CSV : 2463504.088194443
ΔJD : -1e-9 d


✔ This deviation originates from Horizons (provider-level)

---

## 4. CSV Parsing Effects

The CSV parser reads JD into `double`.

Example:

2443872.704000000 → 2443872.7039999999


Cause:
- IEEE-754 floating-point representation

✔ This is NOT a Horizons issue  
✔ This is a binary floating-point artifact

---

## 5. GroundTruth JSON Serialization

### 5.1 Previous Behavior (Incorrect)

JD was written from `double`.

Result:

JD(JSON) ≠ JD(Horizons)


Effect:
- State vector corresponds to a different time than the stored JD

❌ Physically inconsistent

---

### 5.2 Current Behavior (Fixed)

JD is written using the original CSV string (`JulianDateRaw`).


JD(JSON) == JD(CSV)


✔ Physically consistent:

JD ↔ StateVector


---

## 6. Summary of Precision Sources

| Stage                         | Precision Impact            |
|------------------------------|----------------------------|
| Seed → Experiment            | None                       |
| Request Builder              | None                       |
| Horizons API                 | up to ~1e-9 days           |
| CSV Parsing (double)         | binary rounding artifact   |
| JSON Serialization (fixed)   | none (raw string used)     |

---

## 7. Time Deviation Analysis

Maximum observed deviation:

ΔJD = 1e-9 days


Convert to seconds:

Δt = 1e-9 × 86400 s = 8.64e-5 s
Δt = 0.0000864 s = 86.4 µs


---

## 8. Spatial Deviation Estimation

Reference case: Mercury at perihelion

Approximate velocity:

v ≈ 59 km/s


Distance error:

Δs = v × Δt
Δs = 59 km/s × 0.0000864 s
Δs ≈ 0.0051 km
Δs ≈ 5.1 m


---

## 9. Interpretation

Key findings:

- The pipeline preserves precision up to the API boundary
- Horizons introduces small, deterministic deviations (~1e-9 d)
- Previous JSON writing introduced additional artificial error
- This error is now eliminated

Final state:


✔ Pipeline precision = provider-limited
✔ Maximum spatial deviation ≈ 5 meters
✔ JD ↔ StateVector consistency restored


---

## 10. Impact on Validation Strategy

### Old approach:

Run == LastRun (byte-level)


### New reality:

Run ≠ LastRun (due to corrected JD handling)


### Updated validation approach:

1. **Input consistency**

ExperimentJD == RequestJD


2. **Provider deviation**

RequestJD vs CSV JD


3. **Truth consistency**

CSV JD == JSON JD


4. **Physical correctness**

JD(JSON) ↔ StateVector(JSON)


---

## 11. Final Conclusion

The system now meets the intended scientific standard:


✔ Deterministic pipeline
✔ Maximum achievable precision (given Horizons)
✔ Physically consistent GroundTruth data


The limiting factor is no longer the pipeline, but the external provider (Horizons).