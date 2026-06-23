# M2.4.0 Recap

## Status

Version: 1.0  
Status: Frozen  
Milestone: M2.4.0  
Date: 2026-06-23

---

# Purpose

This document records the actual implementation scope accepted for M2.4.0.

It serves as a retrospective milestone freeze specification and provides a stable reference point for future M2.x work.

This document describes:

- implemented functionality
- accepted architectural decisions
- deferred scope
- follow-up milestones

---

# Scope

M2.4.0 introduces the State Machine infrastructure required for deterministic and reproducible simulation execution inside Astronometria.

Primary objective:

> Introduce canonical StateNode-based execution and deterministic replay without extending the physical model.

No new physics were introduced.

The milestone focused exclusively on state architecture and reproducibility.

---

# Acceptance Criteria

The following acceptance criteria were defined for M2.4.0:

- StateNodeType infrastructure available
- Canonical internal node naming
- StateTreeRegistry available
- Ordered path resolution implemented
- StateHash generation implemented
- DataHash generation implemented
- Deterministic replay possible
- Run == LastRun verification successful

---

# Implemented Functionality

## 1. StateNodeType Infrastructure

Implemented:

- canonical StateNodeType concept
- internal node classification
- deterministic node identity

Purpose:

- define computation states independently from implementation details
- provide stable identifiers for StateTree execution

Status:

✓ Accepted

---

## 2. Canonical Internal Node Naming

Implemented internal naming scheme:

```text
PHYS.<Level>.<Origin>.<Plane>.<Epoch>.<Output>
```

Examples:

```text
PHYS.L0.HELIO.ECL.J2000.VEC
PHYS.L0.GEO.ECL.J2000.VEC
```

Purpose:

- remove model-specific naming from node identity
- separate physics state from implementation

Status:

✓ Accepted

---

## 3. StateTreeRegistry

Implemented:

- registry-based node resolution
- deterministic node lookup
- explicit StateTree definition

Purpose:

- centralize path definitions
- eliminate implicit graph construction

Status:

✓ Accepted

---

## 4. Ordered Path Resolution

Implemented:

- deterministic StateTree traversal
- explicit node ordering
- reproducible execution path

Purpose:

- guarantee identical execution order
- support deterministic replay

Status:

✓ Accepted

---

## 5. StateHash

Implemented:

- canonical State hashing
- deterministic hash generation

Purpose:

- uniquely identify input-side state

Status:

✓ Accepted

---

## 6. DataHash

Implemented:

- canonical Data hashing
- deterministic hash generation

Purpose:

- uniquely identify output-side results

Status:

✓ Accepted

---

## 7. Run == LastRun Validation

Successfully verified.

Observed result:

- identical StateHashes
- identical DataHashes
- deterministic replay confirmed

Purpose:

- validate StateMachine concept
- validate reproducibility architecture

Status:

✓ Accepted

---

# Explicitly Deferred Scope

The following topics are intentionally excluded from M2.4.0.

They remain valid future work items but are not acceptance-relevant.

---

## GEO-EQU Branch

Status:

DEFERRED

Reason:

GroundTruth support is currently unavailable.

AstronoTruth does not yet provide matching GEO-EQU datasets suitable for scientific validation.

Introducing GEO-EQU at this stage would require:

- AstronoTruth modifications
- complete GroundTruth regeneration
- full revalidation cycle

This violates the current single-dimension development strategy.

Decision:

```text
GEO-EQU remains architecturally prepared
but is not scientifically activated.
```

Consequences:

- GEO-EQU node definitions may remain in specifications
- GEO-EQU is not part of M2.4 acceptance
- GEO-EQU must not be used as validation evidence

Future milestone:

TBD

Likely after completion of current Astronometria physics roadmap.

---

## PHYS Persistence

Status:

Deferred

Future milestone:

M2.4.1

---

## Frame Cleanup

Status:

Deferred

Future milestone:

M2.4.2

Topics:

- remove Frame.Type
- remove Frame.RefSystem
- eliminate redundant frame metadata

---

## TimeDomain Architecture

Status:

Deferred

Future milestone:

M2.4.5

Topics:

- TDB vs TT separation
- native model time domains
- canonical conversion architecture

---

# Architectural Decisions

## Decision 1

Internal node naming uses:

```text
PHYS.*
```

and not:

```text
VSOP87.*
```

Rationale:

Node identity describes physical state.

Node identity must not describe implementation.

---

## Decision 2

StateTree is deterministic.

Only one valid path exists from entry node to terminal node.

Rationale:

- reproducibility
- diagnosability
- simplified validation

---

## Decision 3

Reproducibility is validated through:

```text
StateHash
+
DataHash
```

Rationale:

Both state and result must remain stable.

---

## Decision 4

Scientific activation requires GroundTruth.

Architectural preparation alone is insufficient.

Rationale:

A branch is considered scientifically active only after successful validation against GroundTruth.

---

# Validation Summary

Validation completed:

✓ Build successful

✓ Test suite successful

✓ StateTree execution successful

✓ Run == LastRun successful

✓ Deterministic replay confirmed

No open blocking issues remain within accepted M2.4.0 scope.

---

# Follow-Up Milestones

## M2.4.1

PHYS persistence

---

## M2.4.2

Frame cleanup

---

## M2.4.5

TimeDomain architecture

---

## M2.4.9

Diagnostics cleanup and standardization


---

## M2.5

Light-Time (L1)

---

# Final Result

M2.4.0 successfully establishes the deterministic State Machine foundation of Astronometria.

The milestone delivers:

- canonical state representation
- deterministic execution
- deterministic replay
- reproducible hashing

without expanding the physical model.

This fulfills the M2.4.0 milestone objective.