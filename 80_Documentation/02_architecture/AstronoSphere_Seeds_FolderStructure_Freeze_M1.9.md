# AstronoSphere – Seeds Folder Structure Freeze (M1.9)

## 1. Purpose

This document defines the final folder structure and semantics for **Seeds**.

Seeds represent the **starting point of the scientific pipeline**.

They are:
- scenario candidates
- generated or curated
- not yet certified as Experiments

---

## 2. Core Principle

> Seeds are first-class citizens and represent potential experiments.

They must:
- be compatible with Scenario structure
- be ingestible by AstronoLab without transformation
- remain traceable throughout the pipeline

---

## 3. Folder Structure

```text
01_Seeds/
  Incoming/
  Prepared/
  Processed/
```

---

## 4. Semantic Definition

### 4.1 Incoming

Incoming contains all newly created Seeds.

Sources:
- Astronolysis (automated generation)
- Meeus Tool
- manual creation

Characteristics:
- unprocessed
- not reviewed
- not validated

---

### 4.2 Prepared

Prepared contains Seeds processed by AstronoLab.

Characteristics:
- structurally validated
- enriched with required metadata
- ready for certification

Important:
- NOT yet official
- NOT yet part of Experiments

---

### 4.3 Processed

Processed contains Seeds that have been handled by AstronoCert.

Characteristics:
- already certified into Experiments
- no longer part of active pipeline
- kept for traceability

---

## 5. Tool Responsibilities

### AstronoLab

- reads: Incoming
- writes: Prepared

AstronoLab MUST NOT write to:
- Experiments
- GroundTruth
- Simulations
- Results

---

### AstronoCert

- reads: Prepared
- writes:
  - Experiments/Released
  - Processed

AstronoCert is the ONLY component allowed to create Experiments.

---

### Astronolysis

- writes: Incoming
- optionally reads: Experiments, GroundTruth, Simulations

Astronolysis may generate Seeds automatically based on analysis.

---

## 6. Design Rationale

### 6.1 State-based model

Folders represent **state transitions**, not tools.

Incoming → Prepared → Processed

This ensures:
- clarity
- determinism
- auditability

---

### 6.2 Separation of concerns

- Creation (Seeds) is separated from certification (Experiments)
- No tool bypasses AstronoCert

---

### 6.3 Scientific workflow alignment

The structure follows a natural lifecycle:

Seed → Preparation → Certification → Experiment

This reflects a scientific process:
- idea generation
- refinement
- validation

---

## 7. Explicit Rules

- Seeds MUST NOT be written directly to Experiments
- Only AstronoCert may create Experiments
- Seeds MUST remain compatible with Scenario format
- No transformation step between Seeds and Scenario ingestion

---

## 8. Naming Consistency

Seeds use the same structure as ScenarioCandidate:

```json
{
  "Event": { ... },
  "Core": { ... },
  "Metadata": { ... }
}
```

---

## 9. Freeze Decision (M1.9)

- Seeds folder structure frozen
- Naming (Incoming / Prepared / Processed) frozen
- Tool responsibilities frozen
- Pipeline transitions frozen
