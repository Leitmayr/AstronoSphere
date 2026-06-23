````md
# AS_ARCH_M2.4_StateMachine_L0
## Startup Post

We are starting the architecture phase for:

```text
Astronometria M2.4
StateMachine / StateGraph implementation
````

---

# 1. Current Project State

M2.3 has been completed successfully.

M2.3 introduced:

* deterministic ScientificRun execution
* SimulationData persistence
* GroundTruth resolution
* Run/LastRun determinism
* Diagnostics
* StateHash/DataHash foundations
* Scientific provenance and citation support
* deterministic simulation pipeline integration into AstronoSphere

The architectural result of M2.3 has been promoted to:

```text
80_Documentation/Core/
```

and is now considered frozen architectural foundation.

---

# 2. Source of Truth

The attached CORE document is the frozen architectural foundation.

Do NOT redesign or reinterpret the architecture unless true inconsistencies are found.

The CORE document defines:

* terminology
* State semantics
* Data semantics
* StateHash/DataHash semantics
* ScientificRun / ExplorationRun / StatisticalRun
* ObservationScene
* StateGraph
* TerminalNode
* diagnostics
* persistence rules
* naming rules
* deterministic semantics
* provenance and citation semantics

The goal of this chat is NOT to recreate these foundations.

The goal is:

```text
derive a focused implementation-oriented M2.4 specification from the CORE document
```

---

# 3. Goal of M2.4

M2.4 introduces the executable StateMachine semantics of Astronometria.

Main focus:

```text
deterministic replayable state-based simulation execution
```

M2.4 shall transform the current simulation engine into a deterministic StateGraph execution architecture.

---

# 4. Focus of this Chat

This chat shall focus on:

* executable StateMachine semantics
* concrete StateNode implementation
* Transition execution
* replayability
* persistence semantics
* deterministic execution order
* node sequencing
* immutable StateTree semantics
* deterministic hash boundaries
* intermediate StateNode semantics
* validation semantics
* replay verification
* deterministic node persistence
* future compatibility with StatisticalRuns

---

# 5. Explicitly Out Of Scope

The following topics are intentionally OUT OF SCOPE for M2.4:

* ExplorationMode implementation
* GUI
* Statistical persistence implementation
* Monte Carlo
* RA/DEC output
* OfDate
* externalized StateTrees
* dynamic graph editors
* distributed execution
* advanced correction physics beyond current milestones
* uncertainty propagation
* user scripting
* adaptive pipelines

These topics may be discussed only if strictly required for architectural consistency.

---

# 6. Architectural Context

Astronometria is not merely a calculator.

Astronometria is evolving into:

```text
a deterministic scientific execution engine
```

inside the larger AstronoSphere ecosystem.

The ecosystem already provides:

* certified experiments
* deterministic GroundTruth
* deterministic dataset persistence
* hashing infrastructure
* diagnostics
* reproducibility infrastructure
* validation infrastructure

M2.4 extends this by introducing deterministic internal state execution semantics.

---

# 7. Core Architectural Principles

## 7.1 StateTree Principle

Astronometria uses a directed StateTree.

Rule:

```text
Exactly one path exists from source to TerminalNode.
```

Implication:

```text
TerminalNodeType uniquely determines the full execution path.
```

Therefore:

* full graph persistence is unnecessary
* replay remains deterministic
* persistence stays compact
* validation becomes simpler

This principle is already frozen in the CORE document.

---

## 7.2 Determinism Principle

Determinism has highest priority.

Scientific reproducibility is mandatory.

Run equality is defined as:

```text
Run == LastRun
```

through deterministic comparison of:

* StateHashes
* DataHashes
* ordered node sequences

---

## 7.3 Scientific Provenance Principle

Scientific datasets must remain traceable.

SimulationData must preserve:

* Experiment provenance
* Truth provenance
* Simulation provenance

Scientific output must remain scientifically citeable.

---

# 8. Working Mode

We are operating under:

## 8.1 STEALTH MODE

One dimension at a time.

No parallel feature expansion.

No uncontrolled future architecture leakage.

Only discuss future topics if they are required to validate current architectural decisions.

---

## 8.2 STRICT MODE

No Trial and Error.

If uncertainty exists:

* ask
* clarify
* specify

before implementation assumptions are made.

No silent interpretation changes.

---

## 8.3 KISS PRINCIPLE

Always prefer:

* simpler structure
* deterministic structure
* replayable structure
* inspectable structure
* explicit structure

Avoid:

* overengineering
* premature abstractions
* dynamic systems without necessity
* unnecessary persistence redundancy

---

# 9. Efficiency Package Rules

Please also follow the attached:

```text
COLLAB_EfficiencyPackage_V1.1.md
```

Important implications:

* short focused answers preferred
* no unnecessary side discussions
* architecture first
* implementation second
* validation integrated continuously
* complete code files preferred later during IMP phase
* milestone discipline mandatory

---

# 10. Expected Outcome of this Chat

This ARCH chat shall produce:

```text
SPEC_M2.4_StateMachine_L0.md
```

The resulting spec shall define:

* concrete M2.4 StateNodes
* Transition semantics
* State persistence semantics
* replay semantics
* hash semantics
* execution order
* deterministic behavior
* validation semantics
* node naming semantics
* intermediate node handling
* preparation for future StatisticalRuns

The resulting spec shall be implementation-ready.

---

# 11. Important Context

M2.3 implementation already exists.

M2.4 therefore extends an existing deterministic simulation infrastructure.

This is NOT greenfield architecture.

The architecture must integrate cleanly with:

```text
01_Seeds
02_Experiments
03_GroundTruth
04_Simulations
05_Results
```

and the already frozen CORE semantics.

---

# 12. Initial Task

Please begin with:

1. brief summary of how you understand the frozen CORE architecture
2. identification of the exact architectural delta between M2.3 and M2.4
3. proposal for the structure of SPEC_M2.4_StateMachine_L0.md
4. identification of the most critical unresolved architectural decisions for M2.4

Attachments: both can be found in CGPT/Sources
* CORE_AstronometriaStateMachine_V1.2.md
* COLLAB_EfficiencyPackage_V1.1.md

```
```


