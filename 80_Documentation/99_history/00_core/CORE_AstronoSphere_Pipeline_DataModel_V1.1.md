# Astronosphere Data Model

# I. Overview (M1.9)

## Example Experiment

Planet Mercury – Inferior Conjunction (INC)  
HELIO / J2000 / TDB  
JD 2451545–2451546, Step 1H  

---

## 1. AstronoLab

### Input
01_Seeds/Incoming/

Example file name:
SCN_xxxxxx.json

### Function
Processes Seeds into structurally valid ExperimentCandidates (without certification)

### Output
01_Seeds/Prepared/

Example:
PLANET-MERCURY-INC__HELIO-J2000-TDB-2451545-2451546-1H.json

---

## 2. AstronoCert

### Input
01_Seeds/Prepared/

### Function
Certifies ExperimentCandidates into official Experiments and assigns CatalogNumber

### Output
02_Experiments/Released/
01_Seeds/Processed/

Example:
AS-000123__PLANET-MERCURY-INC__HELIO-J2000-TDB-2451545-2451546-1H.json

---

## 3. AstronoTruth (Horizons)

### Input
02_Experiments/Released/

### Function
Generates GroundTruth data (Ephemeris)

### Output
03_GroundTruth/Ephemeris/Horizons/Run/

Example:
PLANET-MERCURY-INC__HELIO-J2000-TDB-2451545-2451546-1H__EPH-HORIZONS-VEC-L0.json

---

## 4. Astronometria

### Input

- Experiments: 02_Experiments/Released/
- implicit Measurement (Instrument + Level)

### Function
Simulates Experiments with configurable model accuracy

### Output
04_Simulations/Astronometria/Baseline/

Examples:
PLANET-MERCURY-INC__HELIO-J2000-TDB-2451545-2451546-1H__ASTRONOMETRIA-MEEUS-VEC-L0.json  
PLANET-MERCURY-INC__HELIO-J2000-TDB-2451545-2451546-1H__ASTRONOMETRIA-VSOP87-VEC-L0.json  

---

## 5. Astronolysis

### Input
02_Experiments/Released/  
03_GroundTruth/...  
04_Simulations/...  

### Function
Analyzes deviations and generates new Seeds

### Output
05_Results/  
01_Seeds/Incoming/

Example Result:
MERCURY-INC-2000_ENGINE-COMPARISON.json

Generated Seed:
PLANET-MERCURY-INC__HELIO-J2000-TDB-2451545-2451546-1H.json

---

## Pipeline Flow

Seeds/Incoming  
→ AstronoLab  
→ Seeds/Prepared  
→ AstronoCert  
→ Experiments/Released 
  → GroundTruth
  → Simulations  
→ Astronolysis  
→ Results + new Seeds  

Measurement is implicit in DatasetID and SimulationID

---

## Core Principle

AstronoSphere forms a closed scientific loop:

Seed → Experiment → Measurement → Simulation → Analysis → new Seed

# II. AstronoSphere – Seeds Folder Structure Freeze (M1.9)


## 1. Purpose

This document defines the final folder structure and semantics for **Seeds**.

Seeds represent the **starting point of the scientific pipeline**.

They are:
- experiment candidates
- generated or curated
- not yet certified as Experiments

---

## 2. Core Principle

> Seeds are first-class citizens and represent potential experiments.

They must:
- be compatible with Experiment structure
- be ingestible by AstronoLab without transformation
- remain traceable throughout the pipeline

---

## 3. Folder Structure

```text
01_Seeds/
  Incoming/
  Prepared/
  Processed/
```

---

## 4. Semantic Definition

### 4.1 Incoming

Incoming contains all newly created Seeds.

Sources:
- Astronolysis (automated generation)
- Meeus Tool
- manual creation

Characteristics:
- unprocessed
- not reviewed
- not validated

---

### 4.2 Prepared

Prepared contains Seeds processed by AstronoLab.

Characteristics:
- structurally validated
- enriched with required metadata
- ready for certification

Important:
- NOT yet official
- NOT yet part of Experiments

---

### 4.3 Processed

Processed contains Seeds that have been handled by AstronoCert.

Characteristics:
- already certified into Experiments
- no longer part of active pipeline
- kept for traceability

---

## 5. Tool Responsibilities

### AstronoLab

- reads: Incoming
- writes: Prepared

AstronoLab MUST NOT write to:
- Experiments
- GroundTruth
- Simulations
- Results

---

### AstronoCert

- reads: Prepared
- writes:
  - Experiments/Released
  - Processed

AstronoCert is the ONLY component allowed to create Experiments.

---

### Astronolysis

- writes: Incoming
- optionally reads: Experiments, GroundTruth, Simulations

Astronolysis may generate Seeds automatically based on analysis.

---

## 6. Design Rationale

### 6.1 State-based model

Folders represent **state transitions**, not tools.

Incoming → Prepared → Processed

This ensures:
- clarity
- determinism
- auditability

---

### 6.2 Separation of concerns

- Creation (Seeds) is separated from certification (Experiments)
- No tool bypasses AstronoCert

---

### 6.3 Scientific workflow alignment

The structure follows a natural lifecycle:

Seed → Preparation → Certification → Experiment

This reflects a scientific process:
- idea generation
- refinement
- validation

---

## 7. Explicit Rules

- Seeds MUST NOT be written directly to Experiments
- Only AstronoCert may create Experiments
- Seeds MUST be structurally identical to ExperimentCandidate
- No transformation step between Seeds and Experiment ingestion

---

## 8. Naming Consistency

Seeds use the same structure as ExperimentCandidate:

```json
{
  "Event": { ... },
  "Core": { ... },
  "Metadata": { ... }
}
```

---

## 9. Freeze Decision (M1.9)

- Seeds folder structure frozen
- Naming (Incoming / Prepared / Processed) frozen
- Tool responsibilities frozen
- Pipeline transitions frozen

# III. AstronoSphere – Experiments & AstronoCert Freeze (M1.9)


## 1. Purpose

This document defines:
- the role of AstronoCert
- the structure of Experiments
- the naming convention of Experiment files

Experiments represent **certified physical experiments**.

---

## 2. Core Principle

> An Experiment is a certified experiment.

It is:
- immutable
- uniquely identified
- officially part of the scientific catalog

---

## 3. AstronoCert Responsibilities

AstronoCert is the ONLY component allowed to create Experiments.

### Reads
- 01_Seeds/Prepared

### Writes
- 02_Experiments/Released
- 01_Seeds/Processed (move)

### Rules

- MUST NOT write to GroundTruth, Simulations, Results
- MUST assign CatalogNumber
- MUST preserve experiment integrity (CoreHash unchanged)

---

## 4. Folder Structure

```text
02_Experiments/
  Released/
```

No Created folder exists.

Certification is the final step before persistence.

---

## 5. Experiment Identity

Each Experiment contains:

- ExperimentID (content-derived)
- CoreHash (content integrity)
- CatalogNumber (assigned identity)

---

## 6. File Naming (FROZEN)

### Format

```
<CATALOGNUMBER>__<HUMAN>__<EXPERIMENT>.json
```

### Example

```
AS-000123__PLANET-MERCURY-INC__HELIO-J2000-TDB-2451545-2451546-1H.json
```

---

## 7. Naming Components

### 7.1 CatalogNumber (Prefix)

- format: AS-XXXXXX
- primary identifier
- ensures:
  - stable referencing
  - natural sorting
  - human workflow compatibility

---

### 7.2 Human Readable Part

```
<BODYCLASS>-<BODY>-<CATEGORY>
```

Example:

```
PLANET-MERCURY-INC
```

Purpose:
- fast recognition
- browsing support

---

### 7.3 Experiment Part

```
HELIO-J2000-TDB-2451545-2451546-1H
```

Derived from Experiment Core.

Ensures:
- uniqueness
- traceability

---

## 8. Design Rationale

### 8.1 ID-first design

CatalogNumber is the leading element.

This supports:
- referencing ("AS-000123")
- scientific workflows
- stable identity independent of naming changes

---

### 8.2 Human readability

The HUMAN section allows:
- quick navigation
- intuitive understanding

---

### 8.3 No redundancy in hash

CoreHash is NOT part of filename.

It remains in header only.

---

### 8.4 Uppercase convention

All filenames MUST be uppercase.

Ensures:
- consistency
- cross-platform stability

---

## 9. Immutability Rule

Once created:

- Experiment MUST NOT be modified
- any change → new CatalogNumber

---

## 10. Freeze Decision (M1.9)

- AstronoCert role frozen
- Experiments structure frozen
- Folder structure frozen
- Naming convention frozen
- ID-first principle frozen


# IV. AstronoSphere GroundTruth and Naming Freeze – M1.9


## Status

**Version:** 1.0  
**Status:** Freeze Candidate  
**Scope:** AstronoSphere / AstronoData / GroundTruth / Dataset Naming / Experiment Naming  
**Milestone:** M1.9

---

## 1. Purpose

This document defines the **frozen naming and storage conventions** for the M1.9 stage of AstronoSphere, with particular focus on:

- GroundTruth folder structure
- Experiment identity
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
GroundTruth is an exception, since the word implies that there is only one truth.

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
- experimentID
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

## 12. ExperimentID – Final Decision

A revised ExperimentID was agreed.

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

## 13. Rationale for the ExperimentID Format

### 13.1 Why Start and Stop are both included

A proposal was considered to shorten the ExperimentID by keeping only the start time and step size.

This was rejected.

#### Reason

Without StopJD, the time span of the experiment is not uniquely represented.

The ExperimentID would then no longer fully identify the temporal extent of the experiment.

That would be especially problematic because AstronoCert should not need hidden logic to reconstruct missing duration information.

Including StartJD and StopJD preserves:

- completeness
- determinism
- transparency

and avoids adding inference logic to the certification step.

---

### 13.2 Why Epoch is included

Epoch is explicitly part of the ExperimentID.

Example:

```text
HELIO-J2000-TDB-...
```

This is an important refinement.

#### Reason

A frame without epoch is incomplete for practical scientific identity.
J2000 and of-date representations are not interchangeable.

Including the epoch keeps the ExperimentID more explicit and more future-safe.

---

### 13.3 Why hyphens are used inside the ExperimentID

The ExperimentID uses hyphens consistently.

Reason:

- good readability
- robust parsing
- internal consistency with the DatasetID style

Underscores are not used inside the ExperimentID.

---

## 14. DatasetID – Final Decision

The DatasetID format is:

```text
<ExperimentID>__<DOMAINABBR>-<PROVIDER>-<INSTRUMENT>-<LEVEL>
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

- `__` between Experiment and Dataset parts
- `-` inside each part

This creates a very strong visual and semantic separation.

Example:

```text
[Experiment]__[Dataset]
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
<HumanReadable>__<ExperimentID>__<DatasetID>.json
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

The human-readable prefix is intentionally separate from both ExperimentID and DatasetID.

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

> BodyClass should become part of the Experiment Core and therefore part of the CoreHash.

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
[Human Meaning]__[Experiment Definition]__[Measurement Definition]
```

This is one of the strongest conceptual outcomes of the naming work.

### Block 1 – Human meaning

This tells a human reader what kind of experiment this file represents.

### Block 2 – Experiment definition

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

- HELIO in the ExperimentID
- VEC in the DatasetID

At first glance, they may seem similar, but they are not redundant.

### HELIO

HELIO describes the physical observation context or Experiment origin.

It answers:

- Where is the experiment anchored?
- What is the observation origin / physical context?

### VEC

VEC describes the instrument / measurement form.

It answers:

- What is being measured?
- In what representation is the result stored?

These are orthogonal dimensions.

For example, the same Experiment origin could later be associated with different instruments, and the same instrument can be used with different origins.

Therefore:

> ExperimentID expresses physical experiment identity.  
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

### 21.2 ExperimentID

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
- BodyClass should become part of Experiment Core

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

# V. AstronoSphere – Simulations & Naming Freeze (M1.9 v2)


## 1. Purpose

This document defines the **Simulation layer** of AstronoSphere.

Simulations represent computed results of experiments defined in the ObservationCatalog.

They are generated by:

- Astronometria (primary engine)
- future engines (e.g., Miriade)

---

## 2. Core Principle

Simulations MUST mirror GroundTruth structure.

Goal:

- Direct comparability
- Deterministic validation
- Scientific reproducibility

---

## 3. Folder Structure (Freeze)

```
AstronoData/
  04_Simulations/
    Ephemeris/
      Astronometria/
        Run/
        LastRun/
        Baseline/
```

---

## 4. Run / LastRun / Baseline

Run = current execution  
LastRun = previous snapshot  
Baseline = scientific reference (Git baseline)

---

## 5. Conceptual Model

Simulation is defined by:

- Experiment (experiment)
- Measurement (instrument + level)
- Engine configuration (how computed)

---

## 6. SimulationID (Freeze)

```
<ENGINE>-<ENGINECONFIG>-<INSTRUMENT>-<LEVEL>
```

Examples:

ASTRONOMETRIA-MEEUS-VEC-L0  
ASTRONOMETRIA-VSOP87-VEC-L0  

---

## 7. Rules

MUST include:
- Engine
- EngineConfig
- Instrument
- Level

MUST NOT include:
- Frame
- TimeScale
- Epoch
- Target

---

## 8. Filename

```
<HUMAN>__<EXPERIMENT>__<SIMULATION>.json
```

Example:

PLANET-MERCURY-INC__HELIO-J2000-TDB-2451545-2451546-1H__ASTRONOMETRIA-MEEUS-VEC-L0.json

---

## 9. Key Principle

SimulationID contains only parameters that affect computation.

---

## 10. Freeze (M1.9)

- Structure frozen
- SimulationID frozen
- EngineConfig mandatory

# VI. AstronoSphere – Results (Astronolysis) Freeze (M1.9)


## 1. Purpose

Results represent the output of Astronolysis.

They:
- evaluate multiple Experiments, GroundTruth datasets and Simulations
- produce scientific insights
- generate new Seeds for the next pipeline cycle

---

## 2. Core Principle

> Results describe ANALYSIS, not measurement or simulation.

They operate on:
- multiple Experiments 
- multiple GroundTruth datasets
- multiple Simulations

---

## 3. Naming

### File Name

```
<SCOPE>_<ANALYSISTYPE>.json
```

Example:

```
MARS-OPP-1900-2000_ENGINE-COMPARISON.json
```

---

## 4. Header Structure

```json
{
  "Scope": {
    "Body": "Mars",
    "Category": "OPP",
    "TimeRange": "1900-2000"
  },

  "AnalysisType": "ENGINE_COMPARISON",

  "GroundTruthProviders": [
    "HORIZONS",
    "MIRIADE"
  ],

  "Simulations": [
    "ASTRONOMETRIA-MEEUS-VEC-L0",
    "ASTRONOMETRIA-VSOP87-VEC-L0"
  ]
}
```

---

## 5. Snapshot & Reproducibility

### 5.1 Snapshot Structure

```json
{
  "Snapshot": {
    "Experiments": [
      {
        "ExperimentID": "...",  "CoreHash": "...",  "CatalogNumber": "AS-XXXXXX"
      }
    ],
    "GroundTruth": [
      {
        "GroundTruthID": "...",
        "Hash": "..."
      }
    ],
    "Simulations": [
      {
        "SimulationID": "...",
        "Hash": "..."
      }
    ],
    "SnapshotHash": "..."
  }
}
```
Note: "Experiments" should be written in one line to save space.

---

### 5.2 Canonicalization Rule

SnapshotHash is computed as:

- collect all referenced elements
- sort alphabetically
- canonicalize structure
- apply SHA256

```
SnapshotHash = SHA256(canonical(sorted(input)))
```

This is identical to:
- Experiment CoreHash
- Dataset Hashing

---

## 6. Results Body

Result content is intentionally **not frozen**.

Only top-level structure is defined:

```json
{
  "Results": {
    "Statistics": { ... },
    "Distributions": { ... },
    "Outliers": [ ... ]
  }
}
```

---

## 7. Generated Seeds

### 7.1 Structure (FROZEN)

```json
{
  "GeneratedSeeds": [
    {
      "SeedCandidate": {
        "Event": {
          "Category": "QCR",
          "Qualifier": "L18",
          "Description": "X coordinate sign change (- to +)"
        },
        "Core": {
          "Time": {
            "StartJD": 2478083.966,
            "StopJD": 2478085.966,
            "Step": "1H",
            "TimeScale": "TDB"
          },
          "Observer": {
            "Type": "Heliocentric",
            "Body": "Sun"
          },
          "ObservedObject": {
            "BodyClass": "Planet",
            "Targets": ["Jupiter"]
          },
          "Frame": {
            "Type": "HelioEcliptic",
            "Epoch": "J2000"
          }
        },
        "Metadata": {
          "Author": "Astronolysis",
          "Priority": 2,
          "Status": {
            "Maturity": "Draft",
            "Visibility": "Private"
          }
        },
        "Notes": "Generated automatically from residual peak detection."
      },
      "SeedOrigin": {
        "ResultID": "MARS-OPP-1900-2000_ENGINE-COMPARISON",
        "Reason": "High residual near opposition cluster",
        "Trigger": "ResidualPeak",
        "CreatedAtUtc": "2026-04-03T21:15:00Z"
      }
    }
  ]
}
```

---

### 7.2 Design Rules

- SeedCandidate MUST be valid ExperimentCandidate
- no transformation required for AstronoLab
- SeedOrigin MUST NOT influence Core or Hash

---

## 8. AnalysisType

AnalysisType is an open taxonomy.

Examples:

- ENGINE_COMPARISON
- PROVIDER_COMPARISON
- VALIDATION_RUN
- EDGE_DETECTION

---

## 9. Tool Responsibilities

### Astronolysis

- writes Results
- writes Seeds (Incoming)
- reads:
  - Experiments
  - GroundTruth
  - Simulations

---

## 10. Freeze Decision (M1.9)

- Snapshot model frozen
- Canonicalization frozen
- Seed output format frozen
- Naming frozen
- AnalysisType open
- Result body open


End of Document
