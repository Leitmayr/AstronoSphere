# CM-01 Repository Structure

## 1. Purpose

This document defines the repository structure of the AstronoSphere ecosystem.

The goal is to ensure:

- deterministic navigation
- clear ownership of data and code
- long-term maintainability
- alignment with the AstronoSphere processing pipeline

---

## 2. Top-Level Structure

```
AstronoSphere
|
+- 00_AstronoPipe.pj
+- 01_MeeusScenarioFactory.pj
+- 02_ScenarioHeaderGenerator.pj
+- 03_TruthFactory.pj
+- 04_Astronometria.pj
+- 05_AnalysisTool.pj
|
+- 10_AstronoData.Contracts.pj
+- 11_AstronoData.IO.pj
|
+- 80_Documentation.pj
|
+- AstronoData.pj
```

---

## 3. Numbering Convention

### 3.1 Pipeline (00–09)

Defines the execution flow of AstronoSphere.

| Number | Component |
|--------|----------|
| 00 | AstronoPipe (orchestration) |
| 01 | MeeusScenarioFactory |
| 02 | ScenarioHeaderGenerator |
| 03 | TruthFactory |
| 04 | Astronometria |
| 05 | AnalysisTool |

Rules:

- Numbers represent execution order
- Numbers must never change
- New components are appended

---

### 3.2 Data Access Layer (10–19)

| Number | Component |
|--------|----------|
| 10 | AstronoData.Contracts |
| 11 | AstronoData.IO |

Responsibilities:

- Contracts: interfaces and DTOs
- IO: file system and serialization logic

---

### 3.3 Documentation (80–89)

| Number | Component |
|--------|----------|
| 80 | Documentation |

Documentation is not part of the runtime pipeline.

---

## 4. AstronoData Structure

```
AstronoData.pj
|
+- 01_CandidateData
+- 02_ObservationCatalog
+- 03_ReferenceData
|   +- Runs
|   |   +- Run
|   |   +- LastRun
|   +- Released
|
+- 04_EngineData
|   +- Runs
|   +- Released
|
+- 05_AnalysisData
    +- Runs
    +- Released
```

---

## 5. Data Ownership Model

Each data block has exactly one writer (owner).

| Data Block | Owner |
|-----------|------|
| 01_CandidateData | MeeusScenarioFactory |
| 02_ObservationCatalog | ScenarioHeaderGenerator |
| 03_ReferenceData | TruthFactory |
| 04_EngineData | Astronometria |
| 05_AnalysisData | AnalysisTool |

Rules:

- Only the owner may write
- All other components may read
- No cross-writing allowed

---

## 6. Pipeline Mapping

The numbering of code and data is aligned:

| Code | Data |
|------|------|
| 01 | 01_CandidateData |
| 02 | 02_ObservationCatalog |
| 03 | 03_ReferenceData |
| 04 | 04_EngineData |
| 05 | 05_AnalysisData |

This creates a direct mapping between processing step and data output.

---

## 7. Design Rules

### 7.1 Data Purity

AstronoData contains:

- no code
- no business logic
- no APIs

It is a pure data container.

---

### 7.2 Separation of Concerns

- Data → AstronoData
- Processing → Pipeline components
- Access → Contracts + IO

---

### 7.3 Stability

- Folder names are stable
- Numbering is immutable
- Structure evolves only by extension

---

## 8. Documentation Structure

```
80_Documentation.pj
|
+- 00_core
+- 01_vision
+- 02_architecture
+- 03_scenarios
+- 04_data
+- 05_factories
+- 06_validation
+- 07_reports
+- 08_principles
+- 09_development
|   +- 01_PM
|   +- 02_CM
+- 10_diagrams
+- 98_CGPT-Top20
+- history
```

Configuration management documents are stored in:

```
09_development/02_CM
```

---

## 9. Summary

This structure:

- reflects the AstronoSphere workflow
- enforces clear ownership
- minimizes cognitive load
- ensures long-term maintainability

The dual numbering system (code + data) provides a direct mental mapping of the pipeline.

---

End of document.

