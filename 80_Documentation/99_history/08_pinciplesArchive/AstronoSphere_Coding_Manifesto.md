# AstronoSphere Coding Manifesto

Version: 1.0  
Status: Active  
Scope: All AstronoSphere codebases

---

# 1. Purpose

This document defines the coding principles for AstronoSphere.

Goal:

- ensure long-term maintainability
- ensure scientific traceability
- ensure deterministic behavior
- enable external developers to understand and extend the system

---

# 2. Core Principles

## 2.1 KISS

Keep It Simple and Stable.

- prefer simple solutions
- avoid unnecessary abstraction
- avoid clever code

Clarity is more important than brevity.

---

## 2.2 Determinism First

All code must produce identical results for identical inputs.

- no hidden state
- no implicit behavior
- no environment-dependent output

---

## 2.3 Separation of Concerns

Strict separation between:

- Scenario (physical experiment)
- Factory (measurement)
- Engine (computation)

---

## 2.4 Scientific Integrity

- no silent changes
- no implicit corrections
- no untracked improvements

---

# 3. Commenting Rules

## Code vs Comments

- Code explains HOW  
- Comments explain WHY  

## Purpose Header (mandatory)

Each core class must define:

PURPOSE  
CONTEXT  
CONSTRAINTS  

---

# 4. Coding Rules

- no hidden logic  
- no implicit defaults  
- no side effects in core computations  

---

# 5. Determinism Rules

- canonical serialization  
- byte-level regression  
- identical structure required  

---

# 6. Architecture Reference

Code must reference md architecture documents explicitly.

---

# 7. Definition of Done

- deterministic  
- documented  
- reproducible  
- regression stable  

---

End of document.
