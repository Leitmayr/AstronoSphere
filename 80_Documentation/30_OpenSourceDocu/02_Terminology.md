# AstronoSphere – Terminology and Conceptual Model

Version: 1.0  
Purpose: Define the conceptual language of AstronoSphere

---

# 1. Purpose of this Document

AstronoSphere is not a typical software system.

It is a **scientific validation environment**.

To understand and work with the system, a precise terminology is required.

This document defines the core conceptual model.

---

# 2. AstronoSphere as a Laboratory

AstronoSphere can be understood as a **scientific laboratory**.

Inside this laboratory:

- experiments are defined
- measurements are taken
- theoretical models are evaluated
- results are verified

This perspective is essential for understanding the architecture.

---

# 3. Core Conceptual Mapping

The system is built around four fundamental roles:

```
Scenario   = Experiment
Factory    = Measurement
Model      = Simulation of reality
Analysis   = Verification
```

---

# 4. Definitions

## 4.1 Scenario (Experiment)

A Scenario defines a **physical experiment**.

It describes:

- time
- observer
- targets
- reference frame
- physical effects

A Scenario is:

- deterministic
- independent of implementation
- immutable after release

---

## 4.2 Factory (Measurement)

A Factory represents a **measurement instrument**.

It:

- queries external systems (e.g. JPL Horizons)
- retrieves real-world reference data
- produces deterministic datasets

A Factory does NOT:

- implement physics
- modify the experiment

---

## 4.3 Model (Simulation)

Astronometria represents the **simulation model**.

It:

- implements astronomical theory
- computes expected results
- operates fully deterministic

Important:

The Model is implemented as software (Engine),  
but conceptually it is a **theoretical representation of reality**.

---

## 4.4 Analysis (Verification)

The Analysis layer performs **verification**.

It:

- compares Model output with measured data
- evaluates deviations
- produces validation results

---

# 5. Data Concepts

## 5.1 Dataset

A Dataset is a **measurement of a Scenario**.

It is always tied to:

- a Scenario
- a data source (Factory)
- a specific configuration

---

## 5.2 Reference Data (RD)

Reference Data is the **ground truth measurement**.

It:

- originates from external scientific sources
- is stored deterministically
- defines the validation baseline

---

## 5.3 Engine Data (ED)

Engine Data is the **Model output**.

It:

- is computed by Astronometria
- mirrors the structure of Reference Data
- is compared during validation

---

## 5.4 Analysis Data (AD)

Analysis Data is the **result of validation**.

It:

- contains PASS / FAIL decisions
- may include statistical evaluation

---

# 6. Conceptual Separation

AstronoSphere enforces strict separation:

- Experiment (Scenario)
- Measurement (Factory / RD)
- Theory (Model / ED)
- Verification (Analysis / AD)

No component may violate this separation.

---

# 7. Mental Model

When working with AstronoSphere, think in the following way:

- You define an experiment (Scenario)
- You measure reality (Factory → RD)
- You simulate the same experiment (Model → ED)
- You verify the result (Analysis → AD)

---

# 8. Final Principle

AstronoSphere is not about computing positions.

It is about **proving that computed positions are correct**.

---

End of document.
