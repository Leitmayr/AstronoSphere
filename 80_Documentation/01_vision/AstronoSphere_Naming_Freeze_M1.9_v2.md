# AstronoSphere Naming Freeze – M1.9 (Extended Rationale)

## 1. Purpose

This document defines the **final naming conventions** for the AstronoSphere ecosystem for milestone **M1.9**, including the **rationale behind each naming decision**.

The goal is to establish:

- Clear separation between **Tools (brand-driven)** and **Data (semantic-driven)**
- Consistent and fluent naming
- Long-term stability for architecture, APIs, and documentation
- Strong conceptual metaphors supporting the system

---

## 2. Core Naming Principle

### 2.1 Tool Naming (Brand Layer)

All tools use the **Astrono*** prefix.

Rationale:
- Establishes a strong ecosystem identity
- Groups all active system components under one recognizable brand
- Allows intuitive association of responsibilities

Key decision:
- Avoid artificial CamelCase splits (e.g. NOT AstronoMetria)
- Prefer natural, pronounceable names

### Final Tool Names

- Astronometria
- Astronolysis
- AstronoCert
- AstronoLab
- AstronoTruth
---

### 2.2 Data Naming (Scientific Layer)

Data is **not branded**, but named according to meaning.

Naming rules:

- Scientific clarity over branding
- Plural form for folders
- Singular form for entities

Rationale:
- Data must remain scientifically interpretable
- Avoids coupling data structures to implementation details
- Enables external usability and long-term stability

---

## 3. Data Structure and Rationale

### 01_Seeds

**Meaning:**
Initial inputs into the system.

**Rationale:**
- Replaces "Candidates"
- Introduces a **biological/agricultural metaphor**

A seed:
- contains potential
- is not yet a validated entity
- can grow into a full experiment

This metaphor supports the system loop:

Seed → Experiment → Result → Seed

It also reflects:
- iteration
- growth
- continuous refinement

---

### 02_Experiments

**Meaning:**
Validated, official scientific scenarios.

**Rationale:**
- Replaces "Scenario"
- Stronger alignment with scientific language
- Emphasizes reproducibility and intent

An Experiment:
- is defined
- is controlled
- can be reproduced and validated

---

### 03_GroundTruth

**Meaning:**
Reference data from external authoritative sources.

**Rationale:**
- Replaces "ReferenceData"
- "GroundTruth" is scientifically precise
- Aligns with validation and measurement theory

GroundTruth represents:
- the external reference baseline
- the "measured reality" against which models are tested

---

### 04_Simulations

**Meaning:**
Outputs of Astronometria.

**Rationale:**
- Direct and intuitive
- Clearly separates model output from truth

---

### 05_Results

**Meaning:**
Outputs of Astronolysis.

**Rationale:**
- Replaces "AnalysisReports"
- Shorter, clearer, and more scientific

A Result is:
- the outcome of validation
- not just a document, but a conclusion

---

## 4. Tool Naming and Rationale

### Tool Responsibilities

| Tool | Responsibility |
|------|---------------|
| AstronoLab | Generates Seeds |
| AstronoCert | Converts Seeds into official Experiments |
| AstronoTruth | Generates GroundTruth (e.g. Horizons provider) |
| Astronometria | Computes Simulations |
| Astronolysis | Produces Results and new Seeds |

### AstronoLab

**Role:**
Generates Seeds.

**Rationale:**
- Represents a laboratory environment
- Supports experimentation and generation
- Flexible for multiple generation strategies

---

### AstronoCert

**Role:**
Transforms Seeds into official Experiments.

**Rationale:**
This naming required careful refinement.

The tool does NOT:
- generate data
- modify the experiment content

It DOES:
- validate
- approve
- formalize

The term "Cert" was chosen because:

- Derived from "Certification"
- Implies official validation
- Short, fluent, brand-compatible
- Stronger than "Builder", which implies construction

Conceptual analogy:
- Like an authority stamping an official document
- Turning a draft into a recognized entity

---

### AstronoTruth

**Role:**
Generates GroundTruth data.

**Rationale:**
- Keeps "Truth" as a central scientific concept
- Avoids technical bias like "Factory"
- Clean abstraction for multiple providers

Example:
AstronoTruth.Horizons

---

### Astronometria

**Role:**
Core computation engine.

**Rationale:**
- Established, strong identity
- Already aligned with scientific terminology
- No change required

---

### Astronolysis

**Role:**
Analysis and validation.

**Rationale:**
- Derived from "analysis" but more fluid and brandable
- Strong phonetic identity
- Fits naturally into Astrono naming family

Produces:
- Results
- new Seeds (feedback loop)

---

## 5. Key Design Decisions

- "Experiment" replaces "Scenario" for scientific clarity
- "Seed" replaces "Candidate" for pipeline consistency
- "GroundTruth" replaces "ReferenceData" for scientific precision
- "Results" replaces "AnalysisReports" for simplicity and clarity

## 6. System Loop (Conceptual Model)

Seed → Experiment → GroundTruth → Simulation → Result → Seed

Rationale:
- Closed validation loop
- Self-extending system
- Reflects scientific method:
  - hypothesis (Seed)
  - experiment
  - measurement
  - evaluation
  - refinement

---

## 7. Structural Conventions

### Folder Naming

- Always plural (e.g. Experiments)
- Represents collections

### Entity Naming

- Always singular (e.g. Experiment)
- Represents individual objects

---

## 8. Summary

This naming system achieves:

- Strong branding for tools
- Scientific clarity for data
- Clear separation of responsibilities
- Intuitive mental model
- Long-term scalability

It transforms AstronoSphere into:

**a coherent scientific laboratory system with a natural lifecycle metaphor**

---

End of Document
