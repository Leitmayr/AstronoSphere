# AstronoSphere – Test Plan v1.0 (M1.6)
## Scope
Validation of the end-to-end pipeline:
ObservationCatalog → EphemerisFactory → ReferenceData

---

# 1. Test Philosophy

The system must guarantee:

- Determinism (same input → same output)
- Reproducibility (independent of runtime)
- Scientific correctness (aligned with Horizons)
- Robustness (stable under edge conditions)

---

# 2. Test Suites

## 2.1 Pipeline Smoke Test

Input:
- Holy12 scenarios

Assertions:
- Pipeline completes without error
- Exit code = 0
- All steps executed

---

## 2.2 Determinism Tests

### Run vs LastRun
EXPECT:
- Files are byte-identical

### Run vs Released
EXPECT:
- Files are byte-identical

### Time Independence
Run pipeline at different times

EXPECT:
- Identical outputs

---

## 2.3 API Integrity

ASSERT:
- RequestHash(new) == RequestHash(reference)

Ensures:
- Identical Horizons API calls

---

## 2.4 Scenario → Request Mapping

ASSERT:
- Scenario fields correctly mapped to API parameters

Examples:
- Frame → REF_PLANE
- Observer → CENTER

---

## 2.5 TS-A to TS-C Validation

ASSERT:
- JSON identical to EphemerisRegression output (byte-level)

---

## 2.6 TS-D Validation

ASSERT:
- First datapoint matches reference
- Dataset count matches expectation
- No gaps in sequence

---

## 2.7 Idempotency Test

Run pipeline twice

EXPECT:
- No new files created
- No changes in output

---

## 2.8 File System Validation

ASSERT:
- Correct folder structure
- Correct file naming
- Files in correct directories

---

## 2.9 Empty Input Handling

Cases:
- No scenarios found
- Filter returns no results

EXPECT:
- Clean exit
- Informative message

---

## 2.10 Failure Handling

Cases:
- API failure
- File write error

EXPECT:
- Controlled failure OR skip + log
- No corrupted state

---

## 2.11 Logging

EXPECT:
- Scenario ID logged
- Request URL logged
- Result status logged

---

# 3. Debug Strategy

## 3.1 General Approach

When failure occurs:

1. Identify failing scenario
2. Isolate using filter:
   --scenario=<ID>

---

## 3.2 Stepwise Debug

### Step 1 – Compare RequestHash
- If mismatch → mapping error

### Step 2 – Compare JSON
- If mismatch → data processing issue

### Step 3 – Compare CSV (optional)
- Check raw Horizons output

---

## 3.3 Planet Isolation Strategy

If failure in category:

- Re-run with different planets
- Determine if issue is:
  - global
  - planet-specific

---

## 3.4 TS-D Debug

- Check first datapoint
- Check count
- Verify step size indirectly

---

## 3.5 Repeatability Check

Re-run same scenario:

EXPECT:
- identical result

---

# 4. Execution Strategy

## Phase 1 – Smoke
- Few scenarios

## Phase 2 – Category
- TS-A, TS-B, TS-C individually

## Phase 3 – Scale
- TS-D full run

---

# 5. Pass Criteria

System is accepted if:

- All deterministic tests pass
- No differences vs baseline
- No duplicate data generated
- All mappings correct
- No crashes or undefined states

---

# 6. Key Principle

The system validates itself against:

- its own history (Released)
- its own definition (Scenario)
- external truth (Horizons)

→ Fully deterministic scientific pipeline
