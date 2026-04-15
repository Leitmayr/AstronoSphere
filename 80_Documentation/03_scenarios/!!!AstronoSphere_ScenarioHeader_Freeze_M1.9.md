# AstronoSphere – ScenarioHeader Freeze (M1.9)

## 1. Purpose

This document defines the final structure of the **ScenarioHeader**.

A Scenario represents a **pure physical experiment**.

It must be:
- minimal
- unambiguous
- reproducible
- independent of measurement and simulation

---

## 2. Core Principle

> Scenario describes WHAT happens in the physical experiment.

It must NOT include:
- measurement configuration
- simulation configuration
- analysis data

---

## 3. Scenario Structure

```json
{
  "SchemaVersion": "1.0",

  "ScenarioID": "<ID>",
  "CatalogNumber": "AS-XXXXXX",
  "CoreHash": "<HASH>",

  "Core": {
    "Time": {
      "StartJD": <double>,
      "StopJD": <double>,
      "Step": "<string>",
      "TimeScale": "<string>"
    },

    "Observer": {
      "Type": "<Heliocentric|Geocentric|Topocentric>",
      "Body": "<string>"
    },

    "ObservedObject": {
      "BodyClass": "<Planet|Moon|Asteroid|...>",
      "Targets": ["<string>"]
    },

    "Frame": {
      "Type": "<FrameType>",
      "Epoch": "<Epoch>"
    }
  },

  "Event": {
    "Category": "<short code, e.g. QCR>",
    "Qualifier": "<optional, e.g. L18>",
    "Description": "<short human-readable description>"
  },

  "Rationale": {
    "NumericalRisk": "<optional>",
    "Validates": ["<string>"]
  },

  "Metadata": {
    "Author": "<string>",
    "Priority": <int>,
    "Status": {
      "Maturity": "<Draft|Released>",
      "Visibility": "<Private|Public>"
    }
  },

  "Citation": {
    "Author": "<string>",
    "Source": "<string>",
    "Reference": "<string>"
  },

  "Notes": "<optional short text>"
}
```

---

## 4. Design Decisions

### 4.1 Core is strictly physical

Core MUST contain only:
- Time
- Observer
- ObservedObject
- Frame

Core MUST NOT contain:
- Corrections
- Measurement configuration
- Simulation configuration

---

### 4.2 ObservedObject vs Observer

- Observer → reference point
- ObservedObject → observed target

---

### 4.3 Event

- Category → machine-readable
- Qualifier → specialization
- Description → short explanation

---

### 4.4 Rationale

Optional but recommended.
Must not duplicate Event.

---

### 4.5 Metadata

Non-scientific information only.

---

### 4.6 Notes

Optional free text with strict rules:
- no duplication
- short
- optional

---

## 5. Removed Fields

- ScenarioType
- ScenarioCategory
- EventComment
- ScientificPurpose
- Extensions
- Corrections

---

## 6. ScenarioCandidate (Seeds)

```json
{
  "Event": { ... },
  "Core": { ... },
  "Metadata": { ... }
}
```

---

## 7. Freeze Decision (M1.9)

- Structure frozen
- Core minimal
- Event standardized
- Seeds compatible
