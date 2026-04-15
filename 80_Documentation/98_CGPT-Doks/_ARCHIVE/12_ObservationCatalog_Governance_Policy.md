
# ObservationCatalog Governance Policy
*Rules for the Controlled Evolution of Astronometria Validation Scenarios*

## 1. Purpose

The ObservationCatalog is the central registry of validation scenarios in the AstronoSphere ecosystem.

Its purpose is to define **scientifically meaningful test scenarios** that ensure the correctness,
stability, and reproducibility of astronomical computations.

Because AstronoSphere allows discovery of new scenarios during validation,
a governance policy is required to prevent uncontrolled growth.

This document defines the rules for **adding, modifying, and maintaining scenarios**.

---

# 2. Guiding Principles

The ObservationCatalog follows five core principles:

1. **Scientific relevance**
2. **Minimal scenario set**
3. **Reproducibility**
4. **Scenario stability**
5. **Transparency**

The catalog should remain **compact, meaningful, and stable over time**.

---

# 3. Scenario Categories

Scenarios typically fall into the following categories:

### 3.1 Baseline Scenarios

These represent the standard validation cases.

Examples:

- heliocentric planetary positions
- geocentric positions
- standard time meshes

Baseline scenarios are expected to remain **stable for many years**.

---

### 3.2 Stress Scenarios

Stress scenarios test extreme parameter ranges.

Examples:

- very large epochs
- dense sampling meshes
- extreme orbital positions

These scenarios test **numerical robustness**.

---

### 3.3 Edge Case Scenarios

Edge cases arise from validation anomalies.

Examples:

- perihelion spikes
- numerical instabilities
- frame singularities

These scenarios often originate from the **Self‑Extending Scenario System**.

---

# 4. Scenario Lifecycle

Every scenario passes through the following lifecycle:

Candidate  
↓  
Scientific Review  
↓  
ObservationCatalog Entry  
↓  
Reference Data Generation  
↓  
Permanent Validation Scenario

Only **approved scenarios** enter the catalog.

---

# 5. Candidate Scenario Creation

Candidate scenarios may originate from:

- statistical analysis of validation results
- numerical instability detection
- model comparison studies
- manual discovery by developers
- scenario discovery tools

Candidate scenarios must include:

- description of the phenomenon
- expected scientific value
- minimal reproducible parameters

---

# 6. Scientific Review Process

Before acceptance into the ObservationCatalog, each scenario must be reviewed.

Review criteria include:

1. Does the scenario expose meaningful model behaviour?
2. Is the scenario already covered by an existing test?
3. Can the scenario be generalized?
4. Does it add long‑term value to validation?

If the answer to these questions is positive, the scenario may be accepted.

---

# 7. Scenario Generalization Rule

Whenever possible, a scenario should represent a **class of phenomena**, not a single event.

Example:

Instead of:

Mercury perihelion 1823

Prefer:

Mercury perihelion mesh scenario

Generalized scenarios reduce catalog growth and improve coverage.

---

# 8. Scenario Stability

Once a scenario is part of the ObservationCatalog:

- its **Core definition must not change**
- the **ScenarioID must remain stable**
- modifications require versioning

Factories may generate new datasets for the same scenario,
but the **physical scenario definition remains immutable**.

---

# 9. Scenario Redundancy Control

Before adding a new scenario, the catalog must be checked for overlap.

Questions to consider:

- Does another scenario already test this parameter range?
- Can an existing scenario be extended instead?

Redundant scenarios should not be added.

---

# 10. Scenario Complexity Limits

The ObservationCatalog should remain manageable.

Recommended guidelines:

- prioritize high‑value scenarios
- avoid minor variations
- prefer parameterized scenarios over duplicates

Quality of scenarios is more important than quantity.

---

# 11. Community Contributions

External developers may propose new scenarios.

Such proposals must include:

- scenario definition
- scientific motivation
- reference dataset proposal

All contributions must pass the **Scientific Review Process**.

---

# 12. Summary

The ObservationCatalog is the **scientific backbone** of the AstronoSphere validation framework.

Governance ensures that the catalog remains:

- scientifically meaningful
- compact and maintainable
- reproducible
- stable over time

By enforcing these rules, AstronoSphere avoids scenario explosion
while allowing the system to evolve through controlled discovery of new edge cases.
