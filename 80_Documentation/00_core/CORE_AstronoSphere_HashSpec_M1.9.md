# AstronoSphere Hash Specification (M1.9 – FREEZE)

## 1. Purpose
This document defines the canonical hashing rules for AstronoSphere.

---

## 2. Hash Algorithm
- SHA256
- UTF-8 encoding
- Uppercase hex output

---

## 3. Canonicalization (GLOBAL RULE)

### 3.1 Flattening
Dot notation:
Parent.Child.Key = VALUE

---

### 3.2 Key Rules
- Case-sensitive
- Full path required

---

### 3.3 Sorting (CRITICAL)

Keys MUST be sorted hierarchically:

1. Split key by '.'
2. Compare segments left to right:
   - Parent first
   - then Child
   - then Key
3. Use ordinal (ASCII) string comparison per segment
4. If segments equal, shorter path comes first

Example order:
Frame.Epoch
Observer.Body
Observer.Type
Time.StartJD
Time.StopJD

---

### 3.4 Value Rules

| Type | Rule |
|------|------|
| string | unchanged |
| number | fixed 9 decimal places |
| bool | true/false |
| null | excluded |
| empty | excluded |

---

### 3.5 Numeric Normalization

- exactly 9 decimal places
- truncation (no rounding)
- zero-padding

---

### 3.6 String Handling

- no trimming
- no normalization
- UTF-8 encoding
- special characters preserved

---

### 3.7 Arrays

- keep original order
- no sorting

---

### 3.8 Line Format
KEY=VALUE

---

### 3.9 Join
LF (\n)

---

## 4. CoreHash

Includes:
- Time
- Observer
- ObservedObject (including BodyClass)
- Frame
- Corrections

Note: In M1.9, Corrections are not part of Core.

---

## 5. RequestHash

Canonical API request parameters.

---

## 6. SnapshotHash

Sort → Canonicalize → SHA256

---

## 7. Determinism Rules

Must never change:
- sorting
- numeric format
- encoding
- separators

---

## 8. Debug Requirement

Canonical string must be printed.

---

## 9. Versioning Rule

Any change requires full regeneration.
