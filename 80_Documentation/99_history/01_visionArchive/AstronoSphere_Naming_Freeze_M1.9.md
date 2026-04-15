# AstronoSphere Naming Freeze – M1.9

## 1. Purpose

This document defines the **final naming conventions** for the AstronoSphere ecosystem for milestone **M1.9**.

The goal is to establish:

- Clear separation between **Tools (brand-driven)** and **Data (semantic-driven)**
- Consistent and fluent naming
- Long-term stability for architecture, APIs, and documentation

---

## 2. Core Naming Principle

### 2.1 Tool Naming

All tools use the **Astrono*** prefix as brand identity.

Naming rules:

- Natural language flow (no forced CamelCase splits)
- No artificial separation (e.g., NOT AstronoMetria)
- Must be pronounceable and intuitive

### Final Tool Names

- Astronometria
- Astronolysis
- AstronoCert
- AstronoLab
- AstronoTruth

---

### 2.2 Data Naming

Data is **not branded**, but named according to scientific meaning.

Naming rules:

- Scientific clarity over branding
- Plural form for folders
- Singular form for entities

### Final Data Structure

01_Seeds  
02_Experiments  
03_GroundTruth  
04_Simulations  
05_Results  

---

## 3. System Mapping

### Pipeline Overview

Seed → Experiment → GroundTruth → Simulation → Result → Seed

---

### Tool Responsibilities

| Tool | Responsibility |
|------|---------------|
| AstronoLab | Generates Seeds |
| AstronoCert | Converts Seeds into official Experiments |
| AstronoTruth | Generates GroundTruth (e.g. Horizons provider) |
| Astronometria | Computes Simulations |
| Astronolysis | Produces Results and new Seeds |

---

## 4. Structural Convention

### Namespacing

Hierarchical structure is expressed using dot notation:

Example:

AstronoTruth.Horizons

---

### Folder vs Entity

| Type | Naming |
|------|--------|
| Folder | Plural (e.g. Experiments) |
| File/Object | Singular (e.g. Experiment) |

---

## 5. Key Design Decisions

- "Experiment" replaces "Scenario" for scientific clarity
- "Seed" replaces "Candidate" for pipeline consistency
- "GroundTruth" replaces "ReferenceData" for scientific precision
- "Results" replaces "AnalysisReports" for simplicity and clarity
- Tools remain branded, data remains semantic

---

## 6. Summary

This naming system ensures:

- Strong and consistent branding
- Scientific clarity of data structures
- Clean separation of responsibilities
- Long-term maintainability

---

End of Document
