# AstronoSphere DataStructures

This is the core specification of the data structures in the AstronoSphere pipeline. It is binding with no variants or options, it represents the exact representation. Hence, we call it **canonical**.

This document contains one example data set which is shown in all the states inside the AstronoSphere pipeline: **dataset #23**. 

Dataset #23 represents the following astronomical situation:
Saturn is crossing the L6 Quadrant border, meanding it changes from a heliocentric coordinate state, where initially X > 0° and after crossing < 0°.

In the AstronoSphere pipeline it is first an incoming seed, becomes a prepared seed in AstronoLab, is being certified to an experiment by AstronoCert and ends up as a GroundTruth reference data set after processing by AstronoTruth.

That is the reason, why this document has four chapters, each one representing the data structure #23 in its particular state in the pipeline.


# 1. Incoming Seed 

```json
{
  "GeneratedSeeds":  [
    {
      "SeedCandidate":  {
        "Event":  {
          "Category":  "Quadrant Crossing",
          "Qualifier":  "L6 Crossing: X Coordinate: + to -",
          "Description": "JD 2463505.088194444"
        },
        "Core":  {
          "Time":  {
            "StartJD":  2463504.088194444,
            "StopJD":  2463506.088194444,
            "Step":  "1H",
            "TimeScale":  "TDB"
          },
          "Observer":  {
            "Type":  "Heliocentric",
            "Body":  "Sun"
          },
          "ObservedObject":  {
            "BodyClass":  "Planet",
            "Targets":  [
              "Saturn"
            ]
          },
          "Frame":  {
            "Type":  "HelioEcliptic",
            "Epoch":  "J2000"
          }
        },
        "Metadata":  {
          "Author":  "Scenario Merger",
          "Priority":  1,
          "Status":  {
            "Maturity":  "Created",
            "Visibility":  "Private"
          }
        },
        "Notes":  "Scenario Merger: Generated automatically from 144 existing scenarios."
      },
      "SeedOrigin":  {
        "ResultID":  "144-EXISTING-SCENARIOS_SCENARIO-TRANSFER",
        "Reason":  "Merge of scenario candidates to seeds",
        "Trigger":  "M1.9 Data Model Transfer",
        "CreatedAtUtc":  "2026-04-05T17:05:00Z"
      }
    }
  ]
}
```

# 2. Prepared Seed 

```json
{
  "SchemaVersion": "1.0",
  "ExperimentID": "HELIO-J2000-TDB-2463504-2463506-1H",
  "CatalogNumber": "AS-000023",
  "CoreHash": "TO_BE_REPLACED",
  "Core": {
    "Time": {
      "StartJD": 2463504.088194444,
      "StopJD": 2463506.088194444,
      "Step": "1H",
      "TimeScale": "TDB"
    },
    "Observer": {
      "Type": "Heliocentric",
      "Body": "Sun"
    },
    "ObservedObject": {
      "BodyClass": "Planet",
      "Targets": [
        "Saturn"
      ]
    },
    "Frame": {
      "Type": "HelioEcliptic",
      "Epoch": "J2000"
    }
  },
  "Event": {
    "Category": "Quadrant Crossing",
    "Qualifier": "L6 Crossing: X Coordinate: + to -",
    "Description": "JD 2463505.088194444"
  },
  "Metadata": {
    "Author": "Scenario Merger",
    "Priority": 1,
    "Status": {
      "Maturity": "Released",
      "Visibility": "Private"
    }
  },
  "Notes": "Scenario Merger: Generated automatically from 144 existing scenarios."
}
```

# 3. Experiment

```json
{
  "SchemaVersion": "1.0",
  "ExperimentID": "HELIO-J2000-TDB-2463504-2463506-1H",
  "CatalogNumber": "AS-000023",
  "CoreHash": "F0616A2B",
  "Core": {
    "Time": {
      "StartJD": 2463504.088194444,
      "StopJD": 2463506.088194444,
      "Step": "1H",
      "TimeScale": "TDB"
    },
    "Observer": {
      "Type": "Heliocentric",
      "Body": "Sun"
    },
    "ObservedObject": {
      "BodyClass": "Planet",
      "Targets": [
        "Saturn"
      ]
    },
    "Frame": {
      "Type": "HelioEcliptic",
      "Epoch": "J2000"
    }
  },
  "Event": {
    "Category": "Quadrant Crossing",
    "Qualifier": "L6 Crossing: X Coordinate: + to -",
    "Description": "JD 2463505.088194444"
  },
  "Metadata": {
    "Author": "Scenario Merger",
    "Priority": 1,
    "Status": {
      "Maturity": "Released",
      "Visibility": "Private"
    }
  },
  "Notes": "Scenario Merger: Generated automatically from 144 existing scenarios."
 
}
```

# 4. GroundTruth Reference Dataset

Note about precision: Values reflect Horizons output precision and may differ slightly from input values.

```json
{
  "ExperimentRef": {
    "ExperimentID": "HELIO-J2000-TDB-2463504-2463506-1H",
    "CoreHash": "3B0CD7D4",
    "CatalogNumber": "AS-000023"
  },
"DatasetHeader": {
  "Measurement": {
    "Level": "L0",
    "Type": "VEC"
  },

    "DatasetID": "HELIO-J2000-TDB-2463504-2463506-1H--EPH-HORIZONS-DE440-L0-VEC",

    "TruthMetadata": {
      "CanonicalRequest": "CENTER=@10\nCOMMAND=699\nCSV_FORMAT=YES\nEPHEM_TYPE=VECTORS\nOBJ_DATA=NO\nOUT_UNITS=AU-D\nREF_PLANE=ECLIPTIC\nREF_SYSTEM=ICRF\nSTART_TIME=JD2463504.088194444\nSTEP_SIZE=1H\nSTOP_TIME=JD2463506.088194444",
      "RequestHash": "7F63F0310AE13F102FBCCAFCDBAFFB0678565B1B5BB0611B66B967A654E20F52",
      "EpochHash": "A0703D06A8D91A4DF53CB66160C5611835806E740C88E739DB423B511EA187BF",
      "Requests": [{"CanonicalRequest": "CENTER=@10\nCOMMAND=699\nCSV_FORMAT=YES\nEPHEM_TYPE=VECTORS\nOBJ_DATA=NO\nOUT_UNITS=AU-D\nREF_PLANE=ECLIPTIC\nREF_SYSTEM=ICRF\nSTART_TIME=JD2463504.088194444\nSTEP_SIZE=1H\nSTOP_TIME=JD2463506.088194444", "RequestHash": "7F63F0310AE13F102FBCCAFCDBAFFB0678565B1B5BB0611B66B967A654E20F52", "HorizonsUrl": "https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=699&CENTER=%4010&START_TIME=JD2463504.088194444&STOP_TIME=JD2463506.088194444&STEP_SIZE=1H&EPHEM_TYPE=VECTORS&CSV_FORMAT=YES&OBJ_DATA=NO&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D"}],
      "TruthProviderUrl": "https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=699&CENTER=%4010&START_TIME=JD2463504.088194444&STOP_TIME=JD2463506.088194444&STEP_SIZE=1H&EPHEM_TYPE=VECTORS&CSV_FORMAT=YES&OBJ_DATA=NO&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D",
      "GeneratedAtUtc": null
    },

    "FactoryMetadata": {
      "FactoryName": "EphemerisFactory",
      "FactoryVersion": "1.0.0",
      "Source": "JPL Horizons https://ssd.jpl.nasa.gov/horizons/app.html#/",
      "ReferenceEphemeris": "DE440",
      "Mode": "HELIO",
      "EphemType": "VECTORS",
      "CorrectionLevel": "L0",
      "TimeScale": "TDB"
    },

    "TruthCitation": {
      "Provider": "NASA - Jet Propulsion Laboratory, California Institute of Technology",
      "Source": "https://ssd.jpl.nasa.gov/horizons/",
      "Citation": "PL Solar System Dynamics Group. JPL Horizons On-Line Ephemeris System. California Institute of Technology. Accessed: 2026-03-23. https://ssd.jpl.nasa.gov/horizons/"
    },

    "Provenance": {
      "ScenarioFactory": "AstronoSphere | MeeusScenarioFactory",
      "TruthFactory": "HorizonsTruthFactory",
      "ValidationTarget": {
        "Software": "AstronoSphere",
        "GitCommit": null,
        "GitBranch": null,
        "GitTag": null
      }
    },

    "EngineCitation": {
      "Author": null,
      "Software": null,
      "Citation": null
    },

    "ValidationFingerprint": null
},
"Data": [
  {
    "JD": 2463504.0881944429,
    "Position": { "X": 0.0058761512163683022, "Y": 9.0139253181591137, "Z": -0.1569522876076512 },
    "Velocity": { "X": -0.0058752323285540487, "Y": -3.9393177075647818E-06, "Z": 0.00023389171609691309 }
  },
  {
    "JD": 2463504.1298611099,
    "Position": { "X": 0.0056313496648281707, "Y": 9.0139251509865552, "Z": -0.15694254210490929 },
    "Velocity": { "X": -0.0058752421343079026, "Y": -4.084993730860393E-06, "Z": 0.00023389242918436229 }
  },
 
   ... (third dataset to thirt-to-last dataset truncated in this document)

  {
    "JD": 2463506.0465277759,
    "Position": { "X": -0.0056299746314590806, "Y": 9.0139108369613385, "Z": -0.1564941846041927 },
    "Velocity": { "X": -0.0058757147278691701, "Y": -1.089184072770191E-05, "Z": 0.00023398068370209581 }
  },
  {
    "JD": 2463506.0881944429,
    "Position": { "X": -0.0058747962954170717, "Y": 9.0139103800066636, "Z": -0.1564844353454517 },
    "Velocity": { "X": -0.0058757251523564947, "Y": -1.1041987592380289E-05, "Z": 0.0002339837385673536 }
  }
]
}
```


