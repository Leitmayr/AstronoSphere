# RC6 – Validity Domain Fix

## Status
Completed and validated

## Problem
Horizons returns no data when requested time ranges fall outside
the valid ephemeris domain.

Typical response:
"No ephemeris for target ..."

This resulted in:
- empty datasets
- invalid JSON output
- non-deterministic behavior

## Root Cause
No validation of Horizons response before parsing and dataset creation.

## Fix
Implemented validation in FactoryRunner:

1. Detect invalid responses:
   - "No ephemeris" present
   - missing "$$SOE" marker
   - empty response

2. Detect empty parsed data

3. Skip scenario:
   - no CSV written
   - no JSON written
   - clear console log

## Policy (M1)

ALL-OR-NOTHING DOMAIN POLICY

If any part of the requested time range is outside the Horizons validity domain:
→ the entire dataset is rejected

No clipping or partial recovery is performed.

## Validation

### Test Cases
- AS-000077 (invalid)
- AS-000076 (partial invalid start)
- AS-000125 (partial invalid end)
- AS-000094 (valid)

### Results
- valid → dataset identical → PASS
- invalid → skipped → PASS
- partial invalid → skipped → PASS

## Conclusion
- deterministic behavior achieved
- no invalid datasets generated
- no regression observed

## Future (M2)
- optional domain clipping
- smarter error classification

## Timestamp
2026-04-02T15:55:27.196977
