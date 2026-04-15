
# ObservationCatalog JSON Schema (Core Specification)

## Purpose

The ObservationCatalog defines the **canonical scenario description format**
used across the entire AstronoSphere ecosystem.

All TruthFactories and validation pipelines consume scenarios from this catalog.

The schema is intentionally **minimal, stable, and engine‑agnostic**.

---

# Design Principles

1. Engine‑independent
2. Factory‑independent core
3. Explicit physical parameters
4. Fully reproducible scenarios
5. Stable identifiers (ScenarioID)

---

# Top-Level Structure

```json
{
  "ScenarioID": "string",
  "Core": { },
  "Factory": { }
}
```

The scenario definition is split into two parts:

Core → universal physical scenario description  
Factory → factory-specific configuration

---

# Core Section

The Core section describes the **physical experiment**.

```json
{
  "Core": {
    "Time": {
      "StartJD": number,
      "StopJD": number,
      "StepDays": number,
      "TimeScale": "TDB | TT | UTC"
    },

    "Observer": {
      "Type": "Heliocentric | Geocentric | Topocentric",
      "Body": "Earth",
      "Location": "optional"
    },

    "Targets": [
      "Mercury",
      "Venus",
      "Earth",
      "Mars"
    ],

    "Frame": "HelioEcliptic | HelioEquatorial | GeoEquatorial | GeoEcliptic",

    "Corrections": {
      "LightTime": true,
      "Aberration": false,
      "Precession": false,
      "Nutation": false
    }
  }
}
```

---

# Time Definition

All time values are stored as **Julian Dates**.

This avoids ambiguity caused by the Julian → Gregorian calendar transition.

```json
"Time": {
  "StartJD": 2451545.0,
  "StopJD": 2451546.0,
  "StepDays": 0.25,
  "TimeScale": "TDB"
}
```

---

# Observer Definition

Examples:

```json
"Observer": {
  "Type": "Heliocentric"
}
```

```json
"Observer": {
  "Type": "Geocentric",
  "Body": "Earth"
}
```

Future extension:

```json
"Observer": {
  "Type": "Topocentric",
  "Body": "Earth",
  "Location": {
    "Latitude": 48.0,
    "Longitude": 9.0,
    "Altitude": 500
  }
}
```

---

# Factory Section

Each TruthFactory may define additional parameters.

```json
{
  "Factory": {
    "EphemerisFactory": {
      "Source": "JPL_Horizons",
      "StepMode": "Vector",
      "ReferenceEphemeris": "DE440"
    }
  }
}
```

Factories must ignore unknown parameters.

This ensures **forward compatibility**.

---

# Minimal Example

```json
{
  "ScenarioID": "HELIO-TDB-2451545-2451546-1D-EPH-HORIZONS-DE440",
  "Core": {
    "Time": {
      "StartJD": 2451545.0,
      "StopJD": 2451546.0,
      "StepDays": 1.0,
      "TimeScale": "TDB"
    },

    "Observer": {
      "Type": "Heliocentric"
    },

    "Targets": ["Earth"],

    "Frame": "HelioEcliptic",

    "Corrections": {
      "LightTime": false
    }
  },

  "Factory": {
    "EphemerisFactory": {
      "Source": "JPL_Horizons",
      "ReferenceEphemeris": "DE440"
    }
  }
}
```

---

# Summary

The ObservationCatalog provides a **stable, extensible, and engine‑independent**
definition of astronomical validation scenarios.

It is the **central input interface** of AstronoSphere.
