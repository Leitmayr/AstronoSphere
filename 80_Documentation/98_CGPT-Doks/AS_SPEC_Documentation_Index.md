# AstronoSphere – Documentation Index (M1.9 / M2 Ready)

## Purpose

This document defines the role of each document in the AstronoSphere specification set.

It ensures:

* clarity of responsibilities
* no overlap between documents
* consistent usage during implementation

---

# 1. CORE – System Definition (WHAT the system is)

## CORE_AstronoSphere_Pipeline_DataModel_V1.1.md

Defines the **full pipeline structure and data flow**.

* Seeds → Experiments → GroundTruth → Simulations → Results
* Folder structure (01–05)
* Tool responsibilities
* Naming conventions
* Pipeline transitions

> Primary reference for system architecture

---

## CORE_CanonicalDataExamples_DS23.md

Provides **one complete canonical dataset across all pipeline stages**.

* Incoming Seed
* Prepared Seed
* Experiment
* GroundTruth

> Primary reference for real data structures and validation
> If output differs from this → system is wrong

---

## CORE_AstronoSphere_HashSpec_M1.9.md

Defines **canonicalization and hashing rules**.

* SHA256 rules
* canonical string generation
* numeric normalization (9 decimals)
* sorting rules
* CoreHash / RequestHash / SnapshotHash

> Foundation of determinism and reproducibility

---

## CORE_AstronoMeasurement_Concept_V1.1.md

Defines **measurement semantics (Instrument layer)**.

* separation of Experiment vs Measurement
* L0 / L1 / L2 definition
* mapping to:

  * Engine
  * GroundTruth
* Dataset comparability

> Connects physics, truth, and simulation

---

# 2. PHYSICS – Scientific Model (HOW reality is modeled)

## CORE_LightTimeFundamentalConcept.md

Defines **Light-Time correction (L1)**.

* physical model
* iterative solver
* convergence criteria
* integration into pipeline
* edge cases (Neptune, Mercury, RAWrap)

> First non-trivial physical correction

---

## PHYS_Astronometria_TimeArchitecture.md

Defines **time domain separation**.

* UTC (User Domain)
* TT (Astro Domain)
* Conversion layer (ΔT)
* strict dependency rules

> Foundation of all ephemeris correctness

---

# 3. STRATEGY – Execution Plan (WHAT is built WHEN)

## COLLAB_AstronoSphere_Level1-3_Milestones2x.md

Defines:

### Levels

* Level 1 → Scientific Closure
* Level 2 → Observation Bridge
* Level 3 → Experiential System

### M2.x Plan

* M2.1 Mesh (L0)
* M2.2 Light-Time (L1)
* M2.3 Aberration (L2)
* M2.4 System State
* M2.5 Trust Validation

> Primary roadmap for development

---

# 4. META – Development Discipline (HOW we work)

##

Defines **Stealth Mode rules**.

* strict scope limitation
* no GUI / API / distractions
* one dimension at a time
* validation-first principle

> Prevents scope drift

---

## COLLAB_EfficiencyPackage.md 

Defines **collaboration rules**.

* no trial & error
* full file updates
* short responses
* milestone-based workflow
* German chat / English docs

> Ensures efficient implementation

---

## COLLAB_GenericKISSCodingChart_V1.1.md 

Defines **coding principles**.

* KISS (simple & stable)
* determinism
* separation of concerns
* no hidden logic
* reproducibility

> Governs code quality

---

# 5. SYSTEM VIEW (Summary)

AstronoSphere is defined by four orthogonal layers:

---

## 5.1 Structure (DataModel)

→ what exists

## 5.2 Instance (Canonical Examples)

→ how it looks in reality

## 5.3 Physics (LightTime + TimeArchitecture)

→ how reality is modeled

## 5.4 Execution (Milestones + Stealth + Efficiency + KISS)

→ how it is built

---

# 6. Usage Rules

## During Implementation

* Structure → DataModel
* Data → Canonical Examples
* Physics → LightTime / TimeArchitecture
* Naming / Hashing → HashSpec

---

## During Validation

* Compare against Canonical Examples
* Verify hashes via HashSpec
* Ensure Run == LastRun

---

## During Planning

* Follow Milestones (M2.x)
* Respect Stealth Mode

---

# 7. Final Principle

> The system is fully defined only by the combination of all documents.

* No single document is sufficient
* No document may contradict another
* All documents together define the system boundary

---

# End of Document
