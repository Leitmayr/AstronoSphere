# Future ReferenceData Structure: Domain / Provider Separation

Version: Draft 1.0  
Status: Future Architecture Draft (not yet introduced)  
Scope: AstronoSphere / AstronoData / TruthFactories

---

## 1. Purpose

This document defines the **future structural organization** of reference data
inside AstronoData.

It introduces a two-dimensional classification model:

- **Domain** = what kind of truth is measured
- **Provider** = which external truth source delivers the measurement

This structure is **not introduced immediately**.
It is intentionally documented now and will be introduced later, after:

1. L1-L5 stabilization of the EphemerisFactory
2. expansion of the ObservationCatalog with scenarios from TS-A to TS-D

The purpose of this draft is therefore:

- to reduce architectural uncertainty
- to preserve the current design insight
- to prepare a clean migration path for later milestones
- to avoid accidental ad-hoc folder growth

---

## 2. Current Situation

At the current M1 stage, AstronoSphere operates with a single active truth domain:

- **Ephemeris**

and a single active provider:

- **Horizons**

Therefore, a deeper folder hierarchy is not yet required for daily work.
A flatter structure remains appropriate for the current milestone.

However, the architecture has now become clear enough to foresee that AstronoSphere
will not remain a single-pipeline system.

Future truth generation will likely include multiple independent domains, for example:

- Ephemeris
- Observer
- Mesh or other future truth domains

Within a given domain, multiple external providers may also exist, for example:

- Horizons
- Miriade
- future provider systems

This means that future truth data must be organized in a way that keeps:

- domains clearly separated
- providers clearly separated inside a domain
- scientific comparisons manageable
- long-term storage understandable

---

## 3. Fundamental Insight

AstronoSphere does **not** consist of one single truth pipeline only.
Instead, it consists of multiple **parallel domain-specific truth pipelines**.

Examples:

- an **Ephemeris** truth pipeline
- an **Observer** truth pipeline
- possibly further domain-specific pipelines later

These pipelines are related through common architectural principles,
but they are not identical in:

- scientific meaning
- request formats
- output structures
- file naming conventions
- data interpretation

This insight has an important consequence:

> Reference data must not be structured by one universal file naming rule alone.
> Instead, storage must reflect the distinction between **domain** and **provider**.

---

## 4. Core Concept

Future truth data shall be classified by two independent dimensions.

### 4.1 Domain

**Domain** defines **what kind of scientific quantity is measured**.

Examples:

- **Ephemeris**  
  Position and velocity related truth data
- **Observer**  
  Observer-oriented truth data such as RA/DEC, Az/Alt, apparent sky coordinates
- **Mesh** or other future domains  
  Domain-specific truth representations not reducible to ephemeris vectors alone

Domain therefore represents the **measurement class**.

### 4.2 Provider

**Provider** defines **which external truth source delivers the measurement**.

Examples:

- Horizons
- Miriade
- future scientific data providers

Provider therefore represents the **external measurement source**.

---


## 5. Request Identity (Provider Layer)

Each dataset must store a canonical RequestHash representing the exact external query.

This ensures:
- reproducibility
- traceability
- provider independence

RequestHash is distinct from Measurement identity:
- Measurement = logical configuration
- Request     = concrete execution


## 6. Conceptual Mapping

The following conceptual mapping is used:

- **Scenario** = physical experiment / physics definition
- **Truth** = measurement of that physical experiment
- **Domain** = what kind of truth is measured
- **Provider** = who performs or provides the measurement
- **Level** = which measurement configuration or instrument interpretation is used

This can be summarized as:

```text
Scenario = physical reality
Truth    = measurement of that reality

Truth = Domain + Provider + Level
```

This model keeps the existing AstronoSphere logic intact while extending it in a
controlled and understandable way.

---

## 7. Separation of Concerns

A critical design rule must be preserved:

### 7.1 ObservationCatalog owns identity

The ObservationCatalog remains the single source of truth for scenario identity.

It owns:

- ScenarioID
- CatalogNumber
- CoreHash
- Scenario Core

This remains global and pipeline-independent.

### 7.2 ReferenceData does not own identity

Reference datasets do **not** create a new scientific experiment.
They are measurements derived from an existing scenario.

Therefore, ReferenceData storage is responsible for:

- storing truth measurements
- separating domains
- separating providers
- preserving reproducibility

but **not** for creating scenario identity.

### 7.3 Factories are domain-specific

Each TruthFactory belongs to one domain.

Examples:

- EphemerisFactory -> Ephemeris domain
- ObserverFactory -> Observer domain

A provider is not a factory of its own in the architectural sense.
Instead, a provider is a source used by a domain-specific factory.

This means:

- **Factory = domain pipeline**
- **Provider = external truth source**

This distinction prevents architectural duplication.

---

## 8. Proposed Future Folder Structure

The future target structure for released and working reference data is:

```text
AstronoData/
  03_GroundTruth/
    Ephemeris/
      Horizons/
        Run/
        LastRun/
        Baseline/
      Miriade/
        Run/
        LastRun/
        Baseline/

    OrbitalElements/
      Provider1
        ..
      Provider2
        ..
```

This structure is **domain-first** and **provider-second**.

---

## 9. Why Domain-First is Preferred

The folder structure must begin with **Domain**, not with Provider.

### 9.1 Scientific reason

Domains represent fundamentally different types of measurements.
Ephemeris truth and Observer truth are not merely two versions of the same data.
They are different scientific measurement classes.

Therefore they must be separated at the top level.

### 9.2 Extensibility reason

A single domain may later support multiple providers.
For example:

- Ephemeris/Horizons
- Ephemeris/Miriade

This supports direct comparison of provider outputs within the same domain.

### 9.3 KISS reason

Domain-first structure reflects the architectural ownership model:

- EphemerisFactory writes Ephemeris data
- ObserverFactory writes Observer data

This creates a simple, memorable 1:1 mapping between code and storage.

---

## 10. Why Provider-First is Rejected

A provider-first structure such as:

```text
03_ReferenceData/
  Horizons/
    Ephemeris/
    Observer/
```

is rejected for the following reasons:

1. It groups fundamentally different scientific measurements under one provider root.
2. It makes domain comparison harder to read.
3. It weakens the mapping between factory responsibility and target storage.
4. It encourages provider-centric thinking instead of scientific-domain thinking.

AstronoSphere is intended to remain scientifically structured, not tool-structured.

Therefore:

> ReferenceData shall be structured by **Domain first**, then by **Provider**.

---

## 11. Relationship to File Naming

This document concerns **folder structure**, not a single universal file naming scheme.

That distinction is important.

### 11.1 Identity is not defined by the folder

The folder does not define the scientific identity of a dataset.
Identity still derives from:

- ScenarioID
- DatasetID
- CoreHash
- metadata inside the dataset header

### 11.2 File names may remain domain-specific

Different truth domains may require different naming conventions.

For example:

- Ephemeris datasets may use one naming rule
- Observer datasets may use a different one

This is acceptable and desirable.
There is no requirement that all domains share one universal file name schema.

### 11.3 Provider should normally live in the folder, not the file name

If a file already resides in:

```text
Ephemeris/Horizons/
```

then the provider is already expressed by the path.
Repeating it in the file name is usually unnecessary.

This keeps file names shorter and more stable.

Note: Provider must always be stored in DatasetHeader, even if implied by folder structure.

---

## 12. Relationship to DatasetID

The future Domain/Provider structure has implications for DatasetID semantics.

The conceptual rule is:

```text
DatasetID = ScenarioID + measurement identity
```

DatasetID should contain only the minimal human-readable subset of the measurement:

- Level (mandatory)
- Instrument (recommended)

All other measurement details must be stored in DatasetHeader.

Measurement identity consists of:

- Domain
- Provider
- provider-specific model or reference ephemeris
- Level

A conceptual example:

```text
<ScenarioID>--EPH-HORIZONS-DE440-L0
```

Interpretation:

- EPH      = Domain
- HORIZONS = Provider
- DE440    = provider-specific model/reference ephemeris
- L0       = instrument / level

This document does not finalize the DatasetID syntax,
but it confirms the architectural principle that **Provider belongs to dataset identity**.

---

## 13. Why This Is Not Introduced Immediately

Although the future structure is now conceptually clear, introducing it immediately
would not be KISS.

### 13.1 Current M1 reality

Current M1 uses only:

- one active domain: Ephemeris
- one active provider: Horizons
- one active stable level: L0 (with L1-L5 under stabilization)

A deeper domain/provider folder structure would therefore introduce complexity
before it creates practical benefit.

### 13.2 KISS rule

AstronoSphere should not structure the filesystem for hypothetical future complexity
before the second real use case exists.

At the current milestone, the additional provider folder would be mostly empty structure.
That would be architecturally correct, but operationally premature.

### 13.3 Better introduction point

The correct point for introducing the Domain/Provider folder structure is later, when:

1. L1-L5 are stable
2. the ObservationCatalog has been expanded with TS-A to TS-D scenarios
3. either a second domain or a second provider becomes active in practice

At that point, the structure will no longer be speculative.
It will correspond to real, lived system complexity.

---

## 14. Planned Introduction Point

The planned introduction point is therefore:

### Phase 1: current state

- stabilize EphemerisFactory for L1-L5
- keep current simpler data layout
- avoid early folder expansion

### Phase 2: catalog expansion

- extend ObservationCatalog with TS-A to TS-D scenarios
- broaden actual scenario coverage

### Phase 3: structural introduction

- introduce Domain/Provider structure in ReferenceData
- adapt path helpers and file writers
- migrate or regenerate existing data if needed

This staged approach preserves KISS while still respecting future architecture.

---

## 15. Migration Principle

When the future structure is introduced, migration should follow these principles:

1. **Structure follows real use cases**  
   Introduce only when second domain or provider becomes relevant.

2. **Do not break identity rules**  
   ScenarioID, CoreHash, CatalogNumber and DatasetID semantics remain unchanged.

3. **Prefer regeneration over manual file surgery**  
   Since reference data can be regenerated deterministically, migration should prefer
   controlled reruns over fragile manual file moves whenever practical.

4. **Update path helpers centrally**  
   All path changes should be implemented in centralized path utilities.

5. **Keep file naming independent from folder migration**  
   Folder structure and file naming must remain separate concerns.

---

## 16. Architectural Summary

The future AstronoSphere ReferenceData storage model is based on the following rules:

### Rule 1

ObservationCatalog owns scenario identity.

### Rule 2

ReferenceData stores measurements derived from scenarios.

### Rule 3

TruthFactories are domain-specific.

### Rule 4

Providers are truth sources inside a domain.

### Rule 5

ReferenceData shall ultimately be structured by:

1. Domain
2. Provider

### Rule 6

This structure is recognized now, but introduced later.

---

## 17. Final Recommendation

The Domain/Provider structure should be considered **architecturally accepted**,
but **operationally deferred**.

This means:

- the concept is now documented and understood
- current implementation does not need to adopt it immediately
- later milestones should introduce it deliberately, not ad hoc

This is the preferred compromise between:

- architectural clarity
- long-term scalability
- present-day KISS implementation

---

End of document.
