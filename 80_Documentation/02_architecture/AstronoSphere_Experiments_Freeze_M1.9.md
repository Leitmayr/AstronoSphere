# AstronoSphere – Experiments & AstronoCert Freeze (M1.9)

## 1. Purpose

This document defines:
- the role of AstronoCert
- the structure of Experiments
- the naming convention of Experiment files

Experiments represent **certified physical scenarios**.

---

## 2. Core Principle

> An Experiment is a certified Scenario.

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
- MUST preserve Scenario integrity (CoreHash unchanged)

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

- ScenarioID (content-derived)
- CoreHash (content integrity)
- CatalogNumber (assigned identity)

---

## 6. File Naming (FROZEN)

### Format

```
<CATALOGNUMBER>__<HUMAN>__<SCENARIO>.json
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

### 7.3 Scenario Part

```
HELIO-J2000-TDB-2451545-2451546-1H
```

Derived from Scenario Core.

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
