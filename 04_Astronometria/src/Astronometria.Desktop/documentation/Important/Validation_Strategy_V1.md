
Horizons is treated as external scientific ground truth.

The regression project:

- Does not compute physics  
- Only retrieves and serializes Horizons data  
- Provides deterministic reference datasets  

---

# 4. Validation Layers

Validation is multi-dimensional.

---

## 4.1 TS-A – Quadrant Events

Validates:

- Sign transitions of X/Y  
- Heliocentric geometry  
- Deterministic event detection  

Purpose:  
Geometric integrity of heliocentric motion.

---

## 4.2 TS-B – Node Crossings

Validates:

- Z sign transitions  
- Ecliptic plane crossings  
- Stability in ±1 day window  

Purpose:  
Spatial orientation integrity.

---

## 4.3 TS-C – Distance Extrema

Validates:

- Norm computation  
- Earth subtraction correctness  
- Scalar distance behavior  

Purpose:  
Vector magnitude correctness and subtraction stability.

---

## 4.4 TS-D – RA/DEC Validation

Validates:

- Ecliptic → Equatorial transformation  
- Obliquity handling  
- Coordinate conversion consistency  

Purpose:  
Transformation chain verification.

---

## 4.5 TS-E – Epoch Boundary Validation

Validates:

- Behavior at 1600 and 2500  
- ΔT model limits  
- Long-term drift detection  

Purpose:  
Time model robustness.

---

# 5. Mesh Validation (1600–2500)

To detect systematic drift and frame errors, a temporal mesh is introduced.

## 5.1 Coarse Mesh

Step size: 10 years  
Range: 1600–2500  

Evaluated quantities:

- Helio X,Y,Z  
- Geo X,Y,Z  
- Distance Δr  
- RA / DEC  

Purpose:  
Detect large-scale trends and epoch edge behavior.

---

## 5.2 Annual Mesh

Step size: 1 year  

Purpose:

- Detect periodic errors  
- Detect frame rotation drift  
- Detect transformation inconsistencies  

---

## 5.3 Adaptive Boundary Mesh

Higher density sampling near:

- 1600–1700  
- 2400–2500  

Purpose:  
ΔT and model boundary sensitivity.

---

# 6. Visualization Strategy

Validation data is exported as structured JSON.

Example:

```json
{
  "Planet": "Mars",
  "MeshStepYears": 1,
  "Results": [
    {
      "JD": 2451545.0,
      "DeltaR_AU": 0.0000023,
      "DeltaRA_arcsec": 0.45,
      "DeltaDEC_arcsec": -0.32
    }
  ]
}
