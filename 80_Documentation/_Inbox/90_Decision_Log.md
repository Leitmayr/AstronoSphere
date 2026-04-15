# 90_Decision_Log.md

## Purpose
Tracks all major architectural decisions.

---

## Decision Template

Decision:
Why:
Alternatives:
Consequences:
Date:

---

## Decisions

### D-001 Light-Time as Time Solver

Decision:
Light-Time implemented as time iteration, not spatial correction.

Why:
Physically correct modeling of emission time.

Alternatives:
Treat as vector correction.

Consequences:
Cleaner architecture, better extensibility.

Date:
2026-03

---

### D-002 Strict System Separation

Decision:
Separate Astronometria, AstronoData, AstronoFactories.

Why:
Scientific traceability and reproducibility.

Alternatives:
Monolithic system.

Consequences:
Higher clarity, slightly more structure.

Date:
2026-03

---

### D-003 No logic in factories

Decision:
Take out Event Finder from Ephemeris Regression.

Why:
- Factories are purely interface to Ground Truth data base. Not more.
- Scenario based approach used instead

Alternatives:
EventFinders in Factories

Consequences:
Higher clarity, more structure, separation of concerns.

Date:
2026-03

### D-004 AstronoLab to become only entity for scenario creation

Decision:
AstronoLab creates scenario data. ScenarioHeaderGenerator (in future: AstronoCert) will only generate CoreHash, ScenarioID and CatalogNumber.

Why:
- Full automation of pipeline form beginning
- no intermediate review steps to stop the pipeline flow

Alternatives:
ScenarioHeaderGenerator to also define scenario content (as is today).

Consequences:
Higher clarity, higher automation.

Date:
2026-04

### D-005 Simplification of ScenarioHeader

Decision:
Redundant information and information not fitting to Scenario character were taken out ot the ScenarioHeader, e.g. ScenarioType (redundant), Scientific purpose (redundant), CorrectionLevels (no fitting).

Why:
- keep header short and lean
- Correction Level removed: the scenario = experiment is purely defined by geometry and not by light time, etc.

Alternatives:
keep it as is today: overloaded.

Consequences:
Higher clarity, leaner, curation easier.

Date:
2026-04

### D-005 DatasetHeader generation in AstronoTruth

Decision:
Scenario should only contain scenario header, not dataset header. 

Why:
- AstronoTruth is the only instance knowing about the header information. DatasetHeader cannot be set a priori at scenario definition.

Alternatives:
keep it as is today: overloaded.

Consequences:
Higher clarity, leaner, separation of concerns.

Date:
2026-04

### D-006 Renaming of the components of AstronoSphere

Decision:
Tools have a Astrono* Prefix. Data not. 

Why:
- Astrono* prefix supports the "AstronoSphere-brand".
- Data with Astrono* Prefix would be "too much Astrnon" (inflation).

Alternatives:
- stick to old, inconsistent naming

Consequences:
Higher clarity, supports Framework identity of "Astrono"Sphere.

Date:
2026-04