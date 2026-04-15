# Scientific Validation Policy

AstronoSphere follows a strict validation policy to maintain scientific integrity.

---

## Core Rule

Never change both the engine and the reference data simultaneously.

One side must remain constant.

---

## Factory Changes

When a factory changes:

Astronometria must remain constant.

Workflow:

1. Factory generates new datasets
2. Compare Run vs LastRun
3. Execute Astronometria tests using new data
4. If tests remain valid, data may be promoted to ReferenceData

---

## Engine Changes

When Astronometria changes:

ReferenceData remains constant.

Workflow:

1. Run Astronometria tests
2. Compare results against ReferenceData
3. Investigate differences

Possible causes:

- bug
- numerical improvement
- improved physical model

ReferenceData is only updated after scientific validation.

---

## Promotion Rule

ReferenceData updates require:

- successful factory regression
- successful engine validation

Promotion is a manual step.