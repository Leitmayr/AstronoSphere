# Project Plan – Road to M1 (Ephemerides L0–L5, Mesh 1600–2500 Green)

Version: 1.0  
Date: 2026-03-03  
Project: Astronometria  

---

## 1. M1 Definition (Frozen Target)

M1 is achieved when:

- correction levels **L0–L5** are implemented (with L1 as the Light-Time time-iteration as frozen),
- and the **full mesh validation** for **1600–2500** is **green** for all planets (Mercury … Neptune),
- under the physically consistent tolerance model derived from the VSOP87A accuracy envelope.

This is the “Planetary Ephemeris Completion” milestone.

---

## 2. Strategy (How we get there)

- Develop **sequentially by level**: L0 → L1 → L2 → L3 → L4 → L5.
- After each level: run a **mini-mesh anchor check** at years:
  - 1600, 1800, 2000, 2200, 2500
- Only after L5: run the full mesh suite:
  - coarse (10y)
  - annual (1y)
  - boundary-dense (1600–1700, 2400–2500)

EphemerisRegressionGUI is developed **in lockstep**:
each new level must be selectable and deterministically generatable.

---

## 3. Work Packages and Gates

### WP0 – Hygiene & Baseline Policy (Gate 0)

Goal:
- Make “reproducible runs” operationally boring.

Deliverables:
- Linked baselines policy (Astronometria + EphemerisRegression):
  - include Git hash + dataset hash notes
- Output layout frozen (Raw + Json)
- Canonicalization version fixed

Exit criteria:
- rerun produces identical request hashes and identical file plans

---

### WP1 – L0 Consolidation and Mesh Infrastructure (Gate 1)

Goal:
- L0 is stable and mesh-capable end-to-end.

Deliverables:
- TS-A/TS-B/TS-C stable for L0
- TS-D/TS-E integrated as needed for transformation/time boundary checks
- Mini-mesh anchors for L0 archived

Exit criteria:
- L0 mini-mesh anchors stable
- no structural refactoring in core pipeline required

---

### WP2 – L1 Light-Time (Gate 2)

Astronometria:
- Implement L1 as fixed-point time iteration (convergence-based, time epsilon).
- Add invariants tests:
  - t_emit ≤ t_obs
  - deterministic convergence
  - max iteration safety behavior

EphemerisRegressionGUI:
- L1 selectable
- canonical preview and hash stable
- /L1/ output layout

Exit criteria:
- L1 mini-mesh anchors stable
- linked baseline created

---

### WP3 – L2 Aberration (Gate 3)

Astronometria:
- Implement Aberration as space-only stage after L1.
- Focused regression windows + anchors.

EphemerisRegressionGUI:
- L2 selectable
- mapping documented and versioned

Exit criteria:
- L2 mini-mesh anchors stable
- linked baseline created

---

### WP4 – L3 Relativistic Deflection (Gate 4)

Astronometria:
- Implement deflection stage.
- Include a deterministic, limited “near-Sun sensitivity” test set (not full brute-force).

EphemerisRegressionGUI:
- L3 selectable
- mapping documented and versioned

Exit criteria:
- L3 mini-mesh anchors stable
- linked baseline created

---

### WP5 – L4 Topocentric (Gate 5 – Critical Scope Gate)

Risk:
- L4 can explode scope if observer modeling is not bounded.

M1-bounded decision (normative proposal):
- For M1, define a **single fixed observer configuration** used everywhere:
  - Observer: Earth surface at (lat=0, lon=0, altitude=0) OR a similarly simple canonical location
  - Atmosphere parameters are fixed defaults (even if refraction is not yet applied until L5)

Why:
- makes datasets reproducible and tests deterministic
- avoids building a full “observer subsystem” before M1

Deliverables:
- observer definition frozen in a small config object with explicit versioning
- EphemerisRegressionGUI “Observer” mode enabled for L4 datasets

Exit criteria:
- L4 mini-mesh anchors stable (in observer mode)
- linked baseline created

---

### WP6 – L5 Refraction (Gate 6 – Second Scope Gate)

Risk:
- Refraction requires atmosphere model choices.

M1-bounded decision (normative proposal):
- Use a fixed “standard atmosphere” for refraction:
  - sea level
  - fixed pressure/temperature model
  - explicit documented formula and units
- Do not add weather, humidity, or dynamic conditions for M1.

Deliverables:
- L5 implemented with frozen assumptions
- EphemerisRegressionGUI can generate L5 reference data deterministically (observer mode)

Exit criteria:
- L5 mini-mesh anchors stable
- linked baseline created

---

### WP7 – Full Mesh Validation (M1 Gate)

Generate reference datasets (via GUI):
- Mesh coarse 10y (1600–2500)
- Mesh annual 1y (1600–2500)
- Mesh boundary-dense (1600–1700, 2400–2500)

Run Astronometria regression across the entire mesh:
- Helio and Geo where applicable
- Observer mode where required for L4/L5

Deliverables:
- archived mesh datasets + hashes
- deviation summaries (CSV/JSON)
- M1 completion note

Exit criteria:
- all mesh suites green under tolerance model
- M1 achieved

---

## 4. Documentation Deliverables (until M1)

Required documents (md) updated along the way:

- ArchitectureFreeze_EphemeridesPipelineLx.md (already frozen)
- LightTimeFundamentalConcept.md (already created)
- EphemerisRegressionGUI_M1_Spec.md (this spec)
- A short “Horizons Mapping per Level” note (versioned) whenever mapping is fixed/changed
- M1 Completion Note (with baseline hashes)

---

## 5. Practical Efficiency Rules

- No parallel refactoring during a level gate.
- Any change to time model or mapping requires explicit baseline reset.
- Keep GUI minimal: determinism > comfort.

---

End of document.
