# AstronoTruth GEO-EQU GroundTruth Gap

## Findings and Deferral Decision

**Date:** 2026-06-22  
**Status:** Confirmed gap; implementation deliberately deferred  
**Scope:** AstronoTruth / EphemerisFactory / GEO-EQU L0 GroundTruth  
**Related milestone:** Astronometria M2.4

---

## 1. Purpose

This document records the investigation into missing GroundTruth datasets for the GEO-EQU experiments `AS-000059` through `AS-000072`.

It also records the explicit decision not to modify AstronoTruth during the current Astronometria sprint.

The goal is to preserve the findings for a later dedicated AstronoTruth development step without reopening the analysis.

---

## 2. Initial Observation

The released experiment catalog contains the following fourteen GEO-EQU experiments:

- `AS-000059` through `AS-000065`: geocentric equatorial ascending-node experiments
- `AS-000066` through `AS-000072`: geocentric equatorial descending-node experiments

A direct search in:

```text
AstronoData/03_GroundTruth/Ephemeris/Horizons/Baseline
```

found no GroundTruth dataset referencing any of these catalog numbers.

The result was:

```text
AS-000059  GroundTruthCount = 0
AS-000060  GroundTruthCount = 0
AS-000061  GroundTruthCount = 0
AS-000062  GroundTruthCount = 0
AS-000063  GroundTruthCount = 0
AS-000064  GroundTruthCount = 0
AS-000065  GroundTruthCount = 0
AS-000066  GroundTruthCount = 0
AS-000067  GroundTruthCount = 0
AS-000068  GroundTruthCount = 0
AS-000069  GroundTruthCount = 0
AS-000070  GroundTruthCount = 0
AS-000071  GroundTruthCount = 0
AS-000072  GroundTruthCount = 0
```

### Confirmed finding

No Horizons GroundTruth baseline currently exists for the GEO-EQU experiments `AS-000059` through `AS-000072`.

---

## 3. Source-Code Findings

The relevant AstronoTruth sources were reviewed.

### 3.1 FactoryRunner does not explicitly exclude GEO-EQU

File:

```text
03_AstronoTruth/src/EphemerisFactory/Core/FactoryRunner.cs
```

`FactoryRunner` loads all released experiment JSON files and does not contain an explicit filter that excludes GEO-EQU experiments.

Therefore, the missing datasets are not caused by an intentional GEO-EQU exclusion in the runner.

### 3.2 HorizonsRequestBuilder ignores the experiment plane

File:

```text
03_AstronoTruth/src/EphemerisFactory/Core/HorizonsRequestBuilder.cs
```

The builder evaluates:

- observer type
- target
- time range
- step size

It maps the observer type to the Horizons center:

```text
Heliocentric -> @10
Geocentric   -> 500@399
```

However, it does not read the experiment's frame plane from `Core.Frame`.

Instead, every request currently uses the hard-coded value:

```csharp
RefPlane = "ECLIPTIC"
```

This means the current request builder can distinguish HELIO from GEO, but it cannot distinguish GEO-ECL from GEO-EQU.

### 3.3 HorizonsMapping has no frame-plane mapping

File:

```text
03_AstronoTruth/src/EphemerisFactory/Core/HorizonsMapping.cs
```

The current mapping centralizes only:

- measurement type to `EPHEM_TYPE`
- correction level to vector correction

It contains no mapping for the measurement frame plane.

A future GEO-EQU implementation must add a canonical frame-plane mapping here or in an equivalent single authoritative mapping component.

### 3.4 DatasetBuilder does not solve the request-semantic gap

File:

```text
03_AstronoTruth/src/EphemerisFactory/Core/DatasetBuilder.cs
```

`DatasetBuilder` builds the GroundTruth container from the experiment and the returned Horizons data. It does not determine the Horizons reference plane.

Therefore, changes in `DatasetBuilder` alone cannot add scientifically correct GEO-EQU generation.

---

## 4. Risk of Generating GEO-EQU with the Current Code

Running AstronoTruth for `AS-000059` through `AS-000072` with the current implementation would not be scientifically safe.

The experiments define GEO-EQU semantics, while the generated Horizons request would still use:

```text
REF_PLANE = ECLIPTIC
```

This creates the risk of producing datasets that are associated with GEO-EQU experiments but contain GEO-ECL vectors.

Such datasets could appear structurally valid while being semantically wrong.

This is more dangerous than a missing dataset because it could silently corrupt validation.

---

## 5. Decision

No AstronoTruth source code will be changed during the current Astronometria sprint.

### Rationale

The current sprint dimension is Astronometria M2.4 state-machine integration and validation.

Adding GEO-EQU support to AstronoTruth would introduce another independent dimension:

- provider request mapping
- GroundTruth generation
- GroundTruth validation
- complete AstronoTruth Back-to-Back testing
- baseline regeneration

This would violate the Stealth Mode rule:

> At any time, exactly one dimension may change.

The missing GEO-EQU GroundTruth is therefore accepted as a known and documented boundary of the current sprint.

This is a deliberate scope decision, not an unresolved debugging action.

---

## 6. Consequence for the Current Astronometria Sprint

ScientificRuns for `AS-000059` through `AS-000072` cannot currently resolve a matching GroundTruth baseline.

The expected current behavior is therefore a deterministic missing-GroundTruth diagnostic rather than simulation execution.

These experiments must not be used as proof of end-to-end GEO-EQU validation until dedicated GEO-EQU GroundTruth has been generated, scientifically checked, and baselined.

The Astronometria GEO-EQU computation branch may still be validated by its existing implementation-level and regression evidence, but not yet by the complete ScientificRun-to-Horizons chain for these fourteen experiments.

---

## 7. Future Major Development Step

GEO-EQU support shall be introduced later as a dedicated AstronoTruth development step.

Recommended scope:

1. Define and freeze the Horizons mapping for:
   - GEO-ECL
   - GEO-EQU
2. Read the canonical frame-plane semantics from the experiment.
3. Centralize the provider mapping in `HorizonsMapping`.
4. Remove the hard-coded `RefPlane = "ECLIPTIC"` behavior.
5. Add focused unit tests for request mapping.
6. Generate GroundTruth Run data for `AS-000059` through `AS-000072`.
7. Verify the canonical request and request hash.
8. Perform scientific spot checks against Horizons output.
9. Perform complete AstronoTruth Run/LastRun Back-to-Back validation.
10. Promote the accepted datasets to GroundTruth Baseline.
11. Re-run the Astronometria ScientificRuns for all fourteen GEO-EQU experiments.

The exact Horizons parameter mapping for equatorial frame output must be verified against the authoritative Horizons API documentation during that future step.

---

## 8. Final Handover Statement

The current state is understood:

- GEO-EQU experiments exist and are released.
- Their Horizons GroundTruth baselines do not exist.
- AstronoTruth does not explicitly filter them out.
- AstronoTruth currently hard-codes ecliptic request output and therefore cannot safely generate GEO-EQU GroundTruth.
- No source change is authorized in the current Astronometria sprint.
- GEO-EQU GroundTruth support is deferred to a dedicated major AstronoTruth development step with full Back-to-Back validation.

---

# End of Document
