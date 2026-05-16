# Precision Background and AstronoPrecisionAudit

## Background

During M1.7 Fix Phase, the question arose whether AstronoSphere internally loses numerical precision through serialization, canonicalization or JSON roundtrips.

The concern was especially relevant because:

- AstronoSphere uses deterministic canonicalization with fixed decimal formatting
- StateVectors and Julian Dates are persisted as JSON
- TT/TDB differences and future LightTime corrections require high temporal precision
- silent precision loss ("accuracy creep") would undermine scientific trustworthiness

The analysis showed:

- IEEE754 `double` provides approximately 15–16 significant decimal digits
- for Julian Dates around JD ≈ 2.4–2.6 million, this safely supports 9 decimal places
- from year 6762 onwards, the precision reduces to 8 instead of 9
- therefore, all current M1/M2 core validation scenarios (Holy12, MVH, TS-A…TS-D, MXT1) are inside the safe precision domain
- extreme future mesh ranges (parts of MXT2) may exceed this safe domain and require additional strategy later

A critical architectural distinction was identified:

> Canonicalization precision is NOT equivalent to internal computation precision.

The canonical 9-decimal formatting rule is intended for:
- deterministic hashing
- reproducible serialization
- stable diff behavior

It must never silently reduce the precision of the internal numerical pipeline.

---

## AstronoPrecisionAudit (planned tool)

Purpose:

Provide explicit and reproducible evidence that AstronoSphere does not silently lose precision inside the scientific pipeline.

Planned checks:

1. Code Audit
   - detect suspicious patterns:
     - double → formatted string → double
     - parse/reparse cycles inside computation paths

2. G17 Roundtrip Test
   - verify:
     double → JSON(G17) → double
   - must remain bit-identical

3. Pipeline Integrity Test
   - verify:
     Horizons API == CSV == JSON == Reloaded JSON

4. Precision Boundary Documentation
   - explicitly define the safe JD precision range
   - document MXT2 as extended precision-risk domain

Planned output:

- PrecisionAuditReport.md
- Trust examples for M2.10
- explicit proof against silent accuracy creep

---

## Architectural Rule

AstronoSphere must distinguish strictly between:

- internal numerical precision
- serialization precision
- canonicalization precision
- hashing precision

These layers must never be implicitly coupled.