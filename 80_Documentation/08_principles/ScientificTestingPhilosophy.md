# Scientific Testing Philosophy

Astronometria uses a scenario-driven validation approach.

Testing does not only verify code but also verifies
scientific correctness.

---

## Test Layers

Three levels of validation exist.

### Unit Tests

Verify individual components.

### Pipeline Tests

Validate transformations and algorithm stages.

### Scenario Tests

Validate complete astronomical situations.

---

## Ground Truth Validation

Astronometria results are compared against validated reference datasets.

This ensures physical correctness of the implementation.

---

## Goal

Testing must grow together with the system.

New scenarios strengthen the validation framework
instead of increasing complexity.