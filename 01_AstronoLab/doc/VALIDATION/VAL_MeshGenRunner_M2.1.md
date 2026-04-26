# VAL_MeshGenRunner - AstronoLab

# A. Test Spec

## A.1 Definition of the "Golden Mesh Samples"

> **Rules to create the Golden Mesh Sample Set (GMSS):**
1. There shall be at least one Sample out of every meshType
    - MCRE
    - MXT1
    - MXT2
    - MVH1
    - MVH2
    - MVH3

2. Every planet shall be included in the GMSS

3. For MCRE it shall be Jupiter and Mercury. 
Rationale: JD > 2300 not included in MVH1 but must be included in MCRE
Rationale: Data fully available for Mercury

4. For MXT1 it shall be an outer planet. 
Rationale: outer planets not part of MVH1 but of MXT1. Selection: Uranus

5. For MXT2 it shall be an outer planet. 
Rationale: outer planets not part of MVH2 but of MXT2. Selection: Neptune

6. For MVH1 it shall be Saturn.
Rationale: shortest dataset in [1600, 2500].

7. For MVH2 it shall be Mars and Venus
Rationale: Data partially available for Mars
Rationale: Data fully available for Venus

8. For MVH3 it shall be Earth
Rationale: Data available for every Sub-Epoch

## A.2 GMSS Data Sets

The following selection of data sets should fulfill the rules from A.1:
- all 6 MeshTypes
- all 8 Planets
- Simulation + Validation
- full und partial Availability
- critical data set: MCRE/Jupiter with JD > 2300

```
AS-000146  MCRE  SubEpoch1.1  Mercury
AS-000206  MCRE  SubEpoch1.8  Jupiter   // JD > 2300 erfüllt
AS-000224  MXT1  SubEpoch2.1  Uranus
AS-000257  MXT2  SubEpoch3.1  Neptune
AS-000286  MVH1  SubEpoch1.2  Saturn
AS-000343  MVH2  SubEpoch2.2  Venus
AS-000345  MVH2  SubEpoch2.2  Mars
AS-000366  MVH3  SubEpoch3.2  Earth
```


## A.3 Check against Sorting Spec

### A.3.1 Check correct sequence
- first order: meshType - Sequence okay
    - MCRE
    - MXT1
    - MXT2
    - MVH1
    - MVH2
    - MVH3
- second order: check in every meshType 
    - numbering according to Epoch.SubEpoch
- third order: check sequence of Planets
    - numbering according to Epoch.SubEpoch.Planet 
    - order: Mercury, Venus, Earth, Mars, Jupiter, Saturn, Uranus, Neptune

- Check dataset number for every GMSS dataset
- Check transitions between Epochs and SubEpochs throughout the entire data set (229 files)
- Check the numbering of the 8 GMSS data sets for correctness
- Check the numbering of the 8 GMSS data sets for plausibility (if deviating)

## A.4 Check data availability

- For every GMSS Data set:
    - Check data availability against VAL_MeshTables_AllPlanets.md 
- Check for 229 data sets
    - First data set 146 
    - Last data set = 374 ?
- check GMSS run done
    - Amount of created files done
    - Amount of covered planets  done
    - Amount of covered epochs done
    - File names according spec done  

## A.5 Check against Mapping Spec

- Saturn and Earth out of the GMSS
    - Check correct header done for all GMSS done all
    - Compare with respective 00_Seeds/Incoming all GMSS done all
- Jupiter particularly for MCRE done.
- Use Beyond Compare if applicable 

Optionally: Check
- Notes construction (ResultID + separator + input notes): done.
- Category abbreviation via CategoryMapper: done.

## A.6 Check Determinism

- after full run with 229 files: check if 8 GMSS are identical (Run == LastRun) done
- after full run with 229 files: check if after Re-Run all 229 files are identical (Run == LastRun) done

## A.5 References

1. VAL_MeshTables_AllPlanets.md
2. SPEC_MeshGenerator_AstronoLab_MappingSpec.md
3. SPEC_MeshGenerator_AstronoLab_SortingSpec.md

4.  Reference input data sets: ..\AstronoSphere\AstronoData\01_Seeds\Incoming



# B. Test Protocol


## B.1 TestSuite: Sorting
```md


Observation:
- first order: meshType - Sequence okay
    - MCRE ok
    - MXT1 ok 
    - MXT2 ok
    - MVH1 ok
    - MVH2 ok
    - MVH3 ok
- second order: check in every meshType 
    - numbering according to Epoch.SubEpoch ok
- third order: check sequence of Planets ok
    - numbering according to Epoch.SubEpoch.Planet 
    - order: Mercury, Venus, Earth, Mars, Jupiter, Saturn, Uranus, Neptune

- Check dataset number for every GMSS dataset ok
- Check transitions between Epochs and SubEpochs throughout the entire data set (229 files) ok
- Check the numbering of the 8 GMSS data sets for correctness ok 
- Check the numbering of the 8 GMSS data sets for plausibility (if deviating) ok


Status:
Confirmed
```

---


## B.2 TestSuite: Data availability

```md
Observation: Run with GMSS
- 8 files created as expected. Ok.
- File numbers: ok.
- Files names: ok.
- 6 meshTypes covered: ok.
- 8 planets created: ok.

Observation: Full Run
- 229 Files generated: ok
- first file #146 ok
- last file #374 ok
- no duplicates ok


Status:
Confirmed
```

---

## B.3 TestSuite: Mapping

```md

Observation: Run with GMSS
- File 363
    - ExperimentID ok
    - CatalogNumber ok
    - no CoreHash ok
    - Core ok
    - TimeScale matches Epoch MVH3 ok
    - SubEpoch3.2 - Checked JD with Webtool ok
    - Event ok
    - Notes ok
    - Beyond Compare Compared original input file (LastRun) and - generated File: acceptable deviation in ResultID: ok!
- Beyond Compares made for with the criteria of File 363
    - File 146: ok
    - File 206: ok
    - File 224: ok
    - File 257: ok
    - File 286: ok
    - File 343: ok
    - File 345: ok
    - File 366: ok (see above)
- Compare new generated 00_Seeds/Prepared/#254 with 00_Seeds/Processed (DataSet #23) for structural plausibility
    - Structure fits perfectly. ok.
- Check JD_Start and JD_Stop of MVH1, 2, 3 against Reference 1. (above)
    - DataSet 283: Start 2360257.5 ok| Stop 2378467.5 ok
    - DataSet 342: Start 2305529.5 ok| Stop 2451541.5 ok
    - DataSet 363: Start 1721143.5 ok| Stop 3181388.5 ok
- check critical Jupiter data set MCRE > 2300 for 
    - Dataset 203: Start 2561137.5 ok| Stop 2597617.5 ok

Status:
Confirmed
```

---

## B.4 TestSuite: Determinism

```md

Observation: Full Run
- Run (Full) == LastRun (GMSS) Comparison with Beyond Conmpare
    - File 146: binary identical ok
    - File 206: binary identical ok
    - File 224: binary identical ok
    - File 257: binary identical ok
    - File 286: binary identical ok
    - File 343: binary identical ok
    - File 345: binary identical ok
    - File 366: binary identical ok

- Run (Full) == LastRun (Full) Comparison with Beyond Compare
    - all 229 binary identical

Status:
Confirmed
```