# ============================================================
# FILE: run-pipeline.ps1
# PURPOSE: Full AstronoSphere Pipeline Run (M1)
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

# (Rotation erfolgt innerhalb der EphemerisFactory – KISS für M1)

# ============================================================
# STEP 2: RUN EPHEMERIS FACTORY
# ============================================================

Write-Host "Step 2: Run EphemerisFactory"
Write-Host ""

dotnet run --project "$PSScriptRoot\..\..\03_TruthFactory\src\EphemerisFactory\EphemerisFactory.csproj"

if ($LASTEXITCODE -ne 0) {
    Write-Host ""
    Write-Host "FAILURE: EphemerisFactory failed" -ForegroundColor Red
    exit 1
}

# ============================================================
# STEP 3: COMPARE RUN vs LASTRUN
# ============================================================

Write-Host ""
Write-Host "Step 3: Compare Run vs LastRun"
Write-Host ""

& "$PSScriptRoot\compare-run-lastRun.ps1"

if ($LASTEXITCODE -ne 0) {
    Write-Host ""
    Write-Host "FAILURE: Compare detected differences" -ForegroundColor Red
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