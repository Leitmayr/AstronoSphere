# Astronometria – Fundamental Definition V2

## 1. Change Log (V1 → V2)

This version consolidates architectural, scientific, and reproducibility rules introduced after the TS-A and TS-B refactoring.

### Added in V2

- Deterministic Canonical Request definition
- SHA256-based Request Hash definition
- Mandatory Metadata block in JSON reference files
- Strict CSV integrity rule (no modification allowed)
- Prohibition of static Julian date tables
- Dynamic Event Generation principle
- HELIO / GEO architectural symmetry rule
- Fast / Full execution mode definition
- Formal reproducibility definition
- Clarified separation between Ephemeriden Pipeline and EphemerisRegression
- Unified coordinate and reference frame definitions

V2 represents the first stable reproducibility baseline of Astronometria.

---

## 2. Architectural Separation

Astronometria consists of two independent systems:

- **Ephemeriden Pipeline** – Internal astronomical computation engine
- **EphemerisRegression** – External reference data generation system (JPL Horizons)

Golden Rule:

> No production code is shared between both projects.

Consistency is ensured through Git baselining only.

---

## 3. Reference Data Philosophy

Two artifact types exist:

| Artifact | Purpose |
|----------|----------|
| CSV | Raw, unmodified Horizons output |
| JSON | Deterministic, reproducible reference object |

CSV files are stored exactly as returned by NASA/JPL Horizons.

JSON files contain:

- Parsed state vectors
- Metadata
- Canonical request definition
- Request hash

---

## 4. Deterministic Request Identity

Each Horizons request is uniquely defined by its normalized parameter set.

### 4.1 Canonicalization Rules

1. Parameters are stored in a dictionary.
2. Keys are sorted alphabetically (ordinal).
3. Format per line:

   KEY=VALUE

4. Lines are joined using `\n`.
5. UTF‑8 encoding is used.
6. Null or empty parameters are excluded.
7. Keys and values are normalized (uppercase, trimmed).

### 4.2 Hash Definition

- Algorithm: SHA256
- Encoding: UTF‑8
- Output: Uppercase hexadecimal string

The hash is computed over the canonical request string.

Identical parameters must produce identical hashes.

---

## 5. Metadata Definition

Each JSON reference file contains a `ReferenceMetadata` object:

```
{
  "canonicalRequest": "...",
  "requestHash": "...",
  "horizonsUrl": "",
  "engineVersion": "1.0.0",
  "correctionLevel": "L0",
  "mode": "HELIO | GEO",
  "ephemType": "VECTORS",
  "generatedAtUtc": "..."
}
```

Definitions:

- **CanonicalRequest** – Deterministic request definition
- **RequestHash** – SHA256 hash of canonical request
- **EngineVersion** – Version of the reference generator
- **CorrectionLevel** – L0 (future: L1, L2, …)
- **Mode** – HELIO or GEO
- **EphemType** – VECTORS (future: OBSERVER)
- **GeneratedAtUtc** – Creation timestamp

---

## 6. Event Generation Principle

Static Julian dates are prohibited.

Events must always be generated dynamically via:

- HelioEventGenerationRunner
- GeoEventGenerationRunner

Hard‑coded JD tables are not allowed.

---

## 7. Planet Selection Rules

Geo node calculations must exclude Earth.

Execution modes:

- **Fast Mode** – Inner + major planets
- **Full Mode** – Including Uranus and Neptune

This applies independently to HELIO and GEO.

---

## 8. Reproducibility Definition

A dataset is scientifically reproducible if:

1. CanonicalRequest is identical.
2. RequestHash is identical.
3. Horizons output is physically identical.

If parameters change, the hash must change.

---

## 9. CSV Integrity Rule

CSV files:

- Must remain unmodified.
- Must contain original Horizons headers.
- Must not contain custom metadata.

All contextual information belongs exclusively in JSON.

---

## 10. Time System

All state vectors are requested in:

- TDB (Barycentric Dynamical Time)

No implicit TT/TDB conversions occur during reference generation.

---

## 11. Architectural Stability Level

After TS-A and TS-B refactoring:

- No static JD definitions remain.
- HELIO and GEO are structurally symmetric.
- Hashing is deterministic.
- Metadata is mandatory.
- JSON references are self-describing.

This defines the Astronometria reproducibility baseline (V2).
