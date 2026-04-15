# AstronoSphere – DatasetHeader Specification (M1.9 – FINAL FREEZE)

## 1. Purpose

The DatasetHeader fully describes the origin, identity, and reproducibility of a GroundTruth dataset.

It guarantees:

- deterministic dataset generation
- full reproducibility of external requests (e.g. JPL Horizons)
- strict separation between physical experiment and measurement
- traceability across the entire pipeline

---

## 2. Core Principle

Dataset = Experiment (Core) + Measurement (Request)

- CoreHash represents the physical definition (AstronoCert)
- RequestHash represents the measurement definition (AstronoTruth)

---

## 3. Metadata Source (MANDATORY)

FactoryMetadata and Citations MUST be centrally defined in code.

Rules:

- MUST be implemented via a single provider class
- MUST NOT be duplicated across code
- MUST NOT be loaded from config/files in M1.9
- MUST be deterministic and version-controlled
- MUST be defined exactly once per TruthProvider

Example:

HorizonsMetadataProvider

Future Extension:

The design MUST allow replacement of providers:

AstronoTruth.Horizons → AstronoTruth.Miriade

This MUST NOT require changes in hashing, canonicalization, or DatasetHeader structure.

---

## 4. Canonical Request Object (CRITICAL)

AstronoTruth MUST NOT pass strings to AstronoData.Contracts.

Instead:

A fully materialized OBJECT MUST be passed to the Canonicalizer.

---

### 4.1 Example Object

{
  "Command": "199",
  "Center": "@10",
  "StartTime": "JD2451545.000000000",
  "StopTime": "JD2451546.000000000",
  "StepSize": "1H",
  "TimeScale": "TDB",
  "EphemType": "VECTORS",
  "RefSystem": "ICRF",
  "RefPlane": "ECLIPTIC",
  "OutUnits": "AU-D",
  "CsvFormat": "YES"
}

---

### 4.2 Rules

- ALL defaults MUST be explicitly present
- NO implicit defaults allowed
- NO missing fields allowed
- Object MUST be deterministic
- Property names MUST be stable
- Object structure MUST be canonicalizable
- Only public properties are allowed
- No hidden logic in getters

---

### 4.3 Processing Pipeline

Object
→ Canonicalizer (AstronoData.Contracts)
→ Canonical String
→ SHA256
→ RequestHash

---

### 4.4 Key Principle

Hash the effective request, not the input.

---

## 5. TruthMetadata (CORE BLOCK)

Fields:

- CanonicalRequest (multiline string)
- RequestHash (SHA256, uppercase hex)
- TruthProviderUrl (fully executable URL)
- GeneratedAtUtc (ISO-8601)
- Requests[] (optional, reserved for future)

---

### Rules

- CanonicalRequest MUST be the exact string used for hashing
- RequestHash MUST be derived only from CanonicalRequest
- TruthProviderUrl MUST NOT use encoded "input=" blocks
- Requests[] MUST NOT affect hashing

---

## 6. ExperimentReference

Fields:

- ExperimentID (mandatory)
- CoreHash (mandatory, read-only)

Rules:

- MUST match the Experiment exactly
- MUST NOT be recomputed in AstronoTruth

---

## 7. Measurement

Fields:

- Level (L0, L1, L2, ...)
- Type (VEC, RADEC, ...)

Rules:

- MUST match the effective request
- MUST NOT be implicit

---

## 8. FactoryMetadata

Fields:

- FactoryName
- FactoryVersion
- Source
- ReferenceEphemeris
- Mode
- EphemType
- CorrectionLevel
- TimeScale

Rules:

- MUST be fully explicit
- MUST match CanonicalRequest
- MUST be provided by the central MetadataProvider

---

## 9. Provenance

Fields:

- ScenarioFactory
- TruthFactory
- ValidationTarget (Software, GitCommit, GitBranch, GitTag)

Rules:

- informational only
- MUST NOT affect hashing

---

## 10. Citations

Fields:

- TruthCitation (JPL Horizons)
- EngineCitation (Astronometria)

Rules:

- MUST be provided centrally
- MUST be stable

---

## 11. Hashing Rules

ONLY the following is hashed:

CanonicalRequest

---

## 12. Default Handling (MANDATORY)

EffectiveRequest = Input + Defaults

Rules:

- ALL defaults MUST be explicitly materialized BEFORE hashing
- NO system-dependent defaults allowed
- NO implicit values allowed

---

## 13. Determinism Guarantees

- identical request → identical hash
- identical core → identical experiment
- full reproducibility guaranteed

---

## 14. Forbidden Behaviors

- hashing incomplete requests
- relying on external defaults
- modifying CanonicalRequest after hashing
- duplicating metadata definitions
- recomputing CoreHash in AstronoTruth

---

## 15. Final Principle

A dataset must be reproducible without any hidden information.



# M1.9 DATA GENERATION RULES (STRICT – NO DEVIATION)

## 1. Core Principle

All generated data MUST be:

- deterministic
- precision-stable
- byte-identical across runs

---

## 2. CRITICAL: Numeric Precision (NO ROUNDING!)

ABSOLUTE RULE:

- NO rounding is allowed at any stage
- ONLY truncation is allowed (where explicitly defined)
- NO implicit formatting
- NO culture-dependent conversion

---

## 3. Internal Precision Standard

- All floating point values MUST preserve full precision
- Target precision: up to 9 decimal places (minimum requirement)
- Trailing zeros MAY be omitted
- BUT: existing precision MUST NOT be reduced

Examples:

VALID:
2451545.123456789
2451545.1234567

INVALID:
2451545.123457   (rounded)
2451545.123      (precision lost)

---

## 4. Serialization Rules (CRITICAL)

System.Text.Json MUST be configured as:

- Encoder = UnsafeRelaxedJsonEscaping
- NO scientific notation allowed
- NO forced decimal formatting (e.g. ToString("F6") is FORBIDDEN)
- NO culture-specific formatting (InvariantCulture only)

---

## 5. String Handling

- Special characters MUST be preserved exactly
- NO escaping of:
  - °
  - +
  - >
  - <
- UTF-8 encoding is mandatory

---

## 6. Canonical Consistency

The following MUST be identical:

- values used for hashing
- values written to Dataset
- values sent to TruthProvider

There MUST be ZERO transformation between:

CanonicalRequest → Request → Dataset

---

## 7. Determinism Requirement

Running the same pipeline twice MUST produce:

- identical files
- identical byte content
- identical hashes

ANY deviation is considered a CRITICAL ERROR.

---

## 8. Forbidden Operations

- Math.Round()
- ToString with format specifiers
- implicit double → string conversions without control
- re-parsing numeric values after serialization
- modifying values after canonicalization

---

## 9. Validation Requirement

Each dataset generation MUST be verifiable by:

- Run vs LastRun comparison
- binary equality check

---

## 10. Key Principle

DATA is not presentation.

DATA must remain physically exact and unchanged across the entire pipeline.

## 11. Ordering Rules

- All data collections MUST have deterministic ordering
- Dictionaries MUST be sorted before serialization
- Arrays MUST preserve original order

Non-deterministic ordering is FORBIDDEN.