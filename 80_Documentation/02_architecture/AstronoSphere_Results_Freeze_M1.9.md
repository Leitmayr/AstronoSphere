# AstronoSphere – Results (Astronolysis) Freeze (M1.9)

## 1. Purpose

Results represent the output of Astronolysis.

They:
- evaluate multiple Experiments, GroundTruth datasets and Simulations
- produce scientific insights
- generate new Seeds for the next pipeline cycle

---

## 2. Core Principle

> Results describe ANALYSIS, not measurement or simulation.

They operate on:
- multiple Scenarios (Experiments)
- multiple GroundTruth datasets
- multiple Simulations

---

## 3. Naming

### File Name

```
<SCOPE>_<ANALYSISTYPE>.json
```

Example:

```
MARS-OPP-1900-2000_ENGINE-COMPARISON.json
```

---

## 4. Header Structure

```json
{
  "Scope": {
    "Body": "Mars",
    "Category": "OPP",
    "TimeRange": "1900-2000"
  },

  "AnalysisType": "ENGINE_COMPARISON",

  "GroundTruthProviders": [
    "HORIZONS",
    "MIRIADE"
  ],

  "Simulations": [
    "ASTRONOMETRIA-MEEUS-VEC-L0",
    "ASTRONOMETRIA-VSOP87-VEC-L0"
  ]
}
```

---

## 5. Snapshot & Reproducibility

### 5.1 Snapshot Structure

```json
{
  "Snapshot": {
    "Scenarios": [
      {
        "ScenarioID": "...",
        "CoreHash": "...",
        "CatalogNumber": "AS-XXXXXX"
      }
    ],
    "GroundTruth": [
      {
        "GroundTruthID": "...",
        "Hash": "..."
      }
    ],
    "Simulations": [
      {
        "SimulationID": "...",
        "Hash": "..."
      }
    ],
    "SnapshotHash": "..."
  }
}
```

---

### 5.2 Canonicalization Rule

SnapshotHash is computed as:

- collect all referenced elements
- sort alphabetically
- canonicalize structure
- apply SHA256

```
SnapshotHash = SHA256(canonical(sorted(input)))
```

This is identical to:
- Scenario CoreHash
- Dataset Hashing

---

## 6. Results Body

Result content is intentionally **not frozen**.

Only top-level structure is defined:

```json
{
  "Results": {
    "Statistics": { ... },
    "Distributions": { ... },
    "Outliers": [ ... ]
  }
}
```

---

## 7. Generated Seeds

### 7.1 Structure (FROZEN)

```json
{
  "GeneratedSeeds": [
    {
      "SeedCandidate": {
        "Event": {
          "Category": "QCR",
          "Qualifier": "L18",
          "Description": "X coordinate sign change (- to +)"
        },
        "Core": {
          "Time": {
            "StartJD": 2478083.966,
            "StopJD": 2478085.966,
            "Step": "1H",
            "TimeScale": "TDB"
          },
          "Observer": {
            "Type": "Heliocentric",
            "Body": "Sun"
          },
          "ObservedObject": {
            "BodyClass": "Planet",
            "Targets": ["Jupiter"]
          },
          "Frame": {
            "Type": "HelioEcliptic",
            "Epoch": "J2000"
          }
        },
        "Metadata": {
          "Author": "Astronolysis",
          "Priority": 2,
          "Status": {
            "Maturity": "Draft",
            "Visibility": "Private"
          }
        },
        "Notes": "Generated automatically from residual peak detection."
      },
      "SeedOrigin": {
        "ResultID": "MARS-OPP-1900-2000_ENGINE-COMPARISON",
        "Reason": "High residual near opposition cluster",
        "Trigger": "ResidualPeak",
        "CreatedAtUtc": "2026-04-03T21:15:00Z"
      }
    }
  ]
}
```

---

### 7.2 Design Rules

- SeedCandidate MUST be valid ScenarioCandidate
- no transformation required for AstronoLab
- SeedOrigin MUST NOT influence Core or Hash

---

## 8. AnalysisType

AnalysisType is an open taxonomy.

Examples:

- ENGINE_COMPARISON
- PROVIDER_COMPARISON
- VALIDATION_RUN
- EDGE_DETECTION

---

## 9. Tool Responsibilities

### Astronolysis

- writes Results
- writes Seeds (Incoming)
- reads:
  - Experiments
  - GroundTruth
  - Simulations

---

## 10. Freeze Decision (M1.9)

- Snapshot model frozen
- Canonicalization frozen
- Seed output format frozen
- Naming frozen
- AnalysisType open
- Result body open
