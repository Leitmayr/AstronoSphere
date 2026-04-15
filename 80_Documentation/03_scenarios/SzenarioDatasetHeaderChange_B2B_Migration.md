# AstronoSphere -- B2B Migration Strategy

## Separation of Scenario and DatasetHeader Generation

## 1. Purpose

This document defines a controlled, deterministic migration strategy for
separating: - Scenario generation (MSF / SHG) - DatasetHeader generation
(TruthFactory)

Goal: - non-breaking changes - bitwise identical outputs - full
reproducibility

## 2. Core Principle

Metadata may change -- results must not change.

## 3. Migration Strategy Overview

Baseline → DatasetHeader Refactor → Scenario Refactor → MSF Integration

## 4. Step 1 -- Baseline Stabilization

-   Run full pipeline (144 scenarios)
-   Validate determinism

Output: RD_baseline

## 5. Step 2 -- DatasetHeader Refactor

-   Move DatasetHeader creation to TruthFactory
-   Ensure deterministic generation

Validation: RD_baseline == RD_step2

## 6. Step 2.5 -- Determinism Check

Run same scenario multiple times: Run1 == Run2

## 7. Step 3 -- Scenario Metadata Refactor

-   Adjust metadata (no Core change!)

Validation: RD_step2 == RD_step3

## 8. Step 4 -- MSF Integration

-   MSF generates candidates
-   SHG generates IDs

Validation: RD_step3 == RD_step4

## 9. Final State

MSF → Candidate → SHG → Scenario → TruthFactory → RD

## 10. Key Rules

-   Single ownership
-   Core immutability
-   Determinism
-   No parallel formats

## 11. Success Criteria

-   All 144 scenarios pass
-   All B2B tests identical

## 12. Summary

Fully deterministic scientific pipeline.
