# ============================================================
# FILE: 00_AstronoPipe\scripts\run-pipeline.ps1
# STATUS: M1.9 FINAL
# PURPOSE:
# - run AstronoLab
# - run AstronoCert
# - run AstronoTruth
# - stop immediately on first error
# - no Beyond Compare CLI integration
# ============================================================

$ErrorActionPreference = "Stop"

function Write-Step([string]$text) {
    Write-Host ""
    Write-Host "============================================================" -ForegroundColor DarkGray
    Write-Host $text -ForegroundColor Cyan
    Write-Host "============================================================" -ForegroundColor DarkGray
}

function Invoke-DotNetRun {
    param(
        [Parameter(Mandatory = $true)][string]$ProjectFolder,
        [Parameter(Mandatory = $false)][string[]]$Arguments = @()
    )

    if (-not (Test-Path $ProjectFolder)) {
        throw "Project folder not found: $ProjectFolder"
    }

    Push-Location $ProjectFolder
    try {
        Write-Host "Working directory: $ProjectFolder" -ForegroundColor DarkGray

        $argLine = @("run", "--project", ".")
        if ($Arguments.Count -gt 0) {
            $argLine += "--"
            $argLine += $Arguments
        }

        Write-Host "Command: dotnet $($argLine -join ' ')" -ForegroundColor DarkGray
        & dotnet @argLine

        if ($LASTEXITCODE -ne 0) {
            throw "dotnet run failed in: $ProjectFolder (ExitCode=$LASTEXITCODE)"
        }
    }
    finally {
        Pop-Location
    }
}

try {
    $scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
    $repoRoot  = Resolve-Path (Join-Path $scriptDir "..\..")

    Write-Step "AstronoSphere M1.9 Pipeline Run"
    Write-Host "Repo root: $repoRoot" -ForegroundColor DarkGray

    $astronoLabFolder   = Join-Path $repoRoot "01_AstronoLab\src\AstronoLab"
    $astronoCertFolder  = Join-Path $repoRoot "02_AstronoCert\src"
    $astronoTruthFolder = Join-Path $repoRoot "03_AstronoTruth\src\EphemerisFactory"

    # ------------------------------------------------------------
    # 1) AstronoLab
    # Erwartung:
    # 01_Seeds\Incoming  -> 01_Seeds\Prepared
    # ------------------------------------------------------------
    Write-Step "[1/3] Running AstronoLab"
    Invoke-DotNetRun -ProjectFolder $astronoLabFolder

    # ------------------------------------------------------------
    # 2) AstronoCert
    # Erwartung:
    # 01_Seeds\Prepared  -> 02_Experiments\Released
    # 01_Seeds\Prepared  -> 01_Seeds\Processed
    # ------------------------------------------------------------
    Write-Step "[2/3] Running AstronoCert"
    Invoke-DotNetRun -ProjectFolder $astronoCertFolder

    # ------------------------------------------------------------
    # 3) AstronoTruth
    # Erwartung:
    # 02_Experiments\Released -> 03_GroundTruth\...\Run
    # bestehendes Run wird nach LastRun rotiert
    # ------------------------------------------------------------
    Write-Step "[3/3] Running AstronoTruth"
    Invoke-DotNetRun -ProjectFolder $astronoTruthFolder

    Write-Step "Pipeline completed successfully"
    Write-Host "Next manual step:" -ForegroundColor Yellow
    Write-Host "Run == LastRun mit Beyond Compare GUI prüfen." -ForegroundColor Yellow

    exit 0
}
catch {
    Write-Host ""
    Write-Host "PIPELINE FAILED" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    exit 1
}