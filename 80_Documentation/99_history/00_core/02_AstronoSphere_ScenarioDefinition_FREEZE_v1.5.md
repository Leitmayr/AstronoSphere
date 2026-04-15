# AstronoSphere Scenario Definition Specification (M1)

Version: 1.4\
Status: Architecture Freeze

This document defines the **scenario structure used across the
AstronoSphere ecosystem**. It specifies the Scenario format used by the
**AstronoSphere ObservationCatalog**.

Scenarios describe **astronomical experiments**.\
Factories generate **reference measurements** for those experiments.

The architecture strictly separates:

-   **Scenario Core** → Physical experiment definition
-   **Factory Block** → Measurement / reference data generation

Factories must **never modify the Scenario Core**.

------------------------------------------------------------------------

SchemaVersion: 1.0

# 1. Core Principles

## 1.1 Scenario Core

The **Core** describes only the **physical situation**.

It may contain:

  
  | Field | Description |
  | ------|-------------|
  | Time |                Temporal definition of the experiment         | 
  | Observer|             {Observation origin (Heliocentric, Geocentric, Topocentric), Location (opt)} | 
  | Targets |             Celestial bodies involved | 
  | Frame |               {Coordinate frame, Epoch (opt)} | 
  | Corrections |         Physical effects applied | 
  

Fields inside the Core are optional, however: the Core must contain **at least one physical dimension**. 
Factories may require additional Core fields.

Examples:
-   Time
-   Observer
-   Targets

A scenario with an empty Core would not represent a physical experiment
and is therefore invalid.

Examples:
-   Ephemeris experiments require **Time, Observer, Targets**
-   Delta‑T experiments require **Time only**
-   Observer geometry experiments require **Observer only**

Observer may optionally include **location**.

Observer representations must be canonicalized before CoreHash generation.
Location identifiers such as site names must be resolved to canonical
coordinates before hashing.

Location: 
	Lat: double
	Lon: double
	Elevation (opt): double
	SiteName (opt): string

Example: Observer 

"Observer": {
  "Type": "Topocentric",
  "Body": "Earth",
  "Location": {
    "Lat": 51.477900,
    "Lon": 0.000000,
    "Elevation": 46.000000,
	"SiteName": "Greenwich"
  }
}

SiteName shall not be part of the hash.

Accuracy of values shall be scientific (6 digits). This implies ending zeros in scientifically relevant quantities. 
In ScenarioID, for the sake of better readibility -> truncation.

Default values for Location are 

Location: 
	Lat: 0.000000, 
	Lon: 0.000000,
	Elevation (opt): 0.000000, 
	SiteName: "CenterOfObservingBody"


Frame may optionally include an **epoch definition**.

This clarifies whether the frame refers to a fixed reference frame
(J2000) or a dynamic frame (of date).

Example conceptual structure:

Frame:
  Type: HelioEcliptic
  Epoch: J2000

For backward compatibility, the simple string representation used in
earlier scenarios remains valid.

If no epoch is specified, the default interpretation is:
Epoch = J2000

Epoch must not be included in hash if omitted.

Frame representations must be canonicalized before CoreHash generation.

------------------------------------------------------------------------




------------------------------------------------------------------------

## 1.2 DatasetHeader Context

The **DatasetHeader** describes **how reference data is obtained** by the Factory and which version of the Engine processes the data.

Factories:
-   read the Scenario Core
-   generate measurements
-   append metadata

Factory-specific measurement metadata belongs to the DatasetHeader, not to the Scenario definition.
Factories must **not modify the Scenario Core**.

Engines:
-   add engine specific meta data

Engines must **not modify the Scenario Core**.

------------------------------------------------------------------------

# 2. Scenario and Dataset Identity

AstronoSphere distinguishes between **Scenario identity** and **Dataset
identity**.

## ScenarioID

ScenarioID identifies the **physical experiment**.

Requirements:
-   human readable
-   deterministic
-   independent of any Factory


The scenario ID shall be generated according to this rule: 
ScenarioID = <Origin>-<Timescale>-<StartJD>-<StopJD>-<StepDays>D

Origin in ScenarioID is derived from Observer.Type / Body according to a fixed mapping.
Examples:
Heliocentric → HELIO
Geocentric  → GEO
Topocentric → TOPO

Note: <StepDays> is a double and represents the portion of a day.

Example:
HELIO-TDB-2451545-2451546-0.01D

## DatasetID

DatasetID identifies the **reference dataset produced by a Factory**.

FactoryIdentifier must uniquely identify the reference data source and
model.

FactoryIdentifier format: FACTORY-SOURCE-MODEL

Examples:
- EPH-HORIZONS-DE440\
- EPH-SPICE-DE441\
- TIME-MEEUS\
- OBS-WGS84

Example DatasetID: HELIO-TDB-2451545-2451555-1D--EPH-HORIZONS-DE440

Structure:
ScenarioID--FactoryIdentifier

This allows **multiple datasets to exist for the same scenario**.

------------------------------------------------------------------------

## CoreHash

CoreHash is a **technical integrity hash** derived from the canonical Core definition.

CoreHash is used for:
-   change detection
-   cache validation
-   dataset integrity checks

CoreHash is **not intended as the primary human identifier**.

Hash generation rule: SHA256(canonical(Core)) → first 8 hex characters

------------------------------------------------------------------------

# 3. CatalogNumber

Each released scenario receives a **CatalogNumber** when it is promoted
to `Status.maturity = released`.

Purpose:

-   human‑readable catalog reference
-   stable identifier within the ObservationCatalog

CatalogNumber is stored as an integer internally but displayed using the
format: AS-0001

Example:
- AS-0042

CatalogNumber assignment rule: `next = max(existing CatalogNumber) + 1`

------------------------------------------------------------------------



# 4. Scenario Status

Each scenario contains a Status block.

Status `{ maturity visibility }`

## 4.1 Maturity

Lifecycle states:

`created → released → deprecated`

Factories must **only process scenarios where**

`Status.maturity = released`

## 4.2 Visibility

Visibility controls publication scope.

`private, public`

------------------------------------------------------------------------

# 5. Scenario Metadata


  | Field |               Purpose | 
  | ------------------ | ---------------------------- | 
  ScenarioType |        structural classification | 
  ScenarioCategory|    scientific phenomenon | 
  EventComment |       specific event identifier | 
  Description  |        human readable description | 
  Rationale    |         scientific motivation | 
  ScientificPurpose | why scenario was selected |
  Priority	|	categorizes the scientific relevance |
  ScenarioCitation | documents the intellectual origin |


## 5.1 Scenario Type

`ScenarioType      → structural class`

Example:
- ScenarioType     = NumericalEdgeCase


## 5.2 Scenario Category

`ScenarioCategory  → physical phenomenon`

Example:
- ScenarioCategory = QuadrantCrossing

## 5.3 Description

`Description -> specifics of the ScenarioCategory`

Example:
- L6 = Quadrant Change at Longitude = 6 h


## 5.4 Rationale 

### 5.4.1 Motivation

Rationale describes the scientific or numerical reason **why
the scenario exists**.

It documents the phenomenon being validated and the expected
behavior of the system.
It also documents the scientific purpose of the experiment.

### 5.4.2 Structure

Rationale contains three subfields

**Phenomenon**:
Which astronomical or mathematical phenomenon is being tested?

**NumericalRisk**
Which numerical risk exists and needs to be covered?

**Validates**
Which attributes of the system are being tested?


Resulting structure:

```json
  "Rationale": {

    "Phenomenon": "Quadrant Crossing",

    "NumericalRisk": "SignTransition",

    "Validates": [
      "VelocityStability",
      "NearZeroNumerics"
    ],
```

Rationale becomes **mandatory when a scenario is promoted to released**.

## 5.5 Scientific Purpose


Example:

Short description of the scientific purpose: why was this scenario chosen and what is its physical contribution?

```json
  {
    "ScientificPurpose": "Quadrant crossings occur four times during one revolution of the Earth around the Sun and therefore produce multiple sign transitions in heliocentric x and y coordinates. These transitions are critical points for validating numerical stability of velocity calculations."
  }

```

## 5.6 Additional Scenario Metadata

The following metadata fields extend the Scenario specification without
modifying the Scenario Core or the CoreHash semantics.

These fields are **optional metadata** and therefore do not alter the
physical identity of the experiment.

### 5.6.1 Priority

Priority allows the ObservationCatalog maintainer to highlight
scientifically important scenarios.

Priority is **curated**, not automatically generated.

Field:

Priority: integer

Recommended scale:

| Value | Meaning |
|------|--------|
| 1 | Key benchmark scenario |
| 2 | Important scenario |
| 3 | Standard scenario |

1 = highest priority.

Priority is metadata and must **not influence CoreHash**.

------------------------------------------------------------------------

## 5.6.2 ScenarioCitation

Many scenarios originate from published astronomical work.
ScenarioCitation documents the intellectual origin of the experiment.

Structure:

ScenarioCitation:
  Author: string
  Source: string
  Citation: string

Example:

ScenarioCitation:
  Author: Jean Meeus
  Source: Astronomical Algorithms
  Citation: Meeus, J. (1998). Astronomical Algorithms, 2nd Edition.

------------------------------------------------------------------------


# 6. Time Definition

StartTime and StopTime parameters use **Julian Days**.
Step is a string like used in Hoirzons, e.g. "1H", "0.0417D".
Note that no space must be in the Step string.

Initial 

Time `{ StartJD StopJD Step TimeScale }`

Rules:
-   StepDays must be a string
-   TimeScale may be TT, TDB, or UTC

Factories and Astronometria may internally convert time scales.
Astronometria calculations always operate in TT.

------------------------------------------------------------------------

# 7. Observer Definition

Observer describes the **origin of observation**.

Observer `{ Type, Body, Location (optional) }`

------------------------------------------------------------------------

# 8. Author and Extensions

## 8.1 Author

Text field to include the author of the scenario. This supports scientific traceability, community contributions (once AstronoSphere becomes Open Source) and governance.

The field author must not be included in the CoreHash. The same physical scenario issued by different authors remains the same physics.

## 8.2 Extension

Extensions can include experimental extensions which we do not know at the point of freeze of the ScenarioHeader. It can also include temporary metadata.

Rules for extensions:
1) Extensions must not overwrite Core-Fields
2) Extensions must not be part of the CoreHash
3) Extensions may be ignored by factories

------------------------------------------------------------------------


# 9. DatasetHeader Metadata (Generated by Factories and Engines)

The following metadata does **not belong to the Scenario definition**
but to datasets generated from the Scenario.

These elements therefore appear in the **DatasetHeader**.

## 9.1 DatasetID

The DatasetID shall be generated according to this rule
DatasetID = <ScenarioID>--<FactoryNameAbbreviation>-<Source>-<ReferenceEphemeris>

Factory abbreviations are fixed by convention.
Examples:
EphemerisFactory → EPH
TimeFactory → TIM
ObserverFactory → OBS

Example:
HELIO-TDB-2451545-2451546-0.01D--EPH-HORIZONS-DE440


## 9.2 TruthMetadata

The API request of the truth provider is stored as a CanonicalRequest.
A Request Hash is generated out of the Canonical Request:
RequestHash = SHA256(CanonicalRequest)

Among other fields, the TruthProviderUrl shall be stored for traceability.

## 9.3 FactoryMetadata
FactoryName and Version as well as Source are identifying the type, version and name of the truth provider (e.g. JPL Horizons).
The rest of the FactoryMetadata block are specifying the data. 

## 9.4 TruthCitation

Reference data must include citation metadata describing the scientific
data source.

Structure:

TruthCitation:
  Provider: string
  Source: string
  Citation: string

Example:

TruthCitation:
  Provider: JPL Horizons
  Source: NASA/JPL Solar System Dynamics
  Citation: Giorgini, J.D. et al. (1996).

------------------------------------------------------------------------

## 9.5 Provenance

Provenance documents the complete validation chain.

Scenario → TruthFactory → Validation Engine

Structure:

Provenance:
  ScenarioFactory: string
  TruthFactory: string
  ValidationTarget:
    Software: string
    GitCommit: string
    GitBranch: string (optional)
    GitTag: string (optional)

------------------------------------------------------------------------

## 9.6 EngineCitation

Validation engines may include citation metadata.

Structure:

EngineCitation:
  Author: string
  Software: string
  Citation: string

Example:

EngineCitation:
  Author: Marcus Hiemer
  Software: Astronometria
  Citation: Astronometria Ephemeris Engine
  
EngineCitation must appear in the DatasetHeader when validation results
are produced by a computational engine.

------------------------------------------------------------------------

## 9.7 ValidationFingerprint

Each validation run should be uniquely identifiable.

The fingerprint combines:

ScenarioID
TruthFactory dataset
Engine version
Run timestamp

Example:

ValidationFingerprint: VF-20260316-8F3A12C-AS0001

------------------------------------------------------------------------

# 10. ObservationCatalog Organization

Scenarios are organized by **phenomenon classes** rather than celestial
objects. They shall be identified by the ScenarioCategory tag in the header.

Example list for classical ephemeris stress situations:
1.  Opposition
2.  Conjunction
3.  Inferior Conjunction
4.  Greatest Elongation
5.  Perihelion
6.  Aphelion
7.  Node Crossing
8.  Quadrant Crossing
9.  Stationary Point
10. Zenith Crossing
11. Horizon Crossing
12. Maximum Phase Angle
13. Eclipse Geometry

Additional candidates for ObservationCatalog situations:
- CoordinateTransform
- EpochBoundary

This list is not exhaustive.

------------------------------------------------------------------------

# 11. Header Ownership model

Each header field has exactly one owning component.
No component is allowed to overwrite fields owned by another component.
Scenario Core is immutable after ScenarioHeaderGenerator execution.

ScenarioHeaderGenerator is the only component allowed to:
- assign ScenarioID
- assign CatalogNumber
- compute CoreHash
- persist scenarios into ObservationCatalog

| Field | Writer | Comment |
| ------|-------------|------------|
| **ScenarioIdentification**|
| SchemaVersion | ScenarioHeaderGenerator | fix
| ScenarioID | ScenarioHeaderGenerator | deterministic
| CatalogNumber | ScenarioHeaderGenerator |  max+1
| CoreHash | ScenarioHeaderGenerator |  from Core
| **ScenarioMetadata**|
| Author | Maintainer | mandatory (IP)
| Extensions | Maintainer | optional
| Status (maturity, visibility) | Maintainer | Governance
| ScenarioType | Maintainer | mandatory 
| ScenarioCategory | Maintainer | mandatory
| EventComment | Maintainer | mandatory
| Description | Maintainer | optional
| Rationale (Phenomenon, NumericalRisk, Validates) |  Maintainer | mandatory for release
| ScientificPurpose | Maintainer | optional
| Priority | Maintainer | Mandatory
| ScenarioCitation | Maintainer | Mandatory
| **Core**
| Time  | Scenario Generator | physical data
| Observer | Scenario Generator | physical data
| Targets | Scenario Generator | physical data
| Frame | Scenario Generator | physical data
| Corrections | Scenario Generator | physical data
| **DatasetHeader** |
| DatasetID | TruthFactory | from ScenarioID
| TruthMetadata | TruthFactory | Reference
| FactoryMetadata | TruthFactory | Copy
| TruthCitation | TruthFactory | Factory-Header
| Provenance | TruthFactory | Pipeline
| EngineCitation | Engine | Mandatory for engine-produced datasets
| ValidationFingerprint | AnalysisTool | mandatory for validated comparison datasets



# 12. Json Example for an Ephemeris Scenario "Quadrant Crossing"


The following example illustrates how the Scenario definition and the
DatasetHeader metadata coexist.

``` json
{
  "SchemaVersion": "1.0",

  "ScenarioID": "HELIO-TDB-2451545-2451546-0.01D",
  "CatalogNumber": "AS-0001",
  "CoreHash": "A1B2C3D4",

  "Author": "ObservationCatalog Maintainer",

  "Extensions": "placeholder for future needs",

  "Status": {
    "maturity": "released",
    "visibility": "public"
  },

  "ScenarioType": "NumericalEdgeCase",
  "ScenarioCategory": "QuadrantCrossing",
  "EventComment": "L6 Quadrant Change",

  "Description": "Heliocentric quadrant crossing of Earth used to validate velocity computation near coordinate sign transitions.",

  "Rationale": {
    "Phenomenon": "Quadrant Crossing",
    "NumericalRisk": "SignTransition",
    "Validates": [
      "VelocityStability",
      "NearZeroNumerics"
    ]
  },

  "ScientificPurpose": "Quadrant crossings occur four times during one revolution of the Earth around the Sun and therefore produce multiple sign transitions in heliocentric coordinates. These transitions are critical points for validating numerical stability of velocity calculations.",

  "Priority": 1,

  "ScenarioCitation": {
    "Author": "Jean Meeus",
    "Source": "Astronomical Algorithms",
    "Citation": "Meeus, J. (1998). Astronomical Algorithms, 2nd Edition."
  },

  "Core": {
    "Time": {
      "StartJD": 2451545.0,
      "StopJD": 2451546.0,
      "StepDays": 0.01,
      "TimeScale": "TDB"
    },

    "Observer": {
      "Type": "Heliocentric",
      "Body": "Sun"
    },

    "Targets": [
      "Earth"
    ],

    "Frame": {
      "Type": "HelioEcliptic",
      "Epoch": "J2000"
    },

    "Corrections": {
      "LightTime": false,
      "Aberration": false,
      "Precession": false,
      "Nutation": false
    }
  },


  "DatasetHeader": {

    "DatasetID": "HELIO-TDB-2451545-2451546-0.01D--EPH-HORIZONS-DE440",

    "TruthMetadata": {
      "CanonicalRequest": "CENTER=500@399\nCOMMAND=599\nCSV_FORMAT=NO\nEPHEM_TYPE=VECTORS\nOBJ_DATA=NO\nOUT_UNITS=AU-D\nREF_PLANE=ECLIPTIC\nREF_SYSTEM=ICRF\nSTART_TIME=JD2451545.0\nSTEP_SIZE=0.01D\nSTOP_TIME=JD2451546.0",
      "RequestHash": "4D1EECEBC14C2EE9056D9D18D70DDE0BA7396D65804876A7D659D4B1A214CC6D",
      "EpochHash": null,
      "Requests": null,
      "TruthProviderUrl": "",
      "GeneratedAtUtc": "2026-03-12T19:02:41.5787637Z"
    },

    "FactoryMetadata": {
      "FactoryName": "EphemerisFactory",
      "FactoryVersion": "1.0.0",
      "Source": "JPL_Horizons",
      "ReferenceEphemeris": "DE440",
      "Mode": "HELIO",
      "EphemType": "VECTORS",
      "CorrectionLevel": "L0",
      "TimeScale": "TDB"
    },

    "TruthCitation": {
      "Provider": "JPL Horizons",
      "Source": "NASA/JPL Solar System Dynamics",
      "Citation": "Giorgini, J.D. et al. (1996). JPL Horizons System."
    },

    "Provenance": {
      "ScenarioFactory": "MeeusScenarioFactory",
      "TruthFactory": "HorizonsTruthFactory",
      "ValidationTarget": {
        "Software": "Astronometria",
        "GitCommit": "8F3A12C",
        "GitBranch": "main",
        "GitTag": "v1.0"
      }
    },

    "EngineCitation": {
      "Author": "Marcus Hiemer",
      "Software": "Astronometria",
      "Citation": "Hiemer, M. (2026). Astronometria Ephemeris Engine."
    },

    "ValidationFingerprint": "VF-20260316-8F3A12C-AS0001"
  }
}
```

------------------------------------------------------------------------

# Appendix A -- Scenario Architecture Freeze

The following architectural elements are considered **stable**.

## Scenario Identity

ScenarioID defines the physical experiment and must remain stable.

CoreHash ensures technical integrity.

CatalogNumber is assigned when the scenario becomes released.

## Scenario Lifecycle

created → released → deprecated

Factories process only:

Status.maturity = released

## Catalog Organization

Scenarios are organized by **phenomenon categories**.

## Governance

Scenario release authority lies with the **maintainer of the
ObservationCatalog**.

## Design Philosophy

Scenario complexity should be expressed in **scenario data**, not in
code logic.
