# Beyond Compare 5 Run/LastRun Comparison Framework

## Status

Version: 1.0  
Status: Validated  
Scope: AstronoSphere / Astronometria / Simulation Run/LastRun comparison  
Validated with: Beyond Compare 5.0.1, 64-bit, Standard Edition, German UI  
Date: 2026-06-19

---

## 1. Purpose

This document describes the validated AstronoSphere framework for comparing:

```text
Run
```

against:

```text
LastRun
```

with Beyond Compare 5.

The framework supports automated CLI execution and produces an HTML report containing only relevant differences.

A technically expected difference in:

```json
"GitBranch": "..."
```

is ignored.

All other differences remain relevant and are reported.

The framework is intended for repeated use in:

- MS_2.4.1
- MS_2.4.2
- subsequent AstronoSphere milestones
- future deterministic Run/LastRun validation workflows

---

## 2. Resulting Capabilities

The framework can now:

- compare complete Run and LastRun directories
- execute the comparison from PowerShell or another CLI environment
- ignore the configured `GitBranch` value difference
- detect scientific or structural differences elsewhere in the JSON
- detect files existing on only one side
- generate an HTML report containing only relevant differences
- reuse the same BC5 session and rule set for future runs

---

## 3. Validated Environment

The configuration was created and tested with:

```text
Beyond Compare 5
Version 5.0.1
Build 29877
64-bit
Standard Edition
German user interface
```

The executable used for scripted CLI execution is:

```text
C:\Program Files\Beyond Compare 5\BComp.com
```

`BComp.com` is used because the console waits until the Beyond Compare script has completed.

---

## 4. Comparison Scope

### 4.1 Current directories

Left side:

```text
C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData\04_Simulations\Scientific\Ephemeris\Run
```

Right side:

```text
C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData\04_Simulations\Scientific\Ephemeris\LastRun
```

### 4.2 Current file mask

The custom BC5 text format applies to:

```text
AS-*.json
```

This intentionally limits the rule to AstronoSphere simulation JSON files.

### 4.3 Ignored difference

Only the complete `GitBranch` line is classified as unimportant.

Example:

```json
"GitBranch": "main"
```

and:

```json
"GitBranch": "feature/M2.4-astronometria-state-machine-L0"
```

are treated as equivalent for Run/LastRun comparison.

### 4.4 Relevant differences

All other content remains important, including:

- simulation data
- measurement metadata
- hashes
- node identifiers
- node types
- frame data
- time scale
- engine model metadata
- file presence or absence

---

## 5. One-Time BC5 GUI Configuration

The following configuration is required only once.

All menu and dialog labels below refer to the German BC5 user interface.

---

## 5.1 Create the folder comparison

1. Start Beyond Compare 5.
2. Create a new session of type:

```text
Ordnervergleich
```

3. Set the left folder to:

```text
C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData\04_Simulations\Scientific\Ephemeris\Run
```

4. Set the right folder to:

```text
C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\AstronoData\04_Simulations\Scientific\Ephemeris\LastRun
```

5. Open one matching JSON pair by double-clicking it.

This opens a child:

```text
Textvergleich
```

---

## 5.2 Create the custom text format

In the text comparison:

1. Open:

```text
Extras > Dateiformate...
```

2. Click the upper-left `+`.
3. Create a new text format.
4. Open the tab:

```text
Allgemeines
```

5. Configure:

```text
Maske: AS-*.json
Beschreibung: AstronoSphere Simulation JSON
```

6. Open the tab:

```text
Grammatik
```

7. Click the upper `+` below the grammar list.

Do not use the lower `+` below `Zeilengewichtungen`.

8. Configure the grammar object:

```text
Elementname: GitBranch
Kategorie: Allgemein
```

Enable:

```text
Groß-/Kleinschreibung beachten
Regulärer Ausdruck
```

Keep enabled:

```text
Groß-/Kleinschreibung ist relevant
```

Use this regular expression:

```regex
^\s*"GitBranch"\s*:\s*"[^"]*"\s*,?\s*$
```

9. Confirm with:

```text
OK
```

10. Save the format with:

```text
Speichern unter...
```

Use this format name:

```text
AS-Simulation_RunLastRun_JSON
```

11. Ensure the new format is:

- enabled
- placed at the top of the file-format list
- associated with the mask `AS-*.json`

12. Close the file-format dialog.
13. In the text comparison, click:

```text
Neu laden
```

14. Confirm that both files show the selected format:

```text
AS-Simulation_RunLastRun_JSON
```

---

## 5.3 Classify `GitBranch` as unimportant

In the text comparison:

1. Open:

```text
Sitzung > Sitzungseinstellungen...
```

2. Open the tab:

```text
Wichtigkeit
```

3. In the list of grammar elements:

- clear the checkbox for `GitBranch`
- keep `Alles andere` enabled

4. Enable:

```text
Zeilen-Singles sind immer wichtig
```

5. In the scope dropdown at the bottom, select:

```text
Für alle Dateien innerhalb der Eltern-Sitzung verwenden
```

6. Confirm with:

```text
OK
```

Expected visual result:

- the `GitBranch` difference is still visible
- it is shown as an unimportant difference
- it is no longer treated as a relevant mismatch

---

## 5.4 Configure the parent folder comparison

Return to the parent folder comparison.

Open:

```text
Sitzung > Sitzungseinstellungen...
```

Open the tab:

```text
Vergleich
```

Configure:

```text
Dateigröße vergleichen                         enabled
Datum-/Zeitangaben vergleichen                 enabled
Inhalt vergleichen                             enabled
Regelbasierter Vergleich                       selected
Überspringen, wenn Schnelltest identische
Dateien erkennt                                disabled
Schnelltest-Ergebnisse außer Kraft setzen      enabled
```

Keep the scope:

```text
Nur für diese Ansicht verwenden
```

Confirm with:

```text
OK
```

The important setting is:

```text
Schnelltest-Ergebnisse außer Kraft setzen
```

This allows the rules-based content result to override size or timestamp differences.

The `GitBranch` value may change the file length, but the file can still be treated as equal when no relevant content differs.

---

## 5.5 Save the BC5 session

Save the folder comparison with:

```text
Sitzung > Sitzung speichern unter...
```

Use this session name:

```text
AS-Simulation_RunLastRun
```

This saved session is the central BC5 configuration used by the CLI script.

It contains:

- Run and LastRun folder paths
- rules-based content comparison
- the importance configuration inherited by child text comparisons
- the override of quick-test results

---

## 6. CLI Script

## 6.1 File location

Create:

```text
04_Astronometria\doc\VALIDATION\AS-Simulation_RunLastRun.txt
```

## 6.2 Complete script

```text
# AstronoSphere Simulation Run/LastRun comparison
# The saved BC5 session contains all comparison and importance rules.

log verbose "C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\04_Astronometria\doc\VALIDATION\AS-Simulation_RunLastRun_Log.txt"

load "AS-Simulation_RunLastRun"

expand all

folder-report layout:side-by-side &
options:display-mismatches &
title:"AstronoSphere Simulation Run vs LastRun" &
output-to:"C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\04_Astronometria\doc\VALIDATION\AS-Simulation_RunLastRun_Report.html" &
output-options:html-color
```

---

## 7. PowerShell Execution

Run:

```powershell
& "C:\Program Files\Beyond Compare 5\BComp.com" "@C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\04_Astronometria\doc\VALIDATION\AS-Simulation_RunLastRun.txt"
```

Generated artifacts:

```text
04_Astronometria\doc\VALIDATION\AS-Simulation_RunLastRun_Log.txt
04_Astronometria\doc\VALIDATION\AS-Simulation_RunLastRun_Report.html
```

---

## 8. Report Semantics

The report is generated with:

```text
options:display-mismatches
```

Therefore it contains only relevant mismatches.

The report header contains:

```text
Modus: Unterschiede
Ignoriere Unwichtiges
```

Interpretation:

### Matching file with only `GitBranch` difference

The file does not appear in the report.

### File existing only in `Run`

The file appears as an orphan on the left side.

### File existing only in `LastRun`

The file appears as an orphan on the right side.

### File with a relevant content difference

The file appears as a mismatch on both sides.

---

## 9. Validation Results

The framework was validated with controlled test cases.

### 9.1 Ignored difference test

Difference:

```json
"GitBranch": "main"
```

versus:

```json
"GitBranch": "feature/M2.4-astronometria-state-machine-L0"
```

Expected result:

```text
No relevant mismatch
```

Observed result:

```text
Passed
```

The file was omitted from the mismatch-only HTML report.

### 9.2 Relevant difference test

A relevant engine time-domain value was changed:

```json
"TimeDomain": "TT"
```

to:

```json
"TimeDomain": "TDB"
```

Expected result:

```text
Relevant mismatch
```

Observed result:

```text
Passed
```

The file appeared in the HTML report.

### 9.3 Missing `GitBranch` line

A test with the entire `GitBranch` line removed was not reported as a relevant difference.

This behavior is accepted as out of scope.

Operational assumption:

```text
Every generated AstronoSphere simulation file always contains exactly one GitBranch line.
```

The line is generated automatically by Astronometria.

The framework therefore intentionally validates value differences, not absence of the generated line.

---

## 10. Operational Workflow

For each future Run/LastRun comparison:

1. Generate the new simulation output in `Run`.
2. Keep the previous output in `LastRun`.
3. Execute the PowerShell command.
4. Open:

```text
AS-Simulation_RunLastRun_Report.html
```

5. Interpret:
   - empty report body: no relevant differences
   - orphan files: asymmetric Run/LastRun content
   - paired mismatch: relevant file-content difference
6. Review relevant differences before accepting the run.

---

## 11. Extension for Future Milestones

The framework is designed to be extended without changing the CLI script.

For future milestones such as MS_2.4.1 and MS_2.4.2:

- keep the saved session name
- keep the CLI script
- add or refine grammar elements only when a new technically irrelevant field is explicitly approved
- classify new grammar elements as unimportant only after deliberate review
- validate every new ignore rule with one positive and one negative test

The rule set must remain minimal.

New ignored fields must never be added merely to make a comparison pass.

---

## 12. KISS Rules

The framework follows these principles:

1. Comparison rules live in BC5.
2. The CLI script only loads the validated session and generates the report.
3. No preprocessing of JSON files is required.
4. No temporary normalized copies are required.
5. Original simulation files remain unchanged.
6. Only explicitly approved technical differences are ignored.
7. Scientific differences remain visible.
8. The script stays stable across milestones.

---

## 13. Known Boundary

The framework currently provides:

- deterministic BC5 execution
- mismatch-only HTML reporting
- rule-based significance evaluation

The HTML report is the authoritative human-readable result.

A separate machine-readable process exit-code contract for CI is not defined by this document.

---

## 14. Framework Summary

The validated architecture is:

```text
Astronometria Run / LastRun files
        |
        v
BC5 custom text format
        |
        v
GitBranch grammar element
        |
        v
GitBranch classified as unimportant
        |
        v
Saved folder comparison session
        |
        v
BComp.com CLI script
        |
        v
HTML report with relevant differences only
```

The current state is tested, operational, and reusable for future AstronoSphere milestones.

---

# End of Document
