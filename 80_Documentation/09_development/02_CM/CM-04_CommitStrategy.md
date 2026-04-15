# CM-04 Commit Strategy

## 1. Purpose

This document defines the commit strategy for the AstronoSphere repository.

The goal is to:

- ensure a readable and meaningful Git history
- support reproducibility and traceability
- align commits with the stage-based architecture

---

## 2. Core Principle

> Every commit represents a coherent, minimal, and meaningful step in the system evolution.

---

## 3. Commit Granularity

### 3.1 Atomic Commits

Each commit must:

- address a single logical change
- be self-contained
- not mix unrelated modifications

---

### 3.2 Stage Focus

> Each commit must belong to a single primary stage.

Rules:

- changes should primarily affect one stage
- cross-stage changes must be minimal and justified

---

## 4. Commit Message Convention

### 4.1 Format

```
<stage>: <short description>
```

---

### 4.2 Examples

```
03: improve light-time correction
04: refine velocity computation
01: add Mars opposition scenarios
```

---

### 4.3 Guidelines

- use present tense
- keep description concise
- describe *what* and *why*, not *how*

---

## 5. Commit Content Rules

### 5.1 Allowed

- implementation changes
- test updates
- documentation updates (if directly related)

---

### 5.2 Forbidden

- mixing multiple features in one commit
- large refactoring unrelated to feature
- broken intermediate states

---

## 6. Build & Test Requirement

> Every commit must compile successfully.

Recommended:

- local validation before commit
- no knowingly broken commits

---

## 7. Merge & Squash Policy

### 7.1 Squash on Merge (Mandatory)

> All feature branches must be squashed into a single commit when merged into `main`.

Rationale:

- keeps `main` history clean and readable
- ensures one commit per feature
- simplifies traceability and rollback

---

### 7.2 Resulting Commit Message

The squashed commit message must:

- follow the stage-based format
- clearly describe the feature
- optionally include validation notes

Example:

```
03: implement lighttime correction

- improved numerical stability
- validated with HolyScenarios
```

---

## 8. Relationship to Branching

- commits belong to a feature branch
- commit history may be iterative during development
- final history in `main` is clean and condensed via squash

---

## 9. Traceability

Commits must allow reconstruction of:

- what changed
- why it changed
- which stage is affected

---

## 10. Design Benefits

This strategy ensures:

- clear and understandable history
- easy debugging and rollback
- alignment with stage-based architecture
- clean main branch history

---

## 11. Summary

The commit strategy:

- enforces atomic and meaningful commits
- aligns commits with stages
- requires squash on merge
- supports reproducibility and maintainability

---

End of document.
