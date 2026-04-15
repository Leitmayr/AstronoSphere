# AstronoSphere DataStructures

## Change log 

| Revision | Changes | Date |
| -------- | ------- | ---- |
| V1.1     | Changed the example given from DS #1 to be consistent with the Astronosphere Data Model documentation  | 


# Introduction

This is the core specification of the data structures in the AstronoSphere pipeline. It is binding with no variants or options, it represents the exact representation. Hence, we call it **canonical**.

This document contains one example data set which is shown in all the states inside the AstronoSphere pipeline: **dataset #1**. 

Dataset #1 represents the following astronomical situation:
Mercury is in inferior conjunction with the sun: heliocentric longitudes of mercury and earth are identical - tpyical 0°/180° numerical case for trigonometric functions.

In the AstronoSphere pipeline it is first an incoming seed, becomes a prepared seed in AstronoLab, is being certified to an experiment by AstronoCert and ends up as a GroundTruth reference data set after processing by AstronoTruth.

That is the reason, why this document has four chapters, each one representing the data structure #1 in its particular state in the pipeline.


# 1. Incoming Seed 

```json
{
  "GeneratedSeeds":  [
    {
      "SeedCandidate":  {
        "Event":  {
          "Category":  "Inferior Conjunction",
          "Qualifier":  "d/dt Delta/R has zero crossing in Geocentric Coordinates",
          "Description":  2449297.644
        },
        "Core":  {
          "Time":  {
            "StartJD":  2449296.644,
            "StopJD":  2449298.644,
            "Step":  "1H",
            "TimeScale":  "TDB"
          },
          "Observer":  {
            "Type":  "Geocentric",
            "Body":  "Earth"
          },
          "ObservedObject":  {
            "BodyClass":  "Planet",
            "Targets":  [
              "Mercury"
            ]
          },
          "Frame":  {
            "Type":  "GeoEcliptic",
            "Epoch":  "J2000"
          }
        },
        "Metadata":  {
          "Author":  "Scenario Merger",
          "Priority":  1,
          "Status":  {
            "Maturity":  "Released",
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
  "ExperimentID": "HELIO-J2000-TDB-2449296-2449298-1H",
  "CatalogNumber": "AS-000001",
  "CoreHash": "TO_BE_REPLACED",
  "Core": {
    "Time": {
      "StartJD": 2449296.644,
      "StopJD": 2449298.644,
      "Step": "1H",
      "TimeScale": "TDB"
    },
    "Observer": {
      "Type": "Geocentric",
      "Body": "Earth"
    },
    "ObservedObject": {
      "BodyClass": "Planet",
      "Targets": [
        "Mercury"
      ]
    },
    "Frame": {
      "Type": "GeoEcliptic",
      "Epoch": "J2000"
    }
  },
  "Event": {
    "Category": "Inferior Conjunction",
    "Qualifier": "d/dt Delta/R has zero crossing in Geocentric Coordinates",
    "Description": "2449297.644"
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
  "ExperimentID": "GEO-J2000-TDB-2449296-2449298-1H",
  "CatalogNumber": "AS-000001",
  "CoreHash": "DB57A963",
  "Core": {
    "Time": {
      "StartJD": 2449296.644,
      "StopJD": 2449298.644,
      "Step": "1H",
      "TimeScale": "TDB"
    },
    "Observer": {
      "Type": "Geocentric",
      "Body": "Earth"
    },
    "ObservedObject": {
      "BodyClass": "Planet",
      "Targets": [
        "Mercury"
      ]
    },
    "Frame": {
      "Type": "GeoEcliptic",
      "Epoch": "J2000"
    }
  },
  "Event": {
    "Category": "Inferior Conjunction",
    "Qualifier": "d/dt Delta/R has zero crossing in Geocentric Coordinates",
    "Description": "2449297.644"
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
    "ExperimentID": "GEO-J2000-TDB-2449296-2449298-1H",
    "CoreHash": "DB57A963",
    "CatalogNumber": "AS-000001"
  },
  "DatasetHeader": {
    "Measurement": {
      "Level": "L0",
      "Type": "VEC"
    },
    "DatasetID": "GEO-J2000-TDB-2449296-2449298-1H__EPH-HORIZONS-VEC-L0",
    "TruthMetadata": {
      "CanonicalRequest": "CENTER=500@399\nCOMMAND=199\nCSV_FORMAT=YES\nEPHEM_TYPE=VECTORS\nOBJ_DATA=NO\nOUT_UNITS=AU-D\nREF_PLANE=ECLIPTIC\nREF_SYSTEM=ICRF\nSTART_TIME=JD2449296.644\nSTEP_SIZE=1H\nSTOP_TIME=JD2449298.644",
      "RequestHash": "92A5827CC2E1017AB54B7546BFDFD9E4BB6B94D7D7AEB670BE5CB37EF446896A",
      "EpochHash": "BD611F9E55E3376A7170438BE5A7A615AAB87B8C66471B5C6BF99183A04CB3CF",
      "Requests": [
        {
          "CanonicalRequest": "CENTER=500@399\nCOMMAND=199\nCSV_FORMAT=YES\nEPHEM_TYPE=VECTORS\nOBJ_DATA=NO\nOUT_UNITS=AU-D\nREF_PLANE=ECLIPTIC\nREF_SYSTEM=ICRF\nSTART_TIME=JD2449296.644\nSTEP_SIZE=1H\nSTOP_TIME=JD2449298.644",
          "RequestHash": "92A5827CC2E1017AB54B7546BFDFD9E4BB6B94D7D7AEB670BE5CB37EF446896A",
          "HorizonsUrl": "https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=199&CENTER=500%40399&START_TIME=JD2449296.644&STOP_TIME=JD2449298.644&STEP_SIZE=1H&EPHEM_TYPE=VECTORS&CSV_FORMAT=YES&OBJ_DATA=NO&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D"
        }
      ],
      "TruthProviderUrl": "https://ssd.jpl.nasa.gov/api/horizons.api?format=text&COMMAND=199&CENTER=500%40399&START_TIME=JD2449296.644&STOP_TIME=JD2449298.644&STEP_SIZE=1H&EPHEM_TYPE=VECTORS&CSV_FORMAT=YES&OBJ_DATA=NO&REF_PLANE=ECLIPTIC&REF_SYSTEM=ICRF&OUT_UNITS=AU-D",
      "GeneratedAtUtc": null
    },
    "FactoryMetadata": {
      "FactoryName": "EphemerisFactory",
      "FactoryVersion": "1.0.0",
      "Source": "JPL Horizons https://ssd.jpl.nasa.gov/horizons/app.html#/",
      "ReferenceEphemeris": "DE440",
      "Mode": "GEO",
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
      "JD": 2449296.644000000,
      "Position": {"X":-0.47491238861825691,"Y":-0.47714404838520691,"Z":-0.0071081674765764106},
      "Velocity": {"X":-0.01086005277181718,"Y":0.010846849934563269,"Z":0.0040067463829718307}
    },
    {
      "JD": 2449296.685666667,
      "Position": {"X":-0.47536667494483142,"Y":-0.47669346499283471,"Z":-0.0069411643977827639},
      "Velocity": {"X":-0.01094565479335027,"Y":0.010781075122807061,"Z":0.0040093915609741536}
    },
 
   ... (third dataset to third-to-last dataset truncated in this document)

    {
      "JD": 2449298.602333333,
      "Position": {"X":-0.49994398480055402,"Y":-0.45924953588676332,"Z":0.00081712373876384416},
      "Velocity": {"X":-0.01459900752906218,"Y":0.0072632703107187143,"Z":0.0040636448477577997}
    },
    {
      "JD": 2449298.644000000,
      "Position": {"X":-0.50055378037416698,"Y":-0.45894870624995082,"Z":0.00098643503736304487},
      "Velocity": {"X":-0.014671120495782911,"Y":0.0071764825275489981,"Z":0.004063286147123678}
    }
  ]
}
```


