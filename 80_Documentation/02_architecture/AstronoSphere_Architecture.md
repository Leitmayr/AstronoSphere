# AstronoSphere Architecture

The AstronoSphere architecture separates the astronomical system into
three independent layers.

This separation ensures maintainability and scientific traceability.

---

## System Overview

AstronoSphere
│
├ Astronometria
│
├ AstronoData
│
└ AstronoFactories



---

## Astronometria

Astronomical computation engine.

Provides:

- ephemeris models
- coordinate systems
- time conversions
- physical corrections

Astronometria produces scientific results but does not generate reference data.

---

## AstronoData

Central data infrastructure.

Contains:

- scenario definitions
- validated reference datasets
- statistical evaluation results
- factory run outputs

AstronoData acts as the **scientific data backbone**.

---

## AstronoFactories

Factories generate external ground truth datasets.

Factories follow a common architecture pattern and operate on scenarios from
the ObservationCatalog.

Factories never contain astronomical logic from the engine itself.

Their sole responsibility is to retrieve external reference data.

