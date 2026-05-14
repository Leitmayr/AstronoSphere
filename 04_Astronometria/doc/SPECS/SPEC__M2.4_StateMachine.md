# SPEC_M2.3-4__StateMachine.md

## Status

Draft  
Freeze Status: NOT FROZEN  
Scope: Astronometria / AstronoSphere M2.3–M2.4  
Context: Simulation DatasetHeader, SimulationCore, StateGraph, State Machine Architecture  
Language: English documentation draft  

---

# 1. Terminology

## 1.1 State

A State is the input-side description of a concrete computation point.
It defines what shall be computed.



# 1. Motivation

## 1.1 Purpose

This specification defines the emerging state-machine architecture required for Astronometria to produce reproducible, replayable, hash-verifiable simulation datasets inside AstronoSphere.

The immediate driver is M2.3:

```text
Simulation results must be written into AstronoData with a meaningful DatasetHeader.
````

However, the DatasetHeader cannot be defined correctly without first defining the state semantics behind a simulation result.

Therefore, the state concept of M2.4 must be specified before the final M2.3 Simulation DatasetHeader is implemented.

This is not a milestone reorder.

It is a specification dependency:

- State semantics first.
- DatasetHeader second.
- Simulation output third.


---

## 1.2 Engine Modes

An Engine is an executable of astronomical simulations. In the following, the engine will be calls Astronometria. Astronometria has three major execution modes, covering three different use cases.

### 1.2.1 Scientific Mode 
This is the scientific Use Case to process Experiments and their corresponding Ground Truth data. Astronometria Execution in Scientific Mode be called "ScientificRun". Both data types are stored in AstronoData and are basing on well defined astronomical experiments. Examples for these data are mesh data, but also important astronomical events. In scientific mode, well selected and certified high quality data are being processed:

Eperiment Catalog released data + GroundTruth Baselined data -> Scientific Run & Evaulation in Astronolysis

### 1.2.2 Experimental Mode 
This is the intuitive Use Case which represents the key to enter the AstronoSphere ecosystem for the User. Experimental Mode is used  to generate Observation Scenes by means of the GUI or CLI (GUI development much later than M2.4). These Observation Scene Selections ("Venus and Jupiter at JD X in state L1") are triggering  Astronometria Executions called "ObservationSceneRun". An ObservationSceneRun may consist of one or more ExperimentalRuns, depending on how many targets had been selected for the ObservationScene.
The future Astronometria Ephemeris Engine will be a StateGraph. Every StateNode in this StateGraph is generating a unique and reproducable intermediate result. Exactly one StateNode contains the final results (StateVector) of an ExperimentalRun. This Statenode be called TerminalNode. For the TerminalNode, in addition to the generated result, the reference data shall be determined from the selected Truth provider (e.g. JPL Horizons). Also the delta between Astronometria StateVector of the TermintalNode and the Ground Truth shall be calculated.
Since multiple Targets may be processed, multiple ExperimentalRuns may be needed to cover a complete ObservationScene. 
Results are being visualized in a map in Astronometria or are stored on disk. 
Astronolysis can compare and further process the StateVectors (=Data) of the TerminalNode, the corresponding GroundTruth data and also their deviations.

Note: Every User Seleciton in Experimental Mode can be considered an Experiment. Hence, an ObservationRun can also be interpreted as one ore more Experiments which are simulated by the Engine. 

### 1.2.3 Statistical Mode 
This is a special configuration of both Scientific Mode and Experimental Mode. The Astronometria execution in Statistical Mode be called StatisticalRun.
In addition to the data of the TerminalNode, StatisticalRuns store all or a portion of the the generated intermediate StateNode results on disk as well. For example, the user might decide to save 20% of the data of every intermediate StateNode. Rationale of this mode is to acquire more data and to faster build up a stastical basis of all conducted Astronometria Simulations over time. 
The statistical data basis may later be used to generate Uncertainty of ErrorModels by means of Astonolysis. The statistical data base may also be used to determine the uncertainty range for Monte Carlo simulations.


## 1.3 Data structure of SimulationDat

SimulationData is a json-File containing all information of a simulation run A simulation run can be any of these Runs: ScientificRun, ObservationSceneRun, ExperimentalRun, StatisticalRun.
Astronometria will generate TerminalNode data, Observation Scene data and in Statistical Mode also intermediate StateNode data. 
These data will be arranged as follows in the json SimulationData file. 

SimulationData
{
  RunEnvironment
  {

  }
  ObservationScene
  {
    SceneContext
    {

    }
    ExperimentalRun
    {
      Target1
    }
    ExperimentalRun
    {
      Target2  
    }
    ...
    ExperimentalRun
    {
      TargetN
    }

  }
}


### 1.3.1 RunEnvironment

Information which are common for all StateNodes in one SimulationRun, independent of the execution mode:
- SimulationEngine: 
    SimulationEngine.Provider: Example "Astronometria"
    SimulationEngine.ID: Example <GIT-Hash> -> describes which SW-Version has created the data
- SimulationModel:
    SimulationModel.Family: Example "VSOP"
    SimulationModel.Type: Example "VSOP87A", "VSOP87E"
    SimulationModel.Truncation: Example "VSOP Meeus"
- Ground Truth:
    GroundTruth.Provider: Example "JPL Horizons", "Miriade", ...
    GroundTruth.ReferenceEphemeris: Example "DE440"

The RunEnvironment shall be canonicalized and a RunEnvironmentHash shall be generated.


### 1.3.2 ObservationScene

An ObservationScene contains exactly one Scene Context. 
An ObservationScene contains at least one ExperimentRun for one Target and may contain more ExperimentRuns for additional targets as well. 

#### 1.3.2.1 SceneContext





## 1.2 Architectural Motivation

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
* intermediate results stored for later statistical evaulation

To fulfill this role, every computed position must be traceable to the exact state that produced it.

A plotted position must be more than a visual marker.

It must be a reproducible scientific statement.

---

## 1.3 Statistical 



---

## 1.3 Core Principle

The central principle of this specification is:


Every computed result must be state-backed.


This means:

* the input state is explicit
* the algorithmic path is explicit
* the output data is explicit
* both state and data are hashable
* the result can be replayed from JSON
* deterministic behavior can be verified

The system should support the statement:


Same StateHash + same DataHash
= same computation state and same computed result.


---

## 1.4 Scientific Motivation

Astronometria should eventually support scientific use cases such as:

* compare Meeus, VSOP87, and Horizons positions for the same target
* compare L0, L1, and L2 correction levels
* visualize planetary conjunctions against a real star background
* reload a saved scene and prove that it still computes identically
* compare final simulation states against GroundTruth

This requires a state model that is:

* deterministic
* serializable
* replayable
* hashable
* compatible with GroundTruth mapping
* able to support multiple targets in one observation scene

---

## 1.5 KISS Boundary

The state-machine architecture must remain simple.

The system must not become a graph framework.

For M2.4, the objective is:

A deterministic state graph for L0-ready multi-target simulation scenes.

The following must be avoided unless explicitly required later:

* PathHash
* TransitionHash
* accuracy evaluation for every intermediate node
* advanced temporal anchoring
* GUI dependencies
* dynamic graph generation
* overly intelligent StateNode objects

The StateGraph must explain the computation path.

Only selected final output nodes carry scientific accuracy claims.

---

# 2. Scope

## 2.1 M2.4 Includes

M2.4 includes:

* L0-ready StateGraph
* multi-target ObservationScenes
* StateHash
* DataHash
* replay from JSON
* automatic Horizons fetch for TerminalNodes
* accuracy evaluation for TerminalNodes
* SimulationCore definition
* DatasetHeader foundation for M2.3 simulation outputs

---

## 2.2 M2.4 Excludes

M2.4 excludes:

* advanced TimeAnchor semantics
* reference target emission time
* cross-target temporal synchronization
* PathHash
* TransitionHash
* GUI implementation
* accuracy evaluation for every intermediate StateNode by default
* Astronolysis interpretation
* seed generation
* L1/L2 implementation

---

# 3. Atomic Terminology

This chapter defines the terminology from atomic concepts upward.

---

## 3.1 State

A State is the input-side description of a concrete computation point.

It defines what shall be computed.

A State contains no computed result.

A State may include:

* StateNodeId
* target
* observer
* engine
* engine model
* measurement level
* frame
* epoch
* time scale
* observation time
* output type
* units
* numerical policy
* relevant algorithm parameters

A State must be canonicalizable.

A State must be sufficient to identify the intended computation at a specific point in the StateGraph.

---

## 3.2 Data

Data is the output-side result produced from a State by executing a Transition.

Data contains computed quantities.

Examples:

* vector coordinates
* spherical coordinates
* apparent coordinates
* light-time values
* iteration diagnostics, if applicable later
* final observable quantities

Data must not define what shall be computed.

Data only records what was computed.

---

## 3.3 StateHash

StateHash is the canonical hash over State only.

It identifies the input-side computation state.

StateHash must not include:

* Data
* DataHash
* timestamps
* file names
* DatasetHeader
* generated-at metadata
* external storage paths

StateHash answers:

What exact computation state was entered?

---

## 3.4 DataHash

DataHash is the canonical hash over Data only.

It identifies the output-side computed result.

DataHash must not include:

* State
* StateHash
* timestamps
* file names
* DatasetHeader
* generated-at metadata
* external storage paths

DataHash answers:

What exact result was produced?

---

## 3.5 StateNode

A StateNode is a persisted node in the directed computation graph.

A StateNode contains:

* NodeId
* State
* StateHash
* Data
* DataHash
* NodeRole
* execution status

A StateNode represents a computational state after the relevant Transition has produced Data.

A StateNode is not an algorithm.

A StateNode is a state in the graph.

---

## 3.6 Transition

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
* FromNodeId
* ToNodeId
* algorithm name
* required input type
* produced output type

A Transition does not require its own hash in M2.4.

---

## 3.7 StateGraph

A StateGraph is a directed ordered graph of StateNodes connected by Transitions.

It describes the allowed and executed computation path for an ObservationScene.

A StateGraph contains:

* StateNodes
* Transitions
* execution order
* TerminalNode references

The StateGraph must be replayable from JSON.

The StateGraph must be deterministic.

---

## 3.8 TerminalNode

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


---

## 3.9 SceneContext

SceneContext is the shared context for all StateNodes in one ObservationScene.

It defines the common observational frame of the scene.

For M2.4, SceneContext includes:

* observer
* observation time
* frame
* epoch
* time scale
* instrument
* default measurement domain

For M2.4, the only supported temporal mode is:

TemporalMode = ObservationTime

Advanced time anchoring is deferred.

---

## 3.10 ObservationScene

An ObservationScene is a multi-target computation scenario consisting of:

* one SceneContext
* one StateGraph
* one or more targets
* one or more TerminalNodes

An ObservationScene defines what is computed as a complete scene.

The scene may contain multiple targets, such as Venus and Jupiter, observed from the same observer at the same observation time.

---

## 3.11 SimulationCore

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

Experiment.Core defines the physical experiment identity.
SimulationCore defines the computational scene identity.

---

# 4. Conceptual Architecture

## 4.1 Graph Semantics

The directed graph consists of states and transitions.

StateNode A
  StateHash_A
  DataHash_A

-- Transition -->

StateNode B
  StateHash_B
  DataHash_B

The Transition is the algorithm.

The StateNode is the persisted state.

The graph is therefore not a collection of methods.

It is a collection of states connected by deterministic algorithmic transitions.

---

## 4.2 Execution Semantics

Planetary sequnce in multi target runs: 1) Mercury, 2) Venus, ..., 8) Neptune
If multiple ExperimentRuns for the same target, e.g. Venus are executed, the sequence shall be:
L0, L1, L2
The sequence for the same target on the same Level is predefined by the rule: instrument/Frame dimension before L0, L1, L2 dimension.

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

## 4.3 Determinism Semantics

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

## 4.4 StateGraph vs ValidationGraph

StateGraph and validation must remain conceptually separate.

StateGraph describes the deterministic computation path.

Validation attaches only where scientifically meaningful.

Rule:

All StateNodes are replayable and hash-validated.
TerminalNodes are additionally truth-resolvable and accuracy-rated.

Intermediate StateNodes are not accuracy-rated by default.

---

# 5. Hash Boundaries

## 5.1 StateHash Includes

StateHash includes the canonical form of State.

Typical fields:

* NodeId
* target
* observer
* engine
* engine model
* engine version, if required
* measurement level
* frame
* epoch
* time scale
* observation time
* output type
* units
* numerical policy
* algorithm-relevant parameters

---

## 5.2 StateHash Excludes

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

## 5.3 DataHash Includes

DataHash includes the canonical form of Data.

Typical fields:

* computed coordinates
* computed derived values
* correction-specific result values
* result units
* result arrays, if applicable

For time-series datasets, DataHash may cover the complete ordered data series of a StateNode.

---

## 5.4 DataHash Excludes

DataHash excludes:

* State
* StateHash
* timestamps
* file names
* output paths
* log messages
* execution duration

---

## 5.5 Optional Future Hashes

The following are intentionally deferred:

* PathHash
* TransitionHash
* NodeHash

Reason:

Do not hash the path until the path itself becomes a problem.

For M2.4, ordered StateHash/DataHash pairs are sufficient.

---

# 6. Multi-Target Semantics

## 6.1 Principle

Multi-target scenes are part of M2.4.

Multiple planets may be represented inside one ObservationScene.

Example:

ObservationScene:
Observer = Earth
ObservationTimeJD = 2451545.0
Targets = Venus, Jupiter

Each target receives its own StateNodes and StateHashes.

---

## 6.2 Same Scene, Different Target States

Targets share the same SceneContext.

However, target identity is part of State.

Therefore:

Venus StateHash != Jupiter StateHash

even if all other parameters are identical.

The common relation is expressed by the shared ObservationScene and SceneContext, not by equal StateHashes.

---

## 6.3 Comparison Cases Supported by M2.4

M2.4 must support the following modelable cases:

### Case 1: Same target, different levels

>Venus L0 vs Venus L1

Semantics:

* different StateNodes or graph paths
* same target
* different measurement level
* different StateHashes

L1 implementation itself is not part of M2.4.

The structure must be prepared for it.

---

### Case 2: Different targets, same graph point and level

>Venus L1 vs Jupiter L1

Semantics:

* same conceptual graph node type
* different target instances
* different StateHashes
* same ObservationScene

---

### Case 3: Different targets, different levels

>Venus L0 vs Jupiter L1

Semantics:

* same ObservationScene
* different targets
* different levels
* different StateHashes

---

# 7. Temporal Scope

## 7.1 M2.4 Temporal Mode

M2.4 supports only:

TemporalMode = ObservationTime

All targets in the ObservationScene are referenced to the same observer observation time.

---

## 7.2 Deferred Temporal Modes

The following are deferred:

* TargetEmissionTime
* ReferenceTargetEmissionTime
* CustomTime
* cross-target temporal synchronization

Example deferred use case:

Display Jupiter and Venus at the time when the light left Venus.

Reason for deferral:

Advanced temporal anchoring depends on L1 semantics and must not be introduced before L1 is stable.

---

# 8. GroundTruth Resolution

## 8.1 Principle

TerminalNodes must be truth-resolvable in M2.4.

This means a TerminalNode can be mapped to a GroundTruth request pattern.

For Horizons, this requires:

* a mapping from TerminalNode semantics to Horizons API parameters
* a canonical Horizons request
* a GroundTruth dataset reference
* comparison of TerminalNode Data against GroundTruth Data

---

## 8.2 TruthMappingRef

StateNodes should not embed provider-specific API details directly.

Instead, TerminalNodes may reference a TruthMappingRef.

Example:

TruthMappingRef = HORIZONS.EPHEM.VECTORS.L0.GEOCENTRIC

The mapping service resolves this reference to provider-specific request parameters.

---

## 8.3 Accuracy Evaluation

Accuracy evaluation is performed for TerminalNodes only by default.

A TerminalNode may contain validation metadata such as:

* GroundTruthProvider
* GroundTruthDatasetId
* RequestHash
* Delta metrics
* AccuracyStatus

Intermediate nodes are not accuracy-rated unless explicitly requested in a future debug or research mode.

---

# 9. Example: Venus and Jupiter L0 at Observer Time

## 9.1 Scenario

The example ObservationScene contains:

* Observer: Earth
* Targets: Venus and Jupiter
* ObservationTimeJD: 2451545.0
* TimeScale: TDB
* Frame: Ecliptic
* Epoch: J2000
* Engine: Astronometria
* EngineModel: VSOP87
* Level: L0
* Instrument: VEC

The objective is to compute L0 geocentric vectors for both Venus and Jupiter at the same observation time.

---

## 9.2 Conceptual StateGraph


ObservationScene
|
|-- Venus path
|   |
|   |-- StateNode V1: VSOP heliocentric geometric vector
|   |
|   '-- StateNode V2: geocentric geometric vector [TerminalNode]
|
'-- Jupiter path
    |
    |-- StateNode J1: VSOP heliocentric geometric vector
    |
    '-- StateNode J2: geocentric geometric vector [TerminalNode]




Transitions:

Transition T1:
ComputeVSOPHeliocentricVector

Transition T2:
TransformHeliocentricToGeocentric

The same Transition types are used for Venus and Jupiter.

The StateNodes are target-specific instances.

---

## 9.3 Example JSON Draft

```json
{
  "SimulationCore": {
    "Version": "DRAFT-M2.4",
    "SceneContext": {
      "Observer": {
        "Type": "Geocentric",
        "Body": "Earth"
      },
      "ObservationTime": {
        "JD": 2451545.0,
        "TimeScale": "TDB",
        "TemporalMode": "ObservationTime"
      },
      "Frame": {
        "Type": "Ecliptic",
        "Epoch": "J2000"
      },
      "Instrument": "VEC",
      "Domain": "Ephemeris"
    },
    "StateGraph": {
      "ExecutionOrder": [
        "VENUS_NODE_1",
        "VENUS_NODE_2",
        "JUPITER_NODE_1",
        "JUPITER_NODE_2"
      ],
      "StateNodes": [
        {
          "NodeId": "VENUS_NODE_1",
          "NodeType": "L0.VSOP.HELIOCENTRIC_GEOMETRIC_VECTOR",
          "NodeRole": "Intermediate",
          "State": {
            "Target": "Venus",
            "Engine": "Astronometria",
            "EngineModel": "VSOP87",
            "Level": "L0",
            "OutputType": "Vector3",
            "Units": "AU"
          },
          "StateHash": "<STATEHASH_VENUS_NODE_1>",
          "Data": {
            "X": 0.0,
            "Y": 0.0,
            "Z": 0.0
          },
          "DataHash": "<DATAHASH_VENUS_NODE_1>",
          "Status": "Completed"
        },
        {
          "NodeId": "VENUS_NODE_2",
          "NodeType": "L0.GEOCENTRIC_GEOMETRIC_VECTOR",
          "NodeRole": "Terminal",
          "State": {
            "Target": "Venus",
            "Engine": "Astronometria",
            "EngineModel": "VSOP87",
            "Level": "L0",
            "OutputType": "Vector3",
            "Units": "AU"
          },
          "StateHash": "<STATEHASH_VENUS_NODE_2>",
          "Data": {
            "X": 0.0,
            "Y": 0.0,
            "Z": 0.0
          },
          "DataHash": "<DATAHASH_VENUS_NODE_2>",
          "Status": "Completed",
          "Truth": {
            "TruthMappingRef": "HORIZONS.EPHEM.VECTORS.L0.GEOCENTRIC",
            "GroundTruthProvider": "Horizons",
            "GroundTruthDatasetId": "<GT_DATASET_ID>",
            "AccuracyStatus": "NotEvaluated"
          }
        },
        {
          "NodeId": "JUPITER_NODE_1",
          "NodeType": "L0.VSOP.HELIOCENTRIC_GEOMETRIC_VECTOR",
          "NodeRole": "Intermediate",
          "State": {
            "Target": "Jupiter",
            "Engine": "Astronometria",
            "EngineModel": "VSOP87",
            "Level": "L0",
            "OutputType": "Vector3",
            "Units": "AU"
          },
          "StateHash": "<STATEHASH_JUPITER_NODE_1>",
          "Data": {
            "X": 0.0,
            "Y": 0.0,
            "Z": 0.0
          },
          "DataHash": "<DATAHASH_JUPITER_NODE_1>",
          "Status": "Completed"
        },
        {
          "NodeId": "JUPITER_NODE_2",
          "NodeType": "L0.GEOCENTRIC_GEOMETRIC_VECTOR",
          "NodeRole": "Terminal",
          "State": {
            "Target": "Jupiter",
            "Engine": "Astronometria",
            "EngineModel": "VSOP87",
            "Level": "L0",
            "OutputType": "Vector3",
            "Units": "AU"
          },
          "StateHash": "<STATEHASH_JUPITER_NODE_2>",
          "Data": {
            "X": 0.0,
            "Y": 0.0,
            "Z": 0.0
          },
          "DataHash": "<DATAHASH_JUPITER_NODE_2>",
          "Status": "Completed",
          "Truth": {
            "TruthMappingRef": "HORIZONS.EPHEM.VECTORS.L0.GEOCENTRIC",
            "GroundTruthProvider": "Horizons",
            "GroundTruthDatasetId": "<GT_DATASET_ID>",
            "AccuracyStatus": "NotEvaluated"
          }
        }
      ],
      "Transitions": [
        {
          "TransitionId": "T1",
          "TransitionType": "ComputeVSOPHeliocentricVector",
          "ToNodeTypes": [
            "L0.VSOP.HELIOCENTRIC_GEOMETRIC_VECTOR"
          ]
        },
        {
          "TransitionId": "T2",
          "TransitionType": "TransformHeliocentricToGeocentric",
          "FromNodeType": "L0.VSOP.HELIOCENTRIC_GEOMETRIC_VECTOR",
          "ToNodeType": "L0.GEOCENTRIC_GEOMETRIC_VECTOR"
        }
      ],
      "TerminalNodeRefs": [
        "VENUS_NODE_2",
        "JUPITER_NODE_2"
      ]
    }
  }
}
```

---

# 10. Simulation DatasetHeader Implication

M2.3 Simulation DatasetHeader must contain SimulationCore.

The Simulation DatasetHeader must therefore include:

* Dataset identity
* Experiment reference
* Simulation identity
* SimulationCore
* terminal output references
* StateHash/DataHash values
* GroundTruth comparison metadata for TerminalNodes

The Simulation DatasetHeader does not redefine SimulationCore.

It embeds it.

---

# 11. Open Questions

The following questions are not frozen:

1. Exact JSON field names
2. Whether State should duplicate selected SceneContext values or inherit them by reference
3. Exact canonicalization boundary for arrays of StateNodes
4. Whether DataHash is computed per StateNode or per StateNode time series for mesh datasets
5. Exact structure of TruthMappingRef catalog
6. Exact location of accuracy metadata
7. Whether TerminalNode Truth metadata belongs inside StateGraph or DatasetHeader validation block

---

# 12. Draft Freeze Criteria

This specification can move toward FREEZE when the following are resolved:

* terminology is accepted
* JSON draft is accepted structurally
* State vs Data boundary is accepted
* M2.4 includes/excludes are accepted
* Run == LastRun rule is accepted
* TerminalNode validation boundary is accepted
* M2.3 DatasetHeader dependency is accepted

---

# End of Draft


