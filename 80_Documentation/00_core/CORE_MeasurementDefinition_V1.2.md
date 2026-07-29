# CORE_MeasurementDefinition_V1.1a.md

## Status

Version: V1.1a 
Status: Freeze  
Scope: AstronoSphere / Measurement Semantics / GroundTruth / Simulation / Validation  
Milestone: M2.4+

---

## Change Log

| Revision | Changes | Date |
| -------- | ------- | ---- |
| V1.1 | Initial draft introducing canonical MeasurementDefinition semantics | 2026-05-17 |
| V1.1a | Added explicit HELIO/GEO/TOPO meanings and historical epoch examples | 2026-05-17 |
| V1.2 | Added Examples for MeasurementType in Section 7.3 | 2026-05-17 |
|  | refined definitions for L1 and L2 in Section 7.4 | 2026-05-17 |
|  | changed Sections 7.6...7.9 to 7.5.1...7.5.4, since the parts are elements of Frame | 2026-05-17 |
|  | refined wording in the "Plane" subsection (now 7.5.2): added "ecliptical plane" in ECL and "equatoral plane" in EQU | 2026-05-17 |

---

# 1. Purpose

This document defines the canonical semantics of `MeasurementDefinition` inside AstronoSphere.

A `MeasurementDefinition` specifies:

```text
HOW an astronomical measurement shall be performed.
```

It defines the physical and observational semantics of a measurement.

It does not define:

- a simulation engine
- a GroundTruth provider
- a numerical algorithm
- a numerical theory
- a provider-specific API request

`MeasurementDefinition` is intended to become the shared semantic layer between:

- certified Experiments
- GroundTruth systems
- Astronometria simulations
- validation infrastructure
- future ExplorationRuns
- future StatisticalRuns
- future provider integrations

Examples of GroundTruth systems:

- JPL Horizons
- IMCCE Miriade

Examples of simulation systems:

- Astronometria VSOP87
- future Astronometria models

---

# 2. Core Principle

AstronoSphere separates:

```text
WHAT is observed
```

from:

```text
HOW it is measured
```

and from:

```text
HOW the result is numerically produced
```

The responsibility split is:

| Concept | Responsibility |
| ------- | -------------- |
| Experiment | Defines what physical experiment is performed |
| MeasurementDefinition | Defines how the experiment is measured |
| GroundTruthMapping | Maps the measurement to an external truth provider |
| SimulationMapping | Maps the measurement to a simulation execution |
| SimulationModel | Defines how the simulation engine evaluates the model |

`MeasurementDefinition` is therefore provider-neutral and engine-neutral.

---

# 3. Motivation

AstronoSphere must compare results produced by different systems:

- Horizons
- Miriade
- Astronometria
- future engines or truth providers

Each system uses its own technical interface.

Examples:

- Horizons uses URL parameters.
- Miriade uses its own API parameters.
- Astronometria uses an internal execution structure.

Without a shared semantic layer, every provider and every engine would need its own interpretation of what a measurement means.

This would lead to:

- duplicated configuration
- implicit assumptions
- weak comparability
- fragile validation logic
- provider-specific special cases

`MeasurementDefinition` solves this by defining the measurement once and mapping it outward:

```text
MeasurementDefinition
    ├─ GroundTruthMapping
    └─ SimulationMapping
```

The same measurement semantics can therefore instrument both:

- GroundTruth generation
- simulation execution

This is the foundation for deterministic provider comparison and model validation.

---

# 4. Non-Goals

`MeasurementDefinition` must not become a provider request object.

It must not contain:

- Horizons URL parameters
- Miriade API parameters
- Astronometria method names
- StateTree node sequences
- implementation-specific execution details
- simulation model internals
- native model time-scale rules

Provider-specific and engine-specific details belong to mappings, not to the canonical measurement definition.

---

# 5. Orthogonality Principle

`MeasurementDefinition` is constructed from orthogonal semantic axes.

Each axis describes one independent aspect of the measurement.

Core axes are:

- Domain
- MeasurementType
- CorrectionLevel
- Frame.Origin
- Frame.Plane
- Frame.Epoch
- Frame.TimeScale

The axes must remain independent whenever possible.

This enables:

- deterministic mapping
- provider-neutral semantics
- engine-neutral semantics
- future extension
- clear validation logic

---

# 6. Canonical Structure

The canonical minimal structure is:

```json
{
  "MeasurementDefinition": {
    "Domain": "Ephemeris",
    "MeasurementType": "VEC",
    "CorrectionLevel": "L0",
    "Frame": {
      "Origin": "GEO",
      "Plane": "EQU",
      "Epoch": "J2000",
      "TimeScale": "TDB"
    }
  }
}
```

For M2.4, the supported scope is intentionally limited.

Supported in M2.4:

- Domain = Ephemeris
- MeasurementType = VEC
- CorrectionLevel = L0
- Origin = HELIO or GEO
- Plane = ECL or EQU
- Epoch = J2000
- TimeScale = TDB

Explicitly not included in M2.4:

- topocentric observers
- RA/DEC output
- AZ/ALT output
- apparent observer outputs
- atmospheric refraction
- dynamic graph selection
- TimeDomain branch comparison

---

# 7. Terms and Definitions

## 7.1 MeasurementDefinition

`MeasurementDefinition` defines the complete semantic definition of an astronomical measurement.

It answers:

```text
How shall this astronomical experiment be measured?
```

It describes:

- the scientific measurement domain
- the measurement representation
- the correction semantics
- the spatial reference
- the temporal reference

It does not describe:

- which provider computes the result
- which engine computes the result
- which numerical model computes the result

### Notes

`MeasurementDefinition` replaces the ambiguous idea of an "Instrument" in this context.

The term "Instrument" must not be used as synonym for `MeasurementDefinition`.

Reason:

- "Instrument" may suggest a physical device.
- "Instrument" may suggest only the output format.
- "MeasurementDefinition" explicitly means the complete measurement semantics.

---

## 7.2 Domain

`Domain` defines the scientific category of the measurement.

It answers:

```text
What scientific kind of data is measured?
```

### Examples

- Ephemeris
- OrbitalElements

### Notes

For M2.4 only `Ephemeris` is in scope.

---

## 7.3 MeasurementType

`MeasurementType` defines the representation of the measurement result.

It answers:

```text
In which representation is the measurement expressed?
```

### Examples

- VEC:  Vector output format: karthesion coordinates (x, y, z) in Astronomical Units (AU)
- RADEC:  Right Ascension and Declination, Unit [0...360) degrees
- AZALT:  Azimuth and Altitude
- OBSERVER
- ORBITAL_ELEMENTS

### Notes

For M2.4 only `VEC` is in scope.

`MeasurementType` describes the output semantics, not the engine.

---

## 7.4 CorrectionLevel

`CorrectionLevel` defines the physical correction semantics of the measurement.

It answers:

```text
Which physical correction stage is represented?
```

### Examples

- L0
- L1
- L2

### Notes

For M2.4 only `L0` is in scope.

Conceptual interpretation:

- L0 = geometric
- L1 = geometric + one way light-time corrected
- L2 = geometric + one way light-time corrected + aberration corrected

The exact physical definitions of correction levels are specified in dedicated physics documents and milestone specifications.

---

## 7.5 Frame

`Frame` groups the spatial and temporal reference semantics of the measurement.

It contains:

- Origin
- Plane
- Epoch
- TimeScale

### Notes

`Frame` belongs to the measurement semantics.

It does not define how a simulation model internally evaluates time.



---

### 7.5.1 Origin

`Origin` defines the physical origin of the measurement.

It answers:

```text
From which physical origin is the measurement defined?
```

### Examples

- HELIO = heliocentric
- GEO = geocentric
- TOPO = topocentric

### Notes

For M2.4 only `HELIO` and `GEO` are in scope.

`TOPO` is intentionally deferred.

The abbreviation describes the physical origin of the measurement, not the numerical engine origin.

---

### 7.5.2 Plane

`Plane` defines the reference plane of the measurement.

It answers:

```text
In which reference plane is the measurement expressed?
```

### Examples

- ECL
- EQU

### Notes

For M2.4:

- `ECL` supports heliocentric and geocentric vector measurements in ecliptical plane
- `EQU` is introduced for geocentric equatorial vector measurements in equatorial plane

---

### 7.5.3 Epoch

`Epoch` defines the reference epoch of the coordinate system.

It answers:

```text
To which reference epoch do the coordinates belong?
```

### Examples

- J2000
- J1950
- B1950
- OFDATE

### Notes

For M2.4 only `J2000` is in scope.

`OFDATE` is deferred.

Historical epochs such as `J1950` or `B1950` may be required for legacy catalogs, constellation boundary definitions, or historical astronomical datasets.

---

## 7.5.4 TimeScale

`TimeScale` defines the temporal reference system of the measurement.

It answers:

```text
Which temporal reference system defines the measurement?
```

### Examples

- TDB
- TT
- UTC

### Notes

For M2.4, AstronoSphere Experiments and GroundTruth datasets are TDB-aligned.

Therefore, the M2.4 `MeasurementDefinition.TimeScale` is `TDB`.

Important distinction:

`MeasurementDefinition.TimeScale` is not the same as `SimulationModel.TimeScale`.

`MeasurementDefinition.TimeScale` describes the time semantics of the measurement.

`SimulationModel.TimeScale` describes the native evaluation time scale of a simulation model.

Example:

```json
{
  "MeasurementDefinition": {
    "Frame": {
      "TimeScale": "TDB"
    }
  },
  "Engine": {
    "SimulationModel": {
      "TimeScale": "TT"
    }
  }
}
```

This means:

```text
The measurement is defined in TDB.
The simulation model evaluates natively in TT.
```

The conversion policy is not part of this document.

It belongs to the canonical time architecture and the relevant implementation milestone.

---

# 8. Mapping Principle

`MeasurementDefinition` is provider-neutral and engine-neutral.

Concrete execution requires mappings.

## 8.1 GroundTruthMapping

`GroundTruthMapping` maps a `MeasurementDefinition` to provider-specific GroundTruth request semantics.

Examples:

```text
MeasurementDefinition
→ HorizonsMapping
→ Horizons API request
```

```text
MeasurementDefinition
→ MiriadeMapping
→ Miriade API request
```

GroundTruthMapping may contain provider-specific parameters.

The canonical `MeasurementDefinition` must not.

---

## 8.2 SimulationMapping

`SimulationMapping` maps a `MeasurementDefinition` to simulation-specific execution semantics.

Examples:

```text
MeasurementDefinition
→ AstronometriaMapping
→ simulation terminal output
```

SimulationMapping may contain engine-specific concepts.

The canonical `MeasurementDefinition` must not.

---

# 9. Relationship to Experiments

An `Experiment` defines what physical situation is studied.

A `MeasurementDefinition` defines how that experiment is measured.

Therefore:

```text
Experiment + MeasurementDefinition = fully specified measurement task
```

Examples:

```text
Experiment:
  Venus perihelion around JD 2443872

MeasurementDefinition:
  Ephemeris / VEC / L0 / HELIO / ECL / J2000 / TDB
```

This combination is sufficient to derive:

- a GroundTruth request
- a simulation execution
- a validation comparison target

---

# 10. Relationship to GroundTruth

GroundTruth systems are external providers or truth engines.

Examples:

- JPL Horizons
- IMCCE Miriade

The same `MeasurementDefinition` may be mapped to multiple GroundTruth systems.

This enables provider comparison.

Example:

```text
MeasurementDefinition A
    ├─ HorizonsMapping
    └─ MiriadeMapping
```

Both mappings shall describe the same measurement semantics as far as the provider allows.

Provider limitations must be explicit.

---

# 11. Relationship to Simulation

Simulation systems compute the measurement internally.

Example:

```text
MeasurementDefinition A
    └─ AstronometriaMapping
```

The simulation mapping must preserve the same measurement semantics.

The simulation engine may use a native time scale or internal model representation.

Such implementation details belong to the SimulationModel and the simulation architecture, not to the `MeasurementDefinition`.

---

# 12. Examples

## 12.1 HELIO-ECL L0 VEC

```json
{
  "MeasurementDefinition": {
    "Domain": "Ephemeris",
    "MeasurementType": "VEC",
    "CorrectionLevel": "L0",
    "Frame": {
      "Origin": "HELIO",
      "Plane": "ECL",
      "Epoch": "J2000",
      "TimeScale": "TDB"
    }
  }
}
```

### Notes

This describes a geometric heliocentric ecliptic vector measurement in the J2000 reference frame with TDB measurement semantics.

---

## 12.2 GEO-ECL L0 VEC

```json
{
  "MeasurementDefinition": {
    "Domain": "Ephemeris",
    "MeasurementType": "VEC",
    "CorrectionLevel": "L0",
    "Frame": {
      "Origin": "GEO",
      "Plane": "ECL",
      "Epoch": "J2000",
      "TimeScale": "TDB"
    }
  }
}
```

### Notes

This describes a geometric geocentric ecliptic vector measurement in the J2000 reference frame with TDB measurement semantics.

---

## 12.3 GEO-EQU L0 VEC

```json
{
  "MeasurementDefinition": {
    "Domain": "Ephemeris",
    "MeasurementType": "VEC",
    "CorrectionLevel": "L0",
    "Frame": {
      "Origin": "GEO",
      "Plane": "EQU",
      "Epoch": "J2000",
      "TimeScale": "TDB"
    }
  }
}
```

### Notes

This describes a geometric geocentric equatorial vector measurement in the J2000 reference frame with TDB measurement semantics.

This measurement is introduced as an additional L0 measurement branch for Astronometria M2.4.

---

# 13. Future Extensions

Future extensions may add:

- topocentric observer semantics
- RA/DEC output
- AZ/ALT output
- apparent observer outputs
- additional correction levels
- additional GroundTruth providers
- additional simulation engines
- additional temporal semantics

Any extension must preserve the orthogonality principle.

---

# 14. Explicit Future Boundary: Observer World

Observer outputs such as RA/DEC or AZ/ALT are intentionally not part of M2.4.

They may later be modeled as a separate derived observer measurement domain.

Candidate future architecture:

```text
Physical vector measurements
→ derived observer measurements
```

This future observer layer must not feed back into the physical vector measurement layer.

---

# 15. Explicit Future Boundary: TimeDomain Branches

TimeDomain branch comparison is intentionally not part of M2.4.

M2.4 uses:

```text
MeasurementDefinition.TimeScale = TDB
```

and relies on the active simulation model to declare its native evaluation time scale separately.

Future milestones may introduce explicit TimeDomain branch comparison for models running natively in different time scales.

---

# 16. Canonical Rules

The following rules are canonical:

1. `MeasurementDefinition` defines measurement semantics.
2. `MeasurementDefinition` is provider-neutral.
3. `MeasurementDefinition` is engine-neutral.
4. `MeasurementDefinition` is not a provider request.
5. `MeasurementDefinition` is not a simulation model.
6. `MeasurementDefinition` is not a StateTree.
7. Provider-specific details belong to GroundTruthMapping.
8. Engine-specific details belong to SimulationMapping.
9. Native model time scale belongs to SimulationModel.
10. `Instrument` must not be used as synonym for `MeasurementDefinition`.
11. M2.4 supports only L0 VEC measurements.
12. M2.4 supports only HELIO-ECL, GEO-ECL and GEO-EQU.
13. M2.4 uses TDB measurement semantics.
14. M2.4 does not define topocentric or observer-projection semantics.

---

# 17. Final Principle

The purpose of `MeasurementDefinition` is to keep measurement semantics canonical while allowing different providers and engines to speak their own technical dialects.

In short:

```text
Semantics are central.
Dialects are mapped.
```

# End of Document
