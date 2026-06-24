# VAL_M2.4.1_PHYS-StateHash.md 

## Detail analysis of hash of AS-000003:
- DataHash unchanged: expected - PASSED
- StateHash changed: expected - PASSED
- StateHash manually checked with https://emn178.github.io/online-tools/sha256.html: identical - PASSED
- GitCommit changed: expected - PASSED
- rest of document unchanged: expected - PASSED


## AS-*: All files run

PS C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\04_Astronometria\src\Astronometria.Desktop>\dotnet run -- --all

### TC1: number of data plausible
First Run:
- 235 files created in Run
- 235 files existing in LastRun (yesterday) - identical
- 235 files existing in Baseline
-> amount of files as expected. PASSED.

### TC2: compare scientific content M2.4.1 vs. M2.4.0 implementation
Run (M2.4.1) vs. old Run (M2.4.0 - before change):
- Files in Run and LastRun (yesterday) - identical 
Exception: NodeType, GitBranch, State Hash different -> Expected: passed

Proven with BC5 Rule Based Compare according to Sesssion "Run_LastRun_M2.4.1_Hash"

### TC3: compare scientific content M2.4.1 vs. M2.4.1 implementation 
Scientific/Ephemeris/Run vs. ~/LastRun
First Run/Second Run, both generated after SW change.
- all files binary identical. Expected -> PASSED.

### TC4: compare diag content M2.4.0 vs. M2.4.1 implementation: 

did not manage to compare M2.4.0 vs. M2.4.1 data. Rated as UNCRITICAL.

### TC5: compare diag content M2.4.1 vs. M2.4.1 implementation: 

DiagMessages/Run vs. ~/LastRun

First Run/Second Run, both generated after SW change.
- all files binary identical. Expected -> PASSED.


### Helper Script for CLI:

```text
AstronoSphere Simulation Run/LastRun comparison: M2.4.1 - ignore changes in NodeType, StateHash and GitBranch
The saved BC5 session contains all comparison and importance rules.

log verbose "C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\04_Astronometria\doc\VALIDATION\M2.4.1_R-LR_HashNodeTypeGitBranch_Log.txt"

load "Run <--> LastRun_Hash"

expand all

folder-report layout:side-by-side &
options:display-mismatches &
title:"AstronoSphere Simulation Run vs LastRun" &
output-to:"C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\04_Astronometria\doc\VALIDATION\M2.4.1_R-LR_HashNodeTypeGitBranch_Report.html" &
output-options:html-color
```

### CLI Call
PS C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere> & "C:\Program Files\Beyond Compare 5\BComp.exe" "@04_Astronometria\doc\VALIDATION\BC-SCRIPT__M2.4.1__RunLastRun_HashNodeTypeGitBranch.txt"

### CLI Results

see M2.4.1_R-LR_HashNodeTypeGitBranch_Report.html