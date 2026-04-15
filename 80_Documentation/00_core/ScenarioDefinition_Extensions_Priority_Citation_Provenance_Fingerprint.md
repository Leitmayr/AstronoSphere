
# ScenarioDefinition_Extensions_Priority_Citation_Provenance_Fingerprint.md

Status: Proposed Extension  
Target Document: `02_AstronoSphere_ScenarioDefinition_v1.2.md`  
Purpose: Introduce additional metadata concepts without modifying the existing structure directly.

This document defines **additional optional metadata blocks** that can be integrated into the existing Scenario Definition Standard of AstronoSphere.

The goal is to improve:

- scientific traceability
- reproducibility of validation runs
- documentation quality
- governance of high‑value scenarios

The extensions are **fully compatible with the existing Scenario architecture** and do **not modify the Scenario Core or CoreHash semantics**.

These blocks may be added to the **Scenario Header Metadata Section** of the Scenario Definition specification.

---

# 1 Scenario Priority

## Purpose

The ObservationCatalog should remain compact and focused on scientifically meaningful validation experiments.

A priority field allows the maintainer to identify **high‑value scenarios** within the catalog.

This enables:

- curated validation subsets
- prioritized regression testing
- identification of canonical benchmark scenarios

## Design Principles

Priority is:

- **curated by the ObservationCatalog maintainer**
- **not automatically generated**
- **not part of the Scenario Core**
- **not part of the CoreHash**

This ensures that the **physical experiment identity remains unchanged**.

## Field Definition

```
Priority: integer
```

Recommended scale: the lower the higher the priority

| Value | Meaning |
|------|--------|
| 3 | Standard scenario |
| 2 | Important scenario |
| 1 | Key benchmark scenario |

Higher priority scenarios may be used as:

- regression gate scenarios
- validation demonstrations
- scientific documentation references

## Example

```
Priority: 3
```

## 

---

# 2 Scenario Citation Block

## Purpose

AstronoSphere scenarios often originate from published scientific sources.

Examples include:

- Jean Meeus — Astronomical Algorithms
- historical astronomical observations
- published ephemeris studies

A Citation block ensures **proper scientific attribution** and enables future academic publication of AstronoSphere results.

## Design Principles

The Citation block:

- documents intellectual origin
- supports open‑source collaboration
- enables academic referencing

Citation metadata must **not affect the physical experiment identity** and must therefore **not be included in the CoreHash**.

## Structure

```
ScenarioCitation:
  Author: string
  Source: string
  Citation: string
```

### Field descriptions

| Field | Description |
|------|-------------|
| Author | Original scientific author |
| Source | Algorithm or system used |
| Citation | Formal reference or bibliographic citation |

## Example

```
ScenarioCitation:
  Author: Jean Meeus
  Source: Astronomical Algorithms
  Citation: Meeus, J. (1998). Astronomical Algorithms, 2nd Edition.
```

## Alternative Example (Generated Scenario)

```
ScenarioCitation:
  Author: MeeusScenarioFactory
  Source: Astronometria Scenario Generator
  Citation: Meeus, J. (1998). Astronomical Algorithms.
```

---

# 3 Ground Truth Citation

## Purpose

Scenario definitions describe experiments, but reference datasets are produced by **TruthFactories**.

Scientific reproducibility requires documentation of the **reference data source**.

Typical providers include:

- JPL Horizons
- SPICE kernels
- analytical solutions

## Design Principles

Ground truth metadata describes **the measurement source**, not the experiment.

It therefore belongs to **dataset provenance**, not the Scenario Core.

## Structure

```
TruthCitation:
  Provider: string
  Source: string
  Citation: string
```

## Example

```
TruthCitation:
  Provider: JPL Horizons
  Source: NASA/JPL Solar System Dynamics
  Citation: Giorgini, J.D. et al. (1996). JPL's On-Line Solar System Data Service.
```

---

# 4 Provenance Chain

## Purpose

Scientific validation requires documentation of the **entire computational chain** that produced the results.

AstronoSphere validation consists of three conceptual stages:

```
Scenario → TruthFactory → Astronometria
```

Documenting this chain ensures:

- full traceability
- reproducible validation reports
- transparent scientific workflows

## Structure

```
Provenance:
  ScenarioFactory: string
  TruthFactory: string
  ValidationTarget:
    Software: string
    GitCommit: string
    GitBranch: string (optional)
    GitTag: string (optional)
```

### Field descriptions

| Field | Description |
|------|-------------|
| ScenarioFactory | tool used to generate scenario candidate |
| TruthFactory | system producing reference data |
| ValidationTarget | software performing the validation |

## Example

```
Provenance:
  ScenarioFactory: MeeusScenarioFactory
  TruthFactory: HorizonsTruthFactory
  ValidationTarget:
    Software: Astronometria
    GitCommit: 8F3A12C
    GitTag: v0.9-M1
```

---

# 5 Validation Fingerprint

## Purpose

Each validation run should be uniquely identifiable.

The Validation Fingerprint provides a deterministic identifier for the combination of:

- Scenario
- TruthFactory dataset
- Astronometria version

This enables:

- reproducible validation reports
- comparison of historical validation runs
- regression tracking across engine versions

## Concept

A fingerprint is constructed from the following components:

```
ScenarioID
TruthFactory identifier
Astronometria Git commit
Run date
```

Example construction:

```
VF-20260316-8F3A12C-000001
```

## Field Definition

```
ValidationFingerprint: string
```

## Example

```
ValidationFingerprint: VF-20260316-8F3A12C-AS0001
```

## Usage

The fingerprint should appear in:

- validation reports
- statistical analysis outputs
- dataset metadata
- regression logs

---

# 6 Integration Guidelines

These extensions follow the existing AstronoSphere design philosophy:

- Scenario **Core remains immutable**
- Metadata extensions remain outside the CoreHash
- Factories remain responsible for dataset metadata

The following rules apply:

1. Extensions must **not modify the Scenario Core structure**
2. Extensions must **not influence CoreHash**
3. Extensions may be ignored by factories if unsupported
4. Validation tools may consume these fields for reporting

---

# 7 Example Extended Scenario Header

Example combining the proposed extensions:

```
Priority: 3

ScenarioCitation:
  Author: Jean Meeus
  Source: Astronomical Algorithms
  Citation: Meeus, J. (1998). Astronomical Algorithms.

TruthCitation:
  Provider: JPL Horizons
  Source: NASA/JPL SSD
  Citation: Giorgini et al. (1996)

Provenance:
  ScenarioFactory: MeeusScenarioFactory
  TruthFactory: HorizonsTruthFactory
  ValidationTarget:
    Software: Astronometria
    GitCommit: 8F3A12C
    GitTag: v0.9-M1

ValidationFingerprint: VF-20260316-8F3A12C-AS0001
```

---

# 8 Summary

These extensions introduce four new metadata capabilities:

| Feature | Purpose |
|-------|--------|
| Priority | Curated scenario importance |
| Citation | Scientific attribution |
| Provenance | Full validation pipeline traceability |
| ValidationFingerprint | Unique validation run identifier |

These additions significantly strengthen the **scientific reproducibility** and **documentation quality** of the AstronoSphere framework while remaining fully compatible with the existing Scenario Definition Standard.
