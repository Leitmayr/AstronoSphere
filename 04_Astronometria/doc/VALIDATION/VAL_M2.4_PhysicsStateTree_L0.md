# VAL_M2.4_PhysicsStateTree_L0.md

## Status

Version: V1.0  
Status: Draft  
Scope: Astronometria / M2.4 / PhysicsStateTree / L0 Validation  
Milestone: M2.4  
Date: 2026-05-17  


## Change log 

| Revision | Changes | Date |
| -------- | ------- | ---- |
| V1.0 | intitial revision before implementation of M2.4  | 2026-05-18
| V1.1 | added Section 0. Main change is that GEO-EQU is deferred in M2.4. Added sentence in section 13.3 | 2026-06-22


---

# 0. Current Validation Scope Freeze

Current Milestone = M2.4

M2.4 validates:

✓ StateMachine execution for existing HELIO-ECL L0 outputs  
✓ StateMachine execution for existing GEO-ECL L0 outputs  
✓ PHYS internal node resolution  
✓ Ordered Path execution  
✓ Run == LastRun for existing released SimulationData  
✓ StateHash/DataHash presence and stability  

M2.4 does not validate:

✗ GEO-EQU ScientificRun output  
✗ GEO-EQU Horizons GroundTruth comparison  
✗ GEO-EQU baseline promotion  

Reason:
GEO-EQU Horizons GroundTruth datasets for AS-000059 through AS-000072 are currently missing.
AstronoTruth currently cannot safely generate GEO-EQU GroundTruth because the Horizons request plane mapping is not yet implemented.

Therefore, all GEO-EQU validation sections in this document are deferred and retained only as preparation for a later dedicated milestone.


# 1. Purpose

This validation specification defines the mandatory validation strategy for M2.4.

M2.4 introduces the `PhysicsStateTree` execution structure for L0 while preserving the scientific output of M2.3 for already existing terminal results.

The main validation goal is:

```text
Execution structure changes.
Existing scientific terminal results must not change.
```

In addition, M2.4 introduces new GEO-EQU L0 terminal outputs.

These GEO-EQU outputs are new scientific datasets and therefore require explicit scientific validation against GroundTruth.

---

# 2. Validation Principles

## 2.1 One-Dimension Rule

M2.4 changes only one dimension:

```text
Execution structure -> PhysicsStateTree
```

The following must remain stable:

- existing HELIO-ECL L0 results
- existing GEO-ECL L0 results
- output folder structure
- file naming
- terminal JSON persistence structure
- hashing rules
- GroundTruth lookup behavior

## 2.2 Main Validation Invariant

The main validation invariant is:

```text
Run == LastRun
```

This validates deterministic replay of the new execution structure.

## 2.3 Baseline Validation

Selected samples shall additionally be compared against Baseline:

```text
Run == Baseline
```

Because build metadata such as GitCommit may differ, Baseline comparison may focus on:

- scientific payload
- TerminalNode data
- relevant deterministic header fields
- StateHash/DataHash where applicable

---

# 3. Runtime Trace Requirement

## 3.1 Mandatory Console Output

For every executed StateNode, Astronometria MUST print the executed `NodeType` to the console.

Purpose:

- validate TerminalNode resolution
- validate OrderedNodePath
- validate SinglePath execution
- validate GEO-EQU path execution
- support manual debugging

## 3.2 Required Trace Format

Recommended console format:

```text
[PhysicsStateTree] Execute PHYS.L0.HELIO.ECL.J2000.VEC
[PhysicsStateTree] Execute PHYS.L0.GEO.ECL.J2000.VEC
[PhysicsStateTree] Execute PHYS.L0.GEO.EQU.J2000.VEC
[PhysicsStateTree] Terminal PHYS.L0.GEO.EQU.J2000.VEC
```

The exact prefix may differ, but the `NodeType` must be visible and grep-friendly.

## 3.3 Validation Rule

For every M2.4 terminal node, the console trace must match the expected OrderedNodePath from the StateTreeRegistry.

---

# 4. MeasurementDefinition to TerminalNodeType Validation

## 4.1 Valid Mapping Cases

The following mappings must be tested:

| MeasurementDefinition | Expected TerminalNodeType |
| --- | --- |
| HELIO/ECL/J2000/L0/VEC | PHYS.L0.HELIO.ECL.J2000.VEC |
| GEO/ECL/J2000/L0/VEC | PHYS.L0.GEO.ECL.J2000.VEC |
| GEO/EQU/J2000/L0/VEC | PHYS.L0.GEO.EQU.J2000.VEC |

## 4.2 Invalid Mapping Case

An unsupported MeasurementDefinition must produce:

```text
040.010 Unknown MeasurementDefinition
```

Example invalid cases:

- TOPO/ECL/J2000/L0/VEC
- GEO/EQU/J2000/L0/RADEC
- GEO/ECL/OFDATE/L0/VEC

Expected result:

- diagnostic record written
- no SimulationData written
- deterministic first matching diagnostic behavior preserved

---

# 5. StateTreeRegistry Validation

## 5.1 Valid Registry Cases

The registry must resolve:

| TerminalNodeType | Expected OrderedNodePath |
| --- | --- |
| PHYS.L0.HELIO.ECL.J2000.VEC | [PHYS.L0.HELIO.ECL.J2000.VEC] |
| PHYS.L0.GEO.ECL.J2000.VEC | [PHYS.L0.HELIO.ECL.J2000.VEC, PHYS.L0.GEO.ECL.J2000.VEC] |
| PHYS.L0.GEO.EQU.J2000.VEC | [PHYS.L0.HELIO.ECL.J2000.VEC, PHYS.L0.GEO.ECL.J2000.VEC, PHYS.L0.GEO.EQU.J2000.VEC] |

## 5.2 Invalid Registry Case

A valid TerminalNodeType without registry entry must produce:

```text
040.011 Unknown NodeType
```

Recommended test setup:

- use a test registry JSON
- remove one terminal node path entry from the test registry
- run a valid MeasurementDefinition that resolves to the missing TerminalNodeType

Expected result:

- MeasurementDefinition is valid
- TerminalNodeType resolution succeeds
- registry lookup fails
- diagnostic 040.011 is written
- no SimulationData is written

This test must not require production code modification.

---

# 6. Persistence Validation

## 6.1 TerminalNode Persistence

M2.4 persists only TerminalNode data.

Expected:

```text
TerminalNode data persisted
IntermediateNode data not persisted
Transition data not persisted
Full StateTreePath not persisted
```

## 6.2 JSON Structure

Existing M2.3-compatible SimulationData structure must remain stable.

Expected:

- no new IntermediateNode arrays in regular ScientificRun output
- TerminalNode still contains StateHash and DataHash
- scientific payload format remains stable
- folder structure remains unchanged

---

# 7. Determinism Validation

## 7.1 Run == LastRun

Primary validation:

```text
Run == LastRun
```

This must be performed for the complete M2.4 ScientificRun output set.

Expected:

- byte-identical deterministic output where metadata permits
- no unexplained differences
- no nondeterministic ordering changes
- no timestamp-driven differences

## 7.2 Run == Baseline Sampling

Secondary validation:

```text
Run == Baseline
```

This may be performed on selected representative samples.

Because GitCommit or build metadata may differ, the comparison may focus on:

- TerminalNode scientific data
- position vectors
- velocity vectors
- StateHash/DataHash if intended to be stable
- selected header fields

---

# 8. Output Equivalence Validation

## 8.1 Existing M2.3 Terminal Results

For all pre-existing M2.3-compatible terminal outputs:

```text
M2.4 terminal scientific data == M2.3 terminal scientific data
```

Expected:

- HELIO-ECL L0 data unchanged
- GEO-ECL L0 data unchanged
- no numerical drift introduced by StateTree refactoring

## 8.2 GEO-EQU New Outputs

GEO-EQU L0 is newly introduced in M2.4.

Therefore, GEO-EQU outputs must be scientifically validated.

Validation target:

```text
PHYS.L0.GEO.EQU.J2000.VEC
```

Expected scientific meaning:

```text
Geocentric Equatorial J2000 Geometric Vector
```

Reference:

```text
Horizons Validation Semantics:
Geocentric Equatorial J2000 Geometric Vector
```

Validation requirements:

- compare against matching Horizons GroundTruth where available
- validate position vector components
- validate expected transformation behavior from GEO-ECL to GEO-EQU
- define and apply explicit tolerance
- inspect representative planets manually before baseline promotion

Recommended GEO-EQU validation samples:

- Mercury
- Venus
- Mars
- Jupiter
- Saturn
- Uranus
- Neptune

Recommended edge attention:

- near ecliptic-plane crossing
- high inclination / large Z component cases
- large-distance planets for accumulated transform visibility

---

# 9. Hash Boundary Validation

## 9.1 Hash Responsibility

Astronometria must not implement canonical hashing rules.

Expected:

```text
Astronometria builds State/Data objects.
AstronoData.Contracts canonicalizes and hashes.
```

## 9.2 TerminalNode Hash Presence

Each persisted TerminalNode must contain:

- StateHash
- DataHash

Expected:

- hashes are deterministic
- hashes are produced via Contracts
- hashes are not produced by custom local hashing code in Astronometria

---

# 10. Diagnostic Validation

## 10.1 040.010 Unknown MeasurementDefinition

Trigger:

```text
Unsupported MeasurementDefinition
```

Expected:

- DiagMsg 040.010
- no SimulationData
- deterministic diagnostic folder output

## 10.2 040.011 Unknown NodeType

Trigger:

```text
Known MeasurementDefinition
Known TerminalNodeType
Missing StateTreeRegistry path
```

Expected:

- DiagMsg 040.011
- no SimulationData
- deterministic diagnostic folder output

## 10.3 Diagnostic Folder

Diagnostic output folder:

```text
04_Simulations/
    DiagMessages/
        Run/
        LastRun/
```

Run/LastRun handling must follow the existing deterministic simulation diagnostic handling.

---

# 11. Acceptance Criteria

M2.4 validation is accepted when:

1. All valid MeasurementDefinitions resolve to the expected TerminalNodeType.
2. All TerminalNodeTypes resolve to the expected OrderedNodePath.
3. Runtime console trace prints all executed NodeTypes.
4. Run == LastRun passes for the M2.4 output set.
5. Existing M2.3 scientific terminal outputs remain unchanged.
6. GEO-EQU validation deferred (out of M2.4 scope)
7. IntermediateNodes are not persisted in regular ScientificRun output.
8. 040.010 and 040.011 diagnostics are tested.
9. StateHash/DataHash are present on TerminalNode output.
10. Hashing remains centralized in AstronoData.Contracts.

---

# 12. Final Validation Principle

M2.4 is successful if the new PhysicsStateTree changes the execution structure without changing existing scientific results, while adding scientifically validated GEO-EQU L0 terminal outputs.

In short:

```text
Same science for existing outputs.
New science for GEO-EQU.
Deterministic StateTree execution for all.
```


# 13. Test cases

## 13.1 Data check

### Number of elements
Amount of calculated elements in Run: 235
Amount of calculated elements in LastRun: 235
Amount of calculated elements in Baseline: 235
-> Number matches expectation. PASSED.

### Target Folder
C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData\04_Simulations\Scientific\Ephemeris\Run
-> as expected. PASSED.

### Run/LastRun Logic
all file are being transferred correctly to 
C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData\04_Simulations\Scientific\Ephemeris\LastRunand afterwards new elements are written to 
C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData\04_Simulations\Scientific\Ephemeris\Run
-> as expected. PASSED.

## 13.2 HELIO-ECL und GEO-ECL: Run == LastRun for existing data sets
Run == LastRun true for 235 experiments.
Note: GitBranch excluded from comparison.
-> Behavior as expected: PASSED.

## 13.3 GEO-EQU: Scientific evaluation

Note: for M2.4 all following tests are deferred!

### 13.3.1 Golden Samples of this test suite

The following 2 Golden Samples out of the 14 EQU data sets shall be analyzed further:

1. AS-000059: Venus - Geocentric equatorial declination crossing - to + 
2. AS-000072: Jupiter - Geocentric equatorial declination crossing + to -

### 13.3.2 Check structure

Check Json Structure for AS-000059 and AS-000072 and compare it against Section 4 of the specification.

1) Correct File Name Creation

Test Protocol:
```text
 AS-000059: Venus 
 AS-000072: Jupiter 
```

2) StateHash as expected: 
- check canonicalization from the command line
- check StateHash by means of https://emn178.github.io/online-tools/sha256.html
- check DataHash by means of https://emn178.github.io/online-tools/sha256.html

Test Protocol:
```text
AS-000059: 
- SH:     
- SH Web: 
- DH:     
- DH Web: 
```

```text
 AS-000072: 
- SH:     
- SH Web: 
- DH:     
- DH Web: 
```

3) Check Structure as described in Section 4 of the spec

Test Protocol:
```text
 AS-000059: Venus 
 AS-000072: Jupiter 
```
4) Check GitCommit and GitBranch against values in GIT Webinterface

Test Protocol:

```text
 AS-000059: Venus 
 AS-000072: Jupiter 
```

5) Verify CoreHash and DataHash of SimulationHeader with those of Experiment and GroundTruth input files

Test Protocol:

```text
 AS-000059: Venus 
 AS-000072: Jupiter 
```

6) Verify GroundTruthRef.Provider = "Horizons" -> Verify GroundTruth.Provider field is correctly derived from the GroundTruth input.
Test Protocol:

```text
 AS-000059: Venus 
 AS-000072: Jupiter 
```

7) Verify terminal nodes types: expected

Expectation:
- AS-000059: VSOP87.L0.GEO.EQU.J2000.TDB.VEC
- AS-000072: VSOP87.L0.GEO.EQU.J2000.TDB.VEC


```text
Test Protocol:
- AS-000059: 
- AS-000072: 
```

8) Verify Check 5 evenly distributed samples against Horizons manual call. Expected: only last digit deviation

```text
Even distribution according:
0%
25%
50%
75%
100%
```

```text
Test Protocol:
- AS-000059: 
- AS-000072: 
```

9) Verify Data.Velocity: Expected = 0 for all dimensions

```text
Test Protocol:
- AS-000059: 
- AS-000072: 
```

10) Verify amount of data sets. Exptected number of delta datasets equal to those from Astronometria Test Framework.

```text
Test Protocol:
- AS-000059: 
- AS-000072: 
```

11) Check change for algebraic sign in z

```text
- AS-000059: z changes from - to +
- AS-000072: z changes from + to -
```

12) Check path to terminal node

```text
- AS-000059: 
- AS-000072: 
```

13) 

### 13.3.3 Run/LastRun for EQU

Check Run == LastRun for all 14 EQU Datasets

```text
- AS-000059: 
- AS-000060: 
- AS-000061: 
- AS-000062: 
- AS-000063: 
- AS-000064: 
- AS-000065: 
- AS-000066: 
- AS-000067: 
- AS-000068: 
- AS-000069: 
- AS-000070: 
- AS-000071: 
- AS-000072: 
```

### 13.3.4 Diagnostics

to be added later

# End of Document
