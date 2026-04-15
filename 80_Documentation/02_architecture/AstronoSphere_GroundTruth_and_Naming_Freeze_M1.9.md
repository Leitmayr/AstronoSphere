# AstronoSphere GroundTruth and Naming Freeze – M1.9

## Status

**Version:** 1.0  
**Status:** Freeze Candidate  
**Scope:** AstronoSphere / AstronoData / GroundTruth / Dataset Naming / Scenario Naming  
**Milestone:** M1.9

---

## 1. Purpose

This document defines the **frozen naming and storage conventions** for the M1.9 stage of AstronoSphere, with particular focus on:

- GroundTruth folder structure
- Scenario identity
- Dataset identity
- Human-readable file naming
- separation of semantic responsibilities between folders, headers, and file names

This document is intentionally detailed, because these conventions are not cosmetic.
They directly affect:

- determinism
- scientific traceability
- developer ergonomics
- long-term maintainability
- future extensibility of GroundTruth domains and providers
- consistency of validation workflows in Astronometria and Astronolysis

The goal of this freeze is to establish a system that is:

- scientifically meaningful
- operationally simple
- easy to browse
- easy to filter
- stable over time
- compatible with future extension without requiring premature complexity now

---

## 2. Architectural Context

AstronoSphere distinguishes between two different categories of naming:

### 2.1 Tool naming

Tools follow the **Astrono*** brand logic.

Examples:

- AstronoLab
- AstronoCert
- AstronoTruth
- Astronometria
- Astronolysis

This naming is part of the ecosystem identity and is intentionally brand-driven.

### 2.2 Data naming

Data structures are **not branded**, but named by semantic meaning.

Examples:

- Seeds
- Experiments
- GroundTruth
- Simulations
- Results

This decision ensures that data remains scientifically interpretable and is not over-coupled to current implementation choices.

The naming philosophy therefore is:

> Tools express brand.  
> Data expresses meaning.

This distinction is foundational for the entire system and is intentionally preserved in the GroundTruth design.

---

## 3. Data Area Overview

The agreed data structure for AstronoSphere M1.9 is:

```text
01_Seeds
02_Experiments
03_GroundTruth
04_Simulations
05_Results
```

Plural form is used for top-level folders because folders contain collections.

Singular form is used for individual entities, such as:

- Seed
- Experiment
- Simulation
- Result

This folder naming is part of the frozen system vocabulary.

---

## 4. Why GroundTruth is the Critical Data Area

GroundTruth is the central scientific reference layer of the validation system.

It is the point where:

- physical experiment definitions from Experiments
- external scientific measurements from truth providers
- measurement semantics from AstronoMeasurement
- deterministic storage rules

all converge.

GroundTruth is therefore more sensitive than the other data areas.

A bad GroundTruth structure would cause:

- confusion in provider comparison
- poor traceability in Astronolysis
- brittle folder hierarchies
- excessive clicking through the tree
- duplicate representation of information
- future migration pain

For this reason, GroundTruth required deliberate discussion and is treated as a dedicated freeze topic.

---

## 5. GroundTruth Folder Structure – Final Decision

The final GroundTruth structure for M1.9 is:

```text
03_GroundTruth/

  Ephemeris/
    Horizons/
      Run/
      LastRun/
      Baseline/

    Miriade/
      Run/
      LastRun/
      Baseline/

  OrbitalElements/
    <Provider>/
      Run/
      LastRun/
      Baseline/
```

For M1.9, only the **Ephemeris** domain is active and frozen.
The OrbitalElements branch is shown as a forward-compatible structural pattern.

---

## 6. Rationale for the GroundTruth Folder Structure

### 6.1 Domain first

GroundTruth is structured **domain-first**.

Example:

```text
03_GroundTruth/
  Ephemeris/
    Horizons/
```

This means the top-level split inside GroundTruth is not by provider, but by the scientific kind of truth being measured.

This was chosen for three reasons.

#### 6.1.1 Scientific reason

Different GroundTruth domains represent fundamentally different kinds of measurements.

For example:

- Ephemeris truth
- Orbital elements truth

These are not merely variants of the same data source.
They represent different scientific objects and must therefore be separated at the top level.

The structure of AstronoSphere must remain scientifically organized, not provider-organized.

#### 6.1.2 Architectural reason

Factories are domain-centered.

Examples:

- AstronoTruth for ephemeris truth
- future domain-specific truth tooling for orbital elements

A domain-first layout supports a clean mapping between code responsibility and storage responsibility.

#### 6.1.3 Extensibility reason

A single domain can later support multiple providers.

Example:

- Ephemeris / Horizons
- Ephemeris / Miriade

That enables direct comparison of providers within the same measurement class.

---

### 6.2 Provider second

Inside a domain, provider folders are mandatory.

Example:

```text
Ephemeris/
  Horizons/
  Miriade/
```

This is a frozen decision.

#### Rationale

Provider must remain explicit because:

- Astronolysis must be able to trace which provider produced a dataset
- provider comparison is a core scientific use case
- users must be able to quickly see coverage gaps between providers
- mixed provider data in one shared Run folder would be confusing and dangerous

The provider dimension is therefore considered stable enough to justify a folder level.

---

### 6.3 Why the structure stays shallow

The structure intentionally stops after:

- Domain
- Provider
- Run / LastRun / Baseline

The following dimensions are **not represented as folders**:

- Experiment
- ScenarioID
- Measurement level (L0, L1, ...)
- Instrument (VEC, RADEC, ...)
- model identifiers such as DE440
- body names
- event category
- frame details

This was a deliberate KISS decision.

#### Rationale

All of the above already exist in the dataset header and can be surfaced in the file name.
Introducing them as nested folder levels would create:

- too much clicking through the tree
- unnecessary structural rigidity
- duplicated information
- migration problems when future levels, instruments, or models are added

The system therefore follows this principle:

> Stable scientific axes belong in folders.  
> Variable measurement metadata belongs in headers.  
> Human navigation support belongs in the file name.

This is one of the most important design decisions of this freeze.

---

## 7. Run / LastRun / Baseline – Final Decision

Each provider contains:

```text
Run/
LastRun/
Baseline/
```

This is not considered temporary development scaffolding.
It is part of the permanent operational design.

### 7.1 Run

Run contains the current output of the latest GroundTruth generation.

Purpose:

- active processing output
- current regression candidate
- current comparison input

### 7.2 LastRun

LastRun contains the previous Run state.

Purpose:

- deterministic run-to-run comparison
- immediate regression visibility
- debugging support

### 7.3 Baseline

Baseline stores scientifically accepted reference states.

Purpose:

- long-term frozen comparison anchors
- release-aligned scientific states
- reference points for future regressions

---

## 8. Rationale for Keeping Run / LastRun Permanently

The decision was made to keep Run and LastRun permanently, not only during development.

### Why this matters

AstronoSphere is built around deterministic scientific validation.

Immediate comparison between Run and LastRun is not a temporary convenience.
It is one of the core operational expressions of determinism.

Removing Run / LastRun after "stabilization" would actually weaken the architecture.

The permanent pattern therefore is:

- Run = current state
- LastRun = previous state
- Baseline = accepted long-term reference state

This structure gives the project both:

- day-to-day regression visibility
- long-range scientific reproducibility

---

## 9. Baseline Versioning

Baseline changes are expected to be rare.

Typical triggers are:

- a deliberate provider model change (for example DE440 → DE441)
- a meaningful scientific correction
- a consciously accepted new validated system state

For normal operation, baseline changes should be infrequent and deliberate.

Because of that, baseline versioning is conceptually supported but operationally light.
The folder may later contain named frozen states if required.

The important freeze decision is:

> Baseline represents durable, accepted scientific states and must never be casually overwritten.

---

## 10. Why Model Identifiers Like DE440 Are Not Folder Levels

A specific discussion point was whether provider-specific model identifiers such as DE440 should appear in the folder path.

This was rejected.

### Rationale

DE440 is:

- highly specific to certain providers
- metadata, not a top-level structural axis
- already captured in the dataset header
- not important enough to justify another persistent navigation layer

Putting DE440 into the path would produce unnecessary depth without improving the main workflows.

The chosen design instead keeps model information in the dataset header, where it belongs.

This is consistent with the broader design principle:

> The file system should express stable operational classification, not every metadata field.

---

## 11. Measurement Levels (L0, L1, ...) – Final Decision

Measurement levels such as L0, L1, L2, ... are **not** folder levels.

They remain in the dataset header and appear in the DatasetID.

This is a frozen decision.

### Rationale

L0, L1, L2 are not domains and not providers.
They are part of the measurement definition.

Putting them into folders would:

- overfit the structure to the current level model
- create a brittle hierarchy
- require future migration as levels evolve
- duplicate information already present in the dataset identity

Therefore:

- folders remain stable
- levels remain part of the dataset description

This keeps the GroundTruth structure compact and future-proof.

---

## 12. ScenarioID – Final Decision

A revised ScenarioID was agreed.

### Final format

```text
<ORIGIN>-<EPOCH>-<TIMESCALE>-<STARTJD>-<STOPJD>-<STEP>
```

Example:

```text
HELIO-J2000-TDB-2451545-2451546-1H
```

This replaces earlier ideas that used more verbose or less balanced forms.

---

## 13. Rationale for the ScenarioID Format

### 13.1 Why Start and Stop are both included

A proposal was considered to shorten the ScenarioID by keeping only the start time and step size.

This was rejected.

#### Reason

Without StopJD, the time span of the experiment is not uniquely represented.

The ScenarioID would then no longer fully identify the temporal extent of the experiment.

That would be especially problematic because AstronoCert should not need hidden logic to reconstruct missing duration information.

Including StartJD and StopJD preserves:

- completeness
- determinism
- transparency

and avoids adding inference logic to the certification step.

---

### 13.2 Why Epoch is included

Epoch is explicitly part of the ScenarioID.

Example:

```text
HELIO-J2000-TDB-...
```

This is an important refinement.

#### Reason

A frame without epoch is incomplete for practical scientific identity.
J2000 and of-date representations are not interchangeable.

Including the epoch keeps the ScenarioID more explicit and more future-safe.

---

### 13.3 Why hyphens are used inside the ScenarioID

The ScenarioID uses hyphens consistently.

Reason:

- good readability
- robust parsing
- internal consistency with the DatasetID style

Underscores are not used inside the ScenarioID.

---

## 14. DatasetID – Final Decision

The DatasetID format is:

```text
<ScenarioID>__<DOMAINABBR>-<PROVIDER>-<INSTRUMENT>-<LEVEL>
```

Example:

```text
HELIO-J2000-TDB-2451545-2451546-1H__EPH-HORIZONS-VEC-L0
```

This is a frozen decision.

---

## 15. Rationale for the DatasetID Format

### 15.1 Double underscore as block separator

A key design decision was to use:

- `__` between Scenario and Dataset parts
- `-` inside each part

This creates a very strong visual and semantic separation.

Example:

```text
[Scenario]__[Dataset]
```

This is preferable to one uniform separator because it clearly distinguishes:

- experiment identity
- measurement identity

The double underscore is therefore not stylistic decoration.
It is a semantic boundary marker.

---

### 15.2 Why provider is part of DatasetID

Provider is part of the DatasetID by explicit decision.

Example:

```text
EPH-HORIZONS-VEC-L0
```

This is important because provider identity is scientifically relevant in Astronolysis.

Without provider in the DatasetID, it would be harder to answer questions such as:

- Which provider produced this GroundTruth?
- Is this dataset comparable to a Miriade dataset of the same experiment?
- Which provider was used for this measurement chain?

Provider therefore belongs to dataset identity, not merely to folder structure or secondary metadata.

This was one of the most important naming decisions in the discussion.

---

### 15.3 Why the order is Domain → Provider → Instrument → Level

This order was chosen because it expresses the semantic hierarchy of the measurement:

1. What truth domain is measured?
2. Which provider supplies the truth?
3. Which instrument / measurement form is used?
4. Which correction level applies?

This is clearer than other permutations because it moves from broader scientific class to more specific measurement interpretation.

---

### 15.4 Why model identifiers are not part of DatasetID in M1.9

Model identifiers such as DE440 are important metadata but are not included in the DatasetID at M1.9.

Reason:

- they are provider-specific details
- they change rarely
- they would lengthen IDs considerably
- they already exist in the dataset header

This keeps the DatasetID readable while preserving the full scientific traceability in metadata.

---

### 15.5 Domain abbreviations

Domain abbreviations are frozen as compact three-letter identifiers.

Examples:

- EPH = Ephemeris
- ORE = OrbitalElements

The purpose of abbreviations is to keep the DatasetID compact while preserving domain clarity.

---

## 16. Human-Readable File Name – Final Decision

The final file naming pattern is:

```text
<HumanReadable>__<ScenarioID>__<DatasetID>.json
```

Where:

```text
<HumanReadable> = <BODYCLASS>-<BODY>-<CATEGORYABBR>
```

Example:

```text
PLANET-MERCURY-INC__HELIO-J2000-TDB-2451545-2451546-1H__EPH-HORIZONS-VEC-L0.json
```

Uppercase is used for consistency and visual robustness.

---

## 17. Rationale for the Human-Readable Prefix

The human-readable prefix is intentionally separate from both ScenarioID and DatasetID.

It exists to support human browsing and debugging.

### Why this was needed

Purely technical IDs are deterministic and precise, but they are not always immediately helpful for a human browsing a folder.

The project therefore explicitly adds a human-readable block at the beginning of the file name.

That gives quick access to the experiment meaning before parsing the rest of the name.

This is especially useful when browsing directories during debugging or comparing provider coverage.

---

### 17.1 BodyClass

BodyClass is included because it helps classify the nature of the experiment quickly.

Examples:

- PLANET
- PLANETSYSTEM
- COMET
- STAR
- ASTEROID
- SUN
- MOON

This adds real semantic value and is not merely cosmetic.

A key additional architectural insight from the discussion was:

> BodyClass should become part of the Scenario Core and therefore part of the CoreHash.

This is important because BodyClass contributes to the experiment identity and should not remain only a filename decoration.

---

### 17.2 Body

Body names provide direct scientific readability.

Example:

- MERCURY
- CERES
- SUN

That makes folder browsing and mental indexing much easier.

---

### 17.3 Category abbreviation

Category abbreviations summarize the experiment phenomenon.

Examples include:

- ANO = Ascending Node
- APH = Aphelion
- CON = Conjunction
- DNO = Descending Node
- INC = Inferior Conjunction
- MCRE = MeshValidation
- OPP = Opposition
- PER = Perihelion
- QCR = Quadrant Crossing
- STA = Station
- GWE = Greatest Western Elongation
- GEE = Greatest Eastern Elongation
- MDP = Miscellaneous Data Point

These abbreviations allow a compact but meaningful event classification directly in the file name.

---

## 18. Why the File Name Uses Three Semantic Blocks

The final file name design can be understood as:

```text
[Human Meaning]__[Scenario Definition]__[Measurement Definition]
```

This is one of the strongest conceptual outcomes of the naming work.

### Block 1 – Human meaning

This tells a human reader what kind of experiment this file represents.

### Block 2 – Scenario definition

This captures the physical experiment identity.

### Block 3 – Measurement definition

This captures how the truth was measured.

This tripartite structure is powerful because it avoids confusion between:

- the meaning of the experiment
- the definition of the experiment
- the definition of the measurement

The system therefore remains semantically orthogonal while still being browseable.

---

## 19. Why HELIO and VEC Are Not Redundant

A final important clarification concerned the relationship between items such as:

- HELIO in the ScenarioID
- VEC in the DatasetID

At first glance, they may seem similar, but they are not redundant.

### HELIO

HELIO describes the physical observation context or scenario origin.

It answers:

- Where is the experiment anchored?
- What is the observation origin / physical context?

### VEC

VEC describes the instrument / measurement form.

It answers:

- What is being measured?
- In what representation is the result stored?

These are orthogonal dimensions.

For example, the same scenario origin could later be associated with different instruments, and the same instrument can be used with different origins.

Therefore:

> ScenarioID expresses physical experiment identity.  
> DatasetID expresses measurement identity.

This separation is intentional and should not be collapsed.

---

## 20. Uppercase Convention

The final file naming examples are written in uppercase.

Example:

```text
PLANET-MERCURY-INC__HELIO-J2000-TDB-2451545-2451546-1H__EPH-HORIZONS-VEC-L0.json
```

Uppercase was preferred because it offers:

- stronger visual uniformity
- easier browsing
- fewer case inconsistencies in practice

The extension remains `.json`.

---

## 21. Summary of Frozen Decisions

### 21.1 GroundTruth folder structure

```text
03_GroundTruth/
  Ephemeris/
    Horizons/
      Run/
      LastRun/
      Baseline/
    Miriade/
      Run/
      LastRun/
      Baseline/
```

### 21.2 ScenarioID

```text
HELIO-J2000-TDB-2451545-2451546-1H
```

### 21.3 DatasetID

```text
HELIO-J2000-TDB-2451545-2451546-1H__EPH-HORIZONS-VEC-L0
```

### 21.4 File name

```text
PLANET-MERCURY-INC__HELIO-J2000-TDB-2451545-2451546-1H__EPH-HORIZONS-VEC-L0.json
```

### 21.5 Structural principles

- domain-first
- provider-second
- shallow folders
- measurement metadata in header
- provider mandatory
- Run / LastRun / Baseline permanent
- no L0 / L1 folders
- no DE440 folders
- human-readable prefix added intentionally
- BodyClass should become part of Scenario Core

---

## 22. Final Architectural Interpretation

The frozen system can be summarized as:

- folders classify stable operational spaces
- headers store detailed scientific metadata
- file names support human work and quick orientation
- IDs remain deterministic and semantically separated

This design achieves a balance between:

- KISS
- scientific rigor
- usability
- future extensibility

It is intentionally neither purely filesystem-driven nor purely metadata-driven.
Instead, it assigns the right responsibility to each layer.

That balance is the core achievement of this freeze.

---

End of Document
