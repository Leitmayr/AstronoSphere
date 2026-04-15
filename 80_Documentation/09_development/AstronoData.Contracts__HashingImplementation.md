# AstronoSphere – AstronoData.Contracts - Hashing Implementation (M1.9)

Version: 1.0
Status: IMPLEMENTED & VALIDATED
Scope: Contracts Layer
Component: AstronoData.Contracts.Hashing

---

# 1. Purpose

This document defines the **final and frozen implementation** of canonical hashing in AstronoSphere.

Hashing provides:

> **Deterministic identity for scientific objects**

It is the foundation for:

* CoreHash (AstronoCert)
* RequestHash (AstronoTruth)
* DatasetHash (future)
* SnapshotHash (Analysis)

---

# 2. Architecture Decision (FINAL)

## Input Model

Hashing operates on:

> **Plain C# objects (POCOs)**

NOT allowed:

* JSON strings
* Dictionaries
* dynamic objects
* JsonElement

---

## Responsibility Split

| Component      | Responsibility                  |
| -------------- | ------------------------------- |
| Caller         | Provides fully populated object |
| Canonicalizer  | Deterministic flattening        |
| HashCalculator | SHA256 hashing                  |

---

# 3. API (FROZEN)

```csharp
string canonical = Canonicalizer.Build(object input);
string hash = HashCalculator.Compute(string canonical);
```

OR via service:

```csharp
IHashService hashService = new HashService();

string canonical = hashService.BuildCanonical(core);
string hash = hashService.ComputeHash(canonical);
```

---

# 4. Canonicalization Rules (STRICT)

## 4.1 General

* Pure function (no side effects)
* Deterministic
* No hidden logic
* No reflection order usage

---

## 4.2 Property Handling

* ONLY public properties
* MUST be sorted alphabetically
* Case-sensitive (Ordinal)

---

## 4.3 Flattening

Dot notation:

```
Parent.Child.Property=value
```

Example:

```
Time.StartJD=2463504.088194000
```

---

## 4.4 Hierarchical Sorting

Final sorting:

```
OrderBy(fullKey, Ordinal)
```

---

## 4.5 Arrays

* Order MUST be preserved
* Indexed notation:

```
Targets[0]=Saturn
```

---

## 4.6 Numeric Normalization

* Truncate to 9 decimal places (NO rounding)
* Zero-pad to 9 dp

Example:

```
2463504.088194 → 2463504.088194000
```

---

## 4.7 Strings

* No modification
* UTF-8 encoding (Hash layer)

---

## 4.8 Boolean

```
true / false (lowercase)
```

---

## 4.9 Null Handling

> NULL VALUES ARE FORBIDDEN

* Any null → Exception
* Guarantees fully defined Core

---

## 4.10 Empty Object

> FORBIDDEN

* If canonicalization produces 0 entries → Exception

---

## 4.11 Unsupported Types

> FORBIDDEN

* DateTime
* complex runtime objects

Allowed types:

* string
* numeric (double, float, decimal)
* bool

---

# 5. Hashing Rules

* Algorithm: SHA256
* Encoding: UTF-8
* Output: UPPERCASE HEX

---

# 6. Debug Requirement (MANDATORY)

Canonical string MUST be printed to console.

Purpose:

* external verification
* reproducibility
* debugging

---

# 7. Validation (PROVEN)

## 7.1 Test Strategy

### Test 1 – Determinism

Same object → same hash (multiple runs)
✔ PASSED

---

### Test 2 – Order Independence

Different property order → same hash
✔ PASSED

---

### Test 3 – Numeric Precision

2451545.1 == 2451545.100000000
✔ PASSED

---

### Test 4 – UTF-8 Stability

"München"
✔ PASSED

---

### Test 5 – Binary Canonical Verification

* Expected canonical string vs generated canonical
* Compared via BeyondCompare

✔ BINARY IDENTICAL

---

### Test 6 – External Hash Verification

* Canonical string copied to external SHA256 tool
* Compared with internal hash

✔ IDENTICAL

---

### Test 7 – Repeatability

* Program executed multiple times
* Hash stable

✔ PASSED

---

# 8. Real Data Validation

Tested with real scenario:

SCN_000023

Canonical output fully matches predicted structure.

Hash verified internally and externally.

---

# 9. Critical Guarantees

This implementation guarantees:

* deterministic identity
* cross-system reproducibility
* scientific traceability

---

# 10. Integration Contract (AstronoCert)

## Input

```csharp
Core core
```

Requirements:

* fully populated
* no null values
* valid structure

---

## Usage

```csharp
var canonical = hashService.BuildCanonical(core);
var coreHash = hashService.ComputeHash(canonical);
```

---

## Constraints

* NO preprocessing
* NO transformation
* NO filtering

The Core must be passed **as-is**

---

# 11. Architectural Importance

This component is:

> **Single Source of Truth for identity in AstronoSphere**

Failure tolerance:

> ZERO

---

# 12. Status

✔ IMPLEMENTED
✔ VALIDATED
✔ READY FOR INTEGRATION

---

End of document.
