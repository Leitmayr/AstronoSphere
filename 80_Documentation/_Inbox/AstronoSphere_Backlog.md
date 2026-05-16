## Provenance und Citation

In ca. M3.X, wenn Provenance stabilisiert wird: ergänzen in Experiments, GroundTruth

1) in AstronoLab/AstronoCert wird Provenance "ExperimentFactory" statt "ScenarioFactory" befüllt (später, beim Refactoring Teil von M2)
ExperimentCitation sollte in ExperimentData hinzugefügt werden:

    "ExperimentCitation": {
      "Provider": "AstronoSphere.AstronoLab",
      "Source": "AstronoSphere: https://github.com/Leitmayr/AstronoSphere",
      "Citation": "M. Hiemer, AstronoSphere - An Astronomical Experiment Factory from the AstronoSphere Ephemeris Validation Framework, 2026, https://github.com/Leitmayr/AstronoSphere"
    },
	
	"Provenance":{
	      "ExperimentFactory": "AstronoSphere.AstronoLab"
	}
	
2) in GroundTruth: Provencance "TruthFactory" wird zu "ExperimentFactory" ergänzt.
TruthCitation wird erzeugt und hinzugefügt. Danach: 

    "ExperimentCitation": {
      "Provider": "AstronoSphere.AstronoLab",
      "Source": "AstronoSphere: https://github.com/Leitmayr/AstronoSphere",
      "Citation": "M. Hiemer, AstronoSphere - An Astronomical Experiment Factory from the AstronoSphere Ephemeris Validation Framework, 2026, https://github.com/Leitmayr/AstronoSphere"
    },
	    "TruthCitation": {
      "Provider": "NASA - Jet Propulsion Laboratory, California Institute of Technology",
      "Source": "https://ssd.jpl.nasa.gov/horizons/",
      "Citation": "PL Solar System Dynamics Group. JPL Horizons On-Line Ephemeris System. California Institute of Technology. Accessed: 2026-03-23. https://ssd.jpl.nasa.gov/horizons/"
    },

	"Provenance":{
	      "ExperimentFactory": "AstronoSphere.AstronoLab",
		        "TruthFactory": "JPL Horizons",
	}

3) in Astronometria: Provenance "SimulationEngine" statt "ValidationTarget" wird ergänzt. EngineCitation wird erzeugt und hinzugefügt.

    "ExperimentCitation": {
      "Provider": "AstronoSphere.AstronoLab",
      "Source": "AstronoSphere: https://github.com/Leitmayr/AstronoSphere",
      "Citation": "M. Hiemer, AstronoSphere - An Astronomical Experiment Factory from the AstronoSphere Ephemeris Validation Framework, 2026, https://github.com/Leitmayr/AstronoSphere"
    },
	
    "TruthCitation": {
      "Provider": "NASA - Jet Propulsion Laboratory, California Institute of Technology",
      "Source": "https://ssd.jpl.nasa.gov/horizons/",
      "Citation": "PL Solar System Dynamics Group. JPL Horizons On-Line Ephemeris System. California Institute of Technology. Accessed: 2026-03-23. https://ssd.jpl.nasa.gov/horizons/"
    },
	
	"EngineCitation": {
      "Provider": "AstronoSphere.Astronometria",
      "Source": "https://github.com/Leitmayr/AstronoSphere",
      "Citation": "M. Hiemer, Astronometria - An Astronomical Simulation Engine from the AstronoSphere Ephemeris Validation Framework, 2026, https://github.com/Leitmayr/AstronoSphere"
    },	

	"Provenance":{
	      "ExperimentFactory": "AstronoSphere.AstronoLab",
		        "TruthFactory": "JPL Horizons",
				"SimulationEngine": "AstronoSphere.Astronometria"
	}

4) in Astronolysis: Provenance "Astronolysis" wird erzeugt und hinzugefügt

    "ExperimentCitation": {
      "Provider": "AstronoSphere.AstronoLab",
      "Source": "AstronoSphere: https://github.com/Leitmayr/AstronoSphere",
      "Citation": "M. Hiemer, AstronoSphere - An Astronomical Experiment Factory from the AstronoSphere Ephemeris Validation Framework, 2026, https://github.com/Leitmayr/AstronoSphere"
    },
 
	"TruthCitation": {
      "Provider": "NASA - Jet Propulsion Laboratory, California Institute of Technology",
      "Source": "https://ssd.jpl.nasa.gov/horizons/",
      "Citation": "PL Solar System Dynamics Group. JPL Horizons On-Line Ephemeris System. California Institute of Technology. Accessed: 2026-03-23. https://ssd.jpl.nasa.gov/horizons/"
    },
	
	"EngineCitation": {
      "Provider": "AstronoSphere.Astronometria",
      "Source": "AstronoSphere: https://github.com/Leitmayr/AstronoSphere",
      "Citation": "M. Hiemer, Astronometria - An Astronomical Simulation Engine from the AstronoSphere Ephemeris Validation Framework, 2026, https://github.com/Leitmayr/AstronoSphere"
    },	

	"AnalysisCitation": {
      "Provider": "AstronoSphere.Astronolysis",
      "Source": "AstronoSphere: https://github.com/Leitmayr/AstronoSphere",
      "Citation": "M. Hiemer, Astronolysis - An Astronomical Analysis Tool from the AstronoSphere Ephemeris Validation Framework, 2026, https://github.com/Leitmayr/AstronoSphere"
    },	

	"Provenance":{
	      "ExperimentFactory": "AstronoSphere.AstronoLab",
		        "TruthFactory": "JPL Horizons",
				"SimulationEngine": "AstronoSphere.Astronometria",
				"AnalysisTool": "AstronoSphere.Astronolysis"
	}

## BuildInfoService

Aus 04_Astronometria zentralisieren nach 

11_AstronoData.IO
  Build/
    BuildInfoService.cs
    BuildInfo.cs

oder 
10_AstronoData.Contracts
  Build/
    BuildInfoService.cs
    BuildInfo.cs

Dann können auch andere Programmteile den GIT Commit Hash abholen.

## TT/TDB

Architekturentscheidung – Vorschlag

1. AstronoSphere global astro time = TDB.
2. Experiment definitions use TDB.
3. GroundTruth datasets remain TDB-aligned.
4. Each simulation model declares its native evaluation time scale.
5. Astronometria converts TDB into the model-native timescale before model evaluation.
6. For VSOP87 in AstronoSphere, native evaluation time remains TT.
7. Validation deltas must be interpreted as model deltas only after time-scale alignment has been made explicit.

Neuer Milestone: Begründung

Inserted after M2.4 and before M2.5 because AstronoSphere globally defines astronomical experiment time in TDB, while individual simulation models may require a model-native evaluation time scale such as TT; this must be architecturally clarified before implementing Light-Time.

## TOPO/ALTAZ

Deferred architectural risk:  
Extension to topocentric and horizontal coordinate states will introduce observer location as a state-relevant dimension and may require revision of StateId/FileName conventions. M2.4 intentionally does not solve this.

## Rauschanalyse
- Quantisierungsrauschen
- Ableitungen v, a
- DE440 Terme identifizieren 
- Vergleich des Rauschverhaltens Miriade und Horizons
- Ideale Abtastzeitpunkte durch verschachtelte Zeitreihe 

##  Garbage Collector/ErrorHandler
- die einzelnen GUIs erzeugen Datenelemente und prüfen auf Probleme
- Probleme z.B. Out Of Bounds, too many Data, inconsistent Data, …
- Kategorisierung: ERROR, WARNUNG, INFO abh. Von Schwere
- Zentraler Sammler/Logger für alle erzeugten oder nicht erzeugten Files zum späteren Debugging
- zentrale Schnittstelle über AstronoData.IO?

# Enge Konjunktionen REGULUS, Aldebaran
-SuW 6/26, S.8




## Rauschanalyse
- Quantisierungsrauschen
- Ableitungen v, a
- DE440 Terme identifizieren 
- Vergleich des Rauschverhaltens Miriade und Horizons
- Ideale Abtastzeitpunkte durch verschachtelte Zeitreihe 

##  Garbage Collector/ErrorHandler
- die einzelnen GUIs erzeugen Datenelemente und prüfen auf Probleme
- Probleme z.B. Out Of Bounds, too many Data, inconsistent Data, …
- Kategorisierung: ERROR, WARNUNG, INFO abh. Von Schwere
- Zentraler Sammler/Logger für alle erzeugten oder nicht erzeugten Files zum späteren Debugging
- zentrale Schnittstelle über AstronoData.IO?

## LifecycleDefinition
- Umgang mit deprecated Files definieren
- Dokumentieren, dass AstronoCert und AstronoTruth nur "Released" files verarbeiten (Metadata.Status.Maturity = "Released"). Gehört ins DataModel-Dokument

## Astronolysis – Edge Case Seed Derivation
- Delta logs (Mesh Validation)

Output:
EdgeCaseSeedsDefinition:
    - detect overshoots (ratio > 1.0)
    - detect plateau regions (ratio ~ 0.95–1.0, sustained)
- cluster by experiment

Extract representative JD windowsValidation:
- reproduce known MVH1 findings:  AS-000279  AS-000334  AS-000338  AS-000280 (plateau)
    
Success Criterion:
- derived seeds match manually identified cases

Wichtig
Du hast damit einen perfekten zukünftigen Test:
Astronolysis muss das finden,was wir heute manuell gefunden haben
Das ist Gold wert.

Fazit
Fokus bleibt auf M2.2 ✔Erkenntnis geht nicht verloren ✔Zukünftige Validierung vorbereitet ✔
Genau so arbeitet man sauber durch die Milestones.

## Astonolysis - BackToBack Testing
- Same Delta Algo As today (Test Framework)
- Same statistics as today (Script)

## Astronolysis:
"Inner Planets Pre-0 Epoch Accuracy Analysis" -> starke Abweichungen vorhanden, Before Christ Analyse!
- drehe Anzahl Parameter in VSOP runter und analysiere, ob sich der Zeitraum verändert, zu dem das Modell schlechter wird

## AstronoTruth
- die geoäquatorialen Experimente 59-72 im Catalog müssen noch einmal mit korrektem Horizons Lauf erstellt werden
- Dateinamen sollten ein AS-xxxxxx Prefix erhalten
- Dateinamen sollten REF_PLANE enthalten, also z.B. ECLIPTIC

## Naming
- die GroundTruth-Daten könnten auch ein Prefix mit dem Experiment gebrauchen. Könnte man am Ende dann konsistent durchziehen, und die Pipeline von Anfang bis Ende rennen lassen. Run == LastRun wäre dann der BackToBack Test

## Astronometria

## 2.2 M2.4 Excludes

M2.4 excludes:

* advanced TimeAnchor semantics
* reference target emission time
* cross-target temporal synchronization
* PathHash
* TransitionHash
* GUI implementation
* accuracy evaluation for every intermediate StateNode by default !! <- needs to be impelemented soon in order to create statistical data base
* Astronolysis interpretation
* seed generation
* L1/L2 implementation


# Annex: Post M2.4 topics

IGNORE THIS ANNEX DURING M2.3 AND M2.4 DEVELOPMENT

# 6. Multi-Target Semantics

Multi-target scenes are not part of M2.3 and M2.4.

## 6.1 Principle

Later after M2.4, multiple planets may be represented inside one ObservationScene.

Example:

ObservationScene:
Observer = Earth
ObservationTimeJD = 2451545.0
Targets = Venus, Jupiter

Each target receives its own StateNodes and StateHashes.

---

## 6.2 Same Scene, Different Target States

Targets share the same SceneContext.

However, target identity is part of State.

Therefore:

Venus StateHash != Jupiter StateHash

even if all other parameters are identical.

The common relation is expressed by the shared ObservationScene and SceneContext, not by equal StateHashes.

---

## 6.3 Comparison Cases Supported after M2.4

M2.4 must support the following modelable cases:

### Case 1: Same target, different levels

>Venus L0 vs Venus L1

Semantics:

* different StateNodes or graph paths
* same target
* different measurement level
* different StateHashes

L1 implementation itself is not part of M2.4.

The structure must be prepared for it.

---

### Case 2: Different targets, same graph point and level

>Venus L1 vs Jupiter L1

Semantics:

* same conceptual graph node type
* different target instances
* different StateHashes
* same ObservationScene

---

### Case 3: Different targets, different levels

>Venus L0 vs Jupiter L1

Semantics:

* same ObservationScene
* different targets
* different levels
* different StateHashes

---

# 7. Temporal Scope

## 7.1 M2.4 Temporal Mode

M2.4 supports only:

TemporalMode = ObservationTime

All targets in the ObservationScene are referenced to the same observer observation time.

---

## 7.2 Deferred Temporal Modes

The following are deferred:

* TargetEmissionTime
* ReferenceTargetEmissionTime
* CustomTime
* cross-target temporal synchronization

Example deferred use case:

Display Jupiter and Venus at the time when the light left Venus.

Reason for deferral:

Advanced temporal anchoring depends on L1 semantics and must not be introduced before L1 is stable.

---

# 8. GroundTruth Resolution

Ground Truth validation does not happen in M2.4 but at a later milestone.

## 8.1 Principle

This means a TerminalNode can be mapped to a GroundTruth request pattern.

For Horizons, this requires:

* a mapping from TerminalNode semantics to Horizons API parameters
* a canonical Horizons request
* a GroundTruth dataset reference
* comparison of TerminalNode Data against GroundTruth Data

---

## 8.2 TruthMappingRef

StateNodes should not embed provider-specific API details directly.

Instead, TerminalNodes may reference a TruthMappingRef.

Example:

TruthMappingRef = HORIZONS.EPHEM.VECTORS.L0.GEOCENTRIC

The mapping service resolves this reference to provider-specific request parameters.

---

## 8.3 Accuracy Evaluation

Accuracy evaluation is performed for TerminalNodes only by default.

A TerminalNode may contain validation metadata such as:

* GroundTruthProvider
* GroundTruthDatasetId
* RequestHash
* Delta metrics
* AccuracyStatus

Intermediate nodes are not accuracy-rated unless explicitly requested in a future debug or research mode.