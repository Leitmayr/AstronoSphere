# M2.1  Mesh & Horizons API Batching Concept
## EphemerisRegression Design Document

---

# Executive Summary

This document defines the Mesh generation strategy and the Horizons API batching architecture for M2.1.

Goals:
- Deterministic and reproducible time sampling
- API-efficient batch processing
- Clean separation of concerns (Mesh ≠ API ≠ VSOP)

Example use cases of generated meshes:
- GroundTruth comparions (Horizons, Miriade) on the same grid
- Statistically meaningful accuracy evaluation of VSOP vs DE-440
- Controlled degradation analysis outside the specification interval

---

# 1. Conceptual Architecture

1. Generate time mesh experiments for all 8 planets by means of AstronoLab and AstronoCert
2. Batch request of Horizons reference data

Mesh generation and API batching are strictly separated concerns.

---

# 2. Time Ranges

There are three independent time range with overlapping regions: "Three Epochs Approach".
Requirements for the three Epochs:


Epoch1:  1600–2400, represents the high-accuracy range of VSOP models
Epoch2:  0000–4000, represents the anno domini epoch. However, rather arbitrarily chosen and accepted as such. 
Epoch3: -4000–8000, represents the Julian Day Epoch somehow. However, rather arbitrarily chosen and accepted as such.


# 2. Mesh Design

## 2.1 Core Principle
- Deterministic
- Planet-dependent experiments
- Reproducible via fixed definition

## 2.2 Mesh types

There are different mesh types depending on the purpose. Every mesh type consists of three independent.

>Epoch1: inner Epoch
Epoch2: medium Epoch
Epoch3: outer Epoch


### 2.2.1 MeshType: Simulation
- Focus: Simulation
- Characteristic: large time range covering largely every point of the defined time range
- Names: 
    - Epoch1: MCRE (Core Mesh) 
    - Epoch2: MXT1 (Extended Mesh) 
    - Epoch3: MXT2 (Outer Mesh)

### 2.2.2 MeshType: Validation 
- Focus: GroundTruthAvailability
- Characteristic: time range determined by the availablity of data of the used GroundTruth provider
    - Epoch1: MVH1 - Mesh Validation Horizons 1 (=Core Mesh for Validation with Horizons) 
    - Epoch2: MVH2 - Mesh Validation Horizons 2 (=Extended Mesh for Validation with Horizons) 
    - Epoch3: MVH3 - Mesh Validation Horizons 3 (=Outer Mesh for Validation with Horizons)

Note: when using Miriade as GroundTruth Provider, naming would adapt to MVM1, MVM2, MVM3.

---

# 3. Dataset Limitations 

The generated Mesh Data will be used to 
1) Conduct simulations with the engine (e.g Astronometria)
2) request Reference Data for validation of the simulations (e.g. Miriade, Horizons)
3) design reasonable analyses of astronomical data

Unlimited design of mesh data would cause computational issues (never ending simulations) or unstable requests at the GroundTruth providers. These are reasons why the number of data sets per Epoch must be controlled.The number of steps N defines the number of samples, which is N + 1.


N shall follow the 

> **sampleNumberRule:**
N < 2000

The sample number rule cuts the Epochs in sub-Epochs. For example, in Epoch1 there may exist sub-Epochs 1.1, 1.2, etc.

# 3. Three-Zone Strategy


## 3.1 Starting Point

The exact starting point of the mesh inside the Epochs follows the following rules:
1) Point on the Simulation Mesh (determined by a globally continuous mesh grid per Epoch)
2) Point inside the Availability range of MeshType Validation 

##  3.2 StepDays
1) different StepSize: the inner mesh has the smallest StepSize, the medium one higher, the outer mesh has the biggest StepSize
2) the StepSizes shall be no simple multiple of each others: this delivers asynchronous meshes with no inter-Mesh resonances
3) the StepSizes shall be no simple multiple of any major planet period. E.g. shall it not be a multiple of a earth year, mars year etc. to avoid clustering the mesh sample points in the same heliocentric region. 
4) Prime numbers for are a good choice to avoid resonances.
5) StepDays shall not be influenced by periods of other celestial objects than planets. E.g. moon periods shall be neglected, since Moon experiments do have different scope.


### Design Rules for Epoch 1

KISS: keep existing experiments and verify step size.
Starting point was 30d (~1 month)

- 360d in one year
- after ~10 years on revolution through the phase room (-> acceptable shuffle in heliocentric coordinates)
- no prime number

-> KISS principle beats the optimal number. 

Recommended:
>**StepDays = 30 days**

### Design Rules for Epoch 2

Starting point was 183d (~0.5 earth years)
- avoid near-integer multiples of dominant orbital periods (e.g. Earth year)
- ensure significant phase shift per step
- be chosen as a prime number above a defined threshold

Definition:
>StepDays = NextPrimeLargerThan(k * BaseStep)

with:
BaseStep = 183 days
k ≈ 1.1

Recommended:
>**StepDays = 211 days**


### Design Rules for Epoch 3

Starting point was 730d (~2 earth years)
- avoid near-integer multiples of dominant orbital periods (e.g. Earth year)
- ensure significant phase shift per step
- be chosen as a prime number above a defined threshold

Definition:
> StepDays = NextPrimeLargerThan(k * BaseStep)

with:
BaseStep = 730 days
k ≈ 1.1

Recommended:
>**StepDays = 809 days**

### Final definition for the StepSize (FREEZE)

- StepSize1 (Core Mesh): 30 days
- StepSize2 (Extended Mesh): 211 days
- StepSize3 (Outer Mesh): 809 days


## 3.3 Stop Point

The stop point is determined by the following rule:

> JD_StopDay = JD_StartDay + N*StepDaysOfEpoch

N must fulfill the rule N < 2000. This is to guarantee stable Horizons API calls and to avoid overload of the API.



# 4. Concrete Simulation Meshes

Important Global Rule:

Within each Epoch, the mesh grid is globally continuous.

- StepDays is constant per Epoch
- The grid is generated once starting from the Epoch start date
- SubEpochs are only slicing constructs for batching and validation
- SubEpoch boundaries MUST NOT reset or shift the grid

This ensures statistical consistency across the entire Epoch.

## 4.1 Epoch 1

### 4.1.1 Cutting Mesh into slices

To fulfill the sampleNumberRule the data is sliced of roughly 100 years. These 100 years slices are the "sub-Epochs".

### 4.1.2 Real start and end, StepDays

The real start JDStart of a sub-Epoch shall be derived from a globally continuous mesh grid.

Definition:
- A global mesh grid is defined for the entire Epoch using a fixed StepDays.
- The grid starts at the first day of the Epoch (e.g. 1.1.1600 for Epoch1).
- All subsequent points are generated by repeatedly adding StepDays.

SubEpochs do NOT reset the grid.
They are only logical slices of a continuous grid.

The StepDays shall be 30 days fixed throughout the entire time range of Epoch1.

Important:
The mesh grid is globally continuous across all SubEpochs.
There is no phase reset at SubEpoch boundaries.

The real end shall be calculated as follows: JDend = JDstart + N * StepDays with N < 2000.
This rule implies, that the last day of the SubEpochs will not necessarily fall on December 31 of the last year of the century. Instead, there will be a remainder. Depending on the century, the last day will be either the 17th or the 18th of December of said last year of the century. For the Core Mesh, the end day is determined by JDEnd = JDStart + 1217*30 days.
That means the entire set of data of one century is 1218 elements which fulfills the sampleNumberRule.

Note: numberSamples = N + 1

### 4.1.3 Resulting data slices for Core Mesh

Epoch1: 
SubEpoch1.1: 1600-1700 -> JD 2305447.5 to JD 2341957.5 = 1.1.1600 to 17.12.1699
SubEpoch1.2: 1700-1800 -> JD 2341972.5 to JD 2378482.5 = 1.1.1700 to 18.12.1799
SubEpoch1.3: 1800-1900 -> JD 2378496.5 to JD 2415006.5 = 1.1.1800 to 18.12.1899
SubEpoch1.4: 1900-2000 -> JD 2415020.5 to JD 2451530.5 = 1.1.1900 to 18.12.1999
SubEpoch1.5: 2000-2100 -> JD 2451544.5 to JD 2488054.5 = 1.1.2000 to 17.12.2099
SubEpoch1.6: 2100-2200 -> JD 2488069.5 to JD 2524579.5 = 1.1.2100 to 18.12.2199
SubEpoch1.7: 2200-2300 -> JD 2524593.5 to JD 2561103.5 = 1.1.2200 to 18.12.2299
SubEpoch1.8: 2300-2400 -> JD 2561116.5 to JD 2597627.5 = 1.1.2300 to 18.12.2399
SubEpoch1.9: 2400-2500 -> JD 2597641.5 to JD 2634151.5 = 1.1.2400 to 17.12.2499



## 4.2 Epoch 2

### 4.2.1 Cutting Mesh into slices

To fulfill the sampleNumberRule the data is sliced of roughly 1000 years. These 1000 years-slices are the "sub-Epochs".


### 4.2.2 Real start and end, StepDays

The real start JDStart of a sub-Epoch shall be the first day of the new Millenium, e.g. Jan 1, 1000 or Jan 1, 2000, etc.

The StepDays shall be 211 days fix throughout the entire time range of Epoch2. 

The real end shall be calculated as follows: JDend = JDstart + N * StepDays with N < 2000.
This rule implies, that the last day of the SubEpochs will not necessarily fall on December 31 of the last year of the millenium. Instead, there will be a remainder. Depending on the millenium, the last day will be either the 30th or the 31st of December of said last year of the millenium. For the Extended Mesh, the end day is determined by JDEnd = JDStart + 1731*211 days.
That means the entire set of data of one century is 1732 elements which fulfills the sampleNumberRule.


### 4.2.3 Resulting data slices for Extended Mesh

Epoch2: 
SubEpoch2.1: 0000 - 1000 -> JD 1721059.5 to JD 2086300.5 = 1.1.0000 to 30.12.0999
SubEpoch2.2: 1000 - 2000 -> JD 2086302.5 to JD 2451543.5 = 1.1.1000 to 31.12.1999
SubEpoch2.3: 2000 - 3000 -> JD 2451544.5 to JD 2816785.5 = 1.1.2000 to 30.12.2999
SubEpoch2.4: 3000 - 4000 -> JD 2816787.5 to JD 3182028.5 = 1.1.3000 to 31.12.3999


## 4.3 Epoch 3

### 4.3.1 Cutting Mesh into slices

To fulfill the sampleNumberRule the data is sliced of roughly 4000 years. These 4000 years-slices are the "sub-Epochs".


### 4.3.2 Real start and end, StepDays

The real start JDStart of a sub-Epoch shall be the first day of the Epoch, e.g. Jan 1, -4000 or Jan 1, 4000, etc.

The StepDays shall be 809 days fix throughout the entire time range of Epoch3. 

The real end shall be calculated as follows: JDend = JDstart + N * StepDays with N < 2000.
This rule implies, that the last day of the SubEpochs will not necessarily fall on December 31 of Epoch3. Instead, there will be a remainder. JDEnd = JDStart + 1805*809 days.
That means the entire set of data of one century is 1806 elements which fulfills the sampleNumberRule.


### 4.3.3 Resulting data slices for Outer Mesh

Epoch3: 
SubEpoch3.1: -4000 - 0000 -> JD  260089.5 to JD 1720334.5 = 1.1.-4000 to 06.01.-0002
SubEpoch3.2:  0000 - 4000 -> JD 1721059.5 to JD 3181304.5 = 1.1.0000  to 06.01.3998
SubEpoch3.3:  4000 - 8000 -> JD 3182029.5 to JD 4642274.5 = 1.1.4000  to 06.01.7998




# 5. Concrete Validation Meshes

As mentioned above, the Ground Truth providers may not cover the entire simulation mesh range. For this, the maximum range must be determined and the validation meshes must be adapted to these ranges.

## 5.1 Horizons data ranges

The minimum and maximum dates available in Horizons were determined by means of dedicated API-requests outside the specified range, to achieve an error return from Horizons which tells about the borders:

### 5.1.1 Range Mercury
>https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=199&EPHEM_TYPE=VECTORS&START_TIME=JD0.5&STOP_TIME=JD2305507.5&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
**Beginn bei JD0**

>https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=199&EPHEM_TYPE=VECTORS&START_TIME=JD9000000&STOP_TIME=JD10000D&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
**No ephemeris for target "Mercury" after A.D. 9999-DEC-30 12:00:00.0000 TDB**

### 5.1.2 Range Venus
>https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=299&EPHEM_TYPE=VECTORS&START_TIME=JD0.5&STOP_TIME=JD2305507.5&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
**Beginn bei JD0**

>https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=299&EPHEM_TYPE=VECTORS&START_TIME=JD9000000&STOP_TIME=JD10000D&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
**No ephemeris for target "Venus" after A.D. 9999-DEC-30 12:00:00.0000 TDB**

### 5.1.3 Range Earth
>https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=399&EPHEM_TYPE=VECTORS&START_TIME=JD0.5&STOP_TIME=JD2305507.5&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
**Beginn bei JD0**

>https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=399&EPHEM_TYPE=VECTORS&START_TIME=JD9000000&STOP_TIME=JD10000D&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
**No ephemeris for target "Earth" after A.D. 9999-DEC-30 12:00:00.0000 TDB**

### 5.1.4 Range Mars
>https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=499&EPHEM_TYPE=VECTORS&START_TIME=JD2305447.5&STOP_TIME=JD2305507.5&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
**No ephemeris for target "Mars" prior to A.D. 1600-JAN-02 00:00:00.0000 TDB**

>https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=499&EPHEM_TYPE=VECTORS&START_TIME=JD9000000&STOP_TIME=JD10000D&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
**No ephemeris for target "Mars" after A.D. 2600-JAN-01 00:00:00.0000 TDB**

### 5.1.5 Range Jupiter
>https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=599&EPHEM_TYPE=VECTORS&START_TIME=JD2305447.5&STOP_TIME=JD2305507.5&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
**No ephemeris for target "Jupiter" prior to A.D. 1600-JAN-11 00:00:00.0000 TDB**

>https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=599&EPHEM_TYPE=VECTORS&START_TIME=JD9000000&STOP_TIME=JD10000D&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
**No ephemeris for target "Jupiter" after A.D. 2200-JAN-09 00:00:00.0000 TDB**

### 5.1.6 Range Saturn
>https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=699&EPHEM_TYPE=VECTORS&START_TIME=JD2305447.5&STOP_TIME=JD2305507.5&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
**No ephemeris for target "Saturn" prior to A.D. 1749-DEC-31 00:00:00.0000 TDB**

>https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=699&EPHEM_TYPE=VECTORS&START_TIME=JD9000000&STOP_TIME=JD10000D&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
**No ephemeris for target "Saturn" after A.D. 2250-JAN-05 00:00:00.0000 TDB**

### 5.1.7 Range Uranus
>https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=799&EPHEM_TYPE=VECTORS&START_TIME=JD2305447.5&STOP_TIME=JD2305507.5&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
**No ephemeris for target "Uranus" prior to A.D. 1600-JAN-05 00:00:00.0000 TDB**
>https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=799&EPHEM_TYPE=VECTORS&START_TIME=JD9000000&STOP_TIME=JD10000D&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
**No ephemeris for target "Uranus" after A.D. 2399-DEC-16 00:00:00.0000 TDB**

### 5.1.8 Range Neptune
>https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=899&EPHEM_TYPE=VECTORS&START_TIME=JD2305447.5&STOP_TIME=JD2305507.5&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
**No ephemeris for target "Neptune" prior to A.D. 1600-JAN-05 00:00:00.0000 TDB**

>https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=899&EPHEM_TYPE=VECTORS&START_TIME=JD9000000&STOP_TIME=JD10000D&STEP_SIZE=30D&CENTER=@10&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D
**No ephemeris for target "Neptune" after A.D. 2400-JAN-01 00:00:00.0000 TDB**

## 5.2: Summary: Planet dependent valid time ranges

Compact table of the API requests:

  Planet  |  Min Date (TDB) | Min JD     | Max Date (TDB)|  Max JD
  ---------|----------------| ---------- | ------------- |  -----------
  Mercury  |  -4713-11-25   |        0.5 |  9999-12-30   |    5373482.5
  Venus    |   -4713-11-25  |        0.5 |  9999-12-30   |    5373482.5
  Earth    |  -4713-11-25   |        0.5 |  9999-12-30   |    5373482.5
  Mars     |   1600-01-02   |  2305448.5 |  2600-01-01   |    2670690.5
  Jupiter  |  1600-01-11    |  2305457.5 |  2200-01-09   |    2524601.5
  Saturn   |  1749-12-31    |  2360233.5 |  2250-01-05   |    2542859.5
  Uranus   |  1600-01-05    |  2305451.5 |  2399-12-16   |    2597625.5
  Neptune  |  1600-01-05    |  2305451.5 |  2400-01-01   |    2597641.5


## 5.3 Determination of Start and Stop Day

The start and Stop Days can be determined with this formal definition

>ValidationMesh = {
    t ∈ SimulationMesh
    AND t ∈ ProviderRange(planet)
}

In the following, a detailed example based on this formal definition is described.

### 5.3.1 JD_Start

JD_Start is defined as the first available Horizons value on the simulation mesh grid of the respective Epoch:
> **JD_Start = first t ∈ SimulationMesh WHERE t ≥ ProviderMin**

#### Example: Epoch1

>- First availabe Saturn Data Point: JD 2360233.5 = 1749-12-31
>- First point which is greater or equal than 2360233.5 but on the grid: JD 2360257.5 = 1750-01-24

Check: 1750-01-24 is 25 days after 1749-12-31: 25 < 30 
-> first point in mesh grid after 1749-12-31

### 5.3.2 Step

The Step is fix and defined Epoch specific, see above.

### 5.3.3 JD_Stop

JD_Stop is defined as the last available Horizons value on the simulation mesh grid of the respective Epoch:
> **JD_Stop  = last  t ∈ SimulationMesh WHERE t ≤ ProviderMax**

#### Example: Epoch1
>- Last availabe Saturn Data Point: JD 2542859.5 = 2250-01-05
>- Last point which is less or equal than 2542859.5 but on the grid: JD 2542837.5 = 14.12.2249

Check: 14.12.2249 is 22 days before 2250-01-05: 22 < 30 
-> last point in mesh grid before 2250-01-05

#### 5.3.4 Result: Example Table MHV1 for Saturn 

(updated manually according to manual Test Results)

Sub-Epoch   | Range     |  JD_Start | JD_Stop      | UTC_Start |  UTC_Stop
------------|---------  | --------- | ---------    |---------  |  --------
SubEpoch1.1 | 1600-1700 |     -     |     -        |     -     |      -    
SubEpoch1.2 | 1700-1800 | 2360257.5 | 2378467.5    | 24.1.1750 |  03.12.1799
SubEpoch1.3 | 1800-1900 | 2378497,5 | 2415007.5    | 2.1.1800  |  19.12.1899
SubEpoch1.4 | 1900-2000 | 2415037.5 | 2451517.5    | 18.1.1900 |  05.12.1999
SubEpoch1.5 | 2000-2100 | 2451547.5 | 2488057.5    | 4.1.2000  |  20.12.2099
SubEpoch1.6 | 2100-2200 | 2488087.5 | 2524567.5    | 19.1.2100 |  06.12.2199
SubEpoch1.7 | 2200-2300 | 2524597.5 | 2542837.5    | 5.1.2200  |  14.12.2249
SubEpoch1.8 | 2300-2400 |     -     |     -        |     -     |      -    
SubEpoch1.9 | 2400-2500 |     -     |     -        |     -     |      -    


The same process needs to be repeated for all Epochs and all Planets.



---

# 6. Horizons API Batching Strategy

DO NOT request per timestamp.
Instead: Use START/STOP/STEP batch calls per segment.


Sequential execution recommended.
Optional 500ms delay between calls.

---

NOTE:
## Appendix A. Random Sampling Layer (formerly Section "3. Random Layer" - DEFERRED)

This concept is intentionally excluded from M2.1.

Reason:
- introduces analysis dimension
- violates single-dimension principle of Stealth Mode

Future location:
- Astronolysis (analysis-driven sampling strategies)

Status:
- postponed
