# AstronoCert – CatalogNumber Assignment (M1.9)

## 1. Purpose

AstronoCert is responsible for:

> **Assigning a unique, stable CatalogNumber to each new Experiment**

This process must be:

* deterministic
* duplicate-safe
* independent of execution order
* aligned with the CoreHash identity concept

---

## 2. Core Principle

> **CoreHash defines identity.
> CatalogNumber defines registration.**

* If two Experiments have identical CoreHash → they are the same Experiment
* A CatalogNumber is assigned **only once per unique CoreHash**

---

## 3. Input

AstronoCert processes:

* Seeds from `01_Seeds/Prepared`
* Each Seed contains a fully defined **Core**

---

## 4. Processing Steps

### Step 1 – Canonicalization

* Build canonical representation of the Core
* Use the global Hash Specification (M1.9)

---

### Step 2 – CoreHash Generation

```text
CoreHash = SHA256(canonical(Core))
```

* Canonical string MUST be logged (debug requirement)

---

### Step 3 – Load Existing Experiments

Read all files from:

```text
02_Experiments/Released/
```

Extract:

* existing CoreHashes
* existing CatalogNumbers

---

### Step 4 – Duplicate Detection

```text
IF CoreHash exists:
    → SKIP (no new Experiment)
    → Seed may be moved to Processed
```

> No duplicate Experiments are allowed in the system.

---

### Step 5 – CatalogNumber Assignment

If CoreHash is new:

```text
CatalogNumber = max(existing CatalogNumbers) + 1
```

Format:

```text
AS-XXXXXX
```

Example:

```text
AS-000123
```

Rules:

* based on maximum existing number (NOT count)
* gaps in numbering are allowed
* numbering is strictly monotonic increasing

---

### Step 6 – Experiment Creation

Create new Experiment file:

* assign CatalogNumber
* assign CoreHash
* assign ExperimentID
* persist to:

```text
02_Experiments/Released/
```

---

### Step 7 – Seed Finalization

Move processed Seed to:

```text
01_Seeds/Processed/
```

---

## 5. Determinism Guarantees

The process guarantees:

* identical Core → identical CoreHash → no duplication
* CatalogNumbers are stable across reruns
* system is independent of processing order

---

## 6. Forbidden Behaviors

The following are NOT allowed:

* assigning CatalogNumber based on file count
* assigning CatalogNumber based on hash list length
* creating Experiments without duplicate check
* modifying existing Experiments

---

## 7. Design Rationale

### 7.1 Why CoreHash-first?

CoreHash represents the **physical experiment definition**.

This ensures:

* scientific correctness
* identity stability
* reproducibility

---

### 7.2 Why max+1?

Using:

```text
max(existing) + 1
```

ensures:

* robustness against deleted files
* independence from file ordering
* simplicity (KISS)

---

### 7.3 Why no central index (M1.9)?

* filesystem is the single source of truth
* avoids synchronization issues
* keeps system simple and deterministic

---

## 8. Summary

AstronoCert performs:

```text
Seed → Core → Canonicalize → CoreHash

IF exists:
    SKIP

ELSE:
    CatalogNumber = max + 1
    CREATE Experiment
```

---

## 9. Key Principle

> **No new CatalogNumber without a new CoreHash.**
