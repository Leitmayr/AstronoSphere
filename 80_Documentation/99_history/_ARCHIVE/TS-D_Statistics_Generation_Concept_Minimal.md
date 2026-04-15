# TS-D Statistics Generation Concept (Minimal, Low-Overhead)

Date: 2026-02-26  
Project: Astronometria

## Goal

Generate **minimal but useful** statistical data **during NUnit execution** of TS-D mesh tests, without turning tests into a reporting framework.

Key constraints:

- Tests must remain **fast**, **deterministic**, and **readable**
- Statistics generation must be **optional** (switchable)
- Minimal implementation effort (no Python scripting beyond “load CSV”)

This concept assumes **Option A** for time scales:

> Treat `JD` from Horizons vector output as **TT-equivalent** (TDB ≈ TT for our purposes).

---

## What TS-D Measures

TS-D is a **field test** over time meshes (Core / Extended / Extreme) comparing:

- VSOP87A heliocentric ecliptic **state vectors**
- against Horizons reference vectors

The purpose is not event-geometry correctness (TS-A/TS-B), but:

- numerical stability across epoch ranges
- systematic drift / outliers
- model-accuracy envelope behavior

---

## Minimal Statistics (First Step)

### Sample-Level Values (stored per (planet, mesh, JD))

Only store what is required to compute summaries later.

**Position deviations** (AU):

- `dx = x_ref - x_vsop`
- `dy = y_ref - y_vsop`
- `dz = z_ref - z_vsop`
- `dr = sqrt(dx^2 + dy^2 + dz^2)`

**Velocity deviations** (AU/day):

- `dvx = vx_ref - vx_vsop`
- `dvy = vy_ref - vy_vsop`
- `dvz = vz_ref - vz_vsop`
- `dv = sqrt(dvx^2 + dvy^2 + dvz^2)`

**Context columns** (for grouping/filtering):

- `planet`
- `mesh_id` (1/2/3)
- `mesh_name` (Core/Extended/Extreme)
- `jd`

That’s it for the first iteration.

### Summary KPIs (computed per (planet, mesh))

For `dr` and `dv`, compute:

- `count`
- `mean`
- `rms`  (root mean square)
- `max`
- `p95`  (95th percentile)

These five metrics cover: typical error, energy-like magnitude, worst case, and robust tail behavior.

---

## Output Format

Use **CSV** only (simple, tool-friendly, no custom parsing).

Generate **two CSV files per mesh**:

1. `TS-D_Mesh{mesh_id}_Samples.csv`  
   One row per JD (long/tidy format)

2. `TS-D_Mesh{mesh_id}_Summary.csv`  
   One row per planet

### Samples CSV (schema)

Columns:

- `planet, mesh_id, mesh_name, jd`
- `dx, dy, dz, dr`
- `dvx, dvy, dvz, dv`

### Summary CSV (schema)

Columns:

- `planet, mesh_id, mesh_name, count`
- `mean_dr, rms_dr, max_dr, p95_dr`
- `mean_dv, rms_dv, max_dv, p95_dv`

This format is directly ingestible by Python tools (pandas, ydata-profiling, sweetviz) without extra logic.

---

## Execution Model in NUnit (Low-Overhead)

### Principles

- **Never** compute statistics inside assertions logic.
- Tests should only:
  1. compute VSOP state vector
  2. compare against reference
  3. optionally record deviations

All report generation happens **once** at the end.

### Toggle / Switch

Statistics are enabled only if a runtime switch is set, e.g.:

- environment variable `ASTRO_STATS=1`
- or NUnit TestContext parameter

Default: **disabled**.

### Collector Pattern

- Each TS-D test class owns a `StatisticsCollector` instance.
- Collector stores *only* sample deviations (not full vectors).
- In `[OneTimeTearDown]`, if enabled:
  - compute summaries
  - write CSVs

### Parallelization Safety

To avoid concurrency complexity in iteration 1:

- run TS-D tests **non-parallel**, or ensure the collector is confined to a single test fixture instance.

(Parallel-safe collectors can be added later if needed.)

---

## Recommended Minimal Class Design

### `DeviationSample`

- immutable record/struct containing the sample-level columns

### `StatisticsCollector`

- `Add(sample)`  
- `GetSamplesByMesh(meshId)`

Internally: `Dictionary<int, List<DeviationSample>>`

### `StatisticalReport`

- `Generate(meshId, samples) -> (samplesCsv, summaryCsv)`
- writes files to a known output folder (e.g. `TestContext.CurrentContext.WorkDirectory/TS-D/Stats/`)

---

## Folder & Naming Convention

Output location (example):

- `.../TestResults/TS-D/Stats/`
  - `TS-D_Mesh1_Samples.csv`
  - `TS-D_Mesh1_Summary.csv`
  - `TS-D_Mesh2_Samples.csv`
  - `TS-D_Mesh2_Summary.csv`
  - `TS-D_Mesh3_Samples.csv`
  - `TS-D_Mesh3_Summary.csv`

This is stable, easy to diff, and easy to archive per run.

---

## Later Extensions (Not in Iteration 1)

Only after tests are stable:

- relative errors (`dr/|r_ref|`, `dv/|v_ref|`)
- tolerance utilization (`dr / tolerance_dr`)
- drift slope vs time (linear regression) for Core zone
- export metadata JSON (converter version, dataset version, git hash)

---

## Milestone Note

Once TS-D tests run green **with optional statistics export**, we should add a short project doc entry:
- what KPIs we export
- how to enable stats generation
- where outputs are written
