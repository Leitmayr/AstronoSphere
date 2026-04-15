# AstronoLab GUI – Specification (M1.9, STRICT BUILD SCOPE)

## 1. Purpose

AstronoLab is a minimal desktop tool for deterministic Seed generation.

Scope:
- fast and controlled creation of Seeds (ExperimentCandidates)
- support pipeline stabilization and validation
- strict adherence to M1.9 architecture

Non-goals:
- no pipeline orchestration
- no batch generation
- no styling or UX optimization

---

## 2. Architectural Context

Pipeline:

Seeds → Experiments → GroundTruth → Simulation → Analysis

AstronoLab responsibility:

- read: 01_Seeds/Incoming
- write: 01_Seeds/Prepared

AstronoLab MUST NOT write to:
- Experiments
- GroundTruth
- Simulations
- Results

---

## 3. Core Principle

Experiment = physical scenario  
Measurement = observation interpretation

Corrections:
- NOT part of Seed (M1.9 decision)
- derived from Measurement (L0 only)

---

## 4. Functional Scope (M1.9)

### 4.1 Seed Creation

User defines:

Core:
- Time
- Observer
- Target
- Frame

Event:
- Category (string)
- Description (optional)

---

### 4.2 Validation

Minimal validation only:

- StartJD > 0
- StopJD >= StartJD
- Core model validity: Core can be serialized deterministically

No further validation in M1.9


---

### 4.3 Duplicate Detection



Duplicate detection MUST be performed via AstronoCert.

AstronoLab MUST NOT:
- perform canonicalization
- compute CoreHash independently
- scan Experiments folders

Instead, AstronoLab calls:

AstronoCert.CheckCoreHash(Core)

AstronoCert responsibilities:
- canonicalize Core (via Contracts)
- compute CoreHash
- search in 02_Experiments/Released
- return result

Response:
- CoreHash
- Exists (true/false)
- CatalogNumber (if exists)

Behavior in UI:
- show warning if Exists = true
- DO NOT block save

This ensures:
- single source of truth for identity
- no duplication of canonicalization logic

---

### 4.4 Flow 

AstronoLab
    ↓
Core (Object)      (+) see Note to Entry
    ↓
AstronoCert
    ↓
HashService (Contracts)
    ↓
Hash
    ↓
Search in Experiments
    ↓
Response

#### (+) Note to Entry:
Core object structure MUST match Experiment Core definition:

- Time
- Observer
- ObservedObject
- Frame

Naming MUST be identical to final JSON structure. 

### 4.5 Technical Sequence

User changes Input
    ↓
AstronoLab builds a deterministic Core model
from UI inputs.
    ↓
Call: AstronoCert.CheckCoreHash(Core)
    ↓
Response:
    - CoreHash
    - Exists
    - CatalogNumber
    ↓
UI Update

#### Notes:

Trigger:

Duplicate check MUST be triggered on:
- any Core input change
- after loading a Seed

Calls SHOULD be debounced (e.g. 300ms)
to avoid excessive requests.

### 4.6 Architectural responsibilities

Some further context:

| Component  |	Responsibility              |
| ---------- | ---------------------------- |
| Contracts  | calcualted Hash  (low-level) |
| AstronoCert| determine Identity           | 
| AstronoLab | just displaying              |

### 4.7 Save

Output:

01_Seeds/Prepared

Format:
- NO Corrections block
- deterministic structure
- completely determined from Core and Event
- independent from catalog


Not contained:
- no dataset
- no provider (Horizons, ...)
- no measurement
- no Hash


File Naming (Example): 
PLANET-MERCURY-INC__HELIO-J2000-TDB-2451545-2451546-1H.json

---

## 5. UI Layout

### 5.1 Structure

LEFT: Core Input  
RIGHT: Preview + Validation

---

### 5.2 Core Input Controls


| [Load Seed] |  [New]  | [Save → Prepared] |
| ----------- | ------- | ----------------- | 


| CORE INPUT                              | PREVIEW / VALIDATION                  |
|----------------------------------------|----------------------------------------|
|                                        |                                        |
| Time                                   | JSON Preview (readonly)                |
| StartJD:  [2451545.0        ]          | {                                      |
| StopJD:   [2451546.0        ]          |   "Event": { ... },                    |
| Step:     [1H              ]           |   "Core":  { ... }                     |
|                                        | }                                      |
| Observer                               |                                        |
| Type:   [Heliocentric ▼]               | -------------------------------------- |
| Body:   [Sun ▼]                        | CoreHash: e.g. 8F3A1C... --> CoreHash MUST be taken from AstronoCert response                    |
|                                        | Status:  ✅ UNIQUE                     |
| Target                                 |          ⚠ DUPLICATE (AS-000123)       |
| BodyClass: [Planet ▼]                  |                                        |
| Target:    [Mars ▼]                    | Validation:                            |
|                                        | - JD OK                                |
| Frame                                  | - JSON OK                              |
| Type:   [Ecliptic ▼]                   |                                        |
| Epoch:  [J2000 ▼]                      |                                        |
|                                        |                                        |
|----------------------------------------|----------------------------------------|
| EVENT / METADATA (minimal)             |                                        |
| Category:   [INC        ]              |                                        |
| Description:[optional...]              |                                        |
+----------------------------------------------------------------------------------+

#### Time
- StartJD (TextBox)
- StopJD (TextBox)
- Step (TextBox, e.g. "1H")

#### Observer
- Type (ComboBox): Heliocentric / Geocentric
- Body auto-set:
  - Heliocentric → Sun
  - Geocentric → Earth

#### Target
- BodyClass (fixed: Planet)
- Target (ComboBox): Mercury → Neptune

#### Frame
- Type (ComboBox): Ecliptic / Equatorial
- Epoch (ComboBox): J2000

---

### 5.3 Event Input

- Category (TextBox)
- Description (TextBox, optional)

---

### 5.4 Preview Panel (READONLY)

Displays:

- full JSON (Event + Core)
- CoreHash
- Status:
  - VALID / INVALID
  - UNIQUE / DUPLICATE

---

## 6. Constraints

- deterministic behavior required
- no hidden transformations
- no implicit defaults
- KISS principle

---

## 7. Technical Constraints

- WPF
- no external UI frameworks
- minimal MVVM
- single window sufficient

---

## 8. Definition of Done

- Seed can be created and saved
- JSON is valid and deterministic
- Hash matches canonical rules (derived from AstronoCert)
- Duplicate detection works
- no deviation from architecture