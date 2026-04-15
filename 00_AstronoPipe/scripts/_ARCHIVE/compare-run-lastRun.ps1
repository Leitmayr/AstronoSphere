# ============================================================
# FILE: compare-run-lastRun.ps1
# PURPOSE: Binary comparison Run vs LastRun (deterministic)
# ============================================================

# ============================================================
# WORKSPACE ROOT ERMITTELN
# ============================================================

$workspaceRoot = Split-Path -Parent (Split-Path -Parent $PSScriptRoot)

# ============================================================
# TARGET PATHS
# ============================================================

$root = Join-Path $workspaceRoot "AstronoData\03_ReferenceData\Runs"

$runDir = Join-Path $root "Run"
$lastRunDir = Join-Path $root "LastRun"

Write-Host "============================================"
Write-Host "COMPARE Run vs LastRun"
Write-Host "============================================"
Write-Host "Workspace: $workspaceRoot"
Write-Host ""

# ============================================================
# VALIDATION
# ============================================================

if (!(Test-Path $runDir)) {
    Write-Host "Run folder not found: $runDir" -ForegroundColor Red
    exit 1
}

if (!(Test-Path $lastRunDir)) {
    Write-Host "LastRun folder not found: $lastRunDir" -ForegroundColor Red
    exit 1
}

# ============================================================
# FILE COLLECTION
# ============================================================

$runFiles = Get-ChildItem $runDir -Recurse -File | Where-Object { 
    $_.Name -ne ".gitkeep" -and $_.Extension -eq ".json"
}

$total = 0
$ok = 0
$diff = 0
$missing = 0

# ============================================================
# COMPARISON LOOP
# ============================================================

foreach ($file in $runFiles) {

    $total++

    $relativePath = $file.FullName.Substring($runDir.Length).TrimStart('\')

    $runFile = $file.FullName
    $lastFile = Join-Path $lastRunDir $relativePath

    if (!(Test-Path $lastFile)) {
        Write-Host "[MISSING] $relativePath" -ForegroundColor Yellow
        $missing++
        continue
    }

    $runHash = Get-FileHash $runFile -Algorithm SHA256
    $lastHash = Get-FileHash $lastFile -Algorithm SHA256

    if ($runHash.Hash -eq $lastHash.Hash) {
        Write-Host "[OK]      $relativePath" -ForegroundColor Green
        $ok++
    }
    else {
        Write-Host "[DIFF]    $relativePath" -ForegroundColor Red
        $diff++
    }
}

# ============================================================
# RESULT
# ============================================================

Write-Host ""
Write-Host "============================================"
Write-Host "RESULT"
Write-Host "============================================"
Write-Host "Total   : $total"
Write-Host "OK      : $ok"
Write-Host "DIFF    : $diff"
Write-Host "MISSING : $missing"
Write-Host ""

if ($diff -eq 0 -and $missing -eq 0) {
    Write-Host "SUCCESS: ALL FILES IDENTICAL" -ForegroundColor Green
    exit 0
}
else {
    Write-Host "FAILURE: DIFFERENCES DETECTED" -ForegroundColor Red
    exit 1
}