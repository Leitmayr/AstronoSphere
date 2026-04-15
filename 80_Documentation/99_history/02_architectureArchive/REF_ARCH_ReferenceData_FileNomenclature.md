# REF_ARCH_ReferenceData_FileNomenclature

## Purpose

This document defines the **canonical file naming convention** for all
reference datasets used in the *Astronometria ecosystem*.

The convention is shared by:

-   **EphemerisRegression** -- deterministic Horizons dataset generator
-   **AstroReferenceData** -- versioned reference dataset repository
-   **Astronometria** -- regression validation engine

Goals:

-   deterministic dataset identification
-   easy filtering by test suite
-   compatibility with automated pipelines
-   stable long‑term archival

------------------------------------------------------------------------

# Canonical Filename Format

All files follow the format:

Planet_TS-Suite_Level_Mode_Event.ext

Fields:

Planet -- celestial body (Mercury, Venus, Earth, Mars, Jupiter, ...)

TS-Suite -- test suite identifier

Level -- ephemeris correction level (L0--L5)

Mode -- coordinate or observation mode

Event -- event description

ext -- file extension (`json` or `csv`)

------------------------------------------------------------------------

# Examples

## TS-A --- Quadrant Crossing Tests

Earth_TS-A_L0_Geo_L0.json\
Earth_TS-A_L0_Geo_L6.json\
Earth_TS-A_L0_Helio_L0.json

Meaning:

Planet: Earth\
Suite: TS-A\
Level: L0\
Mode: Geo / Helio\
Event: quadrant crossing (L0, L6, L12, L18)

------------------------------------------------------------------------

## TS-B --- Ecliptic Node Crossing

Mars_TS-B_L0_Geo_GeoAscendingNode.json\
Mars_TS-B_L0_Geo_GeoDescendingNode.json

Event describes the node crossing relative to the ecliptic.

------------------------------------------------------------------------

## TS-C --- Declination Node Crossing

Jupiter_TS-C_L0_Vector_GeoDecAscendingNode.json\
Jupiter_TS-C_L0_Vector_GeoDecDescendingNode.json

Jupiter_TS-C_L0_Observer_GeoDecAscendingNode.json\
Jupiter_TS-C_L0_Observer_GeoDecDescendingNode.json

Modes:

Vector -- state vectors\
Observer -- observer‑centric coordinates

------------------------------------------------------------------------

## TS-D --- Mesh Sampling

Earth_TS-D_L0_Core.json\
Earth_TS-D_L0_Extended.json\
Earth_TS-D_L0_Extreme.json

Raw chunk files:

Earth_TS-D_L0_Core_Chunk_01.csv\
Earth_TS-D_L0_Core_Chunk_02.csv\
...

------------------------------------------------------------------------

# File Types

Two file types exist:

JSON -- canonical reference dataset used by Astronometria tests

CSV -- raw Horizons export chunks for traceability

JSON files are the **authoritative regression references**.

------------------------------------------------------------------------

# Repository Layout

AstroReferenceData

Run\
└─ TS-A\
└─ L0\
├─ Json\
└─ Raw

└─ TS-B\
└─ L0

└─ TS-C\
└─ L0

└─ TS-D\
└─ L0

Future levels extend naturally:

TS-A/L1\
TS-A/L2\
...

------------------------------------------------------------------------

# Design Principles

Deterministic\
Every dataset is uniquely identifiable from its filename.

Pipeline Friendly\
Filtering by suite is trivial:

**TS-A**\
**TS-B**\
**TS-C**\
**TS-D**

Level Aware\
Supports the full ephemeris pipeline:

L0 → L1 → L2 → L3 → L4 → L5

Stable\
File names remain valid even if algorithms evolve.

------------------------------------------------------------------------

# Summary

Canonical format:

Planet_TS-Suite_Level_Mode_Event.ext

Examples:

Earth_TS-A_L0_Geo_L0.json\
Mars_TS-B_L0_Geo_GeoDescendingNode.json\
Jupiter_TS-C_L0_Vector_GeoDecAscendingNode.json\
Earth_TS-D_L0_Core.json

This naming scheme forms the **backbone of the Astronometria reference
data pipeline**.
