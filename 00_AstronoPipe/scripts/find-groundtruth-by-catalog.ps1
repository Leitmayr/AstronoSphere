param(
    [Parameter(Mandatory=$true)]
    [string]$CatalogNumber,

    [string]$RepoRoot = "",

    [string]$OutputFolder = "tmp"
)

Write-Host "=== Find GroundTruth by CatalogNumber ==="
Write-Host "CatalogNumber: $CatalogNumber"

# --------------------------------------------------
# Resolve Repo Root
# --------------------------------------------------
if (-not $RepoRoot -or $RepoRoot -eq "") {
    $current = Get-Location

    while ($null -ne $current) {
        if (Test-Path (Join-Path $current "AstronoData")) {
            $RepoRoot = $current
            break
        }
        $current = Split-Path $current -Parent
        if ($current -eq "") { break }
    }
}

if (-not $RepoRoot) {
    throw "Could not determine RepoRoot. Please pass -RepoRoot."
}

Write-Host "RepoRoot: $RepoRoot"

# --------------------------------------------------
# Paths
# --------------------------------------------------
$groundTruthPath = Join-Path $RepoRoot "AstronoData\03_GroundTruth\Ephemeris\Horizons\Baseline"

if (-not (Test-Path $groundTruthPath)) {
    throw "GroundTruth path not found: $groundTruthPath"
}

$outputPath = Join-Path $RepoRoot $OutputFolder

if (-not (Test-Path $outputPath)) {
    New-Item -ItemType Directory -Path $outputPath | Out-Null
}

# --------------------------------------------------
# Scan Files
# --------------------------------------------------
$files = Get-ChildItem -Path $groundTruthPath -Filter *.json -File

$matches = @()

foreach ($file in $files) {
    try {
        $json = Get-Content $file.FullName -Raw | ConvertFrom-Json

        if ($null -ne $json.ExperimentRef -and $json.ExperimentRef.CatalogNumber -eq $CatalogNumber) {
            $matches += $file
        }
    }
    catch {
        Write-Warning "Skipping invalid JSON: $($file.Name)"
    }
}

# --------------------------------------------------
# Result
# --------------------------------------------------
if ($matches.Count -eq 0) {
    Write-Warning "No GroundTruth file found for $CatalogNumber"
    exit
}

foreach ($match in $matches) {
    $dest = Join-Path $outputPath $match.Name
    Copy-Item $match.FullName $dest -Force
    Write-Host "Copied: $($match.Name)"
}

Write-Host "Done. Files copied to: $outputPath"