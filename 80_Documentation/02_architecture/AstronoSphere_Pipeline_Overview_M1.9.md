# AstronoSphere – Pipeline Overview (M1.9)

## Example Scenario

Planet Mercury – Inferior Conjunction (INC)  
HELIO / J2000 / TDB  
JD 2451545–2451546, Step 1H  

---

## 1. AstronoLab

### Input
01_Seeds/Incoming/

### Function
Processes Seeds into structurally valid ScenarioCandidates (without certification)

### Output
01_Seeds/Prepared/

Example:
PLANET-MERCURY-INC__HELIO-J2000-TDB-2451545-2451546-1H.json

---

## 2. AstronoCert

### Input
01_Seeds/Prepared/

### Function
Certifies ScenarioCandidates into official Experiments and assigns CatalogNumber

### Output
02_Experiments/Released/
01_Seeds/Processed/

Example:
AS-000123__PLANET-MERCURY-INC__HELIO-J2000-TDB-2451545-2451546-1H.json

---

## 3. AstronoTruth (Horizons)

### Input
02_Experiments/Released/

### Function
Generates GroundTruth data (Ephemeris)

### Output
03_GroundTruth/Ephemeris/Horizons/Baseline/

Example:
PLANET-MERCURY-INC__HELIO-J2000-TDB-2451545-2451546-1H__EPH-HORIZONS-VEC-L0.json

---

## 4. Astronometria

### Input
02_Experiments/Released/

### Function
Simulates Experiments with configurable model accuracy

### Output
04_Simulations/Astronometria/Baseline/

Examples:
PLANET-MERCURY-INC__HELIO-J2000-TDB-2451545-2451546-1H__ASTRONOMETRIA-MEEUS-VEC-L0.json  
PLANET-MERCURY-INC__HELIO-J2000-TDB-2451545-2451546-1H__ASTRONOMETRIA-VSOP87-VEC-L0.json  

---

## 5. Astronolysis

### Input
02_Experiments/Released/  
03_GroundTruth/...  
04_Simulations/...  

### Function
Analyzes deviations and generates new Seeds

### Output
05_Results/  
01_Seeds/Incoming/

Example Result:
MERCURY-INC-2000_ENGINE-COMPARISON.json

Generated Seed:
PLANET-MERCURY-INC__HELIO-J2000-TDB-2451545-2451546-1H.json

---

## Pipeline Flow

Seeds/Incoming  
→ AstronoLab  
→ Seeds/Prepared  
→ AstronoCert  
→ Experiments/Released  
→ GroundTruth + Simulations  
→ Astronolysis  
→ Results + new Seeds  

---

## Core Principle

AstronoSphere forms a closed scientific loop:

Seed → Experiment → Measurement → Simulation → Analysis → new Seed
