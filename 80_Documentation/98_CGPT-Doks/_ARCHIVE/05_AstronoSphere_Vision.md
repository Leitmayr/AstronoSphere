# AstronoSphere Vision

AstronoSphere is the conceptual ecosystem surrounding the Astronometria engine.

Its goal is to create a scientifically reliable, reproducible framework for
astronomical computation and validation.

The system separates three core responsibilities:

- computation
- reference data
- ground truth generation

This separation allows Astronometria to evolve while maintaining scientific
traceability and reproducibility.

---

## Core Components

AstronoSphere consists of three primary systems:

### Astronometria
The astronomical computation engine.

Responsibilities:
- ephemerides computation
- coordinate transformations
- time systems
- physical corrections

Astronometria represents the **scientific model**.

---

### AstronoData

Central repository for all scenario definitions and scientific reference datasets.

Responsibilities:
- scenario catalog
- reference datasets
- statistical validation results
- factory run artifacts

AstronoData represents the **scientific data infrastructure**.

---

### AstronoFactories

Ground truth generation tools used to produce external reference datasets.

Example factories:

- EphemerisFactory
- TimeFactory
- ObserverFactory

Factories generate datasets that are later validated and promoted to reference data.

---

## Long-Term Goal

AstronoSphere aims to become a reproducible astronomical validation environment.

The system enables:

- controlled scientific regression testing
- reproducible astronomical scenarios
- long-term validation of astronomical models