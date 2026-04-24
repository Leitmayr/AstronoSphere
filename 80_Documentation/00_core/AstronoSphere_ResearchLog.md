# AstronoSphere Research Log

## 2026
### January

- year begin with a nice grafical tool Sternkarten showing the heliocentric positions of the planets according to the Sterne und Weltraum monthly pages
- began with a first grafical representation of Astronometria in C# and WPF: 
    - included Bright Star Catalog Data and created first "Drehbare Sternscheibe" Code with constallations and various set of starts. Also added star colors according to Spectral Type.
    - decided to "add the planets quickly"
    - decided to postpone visualization of the planets "for a bit" to validate VSOP data first
- first thoughts about a Ground Truth generation with Horizons
- decision to develop Astronometria with Chat GPT

### February


#### 2025-02-03
- idea of developing a program which provides data from JPL Horizons
- Program called Ephemeris Regression (ER)

#### 2025-02-10
- spent a compelte Saturday to refactor the time domain: threw out UTC out of AstroDomain

#### 2025-02-17
- created code to generate two test suites TS-A, -B. EventDetection included in ER
- added TS-D: Mesh to evaluate VSOP
- created simple statistical report about VSOP vs. DE440

### March

#### 2026-03-02

- added TS-C quite quickly.
- realized that something is wrong with the runners and that another comprehensive refactoring would be necessary

#### 2026-03-09
- so far, ER was generating reference data, which I manually copied to dedicated folders of Astronometria for testing
- idea of setting up an Astronomical Data Base storing the reference data. Called it AstroReferenceData: ER was writing data to ARD, Astronometria used the ARD for the regression tests
- comprehensive, intensive, exhaustive renaming of the system. First thoughts of an integrated framework to overcome future pit holes
- first thoughts of taking out EventDetection out of EphemerisValidation: the event algos are slow and regression takes very long


#### 2026-03-14
(start of this ResearchLog - entries before were documentation of the history)

Breakthrough:
Birth of AstronoSphere.

Insight:
Scenario driven validation.

Architecture change:
EventGenerators removed from factories.

Hypothesis:
ObservationCatalog + ScenarioID may become
the central organizing principle of the system.

Next step:
Rebuild EphemerisFactory based on this model.


I have the impression something big is born.

#### Done Week 11 / 2026
✔ ScenarioID concept defined
✔ ObservationCatalog schema drafted
✔ Factory architecture defined
✔ AstronoSphere repo structure decided


#### 2026-03-15


Enhancement of the Scenario-Header:
- added author (Open Source feature)
- extension (placeholder if something was forgotton in the definition)
- Rationale: why was the scenario selected
- Scientific Purpose

#### 2026-03-16

Further enhancement to cover IP and Provenance
- Citation field added for Scenario
- Citation field added for TruthFactory
- added Provenance chain containing Scenario+TruthFactory+Validation fingerprint
- added Frame-Epoch to Scenario Core
- added Observer-Location to Senario Core
- generated V1.3 of Scenario definition
- had a phantastic and visionary "Spinner" chat about where AstronoSphere can develop to

#### 2026-03-17

Finalized V1.4 of Scenario Definition -> FREEZE

#### 2026-03-18

- Configuration Management Plan written
- New AstronoSphere GIT structure and missing repositories created

#### 2026-03-21

- Introduced AstronoMeasurement as the instance to define "Instruments" to measure with Horizons: simple and clear way to model L0...L5 to the Horizons configs
- setup of the VS project "AstronoSphere" as a Monoproject. First successful build w/o Astronometria active

#### 2026-03-22

- added remaining components to VS project (AstronoData)
- first implementation of SHG -> tag M1-SHG-complete

#### 2026-03-23

- filled the first three Scenarios in my role as Maintainer
- first implementation of EphemerisFactory -> baseline/M1-closed-pipeline-pilot
- first closed pipeline

### 2026-03-27
- Horizons-Astronometria Mapping (PPTX Presentation)

### 2026-03-28
- Pipeline running Observation Catalog to Reference Data (Delta evaluation Run/LastRun)

### 2026-03-29
- Pipeline Process refinement 23_*.PPTX: Process now entirely defined
- 144 scenarios implemented and added to the Observation catalog

### 2026-03-30
- Testplan for the pipeline written

### 2026-03-31
- a selection of the 144 scenarios tested in the pipeline
- identified 7 Bugs to be fixed. Session was extremely exhausting, many hours fully concentrated work. But very effective!

### 2026-04-02
- fixed all 7 Bugs in an evenmore exhausting session. I prepared the session extremely diligent and opened a dedicated chat to solve the issues. At the end of the evening all bugs were fixed and the pipeline was logically and numerically stabilized. Highlight Session!

### 2026-04-03
- renaming decision: AstronoLab, AstronoCert, AstronoTuth, Astronometria, Astronalysis
- specified and documented complete data model of Astronosphere in an 8 hours session: file names, folders, headers, workflow and writing permits.

### 2026-04-04
- Planning of the migration: ScenarioMerger defined
- Validation plan created

### 2026-04-06
- implemented ScenarioMerger Part 1
- "STRICT-Implementation Mode" established: needed to avoid that CGPT begins to interpret and deviates from the plan.
- the session was bad... so the STRICT mode was born as a countermeasure
- still learning how to use AI efficiently, still observing suprises

### 2026-04-07
- AstronoLab implemented: seeds are being generated now in AstronoLab
- Secenario Merger Part 2 used to migrate old data sets

### 2026-04-08
- decided in the morning to establish a central hashing entity in AstronoData.Contracts, which is being used to canonicalize and hash all inputs in the same way, independent if experiments, dataset (measurements) or analysis data are being hashed
- specified hashing and implemented AstronoData.Contracts in the evening
- implemented AstronoCert - struggled with precision topics and escape sequences again

### 2026-04-10
- META chat about use cases of AstronoSphere in the morning
- afternoon Dataset23: AstronoTruth implemented - ExperimentID, DatasetIF, Filename, DatasetHeader, Canonical, Hash centralized -> fully validated
- Refactoring:  EphemerisRegression Legacy code cleaned up. Folder renaming, old files deleted. Very good cleanup!
- A super session! I prepared it most diligently. Every step, a lot of interaction with CGPT. One of the strongest sessions ever today!
- see extra session description below

### 2026-04-11
- established the future development strategy: change only one dimension to control complexity
- prepare sessions diligently
- use CGPT as a reviewer/sparrings partner not just a coding monkey
- planned M2 
- setup a test plan for finalizing the whole pipeline
- identified Beyond Compare CLI capabilities for more efficient validation -> will probably purchase the full version of the program: it became a core tool for me
- Stealth Manifest formulated
- in the evening: brillant Web-Session on the Miriade-Homepage: plenty of different reference data available there. Enough material for scientific research!


### 2026-04-12
- updated documentation and threw out outdated files
- made a diligent pipeline validation plan
- identified three optimization points during testing and fixed them right away
- almost finished testing. Identified dublettes in the data - the Pipeline is correct but the input data was not. The pipeline has taken care of it: strong signal for stable pipeline.
- Tomorrow last GoldenSample validation and then the automatic run of the entire pipeline: let's us hope for the best. I am quite confident...
- a brillant and very productive week is over: the pipeline is almost finished

### 2026-04-13

- Pipeline just one step before finalization
- one major issue with precision found and another one with the Request Hash
- did not think I would encounter such topics short before closure
- after almost one day in tweaking 8 decimals behind the comma I am done
- postponed the final test with the pipeline to tomorrow but prepared everything for the big showdown :-)


### 2026-04-15
- showdown run successfully passed all tests, especially Run == LastRun, see special Info 
- it is an extremely good day because it was hell of a fight to get it all fixed to reach this milestone. It is a fundamental basis now for future extensions: **very happy today!!! :-)**
- see extra session description below

### 2026-04-18
- worked out a firm specification for the Mesh-Files. Decided to implement a MeshGenerator, because Start- und Stop-Points on a Mesh are hard to determine manually

### 2026-04-22
- began with the MeshGenerator. Found some spec inconsistencies, fixed them. Could not finalize the MeshGenerator, though.

### 2026-04-23
- finalized the MeshGenerator and updated spec accordingly
- all new mesh files can be generated now
- old mesh files are inconsistent and shall be deprecated (what a bummer, but it must be for clarity reasons)
- generated DocumentationPolicy. I found that, because of the high development speed, too many documents are being generated and not filed with enough structure. Now I have defined which documents to generate and where to store them.

### 2026-04-24
-  began implementing new data storage as per documentation policy


------------------

## Special info about this great session of 2026-04-15: Milestone 1 successfully reached

>**Mission accomplished: M1.9 Milestone successfully reached**


## Technisch

```text
✔ deterministische Pipeline
✔ stabile Hashes
✔ reproduzierbare API Calls
✔ konsistente Daten
```

---

## Wissenschaftlich

```text
✔ vollständige Provenance
✔ physikalische Konsistenz
✔ nachvollziehbare Unsicherheit
```

---

## Operativ

```text
✔ automatisierbarer Pipeline-Run
✔ Validierung über Run == LastRun
✔ stabile Datenbasis
```

## Special info about this great session of 2026-04-10

### Overview
Highly productive session with major architectural and validation progress in AstronoTruth.

---

### META

- Morning session: discussion of AstronoSphere use cases
- Clarified strategic direction:
  - focus on correctness first
  - discovery phase later
  - publication as final step

---

### AstronoTruth Implementation (Dataset #23)

- Full implementation of AstronoTruth pipeline for Dataset #23

Completed components:

- ExperimentID integration
- DatasetID generation
- Filename normalization
- DatasetHeader generation
- CanonicalRequest construction
- Hash generation (RequestHash, EpochHash)

---

### Precision Fix (CRITICAL)

- Identified root cause of precision inconsistencies:
  - mixed usage of ScenarioID and Core.Time
- Implemented fix:
  - Core.Time is now the single source of truth
  - string-based propagation (GetRawText)
  - no double parsing, no formatting

Result:

- sub-millisecond precision
- deterministic Horizons requests
- stable hashing

---

### Validation

- Dataset #23 fully validated
- Dataset #3 successfully migrated and validated

Validation method:

- Run vs LastRun comparison
- binary equality confirmed
- manual inspection of DatasetHeader
- canonical/hash verification

---

### Refactoring (Major Cleanup)

- Removed EphemerisRegression legacy code:
  - EventFinding
  - Mesh
  - Regression
  - Runner
  - Batching

- Reduced EphemerisFactory to minimal core:
  - FactoryRunner
  - HorizonsRequestBuilder
  - HorizonsApiClient
  - HorizonsCsvParser
  - DatasetBuilder

- Removed dependency to AstronoMeasurement (M1 simplification)

---

### Structural Changes

- Folder rename:
  - 03_TruthFactory → 03_AstronoTruth
  - 05_AnalysisTool → 05_Astronolysis

- Cleaned up file system:
  - unused folders deleted
  - project structure simplified

---

### Development Quality

- Extensive preparation before session
- tight feedback loop with ChatGPT
- step-by-step validation
- no uncontrolled changes

---

### Outcome

- AstronoTruth is now:
  - deterministic
  - precision-stable
  - minimal
  - testable

- Dataset generation is reproducible and trustworthy

---

### Personal Note

A super session.

One of the strongest sessions so far:
- high focus
- clean execution
- strong architectural decisions
- no wasted effort

---


## Backlog

### 2026-04-03

Future Feature:
Astronometria → Seed Export

Status:
post M1.9

Reason:
Exploration-driven scenario discovery

