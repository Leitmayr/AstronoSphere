# ============================================================
# FILE: run-pipeline.ps1
# PURPOSE: Full AstronoSphere Pipeline Run (M1.5)
# ============================================================

Write-Host "============================================"
Write-Host " AstronoSphere Pipeline Run"
Write-Host "============================================"
Write-Host ""

# ============================================================
# STEP 1: ROTATE RUN -> LASTRUN
# ============================================================

Write-Host "Step 1: Rotate Run -> LastRun"
Write-Host ""

# Rotation passiert in der EphemerisFactory (KISS)

# ============================================================
# STEP 2: RUN EPHEMERIS FACTORY (NON-INTERACTIVE)
# ============================================================

Write-Host "Step 2: Run EphemerisFactory"
Write-Host ""

$projectPath = Join-Path $PSScriptRoot "..\..\03_TruthFactory\src\EphemerisFactory\EphemerisFactory.csproj"

$process = Start-Process `
    -FilePath "dotnet" `
    -ArgumentList "run --project `"$projectPath`"" `
    -NoNewWindow `
    -Wait `
    -PassThru

if ($process.ExitCode -ne 0) {
    Write-Host ""
    Write-Host "FAILURE: EphemerisFactory failed" -ForegroundColor Red
    exit 1
}

# ============================================================
# STEP 3: COMPARE RUN vs LASTRUN (Determinismus)
# ============================================================

Write-Host ""
Write-Host "Step 3: Compare Run vs LastRun"
Write-Host ""

$compareLastRun = Join-Path $PSScriptRoot "compare-run-lastRun.ps1"

& $compareLastRun

if ($LASTEXITCODE -ne 0) {
    Write-Host ""
    Write-Host "FAILURE: Determinism check failed (Run vs LastRun)" -ForegroundColor Red
    exit 1
}

# ============================================================
# STEP 4: COMPARE RUN vs RELEASED (Baseline / CM Gate)
# ============================================================

Write-Host ""
Write-Host "Step 4: Compare Run vs Released"
Write-Host ""

$compareReleased = Join-Path $PSScriptRoot "compare-run-released.ps1"

& $compareReleased

if ($LASTEXITCODE -ne 0) {
    Write-Host ""
    Write-Host "FAILURE: Baseline regression detected (Run vs Released)" -ForegroundColor Red
    exit 1
}

# ============================================================
# DONE
# ============================================================

Write-Host ""
Write-Host "============================================"
Write-Host " PIPELINE SUCCESS"
Write-Host "============================================"
Write-Host ""