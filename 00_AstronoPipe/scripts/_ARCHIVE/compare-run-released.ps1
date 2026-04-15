# ============================================================
# FILE: compare-run-released.ps1
# PURPOSE: Binary comparison Run vs Released (Baseline)
# ============================================================

# ============================================================
# WORKSPACE ROOT ERMITTELN
# ============================================================

$workspaceRoot = Split-Path -Parent (Split-Path -Parent $PSScriptRoot)

# ============================================================
# TARGET PATHS
# ============================================================

$runDir = Join-Path $workspaceRoot "AstronoData\03_ReferenceData\Runs\Run"
$releasedDir = Join-Path $workspaceRoot "AstronoData\03_ReferenceData\Released"

Write-Host "============================================"
Write-Host "COMPARE Run vs Released (Baseline)"
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

if (!(Test-Path $releasedDir)) {
    Write-Host "Released folder not found: $releasedDir" -ForegroundColor Red
    exit 1
}

# ============================================================
# FILE COLLECTION (JSON only!)
# ============================================================

$runFiles = Get-ChildItem $runDir -Recurse -File | Where-Object {
    $_.Extension -eq ".json" -and $_.Name -ne ".gitkeep"
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
    $releasedFile = Join-Path $releasedDir $relativePath

    if (!(Test-Path $releasedFile)) {
        Write-Host "[MISSING] $relativePath" -ForegroundColor Yellow
        $missing++
        continue
    }

    $runHash = Get-FileHash $runFile -Algorithm SHA256
    $releasedHash = Get-FileHash $releasedFile -Algorithm SHA256

    if ($runHash.Hash -eq $releasedHash.Hash) {
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
    Write-Host "SUCCESS: RUN MATCHES RELEASED BASELINE" -ForegroundColor Green
    exit 0
}
else {
    Write-Host "FAILURE: RUN DIFFERS FROM RELEASED BASELINE" -ForegroundColor Red
    exit 1
}