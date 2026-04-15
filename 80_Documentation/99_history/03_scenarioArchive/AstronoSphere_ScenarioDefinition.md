# AstronoSphere Scenario Definition Specification (M1)



This document defines the **scenario structure used across the AstronoSphere ecosystem**.

Scenarios describe **astronomical experiments**.  
Factories generate **reference measurements** for those experiments.

The architecture strictly separates:

- **Scenario Core** → Physical experiment definition
- **Factory Block** → Measurement / reference data generation

Factories must **never modify the Scenario Core**.

---

SchemaVersion: 1.0

# 1. Core Principles

## 1.1 Scenario Core

The **Core** describes only the **physical situation**.

It may contain:

| Field | Description |
|------|-------------|
| Time | Temporal definition of the experiment |
| Observer | Observation origin (Heliocentric, Geocentric, Topocentric) |
| Targets | Celestial bodies involved |
| Frame | Coordinate frame |
| Corrections | Physical effects applied |

Fields inside the Core are optional, however:

The Core must contain **at least one physical dimension**.

Examples:

- Time
- Observer
- Targets

A scenario with an empty Core would not represent a physical experiment and is therefore invalid.

Example:

- Ephemeris experiments require Time, Observer, Targets.
- DeltaT experiments require only Time.
- Observer geometry experiments require only Observer.

---

## 1.2 Factory Block

The **Factory section** describes **how reference data is obtained**.

Factories:

- read the Scenario Core
- generate measurements
- append metadata

Factories must **not modify the Scenario Core**.

---

# 2. Scenario Identity

Every scenario contains three identifiers.

# 2. Scenario and Dataset Identity

AstronoSphere distinguishes between **Scenario identity** and **Dataset identity**.

## ScenarioID

ScenarioID identifies the **physical experiment**.

It must be:

- human readable
- deterministic
- independent of any Factory

Example:

HELIO-TDB-2451545-2451555-1D

## DatasetID

DatasetID identifies the **reference dataset produced by a Factory**.

FactoryIdentifier must uniquely identify the reference data source and model.

Examples:

EPH-HORIZONS-DE440
EPH-SPICE-DE441
TIME-MEEUS
OBS-WGS84

Example:

HELIO-TDB-2451545-2451555-1D--EPH-HORIZONS-DE440

Structure:

ScenarioID--FactoryIdentifier

This allows multiple datasets to exist for the same scenario.

Example:

Scenario
→ multiple DatasetIDs

This supports the AstronoSphere validation architecture:

Scenario  
→ TruthFactory Run  
→ ScientificReference Dataset  
→ Astronometria Validation


CoreHash is a **technical integrity hash** derived from the canonical Core definition.

CoreHash is used for:

- change detection
- cache validation
- dataset integrity checks

CoreHash is **not intended as the primary human identifier**.

Hash generation:

SHA256(canonical(Core))
→ first 8 hex characters
Hash generation rule:

```
SHA256(canonical(Core))
→ first 8 hex characters
```

Recommended dataset filename structure:

```

DatasetID__CoreHash.json
```

Beispiel:
```
HELIO-TDB-2451545-2451555-1D__EPH-HORIZONS-DE440__71B2D0F3.json
```

Only the **CoreHash** appears in filenames.
`

Dataset metadata must include:

```

DatasetID
ScenarioID
FactoryName
FactoryVersion
CreationTimestamp

``

---

# 3. Time Definition

Time parameters use **Julian Days**.

```
Time {
    StartJD
    StopJD
    StepDays
    TimeScale
}
```

Rules:

- StepDays must be a **decimal number in days**
- TimeScale may be **TT, TDB, or UTC**

Factories may internally convert the time scale.

Example:

```
Scenario → TT
Factory → request TDB (Horizons)
```

---

# 4. Observer Definition

Observer describes the **origin of observation**.

```
Observer {
    Type
    Body
    Location (optional)
}
```

```
Location identifies the observation site for topocentric scenarios.

Example:

Observer {
  Type: Topocentric
  Body: Earth
  Location: Greenwich
}

Factories may use this information to compute additional parameters such as:

- geodetic height
- Earth ellipsoid corrections
- station coordinates

```

Examples:

| Type | Description |
|-----|-------------|
| Heliocentric | Origin at Sun |
| Geocentric | Origin at Earth's center |
| Topocentric | Observer located on a planetary body |

Topocentric coordinates should be provided by **ObserverFactory**, not inside the Core.

---

# 5. Scenario Classification

Additional metadata improves human readability.

| Field | Purpose |
|------|---------|
| ScenarioCategory | Scientific class |
| EventComment | Specific event description |

Example:

```
ScenarioCategory: QuadrantCrossing
EventComment: L6
```

---

# 6. Example Scenarios

Below are three representative scenarios demonstrating different factory types.

---

# 6.1 EphemerisFactory Example

Purpose: planetary ephemeris comparison.

```json
{
  {
  "SchemaVersion": "1.0",
  
  "ScenarioName": "Mars_Opposition_2025",

  "CoreHash": "71B2D0F3",

  "ScenarioID": "GEO-TDB-2460000-2460010-1D",

  "ScenarioCategory": "Opposition",
  "EventComment": "Mars 2025",

  "Description": "Mars opposition around January 2025",

  "Core": {

    "Time": {
      "StartJD": 2460000.0,
      "StopJD": 2460010.0,
      "StepDays": 1.0,
      "TimeScale": "TT"
    },

    "Observer": {
      "Type": "Geocentric",
      "Body": "Earth"
    },

    "Targets": [
      "Mars"
    ],

    "Frame": "GeoEquatorial",

    "Corrections": {
      "LightTime": false,
      "Aberration": false,
      "Precession": false,
      "Nutation": false
    }

  },

  "Factory": {

    "EphemerisFactory": {

      "Source": "JPL_Horizons",
      "ReferenceEphemeris": "DE440",
      "EphemType": "VECTORS",
      "TimeScale": "TDB"

    }

  }

}
```

---

# 6.2 TimeFactory Example

Purpose: generate a reference series for **Delta-T**.

```json
{
  "SchemaVersion": "1.0",

  "ScenarioName": "DeltaT_Meeus_ReferenceSeries",

  "CoreHash": "4AC91F22",

  "ScenarioID": "TIME-TT-2400000-2403650-1D",

  "ScenarioCategory": "TimeModel",
  "EventComment": "Meeus polynomial comparison",

  "Description": "Delta T reference series compared with Meeus polynomial model",

  "Core": {

    "Time": {
      "StartJD": 2400000.0,
      "StopJD": 2403650.0,
      "StepDays": 1.0,
      "TimeScale": "TT"
    }

  },

  "Factory": {

    "TimeFactory": {

      "Source": "Meeus",
      "Quantity": "DeltaT",
      "Model": "MeeusPolynomial"

    }

  }

}
```

---

# 6.3 ObserverFactory Example

Purpose: analyze topocentric observer geometry.

```json
{
  "SchemaVersion": "1.0",

  "ScenarioName": "ObserverHeight_EarthGeometry",

  "CoreHash": "9C7A31F5",

  "ScenarioID": "OBS-EARTH-TOPO-HEIGHT",

  "ScenarioCategory": "ObserverGeometry",
  "EventComment": "Sea level vs spherical Earth",

  "Description": "Comparison between spherical Earth surface and real altitude above sea level",

  "Core": {

    "Observer": {
      "Type": "Topocentric",
      "Body": "Earth"
    }

  },

  "Factory": {

    "ObserverFactory": {

      "Locations": [
        "Greenwich",
        "Paranal",
        "MaunaKea"
      ],

      "Quantity": "ObserverPosition",
      "ReferenceEphemeris": "WGS84"

    }

  }

}
```

---

# 7. Architectural Result

The same scenario system can support multiple scientific domains.

| Factory | Example |
|-------|---------|
| EphemerisFactory | planetary ephemerides |
| TimeFactory | Delta-T reference series |
| ObserverFactory | observer geometry |

This makes AstronoSphere **factory-agnostic** and extensible.

---

# End of Document