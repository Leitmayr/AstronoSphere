# AstronoSphere Truth Factories – Reference Data Strategy

This document is the Scientific Reference Policy Documentatinon. It defines the fundamental, non-negotiable architectural and scientific principles of the AstronoSpheres Truth Factory Systems to ensure the Ground Truth Pinciple.

These rules ensure deterministic reproducibility, scientific traceability, and long-term maintainability.

---

# 1. Architectural Separation

Astronometria consists of two independent systems:

- **Ephemeriden Pipeline**  
  Generates astronomical state vectors using internal algorithms.

- **EphemerisRegression**  
  Generates and stores external reference data from JPL Horizons.

Golden Rule:

> No production code is shared between both projects.

Both systems remain coherent through Git baselining only.

---

# 2. Reference Data Philosophy

Two types of artifacts exist:

| Artifact | Purpose |
|----------|----------|
| CSV      | Unmodified raw Horizons output |
| JSON     | Deterministic, reproducible reference object |

CSV files are stored 1:1 exactly as returned by NASA/JPL Horizons.

JSON files contain:
- Parsed state vectors
- Metadata
- Canonical request
- Request hash

---

# 3. Deterministic Request Identity

Each Horizons request is uniquely defined by its normalized parameter set.

## 3.1 Canonicalization Rules

1. Parameters are stored in a dictionary.
2. Keys are sorted alphabetically (ordinal).
3. Key-value pairs are concatenated as:

   KEY=VALUE

4. Lines are joined using:

   \n

5. UTF-8 encoding is used.
6. Null or empty parameters are excluded.

---

## 3.2 Hash Definition

- Algorithm: SHA256
- Encoding: UTF-8
- Output: Uppercase hexadecimal string

The hash is computed over the canonical request string.

Identical parameters must always produce identical hashes.

---

# 4. Metadata Definition for EphemerisFactory

Each JSON reference file contains a ReferenceMetadata object:

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

Definitions:

- CanonicalRequest  
  Deterministically normalized request definition.

- RequestHash  
  SHA256 hash of CanonicalRequest.

- HorizonsUrl  
  Optional traceability field.

- EngineVersion  
  Version of EphemerisRegression generator.

- CorrectionLevel  
  L0 (geometric), future: L1, L2, …

- Mode  
  HELIO or GEO.

- EphemType  
  VECTORS (future: OBSERVER for RA/DEC).

- GeneratedAtUtc  
  Timestamp of JSON creation.

---

# 5. Event Generation Principle

No event Generation any more in TruthFactories.

---

# 6. Planet Selection Rules

Geo node calculations must exclude Earth.

Two execution modes exist:

- Fast mode (inner + major planets)
- Full mode (including Uranus and Neptune)

This applies independently to HELIO and GEO.

---

# 7. Reproducibility Definition

A reference dataset is scientifically reproducible if:

1. CanonicalRequest is identical.
2. RequestHash is identical.
3. Horizons returns identical physical output.

If any parameter changes, the hash must change.

---

# 8. CSV Integrity Rule

CSV files:

- Must remain unmodified.
- Must contain original Horizons headers.
- Must never contain custom metadata.

All contextual data belongs exclusively in JSON.

---

# 9. Time System

All state vectors are requested in:

- TDB (Barycentric Dynamical Time)

No implicit TT/TDB conversions occur inside reference generation.

---


---

# End of Document
