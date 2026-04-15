# AstronoSphere – System Overview (v2)

Version: 2.0  
Purpose: Precise architectural map based on the canonical pipeline

---

# 1. Core Architecture

AstronoSphere is a closed validation loop built from concrete components.

```
+---------------------+        +---------------------+        +---------------------+
| Meeus Scenario      |        | Scenario Header     |        | Observation         |
| Factory (MSF)       | -----> | Generator (SHG)     | -----> | Catalog (OC)        |
| (Candidates)        |        | (ID, Hash, Persist) |        | (Scenarios)         |
+---------------------+        +---------------------+        +---------------------+
                                                                  |
                                                                  v
                                                        +---------------------+
                                                        | Truth Factory (TF)  |
                                                        | (External Data)     |
                                                        +---------------------+
                                                                  |
                                                                  v
                                                        +---------------------+
                                                        | Reference Data (RD) |
                                                        | (JSON / CSV)        |
                                                        +---------------------+
                                                                  |
                                                                  v
+---------------------+                                   +---------------------+
| Astronometria (AST)| <-------------------------------- | Engine Input        |
| (Computation Engine)|                                   | (Scenario + RD)     |
+---------------------+                                   +---------------------+
           |
           v
+---------------------+
| Engine Data (ED)    |
| (Computed Results)  |
+---------------------+
           |
           v
+---------------------+
| Analysis Tool (AT)  |
| (Comparison)        |
+---------------------+
           |
           v
+---------------------+
| Analysis Data (AD)  |
| (Validation Output) |
+---------------------+
```

---

# 2. Component Responsibilities

## MSF – Meeus Scenario Factory
- generates ScenarioCandidates
- provides approximate event seeds
- contains NO header logic

---

## SHG – Scenario Header Generator
- generates ScenarioID
- assigns CatalogNumber
- computes CoreHash
- persists to ObservationCatalog

CONSTRAINT:
- single authority for scenario creation
- must be deterministic

---

## OC – Observation Catalog
- stores released scenarios
- immutable Scenario Core
- central registry of experiments

---

## TF – Truth Factory
- reads scenarios from OC
- queries external systems (e.g. Horizons)
- produces deterministic datasets

CONSTRAINT:
- no internal physics
- no modification of Scenario Core

---

## RD – Reference Data
- ground truth dataset
- canonical JSON + raw CSV
- baseline for validation

---

## AST – Astronometria
- computes astronomical results
- uses Scenario + RD as input
- no dependency on factory logic

---

## ED – Engine Data
- computed results from Astronometria
- same structure as RD

---

## AT – Analysis Tool
- compares ED vs RD
- performs validation

---

## AD – Analysis Data
- validation results
- PASS / FAIL + statistics

---

# 3. Validation Loop

```
Scenario (OC)
→ TruthFactory → RD
→ Astronometria → ED
→ AnalysisTool → AD
```

This loop is:

- deterministic
- reproducible
- scientifically traceable

---

# 4. Key Architectural Rules

- Scenario Core is immutable
- SHG is the only writer to OC
- Factories do not implement physics
- Engine does not access external systems
- No shared logic between Engine and Factories

---

# 5. Mental Model

Scenario = Experiment  
Factory = Measurement  
Engine = Theory  
Analysis = Verification  

---

# 6. Developer Orientation

To understand the system:

1. Start with a Scenario in OC
2. Follow it through TF → RD
3. Follow RD into Astronometria → ED
4. Observe validation in AT → AD

---

End of document.
