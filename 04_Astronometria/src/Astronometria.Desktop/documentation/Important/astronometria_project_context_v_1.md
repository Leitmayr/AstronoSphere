# Astronometria – Project Context (v1)

---

## 1. Vision & Scope

Astronometria is a long-term hobby project (2–3 years) aiming to build a scientifically precise and didactically usable astronomy application.

The system is intentionally designed as a hybrid tool:

- Interactive for exploration and teaching
- Algorithmically precise for simulation and analysis

### Core Objectives

- High-precision astronomical computation (\~1 arcsecond target accuracy)
- Interactive sky maps in multiple projections
- Simulation and long-term event detection
- Automated generation of monthly astronomical reports
- Modular extensibility (e.g., planetary moon systems)

### Target Users

- Primary: personal use (advanced mode)
- Secondary: educational usage (beginner-friendly mode)

Dual-mode philosophy:

- **Professional Mode** – full numerical transparency and control
- **Beginner Mode** – intuitive, reduced interface

---

## 2. Scientific & Mathematical Foundations

### Angle Model

- Internal unit: **radians**
- Public API: `Angle` struct
- External representation (tests, UI): degrees
- Strict separation between numerical core and presentation

Principle:

> Numerics in radians, transparency in degrees.

---

### Time Model

GUI Level:

- Local system time

Core Level:

- UTC as input
- Conversion to TT using ΔT (Meeus model)
- Ephemeris calculations based on JDE (TT): Julian Date with Dynamical Time

Decisions:

- ΔT model according to Meeus
- No explicit UT1 modeling: UT1 too complex and not needed for 1" accuracy
- UTC ≈ UT for sidereal time purposes
- Valid time range: 1600–2500 (model dependent)

---

### Reference Frame

- Inertial system based on J2000.0
- Effectively compatible with ICRS at \~1" level
- No explicit FK5 implementation
- No high-precision ICRS transformation (overengineering avoided)

---

## 3. Simulation Model

- Purely analytical
- Stateless design
- No numerical N-body integration
- Time supplied exclusively via `ITimeProvider`
- Ephemeris access via `IEphemerisProvider`

Principle:

> Avoid unnecessary complexity. Prefer analytical models over integrators.

Motivation rule:

- Visible results and progress take precedence over theoretical completeness.

---

## 4. Architecture Overview

Layered architecture (strict upward dependency only):

1. UI / Rendering
2. Viewport / Interaction
3. Map Projections
4. Coordinate Transforms
5. Simulation Engine
6. Ephemerides
7. Domain Objects
8. Data Import / Parser

Rendering is fully separated from simulation and ephemeris logic.

---

## 5. Data Policy

### General Principles

- Raw data is never parsed at runtime
- Parsing generates optimized internal data structures
- External data updates are manual only
- Data sets are versionable
- Old snapshots remain reproducible

---

### Ephemerides

- VSOP87A as planetary basis
- Original data archived
- Internal compiled representation used at runtime
- Versioning foreseen

---

### Star Catalog

- Basis: Tycho/Gaia subset
- Limiting magnitude: mag 12
- No proper motion (v1)
- Catalog exchangeable via interface

Storage strategy:

- RA/Dec binning
- 2° × 2° cells
- Fully indexed at application startup
- Magnitude filtering handled at rendering level

---

### Minor Bodies

- Source: MPC
- Manual snapshot updates
- Elliptical elements only (v1)
- Dataset version stored for reproducibility

---

### Reproducibility

Reports shall store:

- Dataset version
- Time model (UTC + ΔT)
- Optional code version

Goal:

> A report must be reproducible years later.

---

## 6. Rendering Status (Current State)

Current prototype:

- Polar projection (Polaris-centered)
- Spectral-type based star coloring (temperature derived from BSC spectral class)
- Monolithic rendering in `StarMapControl.OnRender()`
- CPU-based WPF drawing
- Toggleable visual elements (Stars, Constellations, Planets, Circles, Star Colors)

Rendering refactoring postponed until after ephemeris integration.

---

## 7. Documentation Strategy

Chat discussion language: German

Project documentation (working version): German

Source code (C#, XML comments, public API): English

Detailed architectural documentation maintained in Markdown files inside repository.

---

## 8. Guiding Principles

- Clean separation of concerns
- Deterministic and testable numerical core
- Avoid overengineering
- Prefer clarity over premature optimization
- Preserve long-term maintainability

---

End of Project Context v1

