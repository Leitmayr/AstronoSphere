# SPEC_MeshGenerator_AstronoLab_MappingSpec.md

## ChangeLog

ID   | Version | Change                       | Date         |
--   | ------- | ---------------------------- | -------------|
1    |   1.0   | Initial revision             |  2026-04-23  |


## Purpose

This file is to define which elements from the MeshGenerator files in Seeds/Incoming are being written into what 
elements of the  ExperimentHeader

# 1. Mapping

Input: 00_Seeds/Incoming: newly generated Files from MeshGenerator
Output: 00_Seeds/Prepared: from AstronoLab/MeshGenRunner.cs created Experiment-Candidates for AstronoCert

Seeds/Prepared
This list shows how which element of the Seeds/Prepared are to be filled by data from the Seeds/Incoming

1) "SchemaVersion": "1.0",	-> create exactly like this
2) "ExperimentID": "HELIO-J2000-TDB-2305447-2341957-30D", -> Create based on the data in Seeds/Incoming - Core
3) "CatalogNumber": "AS-000XXX", -> see Chapter 2. below for further details 
4) "CoreHash": "TO_BE_REPLACED", -> create exactly like this
5) "Core": 
	"Core.Time" -> Copy all sub-elements one by one exactly in the same way as specified in Seeds/Incoming. 
	"Core.Observer" -> Copy all sub-elements one by one exactly in the same way as specified in Seeds/Incoming. 
	"Core.ObservedObject" -> Copy all sub-elements one by one exactly in the same way as specified in Seeds/Incoming. 
	"Core.Frame" -> Copy all sub-elements one by one exactly in the same way as specified in Seeds/Incoming. 
6) "Event": 
	{
		"Category": <derived via mapping rules defined in Chapter 3>
		"Qualifier": " ", -> copy "Qualifier" field exactly from SeedCandidate.Event.Qualifier of Seed/Incoming
		"Description": " " -> copy "Description" field exactly from SeedCandidate.Event.Description of Seed/Incoming
	}
7) "Metadata": 1 - 1 Copy from "Metadata" out of Seeds/Incoming
8) "Notes" -> see Chapter 4. below for further details

Note to entry 2): ExperimentID must follow CORE_AstronoSphere_HashSpec_M1.9.md

# 2. CatalogNumber

The CatalogNumber of the first new element shall be AS-000143. Hard coded. It is a one time process to generate the new Mesh Data and hence it is accepted to do this. Furthermore, is fulfills one Key Principle of AstronoSphere: KISS.

# 3. Event.Category

The Event.Category follows a naming convention and mapping table defined in 10_AstronoData.Contracts/src/AstronoData.Contracts/Domain/CategoryMapper.cs. It is important to stick to this mechanism, since the abbreviation needed to create the file name in AstronoLab is determined by this mapping table.
It is required to add new elements to that list first:

Element taken from CategoryMapper.cs and changed where marked:

       public static string ToAbbreviation(string input)
       {
           var normalized = Normalize(input);

           return normalized switch
           {
               "ASCENDING NODE" => "ANO",
               "APHELION" => "APH",
               "CONJUNCTION" => "CON",
               "DESCENDING NODE" => "DNO",
               "INFERIOR CONJUNCTION" => "INC",
               "MESH VALIDATION" => "MCRE", <- will be deleted. Existing Seeds/Processed, Experiments and GroundTruth-Data change their state to "deprecated".
			   "MESH SIMULATION CORE" => "MCRE", <- added
			   "MESH SIMULATION EXTENDED" => "MXT1", <- added
			   "MESH SIMULATION OUTER" => "MXT2", <- added
			   "MESH VALIDATION HORIZONS CORE" => "MVH1", <- added
			   "MESH VALIDATION HORIZONS EXTENDED" => "MVH2", <- added
			   "MESH VALIDATION HORIZONS OUTER" => "MVH3", <- added
               "OPPOSITION" => "OPP",
               "PERIHELION" => "PER",
               "QUADRANT CROSSING" => "QCR",
               "STATION" => "STA",
               "GREATEST WESTERN ELONGATION" => "GWE",
               "GREATEST EASTERN ELONGATION" => "GEE",
               "MISCELLANEOUS DATA POINT" => "MDP",

               // GEO EXTENSIONS
               "GEO EQ ASCENDING NODE" => "GEQ-ANO",
               "GEO EQ DESCENDING NODE" => "GEQ-DNO",

               _ => throw new Exception($"Unknown category: {input} (normalized: {normalized})")
           };
       }

With these additions, the following Mapping applies:

"Event.Category" = MESH SIMULATION CORE, if in Seeds/Incoming "SeedCandidate.Event.Description == MCRE"
"Event.Category" = MESH SIMULATION EXTENDED, if in Seeds/Incoming "SeedCandidate.Event.Description == MXT1"
"Event.Category" = MESH SIMULATION OUTER, if in Seeds/Incoming "SeedCandidate.Event.Description == MXT2"
"Event.Category" = MESH VALIDATION HORIZONS CORE, if in Seeds/Incoming "SeedCandidate.Event.Description == MVH1"
"Event.Category" = MESH VALIDATION HORIZONS EXTENDED, if in Seeds/Incoming "SeedCandidate.Event.Description == MVH2"
"Event.Category" = MESH VALIDATION HORIZONS OUTER, if in Seeds/Incoming "SeedCandidate.Event.Description == MVH3"

# 4. Notes

Notes = ResultID + "<_|_>" + Input.Notes

Attention: other than displayed here, it is <"Underscore"|"Underscore">

If Input.Notes is empty:
Notes = ResultID

# 5. Discussion
a) Does the CatalogNumber determination work? Do you need further information/source files from the verify?
--> solved: fixed start value 143
b) MeshValidation is a key word in the mapping table which is linked to the existing mesh files #72...#143 in the experiment catalog. However, these files are actually MeshType = "Simulation" and not MeshType = "Validation" as indicated by the entry in the mapping table: how can we resolve this?
--> solved: MCRE is the new mapping to Simulations/Core.  Existing Seeds/Processed, Experiments and GroundTruth-Data change their state to "deprecated".
c) What is your thinking about the Notes Chapter?
--> specified more detailed. Solved now.

# 6. Conclusion

All major uncertainties now resolved. 



# ANNEX A: Extract from Canonical Mesh Definitions

...

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

...
