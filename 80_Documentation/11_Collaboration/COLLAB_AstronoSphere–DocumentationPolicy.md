# AstronoSphere – Documentation Policy (M2+)


## ChangeLog

ID   | Version | Change                       | Date         |
--   | ------- | ---------------------------- | -------------|
1    |   1.0   | Initial revision             |  2026-04-23  |

## Purpose

This document defines the documentation strategy for AstronoSphere.

Goals:

* preserve **scientific integrity**
* ensure **deterministic reproducibility**
* enable **high-speed implementation without knowledge loss**
* prevent **spec divergence**

---

# 1. Core Principle

> The system is defined ONLY by `/80_Documentation`.

All other documentation layers exist to:

* support implementation
* capture knowledge
* enable validation

They MUST NOT redefine system behavior.

---

# 2. Documentation Layers

AstronoSphere uses a strict **3-layer documentation model**:
Documentation format shall be md-file, preferrably.

---

## 2.1 System Layer (Single Source of Truth)

```
/80_Documentation
```

Contains:

* CORE (DataModel, HashSpec, Measurement)
* PHYS (Time, LightTime)
* COLLAB (Stealth, Efficiency, KISS)
* AS_SPEC (Index, integration rules)

### Rules

* defines 
    * system behavior 
    * processes and workflows
    * collaboration policies
* globally valid
* versioned and stable
* created in META / ARCH context only

### Forbidden

* duplication in project docs
* local overrides
* implicit extensions

---

## 2.2 Project Layer (Local Documentation)

```
/<Project>/doc
```

Purpose:

* support implementation
* capture local knowledge
* document validation behavior

### Structure

```
/doc
    /SPECS
    /NOTES
    /VALIDATION
    README.md
```

Example in AstronoTruth:

/03_Astronotruth
    /src
        /EphemerisFactory
            EphemerisFactory.csproj // this is were the project resides
            /doc
                /NOTES
                /SPECS                
                /VAL
                Readme.md


---

### 2.2.1 `/doc/SPECS`

Contains:

* implementation-specific mappings
* concrete realizations of system specs

Examples:

* Horizons request mapping
* DatasetHeader mapping
* Engine integration contracts

#### Rules

* MUST reference system specs
* MUST NOT redefine system rules
* MUST remain project-scoped
* MUST contain an change log at the beginning of the document

#### Template

```md
Rule:
<concrete implementation rule>

Reference:
<CORE/PHYS document>

Rationale:
<short explanation>
```

---

### 2.2.2 `/doc/NOTES`

Contains:

* sprint notes
* design thoughts
* intermediate reasoning
* chat-derived knowledge

#### Rules

* may be incomplete or "dirty"
* no obligation for structure completeness
* serves as thinking space

#### Naming

```
YYYY-MM-DD_<Topic>.md
```

#### Minimal Template

```md
# Context
<short description>

# Key Decisions
- ...

# Open Questions
- ...

# Insights
- ...

# Next Steps
- ...
```

---

### 2.2.3 `/doc/VALIDATION`

Contains:

* validation strategies
* golden samples
* edge cases
* observed system behavior

#### Purpose

This is the **core layer for scientific validation**.

#### Template

```md
Case: <Name>

Observation:
<what was observed>

Impact:
<why it matters>

Status:
Confirmed / Open
```

---

### 2.2.4 `/doc/README.md`

Defines local documentation rules.

#### Mandatory Content

```md
# <ProjectName> – Local Documentation

## Scope
This folder contains project-local documentation.

## Rules
- MUST NOT redefine system-level specifications (/80_Documentation)
- MUST reference system specs where applicable
- Exists only to support implementation and validation

## Structure
- SPECS → implementation-specific mappings
- NOTES → temporary sprint knowledge
- VALIDATION → validation logic
```

---

# 3. Knowledge Promotion Model

AstronoSphere uses a **progressive knowledge elevation model**:

---

## 3.1 Levels

| Level      | Description            |
| ---------- | ---------------------- |
| NOTES      | raw thinking           |
| VALIDATION | verified behavior      |
| SPECS      | defined implementation |
| /80        | system-level law       |

---

## 3.2 Promotion Triggers

Promote knowledge if:

* repeated explanation is required
* behavior is critical for correctness
* knowledge is likely to be forgotten

---

## 3.3 Promotion Rules

### Rule 1 – Minimal Effort

Promotion must take **< 30 seconds**

No overhead allowed.

---

### Rule 2 – Target Selection

| Target     | Use when              |
| ---------- | --------------------- |
| NOTES      | raw idea              |
| VALIDATION | reproducible behavior |
| SPECS      | implementation rule   |
| /80        | system-wide relevance |

---

### Rule 3 – No Over-Promotion

> If unsure → do NOT promote

Clarity is more important than completeness.

---

## 3.4 Promotion Templates

### VALIDATION

```md
Case: <Name>

Observation:
<1 sentence>

Impact:
<why relevant>

Status:
Confirmed / Open
```

---

### SPECS

```md
Rule:
<implementation rule>

Reference:
<system spec>

Rationale:
<1 sentence>
```

---

# 4. Hard Constraints

## 4.1 No Duplication

* system rules must exist only once
* duplication leads to divergence → forbidden

---

## 4.2 Reference Requirement

Every SPECS entry MUST reference:

* CORE or PHYS document

---

## 4.3 Promotion Rule

If knowledge becomes:

* cross-project relevant
* system-critical

→ MUST be moved to `/80_Documentation`

---

## 4.4 No Trial-and-Error Documentation

Documentation must reflect:

* understood behavior only
* validated knowledge

---

# 5. Alignment with System Principles

This policy enforces:

---

## Determinism

* clear rules
* no ambiguity
* reproducible knowledge

---

## KISS

* minimal structure
* no over-engineering
* only necessary layers

---

## Stealth Mode

* focus on validation
* no unnecessary documentation
* no distraction from core work

---

## Scientific Integrity

* explicit reasoning
* traceable decisions
* reproducible validation

---

# 6. Final Principle

> Documentation is more than only a description of the system.



---

# End of Document
