# AstronoSphere – Backlog (M1.9)

## Status
Active backlog for M1.9 implementation phase  
Focus: deterministic pipeline, architecture stability, zero scientific change

---

# 1. CORE IDENTITY & DETERMINISM (CRITICAL)

## NOW

- Implement **Experiment Hash Index**
  - Key: CoreHash
  - Scope: 02_Experiments (Created + Released)
  - Purpose: duplicate detection, fast lookup

- Implement **Dataset Hash Index**
  - Key: RequestHash / DatasetHash
  - Scope: GroundTruth
  - Purpose: avoid duplicate GT generation

- Canonicalization Debug Output
  - Output canonical string to console before hashing
  - Required for SHA256 external verification

## NEXT

- SnapshotHash implementation (Astronolysis)
  - canonical(sorted(inputs)) → SHA256

- Hash collision policy (definition only, no implementation)

---

# 2. PIPELINE & ORCHESTRATION

## NOW

- Define **AstronoPipe Orchestrator (CLI)**
  - Single entry point
  - Runs full pipeline end-to-end

- Enforce **Full Deterministic Run Mode**
  - Always generate all datasets
  - No SOLL/IST branching (KISS)

## NEXT

- Allow AstronoLab to trigger pipeline (optional hook)

## LATER

- Partial Run Mode (selective execution)

---

# 3. DATA ACCESS & UX

## NOW

- Seed editing capability in AstronoLab
  - Load → modify → save as new Seed

## NEXT

- Experiment Explorer
- Dataset Explorer
- "Clone Experiment → Seed" feature

## LATER

- Advanced search (hash-based, metadata-based)

---

# 4. TOOL RESPONSIBILITIES (STRICT)

## FROZEN RULES

- AstronoLab → Seeds only  
- AstronoCert → Experiments only  
- AstronoTruth → GroundTruth only  
- Astronolysis → Analysis + Seeds  

## NOW

- Enforce boundaries in code (fail fast if violated)

---

# 5. SELF-EXTENDING SYSTEM

## NOW

- Ensure Astronolysis can write Seeds (Incoming)

## NEXT

- Duplicate detection for Seeds (CoreHash)
- Seed prioritization (Metadata.Priority)

## LATER

- Automated Seed filtering / clustering

---

# 6. VALIDATION & REGRESSION

## NOW

- Define Golden Sample Set
- Standardize Compare Scripts

## NEXT

- Centralize tolerance definitions

## LATER

- Automated validation reports

---

# 7. GUI / UX STRATEGY

## NOW

- Define UI baseline (WPF or MAUI)

## NEXT

- Shared Design System
- Align AstronoLab + Astronolysis UI

## LATER

- Unified application shell

---

# 8. FUTURE EXTENSIONS (OUT OF SCOPE M1.9)

- ALTAZ Instrument
- Multi-provider GroundTruth
- MeasurementDefinition (M2)
- Visualization (Astronometria UI)
- Telescope integration (INDI)

---

# 9. KEY PRINCIPLES

- Determinism first
- No hidden logic
- Strict separation of responsibilities
- KISS

---

# 10. PRIORITY SUMMARY

## NOW
- Hash Index
- Canonicalization output
- Orchestrator
- Boundary enforcement
- Golden samples

## NEXT
- Explorers
- SnapshotHash
- Seed deduplication

## LATER
- Partial runs
- Automation
- Multi-provider

---

# End of Backlog
