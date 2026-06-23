# SPEC_M2.4__MeasurementStateArchitecture_V1.0.md

## Status

Version: V1.0  
Status: Draft for Review  
Scope: AstronoSphere / Astronometria / AstronoMeasurement / AstronoData.Contracts / Measurement Semantics / State Architecture
Milestone: M2.4


## Change log 

| Revision | Changes | Date |
| -------- | ------- | ---- |
| V1.0 | intitial revision before implementation of M2.4  | 2026-05-17
| V1.1 | Scope added, Section 3: Layer Model - GT added , Adapted wording in  the table in Section 5.1 to the same wording as Core-Spec | 2026-05-17
|  | inserted new Section #13: Simplicity first| 2026-05-17  | inserted new Section #13: Simplicity firs | 2026-05-17
|  | inserted Section 10.1 with AstronoSphere Standard TimeScale and added note in new section 10.3: Simplicity first| 2026-05-17  | inserted new Section #13: Simplicity firs | 2026-05-17
|  | added TDB TimeScale Branches in "Future Extension" (new Section 15) and added AstronoTruth in the part "In Short" right at the end of the document | 2026-05-17  | inserted new Section #13: Simplicity first | 2026-05-17
|  | added sections 8.4 and 8.5: NodeType definitions and StateTreeRegistry  | 2026-05-17
|  | inserted new sections #14.: Diagnostics | 2026-05-17
|  | Section 4.5: folder structure unchanged | 2026-05-17

## Scope

The focus of this document is on Milestone 2.4, which is implementing the Astronometria PhysicsStateTree for L0. AstronoMeasurement has a tight coupling to GroundTruth providers, but will contain full GroundTruth specification only in M2.6.8. In terms of GroundTruth, this document might be incomplete.
Once the GroundTruth part has fully been specified, the document will likely be promoted to a Core document.
This fact emphasizes the importance of this spec.

> **Note: further detail can be explored in the corresponding Core Specification CORE_MeasurementDefinition_V1.2.md.**

---

# 1. Purpose

This specification defines the canonical architecture separation between:

- AstronoMeasurement
- AstronoData.Contracts
- Astronometria

for the M2.4 milestone.

The objective is to establish a deterministic and scientifically reproducible state architecture for Astronometria while preserving strict semantic orthogonality across the AstronoSphere ecosystem.

This specification clarifies:

- what belongs to canonical measurement semantics
- what belongs to deterministic contracts and hashing
- what belongs to executable simulation logic

This separation is mandatory to avoid architectural coupling between:

- measurement semantics
- provider mappings
- simulation execution
- StateTree realization
- persistence contracts

---

# 2. Motivation

M2.4 introduces the StateMachine principle into Astronometria.

At the same time, M2.4 introduces canonical `MeasurementDefinition` semantics.

Without strict architectural separation, the following anti-patterns would emerge:

- provider semantics leaking into StateTree execution
- simulation logic leaking into canonical measurement semantics
- hashing logic leaking into simulation engines
- engine-specific node sequences becoming part of canonical semantics
- GroundTruth mappings becoming coupled to simulation implementation details

The architecture therefore separates:

```text
Measurement semantics
≠
Simulation execution
≠
GroundTruth interaction
≠
Persistence contracts
````


This separation is essential for:

* deterministic reproducibility
* provider-neutral semantics
* engine-neutral semantics
* future multi-provider validation
* future multi-engine comparison
* long-term maintainability

---

# 3. Architectural Overview

## 3.1 Layer Model

````text
AstronoMeasurement
    ↓ semantic mapping

Astronometria                       AstronoTruth
    ↓ execution results                     ↓ GT request return

AstronoData.Contracts
    ↓ persistence + canonicalization

AstronoData
````

---

## 3.2 Core Principle

The architecture separates:

````text
WHAT is measured
````

from:

````text
HOW it is computed
````

and from:

````text
HOW it is persisted and hashed
````

---

# 4. Responsibility Separation

## 4.1 AstronoMeasurement

AstronoMeasurement owns:

````text
canonical measurement semantics
````

AstronoMeasurement defines:

* MeasurementDefinition
* semantic axes
* semantic meaning of measurement branches
* provider-neutral semantics
* engine-neutral semantics

AstronoMeasurement does NOT define:

* StateTree execution
* Transition ordering
* node sequences
* provider API requests
* simulation algorithms
* persistence logic
* canonical hashing rules

---

## 4.2 AstronoData.Contracts

AstronoData.Contracts owns:

````text
canonical persistence semantics
````

AstronoData.Contracts defines:

* POCO contracts (POCO: simple class with loose coupling)
* canonical serialization
* canonicalization rules
* deterministic hashing rules
* persistence contracts
* SimulationData structures
* StateHash/DataHash generation rules

AstronoData.Contracts does NOT define:

* physics
* provider mappings
* simulation execution
* StateTree execution logic
* measurement semantics

---

## 4.3 Astronometria

Astronometria owns:

````text
executable simulation realization
````

Astronometria defines:

* StateTree execution
* Transition algorithms
* SimulationRun execution
* TerminalNode resolution
* SimulationMapping realization
* physics implementation
* Scene execution
* numerical algorithms
* deterministic node ordering

Astronometria does NOT define:

* canonical measurement semantics
* canonical hashing rules
* provider-neutral semantics

##  4.4 AstronoTruth

AstronoTruth mapping is out of scope for M2.4 and will be specified in M2.6.8.

## 4.5 Folder Structure for Output

The input and output folders for ScientificRun simulations as well as the file names of the data persisted shall not change.

Still the same rules apply: 
1) TerminalNode data are persisted
2) IntermediateNode data are not persisted


---

# 5. Canonical Separation Principle

## 5.1 Core Rule

The following concepts are strictly orthogonal:

| Layer                 | Responsibility          |
| --------------------- | ----------------------- |
| Experiment            | Defines what physical experiment is performed     |
| MeasurementDefinition | Defines how experiment is measured   |
| SimulationModel       | Defines how the simulation evaluates the model         |
| StateTree             | execution path          |
| Contracts             | persistence and hashing |

---

## 5.2 Important Non-Equivalences

The following concepts are explicitly NOT identical:

````text
MeasurementDefinition
≠
StateTreePath
````

````text
MeasurementDefinition
≠
SimulationModel
````

````text
MeasurementDefinition
≠
GroundTruthRequest
````

````text
SimulationModel
≠
Measurement semantics
````

````text
StateTree
≠
canonical semantics
````

---

# 6. MeasurementDefinition Semantics

## 6.1 Purpose

`MeasurementDefinition` defines:

````text
HOW an astronomical experiment shall be measured
````

It does not define:

* how the result is numerically computed
* how the result is requested from a provider
* how the result is persisted
* which node sequence shall be executed

---

## 6.2 M2.4 Supported Measurement Branches

M2.4 supports the following canonical measurement branches:

### HELIO-ECL

````text
Origin = HELIO
Plane  = ECL
Epoch  = J2000
TimeScale = TDB
````

### GEO-ECL

````text
Origin = GEO
Plane  = ECL
Epoch  = J2000
TimeScale = TDB
````

### GEO-EQU

````text
Origin = GEO
Plane  = EQU
Epoch  = J2000
TimeScale = TDB
````

---

## 6.3 M2.4 Scope Restrictions

M2.4 supports only:

* Domain = Ephemeris
* MeasurementType = VEC
* CorrectionLevel = L0
* Epoch = J2000
* TimeScale = TDB

Deferred:

* RA/DEC
* AZ/ALT
* TOPO
* OFDATE
* dynamic StateTree selection
* observer projection layers

---

# 7. Simulation Mapping

## 7.1 Purpose

Astronometria realizes a `MeasurementDefinition` by means of a deterministic executable StateTree.

This realization is called:

````text
SimulationMapping
````

SimulationMapping is engine-specific.

---

## 7.2 Core Principle

The same `MeasurementDefinition` may map to:

* Horizons
* Miriade
* Astronometria
* future engines

while preserving identical semantic meaning.

---

## 7.3 Example

### MeasurementDefinition

````text
GEO-EQU / L0 / VEC / J2000 / TDB
````

### Astronometria SimulationMapping

````text
PHYS HELIO-ECL
    ↓ Origin Transform
PHYS GEO-ECL
    ↓ Plane Transform
PHYS GEO-EQU
````

The StateTree realization belongs exclusively to Astronometria.

It must not become part of the canonical MeasurementDefinition.

## 7.4 TerminalNode Resolution Rule
MeasurementDefinition
→ SimulationMapping
→ exactly one TerminalNodeType

and:

TerminalNodeType
→ exactly one PhysicsStateTreePath

This rule guarantees:

deterministic execution
deterministic replay
deterministic hashing
SinglePath semantics

The runtime system must therefore never dynamically choose between multiple execution paths for the same MeasurementDefinition and SimulationModel combination.

## 7.5 PhysicsStateTree Mapping Table (M2.4)

| MeasurementDefinition	| TerminalNodeType | 	PhysicsStateTreePath	|  Horizons Validation Semantics | 
| ------| -----| ------| ------------| 
| HELIO/ECL/J2000/L0/VEC	| PHYS.L0.HELIO.ECL.J2000.VEC	| [HELIO-ECL]	| Heliocentric Ecliptic J2000 Geometric Vector| 
| GEO/ECL/J2000/L0/VEC	| PHYS.L0.GEO.ECL.J2000.VEC | 	[HELIO-ECL, GEO-ECL]	| Geocentric Ecliptic J2000 Geometric Vector
| GEO/EQU/J2000/L0/VEC | 	PHYS.L0.GEO.EQU.J2000.VEC| 	[HELIO-ECL, GEO-ECL, GEO-EQU]| 	Geocentric Equatorial J2000 Geometric Vector

> The PhysicsStateTreePath is not persisted in M2.4.
The path is deterministically derived from TerminalNodeType.

---

# 8. StateTree Semantics

## 8.1 Purpose

The StateTree defines:

````text
HOW the engine reaches the terminal result
````

The StateTree is executable architecture.

It is not semantic measurement definition.

---

## 8.2 M2.4 Directed StateTree Rule

The M2.4 StateTree is a directed tree:

````text
exactly one path exists
from root to terminal node
````

This ensures:

* deterministic replay
* deterministic hashing
* reproducible execution
* stable validation

---

## 8.3 TerminalNode Principle

A TerminalNode defines:

````text
the selected output state
of the SimulationRun
````

A TerminalNode is execution-specific.

It is not part of canonical measurement semantics.

## 8.4 NodeType Naming

NamingRule of the PhysicsStateTree NodeTypes


Examples:
````text
PHYS.L0.HELIO.ECL.J2000.VEC
PHYS.L0.GEO.ECL.J2000.VEC
PHYS.L0.GEO.EQU.J2000.VEC
````

> Note: PHYS is the state tree domain which can execute different Simulations models, such as but not limited to
- VSOP87A full
- VSOP87A Meeus truncation
- DE440
- INPOP

Later (>M2.4) ObserverStateTree NodeTypes can be added in a second, independent StateTree:

Examples:
````text
OBS.L0.HELIO.ECL.J2000.VEC
OBS.L0.GEO.ECL.J2000.VEC
OBS.L0.GEO.EQU.J2000.VEC
````
> Note: ignore OBS-StateTree Nodes for M2.4.


## 8.5 StateTree Registry

MeasurementDefinition
→ TerminalNodeType
→ OrderedNodePath[]
→ foreach node
    execute transition

Example:

PHYS.L0.GEO.EQU.J2000.VEC
→ [
  PHYS.L0.HELIO.ECL.J2000.VEC,
  PHYS.L0.GEO.ECL.J2000.VEC,
  PHYS.L0.GEO.EQU.J2000.VEC
]

The registry shall not be hard coded but be implemented as a data file in json format.

Example implementation:

````json
[
  {
    "TerminalNodeType": "PHYS.L0.HELIO.ECL.J2000.VEC",
    "OrderedNodePath": [
      "PHYS.L0.HELIO.ECL.J2000.VEC"
    ]
  },
  {
    "TerminalNodeType": "PHYS.L0.GEO.ECL.J2000.VEC",
    "OrderedNodePath": [
      "PHYS.L0.HELIO.ECL.J2000.VEC",
      "PHYS.L0.GEO.ECL.J2000.VEC"
    ]
  }
]
```` 
---

# 9. StateHash and DataHash Responsibilities

## 9.1 Astronometria Responsibility

Astronometria creates:

* State structures
* Data structures
* deterministic node order

---

## 9.2 Contracts Responsibility

AstronoData.Contracts performs:

* canonicalization
* serialization normalization
* deterministic hashing

using the canonical rules defined in HashSpec.

---

## 9.3 Hash Boundary Rule

Astronometria must never implement its own hashing rules.

All hashing semantics are centralized in AstronoData.Contracts.

---

# 10. Time Architecture Integration

AstronoSphere distinguishes multiple TimeScale layers.

## 10.1. General Time Domain Rule in AstronoSphere

AstronoSphere standard TimeScale is TDB. The Julian Dates of all Seeds and Experiments are TDB.
However, depending on the instance, the GroundTruth provider or the SimulationModel might deviate from this rule. Hence, TimeScale can differ from the global standard TimeScale TDB.

## 10.2 MeasurementDefinition.TimeScale

`MeasurementDefinition.TimeScale` defines:

````text
measurement semantics
````

Example:

````text
TDB
````

---

## 10.3 SimulationModel.TimeScale

`SimulationModel.TimeScale` defines:

````text
native engine evaluation domain
````

Example:

````text
TT
````

> Note: VSOP runs in TT, for example. DE440 is TDB. Both TimeScale settings are supported.


---

## 10.4 Important Separation

The following concepts are NOT identical:

````text
MeasurementDefinition.TimeScale
≠
SimulationModel.TimeScale
````

Example:

````json
MeasurementDefinition.TimeScale = TDB
SimulationModel.TimeScale = TT
````

This means:

````text
measurement semantics are TDB
while the engine evaluates internally in TT
````

The conversion policy belongs to:

* TimeArchitecture
* simulation implementation

not to MeasurementDefinition.

---

# 11. GroundTruth Mapping

## 11.1 Purpose

GroundTruth providers use provider-specific request dialects.

Examples:

* Horizons API
* Miriade API

---

## 11.2 Mapping Principle

Provider mappings translate:

````text
MeasurementDefinition
→ provider request
````

Examples:

````text
MeasurementDefinition
    → HorizonsMapping
    → Horizons URL
````

````text
MeasurementDefinition
    → MiriadeMapping
    → Miriade request
````

---

## 11.3 Important Rule

Provider-specific parameters must never become part of canonical measurement semantics.

---

# 12. Determinism Principles

## 12.1 Core Rule

Identical inputs must produce:

````text
identical StateTree execution
identical StateHashes
identical DataHashes
identical SimulationData
````

---

## 12.2 Deterministic Replay

The following must be reproducible:

* SimulationRun
* StateTree execution
* TerminalNode result
* provider mappings
* hash generation

---

## 12.3 Forbidden

The following are forbidden:

* implicit provider assumptions
* dynamic node selection
* hidden transition ordering
* provider-specific semantics inside MeasurementDefinition
* hashing outside Contracts

---

# 13. Implementation goals

## 13.1 Simplicity-first principle

The StateMachine implementation MUST be KISS-first.
It must be understandable by a programming beginner.
Transparency beats elegance.
Explicit code beats clever abstraction.
No OOP magic.
No dynamic graph framework.
No hidden dispatch.
No generic architecture gymnastics.

Why KISS- and Simplicity first?

Because:

- debuggability
- explicitness
- determinism
- transparency

matter MUCH more here than “architectural cleverness”.


**Consequence:**

TerminalNodeType
→ simple switch / dictionary
→ ordered NodeType list
→ simple for-loop
→ explicit transition methods

## 13.2 No Goals

IStateMachine<TNode, TContext>
GraphExecutorFactory
TransitionResolverPipeline
reflection
attribute magic
dependency-injected graph framework

## 13.3. Three years outlook

M2.4 shall read in three years like this:

1. Which TerminalNode do we need?
2. Which fixed path belongs to it?
3. Execute node 1.
4. Execute node 2.
5. Execute node 3.
6. Write TerminalNode result.


# 14. Diagnostics

## 14.1 Diagnostic Codes

There are two potential error situations which need to be foreseen:

1) unknown MeasurementDefinition: this is in case the Engine Algo cannot interpret the MeasurementDefintion

2) unknown TerminalNode: this is is if - e.g in the case of replay - the required terminal node is missing, maybe because the NodeType definition has evolved.

To cover these cases, two new, different Diagnostic Codes must be introduced:

````text
040.010
Unknown MeasurementDefinition: Warning
```` 

````text
040.011
Unknown NodeType: Warning
```` 

Both codes can be put at the end of the priority list.

## 14.2 Diagnostic Diagnostics evaluation order

1. 040.003 Invalid ExperimentMaturity
   - evaluated immediately after Experiment load
   - before GroundTruth lookup
   - before unsupported configuration checks
   - before provider range checks
   - before any simulation execution
2. 040.008 Unsupported ScientificRun configuration
3. 040.004 Provider range violation
4. 040.002 Multiple matching GroundTruth datasets found
5. 040.009 No matching GroundTruth dataset found
NEW:
6. 040.010 Unknown MeasurementDefinition
7. 040.011 Unknown NodeType

> Note: In M2.4 legacy GT lookup may still happen before MeasurementDefinition resolution.
Future Measurement-driven GT lookup may reorder diagnostics.

## 14.3 Folder structure for diagnostic codes

```text
04_Simulations/
    DiagMessages/
        Run/
        LastRun/
```

# 15. Future Extensions

Future milestones may introduce:

* L1 StateTree branches
* L2 aberration branches
* observer projection layers
* TOPO measurements
* RA/DEC outputs
* AZ/ALT outputs
* dynamic StateTree selection
* multi-engine comparisons
* multiple TimeDomain branches

All future extensions must preserve:

````text
semantic orthogonality
````

---

# 16. Final Principle

AstronoSphere separates:

````text
Semantics
````

from:

````text
Execution
````

from:

````text
Persistence
````

This enables:

* deterministic validation
* provider-neutral measurement semantics
* engine-neutral measurement semantics
* reproducible simulation execution
* long-term scientific consistency

In short:

````text
MeasurementDefinition defines meaning.
Astronometria realizes execution.
AstronoTruth realizes GT-interaction
Contracts guarantee reproducibility.
````

# End of Document


