# 1. AstronoSphere Eco System

This document describes the main logical components, work products and data storage inside the AstronoSphere Eco System (AES).
Central part is the AstronoData component which stores the data of the system in various Data Bases. 
This document also describes the work flow during the AES.

# 2. MeeusScenarioFactory

The MeeusScenarioFactory implements various of the approximation algorithms for some of the interesting scenario categories. The algorithms provided by Meeus are rough estimators for astronomical events (see Appendix A.1).
A scenario designer decides which scenarios are supposed to be created by the MeeusScanarioFactors. These rough estimates are considered not more than ScenarioCandidates for the ObservationCatalog.

Like the MeeusScenarioFactory, other Scenario-Generatos can provide such candidates. 

These Scenario generators provide the **data** for the scenario (e.g. Julian Date of a dedicated event for which the EphemerisFactory is supposed to determine the reference data).
Note that the Scenario generators do not implement any part of the scenario header.

# 3. Scenario Candidates

Scenario candidates are stored in AstronoData.
Task to do: specify data format for ScenarioCandidates.

# 4. Scenario Selection Step

ScenarioCandidates are being reviewed by the Maintainer of the ObservationCatalog. He not only maintains the Catalog itself but also the Governance of theScenario generation.
If a candidate gets approval for the catalog, the maintainer fills out thefirst part of the header of the scenario. 
That means, in this step, the **data** provided by the ScenarioGenerators is extended include the first portion of the **header** as well. 
Things to be filled out in the Scenario Selection Step by the Maintainer:
1) Scenario Name
2) Status.Maturity: released
3) Status.Visibility: Private, Public
4) ScenarioType
5) ScenarioCategory
6) EventComment
7) Description
8) Rationale
9) Scientific Purpose
10) Author
11) Extensions

# 5. ScenarioGenerator

The ScenarioGenerator is another tool which eventually completes the header definition:

1) SchemaVersion
2) ScenarioID
3) CatalogNumber
4) CoreHash

# 5.1 Input

Reviewed Scenario Candidates which require completion of the ScenarioHeader.

## 5.2 Logic

### 5.2.1 CatalogNumber

The catalog number is defined as N = (max(existing CatalogNumber) of elements in the ObservationCatalog. That means, every new scenario stored in the ObservationCatalog increments N.
The catalog number shall have a prefix "AS-".
The second part has six digits including leading zeros. It contains the catalog number and it is (N+1). Say (N+1) is 492. Then the second part shall be 000492. The three leading zeros are completing the six digits of the second part.
The resultsing CatalogNumber therefore is: AS-<leadingZeros><N+1)>

### 5.2.2 CoreHash

The Core Hash is calculted as specified in the leading document 02_AstronoSphere_ScenarioDefinition_v1.2.md.

# 5.3 Output

New complete and released Scenario  (ID xy).


# 6. ObservationCatalog

# 6.1 Input

New released scenario  (ID xy)

# 6.2 Logic

The ScenarioHeaderGenerator shall be the one and only instance which can add new scenarios to the ObservationCatalog. For other components of AstronoSphere, write access shall be denied.

The ObservationCatalog contains the released Scenarios. That implies, all required fields in the Scenario definition (header and data) are filled reasonably. The number of elements in the ObservationCatalog is (N+1), e.g. 492 as described in the example in Section 5.1.

# Output 

Released scenario  (ID xy)

# 7. TruthFactories

# 7.1 Input

Released scenario (ID xy)

# 7.2 Logic

TruthFactories need an additional layer to import secnarios. The import layer shall reject scenarios, which are not in Status.Maturity == release. The Truth factories derive all necessary information from the scenario header and data and determine the reference data from the external source they are connected to.

To do: implement import layer and resulting runner

# 7.3 Output

Released Reference Data corresponding to input (ID xy)

# 8. FactoryReferenceData

# 8.1 Input

# 8.2 Logic

Every TruthFactory stores its data in the FactoryReferenceData storage.
Every TruthFactory writes their current data in a sub folder called "Run".
A compare tool can be used for regression and compare the data in "Run" with the data in "LastRun". Only if the data are consistent, the data shall be made available to the next stage of the chain.
"Making available" is done by the Maintainer of the data base.

# 8.3 Output

Released ReferenceData (ID xy)

# 9. Astronometria

# 9.1 Input

- released Scenario (ID xy)
- released ReferenceData (ID xy)

# 9.2 Logic 

Astronometria is the Engine of AstroSphere. Its Testing Stack picks released ScenarioData (e.g. ID xy), runs the Astronometria algorithms and calculates Astronometria Data (ID xy). Internally, it validates if the delta between its calculated AstronometriaData and the FactoryReferenceData is within the tolerances of the scenario. This is the regression part of Astronometria.
Moreover, it stores its Run data in Astronometria Data.

# 9.3 Output 

Astronometria Simulation Data (ID xy)


# 10. Astronometria Data

# 10.1 Input

Astronometria Simulation Data (ID xy)

# 10.2 Logic

Astronometria stores its Data in the AstronometriaData storage.
Astronometria wriths its current data in a sub folder called "Run".
A compare tool can be used for regression and compare the data in "Run" with the data in "LastRun". Only if the data are consistent, the data shall be made available to the next stage of the chain.
"Making available" is done by the Maintainer of the data base.

# 10.3 Output

Released AstronometriaData (ID xy)

# 11. Analysis Tool

# Input

Released AstronometriaData (ID xy)
Released ReferenceData (ID xy)

# Logic

Based on the Data from Astronmetria and the factory reference data, an Analysis Tool (e.g. based on Python, potentially even extrnal) may conduct statistical analysis (diff, mean, 95Perc, trend, ...) and create a dedicated analysis report for a given scenario (ID xy).

# Output

Analysis Report (ID xy)

# 12. Self extending path

The Maintainer may review the analysis reports and determine interesing cases for new simulation scenarios. This final step may push new edge case scenario creation.


# Appendix

## A.1 Scenario Categories

This is a list of scenario categories interesting for astronomical computation. The list is not exhaustive.
1.  Opposition
2.  Conjunction
3.  Inferior Conjunction
4.  Greatest Elongation
5.  Perihelion
6.  Aphelion
7.  Node Crossing
8.  Quadrant Crossing
9.  Stationary Point
10. Zenith Crossing
11. Horizon Crossing
12. Maximum Phase Angle
13. Eclipse Geometry