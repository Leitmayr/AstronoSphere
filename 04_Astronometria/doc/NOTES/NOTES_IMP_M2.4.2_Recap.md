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

# M2.4.x Addendum

## Purpose

This addendum records the implementation and validation work completed after the original M2.4.0 milestone freeze.

While M2.4.0 established the State Machine architecture, additional cleanup and persistence work was completed afterwards and scientifically validated before proceeding to the next milestone.

The purpose of this addendum is to document the final M2.4.x state accepted before continuing with the physics roadmap.

---

# M2.4.1 – PHYS Persistence

## Scope

The original M2.4.0 implementation introduced canonical internal node naming based on:

```text
PHYS.<Level>.<Origin>.<Plane>.<Epoch>.<Output>
```

However, persisted ScientificRun datasets still contained legacy node names derived from the simulation model.

M2.4.1 completed the migration by persisting canonical PHYS node identities directly into ScientificRun JSON files.

Example:

```text
Before:
VSOP87.L0.HELIO.ECL.J2000.TDB.VEC

After:
PHYS.L0.HELIO.ECL.J2000.VEC
```

---

## Validation

Validation was performed in two stages.

### Stage 1 – Single Dataset Validation

Dataset AS-000003 was used as a detailed verification target.

Verified:

* NodeType changed as expected
* StateHash changed as expected
* DataHash remained unchanged
* Simulation results remained unchanged

An independent SHA256 verification confirmed the generated StateHash.

### Stage 2 – Full ScientificRun Validation

All ScientificRun datasets were regenerated.

Rule-based Beyond Compare validation demonstrated:

```text
Run(M2.4.1)
vs
LastRun(M2.4.0)
```

No relevant differences remained after excluding:

* GitBranch
* GitCommit
* NodeType
* StateHash

Scientific results remained identical.

---

## Result

PHYS persistence was successfully completed.

The persisted ScientificRun representation is now aligned with the canonical internal StateTree architecture.

Status:

✓ Accepted

---

# M2.4.2 – Frame Cleanup

## Scope

The original ScientificRun frame representation still contained redundant information:

```json
{
  "Origin": "HELIO",
  "Plane": "ECL",
  "Type": "HelioEcliptic",
  "Epoch": "J2000",
  "RefSystem": "J2000"
}
```

The fields:

```text
Type
RefSystem
```

were fully derivable from the remaining frame definition and therefore violated the KISS principle.

M2.4.2 removed these redundant fields.

Final structure:

```json
{
  "Origin": "HELIO",
  "Plane": "ECL",
  "Epoch": "J2000"
}
```

---

## Validation

Validation followed the same strategy used for M2.4.1.

### Stage 1 – Single Dataset Validation

Dataset AS-000003 was inspected manually.

Verified:

* Frame.Type removed
* Frame.RefSystem removed
* StateHash changed
* DataHash unchanged
* Scientific data unchanged

### Stage 2 – Full ScientificRun Validation

All 235 ScientificRun datasets were regenerated.

Rule-based Beyond Compare validation demonstrated:

```text
Run(M2.4.2)
vs
LastRun(M2.4.1)
```

No relevant differences remained after excluding:

* GitBranch
* GitCommit
* StateHash
* Frame.Type
* Frame.RefSystem

### Stage 3 – Deterministic Replay

A second complete M2.4.2 run was executed.

Results:

* ScientificRun JSON files were binary identical
* Diagnostic files were binary identical

This confirmed deterministic replay after the cleanup.

---

## Result

Frame cleanup was successfully completed.

The ScientificRun representation now contains only canonical frame semantics.

Status:

✓ Accepted

---

# Key Learnings

## One-Dimension Rule Successfully Defended

During M2.4.x, an attempt was made to activate the GEO-EQU branch.

Investigation immediately revealed that suitable GroundTruth data did not yet exist.

Activating GEO-EQU would have required:

* AstronoTruth modifications
* GroundTruth regeneration
* full scientific revalidation

This would have opened a second development frontier while M2.4 was still in progress.

The decision was therefore made to defer GEO-EQU completely.

This became the first major practical validation of the One-Dimension Rule defined in the AstronoSphere Stealth Manifest.

The decision reduced scope, preserved validation integrity and prevented unnecessary rework.

---

## Diagnostics Paid Off

The missing GEO-EQU GroundTruth problem was detected within minutes.

The issue was identified directly through the diagnostic system rather than through debugger analysis.

This demonstrated the value of introducing diagnostics early and treating diagnostics as a first-class architectural component.

---

## Beyond Compare Validation Framework

During M2.4.x a reusable validation framework based on Beyond Compare 5 was established.

Capabilities:

* automated Run/LastRun evaluation
* rule-based exception handling
* reusable validation sessions
* HTML report generation
* deterministic replay verification

The framework significantly reduces future validation effort and is expected to become a standard validation tool for future milestones.

---

# Final M2.4.x State

Accepted implementation status before entering M2.4.5:

```text
✓ StateNodeType infrastructure
✓ PHYS canonical node identities
✓ StateTreeRegistry
✓ Ordered path resolution
✓ StateHash
✓ DataHash
✓ Deterministic replay
✓ PHYS persistence
✓ Frame cleanup
✓ RefSystem removal
✓ Type removal
✓ Diagnostic infrastructure
✓ Automated BC5 validation framework

✗ GEO-EQU (deferred)
✗ TimeDomain architecture
✗ L1 LightTime
✗ L2 Aberration
```

M2.4.x therefore concludes with a deterministic, reproducible and scientifically validated State Machine foundation ready for future physics extensions.
