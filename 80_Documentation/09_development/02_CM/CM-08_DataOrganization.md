# CM-08 Data Organization & Scenario Identity

## 1. Purpose

This document defines how scenario data is identified, named, and organized
within AstronoSphere.

The goal is to:

- ensure a single, stable identity for each scenario
- separate identity, context, and storage concerns
- enable simple and deterministic data handling

---

## 2. Core Principle

> Scenario identity, processing context, and storage structure are strictly separated.

| Aspect | Responsibility |
|--------|---------------|
| Scenario ID | Identity |
| Dataset Header | Processing definition |
| Folder Structure | Human-readable classification |

---

## 3. Scenario Identity

### 3.1 Definition

> The file name equals the canonical Scenario ID.

Rules:

- must be unique
- must be stable (never changes)
- must be human-readable
- must not contain technical processing information

---

### 3.2 Format

```
<Object>_<Event>_<Date>
```

Examples:

```
Mars_Opposition_2003-08-28
Venus_GreatestElongation_2025-06-04
Jupiter_Stationary_1999-11-15
```

---

## 4. File Naming Rule

> File name = Scenario ID

Example:

```
Mars_Opposition_2003-08-28.json
```

Forbidden:

- adding factory name
- adding stage
- adding version
- adding processing parameters

---

## 5. Dataset Header (Authoritative Context)

The Dataset Header defines how the data was produced.

Example:

```json
{
  "ScenarioId": "Mars_Opposition_2003-08-28",
  "Factory": "Horizons",
  "Frame": "HeliocentricEcliptic",
  "Corrections": ["LightTime"],
  "Version": "v1.0"
}
```

Rules:

- header is the authoritative source of truth
- all processing parameters must be defined here
- header overrides folder interpretation in case of conflict

---

## 6. Folder Structure

### 6.1 Principle

> Folder structure provides human-readable classification only.

---

### 6.2 Structure

```
<Stage>/
   <Factory>/
      <ScenarioId>.json
```

Example:

```
03_ReferenceData/
   Horizons/
      Mars_Opposition_2003-08-28.json
   SPICE/
      Mars_Opposition_2003-08-28.json
```

---

## 7. Allowed Folder Dimensions

Only the following dimensions are allowed in folder structure:

- Stage (mandatory)
- Factory (recommended)

---

## 8. Forbidden Folder Dimensions

The following must NOT be encoded in folder structure:

- Frame
- Corrections
- TimeScale
- Observer
- Version
- Any processing parameter

---

## 9. Consistency Rule

> Folder classification and Dataset Header must be consistent.

If a conflict occurs:

> The Dataset Header is authoritative.

---

## 10. Design Benefits

This approach ensures:

- unique and stable scenario identity
- simple file comparison (Run vs LastRun)
- clear separation of concerns
- minimal structural complexity (KISS)
- full reproducibility via header

---

## 11. Summary

The data organization strategy:

- defines Scenario ID as the file name
- uses Dataset Header as authoritative processing definition
- limits folder structure to Stage and Factory
- enforces strict separation between identity and context

---

End of document.
