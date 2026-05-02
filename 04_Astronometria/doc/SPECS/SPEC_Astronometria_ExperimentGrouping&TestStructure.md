# AstronoSphere – Experiment Grouping & Test Structure (M2.2 Spec Draft)

## Purpose

This document defines the temporary (M2.2) and future (post-M2.x) approach  
for organizing, grouping, and selecting Experiments for validation and testing.

Goals:
- maintain KISS and Stealth Mode discipline
- enable deterministic and reproducible test subsets
- avoid premature architectural complexity
- prepare a clean transition to a future ExperimentExplorer

---

# 1. Core Problem

We need a way to:

- group Experiments into meaningful validation sets
- run targeted subsets (e.g. Holy12, Mesh)
- avoid ambiguous constructs like:
  "Catalog = everything else"

Additionally, we need to answer future questions such as:

- How many experiments exist?
- Which are Released vs Deprecated?
- Which belong to which validation group?
- Which subsets should be executed in pipelines?

---

# 2. Constraints (Stealth Mode)

- one dimension at a time
- no premature architecture
- validation-first approach
- no side-systems (e.g. Explorer, GUI)

Conclusion:

No dynamic grouping system in M2.2.

---

# 3. M2.2 Solution – Static ExperimentSetMapper

## 3.1 Principle

Introduce a simple, explicit, deterministic mapping:

CatalogNumber → ExperimentSet

Where:

ExperimentSet ∈ { Holy12, Mesh, Catalog }

MUST throw exception if CatalogNumber is not mapped.

---

## 3.2 Location

10_AstronoData.Contracts
  /Domain
    ExperimentSet.cs
    ExperimentSetMapper.cs

Reason:

- grouping is domain knowledge
- must be shared across:
  - Astronometria
  - TestFramework
  - future tools

- must NOT belong to:
  - AstronoCert (certification only)
  - AstronoData.IO (I/O only)

---

## 3.3 Design Rules

- MUST use explicit mapping (allowlist style)
- MUST NOT infer grouping from:
  - filename
  - category
  - mesh type
  - path
- MUST be deterministic
- MUST be stable across runs

---

## 3.4 Important Clarification

Catalog is NOT defined as "everything else".

Instead:

- Catalog is explicitly mapped
- no implicit fallback logic

This avoids:

- hidden assumptions
- non-deterministic grouping
- future inconsistencies

---

# 4. Test Structure (L0)

## 4.1 Folder Layout

/→ 04_Astronometria/tests
  /L0
    /Holy12
    /Mesh
    /Catalog
    /Regression
    /Results

Experiment input
→ AstronoData/02_Experiments/Released/

Grouping
→ AstronoData.Contracts/Domain/ExperimentSetMapper.cs

GroundTruth reference
→ AstronoData/03_GroundTruth/Ephemeris/Horizons/Baseline/

Execution
→ Astronometria engine, L0, in-memory


Comparison
→ engine result vs Horizons Baseline

No output
→ no production write to 04_Simulations in M2.2


TestFramework MUST:
- process the data in ascending CatalogNumber order
- use AstronoData.IO to load all Released experiments -> no direct file access
- filter via ExperimentSetMapper
- execute ONLY selected set:
  - run Astronometria Algo
  - compare Astronometria results with GroundTruth
  - generate asserts based on compare


Compare:
- compare considers the tolerances allowed between Horizons and VSOP (Astronometria Engine used for the tests).
- Tolerances are derived and specified in 17_VSOP87A_Position+Velocity_Tolerance_Derivation.md.
- Matching MUST be based on Experiment identity: CatalogNumber + ExperimentID
- Comparison MUST be performed per sample (time step)
- Comparison MUST be vector-based: for M2.2 position only

---

## 4.2 Group Semantics

### 4.2.1 Holy12:
- curated trust / golden reference cases
- Set_Holy12 = {  AS-000001, AS-000002, AS-000003, AS-000004,
                  AS-000005, AS-000006, AS-000007, AS-000008,
                  AS-000009, AS-000010, AS-000011, AS-000012}

### 4.2.2 Mesh:
- large-scale systematic validation
- Set_Mesh = {  AS-000146, AS-000147, ... AS-000374}

For machines: Set_Mesh = AS-000146 through AS-000374 inclusive, step size 1


### 4.2.3 Catalog:
- regular experiments outside special sets
- Set_Catalog = { AS-000013, AS-000014, ... AS-000072} \ { AS-000020, AS-000034, AS-000048 } 

Notes about Set_Catalog:
- AS-000020, AS-000034, AS-000048 are not part of Set_Catalog. 
- AS-000020, AS-000034, AS-000048 are deprecated experiments as well and not part of any experiment set. 
- AS-000073, AS-000074, ..., AS-000145 are deprecated experiments as well and not part of any experiment set.


### 4.2.4 Regression:
- protection of technical invariants

Regression is a test category, not an ExperimentSet group.



### 4.2.5 General Rules
Notes:
- Each Released Experiment MUST belong to exactly ONE ExperimentSet
- Each Deprecated Experiment MUST NOT belong to ANY ExperimentSet


For M2.2:
Mapping is the ONLY source of grouping truth
Filesystem MUST NOT be used for grouping


---

## 4.3 Key Principle

Test folders are organized by validation purpose, not by category.

---

# 5. Regression Definition

Regression is NOT a fallback group.

Regression tests protect already validated behavior.

MUST compare:
- filename (string equality)
- dataset content (byte-level)
- hash values (exact match)

They must fail if:
- hashes change
- filenames change
- dataset structure changes
- mapping logic changes
- determinism breaks

Regression does NOT contain:
- Mesh tests
- Holy12 tests
- Catalog experiments

---

# 6. Usage in Astronometria

The TestFramework uses:

ExperimentSetMapper → filter experiments → run subset

Examples:
- run Holy12 only
- run Mesh only
- run Catalog only
- combine subsets

Note about combine: 
- Combination = UNION of sets
- duplicates MUST NOT occur

---

# 7. Future Architecture (NOT in M2.2)

Future components MUST NOT influence M2.2 implementation

## 7.1 Problem to Solve Later

Static mapping cannot answer:
- total experiment count
- maturity distribution
- dynamic subsets
- user-defined queries

---

## 7.2 Future Component

ExperimentExplorer (new VS project)

Responsibilities:
- read experiment inventory
- filter by:
  - Maturity (Released / Deprecated)
  - Category
  - Time ranges
  - user-defined rules
- build dynamic subsets

---

## 7.3 Catalog Index Concept

Future component:

ExperimentCatalogIndex

Responsibilities:
- discover experiments from:
  - 02_Experiments/Released
- provide queries:
  - all experiments
  - released only
  - deprecated only
  - subset membership
- validate grouping completeness

---

## 7.4 Architectural Separation

AstronoCert:
- certification only

AstronoData.Contracts:
- domain models + static mapper

AstronoData.IO:
- file access only

ExperimentExplorer:
- grouping, querying, user interaction

---

# 8. Transition Strategy

M2.2:
- static mapping only
- explicit allowlist
- no inventory logic

Post-M2.x:
- introduce ExperimentExplorer
- replace static mapper with dynamic grouping
- optionally generate mapper from configuration

---

# 9. Design Philosophy

## 9.1 Determinism First

Grouping must be:
- explicit
- reproducible
- stable

---

## 9.2 KISS

- no dynamic rules in M2.2
- no inference logic
- no hidden classification

---

## 9.3 Separation of Concerns

- certification ≠ grouping
- I/O ≠ domain logic
- domain logic ≠ UI/query system

---

# 10. Final Principle

In M2.2, experiment grouping is explicit and static.  
In the future, it becomes dynamic and query-driven, but in a separate system.