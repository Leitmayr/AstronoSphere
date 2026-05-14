# SPEC_M2.3-4__StateMachine.md

## Status


Freeze Status: FROZEN  for M2.3/M2.4
Scope: Astronometria / AstronoSphere M2.3–M2.4  

Needs extension for M2.4+


## Change log 

| Revision | Changes | Date |
| -------- | ------- | ---- |
| V1.0 | - intitial revision before implemenation of M2.3  | 2026-05-14

---

# 1. Motivation


Astronometria is not only an engine that computes positions.

It is intended to become the interactive access layer to AstronoSphere.

AstronoSphere provides:

* certified experiments
* GroundTruth
* simulation containers
* deterministic pipeline behavior
* reproducibility through hashes
* validation infrastructure

Astronometria provides:

* computation
* model comparison
* physical correction levels
* visual exploration
* future developer sandbox functionality
* intermediate results stored for later statistical evaluation

To fulfill this role, every computed position must be traceable to the exact state that produced it.

A plotted position must be more than a visual marker.
It must be a reproducible scientific statement.


This specification defines the emerging state-machine architecture required for Astronometria to produce reproducible, replayable, hash-verifiable simulation datasets inside AstronoSphere.

The immediate driver is M2.3:

```text
Simulation results must be written into AstronoData with a meaningful DatasetHeader.
```

However, the DatasetHeader cannot be defined correctly without first defining the state semantics behind a simulation result.

Therefore, the state concept of M2.4 must be specified before the final M2.3 Simulation DatasetHeader is implemented.

This is not a milestone reorder.

It is a specification dependency:

- State semantics first.
- DatasetHeader second.
- Simulation output third.



---

# 2. Terminology

## 2.1 Observer

An observer is a location from which celestial objects are being observed.

Examples:
- earth (=center of the earth)
- topocentric coordinates such like 50°N, 10°E, 0km 

## 2.2 Target

A target is celestial object which is being observed by an observer.

Examples:
- Venus
- Jupiter

## 2.3 State

A State is the input-side description of a concrete computation point.
It defines what shall be computed.
A State must be canonicalizable. 
A State must be sufficient to identify the intended computation at a specific point in the StateGraph.

A State may include:

* StateNodeId
* target
* observer
* engine
* engine model
* measurement level (e.g. L0, L1, L2)
* frame
* epoch
* time scale
* observation time
* output type
* units
* numerical policy
* relevant algorithm parameters

## 2.4 Data

Data is the output-side result produced from a State by executing a Transition.
Data contains computed quantities.
Data must not define what shall be computed.
Data only records what was computed.

Examples:
* vector coordinates
* spherical coordinates
* apparent coordinates
* light-time values
* iteration diagnostics, if applicable later
* final observable quantities


## 2.5 StateHash

StateHash is the canonical hash over State only.
It identifies the input-side computation state.
StateHash answers: what exact computation state was entered?

## 2.6 DataHash

DataHash is the canonical hash over Data only.
It identifies the output-side computed result.
DataHash answers: what exact result was produced?

## 2.7 StateNode

A StateNode is a conceptual persisted-capable node.
A StateNode represents a computational state after the relevant Transition has produced Data.
A StateNode is not an algorithm.
A StateNode is a state in the graph.

## 2.8 Transition

A Transition is a deterministic algorithmic step that transforms one StateNode into another StateNode.
In code, a Transition typically corresponds to a method or computation step.

Examples:
* compute VSOP heliocentric geometric vector
* transform heliocentric vector to geocentric vector
* apply light-time correction
* apply aberration correction
* project to observer coordinates

A Transition has:
* TransitionId
* FromNodeId (e.g. StateNodeID of input State)
* ToNodeId (e.g. StateNodeID of output State)
* algorithm name
* required input type
* produced output type

A Transition does not require its own hash in M2.4.

---

## 2.9 StateGraph

A StateGraph is a directed ordered graph of StateNodes connected by Transitions.
It describes the allowed and executed computation path for an ObservationScene.

A StateGraph contains:
* StateNodes
* Transitions
* execution order
* TerminalNode references

The StateGraph must be replayable from JSON.
The StateGraph must be deterministic.


## 2.10 TerminalNode

A TerminalNode is a StateNode that produces a final dataset output of an ObservationScene.

A TerminalNode is the endpoint of a selected StateGraph path whose Data is intended to be:

* persisted as SimulationData
* visualized in Astronometria
* compared against GroundTruth
* passed to Astronolysis

A StateNode becomes a TerminalNode by role in a selected execution path, not by its intrinsic computation type.

Example:

* an L0 geocentric vector node may be a TerminalNode in an L0 simulation
* the same conceptual node may be an intermediate node in an L1 path

Rule:
TerminalNode is not a node class.
It is the declared output role of a StateNode within a specific ObservationScene execution.

Every SimulationRun ends in exactly one TerminalNode per target path.
The execution pipeline treats the selected TerminalNode as evaluation output.

## 2.11 SceneContext

SceneContext is the shared context for all StateNodes in one ObservationScene.
It defines the common observational frame of the scene.

For M2.4, the only supported temporal mode is:
TemporalMode = ObservationTime
Advanced time anchoring is deferred.

## 2.12 ObservationScene

An ObservationScene is a multi-target computation scenario.
An ObservationScene defines what is computed as a complete scene.

The scene may contain multiple targets, such as Venus and Jupiter, observed from the same observer at the same observation time.

## 2.13 SimulationCore

SimulationCore is the persisted identity block of an ObservationScene inside a Simulation DatasetHeader.

It contains:
* SceneContext
* StateGraph definition
* StateNodes
* Transitions
* TerminalNode references
* StateHashes
* DataHashes
* validation metadata for TerminalNodes

SimulationCore is the simulation-side counterpart to Experiment.Core.

Difference:
- Experiment.Core defines the physical experiment identity.
- SimulationCore defines the computational scene identity.


## 2.14 SimulationRun

A SimulationRun is the deterministic execution of one ObservationScene over a defined time interval.

SimulationRun
    TargetSimulations[] (list of targets to be simulated)
        Samples[] (time instants for which the run is carried out)
            StateNodes[] (chain of nodes from the EntryNode to the TerminalNode)


Each of the following is a simulation run.


### 2.14.1 ScientificRun

Engine execution, where AstronoSphere Experiments are being processed for exactly one unique SceneContext. 

> ScientificRun = constrained SimulationRun

### 2.14.2 ExplorationRun

Engine execution, where simulation settings are selected via GUI in the Exploration Mode. Based on the settings, a flexible simulation run for one or more targets in  exactly one unique SceneContext can be carried out. 

> ExplorationRun = flexible SimulationRun


### 2.14.3 StatisticalRun

Instrumentation mode of a SimulationRun that additionally persists intermediate StateNode Data.

## 2.15 Compact summary

> **ObservationScene**: decribes

> **SimulationRun**: executes 

> **Scientific/Exploration/Statistical**: classify

> **StateGraph**: defines paths

> **TargetSimulation**: concrete target branch


---

# 3 Astronometria Modes

Astronometria is the execution entity of astronomical simulations in AstronoSphere. Astronometria support three major operation modes. 

## 3.1 Scientific Mode 
This is the scientific Use Case to process Experiments in a **ScientificRun**. Both data types are stored in AstronoData and base on well defined astronomical experiments. 
In scientific mode, well selected and certified high quality data are being processed:

 
> Experiment Catalog released data + GroundTruth Baselined data -> Scientific Run & evaluation in Astronolysis


Examples for these data are mesh data, but also important astronomical events. 

> Note: A Scientfic Mode knows about its identity ("I am supposed to execute a ScientificRun now") from the input: an Experiment and its corresponding GroundTruth .

> Note: M2.3 and M2.4 support only Scientific Mode.

## 3.2 Exploration Mode 

This is the intuitive Use Case which represents the key to enter the AstronoSphere ecosystem for the User. Exploration Mode is used  to generate Observation Scenes by means of the GUI or CLI (GUI development much later than M2.4). One Observation Scene Selection ("Venus and Jupiter at JD X in state L1") triggers one SimulationRun with multiple targets. 
The future Astronometria Ephemeris Engine will be a StateGraph. Every StateNode in this StateGraph is generating a unique and reproducable intermediate result. The **TerminalNode** contains 
- the final data (StateVector) of an **ExplorationRun**
- the reference data shall be determined from the selected Truth provider (e.g. JPL Horizons). 
- the delta between Astronometria StateVector of the TermintalNode and the Ground Truth shall be calculated.

Results are being visualized in a map in Astronometria or are stored on disk. 
Astronolysis can compare and further process the StateVectors (=Data) of the TerminalNode, the corresponding GroundTruth data and also their deviations.

 

> Note: An Exploration Mode knows about its identity ("I am supposed to execute and ExplorationRun now") from the caller: GUI.

> Note: M2.3 and M2.4 will not support Exploration Mode.

## 3.3 Statistical Mode 

This is a special configuration of both Scientific Mode and Exploration Mode. 
In addition to the data of the TerminalNode, StatisticalRuns store all or a portion of the the generated intermediate StateNode results on disk as well. For example, the user might decide to save 20% of the data of every intermediate StateNode. Rationale of this mode is to acquire more data and to faster build up a stastical basis of all conducted Astronometria Simulations over time. 
The statistical data basis may later be used to generate Uncertainty of ErrorModels by means of Astonolysis. The statistical data base may also be used to determine the uncertainty range for Monte Carlo simulations.

> Note: A Statistical Mode knows about its identity ("I am supposed to execute and StatisticalRun now") from the caller: GUI with additional selection of "Statistics".

> Note: M2.3 and M2.4 will not support Statistical Mode.

# 4 Data structure

## 4.1 SimulationData

SimulationData is persisted in a json-File containing all information of a simulation run. 

One data structure definition for SimulationData shall cover both ScientificRun and ExplorationRun results.

For M2.3 and M2.4, only ScientificRuns shall be stored.



**Scientific:**
```text
Targets.Count == 1
InputOrigin == CertifiedExperiment
CatalogNumber == AS-xxxxx
RunType == ScientificRun
```

> Targets.Count == 1 means, that an ObservationScene with one target is processed.

**Exploration:**
```text
Targets.Count >= 1
InputOrigin == ObservationScene
CatalogNumber == null / absent
RunType == ExplorationRun
```

A SimultionRun can be any of these Runs: ScientificRun, ExplorationRun, StatisticalRun.
Astronometria will generate TerminalNode data, Observation Scene data and in Statistical Mode also intermediate StateNode data. 
These data will be arranged as follows in the json SimulationData file. 

**Structure** 

```text
SimulationData
{
  RunClassification{   }
  ExperimentRef  {   }
  Measurement  {   }
  GroundTruth   {   }
  Engine  {   }
  ObservationScene
  {
    SceneContext {    }
    TargetSimulation
    {
      Target1
      TerminalNode1 {   }
    }
    TargetSimulation
    {
      Target2  
      TerminalNode2 {   }
    }
    ...
    TargetSimulation
    {
      TargetN
      TerminalNodeN {   }
    }
  }
}
```
Only data from the TerminalNode shall be stored.
No transition data shall be stored.


## 4.2 RunEnvironment

RunEnvironment example data are taken from real data set AS-000015.

### 4.2.1 Example for RunClassification

```text 
  "RunClassification": 
  {
    "RunType": "ScientificRun",
    "InputType": "CertifiedExperiment",
    "TargetCardinality": "SingleTarget"
  }
```

### 4.2.2 Example for ExperimentRef

```text
  ExperimentRef: 
  {
    "CatalogNumber": "AS-000015",
    "ExperimentID": "HELIO-J2000-TDB-2486286-2486288-1H",
    "CoreHash": "8788B6C1"
    "SourceFile": "AS-000015__PLANET-URANUS-QCR__HELIO-J2000-TDB-2486286-2486288-1H.json"
  }
```

### 4.2.3 Example for Meaurement

```text
  "Measurement": {
    "Domain": "Ephemeris",
    "Instrument": "VECTORS",
    "CorrectionLevel": "L0",
    "TimeScale": "TDB"
  }
  ```

### 4.2.4 Example for GroundTruth

   ```text
  "GroundTruthRef": {
    "Provider": "Horizons", -> dervied from FactorMetadata.Source
    "DatasetID": "HELIO-J2000-TDB-2486286-2486288-1H__EPH-HORIZONS-VEC-L0",
    "RequestHash": "39FB32DF1BC5C7E713823374520DE92223491BC65E5DD65857F0DFD97DB89B72",
    "SourceFile": "PLANET-URANUS-QCR__HELIO-J2000-TDB-2486286-2486288-1H__EPH-HORIZONS-VEC-L0.json"
  }
  ```

### 4.2.5 Example for Engine

```text
"Engine": {
    "Name": "Astronometria"
    "SimulationModel": 
    {
      "Family": "VSOP",
      "Type": "VSOP87A"
      "Truncation": "VSOP Meeus"
    }
    "Build":{  
      "GITCommit": "<GIT-Hash>"
      "GitBranch": "feature/M2.3-SimulationIntegration"
    }
  
}
```

The RunEnvironment shall be canonicalized and a RunEnvironmentHash shall be generated.


## 4.3 ObservationScene

An ObservationScene contains:
* one SceneContext
* one or more targets
* one or more TerminalNodes


#### 4.3.1 SceneContext

For M2.3 and M2.4 SceneContext includes:
* observation time [Julian Date minimum accuracy: 9 digits]
* time scale [TT, TDB: default TDB]
* observer [Earth: default "geocentric"]
* frame [default Ecliptic]
* epoch [default J2000]
* instrument [default VEC]
* default measurement domain [default "Ephemeris"]

> Note: in brackets [] are the default values for Milestones M2.3 and M2.4.

#### 4.3.2 TargetSimulation

For M2.3 and M2.4 the result of an TargetSimulation contains:


##### 4.3.2.1 Target

The target for which the SimulationRun is being carried out.

Example: Venus

##### 4.3.2.2 Terminal Node:

* NodeId
* NodeType
* State
* StateHash
* Data
* DataHash
* NodeRole
* execution status

NodeId contains the target.


State contains 

```text
{
  Target 
  Engine [default Astronometria]
  SimulationModel.Family
  VEC_CORR
  Units [AU]
}
```


StateHash calculation see below.

```text
Data
{
      "Position": {Vector3, accuracy at least 9 digits}
      "Velocity": {Vector3, accuracy at least 9 digits}
}
```


> Note: Data.Velocity is {0.0, 0.0, 0.0} for M2.3 and M2.4.

> Note: after M2.4 and later, the Data representation can also be e.g. 

```text
Data
{
      "RA": {degrees in [0, 360), accuracy at least 9 digits}
      "DEC": {degrees in [0, 360), accuracy at least 9 digits}
}
```


DataHash calculation see below.

NodeRole: example is "TerminalNode"

Status is computation status: set to "Completed" if state calculation is finished.


---



# 5. Conceptual Architecture

## 5.1 Astronometria StateGraph Characteristics

The Astronometria StateGraph is a **directed StateTree**: that means, only one path exists from source to leaf. In other words, there is exactly one path through the StateGraph to the TerminalNode node. Intermediate nodes are fully determined by knowing the TerminalNode.

## 5.2 Graph Semantics 

The directed graph consists of states and transitions.

```
StateNode A
  StateHash_A
  DataHash_A

-- Transition -->

StateNode B
  StateHash_B
  DataHash_B
```

The Transition is the algorithm.

The StateNode is the persisted state.

The graph is therefore not a collection of methods.

It is a collection of states connected by deterministic algorithmic transitions.

---

## 5.3 Determinism Semantics

Determinism is validated by comparing ordered StateNodes.

Run == LastRun is true if:

For every ordered StateNode:
Run.StateHash == LastRun.StateHash
AND
Run.DataHash == LastRun.DataHash

This provides both:

* local node-level diagnosis
* full scene-level replay verification

---

## 5.4 Potential Sequence


CLI
  → Astronometria.Runner
  → SimulationPlanner
      → load Experiment by CatalogNumber
      → resolve exactly one GT Baseline
      → derive TerminalNode
      → resolve immutable StateGraphPath
  → execute SimulationLine
  → write Scientific Simulation JSON
  → DiagLog on resolution error

## 5.5 Transition Execution Semantics

The execution sequence for a Transition is:

1. Read input StateNode
2. Build output State
3. Compute output StateHash
4. Execute deterministic Transition algorithm
5. Produce output Data
6. Compute output DataHash
7. Persist output StateNode

A StateNode is formally completed only when both StateHash and DataHash exist.

---


## 5.6 Multi Target Execution

For Multi Target Run (> M2.4):

All simulation outputs are target-major. That means, all time points of one target are processed and outputs are calculated. Then all points are calculated with the next target, etc.

Rational: 
- standardized for Scientific, Exploration and Statistical
- similar to Horizons
- better in terms of simulation duration
- no mode change
- Scientific is special case with Targets.Count == 1

For scene logic (multiple targets at the same time instant):

SceneGrabber
= read target-major data
= extract all selected targets at time t or interval [t1,t2]
= build transient SceneState


## 5.7 Input data determination

In Scientific Mode, Astronometria must read data from two sources:

1) Experiment
2) Corresponding GroundTruthData

For this, in a first step, Astronometria must scan the GroundTruthData files for the ExperimentID of the Experiment to find the corresponding file.
The result must be exactly one corresponding file.
How to treat deviations (no file found or two files found) -> see Chapter "Diagnostics".


## 5.8 Git Commit Determination

At startup, the following command is executed:

```text
git rev-parse --short HEAD
```

and the return is written to a file 

```text
buildinfo.json
```

stored in solution-root: 

```text
AstronoSphere/
    buildinfo.json
```

Astronometria reads the GIT commit ID from there and writes it to the json file specified above.

---

# 6. Hash Boundaries

## 6.1 StateHash Includes

StateHash includes the canonical form of State.

Typical fields:

* NodeId
* NodeType
* target
* observer
* engine
* SimulationModel.Family
* engine version, if required
* measurement level
* frame
* epoch
* time scale
* observation time
* units
* numerical policy
* algorithm-relevant parameters

---

## 6.2 StateHash Excludes

StateHash excludes:

* Data
* DataHash
* timestamps
* file names
* output paths
* DatasetHeader metadata
* generated-at values
* comments
* execution duration
* log messages

---

## 6.3 DataHash Includes

DataHash includes the canonical form of Data.

Typical fields:

* computed coordinates
* computed derived values
* correction-specific result values
* result units
* result arrays, if applicable

For time-series datasets, DataHash may cover the complete ordered data series of a StateNode.

---

## 6.4 DataHash Excludes

DataHash excludes:

* State
* StateHash
* timestamps
* file names
* output paths
* log messages
* execution duration

---


# 7. Input- and Output Data Storage

## 7.1 Scientific Mode

### 7.1.1 Input

Experiments: AstronoData\02_Experiments\Released
GrountTruth: AstronoData\03_GroundTruth\Ephemeris\Horizons\Baseline

### 7.1.2 Output

```text
04_EngineData/
    Scientific/
        Ephemeris/
            Astronometria/
                Run/
                LastRun/
                Baseline/
```

### 7.1.3 File Naming

Prefix AS-xxxxxx, according to Experiment.

```text
AS-XXXXXX__PLANET-MERCURY-INC__HELIO-ECL-J2000-TDB-2451545-2451546-1H__VSOP87-VEC-L0.json
```

```text
AS-XXXXXX
→ certified identity
```

```text
PLANET-MERCURY-INC
→ human meaning
```

```text
HELIO-ECL-J2000-TDB-...
→ experiment/state definition
```

```text
VSOP87-VEC-L0
→ simulation/measurement realization
```

## 7.2 Exploration Mode

### 7.2.1 Input

Experiments/SceneSelection: User Selection from GUI
GroundTruth is not pre-resolved from AstronoData.
GroundTruth may be requested dynamically from the selected provider during execution.

## 7.2.2 Output

```text
04_EngineData/
    Exploration/
        Astronometria/
            Scenes/
            Results/
```

## 7.2.3 File Naming

ExplorationRun filename:

```text
EXPLORE__<*TARGET_LIST*>__<EXPERIMENT_FRAME_TIME>__<SIMULATION>.json
```

Example:
```text
EXPLORE__VEN-JUP__HELIO-ECL-J2000-TDB-2451545-2451546-1H__VSOP87-VEC-L0.json
```

<*TARGET_LIST*>:
- "-" separated target abbreviations
- planets use fixed 3-letter abbreviations:
  MER, VEN, EAR, MAR, JUP, SAT, URA, NEP
- order follows increasing mean solar distance:
  MER → VEN → EAR → MAR → JUP → SAT → URA → NEP
- non-planet bodies use temporary placeholders:
  BD1, BD2, ...
- full body identity MUST be stored in the JSON header

## 7.3 Stastical Mode

StatisticalRun is an instrumentation mode of a SimulationRun.

### 7.3.1 Input

Experiments/SceneSelection: User Selection from GUI
GroundTruth: not applicable - will be requested by GT provider directly during run


### 7.3.2 Output

```text
04_Simulations/
    Statistical/
        Astronometria/
            StateSampling/
            Aggregates/
            Raw/
```

### 7.3.3 File Naming

to be specified.

# 8. Node Naming

## 8.1 Node Type

```text
EngineConfig.Physics_Correction.Origin.Plane.RefSystem.TimeScale.Output
```

- EngineConfig, e.g. VSOP87, MEEUS: which model is it from? In which context is this StateGraphRunning
- Physics_Correction, e.g. L0: describes which row in the StateGraph
- Origin+Plane, e.g. HELIO-ECL, GEO-EQU: define, that the left column in the picture is taken
- RefSystem, e.g. J2000 or OfDate: time reference, potential side branch
- TimeScale, e.g. TT or TDB: potential side branch 
- Output-Format, e.g. VEC or RA/DEC: describes the output format

Standard main branch is J2000, TDB. 

> Note: Alternative time bases or Ref_System can be side branches from the main branch

Example for node naming:
```text
Node 1:  VSOP87.L0.HELIO.ECL.J2000.TDB.VEC
```

NodeType must not contain a target.

> Note: NodeType answers the question "which calculation step?"



## 8.2 NodeID

```text
"NodeId": "VEN_NODE_001"
```

VENUS is the target and NODE_1 could be a reference.

NodeID must contain the target.

> Note: NodeID answers the question "which concrete instance of the run?"


## 8.3 List of all relevant nodes vor M2.3/M2.4

NodeTypes:

1) VSOP87.L0.HELIO.ECL.J2000.TDB.VEC
2) VSOP87.L0.GEO.ECL.J2000.TDB.VEC
3) VSOP87.L1.HELIO.ECL.J2000.TDB.VEC
4) VSOP87.L1.GEO.ECL.J2000.TDB.VEC
5) VSOP87.L2.HELIO.ECL.J2000.TDB.VEC
6) VSOP87.L2.GEO.ECL.J2000.TDB.VEC





# 9. Diagnostics

ScientificRun requires deterministic GroundTruth resolution. Astronometria scans the GroundTruth files for the ExperimentID of the corresponding Experiment.

Rules to maintain scientific integrity:

```text
GT lookup result count == 1
→ proceed

GT lookup result count == 0
→ AstronoDiag

GT lookup result count > 1
→ AstronoDiag
```

To cover these cases count unequal to 1, a new Diag-Familiy must be introduced:

```text
040.xxx
ScientificRun resolution errors
```

Examples:

```text
040.001
No matching GroundTruth dataset found

040.002
Multiple matching GroundTruth datasets found
```


# 10. Single Target Example: Venus L0 at Observer Time

## 10.1 Scenario

The example ObservationScene contains:

* Observer: Earth
* Targets: Venus
* ObservationTimeJD: 2451545.0
* TimeScale: TDB
* Frame: Ecliptic
* Epoch: J2000
* Engine: Astronometria
* EngineModel: VSOP87
* Level: L0
* Instrument: VEC

The objective is to compute L0 geocentric vectors for Venus at observation time.

---

## 10.2 Conceptual StateGraph

```text
ObservationScene
|
|-- Venus path
    |
    |-- StateNode V1: VSOP heliocentric geometric vector
    |
    '-- StateNode V2: geocentric geometric vector [TerminalNode]
```

**Transitions:**

- Transition T1:
ComputeVSOPHeliocentricVector

- Transition T2:
TransformHeliocentricToGeocentric

The StateNodes are target-specific instances.

---

## 10.3 Example JSON Draft




# End of Draft




