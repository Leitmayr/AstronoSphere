# VAL_M2.4.2_FrameCleanup.md 

## Detail analysis of hash of AS-000003:
- DataHash unchanged: expected - PASSED
- StateHash changed: expected - PASSED
- StateHash manually checked with https://emn178.github.io/online-tools/sha256.html: 
Web: 28C861A2AC0E0FE9A0D15415F925ADF7F698D4235FD053B8979F6064542DB3E2
Astronometria: 28C861A2AC0E0FE9A0D15415F925ADF7F698D4235FD053B8979F6064542DB3E2
identical - PASSED
- GitCommit changed: expected - PASSED
- Epoch changed. Expected: comma added - PASSED
- StateHash changed, Frame is part of SceneContext, which is part of StateHash. 
Expected: changed - PASSED
- Type missing. Expected - PASSED
- RefSystem missing. Expected - PASSED




## AS-*: All files run

PS C:\Users\Marcu\source\repos\AstroWorkspace\AstronoSphere\04_Astronometria\src\Astronometria.Desktop>\dotnet run -- --all

### TC1: number of data plausible
First Run:
- 235 files created in Run
- 235 files existing in LastRun (M2.4.1) - identical
- 235 files existing in Baseline
-> amount of files as expected. PASSED.

### TC2: compare scientific content M2.4.2 vs. M2.4.1 implementation
Run (M2.4.2) vs. old Run (M2.4.1 - before change):
- Files in Run and LastRun (M2.4.1) - identical 
Exception: Epoch, Type, RefSystem, GitCommit, State Hash different -> Expected: PASSED

Proven with BC5 Rule Based Compare according to Sesssion "Run_LastRun_M2.4.1_Hash"
Note: old M2.4.1 Session could be re-used

### TC3: compare scientific content M2.4.2 vs. M2.4.1 implementation 
Scientific/Ephemeris/Run vs. ~/LastRun
First Run/Second Run, both generated after SW change.
- all files binary identical. Expected -> PASSED.

### TC4: compare diag content M2.4.2 vs. M2.4.1 implementation: 

DiagMessages/Run vs. ~/LastRun
Run (M2.4.2) == LastRun (M2.4.1) - PASSED.


### TC5: compare diag content M2.4.2 vs. M2.4.1 implementation: 

DiagMessages/Run vs. ~/LastRun

First Run/Second Run, both generated after SW change.
- all files binary identical. Expected -> PASSED.


### Helper Script for CLI:

Note: Re-Use of M2.4.1 Script

```text
AstronoSphere Simulation Run/LastRun comparison: M2.4.2 - ignore changes in NodeType, StateHash and GitBranch
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

see M2.4.2_R-LR_FrameCleanUp_Report.html