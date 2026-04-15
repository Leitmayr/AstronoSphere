# CM-00 Configuration Management Plan (v1.0)

## 1. Purpose

This document consolidates all configuration management rules of AstronoSphere
into a single, coherent reference.

It defines how:

- the repository is structured
- code and data are versioned
- development is organized
- scientific correctness is ensured
- releases are created

---

## 2. Core Philosophy

AstronoSphere follows three fundamental principles:

1. KISS (Keep It Simple and Structured)
2. Scientific correctness over convenience
3. Reproducibility as a first-class requirement

---

## 3. Repository Structure (CM-01)

- Monorepo approach
- All components reside in one repository
- Relative paths from a single root
- Data containers contain only data

---

## 4. Project Numbering (CM-02)

- Numbers represent system roles
- Stable and immutable
- Reflect pipeline structure

Key ranges:

- 00–05 → Pipeline
- 10–19 → Data Access
- 80–89 → Documentation

Stages represent roles, not implementations.

---

## 5. Branching Strategy (CM-03)

- main = always consistent and scientifically valid
- feature branches for all development

Naming:

```
feature/<stage>-<description>
```

Rules:

- one primary stage per feature
- minimal cross-stage changes

Merge gate:

- all HolyScenarios must pass within tolerance

---

## 6. Commit Strategy (CM-04)

- atomic commits
- stage-based commit messages:

```
<stage>: <description>
```

- squash mandatory on merge to main
- main history = clean, one commit per feature

---

## 7. Baseline Strategy (CM-05)

- baseline = validated system state
- defined by Git commit + data + validation

Criteria:

- HolyScenarios pass
- system integrity ensured

Tagging:

```
baseline/vX.Y
```

---

## 8. Data Versioning (CM-06)

- data versioned together with code
- Git is single source of truth
- no separate data versioning system

Rules:

- ReferenceData is immutable once released
- Run / LastRun used for validation

---

## 9. Release Process (CM-07)

- release = selected baseline

Criteria:

- scientific validation passed
- system stable

Tagging:

```
release/vX.Y
```

Releases are immutable.

---

## 10. Data Organization (CM-08)

Separation of concerns:

| Aspect | Responsibility |
|--------|---------------|
| Scenario ID | Identity |
| Header | Processing definition |
| Folder | Classification |

Rules:

- file name = Scenario ID
- header is authoritative
- folders limited to Stage + Factory

---

## 11. Scientific Validation

- HolyScenarios define correctness
- validation is mandatory for merge and release
- tolerances are part of specification

---

## 12. Reproducibility Guarantee

AstronoSphere guarantees:

- identical results for identical inputs
- full traceability of code and data
- independence from external systems

---

## 13. Summary

AstronoSphere CM ensures:

- structured development
- deterministic behavior
- scientific reliability
- long-term maintainability

---

End of document.
