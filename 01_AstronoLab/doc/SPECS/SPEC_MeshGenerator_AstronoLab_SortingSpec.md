# SPEC_MeshGenerator_AstronoLab_SortSpec

## ChangeLog

ID   | Version | Change                       | Date         |
--   | ------- | ---------------------------- | -------------|
1    |   1.0   | Initial revision             |  2026-04-23  |



## Purpose

This document defines the deterministic sorting rule for MeshGenerator seed files
when imported by AstronoLab for preparation and experiment header generation.

The goal is:

- stable and reproducible processing order
- grouped catalog numbering for logically related mesh data
- independence from file system enumeration order
- deterministic `AS_<CatalogNumber>` assignment

---

## Scope

This specification applies only to:

- MeshGenerator seed files in `AstronoData/01_Seeds/Incoming`
- AstronoLab import and preparation flow
- deterministic ordering before generating prepared seeds / experiment headers

This specification does NOT define GUI behavior.

---

## Core Principle

> File system order must never define scientific processing order.

AstronoLab MUST determine processing order explicitly and deterministically.

The order MUST be derived from the seed content.

The file name may be used as a secondary fallback or validation aid,
but not as the primary source of truth.

---

## Input Assumption

Each MeshGenerator seed contains sufficient structured information to derive
a deterministic sort key, including at least:

- Event.Description
- Event.Qualifier
- Core.ObservedObject.Targets[0]
- Core.Time.StartJD
- Core.Time.StopJD

---

## Sorting Objective

Files that are logically related shall appear consecutively in catalog numbering.

This means:

- same mesh type grouped together
- same sub-epoch grouped together
- planets ordered consistently
- chronological order stable

---

## Normative Sorting Rule

AstronoLab MUST sort all MeshGenerator input seeds by the following key order:

1. MeshRank
2. SubEpochRank
3. PlanetRank
4. StartJD
5. StopJD
6. ResultID (fallback only)

---

## 1. MeshRank

The following fixed mesh order is mandatory:

1. MCRE
2. MXT1
3. MXT2
4. MVH1
5. MVH2
6. MVH3

This order reflects:

- simulation meshes first
- validation meshes second
- inner epoch before outer epoch

---

## 2. SubEpochRank

SubEpochs must be ordered numerically by epoch and sub-index.

Examples:

- SubEpoch1.1 < SubEpoch1.2 < ... < SubEpoch1.9
- SubEpoch2.1 < SubEpoch2.2 < ... < SubEpoch2.4
- SubEpoch3.1 < SubEpoch3.2 < SubEpoch3.3

The value must be parsed from `Event.Qualifier`.

Examples:

- `Simulation_SubEpoch1.4_Core`
- `Validation_SubEpoch2.3_Extended`

AstronoLab MUST extract:

- EpochNumber
- SubEpochNumber

and sort numerically, not lexicographically.

---

## 3. PlanetRank

The following fixed planet order is mandatory:

1. Mercury
2. Venus
3. Earth
4. Mars
5. Jupiter
6. Saturn
7. Uranus
8. Neptune

This order is deterministic and system-wide fixed.

---

## 4. StartJD

Within identical MeshRank, SubEpochRank, and PlanetRank,
sorting MUST use ascending `Core.Time.StartJD`.

---

## 5. StopJD

If StartJD is equal, sorting MUST use ascending `Core.Time.StopJD`.

---

## 6. ResultID (Fallback Only)

If all previous sort components are equal,
sorting MUST finally use `SeedOrigin.ResultID` in ordinal string order.

This is only a deterministic fallback and must normally not affect ordering.

---

## Derived Sort Key

The effective sort key is therefore:

`(MeshRank, EpochNumber, SubEpochNumber, PlanetRank, StartJD, StopJD, ResultID)`

---

## Parsing Rules

### Mesh Abbreviation

AstronoLab MUST derive the mesh abbreviation from:

- `Event.Description`

Expected values:

- MCRE
- MXT1
- MXT2
- MVH1
- MVH2
- MVH3

Unknown values must be rejected.

---

### SubEpoch

AstronoLab MUST parse `Event.Qualifier`.

Expected pattern:

`<Simulation|Validation>_SubEpoch<EpochNumber>.<SubEpochNumber>_<Core|Extended|Outer>`

Examples:

- `Simulation_SubEpoch1.1_Core`
- `Validation_SubEpoch2.4_Extended`

The parser must extract:

- EpochNumber
- SubEpochNumber

Unknown or malformed formats must be rejected.

---

### Planet

AstronoLab MUST derive the planet from:

- `Core.ObservedObject.Targets[0]`

Exactly one target is expected for MeshGenerator seeds.

Unknown values must be rejected.

---

## Determinism Rules

AstronoLab MUST guarantee:

- identical input files -> identical processing order
- identical processing order -> identical catalog numbering
- no dependency on:
  - OS directory enumeration order
  - Explorer sorting
  - file creation timestamp
  - file modification timestamp

---

## Error Handling

AstronoLab MUST fail fast if:

- mesh abbreviation is unknown
- qualifier cannot be parsed
- target list is empty
- target planet is unknown
- StartJD or StopJD is missing

Silent fallback behavior is forbidden.

---

## Rationale

This ordering ensures that related mesh datasets are grouped together.

Example:

All files for:

- `MCRE`
- `SubEpoch1.1`
- all planets

appear together before:

- `MCRE`
- `SubEpoch1.2`

This improves:

- catalog readability
- traceability
- manual review
- later analysis workflows in Astronolysis

---

## Example Ordering

Example prefix of the expected order:

1. MCRE / SubEpoch1.1 / Mercury
2. MCRE / SubEpoch1.1 / Venus
3. MCRE / SubEpoch1.1 / Earth
4. MCRE / SubEpoch1.1 / Mars
5. MCRE / SubEpoch1.1 / Jupiter
6. MCRE / SubEpoch1.1 / Saturn
7. MCRE / SubEpoch1.1 / Uranus
8. MCRE / SubEpoch1.1 / Neptune
9. MCRE / SubEpoch1.2 / Mercury
10. MCRE / SubEpoch1.2 / Venus

etc.

---

## Final Principle

AstronoLab must not “discover” an order.

AstronoLab must enforce the order defined here.

This is required for reproducibility and deterministic catalog generation.

---