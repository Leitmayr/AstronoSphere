# ScenarioCandidate JSON Format Specification (CAND)

## 1. Purpose

This document defines the exact JSON structure required for ScenarioCandidate input files.
These files are consumed by the ScenarioHeaderGenerator (SHG).

---

## 2. Root Structure

```json
{
  "Event": { ... },
  "Core": { ... }
}
```

---

## 3. Event Block (optional)

```json
"Event": {
  "Category": "string",
  "ApproximateJD": number,
  "Source": "string",
  "Comment": "string (optional)"
}
```

---

## 4. Core Block (required)

```json
"Core": {
  "Time": { ... },
  "Observer": { ... },
  "Targets": [ ... ],
  "Frame": { ... },
  "Corrections": { ... }
}
```

---

## 5. Time Block (optional but strongly recommended)

```json
"Time": {
  "StartJD": number,
  "StopJD": number,
  "StepDays": number,
  "TimeScale": "TDB | TT | UTC"
}
```

Rules:
- StartJD <= StopJD
- StepDays > 0
- Single-point: StartJD == StopJD

---

## 6. Observer Block (optional)

```json
"Observer": {
  "Type": "Heliocentric | Geocentric | Topocentric",
  "Body": "string",
  "Location": {
    "Lat": number,
    "Lon": number,
    "Elevation": number (optional),
    "SiteName": "string (optional)"
  }
}
```

---

## 7. Targets (optional)

```json
"Targets": ["Mars", "Earth"]
```

---

## 8. Frame (optional)

```json
"Frame": {
  "Type": "string",
  "Epoch": "J2000 | OfDate"
}
```

---

## 9. Corrections (optional)

```json
"Corrections": {
  "LightTime": true,
  "Aberration": true,
  "Precession": true,
  "Nutation": true
}
```

---

## 10. Constraints

- No ScenarioID
- No Status
- No Metadata
- No Hash
- No CatalogNumber

Candidate files must contain ONLY physical scenario definition.

---

## 11. Full Example

```json
{
  "Event": {
    "Category": "Opposition",
    "ApproximateJD": 2459396.5,
    "Source": "Meeus",
    "Comment": "Mars opposition example"
  },
  "Core": {
    "Time": {
      "StartJD": 2459396.5,
      "StopJD": 2459396.5,
      "StepDays": 1.0,
      "TimeScale": "TDB"
    },
    "Observer": {
      "Type": "Heliocentric",
      "Body": "Sun"
    },
    "Targets": ["Mars"],
    "Frame": {
      "Type": "HelioEcliptic",
      "Epoch": "J2000"
    },
    "Corrections": {
      "LightTime": true,
      "Aberration": true,
      "Precession": true,
      "Nutation": true
    }
  }
}
```

---

End of document.
