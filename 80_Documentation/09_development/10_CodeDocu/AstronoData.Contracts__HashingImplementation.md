# AstronoSphere – Hashing Implementation (M1.9)

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

- CoreHash (AstronoCert)
- RequestHash (AstronoTruth)
- DatasetHash (future)
- SnapshotHash (Analysis)

---

# 2. Architecture Decision (FINAL)

### Input Model

Hashing operates on:

> **Plain C# objects (POCOs)**

NOT allowed:

- JSON strings
- Dictionaries
- dynamic objects
- JsonElement

---

### Responsibility Split

| Component        | Responsibility |
|-----------------|---------------|
| Caller          | Provides fully populated object |
| Canonicalizer   | Deterministic flattening |
| HashCalculator  | SHA256 hashing |

---

# 3. API (FROZEN)

```csharp
string canonical = Canonicalizer.Build(object input);
string hash = HashCalculator.Compute(string canonical);