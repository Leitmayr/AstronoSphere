# AstronoSphere Research Log

# 2026

## June 2026


### 2026-06-22

### 2026-06-22

- Today's target was to add GEO-EQU. After 5 minutes it became clear that no matching GroundTruth data exists yet. Investigation showed that enabling GEO-EQU would require changes in AstronoTruth first, thereby opening a second development frontier.

- Decision: GEO-EQU was deferred from M2.4 and moved into a dedicated later milestone together with the required AstronoTruth and AstronoMeasurement work.

- Result:
  - StateMachine scope became cleaner.
  - M2.4 Spec and Validation Spec were aligned accordingly.
  - The overall M2.x milestone plan was updated.
  - The OneDimension Rule from the Stealth Manifest was preserved.

- No visible implementation progress was achieved today. However, a potentially dangerous cross-project dependency was identified before development started, preventing scope creep and additional validation effort.

-> Slightly dissatisfying session from a feature perspective, but likely the correct engineering decision.


### 2026-06-21

- 45min speed session: prove, that M2.4 state machine is computed
- BC5 of Run/LastRun shows no differences in all (~200 !) simulations. Seems as we had switche to state machine computation successfully!

### 2026-06-19

- Found Beyond Compare 5 settings which allow rule based exceptions. This is a game changer, because it allow in the future, to conduct automatic evaluations of Run/LastRun by defining proven excetions. Hence it will reduce validation in the future substantially. One of these great achievements which accelerate the flow in every future session!


### 2026-06-18

- M2.4 W1-6 implemented, in 1.5 hours instead of 4 estimated hours
- Only problem is to activate RuleBased Compare in BeyondCompare. I fear I must read the manual - neither MetaAI nor CGPT can help here. MetaAI really did a poor job in defining the rules.
- Nevertheless satisfied: Astronometria State Machine calculates results and writes JSONS now like before.

### 2026-06-17

- getting started again took an evening
- we identified some major architectural gaps, which I did not expect. But now we are ready for implementation.
- honestly: I had thought it would become more difficult to get up to speed again. That is a good sign.

## May 2026
 
 
### 2026-05-19...2026-06-17

long break due to vacation and business trips. No progress.

### 2026-05-18

- Today just research, no programming or specifying.
- Found old Sterne und Weltraum (SuW) journals with Events of the Month.
- Took Picture of the tables, CGPT digitalized the table without information loss
- Huge field of certified experiments for enhancing the experiment data base
- Scanned Ground Truth providers in the evening: found USNO as a GT for Sidereal Time, USNO also for Julian Date (nice opportunity to get test data), Miriade for Ephemeris (INPOP), but also for Rise,Transit,Set and Visibility (!). THis is huge, because it ideally combines celestial observation planning with Astronometria.
- IERS is a universal GT provider for time and Earth Rotation

With these GT providers, certification of an ObservationScene (Observer -> Target, at time) is possible. I would say: for all necessary quantities there are GroundTruth Providers which we can combine in AstronoSphere to get trustworthy, certified astronomical data.

```text
ObservationScene
=
Target dynamics
+
Observer dynamics
+
Time/Earth rotation truth
+
Measurement semantics
```

Basic essence of today:

> **AstronoSphere can become
a deterministic orchestration layer
for independent astronomical truth systems.**


### 2026-05-17

Intensive Architecture Session about TDB/TT Mapping, MeasurementDefinition for GroundTruth and Astronometria - very strong. Very long. Very happy. :-)

Vor allem, weil heute mehrere echte Architektur-Knoten geplatzt sind:

MeasurementDefinition als kanonische Semantikschicht
klare Trennung:
Measurement
Mapping
SimulationModel
StateTree
GEO-EQU sauber integriert
TimeScale vs TimeDomain endlich konsistent
PhysicsStateTree stabilisiert
ObserverWorld architektonisch vorbereitet
deterministische TerminalNode-Auflösung
SinglePath-Regel
KISS-first Runtime-Modell

Und der wichtigste Punkt:

MeasurementDefinition
→ gemeinsame Sprache zwischen
GroundTruth und Simulation

Das war heute vermutlich der eigentliche Durchbruch.

### 2026-05-16

- Even more extreme Promotion procedure to promote simulations from Run to Baseline
- very well structured and documented
- but again high accuracy level cost a lot of energy
- very accurate cleanup and milestone preparation, also GIT commit, tag, merge and new feature branch very diligently
- FINALIZED M2.3 - very happy with the result :-)

### 2026-05-15

- Extremely diligent and fruitful validation session for M2.3
- a lot of findings, everything explainable, everything finally resolved.
- very exhausting session, but happy having succeeded :-)

### 2026-05-14

- implementation of M2.3 according to Spec 

### 2026-05-06 to 2025-05-13

- intensive spec phase for Astronometria, e.g.
  - productive runs: ScientificRun, ExplorationRun and StatisticalRun were born
  - ExplorationRun with GroundTruth Call
  - Definition of json file format for Simulation results

### 2026-05-05
- implemented CatalogExperiments, statiscial analysis showed no new effects 
- identified, that Ground Truth Data for geo-quatorial experiments were generated falsely -> deprecated Ground Truth Folder introduced.
- Result: 156 high quality tests, 139 passed, 17 fail but are analyzed and explainable
- cleaned up former Astronometria tests
- finalized GIT and closed M2.2

### 2026-05-04
- implemented MVH2 and MVH3
- statistical analysis shows that MVH3 accuracy is deteriorating for times before year 0 (BC). Great result, because it shows the limits of VSOP for the inner planets, where we have also Horizons data.
- a great day! We are transitioning now to scientific working.

### 2026-05-03
- implemented Holy12 Tests based on 02_Experiments/Released und 04_GroundTruth/.../Baseline
- conducted simple deviation analysis to confirm proper implementation of tests
- implemented Mesh Tests MVH1 for validation in Horizons
- first statistical analysis of MVH1 - very interesing. Entered Scientific mode after weeks in Architecture, Spec and Implementation mode. :-)
- Pending: MVH2, MVH3

### 2026-05-02
- implemented AstronoDiag
- created all experiments with the updated TruthFactory
- reached Milestone M2.1 today

## April 2026

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
- began implementing new data storage as per documentation policy 
- implemented MeshGenRunner to generate Mesh SeedsPrepared. Created excellent Specification documents and also a fabulous test spec for AstronoLab, wher MeshGenRunner.cs resides. Tests were without failure. 
- Applied new files to AstronoCert thereafter and identified spec failure one step earlier in AstronoTools for the MeshGenerator. Root cause was a weak and nasty spec. -> Back to MeshGenerator 
- Other than that: the MeshGenRunner was one of the strongest ever. Excellent collaboration with CGPT. - will fix the MeshGenerator later. Have to stop now with existing bug but with known root cause, fix strategy and back2back validation plan. Could be worse.

### 2026-04-26
- eliminated bug in MeshGenerator: 229 experiments now correctly generated in AstronoCert
- introduced AstronoDiag as the global system diagnosis instance of AstronoSphere.
- Developed good spec for AstronoDiag for M2.1, structured VS project diligently, created Validation Spec as well --> perfectly prepared for next implementation session
> **Best of this week: stable work mode established: Idea -> Chat Discussion -> Spec -> Chat Discussion (% Fulfillment) -> Validation Spec -> Chat Discussion (Iteration) -> Freeze -> Implementation**

### March 2026

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


### February 2026 


#### 2025-02-03
- idea of developing a program which provides data from JPL Horizons
- Program called Ephemeris Regression (ER)

#### 2025-02-10
- spent a compelte Saturday to refactor the time domain: threw out UTC out of AstroDomain

#### 2025-02-17
- created code to generate two test suites TS-A, -B. EventDetection included in ER
- added TS-D: Mesh to evaluate VSOP
- created simple statistical report about VSOP vs. DE440




### January 2026

- year begin with a nice grafical tool Sternkarten showing the heliocentric positions of the planets according to the Sterne und Weltraum monthly pages
- began with a first grafical representation of Astronometria in C# and WPF: 
    - included Bright Star Catalog Data and created first "Drehbare Sternscheibe" Code with constallations and various set of starts. Also added star colors according to Spectral Type.
    - decided to "add the planets quickly"
    - decided to postpone visualization of the planets "for a bit" to validate VSOP data first
- first thoughts about a Ground Truth generation with Horizons
- decision to develop Astronometria with Chat GPT




------------------

# Annex

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

