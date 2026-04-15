# CM-07 Release Process

## 1. Purpose

This document defines how official releases of AstronoSphere are created.

The goal is to:

- provide stable and reproducible system versions
- clearly distinguish between development and released states
- ensure scientific validity of all releases

---

## 2. Core Principle

> A release is a formally accepted baseline.

Every release corresponds to a validated and reproducible system state.

---

## 3. Relationship to Baselines

- every release is based on a baseline
- not every baseline becomes a release

> Release = selected baseline with official status

---

## 4. Release Criteria

A release is created only if:

### 4.1 Scientific Validation

- all HolyScenarios pass within defined tolerances

---

### 4.2 Stability

- no known critical issues
- pipeline runs consistently

---

### 4.3 Completeness

- all relevant stages are functional
- data and code are aligned

---

## 5. Release Versioning

### 5.1 Format

```
vMAJOR.MINOR
```

Examples:

```
v1.0
v1.1
v2.0
```

---

### 5.2 Rules

- MAJOR: breaking or fundamental changes
- MINOR: improvements, extensions, refinements

---

## 6. Release Tagging

Each release is marked with a Git tag:

```
release/vX.Y
```

Example:

```
release/v1.0
```

---

## 7. Release Content

A release includes:

- Git commit (code)
- ReferenceData
- Validation result (HolyScenarios)

---

## 8. Release Workflow

1. Feature branches merged into main
2. Baseline validation completed
3. Decision: promote baseline to release
4. Create release tag
5. (Optional) documentation update

---

## 9. Immutability

> Released versions must never change.

If changes are required:

- create new release version
- never modify existing release

---

## 10. Traceability

Each release must be traceable to:

- Git commit
- baseline
- validation result
- ReferenceData

---

## 11. Design Benefits

This strategy ensures:

- stable and reproducible releases
- clear separation between development and production states
- scientific reliability

---

## 12. Summary

The release process:

- builds on validated baselines
- enforces scientific correctness
- uses versioned and tagged system states

---

End of document.
